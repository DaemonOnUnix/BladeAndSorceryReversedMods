using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using Mono.Cecil.Cil;
using MonoMod.Utils;

namespace MonoMod.RuntimeDetour.Platforms
{
	// Token: 0x0200047C RID: 1148
	public abstract class DetourRuntimeILPlatform : IDetourRuntimePlatform
	{
		// Token: 0x060018C0 RID: 6336
		protected abstract RuntimeMethodHandle GetMethodHandle(MethodBase method);

		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x060018C1 RID: 6337
		public abstract bool OnMethodCompiledWillBeCalled { get; }

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060018C2 RID: 6338
		// (remove) Token: 0x060018C3 RID: 6339
		public abstract event OnMethodCompiledEvent OnMethodCompiled;

		// Token: 0x060018C4 RID: 6340 RVA: 0x00055E2C File Offset: 0x0005402C
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
			MethodInfo method5 = typeof(DetourRuntimeILPlatform._SelftestStruct).GetMethod("_SelftestGetInStruct", BindingFlags.Instance | BindingFlags.Public);
			MethodInfo method6 = typeof(DetourRuntimeILPlatform).GetMethod("_SelftestGetInStructHook", BindingFlags.Static | BindingFlags.NonPublic);
			this._HookSelftest(method5, method6);
			fixed (DetourRuntimeILPlatform.GlueThiscallStructRetPtrOrder* ptr = &this.GlueThiscallInStructRetPtr)
			{
				DetourRuntimeILPlatform.GlueThiscallStructRetPtrOrder* ptr3 = ptr;
				object obj = default(DetourRuntimeILPlatform._SelftestStruct);
				*ptr3 = (DetourRuntimeILPlatform.GlueThiscallStructRetPtrOrder)((Func<short>)Delegate.CreateDelegate(typeof(Func<short>), obj, method5))();
				if (*ptr3 == (DetourRuntimeILPlatform.GlueThiscallStructRetPtrOrder)(-1))
				{
					throw new Exception("_SelftestGetInStruct failed!");
				}
			}
			this.Pin(method);
			this.ReferenceNonDynamicPoolPtr = this.GetNativeStart(method);
			if (DynamicMethodDefinition.IsDynamicILAvailable)
			{
				MethodBase methodBase;
				using (DynamicMethodDefinition dynamicMethodDefinition = new DynamicMethodDefinition(DetourRuntimeILPlatform._MemAllocScratchDummy))
				{
					dynamicMethodDefinition.Name = "MemAllocScratch<Reference>";
					methodBase = DMDGenerator<DMDEmitDynamicMethodGenerator>.Generate(dynamicMethodDefinition, null);
				}
				this.Pin(methodBase);
				this.ReferenceDynamicPoolPtr = this.GetNativeStart(methodBase);
			}
		}

		// Token: 0x060018C5 RID: 6341 RVA: 0x00056014 File Offset: 0x00054214
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

		// Token: 0x060018C6 RID: 6342 RVA: 0x00056088 File Offset: 0x00054288
		[MethodImpl(MethodImplOptions.NoInlining)]
		private IntPtr _SelftestGetRefPtr()
		{
			Console.Error.WriteLine("If you're reading this, the MonoMod.RuntimeDetour selftest failed.");
			throw new Exception("This method should've been detoured!");
		}

		// Token: 0x060018C7 RID: 6343 RVA: 0x00017E2C File Offset: 0x0001602C
		private static IntPtr _SelftestGetRefPtrHook(IntPtr self)
		{
			return self;
		}

		// Token: 0x060018C8 RID: 6344 RVA: 0x000560A3 File Offset: 0x000542A3
		[MethodImpl(MethodImplOptions.NoInlining)]
		private DetourRuntimeILPlatform._SelftestStruct _SelftestGetStruct(IntPtr x, IntPtr y, IntPtr thisPtr)
		{
			Console.Error.WriteLine("If you're reading this, the MonoMod.RuntimeDetour selftest failed.");
			throw new Exception("_SelftestGetStruct failed!");
		}

		// Token: 0x060018C9 RID: 6345 RVA: 0x000560BE File Offset: 0x000542BE
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

		// Token: 0x060018CA RID: 6346 RVA: 0x000560ED File Offset: 0x000542ED
		private unsafe static short _SelftestGetInStructHook(IntPtr a)
		{
			*(short*)(void*)a = 2;
			return 0;
		}

		// Token: 0x060018CB RID: 6347 RVA: 0x000560F8 File Offset: 0x000542F8
		protected virtual IntPtr GetFunctionPointer(MethodBase method, RuntimeMethodHandle handle)
		{
			return handle.GetFunctionPointer();
		}

		// Token: 0x060018CC RID: 6348 RVA: 0x00056101 File Offset: 0x00054301
		protected virtual void PrepareMethod(MethodBase method, RuntimeMethodHandle handle)
		{
			RuntimeHelpers.PrepareMethod(handle);
		}

		// Token: 0x060018CD RID: 6349 RVA: 0x00056109 File Offset: 0x00054309
		protected virtual void PrepareMethod(MethodBase method, RuntimeMethodHandle handle, RuntimeTypeHandle[] instantiation)
		{
			RuntimeHelpers.PrepareMethod(handle, instantiation);
		}

		// Token: 0x060018CE RID: 6350 RVA: 0x00018105 File Offset: 0x00016305
		protected virtual void DisableInlining(MethodBase method, RuntimeMethodHandle handle)
		{
		}

		// Token: 0x060018CF RID: 6351 RVA: 0x00056114 File Offset: 0x00054314
		public virtual MethodBase GetIdentifiable(MethodBase method)
		{
			DetourRuntimeILPlatform.PrivateMethodPin privateMethodPin;
			if (!this.PinnedHandles.TryGetValue(this.GetMethodHandle(method), out privateMethodPin))
			{
				return method;
			}
			return privateMethodPin.Pin.Method;
		}

		// Token: 0x060018D0 RID: 6352 RVA: 0x00056144 File Offset: 0x00054344
		public virtual DetourRuntimeILPlatform.MethodPinInfo GetPin(MethodBase method)
		{
			DetourRuntimeILPlatform.PrivateMethodPin privateMethodPin;
			if (!this.PinnedMethods.TryGetValue(method, out privateMethodPin))
			{
				return default(DetourRuntimeILPlatform.MethodPinInfo);
			}
			return privateMethodPin.Pin;
		}

		// Token: 0x060018D1 RID: 6353 RVA: 0x00056174 File Offset: 0x00054374
		public virtual DetourRuntimeILPlatform.MethodPinInfo GetPin(RuntimeMethodHandle handle)
		{
			DetourRuntimeILPlatform.PrivateMethodPin privateMethodPin;
			if (!this.PinnedHandles.TryGetValue(handle, out privateMethodPin))
			{
				return default(DetourRuntimeILPlatform.MethodPinInfo);
			}
			return privateMethodPin.Pin;
		}

		// Token: 0x060018D2 RID: 6354 RVA: 0x000561A1 File Offset: 0x000543A1
		public virtual DetourRuntimeILPlatform.MethodPinInfo[] GetPins()
		{
			return (from p in this.PinnedHandles.Values.ToArray<DetourRuntimeILPlatform.PrivateMethodPin>()
				select p.Pin).ToArray<DetourRuntimeILPlatform.MethodPinInfo>();
		}

		// Token: 0x060018D3 RID: 6355 RVA: 0x000561DC File Offset: 0x000543DC
		public virtual IntPtr GetNativeStart(MethodBase method)
		{
			method = this.GetIdentifiable(method);
			DetourRuntimeILPlatform.PrivateMethodPin privateMethodPin;
			if (this.PinnedMethods.TryGetValue(method, out privateMethodPin))
			{
				return this.GetFunctionPointer(method, privateMethodPin.Pin.Handle);
			}
			return this.GetFunctionPointer(method, this.GetMethodHandle(method));
		}

		// Token: 0x060018D4 RID: 6356 RVA: 0x00056224 File Offset: 0x00054424
		public virtual void Pin(MethodBase method)
		{
			method = this.GetIdentifiable(method);
			Interlocked.Increment(ref this.PinnedMethods.GetOrAdd(method, delegate(MethodBase m)
			{
				DetourRuntimeILPlatform.PrivateMethodPin privateMethodPin = new DetourRuntimeILPlatform.PrivateMethodPin();
				privateMethodPin.Pin.Method = m;
				RuntimeMethodHandle runtimeMethodHandle = (privateMethodPin.Pin.Handle = this.GetMethodHandle(m));
				this.PinnedHandles[runtimeMethodHandle] = privateMethodPin;
				this.DisableInlining(method, runtimeMethodHandle);
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
				return privateMethodPin;
			}).Pin.Count);
		}

		// Token: 0x060018D5 RID: 6357 RVA: 0x00056284 File Offset: 0x00054484
		public virtual void Unpin(MethodBase method)
		{
			method = this.GetIdentifiable(method);
			DetourRuntimeILPlatform.PrivateMethodPin privateMethodPin;
			if (!this.PinnedMethods.TryGetValue(method, out privateMethodPin))
			{
				return;
			}
			if (Interlocked.Decrement(ref privateMethodPin.Pin.Count) <= 0)
			{
				DetourRuntimeILPlatform.PrivateMethodPin privateMethodPin2;
				this.PinnedMethods.TryRemove(method, out privateMethodPin2);
				this.PinnedHandles.TryRemove(privateMethodPin.Pin.Handle, out privateMethodPin2);
			}
		}

		// Token: 0x060018D6 RID: 6358 RVA: 0x000562E8 File Offset: 0x000544E8
		public MethodInfo CreateCopy(MethodBase method)
		{
			method = this.GetIdentifiable(method);
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

		// Token: 0x060018D7 RID: 6359 RVA: 0x00056364 File Offset: 0x00054564
		public bool TryCreateCopy(MethodBase method, out MethodInfo dm)
		{
			method = this.GetIdentifiable(method);
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

		// Token: 0x060018D8 RID: 6360 RVA: 0x000563BC File Offset: 0x000545BC
		public MethodBase GetDetourTarget(MethodBase from, MethodBase to)
		{
			to = this.GetIdentifiable(to);
			MethodInfo methodInfo = null;
			MethodInfo methodInfo2 = from as MethodInfo;
			if (methodInfo2 != null && !from.IsStatic)
			{
				MethodInfo methodInfo3 = to as MethodInfo;
				if (methodInfo3 != null && to.IsStatic && methodInfo2.ReturnType == methodInfo3.ReturnType && methodInfo2.ReturnType.IsValueType)
				{
					Type declaringType = from.DeclaringType;
					DetourRuntimeILPlatform.GlueThiscallStructRetPtrOrder glueThiscallStructRetPtrOrder;
					if ((glueThiscallStructRetPtrOrder = ((declaringType != null && declaringType.IsValueType) ? this.GlueThiscallInStructRetPtr : this.GlueThiscallStructRetPtr)) != DetourRuntimeILPlatform.GlueThiscallStructRetPtrOrder.Original)
					{
						int managedSize = methodInfo2.ReturnType.GetManagedSize();
						if (managedSize == 3 || managedSize == 5 || managedSize == 6 || managedSize == 7 || managedSize > IntPtr.Size)
						{
							Type thisParamType = from.GetThisParamType();
							Type type = methodInfo2.ReturnType.MakeByRefType();
							int num = 0;
							int num2 = 1;
							if (glueThiscallStructRetPtrOrder == DetourRuntimeILPlatform.GlueThiscallStructRetPtrOrder.RetThisArgs)
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

		// Token: 0x060018D9 RID: 6361 RVA: 0x00056614 File Offset: 0x00054814
		public uint TryMemAllocScratchCloseTo(IntPtr target, out IntPtr ptr, int size)
		{
			if (size == 0 || (long)size > (long)((ulong)DetourRuntimeILPlatform._MemAllocScratchDummySafeSize))
			{
				ptr = IntPtr.Zero;
				return 0U;
			}
			bool flag = Math.Abs((long)target - (long)this.ReferenceNonDynamicPoolPtr) < 1073741824L;
			bool flag2 = DynamicMethodDefinition.IsDynamicILAvailable && Math.Abs((long)target - (long)this.ReferenceDynamicPoolPtr) < 1073741824L;
			if (!flag && !flag2)
			{
				ptr = IntPtr.Zero;
				return 0U;
			}
			MethodBase methodBase;
			using (DynamicMethodDefinition dynamicMethodDefinition = new DynamicMethodDefinition(DetourRuntimeILPlatform._MemAllocScratchDummy))
			{
				dynamicMethodDefinition.Name = string.Format("MemAllocScratch<{0:X16}>", (long)target);
				if (flag2)
				{
					methodBase = DMDGenerator<DMDEmitDynamicMethodGenerator>.Generate(dynamicMethodDefinition, null);
				}
				else
				{
					methodBase = DMDGenerator<DMDCecilGenerator>.Generate(dynamicMethodDefinition, null);
				}
			}
			this.Pin(methodBase);
			ptr = this.GetNativeStart(methodBase);
			DetourHelper.Native.MakeReadWriteExecutable(ptr, DetourRuntimeILPlatform._MemAllocScratchDummySafeSize);
			return DetourRuntimeILPlatform._MemAllocScratchDummySafeSize;
		}

		// Token: 0x060018DA RID: 6362 RVA: 0x0005670C File Offset: 0x0005490C
		public static int MemAllocScratchDummy(int a, int b)
		{
			if (a >= 1024 && b >= 1024)
			{
				return a + b;
			}
			return DetourRuntimeILPlatform.MemAllocScratchDummy(a + b, b + 1);
		}

		// Token: 0x04001246 RID: 4678
		protected DetourRuntimeILPlatform.GlueThiscallStructRetPtrOrder GlueThiscallStructRetPtr;

		// Token: 0x04001247 RID: 4679
		protected DetourRuntimeILPlatform.GlueThiscallStructRetPtrOrder GlueThiscallInStructRetPtr;

		// Token: 0x04001248 RID: 4680
		protected ConcurrentDictionary<MethodBase, DetourRuntimeILPlatform.PrivateMethodPin> PinnedMethods = new ConcurrentDictionary<MethodBase, DetourRuntimeILPlatform.PrivateMethodPin>();

		// Token: 0x04001249 RID: 4681
		protected ConcurrentDictionary<RuntimeMethodHandle, DetourRuntimeILPlatform.PrivateMethodPin> PinnedHandles = new ConcurrentDictionary<RuntimeMethodHandle, DetourRuntimeILPlatform.PrivateMethodPin>();

		// Token: 0x0400124A RID: 4682
		private IntPtr ReferenceNonDynamicPoolPtr;

		// Token: 0x0400124B RID: 4683
		private IntPtr ReferenceDynamicPoolPtr;

		// Token: 0x0400124C RID: 4684
		protected static readonly uint _MemAllocScratchDummySafeSize = 16U;

		// Token: 0x0400124D RID: 4685
		protected static readonly MethodInfo _MemAllocScratchDummy = typeof(DetourRuntimeILPlatform).GetMethod("MemAllocScratchDummy", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

		// Token: 0x0200047D RID: 1149
		private struct _SelftestStruct
		{
			// Token: 0x060018DC RID: 6364 RVA: 0x00056751 File Offset: 0x00054951
			[MethodImpl(MethodImplOptions.NoInlining)]
			public short _SelftestGetInStruct()
			{
				Console.Error.WriteLine("If you're reading this, the MonoMod.RuntimeDetour selftest failed.");
				return -1;
			}

			// Token: 0x0400124E RID: 4686
			private readonly short Value;

			// Token: 0x0400124F RID: 4687
			private readonly byte E1;

			// Token: 0x04001250 RID: 4688
			private readonly byte E2;

			// Token: 0x04001251 RID: 4689
			private readonly byte E3;
		}

		// Token: 0x0200047E RID: 1150
		protected class PrivateMethodPin
		{
			// Token: 0x04001252 RID: 4690
			public DetourRuntimeILPlatform.MethodPinInfo Pin;
		}

		// Token: 0x0200047F RID: 1151
		public struct MethodPinInfo
		{
			// Token: 0x060018DE RID: 6366 RVA: 0x00056763 File Offset: 0x00054963
			public override string ToString()
			{
				return string.Format("(MethodPinInfo: {0}, {1}, 0x{2:X})", this.Count, this.Method, (long)this.Handle.Value);
			}

			// Token: 0x04001253 RID: 4691
			public int Count;

			// Token: 0x04001254 RID: 4692
			public MethodBase Method;

			// Token: 0x04001255 RID: 4693
			public RuntimeMethodHandle Handle;
		}

		// Token: 0x02000480 RID: 1152
		protected enum GlueThiscallStructRetPtrOrder
		{
			// Token: 0x04001257 RID: 4695
			Original,
			// Token: 0x04001258 RID: 4696
			ThisRetArgs,
			// Token: 0x04001259 RID: 4697
			RetThisArgs
		}
	}
}
