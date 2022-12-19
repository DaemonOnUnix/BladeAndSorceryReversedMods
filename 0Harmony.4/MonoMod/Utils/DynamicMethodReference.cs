using System;
using System.Reflection;
using Mono.Cecil;

namespace MonoMod.Utils
{
	// Token: 0x0200043A RID: 1082
	internal class DynamicMethodReference : MethodReference
	{
		// Token: 0x060016C8 RID: 5832 RVA: 0x0004C114 File Offset: 0x0004A314
		public DynamicMethodReference(ModuleDefinition module, MethodInfo dm)
			: base("", module.TypeSystem.Void)
		{
			this.DynamicMethod = dm;
		}

		// Token: 0x04000FE9 RID: 4073
		public MethodInfo DynamicMethod;
	}
}
