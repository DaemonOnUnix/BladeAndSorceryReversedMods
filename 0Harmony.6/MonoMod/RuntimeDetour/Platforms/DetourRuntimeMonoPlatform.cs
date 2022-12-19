using System;
using System.Reflection;
using System.Reflection.Emit;

namespace MonoMod.RuntimeDetour.Platforms
{
	// Token: 0x02000381 RID: 897
	internal class DetourRuntimeMonoPlatform : DetourRuntimeILPlatform
	{
		// Token: 0x0600151C RID: 5404 RVA: 0x0004D474 File Offset: 0x0004B674
		protected override RuntimeMethodHandle GetMethodHandle(MethodBase method)
		{
			if (method is DynamicMethod)
			{
				MethodInfo dynamicMethod_CreateDynMethod = DetourRuntimeMonoPlatform._DynamicMethod_CreateDynMethod;
				if (dynamicMethod_CreateDynMethod != null)
				{
					dynamicMethod_CreateDynMethod.Invoke(method, DetourRuntimeMonoPlatform._NoArgs);
				}
				if (DetourRuntimeMonoPlatform._DynamicMethod_mhandle != null)
				{
					return (RuntimeMethodHandle)DetourRuntimeMonoPlatform._DynamicMethod_mhandle.GetValue(method);
				}
			}
			return method.MethodHandle;
		}

		// Token: 0x0600151D RID: 5405 RVA: 0x0004D4C4 File Offset: 0x0004B6C4
		protected unsafe override void DisableInlining(MethodBase method, RuntimeMethodHandle handle)
		{
			ushort* ptr = (long)handle.Value / 2L + 2L;
			ushort* ptr2 = ptr;
			*ptr2 |= 8;
		}

		// Token: 0x040011EF RID: 4591
		private static readonly object[] _NoArgs = new object[0];

		// Token: 0x040011F0 RID: 4592
		private static readonly MethodInfo _DynamicMethod_CreateDynMethod = typeof(DynamicMethod).GetMethod("CreateDynMethod", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x040011F1 RID: 4593
		private static readonly FieldInfo _DynamicMethod_mhandle = typeof(DynamicMethod).GetField("mhandle", BindingFlags.Instance | BindingFlags.NonPublic);
	}
}
