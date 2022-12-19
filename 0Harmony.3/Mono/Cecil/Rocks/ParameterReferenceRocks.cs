using System;

namespace Mono.Cecil.Rocks
{
	// Token: 0x0200032A RID: 810
	internal static class ParameterReferenceRocks
	{
		// Token: 0x060012D9 RID: 4825 RVA: 0x0003FE77 File Offset: 0x0003E077
		public static int GetSequence(this ParameterReference self)
		{
			return self.Index + 1;
		}
	}
}
