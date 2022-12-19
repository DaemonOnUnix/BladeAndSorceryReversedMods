using System;

namespace System.Speech.Internal.SrgsParser
{
	// Token: 0x020000D7 RID: 215
	internal interface IRule : IElement
	{
		// Token: 0x17000189 RID: 393
		// (get) Token: 0x06000785 RID: 1925
		// (set) Token: 0x06000784 RID: 1924
		string BaseClass { get; set; }

		// Token: 0x06000786 RID: 1926
		void CreateScript(IGrammar grammar, string rule, string method, RuleMethodScript type);
	}
}
