using System;
using System.Reflection;

namespace MonoMod.Utils
{
	// Token: 0x0200042A RID: 1066
	internal interface _IDMDGenerator
	{
		// Token: 0x06001674 RID: 5748
		MethodInfo Generate(DynamicMethodDefinition dmd, object context);
	}
}
