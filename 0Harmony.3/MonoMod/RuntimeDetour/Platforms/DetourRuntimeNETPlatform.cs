using System;
using System.Net;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using MonoMod.Utils;

namespace MonoMod.RuntimeDetour.Platforms
{
	// Token: 0x02000383 RID: 899
	internal class DetourRuntimeNETPlatform : DetourRuntimeILPlatform
	{
		// Token: 0x06001522 RID: 5410 RVA: 0x0004D54C File Offset: 0x0004B74C
		protected override RuntimeMethodHandle GetMethodHandle(MethodBase method)
		{
			DynamicMethod dynamicMethod = method as DynamicMethod;
			if (dynamicMethod != null)
			{
				if (DetourRuntimeNETPlatform._RuntimeHelpers__CompileMethod_TakesIntPtr)
				{
					DetourRuntimeNETPlatform._RuntimeHelpers__CompileMethod.Invoke(null, new object[] { ((RuntimeMethodHandle)DetourRuntimeNETPlatform._DynamicMethod_GetMethodDescriptor.Invoke(dynamicMethod, DetourRuntimeNETPlatform._NoArgs)).Value });
				}
				else if (DetourRuntimeNETPlatform._RuntimeHelpers__CompileMethod_TakesIRuntimeMethodInfo)
				{
					DetourRuntimeNETPlatform._RuntimeHelpers__CompileMethod.Invoke(null, new object[] { DetourRuntimeNETPlatform._RuntimeMethodHandle_m_value.GetValue((RuntimeMethodHandle)DetourRuntimeNETPlatform._DynamicMethod_GetMethodDescriptor.Invoke(dynamicMethod, DetourRuntimeNETPlatform._NoArgs)) });
				}
				else
				{
					try
					{
						dynamicMethod.CreateDelegate(typeof(MulticastDelegate));
					}
					catch
					{
					}
				}
				if (DetourRuntimeNETPlatform._DynamicMethod_m_method != null)
				{
					return (RuntimeMethodHandle)DetourRuntimeNETPlatform._DynamicMethod_m_method.GetValue(method);
				}
				if (DetourRuntimeNETPlatform._DynamicMethod_GetMethodDescriptor != null)
				{
					return (RuntimeMethodHandle)DetourRuntimeNETPlatform._DynamicMethod_GetMethodDescriptor.Invoke(method, DetourRuntimeNETPlatform._NoArgs);
				}
			}
			return method.MethodHandle;
		}

		// Token: 0x06001523 RID: 5411 RVA: 0x00012279 File Offset: 0x00010479
		protected override void DisableInlining(MethodBase method, RuntimeMethodHandle handle)
		{
		}

		// Token: 0x06001524 RID: 5412 RVA: 0x0004D658 File Offset: 0x0004B858
		protected unsafe override IntPtr GetFunctionPointer(MethodBase method, RuntimeMethodHandle handle)
		{
			MMDbgLog.Log("mets: " + method.GetID(null, null, true, false, false));
			MMDbgLog.Log(string.Format("meth: 0x{0:X16}", (long)handle.Value));
			MMDbgLog.Log(string.Format("getf: 0x{0:X16}", (long)handle.GetFunctionPointer()));
			if (method.IsVirtual)
			{
				Type declaringType = method.DeclaringType;
				if (declaringType != null && declaringType.IsValueType)
				{
					MMDbgLog.Log(string.Format("ldfn: 0x{0:X16}", (long)method.GetLdftnPointer()));
					return method.GetLdftnPointer();
				}
			}
			IntPtr intPtr = base.GetFunctionPointer(method, handle);
			if (!PlatformHelper.Is(Platform.ARM))
			{
				if (IntPtr.Size == 4)
				{
					int num = (int)intPtr;
					if (*(IntPtr)num == 184 && *(IntPtr)(num + 5) == 144 && *(IntPtr)(num + 6) == 232 && *(IntPtr)(num + 11) == 233)
					{
						int num2 = num + 11;
						int num3 = *(IntPtr)(num2 + 1) + (num2 + 1 + 4);
						intPtr = this.NotThePreStub(intPtr, (IntPtr)num3);
						MMDbgLog.Log(string.Format("ngen: 0x{0:X16}", (long)intPtr));
						return intPtr;
					}
				}
				else
				{
					long num4 = (long)intPtr;
					if (*(UIntPtr)num4 == 1959363912U && *(UIntPtr)(num4 + 5L) == 1224837960U && *(UIntPtr)(num4 + 18L) == 1958886217U && *(UIntPtr)(num4 + 23L) == 47176)
					{
						intPtr = this.NotThePreStub(intPtr, (IntPtr)(*(UIntPtr)(num4 + 25L)));
						MMDbgLog.Log(string.Format("ngen: 0x{0:X16}", (long)intPtr));
						return intPtr;
					}
					if (*(UIntPtr)num4 == 233 && *(UIntPtr)(num4 + 5L) == 95)
					{
						long num5 = num4;
						long num6 = (long)(*(UIntPtr)(num5 + 1L)) + (num5 + 1L + 4L);
						intPtr = this.NotThePreStub(intPtr, (IntPtr)num6);
						MMDbgLog.Log(string.Format("ngen: 0x{0:X16}", (long)intPtr));
						return intPtr;
					}
				}
			}
			return intPtr;
		}

		// Token: 0x06001525 RID: 5413 RVA: 0x0004D868 File Offset: 0x0004BA68
		private IntPtr NotThePreStub(IntPtr ptrGot, IntPtr ptrParsed)
		{
			if (DetourRuntimeNETPlatform.ThePreStub == IntPtr.Zero)
			{
				DetourRuntimeNETPlatform.ThePreStub = (IntPtr)(-2);
				Type type = typeof(HttpWebRequest).Assembly.GetType("System.Net.Connection");
				MethodInfo methodInfo = ((type != null) ? type.GetMethod("SubmitRequest", BindingFlags.Instance | BindingFlags.NonPublic) : null);
				if (methodInfo != null)
				{
					DetourRuntimeNETPlatform.ThePreStub = base.GetNativeStart(methodInfo);
					MMDbgLog.Log(string.Format("ThePreStub: 0x{0:X16}", (long)DetourRuntimeNETPlatform.ThePreStub));
				}
				else if (PlatformHelper.Is(Platform.Windows))
				{
					DetourRuntimeNETPlatform.ThePreStub = (IntPtr)(-1);
				}
			}
			if (!(ptrParsed == DetourRuntimeNETPlatform.ThePreStub) && !(DetourRuntimeNETPlatform.ThePreStub == (IntPtr)(-1)))
			{
				return ptrParsed;
			}
			return ptrGot;
		}

		// Token: 0x040011F2 RID: 4594
		private static readonly object[] _NoArgs = new object[0];

		// Token: 0x040011F3 RID: 4595
		private static readonly FieldInfo _DynamicMethod_m_method = typeof(DynamicMethod).GetField("m_method", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x040011F4 RID: 4596
		private static readonly MethodInfo _DynamicMethod_GetMethodDescriptor = typeof(DynamicMethod).GetMethod("GetMethodDescriptor", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x040011F5 RID: 4597
		private static readonly FieldInfo _RuntimeMethodHandle_m_value = typeof(RuntimeMethodHandle).GetField("m_value", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x040011F6 RID: 4598
		private static readonly MethodInfo _RuntimeHelpers__CompileMethod = typeof(RuntimeHelpers).GetMethod("_CompileMethod", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x040011F7 RID: 4599
		private static readonly bool _RuntimeHelpers__CompileMethod_TakesIntPtr = DetourRuntimeNETPlatform._RuntimeHelpers__CompileMethod != null && DetourRuntimeNETPlatform._RuntimeHelpers__CompileMethod.GetParameters()[0].ParameterType.FullName == "System.IntPtr";

		// Token: 0x040011F8 RID: 4600
		private static readonly bool _RuntimeHelpers__CompileMethod_TakesIRuntimeMethodInfo = DetourRuntimeNETPlatform._RuntimeHelpers__CompileMethod != null && DetourRuntimeNETPlatform._RuntimeHelpers__CompileMethod.GetParameters()[0].ParameterType.FullName == "System.IRuntimeMethodInfo";

		// Token: 0x040011F9 RID: 4601
		private static IntPtr ThePreStub = IntPtr.Zero;
	}
}
