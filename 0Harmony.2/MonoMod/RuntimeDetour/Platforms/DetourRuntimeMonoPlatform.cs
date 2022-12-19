using System;
using System.Reflection;
using System.Reflection.Emit;

namespace MonoMod.RuntimeDetour.Platforms
{
	// Token: 0x02000483 RID: 1155
	internal class DetourRuntimeMonoPlatform : DetourRuntimeILPlatform
	{
		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x060018E6 RID: 6374 RVA: 0x00017DC4 File Offset: 0x00015FC4
		public override bool OnMethodCompiledWillBeCalled
		{
			get
			{
				return false;
			}
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x060018E7 RID: 6375 RVA: 0x0005688C File Offset: 0x00054A8C
		// (remove) Token: 0x060018E8 RID: 6376 RVA: 0x000568C4 File Offset: 0x00054AC4
		public override event OnMethodCompiledEvent OnMethodCompiled;

		// Token: 0x060018E9 RID: 6377 RVA: 0x000568FC File Offset: 0x00054AFC
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

		// Token: 0x060018EA RID: 6378 RVA: 0x0005694C File Offset: 0x00054B4C
		protected unsafe override void DisableInlining(MethodBase method, RuntimeMethodHandle handle)
		{
			ushort* ptr = (long)handle.Value / 2L + 2L;
			ushort* ptr2 = ptr;
			*ptr2 |= 8;
		}

		// Token: 0x04001260 RID: 4704
		private static readonly object[] _NoArgs = new object[0];

		// Token: 0x04001261 RID: 4705
		private static readonly MethodInfo _DynamicMethod_CreateDynMethod = typeof(DynamicMethod).GetMethod("CreateDynMethod", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04001262 RID: 4706
		private static readonly FieldInfo _DynamicMethod_mhandle = typeof(DynamicMethod).GetField("mhandle", BindingFlags.Instance | BindingFlags.NonPublic);
	}
}
