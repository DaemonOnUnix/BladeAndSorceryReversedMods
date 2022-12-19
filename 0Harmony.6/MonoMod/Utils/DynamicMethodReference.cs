using System;
using System.Reflection.Emit;
using Mono.Cecil;

namespace MonoMod.Utils
{
	// Token: 0x02000342 RID: 834
	internal class DynamicMethodReference : MethodReference
	{
		// Token: 0x06001351 RID: 4945 RVA: 0x00044074 File Offset: 0x00042274
		public DynamicMethodReference(ModuleDefinition module, DynamicMethod dm)
			: base("", module.TypeSystem.Void)
		{
			this.DynamicMethod = dm;
		}

		// Token: 0x04000FAA RID: 4010
		public DynamicMethod DynamicMethod;
	}
}
