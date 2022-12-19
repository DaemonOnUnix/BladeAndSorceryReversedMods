using System;

namespace Mono.Cecil.Rocks
{
	// Token: 0x02000420 RID: 1056
	internal static class ParameterReferenceRocks
	{
		// Token: 0x06001648 RID: 5704 RVA: 0x00047DBF File Offset: 0x00045FBF
		public static int GetSequence(this ParameterReference self)
		{
			return self.Index + 1;
		}
	}
}
