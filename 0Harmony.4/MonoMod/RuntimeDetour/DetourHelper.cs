using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.RuntimeDetour.Platforms;
using MonoMod.Utils;

namespace MonoMod.RuntimeDetour
{
	// Token: 0x02000464 RID: 1124
	internal static class DetourHelper
	{
		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x0600182D RID: 6189 RVA: 0x0005470C File Offset: 0x0005290C
		// (set) Token: 0x0600182E RID: 6190 RVA: 0x000547AC File Offset: 0x000529AC
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
					else if (DetourHelper._RuntimeInit)
					{
						detourRuntimePlatform = null;
					}
					else
					{
						DetourHelper._RuntimeInit = true;
						if (ReflectionHelper.IsMono)
						{
							DetourHelper._Runtime = new DetourRuntimeMonoPlatform();
						}
						else if (ReflectionHelper.IsCore)
						{
							DetourHelper._Runtime = DetourRuntimeNETCorePlatform.Create();
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

		// Token: 0x1700057A RID: 1402
		// (get) Token: 0x0600182F RID: 6191 RVA: 0x000547B4 File Offset: 0x000529B4
		// (set) Token: 0x06001830 RID: 6192 RVA: 0x00054900 File Offset: 0x00052B00
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
					else if (DetourHelper._NativeInit)
					{
						detourNativePlatform = null;
					}
					else
					{
						DetourHelper._NativeInit = true;
						IDetourNativePlatform detourNativePlatform2;
						if (PlatformHelper.Is(Platform.ARM))
						{
							detourNativePlatform2 = new DetourNativeARMPlatform();
						}
						else
						{
							detourNativePlatform2 = new DetourNativeX86Platform();
						}
						if (PlatformHelper.Is(Platform.Windows))
						{
							detourNativePlatform = (DetourHelper._Native = new DetourNativeWindowsPlatform(detourNativePlatform2));
						}
						else
						{
							if (ReflectionHelper.IsMono)
							{
								try
								{
									return DetourHelper._Native = new DetourNativeMonoPlatform(detourNativePlatform2, "libmonosgen-2.0." + PlatformHelper.LibrarySuffix);
								}
								catch
								{
								}
							}
							string environmentVariable = Environment.GetEnvironmentVariable("MONOMOD_RUNTIMEDETOUR_MONOPOSIXHELPER");
							if ((ReflectionHelper.IsMono && environmentVariable != "0") || environmentVariable == "1")
							{
								try
								{
									return DetourHelper._Native = new DetourNativeMonoPosixPlatform(detourNativePlatform2);
								}
								catch
								{
								}
							}
							try
							{
								return DetourHelper._Native = new DetourNativeLibcPlatform(detourNativePlatform2);
							}
							catch
							{
							}
							detourNativePlatform = detourNativePlatform2;
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

		// Token: 0x06001831 RID: 6193 RVA: 0x00054908 File Offset: 0x00052B08
		public static void MakeWritable(this IDetourNativePlatform plat, NativeDetourData detour)
		{
			plat.MakeWritable(detour.Method, detour.Size);
		}

		// Token: 0x06001832 RID: 6194 RVA: 0x0005491C File Offset: 0x00052B1C
		public static void MakeExecutable(this IDetourNativePlatform plat, NativeDetourData detour)
		{
			plat.MakeExecutable(detour.Method, detour.Size);
		}

		// Token: 0x06001833 RID: 6195 RVA: 0x00054930 File Offset: 0x00052B30
		public static void FlushICache(this IDetourNativePlatform plat, NativeDetourData detour)
		{
			plat.FlushICache(detour.Method, detour.Size);
		}

		// Token: 0x06001834 RID: 6196 RVA: 0x00054944 File Offset: 0x00052B44
		public unsafe static void Write(this IntPtr to, ref int offs, byte value)
		{
			*(UIntPtr)((long)to + (long)offs) = value;
			offs++;
		}

		// Token: 0x06001835 RID: 6197 RVA: 0x00054959 File Offset: 0x00052B59
		public unsafe static void Write(this IntPtr to, ref int offs, ushort value)
		{
			*(UIntPtr)((long)to + (long)offs) = (short)value;
			offs += 2;
		}

		// Token: 0x06001836 RID: 6198 RVA: 0x0005496E File Offset: 0x00052B6E
		public unsafe static void Write(this IntPtr to, ref int offs, uint value)
		{
			*(UIntPtr)((long)to + (long)offs) = (int)value;
			offs += 4;
		}

		// Token: 0x06001837 RID: 6199 RVA: 0x00054983 File Offset: 0x00052B83
		public unsafe static void Write(this IntPtr to, ref int offs, ulong value)
		{
			*(UIntPtr)((long)to + (long)offs) = (long)value;
			offs += 8;
		}

		// Token: 0x06001838 RID: 6200 RVA: 0x00054998 File Offset: 0x00052B98
		public static MethodBase GetIdentifiable(this MethodBase method)
		{
			return DetourHelper.Runtime.GetIdentifiable(method);
		}

		// Token: 0x06001839 RID: 6201 RVA: 0x000549A5 File Offset: 0x00052BA5
		public static IntPtr GetNativeStart(this MethodBase method)
		{
			return DetourHelper.Runtime.GetNativeStart(method);
		}

		// Token: 0x0600183A RID: 6202 RVA: 0x000549B2 File Offset: 0x00052BB2
		public static IntPtr GetNativeStart(this Delegate method)
		{
			return method.Method.GetNativeStart();
		}

		// Token: 0x0600183B RID: 6203 RVA: 0x000549BF File Offset: 0x00052BBF
		public static IntPtr GetNativeStart(this Expression method)
		{
			return ((MethodCallExpression)method).Method.GetNativeStart();
		}

		// Token: 0x0600183C RID: 6204 RVA: 0x000549D1 File Offset: 0x00052BD1
		public static MethodInfo CreateILCopy(this MethodBase method)
		{
			return DetourHelper.Runtime.CreateCopy(method);
		}

		// Token: 0x0600183D RID: 6205 RVA: 0x000549DE File Offset: 0x00052BDE
		public static bool TryCreateILCopy(this MethodBase method, out MethodInfo dm)
		{
			return DetourHelper.Runtime.TryCreateCopy(method, out dm);
		}

		// Token: 0x0600183E RID: 6206 RVA: 0x000549EC File Offset: 0x00052BEC
		public static T Pin<T>(this T method) where T : MethodBase
		{
			DetourHelper.Runtime.Pin(method);
			return method;
		}

		// Token: 0x0600183F RID: 6207 RVA: 0x000549FF File Offset: 0x00052BFF
		public static T Unpin<T>(this T method) where T : MethodBase
		{
			DetourHelper.Runtime.Unpin(method);
			return method;
		}

		// Token: 0x06001840 RID: 6208 RVA: 0x00054A14 File Offset: 0x00052C14
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
			using (DynamicMethodDefinition dynamicMethodDefinition = new DynamicMethodDefinition("Native<" + ((long)target).ToString("X16", CultureInfo.InvariantCulture) + ">", type, array))
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

		// Token: 0x06001841 RID: 6209 RVA: 0x00054B2C File Offset: 0x00052D2C
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

		// Token: 0x06001842 RID: 6210 RVA: 0x00054B6C File Offset: 0x00052D6C
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

		// Token: 0x06001843 RID: 6211 RVA: 0x00054BEC File Offset: 0x00052DEC
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

		// Token: 0x06001844 RID: 6212 RVA: 0x00054C88 File Offset: 0x00052E88
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

		// Token: 0x0400108D RID: 4237
		private static readonly object _RuntimeLock = new object();

		// Token: 0x0400108E RID: 4238
		private static bool _RuntimeInit = false;

		// Token: 0x0400108F RID: 4239
		private static IDetourRuntimePlatform _Runtime;

		// Token: 0x04001090 RID: 4240
		private static readonly object _NativeLock = new object();

		// Token: 0x04001091 RID: 4241
		private static bool _NativeInit = false;

		// Token: 0x04001092 RID: 4242
		private static IDetourNativePlatform _Native;

		// Token: 0x04001093 RID: 4243
		private static readonly FieldInfo _f_Native = typeof(DetourHelper).GetField("_Native", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001094 RID: 4244
		private static readonly MethodInfo _m_ToNativeDetourData = typeof(DetourHelper).GetMethod("ToNativeDetourData", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04001095 RID: 4245
		private static readonly MethodInfo _m_Copy = typeof(IDetourNativePlatform).GetMethod("Copy");

		// Token: 0x04001096 RID: 4246
		private static readonly MethodInfo _m_Apply = typeof(IDetourNativePlatform).GetMethod("Apply");

		// Token: 0x04001097 RID: 4247
		private static readonly ConstructorInfo _ctor_Exception = typeof(Exception).GetConstructor(new Type[] { typeof(string) });
	}
}
