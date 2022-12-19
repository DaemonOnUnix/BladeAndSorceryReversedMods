using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using MonoMod.Utils;

namespace MonoMod.RuntimeDetour.Platforms
{
	// Token: 0x02000492 RID: 1170
	public class DetourRuntimeNETCorePlatform : DetourRuntimeNETPlatform
	{
		// Token: 0x0600191B RID: 6427 RVA: 0x000573B1 File Offset: 0x000555B1
		public DetourRuntimeNETCorePlatform()
		{
			this.GlueThiscallInStructRetPtr = this.GlueThiscallStructRetPtr;
		}

		// Token: 0x0600191C RID: 6428 RVA: 0x000573C8 File Offset: 0x000555C8
		protected static IntPtr GetJitObject()
		{
			if (DetourRuntimeNETCorePlatform.getJit == null)
			{
				ProcessModule processModule = Process.GetCurrentProcess().Modules.Cast<ProcessModule>().FirstOrDefault((ProcessModule m) => Path.GetFileNameWithoutExtension(m.FileName).EndsWith("clrjit", StringComparison.Ordinal));
				if (processModule == null)
				{
					throw new PlatformNotSupportedException();
				}
				IntPtr intPtr;
				if (!DynDll.TryOpenLibrary(processModule.FileName, out intPtr, false, null))
				{
					throw new PlatformNotSupportedException();
				}
				if (PlatformHelper.Is(Platform.Windows))
				{
					DetourRuntimeNETCorePlatform.isNet5Jit = processModule.FileVersionInfo.ProductMajorPart >= 5;
				}
				else
				{
					DetourRuntimeNETCorePlatform.isNet5Jit = typeof(object).Assembly.GetName().Version.Major >= 5;
				}
				try
				{
					DetourRuntimeNETCorePlatform.getJit = intPtr.GetFunction("getJit").AsDelegate<DetourRuntimeNETCorePlatform.d_getJit>();
				}
				catch
				{
					DynDll.CloseLibrary(intPtr);
					throw;
				}
			}
			return DetourRuntimeNETCorePlatform.getJit();
		}

		// Token: 0x0600191D RID: 6429 RVA: 0x000574C4 File Offset: 0x000556C4
		protected static Guid GetJitGuid(IntPtr jit)
		{
			int num = (DetourRuntimeNETCorePlatform.isNet5Jit ? 2 : 4);
			Guid guid;
			DetourRuntimeNETCorePlatform.ReadObjectVTable(jit, num).AsDelegate<DetourRuntimeNETCorePlatform.d_getVersionIdentifier>()(jit, out guid);
			return guid;
		}

		// Token: 0x1700057F RID: 1407
		// (get) Token: 0x0600191E RID: 6430 RVA: 0x00017DC4 File Offset: 0x00015FC4
		protected virtual int VTableIndex_ICorJitCompiler_compileMethod
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x0600191F RID: 6431 RVA: 0x000574F2 File Offset: 0x000556F2
		protected unsafe static IntPtr* GetVTableEntry(IntPtr @object, int index)
		{
			return *(IntPtr*)(void*)@object / (IntPtr)sizeof(IntPtr) + index * sizeof(IntPtr);
		}

		// Token: 0x06001920 RID: 6432 RVA: 0x00057505 File Offset: 0x00055705
		protected unsafe static IntPtr ReadObjectVTable(IntPtr @object, int index)
		{
			return *DetourRuntimeNETCorePlatform.GetVTableEntry(@object, index);
		}

		// Token: 0x06001921 RID: 6433 RVA: 0x00018105 File Offset: 0x00016305
		protected override void DisableInlining(MethodBase method, RuntimeMethodHandle handle)
		{
		}

		// Token: 0x06001922 RID: 6434 RVA: 0x0005750F File Offset: 0x0005570F
		protected virtual void InstallJitHooks(IntPtr jitObject)
		{
			throw new PlatformNotSupportedException();
		}

		// Token: 0x17000580 RID: 1408
		// (get) Token: 0x06001923 RID: 6435 RVA: 0x00017DC4 File Offset: 0x00015FC4
		public override bool OnMethodCompiledWillBeCalled
		{
			get
			{
				return false;
			}
		}

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06001924 RID: 6436 RVA: 0x00057518 File Offset: 0x00055718
		// (remove) Token: 0x06001925 RID: 6437 RVA: 0x00057550 File Offset: 0x00055750
		public override event OnMethodCompiledEvent OnMethodCompiled;

		// Token: 0x06001926 RID: 6438 RVA: 0x00057588 File Offset: 0x00055788
		protected virtual void JitHookCore(RuntimeTypeHandle declaringType, RuntimeMethodHandle methodHandle, IntPtr methodBodyStart, ulong methodBodySize, RuntimeTypeHandle[] genericClassArguments, RuntimeTypeHandle[] genericMethodArguments)
		{
			try
			{
				Type type = Type.GetTypeFromHandle(declaringType);
				if (genericClassArguments != null && type.IsGenericTypeDefinition)
				{
					type = type.MakeGenericType(genericClassArguments.Select(new Func<RuntimeTypeHandle, Type>(Type.GetTypeFromHandle)).ToArray<Type>());
				}
				MethodBase methodBase = MethodBase.GetMethodFromHandle(methodHandle, type.TypeHandle);
				if (methodBase == null)
				{
					methodBase = this.GetPin(methodHandle).Method;
				}
				try
				{
					OnMethodCompiledEvent onMethodCompiled = this.OnMethodCompiled;
					if (onMethodCompiled != null)
					{
						onMethodCompiled(methodBase, methodBodyStart, methodBodySize);
					}
				}
				catch (Exception ex)
				{
					MMDbgLog.Log(string.Format("Error executing OnMethodCompiled event: {0}", ex));
				}
			}
			catch (Exception ex2)
			{
				MMDbgLog.Log(string.Format("Error in JitHookCore: {0}", ex2));
			}
		}

		// Token: 0x06001927 RID: 6439 RVA: 0x00057648 File Offset: 0x00055848
		public static DetourRuntimeNETCorePlatform Create()
		{
			try
			{
				IntPtr jitObject = DetourRuntimeNETCorePlatform.GetJitObject();
				Guid jitGuid = DetourRuntimeNETCorePlatform.GetJitGuid(jitObject);
				DetourRuntimeNETCorePlatform detourRuntimeNETCorePlatform = null;
				if (jitGuid == DetourRuntimeNET60Platform.JitVersionGuid)
				{
					detourRuntimeNETCorePlatform = new DetourRuntimeNET60Platform();
				}
				else if (jitGuid == DetourRuntimeNET50Platform.JitVersionGuid)
				{
					detourRuntimeNETCorePlatform = new DetourRuntimeNET50Platform();
				}
				else if (jitGuid == DetourRuntimeNETCore30Platform.JitVersionGuid)
				{
					detourRuntimeNETCorePlatform = new DetourRuntimeNETCore30Platform();
				}
				if (detourRuntimeNETCorePlatform == null)
				{
					return new DetourRuntimeNETCorePlatform();
				}
				if (detourRuntimeNETCorePlatform != null)
				{
					detourRuntimeNETCorePlatform.InstallJitHooks(jitObject);
				}
				return detourRuntimeNETCorePlatform;
			}
			catch (Exception ex)
			{
				MMDbgLog.Log("Could not get JIT information for the runtime, falling out to the version without JIT hooks");
				MMDbgLog.Log(string.Format("Error: {0}", ex));
			}
			return new DetourRuntimeNETCorePlatform();
		}

		// Token: 0x04001290 RID: 4752
		private static DetourRuntimeNETCorePlatform.d_getJit getJit;

		// Token: 0x04001291 RID: 4753
		private static bool isNet5Jit;

		// Token: 0x04001292 RID: 4754
		private const int vtableIndex_ICorJitCompiler_getVersionIdentifier = 4;

		// Token: 0x04001293 RID: 4755
		private const int vtableIndex_ICorJitCompiler_getVersionIdentifier_net5 = 2;

		// Token: 0x02000493 RID: 1171
		// (Invoke) Token: 0x06001929 RID: 6441
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate IntPtr d_getJit();

		// Token: 0x02000494 RID: 1172
		// (Invoke) Token: 0x0600192D RID: 6445
		[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
		private delegate void d_getVersionIdentifier(IntPtr thisPtr, out Guid versionIdentifier);
	}
}
