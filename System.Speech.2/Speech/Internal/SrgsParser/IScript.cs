using System;

namespace System.Speech.Internal.SrgsParser
{
	// Token: 0x020000DC RID: 220
	internal interface IScript : IElement
	{
		// Token: 0x06000788 RID: 1928
		IScript Create(string rule, RuleMethodScript onInit);
	}
}
