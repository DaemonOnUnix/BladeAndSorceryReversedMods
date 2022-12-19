using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Mono.Cecil.Cil;
using MonoMod.Utils;

namespace MonoMod.RuntimeDetour.Platforms
{
	// Token: 0x0200037C RID: 892
	public abstract class DetourRuntimeILPlatform : IDetourRuntimePlatform
	{
		// Token: 0x06001506 RID: 5382
		protected abstract RuntimeMethodHandle GetMethodHandle(MethodBase method);

		// Token: 0x06001507 RID: 5383 RVA: 0x0004CDEC File Offset: 0x0004AFEC
		public unsafe DetourRuntimeILPlatform()
		{
			MethodInfo method = typeof(DetourRuntimeILPlatform).GetMethod("_SelftestGetRefPtr", BindingFlags.Instance | BindingFlags.NonPublic);
			MethodInfo method2 = typeof(DetourRuntimeILPlatform).GetMethod("_SelftestGetRefPtrHook", BindingFlags.Static | BindingFlags.NonPublic);
			this._HookSelftest(method, method2);
			IntPtr intPtr = ((Func<IntPtr>)Delegate.CreateDelegate(typeof(Func<IntPtr>), this, method))();
			MethodInfo method3 = typeof(DetourRuntimeILPlatform).GetMethod("_SelftestGetStruct", BindingFlags.Instance | BindingFlags.NonPublic);
			MethodInfo method4 = typeof(DetourRuntimeILPlatform).GetMethod("_SelftestGetStructHook", BindingFlags.Static | BindingFlags.NonPublic);
			this._HookSelftest(method3, method4);
			fixed (DetourRuntimeILPlatform.GlueThiscallStructRetPtrOrder* ptr = &this.GlueThiscallStructRetPtr)
			{
				DetourRuntimeILPlatform.GlueThiscallStructRetPtrOrder* ptr2 = ptr;
				((Func<IntPtr, IntPtr, IntPtr, DetourRuntimeILPlatform._SelftestStruct>)Delegate.CreateDelegate(typeof(Func<IntPtr, IntPtr, IntPtr, DetourRuntimeILPlatform._SelftestStruct>), this, method3))((IntPtr)((void*)ptr2), (IntPtr)((void*)ptr2), intPtr);
			}
		}

		// Token: 0x06001508 RID: 5384 RVA: 0x0004CED0 File Offset: 0x0004B0D0
		private void _HookSelftest(MethodInfo from, MethodInfo to)
		{
			this.Pin(from);
			this.Pin(to);
			NativeDetourData nativeDetourData = DetourHelper.Native.Create(this.GetNativeStart(from), this.GetNativeStart(to), null);
			DetourHelper.Native.MakeWritable(nativeDetourData);
			DetourHelper.Native.Apply(nativeDetourData);
			DetourHelper.Native.MakeExecutable(nativeDetourData);
			DetourHelper.Native.FlushICache(nativeDetourData);
			DetourHelper.Native.Free(nativeDetourData);
		}

		// Token: 0x06001509 RID: 5385 RVA: 0x0004CF44 File Offset: 0x0004B144
		[MethodImpl(MethodImplOptions.NoInlining)]
		private IntPtr _SelftestGetRefPtr()
		{
			Console.Error.WriteLine("If you're reading this, the MonoMod.RuntimeDetour selftest failed.");
			throw new Exception("This method should've been detoured!");
		}

		// Token: 0x0600150A RID: 5386 RVA: 0x00011FA0 File Offset: 0x000101A0
		private static IntPtr _SelftestGetRefPtrHook(IntPtr self)
		{
			return self;
		}

		// Token: 0x0600150B RID: 5387 RVA: 0x0004CF44 File Offset: 0x0004B144
		[MethodImpl(MethodImplOptions.NoInlining)]
		private DetourRuntimeILPlatform._SelftestStruct _SelftestGetStruct(IntPtr x, IntPtr y, IntPtr thisPtr)
		{
			Console.Error.WriteLine("If you're reading this, the MonoMod.RuntimeDetour selftest failed.");
			throw new Exception("This method should've been detoured!");
		}

		// Token: 0x0600150C RID: 5388 RVA: 0x0004CF5F File Offset: 0x0004B15F
		private unsafe static void _SelftestGetStructHook(IntPtr a, IntPtr b, IntPtr c, IntPtr d, IntPtr e)
		{
			if (b == c)
			{
				*(int*)(void*)b = 0;
				return;
			}
			if (b == e)
			{
				*(int*)(void*)c = 2;
				return;
			}
			*(int*)(void*)c = 1;
		}

		// Token: 0x0600150D RID: 5389 RVA: 0x0004CF8E File Offset: 0x0004B18E
		protected virtual IntPtr GetFunctionPointer(MethodBase method, RuntimeMethodHandle handle)
		{
			return handle.GetFunctionPointer();
		}

		// Token: 0x0600150E RID: 5390 RVA: 0x0004CF97 File Offset: 0x0004B197
		protected virtual void PrepareMethod(MethodBase method, RuntimeMethodHandle handle)
		{
			RuntimeHelpers.PrepareMethod(handle);
		}

		// Token: 0x0600150F RID: 5391 RVA: 0x0004CF9F File Offset: 0x0004B19F
		protected virtual void PrepareMethod(MethodBase method, RuntimeMethodHandle handle, RuntimeTypeHandle[] instantiation)
		{
			RuntimeHelpers.PrepareMethod(handle, instantiation);
		}

		// Token: 0x06001510 RID: 5392 RVA: 0x00012279 File Offset: 0x00010479
		protected virtual void DisableInlining(MethodBase method, RuntimeMethodHandle handle)
		{
		}

		// Token: 0x06001511 RID: 5393 RVA: 0x0004CFA8 File Offset: 0x0004B1A8
		public IntPtr GetNativeStart(MethodBase method)
		{
			Dictionary<MethodBase, DetourRuntimeILPlatform.MethodPin> pinnedMethods = this.PinnedMethods;
			DetourRuntimeILPlatform.MethodPin methodPin;
			bool flag2;
			lock (pinnedMethods)
			{
				flag2 = this.PinnedMethods.TryGetValue(method, out methodPin);
			}
			if (flag2)
			{
				return this.GetFunctionPointer(method, methodPin.Handle);
			}
			return this.GetFunctionPointer(method, this.GetMethodHandle(method));
		}

		// Token: 0x06001512 RID: 5394 RVA: 0x0004D010 File Offset: 0x0004B210
		public void Pin(MethodBase method)
		{
			Dictionary<MethodBase, DetourRuntimeILPlatform.MethodPin> pinnedMethods = this.PinnedMethods;
			lock (pinnedMethods)
			{
				DetourRuntimeILPlatform.MethodPin methodPin;
				if (this.PinnedMethods.TryGetValue(method, out methodPin))
				{
					methodPin.Count++;
				}
				else
				{
					methodPin = new DetourRuntimeILPlatform.MethodPin();
					methodPin.Count = 1;
					RuntimeMethodHandle runtimeMethodHandle = (methodPin.Handle = this.GetMethodHandle(method));
					Type declaringType = method.DeclaringType;
					if (declaringType != null && declaringType.IsGenericType)
					{
						this.PrepareMethod(method, runtimeMethodHandle, (from type in method.DeclaringType.GetGenericArguments()
							select type.TypeHandle).ToArray<RuntimeTypeHandle>());
					}
					else
					{
						this.PrepareMethod(method, runtimeMethodHandle);
					}
					this.DisableInlining(method, runtimeMethodHandle);
					this.PinnedMethods[method] = methodPin;
				}
			}
		}

		// Token: 0x06001513 RID: 5395 RVA: 0x0004D0FC File Offset: 0x0004B2FC
		public void Unpin(MethodBase method)
		{
			Dictionary<MethodBase, DetourRuntimeILPlatform.MethodPin> pinnedMethods = this.PinnedMethods;
			lock (pinnedMethods)
			{
				DetourRuntimeILPlatform.MethodPin methodPin;
				if (this.PinnedMethods.TryGetValue(method, out methodPin))
				{
					if (methodPin.Count <= 1)
					{
						this.PinnedMethods.Remove(method);
					}
					else
					{
						methodPin.Count--;
					}
				}
			}
		}

		// Token: 0x06001514 RID: 5396 RVA: 0x0004D170 File Offset: 0x0004B370
		public MethodInfo CreateCopy(MethodBase method)
		{
			if (method == null || (method.GetMethodImplementationFlags() & MethodImplAttributes.CodeTypeMask) != MethodImplAttributes.IL)
			{
				throw new InvalidOperationException("Uncopyable method: " + (((method != null) ? method.ToString() : null) ?? "NULL"));
			}
			MethodInfo methodInfo;
			using (DynamicMethodDefinition dynamicMethodDefinition = new DynamicMethodDefinition(method))
			{
				methodInfo = dynamicMethodDefinition.Generate();
			}
			return methodInfo;
		}

		// Token: 0x06001515 RID: 5397 RVA: 0x0004D1E0 File Offset: 0x0004B3E0
		public bool TryCreateCopy(MethodBase method, out MethodInfo dm)
		{
			if (method == null || (method.GetMethodImplementationFlags() & MethodImplAttributes.CodeTypeMask) != MethodImplAttributes.IL)
			{
				dm = null;
				return false;
			}
			bool flag;
			try
			{
				dm = this.CreateCopy(method);
				flag = true;
			}
			catch
			{
				dm = null;
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001516 RID: 5398 RVA: 0x0004D22C File Offset: 0x0004B42C
		public MethodBase GetDetourTarget(MethodBase from, MethodBase to)
		{
			Type declaringType = to.DeclaringType;
			MethodInfo methodInfo = null;
			if (this.GlueThiscallStructRetPtr != DetourRuntimeILPlatform.GlueThiscallStructRetPtrOrder.Original)
			{
				MethodInfo methodInfo2 = from as MethodInfo;
				if (methodInfo2 != null && !from.IsStatic)
				{
					MethodInfo methodInfo3 = to as MethodInfo;
					if (methodInfo3 != null && to.IsStatic && methodInfo2.ReturnType == methodInfo3.ReturnType && methodInfo2.ReturnType.IsValueType)
					{
						int managedSize = methodInfo2.ReturnType.GetManagedSize();
						if (managedSize == 3 || managedSize == 5 || managedSize == 6 || managedSize == 7 || managedSize >= 9)
						{
							Type thisParamType = from.GetThisParamType();
							Type type = methodInfo2.ReturnType.MakeByRefType();
							int num = 0;
							int num2 = 1;
							if (this.GlueThiscallStructRetPtr == DetourRuntimeILPlatform.GlueThiscallStructRetPtrOrder.RetThisArgs)
							{
								num = 1;
								num2 = 0;
							}
							List<Type> list = new List<Type> { thisParamType };
							list.Insert(num2, type);
							list.AddRange(from p in @from.GetParameters()
								select p.ParameterType);
							using (DynamicMethodDefinition dynamicMethodDefinition = new DynamicMethodDefinition(string.Concat(new string[]
							{
								"Glue:ThiscallStructRetPtr<",
								from.GetID(null, null, true, false, true),
								",",
								to.GetID(null, null, true, false, true),
								">"
							}), typeof(void), list.ToArray()))
							{
								ILProcessor ilprocessor = dynamicMethodDefinition.GetILProcessor();
								ilprocessor.Emit(OpCodes.Ldarg, num2);
								ilprocessor.Emit(OpCodes.Ldarg, num);
								for (int i = 2; i < list.Count; i++)
								{
									ilprocessor.Emit(OpCodes.Ldarg, i);
								}
								ilprocessor.Emit(OpCodes.Call, ilprocessor.Body.Method.Module.ImportReference(to));
								ilprocessor.Emit(OpCodes.Stobj, ilprocessor.Body.Method.Module.ImportReference(methodInfo2.ReturnType));
								ilprocessor.Emit(OpCodes.Ret);
								methodInfo = dynamicMethodDefinition.Generate();
							}
						}
					}
				}
			}
			return methodInfo ?? to;
		}

		// Token: 0x040011E1 RID: 4577
		protected Dictionary<MethodBase, DetourRuntimeILPlatform.MethodPin> PinnedMethods = new Dictionary<MethodBase, DetourRuntimeILPlatform.MethodPin>();

		// Token: 0x040011E2 RID: 4578
		private readonly DetourRuntimeILPlatform.GlueThiscallStructRetPtrOrder GlueThiscallStructRetPtr;

		// Token: 0x0200037D RID: 893
		private struct _SelftestStruct
		{
			// Token: 0x040011E3 RID: 4579
			private readonly byte A;

			// Token: 0x040011E4 RID: 4580
			private readonly byte B;

			// Token: 0x040011E5 RID: 4581
			private readonly byte C;
		}

		// Token: 0x0200037E RID: 894
		protected class MethodPin
		{
			// Token: 0x040011E6 RID: 4582
			public int Count;

			// Token: 0x040011E7 RID: 4583
			public RuntimeMethodHandle Handle;
		}

		// Token: 0x0200037F RID: 895
		private enum GlueThiscallStructRetPtrOrder
		{
			// Token: 0x040011E9 RID: 4585
			Original,
			// Token: 0x040011EA RID: 4586
			ThisRetArgs,
			// Token: 0x040011EB RID: 4587
			RetThisArgs
		}
	}
}
