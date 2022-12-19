using System;

namespace System.Speech.Internal.SrgsParser
{
	// Token: 0x020000C5 RID: 197
	internal interface IScript : IElement
	{
		// Token: 0x06000454 RID: 1108
		IScript Create(string rule, RuleMethodScript onInit);
	}
}
