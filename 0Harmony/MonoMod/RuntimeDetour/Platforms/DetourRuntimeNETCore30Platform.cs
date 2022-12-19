using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using Mono.Cecil;
using MonoMod.Utils;

namespace MonoMod.RuntimeDetour.Platforms
{
	// Token: 0x02000486 RID: 1158
	public class DetourRuntimeNETCore30Platform : DetourRuntimeNETCorePlatform
	{
		// Token: 0x060018F1 RID: 6385 RVA: 0x000569F4 File Offset: 0x00054BF4
		protected unsafe override void DisableInlining(MethodBase method, RuntimeMethodHandle handle)
		{
			ushort* ptr = (ushort*)((byte*)(void*)handle.Value + 6);
			ushort* ptr2 = ptr;
			*ptr2 |= 8192;
		}

		// Token: 0x060018F2 RID: 6386 RVA: 0x00056A1B File Offset: 0x00054C1B
		private DetourRuntimeNETCore30Platform.d_compileMethod GetCompileMethod(IntPtr jit)
		{
			return DetourRuntimeNETCorePlatform.ReadObjectVTable(jit, this.VTableIndex_ICorJitCompiler_compileMethod).AsDelegate<DetourRuntimeNETCore30Platform.d_compileMethod>();
		}

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x060018F3 RID: 6387 RVA: 0x000183ED File Offset: 0x000165ED
		public override bool OnMethodCompiledWillBeCalled
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060018F4 RID: 6388 RVA: 0x00056A30 File Offset: 0x00054C30
		protected unsafe override void InstallJitHooks(IntPtr jit)
		{
			this.SetupJitHookHelpers();
			this.real_compileMethod = this.GetCompileMethod(jit);
			this.our_compileMethod = new DetourRuntimeNETCore30Platform.d_compileMethod(this.CompileMethodHook);
			IntPtr functionPointerForDelegate = Marshal.GetFunctionPointerForDelegate<DetourRuntimeNETCore30Platform.d_compileMethod>(this.our_compileMethod);
			NativeDetourData nativeDetourData = DetourRuntimeNETCore30Platform.CreateNativeTrampolineTo(functionPointerForDelegate);
			DetourRuntimeNETCore30Platform.d_compileMethod d_compileMethod = nativeDetourData.Method.AsDelegate<DetourRuntimeNETCore30Platform.d_compileMethod>();
			IntPtr zero = IntPtr.Zero;
			IntPtr zero2 = IntPtr.Zero;
			DetourRuntimeNETCore30Platform.CORINFO_METHOD_INFO corinfo_METHOD_INFO = default(DetourRuntimeNETCore30Platform.CORINFO_METHOD_INFO);
			byte* ptr;
			ulong num;
			d_compileMethod(zero, zero2, corinfo_METHOD_INFO, 0U, out ptr, out num);
			DetourRuntimeNETCore30Platform.FreeNativeTrampoline(nativeDetourData);
			int num2 = DetourRuntimeNETCore30Platform.hookEntrancy;
			IntPtr* vtableEntry = DetourRuntimeNETCorePlatform.GetVTableEntry(jit, this.VTableIndex_ICorJitCompiler_compileMethod);
			DetourHelper.Native.MakeWritable((IntPtr)((void*)vtableEntry), (uint)IntPtr.Size);
			this.real_compileMethodPtr = *vtableEntry;
			*vtableEntry = functionPointerForDelegate;
		}

		// Token: 0x060018F5 RID: 6389 RVA: 0x00056AD8 File Offset: 0x00054CD8
		private static NativeDetourData CreateNativeTrampolineTo(IntPtr target)
		{
			IntPtr intPtr = DetourHelper.Native.MemAlloc(64U);
			NativeDetourData nativeDetourData = DetourHelper.Native.Create(intPtr, target, null);
			DetourHelper.Native.MakeWritable(nativeDetourData);
			DetourHelper.Native.Apply(nativeDetourData);
			DetourHelper.Native.MakeExecutable(nativeDetourData);
			DetourHelper.Native.FlushICache(nativeDetourData);
			return nativeDetourData;
		}

		// Token: 0x060018F6 RID: 6390 RVA: 0x00056B35 File Offset: 0x00054D35
		private static void FreeNativeTrampoline(NativeDetourData data)
		{
			DetourHelper.Native.MakeWritable(data);
			DetourHelper.Native.MemFree(data.Method);
			DetourHelper.Native.Free(data);
		}

		// Token: 0x060018F7 RID: 6391 RVA: 0x00056B60 File Offset: 0x00054D60
		private unsafe DetourRuntimeNETCore30Platform.CorJitResult CompileMethodHook(IntPtr jit, IntPtr corJitInfo, in DetourRuntimeNETCore30Platform.CORINFO_METHOD_INFO methodInfo, uint flags, out byte* nativeEntry, out ulong nativeSizeOfCode)
		{
			nativeEntry = (IntPtr)((UIntPtr)0);
			nativeSizeOfCode = 0UL;
			if (jit == IntPtr.Zero)
			{
				return DetourRuntimeNETCore30Platform.CorJitResult.CORJIT_OK;
			}
			DetourRuntimeNETCore30Platform.hookEntrancy++;
			DetourRuntimeNETCore30Platform.CorJitResult corJitResult2;
			try
			{
				DetourRuntimeNETCore30Platform.CorJitResult corJitResult = this.real_compileMethod(jit, corJitInfo, methodInfo, flags, out nativeEntry, out nativeSizeOfCode);
				if (DetourRuntimeNETCore30Platform.hookEntrancy == 1)
				{
					try
					{
						RuntimeTypeHandle[] array = null;
						RuntimeTypeHandle[] array2 = null;
						if (methodInfo.args.sigInst.classInst != null)
						{
							array = new RuntimeTypeHandle[methodInfo.args.sigInst.classInstCount];
							for (int i = 0; i < array.Length; i++)
							{
								array[i] = this.GetTypeFromNativeHandle(methodInfo.args.sigInst.classInst[(IntPtr)i * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)]).TypeHandle;
							}
						}
						if (methodInfo.args.sigInst.methInst != null)
						{
							array2 = new RuntimeTypeHandle[methodInfo.args.sigInst.methInstCount];
							for (int j = 0; j < array2.Length; j++)
							{
								array2[j] = this.GetTypeFromNativeHandle(methodInfo.args.sigInst.methInst[(IntPtr)j * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)]).TypeHandle;
							}
						}
						RuntimeTypeHandle typeHandle = this.GetDeclaringTypeOfMethodHandle(methodInfo.ftn).TypeHandle;
						RuntimeMethodHandle runtimeMethodHandle = this.CreateHandleForHandlePointer(methodInfo.ftn);
						this.JitHookCore(typeHandle, runtimeMethodHandle, (IntPtr)nativeEntry, nativeSizeOfCode, array, array2);
					}
					catch
					{
					}
				}
				corJitResult2 = corJitResult;
			}
			finally
			{
				DetourRuntimeNETCore30Platform.hookEntrancy--;
			}
			return corJitResult2;
		}

		// Token: 0x060018F8 RID: 6392 RVA: 0x00056D20 File Offset: 0x00054F20
		protected RuntimeMethodHandle CreateHandleForHandlePointer(IntPtr handle)
		{
			return this.CreateRuntimeMethodHandle(this.CreateRuntimeMethodInfoStub(handle, this.MethodHandle_GetLoaderAllocator(handle)));
		}

		// Token: 0x060018F9 RID: 6393 RVA: 0x00056D48 File Offset: 0x00054F48
		protected virtual void SetupJitHookHelpers()
		{
			MethodInfo methodInfo = typeof(object).Assembly.GetType("Internal.Runtime.CompilerServices.Unsafe").GetMethods().First((MethodInfo m) => m.Name == "As" && m.ReturnType.IsByRef);
			MethodInfo method = typeof(RuntimeMethodHandle).GetMethod("GetLoaderAllocator", BindingFlags.Static | BindingFlags.NonPublic);
			MethodInfo methodInfo2;
			using (DynamicMethodDefinition dynamicMethodDefinition = new DynamicMethodDefinition("MethodHandle_GetLoaderAllocator", typeof(object), new Type[] { typeof(IntPtr) }))
			{
				ILGenerator ilgenerator = dynamicMethodDefinition.GetILGenerator();
				Type parameterType = method.GetParameters().First<ParameterInfo>().ParameterType;
				ilgenerator.Emit(OpCodes.Ldarga_S, 0);
				ilgenerator.Emit(OpCodes.Call, methodInfo.MakeGenericMethod(new Type[]
				{
					typeof(IntPtr),
					parameterType
				}));
				ilgenerator.Emit(OpCodes.Ldobj, parameterType);
				ilgenerator.Emit(OpCodes.Call, method);
				ilgenerator.Emit(OpCodes.Ret);
				methodInfo2 = dynamicMethodDefinition.Generate();
			}
			this.MethodHandle_GetLoaderAllocator = methodInfo2.CreateDelegate<DetourRuntimeNETCore30Platform.d_MethodHandle_GetLoaderAllocator>();
			MethodInfo orCreateGetTypeFromHandleUnsafe = this.GetOrCreateGetTypeFromHandleUnsafe();
			this.GetTypeFromNativeHandle = orCreateGetTypeFromHandleUnsafe.CreateDelegate<DetourRuntimeNETCore30Platform.d_GetTypeFromNativeHandle>();
			Type type = typeof(RuntimeMethodHandle).Assembly.GetType("System.RuntimeMethodHandleInternal");
			MethodInfo method2 = typeof(RuntimeMethodHandle).GetMethod("GetDeclaringType", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[] { type }, null);
			MethodInfo methodInfo3;
			using (DynamicMethodDefinition dynamicMethodDefinition2 = new DynamicMethodDefinition("GetDeclaringTypeOfMethodHandle", typeof(Type), new Type[] { typeof(IntPtr) }))
			{
				ILGenerator ilgenerator2 = dynamicMethodDefinition2.GetILGenerator();
				ilgenerator2.Emit(OpCodes.Ldarga_S, 0);
				ilgenerator2.Emit(OpCodes.Call, methodInfo.MakeGenericMethod(new Type[]
				{
					typeof(IntPtr),
					type
				}));
				ilgenerator2.Emit(OpCodes.Ldobj, type);
				ilgenerator2.Emit(OpCodes.Call, method2);
				ilgenerator2.Emit(OpCodes.Ret);
				methodInfo3 = dynamicMethodDefinition2.Generate();
			}
			this.GetDeclaringTypeOfMethodHandle = methodInfo3.CreateDelegate<DetourRuntimeNETCore30Platform.d_GetDeclaringTypeOfMethodHandle>();
			Type[] array = new Type[]
			{
				typeof(IntPtr),
				typeof(object)
			};
			Type type2 = typeof(RuntimeMethodHandle).Assembly.GetType("System.RuntimeMethodInfoStub");
			ConstructorInfo constructor = type2.GetConstructor(array);
			MethodInfo methodInfo4;
			using (DynamicMethodDefinition dynamicMethodDefinition3 = new DynamicMethodDefinition("new RuntimeMethodInfoStub", type2, array))
			{
				ILGenerator ilgenerator3 = dynamicMethodDefinition3.GetILGenerator();
				ilgenerator3.Emit(OpCodes.Ldarg_0);
				ilgenerator3.Emit(OpCodes.Ldarg_1);
				ilgenerator3.Emit(OpCodes.Newobj, constructor);
				ilgenerator3.Emit(OpCodes.Ret);
				methodInfo4 = dynamicMethodDefinition3.Generate();
			}
			this.CreateRuntimeMethodInfoStub = methodInfo4.CreateDelegate<DetourRuntimeNETCore30Platform.d_CreateRuntimeMethodInfoStub>();
			ConstructorInfo constructorInfo = typeof(RuntimeMethodHandle).GetConstructors(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).First<ConstructorInfo>();
			MethodInfo methodInfo5;
			using (DynamicMethodDefinition dynamicMethodDefinition4 = new DynamicMethodDefinition("new RuntimeMethodHandle", typeof(RuntimeMethodHandle), new Type[] { typeof(object) }))
			{
				ILGenerator ilgenerator4 = dynamicMethodDefinition4.GetILGenerator();
				ilgenerator4.Emit(OpCodes.Ldarg_0);
				ilgenerator4.Emit(OpCodes.Newobj, constructorInfo);
				ilgenerator4.Emit(OpCodes.Ret);
				methodInfo5 = dynamicMethodDefinition4.Generate();
			}
			this.CreateRuntimeMethodHandle = methodInfo5.CreateDelegate<DetourRuntimeNETCore30Platform.d_CreateRuntimeMethodHandle>();
		}

		// Token: 0x060018FA RID: 6394 RVA: 0x000570EC File Offset: 0x000552EC
		private MethodInfo GetOrCreateGetTypeFromHandleUnsafe()
		{
			if (this._getTypeFromHandleUnsafeMethod != null)
			{
				return this._getTypeFromHandleUnsafeMethod;
			}
			Assembly assembly;
			using (ModuleDefinition moduleDefinition = ModuleDefinition.CreateModule("MonoMod.RuntimeDetour.Runtime.NETCore3+Helpers", new ModuleParameters
			{
				Kind = ModuleKind.Dll
			}))
			{
				TypeDefinition typeDefinition = new TypeDefinition("System", "Type", Mono.Cecil.TypeAttributes.Public | Mono.Cecil.TypeAttributes.Abstract)
				{
					BaseType = moduleDefinition.TypeSystem.Object
				};
				moduleDefinition.Types.Add(typeDefinition);
				MethodDefinition methodDefinition = new MethodDefinition("GetTypeFromHandleUnsafe", Mono.Cecil.MethodAttributes.FamANDAssem | Mono.Cecil.MethodAttributes.Family | Mono.Cecil.MethodAttributes.Static, moduleDefinition.ImportReference(typeof(Type)))
				{
					IsInternalCall = true
				};
				methodDefinition.Parameters.Add(new ParameterDefinition(moduleDefinition.ImportReference(typeof(IntPtr))));
				typeDefinition.Methods.Add(methodDefinition);
				assembly = ReflectionHelper.Load(moduleDefinition);
			}
			this.MakeAssemblySystemAssembly(assembly);
			return this._getTypeFromHandleUnsafeMethod = assembly.GetType("System.Type").GetMethod("GetTypeFromHandleUnsafe");
		}

		// Token: 0x060018FB RID: 6395 RVA: 0x000571F4 File Offset: 0x000553F4
		protected unsafe virtual void MakeAssemblySystemAssembly(Assembly assembly)
		{
			IntPtr intPtr = (IntPtr)DetourRuntimeNETCore30Platform._runtimeAssemblyPtrField.GetValue(assembly);
			int num = IntPtr.Size + IntPtr.Size + IntPtr.Size + IntPtr.Size + IntPtr.Size + 4 + IntPtr.Size + IntPtr.Size + 4 + 4 + IntPtr.Size + IntPtr.Size + 4 + 4 + IntPtr.Size;
			if (IntPtr.Size == 8)
			{
				num += 4;
			}
			IntPtr intPtr2 = *(IntPtr*)((byte*)(void*)intPtr + num);
			int num2 = IntPtr.Size + IntPtr.Size + IntPtr.Size + IntPtr.Size;
			IntPtr intPtr3 = *(IntPtr*)((byte*)(void*)intPtr2 + num2);
			int num3 = IntPtr.Size + IntPtr.Size + IntPtr.Size + 4 + 4 + IntPtr.Size + IntPtr.Size + IntPtr.Size + IntPtr.Size + 4;
			int* ptr = (int*)((byte*)(void*)intPtr3 + num3);
			*ptr |= 1;
		}

		// Token: 0x060018FC RID: 6396 RVA: 0x000572C9 File Offset: 0x000554C9
		protected void HookPermanent(MethodBase from, MethodBase to)
		{
			this.Pin(from);
			this.Pin(to);
			this.HookPermanent(this.GetNativeStart(from), this.GetNativeStart(to));
		}

		// Token: 0x060018FD RID: 6397 RVA: 0x000572F0 File Offset: 0x000554F0
		protected void HookPermanent(IntPtr from, IntPtr to)
		{
			NativeDetourData nativeDetourData = DetourHelper.Native.Create(from, to, null);
			DetourHelper.Native.MakeWritable(nativeDetourData);
			DetourHelper.Native.Apply(nativeDetourData);
			DetourHelper.Native.MakeExecutable(nativeDetourData);
			DetourHelper.Native.FlushICache(nativeDetourData);
			DetourHelper.Native.Free(nativeDetourData);
		}

		// Token: 0x04001266 RID: 4710
		public static readonly Guid JitVersionGuid = new Guid("d609bed1-7831-49fc-bd49-b6f054dd4d46");

		// Token: 0x04001267 RID: 4711
		private DetourRuntimeNETCore30Platform.d_compileMethod our_compileMethod;

		// Token: 0x04001268 RID: 4712
		private IntPtr real_compileMethodPtr;

		// Token: 0x04001269 RID: 4713
		private DetourRuntimeNETCore30Platform.d_compileMethod real_compileMethod;

		// Token: 0x0400126A RID: 4714
		[ThreadStatic]
		private static int hookEntrancy = 0;

		// Token: 0x0400126B RID: 4715
		protected DetourRuntimeNETCore30Platform.d_MethodHandle_GetLoaderAllocator MethodHandle_GetLoaderAllocator;

		// Token: 0x0400126C RID: 4716
		protected DetourRuntimeNETCore30Platform.d_CreateRuntimeMethodInfoStub CreateRuntimeMethodInfoStub;

		// Token: 0x0400126D RID: 4717
		protected DetourRuntimeNETCore30Platform.d_CreateRuntimeMethodHandle CreateRuntimeMethodHandle;

		// Token: 0x0400126E RID: 4718
		protected DetourRuntimeNETCore30Platform.d_GetDeclaringTypeOfMethodHandle GetDeclaringTypeOfMethodHandle;

		// Token: 0x0400126F RID: 4719
		protected DetourRuntimeNETCore30Platform.d_GetTypeFromNativeHandle GetTypeFromNativeHandle;

		// Token: 0x04001270 RID: 4720
		private MethodInfo _getTypeFromHandleUnsafeMethod;

		// Token: 0x04001271 RID: 4721
		private static FieldInfo _runtimeAssemblyPtrField = Type.GetType("System.Reflection.RuntimeAssembly").GetField("m_assembly", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x02000487 RID: 1159
		private enum CorJitResult
		{
			// Token: 0x04001273 RID: 4723
			CORJIT_OK
		}

		// Token: 0x02000488 RID: 1160
		private struct CORINFO_SIG_INST
		{
			// Token: 0x04001274 RID: 4724
			public uint classInstCount;

			// Token: 0x04001275 RID: 4725
			public unsafe IntPtr* classInst;

			// Token: 0x04001276 RID: 4726
			public uint methInstCount;

			// Token: 0x04001277 RID: 4727
			public unsafe IntPtr* methInst;
		}

		// Token: 0x02000489 RID: 1161
		private struct CORINFO_SIG_INFO
		{
			// Token: 0x04001278 RID: 4728
			public int callConv;

			// Token: 0x04001279 RID: 4729
			public IntPtr retTypeClass;

			// Token: 0x0400127A RID: 4730
			public IntPtr retTypeSigClass;

			// Token: 0x0400127B RID: 4731
			public byte retType;

			// Token: 0x0400127C RID: 4732
			public byte flags;

			// Token: 0x0400127D RID: 4733
			public ushort numArgs;

			// Token: 0x0400127E RID: 4734
			public DetourRuntimeNETCore30Platform.CORINFO_SIG_INST sigInst;

			// Token: 0x0400127F RID: 4735
			public IntPtr args;

			// Token: 0x04001280 RID: 4736
			public IntPtr pSig;

			// Token: 0x04001281 RID: 4737
			public uint sbSig;

			// Token: 0x04001282 RID: 4738
			public IntPtr scope;

			// Token: 0x04001283 RID: 4739
			public uint token;
		}

		// Token: 0x0200048A RID: 1162
		private struct CORINFO_METHOD_INFO
		{
			// Token: 0x04001284 RID: 4740
			public IntPtr ftn;

			// Token: 0x04001285 RID: 4741
			public IntPtr scope;

			// Token: 0x04001286 RID: 4742
			public unsafe byte* ILCode;

			// Token: 0x04001287 RID: 4743
			public uint ILCodeSize;

			// Token: 0x04001288 RID: 4744
			public uint maxStack;

			// Token: 0x04001289 RID: 4745
			public uint EHcount;

			// Token: 0x0400128A RID: 4746
			public int options;

			// Token: 0x0400128B RID: 4747
			public int regionKind;

			// Token: 0x0400128C RID: 4748
			public DetourRuntimeNETCore30Platform.CORINFO_SIG_INFO args;

			// Token: 0x0400128D RID: 4749
			public DetourRuntimeNETCore30Platform.CORINFO_SIG_INFO locals;
		}

		// Token: 0x0200048B RID: 1163
		// (Invoke) Token: 0x06001901 RID: 6401
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private unsafe delegate DetourRuntimeNETCore30Platform.CorJitResult d_compileMethod(IntPtr thisPtr, IntPtr corJitInfo, in DetourRuntimeNETCore30Platform.CORINFO_METHOD_INFO methodInfo, uint flags, out byte* nativeEntry, out ulong nativeSizeOfCode);

		// Token: 0x0200048C RID: 1164
		// (Invoke) Token: 0x06001905 RID: 6405
		protected delegate object d_MethodHandle_GetLoaderAllocator(IntPtr methodHandle);

		// Token: 0x0200048D RID: 1165
		// (Invoke) Token: 0x06001909 RID: 6409
		protected delegate object d_CreateRuntimeMethodInfoStub(IntPtr methodHandle, object loaderAllocator);

		// Token: 0x0200048E RID: 1166
		// (Invoke) Token: 0x0600190D RID: 6413
		protected delegate RuntimeMethodHandle d_CreateRuntimeMethodHandle(object runtimeMethodInfo);

		// Token: 0x0200048F RID: 1167
		// (Invoke) Token: 0x06001911 RID: 6417
		protected delegate Type d_GetDeclaringTypeOfMethodHandle(IntPtr methodHandle);

		// Token: 0x02000490 RID: 1168
		// (Invoke) Token: 0x06001915 RID: 6421
		protected delegate Type d_GetTypeFromNativeHandle(IntPtr handle);
	}
}
