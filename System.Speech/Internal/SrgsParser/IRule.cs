using System;

namespace System.Speech.Internal.SrgsParser
{
	// Token: 0x020000B1 RID: 177
	internal interface IRule : IElement
	{
		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060003E9 RID: 1001
		// (set) Token: 0x060003E8 RID: 1000
		string BaseClass { get; set; }

		// Token: 0x060003EA RID: 1002
		void CreateScript(IGrammar grammar, string rule, string method, RuleMethodScript type);
	}
}
