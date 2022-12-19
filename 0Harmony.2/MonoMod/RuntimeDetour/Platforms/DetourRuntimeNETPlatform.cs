using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using MonoMod.Utils;

namespace MonoMod.RuntimeDetour.Platforms
{
	// Token: 0x02000496 RID: 1174
	public class DetourRuntimeNETPlatform : DetourRuntimeILPlatform
	{
		// Token: 0x06001933 RID: 6451 RVA: 0x00057714 File Offset: 0x00055914
		public override MethodBase GetIdentifiable(MethodBase method)
		{
			if (DetourRuntimeNETPlatform._RTDynamicMethod_m_owner != null && method.GetType() == DetourRuntimeNETPlatform._RTDynamicMethod)
			{
				return (MethodBase)DetourRuntimeNETPlatform._RTDynamicMethod_m_owner.GetValue(method);
			}
			return base.GetIdentifiable(method);
		}

		// Token: 0x06001934 RID: 6452 RVA: 0x00057750 File Offset: 0x00055950
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
				else if (DetourRuntimeNETPlatform._RuntimeHelpers__CompileMethod_TakesRuntimeMethodHandleInternal)
				{
					DetourRuntimeNETPlatform._RuntimeHelpers__CompileMethod.Invoke(null, new object[] { DetourRuntimeNETPlatform._IRuntimeMethodInfo_get_Value.Invoke(DetourRuntimeNETPlatform._RuntimeMethodHandle_m_value.GetValue((RuntimeMethodHandle)DetourRuntimeNETPlatform._DynamicMethod_GetMethodDescriptor.Invoke(dynamicMethod, DetourRuntimeNETPlatform._NoArgs)), null) });
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

		// Token: 0x06001935 RID: 6453 RVA: 0x00018105 File Offset: 0x00016305
		protected override void DisableInlining(MethodBase method, RuntimeMethodHandle handle)
		{
		}

		// Token: 0x06001936 RID: 6454 RVA: 0x000578AC File Offset: 0x00055AAC
		protected unsafe override IntPtr GetFunctionPointer(MethodBase method, RuntimeMethodHandle handle)
		{
			MMDbgLog.Log("mets: " + method.GetID(null, null, true, false, false));
			MMDbgLog.Log(string.Format("meth: 0x{0:X16}", (long)handle.Value));
			MMDbgLog.Log(string.Format("getf: 0x{0:X16}", (long)handle.GetFunctionPointer()));
			bool flag = false;
			IntPtr intPtr;
			long num;
			for (;;)
			{
				if (!method.IsVirtual)
				{
					goto IL_EA;
				}
				Type declaringType = method.DeclaringType;
				if (declaringType == null || !declaringType.IsValueType)
				{
					goto IL_EA;
				}
				MMDbgLog.Log(string.Format("ldfn: 0x{0:X16}", (long)method.GetLdftnPointer()));
				bool flag2 = false;
				foreach (Type type in method.DeclaringType.GetInterfaces())
				{
					if (method.DeclaringType.GetInterfaceMap(type).TargetMethods.Contains(method))
					{
						flag2 = true;
						break;
					}
				}
				intPtr = method.GetLdftnPointer();
				if (!flag2)
				{
					break;
				}
				IL_F3:
				if (PlatformHelper.Is(Platform.ARM))
				{
					goto Block_6;
				}
				if (IntPtr.Size == 4)
				{
					goto Block_10;
				}
				num = (long)intPtr;
				if (*(UIntPtr)num == 1959363912U && *(UIntPtr)(num + 5L) == 1224837960U && *(UIntPtr)(num + 18L) == 1958886217U && *(UIntPtr)(num + 23L) == 47176)
				{
					goto Block_20;
				}
				if (*(UIntPtr)num == 233 && *(UIntPtr)(num + 5L) == 95)
				{
					goto Block_22;
				}
				if (*(UIntPtr)num == 232 && !flag)
				{
					MMDbgLog.Log("Method thunk reset; regenerating");
					flag = true;
					long num2 = (long)(*(UIntPtr)(num + 1L)) + (num + 1L + 4L);
					MMDbgLog.Log(string.Format("PrecodeFixupThunk: 0x{0:X16}", num2));
					this.PrepareMethod(method, handle);
					continue;
				}
				return intPtr;
				IL_EA:
				intPtr = base.GetFunctionPointer(method, handle);
				goto IL_F3;
			}
			return intPtr;
			Block_6:
			if (IntPtr.Size != 4)
			{
				int num3 = 0;
				IntPtr intPtr2 = this.<GetFunctionPointer>g__WalkPrecode|14_0(intPtr);
				while (intPtr2 != intPtr)
				{
					if (num3 >= 16)
					{
						break;
					}
					num3++;
					intPtr = intPtr2;
					intPtr2 = this.<GetFunctionPointer>g__WalkPrecode|14_0(intPtr);
				}
				return intPtr;
			}
			return intPtr;
			Block_10:
			int num4 = (int)intPtr;
			if (*(IntPtr)num4 == 184 && *(IntPtr)(num4 + 5) == 144 && *(IntPtr)(num4 + 6) == 232 && *(IntPtr)(num4 + 11) == 233)
			{
				int num5 = num4 + 11;
				int num6 = *(IntPtr)(num5 + 1) + (num5 + 1 + 4);
				intPtr = this.NotThePreStub(intPtr, (IntPtr)num6);
				MMDbgLog.Log(string.Format("ngen: 0x{0:X8}", (long)intPtr));
			}
			num4 = (int)intPtr;
			if (*(IntPtr)num4 == 233 && *(IntPtr)(num4 + 5) == 95)
			{
				int num7 = num4;
				int num8 = *(IntPtr)(num7 + 1) + (num7 + 1 + 4);
				intPtr = this.NotThePreStub(intPtr, (IntPtr)num8);
				MMDbgLog.Log(string.Format("ngen: 0x{0:X8}", (int)intPtr));
				return intPtr;
			}
			return intPtr;
			Block_20:
			intPtr = this.NotThePreStub(intPtr, (IntPtr)(*(UIntPtr)(num + 25L)));
			MMDbgLog.Log(string.Format("ngen: 0x{0:X16}", (long)intPtr));
			return intPtr;
			Block_22:
			long num9 = num;
			long num10 = (long)(*(UIntPtr)(num9 + 1L)) + (num9 + 1L + 4L);
			intPtr = this.NotThePreStub(intPtr, (IntPtr)num10);
			for (int j = 0; j < 16; j++)
			{
				num = (long)intPtr + (long)j;
				if (*(UIntPtr)num == 47176 && *(UIntPtr)(num + 10L) == 57599)
				{
					num10 = *(UIntPtr)(num + 2L);
					intPtr = this.NotThePreStub(intPtr, (IntPtr)num10);
					j = -1;
				}
				else if ((*(UIntPtr)num & 65520) == 47168 && (*(UIntPtr)(num + 10L) & 15794175U) == 65382U && *(UIntPtr)(num + 13L) == 34063 && (*(UIntPtr)num & 15) == (*(UIntPtr)(num + 12L) & 15))
				{
					num9 = num;
					num10 = (long)(*(UIntPtr)(num9 + 13L + 2L)) + (num9 + 13L + 2L + 4L);
					intPtr = this.NotThePreStub(intPtr, (IntPtr)num10);
					j = -1;
				}
			}
			MMDbgLog.Log(string.Format("ngen: 0x{0:X16}", (long)intPtr));
			return intPtr;
		}

		// Token: 0x17000581 RID: 1409
		// (get) Token: 0x06001937 RID: 6455 RVA: 0x00017DC4 File Offset: 0x00015FC4
		public override bool OnMethodCompiledWillBeCalled
		{
			get
			{
				return false;
			}
		}

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06001938 RID: 6456 RVA: 0x00057CD8 File Offset: 0x00055ED8
		// (remove) Token: 0x06001939 RID: 6457 RVA: 0x00057D10 File Offset: 0x00055F10
		public override event OnMethodCompiledEvent OnMethodCompiled;

		// Token: 0x0600193A RID: 6458 RVA: 0x00057D48 File Offset: 0x00055F48
		private IntPtr NotThePreStub(IntPtr ptrGot, IntPtr ptrParsed)
		{
			if (DetourRuntimeNETPlatform.ThePreStub == IntPtr.Zero)
			{
				DetourRuntimeNETPlatform.ThePreStub = (IntPtr)(-2);
				Type type = typeof(HttpWebRequest).Assembly.GetType("System.Net.Connection");
				MethodInfo methodInfo = ((type != null) ? type.GetMethod("SubmitRequest", BindingFlags.Instance | BindingFlags.NonPublic) : null);
				if (methodInfo != null)
				{
					DetourRuntimeNETPlatform.ThePreStub = this.GetNativeStart(methodInfo);
					MMDbgLog.Log(string.Format("ThePreStub: 0x{0:X16}", (long)DetourRuntimeNETPlatform.ThePreStub));
				}
				else if (PlatformHelper.Is(Platform.Windows))
				{
					DetourRuntimeNETPlatform.ThePreStub = (IntPtr)(-1);
				}
			}
			if (!(ptrParsed == DetourRuntimeNETPlatform.ThePreStub))
			{
				return ptrParsed;
			}
			return ptrGot;
		}

		// Token: 0x0600193C RID: 6460 RVA: 0x00057DFC File Offset: 0x00055FFC
		// Note: this type is marked as 'beforefieldinit'.
		static DetourRuntimeNETPlatform()
		{
			Type rtdynamicMethod = DetourRuntimeNETPlatform._RTDynamicMethod;
			DetourRuntimeNETPlatform._RTDynamicMethod_m_owner = ((rtdynamicMethod != null) ? rtdynamicMethod.GetField("m_owner", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) : null);
			DetourRuntimeNETPlatform._DynamicMethod_m_method = typeof(DynamicMethod).GetField("m_method", BindingFlags.Instance | BindingFlags.NonPublic);
			DetourRuntimeNETPlatform._DynamicMethod_GetMethodDescriptor = typeof(DynamicMethod).GetMethod("GetMethodDescriptor", BindingFlags.Instance | BindingFlags.NonPublic);
			DetourRuntimeNETPlatform._RuntimeMethodHandle_m_value = typeof(RuntimeMethodHandle).GetField("m_value", BindingFlags.Instance | BindingFlags.NonPublic);
			Type type = typeof(RuntimeMethodHandle).Assembly.GetType("System.IRuntimeMethodInfo");
			DetourRuntimeNETPlatform._IRuntimeMethodInfo_get_Value = ((type != null) ? type.GetMethod("get_Value") : null);
			DetourRuntimeNETPlatform._RuntimeHelpers__CompileMethod = typeof(RuntimeHelpers).GetMethod("_CompileMethod", BindingFlags.Static | BindingFlags.NonPublic);
			MethodInfo runtimeHelpers__CompileMethod = DetourRuntimeNETPlatform._RuntimeHelpers__CompileMethod;
			DetourRuntimeNETPlatform._RuntimeHelpers__CompileMethod_TakesIntPtr = ((runtimeHelpers__CompileMethod != null) ? runtimeHelpers__CompileMethod.GetParameters()[0].ParameterType.FullName : null) == "System.IntPtr";
			MethodInfo runtimeHelpers__CompileMethod2 = DetourRuntimeNETPlatform._RuntimeHelpers__CompileMethod;
			DetourRuntimeNETPlatform._RuntimeHelpers__CompileMethod_TakesIRuntimeMethodInfo = ((runtimeHelpers__CompileMethod2 != null) ? runtimeHelpers__CompileMethod2.GetParameters()[0].ParameterType.FullName : null) == "System.IRuntimeMethodInfo";
			MethodInfo runtimeHelpers__CompileMethod3 = DetourRuntimeNETPlatform._RuntimeHelpers__CompileMethod;
			DetourRuntimeNETPlatform._RuntimeHelpers__CompileMethod_TakesRuntimeMethodHandleInternal = ((runtimeHelpers__CompileMethod3 != null) ? runtimeHelpers__CompileMethod3.GetParameters()[0].ParameterType.FullName : null) == "System.RuntimeMethodHandleInternal";
			DetourRuntimeNETPlatform.ThePreStub = IntPtr.Zero;
		}

		// Token: 0x0600193D RID: 6461 RVA: 0x00057F78 File Offset: 0x00056178
		[CompilerGenerated]
		private unsafe IntPtr <GetFunctionPointer>g__WalkPrecode|14_0(IntPtr curr)
		{
			long num = (long)curr;
			if (*(UIntPtr)num == 268435593U && *(UIntPtr)(num + 4L) == 2839556394U && *(UIntPtr)(num + 8L) == 3592356160U)
			{
				IntPtr intPtr = *(UIntPtr)(num + 16L);
				return this.NotThePreStub(curr, intPtr);
			}
			if (*(UIntPtr)num == 268435595U && *(UIntPtr)(num + 4L) == 2839556458U && *(UIntPtr)(num + 8L) == 3592356160U)
			{
				IntPtr intPtr2 = *(UIntPtr)(num + 16L);
				return this.NotThePreStub(curr, intPtr2);
			}
			if (*(UIntPtr)num == 268435468U && *(UIntPtr)(num + 4L) == 1476395115U && *(UIntPtr)(num + 8L) == 3592356192U)
			{
				IntPtr intPtr3 = *(UIntPtr)(num + 16L);
				return this.NotThePreStub(curr, intPtr3);
			}
			if (*(UIntPtr)num == 2432696336U && *(UIntPtr)(num + 4L) == 2432696352U && *(UIntPtr)(num + 8L) == 2432696833U && *(UIntPtr)(num + 12L) == 1476395120U && *(UIntPtr)(num + 16L) == 3592356352U)
			{
				IntPtr intPtr4 = *(UIntPtr)(num + 24L);
				return this.NotThePreStub(curr, intPtr4);
			}
			return curr;
		}

		// Token: 0x04001297 RID: 4759
		private static readonly object[] _NoArgs = new object[0];

		// Token: 0x04001298 RID: 4760
		private static readonly Type _RTDynamicMethod = typeof(DynamicMethod).GetNestedType("RTDynamicMethod", BindingFlags.NonPublic);

		// Token: 0x04001299 RID: 4761
		private static readonly FieldInfo _RTDynamicMethod_m_owner;

		// Token: 0x0400129A RID: 4762
		private static readonly FieldInfo _DynamicMethod_m_method;

		// Token: 0x0400129B RID: 4763
		private static readonly MethodInfo _DynamicMethod_GetMethodDescriptor;

		// Token: 0x0400129C RID: 4764
		private static readonly FieldInfo _RuntimeMethodHandle_m_value;

		// Token: 0x0400129D RID: 4765
		private static readonly MethodInfo _IRuntimeMethodInfo_get_Value;

		// Token: 0x0400129E RID: 4766
		private static readonly MethodInfo _RuntimeHelpers__CompileMethod;

		// Token: 0x0400129F RID: 4767
		private static readonly bool _RuntimeHelpers__CompileMethod_TakesIntPtr;

		// Token: 0x040012A0 RID: 4768
		private static readonly bool _RuntimeHelpers__CompileMethod_TakesIRuntimeMethodInfo;

		// Token: 0x040012A1 RID: 4769
		private static readonly bool _RuntimeHelpers__CompileMethod_TakesRuntimeMethodHandleInternal;

		// Token: 0x040012A2 RID: 4770
		private static IntPtr ThePreStub;
	}
}
