using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Speech.Recognition.SrgsGrammar;

namespace System.Speech.Internal.SrgsParser
{
	// Token: 0x020000D2 RID: 210
	internal interface IGrammar : IElement
	{
		// Token: 0x0600076D RID: 1901
		IRule CreateRule(string id, RulePublic publicRule, RuleDynamic dynamic, bool hasSCript);

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x0600076F RID: 1903
		// (set) Token: 0x0600076E RID: 1902
		string Root { get; set; }

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000771 RID: 1905
		// (set) Token: 0x06000770 RID: 1904
		SrgsTagFormat TagFormat { get; set; }

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000773 RID: 1907
		// (set) Token: 0x06000772 RID: 1906
		Collection<string> GlobalTags { get; set; }

		// Token: 0x1700017F RID: 383
		// (set) Token: 0x06000774 RID: 1908
		GrammarType Mode { set; }

		// Token: 0x17000180 RID: 384
		// (set) Token: 0x06000775 RID: 1909
		CultureInfo Culture { set; }

		// Token: 0x17000181 RID: 385
		// (set) Token: 0x06000776 RID: 1910
		Uri XmlBase { set; }

		// Token: 0x17000182 RID: 386
		// (set) Token: 0x06000777 RID: 1911
		AlphabetType PhoneticAlphabet { set; }

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000779 RID: 1913
		// (set) Token: 0x06000778 RID: 1912
		string Language { get; set; }

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x0600077B RID: 1915
		// (set) Token: 0x0600077A RID: 1914
		string Namespace { get; set; }

		// Token: 0x17000185 RID: 389
		// (set) Token: 0x0600077C RID: 1916
		bool Debug { set; }

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x0600077D RID: 1917
		// (set) Token: 0x0600077E RID: 1918
		Collection<string> CodeBehind { get; set; }

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x0600077F RID: 1919
		// (set) Token: 0x06000780 RID: 1920
		Collection<string> ImportNamespaces { get; set; }

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06000781 RID: 1921
		// (set) Token: 0x06000782 RID: 1922
		Collection<string> AssemblyReferences { get; set; }
	}
}
