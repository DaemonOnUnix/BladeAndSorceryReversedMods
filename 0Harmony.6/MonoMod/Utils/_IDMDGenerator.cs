using System;
using System.Reflection;

namespace MonoMod.Utils
{
	// Token: 0x02000332 RID: 818
	internal interface _IDMDGenerator
	{
		// Token: 0x060012FE RID: 4862
		MethodInfo Generate(DynamicMethodDefinition dmd, object context);
	}
}
