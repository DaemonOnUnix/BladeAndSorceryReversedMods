using System;
using System.Linq.Expressions;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.RuntimeDetour.Platforms;
using MonoMod.Utils;

namespace MonoMod.RuntimeDetour
{
	// Token: 0x02000365 RID: 869
	internal static class DetourHelper
	{
		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x06001484 RID: 5252 RVA: 0x0004B78C File Offset: 0x0004998C
		// (set) Token: 0x06001485 RID: 5253 RVA: 0x0004B844 File Offset: 0x00049A44
		public static IDetourRuntimePlatform Runtime
		{
			get
			{
				if (DetourHelper._Runtime != null)
				{
					return DetourHelper._Runtime;
				}
				object runtimeLock = DetourHelper._RuntimeLock;
				IDetourRuntimePlatform detourRuntimePlatform;
				lock (runtimeLock)
				{
					if (DetourHelper._Runtime != null)
					{
						detourRuntimePlatform = DetourHelper._Runtime;
					}
					else
					{
						if (Type.GetType("Mono.Runtime") != null)
						{
							DetourHelper._Runtime = new DetourRuntimeMonoPlatform();
						}
						else if (typeof(object).Assembly.GetName().Name == "System.Private.CoreLib")
						{
							DetourHelper._Runtime = new DetourRuntimeNETCorePlatform();
						}
						else
						{
							DetourHelper._Runtime = new DetourRuntimeNETPlatform();
						}
						detourRuntimePlatform = DetourHelper._Runtime;
					}
				}
				return detourRuntimePlatform;
			}
			set
			{
				DetourHelper._Runtime = value;
			}
		}

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x06001486 RID: 5254 RVA: 0x0004B84C File Offset: 0x00049A4C
		// (set) Token: 0x06001487 RID: 5255 RVA: 0x0004B970 File Offset: 0x00049B70
		public static IDetourNativePlatform Native
		{
			get
			{
				if (DetourHelper._Native != null)
				{
					return DetourHelper._Native;
				}
				object nativeLock = DetourHelper._NativeLock;
				IDetourNativePlatform detourNativePlatform;
				lock (nativeLock)
				{
					if (DetourHelper._Native != null)
					{
						detourNativePlatform = DetourHelper._Native;
					}
					else
					{
						if (PlatformHelper.Is(Platform.ARM))
						{
							DetourHelper._Native = new DetourNativeARMPlatform();
						}
						else
						{
							DetourHelper._Native = new DetourNativeX86Platform();
						}
						if (PlatformHelper.Is(Platform.Windows))
						{
							detourNativePlatform = (DetourHelper._Native = new DetourNativeWindowsPlatform(DetourHelper._Native));
						}
						else
						{
							if (Type.GetType("Mono.Runtime") != null)
							{
								try
								{
									return DetourHelper._Native = new DetourNativeMonoPlatform(DetourHelper._Native, "libmonosgen-2.0." + PlatformHelper.LibrarySuffix);
								}
								catch
								{
								}
							}
							try
							{
								DetourHelper._Native = new DetourNativeMonoPosixPlatform(DetourHelper._Native);
							}
							catch
							{
							}
							try
							{
								DetourHelper._Native = new DetourNativeLibcPlatform(DetourHelper._Native);
							}
							catch
							{
							}
							detourNativePlatform = DetourHelper._Native;
						}
					}
				}
				return detourNativePlatform;
			}
			set
			{
				DetourHelper._Native = value;
			}
		}

		// Token: 0x06001488 RID: 5256 RVA: 0x0004B978 File Offset: 0x00049B78
		public static void MakeWritable(this IDetourNativePlatform plat, NativeDetourData detour)
		{
			plat.MakeWritable(detour.Method, detour.Size);
		}

		// Token: 0x06001489 RID: 5257 RVA: 0x0004B98C File Offset: 0x00049B8C
		public static void MakeExecutable(this IDetourNativePlatform plat, NativeDetourData detour)
		{
			plat.MakeExecutable(detour.Method, detour.Size);
		}

		// Token: 0x0600148A RID: 5258 RVA: 0x0004B9A0 File Offset: 0x00049BA0
		public static void FlushICache(this IDetourNativePlatform plat, NativeDetourData detour)
		{
			plat.FlushICache(detour.Method, detour.Size);
		}

		// Token: 0x0600148B RID: 5259 RVA: 0x0004B9B4 File Offset: 0x00049BB4
		public unsafe static void Write(this IntPtr to, ref int offs, byte value)
		{
			*(UIntPtr)((long)to + (long)offs) = value;
			offs++;
		}

		// Token: 0x0600148C RID: 5260 RVA: 0x0004B9C9 File Offset: 0x00049BC9
		public unsafe static void Write(this IntPtr to, ref int offs, ushort value)
		{
			*(UIntPtr)((long)to + (long)offs) = (short)value;
			offs += 2;
		}

		// Token: 0x0600148D RID: 5261 RVA: 0x0004B9DE File Offset: 0x00049BDE
		public unsafe static void Write(this IntPtr to, ref int offs, uint value)
		{
			*(UIntPtr)((long)to + (long)offs) = (int)value;
			offs += 4;
		}

		// Token: 0x0600148E RID: 5262 RVA: 0x0004B9F3 File Offset: 0x00049BF3
		public unsafe static void Write(this IntPtr to, ref int offs, ulong value)
		{
			*(UIntPtr)((long)to + (long)offs) = (long)value;
			offs += 8;
		}

		// Token: 0x0600148F RID: 5263 RVA: 0x0004BA08 File Offset: 0x00049C08
		public static IntPtr GetNativeStart(this MethodBase method)
		{
			return DetourHelper.Runtime.GetNativeStart(method);
		}

		// Token: 0x06001490 RID: 5264 RVA: 0x0004BA15 File Offset: 0x00049C15
		public static IntPtr GetNativeStart(this Delegate method)
		{
			return method.Method.GetNativeStart();
		}

		// Token: 0x06001491 RID: 5265 RVA: 0x0004BA22 File Offset: 0x00049C22
		public static IntPtr GetNativeStart(this Expression method)
		{
			return ((MethodCallExpression)method).Method.GetNativeStart();
		}

		// Token: 0x06001492 RID: 5266 RVA: 0x0004BA34 File Offset: 0x00049C34
		public static MethodInfo CreateILCopy(this MethodBase method)
		{
			return DetourHelper.Runtime.CreateCopy(method);
		}

		// Token: 0x06001493 RID: 5267 RVA: 0x0004BA41 File Offset: 0x00049C41
		public static bool TryCreateILCopy(this MethodBase method, out MethodInfo dm)
		{
			return DetourHelper.Runtime.TryCreateCopy(method, out dm);
		}

		// Token: 0x06001494 RID: 5268 RVA: 0x0004BA4F File Offset: 0x00049C4F
		public static T Pin<T>(this T method) where T : MethodBase
		{
			DetourHelper.Runtime.Pin(method);
			return method;
		}

		// Token: 0x06001495 RID: 5269 RVA: 0x0004BA62 File Offset: 0x00049C62
		public static T Unpin<T>(this T method) where T : MethodBase
		{
			DetourHelper.Runtime.Unpin(method);
			return method;
		}

		// Token: 0x06001496 RID: 5270 RVA: 0x0004BA78 File Offset: 0x00049C78
		public static MethodInfo GenerateNativeProxy(IntPtr target, MethodBase signature)
		{
			MethodInfo methodInfo = signature as MethodInfo;
			Type type = ((methodInfo != null) ? methodInfo.ReturnType : null) ?? typeof(void);
			ParameterInfo[] parameters = signature.GetParameters();
			Type[] array = new Type[parameters.Length];
			for (int i = 0; i < parameters.Length; i++)
			{
				array[i] = parameters[i].ParameterType;
			}
			MethodInfo methodInfo2;
			using (DynamicMethodDefinition dynamicMethodDefinition = new DynamicMethodDefinition("Native<" + ((long)target).ToString("X16") + ">", type, array))
			{
				methodInfo2 = dynamicMethodDefinition.StubCriticalDetour().Generate().Pin<MethodInfo>();
			}
			NativeDetourData nativeDetourData = DetourHelper.Native.Create(methodInfo2.GetNativeStart(), target, null);
			DetourHelper.Native.MakeWritable(nativeDetourData);
			DetourHelper.Native.Apply(nativeDetourData);
			DetourHelper.Native.MakeExecutable(nativeDetourData);
			DetourHelper.Native.FlushICache(nativeDetourData);
			DetourHelper.Native.Free(nativeDetourData);
			return methodInfo2;
		}

		// Token: 0x06001497 RID: 5271 RVA: 0x0004BB8C File Offset: 0x00049D8C
		private static NativeDetourData ToNativeDetourData(IntPtr method, IntPtr target, uint size, byte type, IntPtr extra)
		{
			return new NativeDetourData
			{
				Method = method,
				Target = target,
				Size = size,
				Type = type,
				Extra = extra
			};
		}

		// Token: 0x06001498 RID: 5272 RVA: 0x0004BBCC File Offset: 0x00049DCC
		public static DynamicMethodDefinition StubCriticalDetour(this DynamicMethodDefinition dm)
		{
			ILProcessor ilprocessor = dm.GetILProcessor();
			ModuleDefinition module = ilprocessor.Body.Method.Module;
			for (int i = 0; i < 32; i++)
			{
				ilprocessor.Emit(OpCodes.Nop);
			}
			ilprocessor.Emit(OpCodes.Ldstr, dm.Definition.Name + " should've been detoured!");
			ilprocessor.Emit(OpCodes.Newobj, module.ImportReference(DetourHelper._ctor_Exception));
			ilprocessor.Emit(OpCodes.Throw);
			return dm;
		}

		// Token: 0x06001499 RID: 5273 RVA: 0x0004BC4C File Offset: 0x00049E4C
		public static void EmitDetourCopy(this ILProcessor il, IntPtr src, IntPtr dst, byte type)
		{
			ModuleDefinition module = il.Body.Method.Module;
			il.Emit(OpCodes.Ldsfld, module.ImportReference(DetourHelper._f_Native));
			il.Emit(OpCodes.Ldc_I8, (long)src);
			il.Emit(OpCodes.Conv_I);
			il.Emit(OpCodes.Ldc_I8, (long)dst);
			il.Emit(OpCodes.Conv_I);
			il.Emit(OpCodes.Ldc_I4, (int)type);
			il.Emit(OpCodes.Conv_U1);
			il.Emit(OpCodes.Callvirt, module.ImportReference(DetourHelper._m_Copy));
		}

		// Token: 0x0600149A RID: 5274 RVA: 0x0004BCE8 File Offset: 0x00049EE8
		public static void EmitDetourApply(this ILProcessor il, NativeDetourData data)
		{
			ModuleDefinition module = il.Body.Method.Module;
			il.Emit(OpCodes.Ldsfld, module.ImportReference(DetourHelper._f_Native));
			il.Emit(OpCodes.Ldc_I8, (long)data.Method);
			il.Emit(OpCodes.Conv_I);
			il.Emit(OpCodes.Ldc_I8, (long)data.Target);
			il.Emit(OpCodes.Conv_I);
			il.Emit(OpCodes.Ldc_I4, (int)data.Size);
			il.Emit(OpCodes.Ldc_I4, (int)data.Type);
			il.Emit(OpCodes.Conv_U1);
			il.Emit(OpCodes.Ldc_I8, (long)data.Extra);
			il.Emit(OpCodes.Conv_I);
			il.Emit(OpCodes.Call, module.ImportReference(DetourHelper._m_ToNativeDetourData));
			il.Emit(OpCodes.Callvirt, module.ImportReference(DetourHelper._m_Apply));
		}

		// Token: 0x0400102B RID: 4139
		private static readonly object _RuntimeLock = new object();

		// Token: 0x0400102C RID: 4140
		private static IDetourRuntimePlatform _Runtime;

		// Token: 0x0400102D RID: 4141
		private static readonly object _NativeLock = new object();

		// Token: 0x0400102E RID: 4142
		private static IDetourNativePlatform _Native;

		// Token: 0x0400102F RID: 4143
		private static readonly FieldInfo _f_Native = typeof(DetourHelper).GetField("_Native", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001030 RID: 4144
		private static readonly MethodInfo _m_ToNativeDetourData = typeof(DetourHelper).GetMethod("ToNativeDetourData", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001031 RID: 4145
		private static readonly MethodInfo _m_Copy = typeof(IDetourNativePlatform).GetMethod("Copy");

		// Token: 0x04001032 RID: 4146
		private static readonly MethodInfo _m_Apply = typeof(IDetourNativePlatform).GetMethod("Apply");

		// Token: 0x04001033 RID: 4147
		private static readonly ConstructorInfo _ctor_Exception = typeof(Exception).GetConstructor(new Type[] { typeof(string) });
	}
}
