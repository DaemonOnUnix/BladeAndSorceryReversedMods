using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Speech.Recognition.SrgsGrammar;

namespace System.Speech.Internal.SrgsParser
{
	// Token: 0x020000AB RID: 171
	internal interface IGrammar : IElement
	{
		// Token: 0x060003AE RID: 942
		IRule CreateRule(string id, RulePublic publicRule, RuleDynamic dynamic, bool hasSCript);

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060003B0 RID: 944
		// (set) Token: 0x060003AF RID: 943
		string Root { get; set; }

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060003B2 RID: 946
		// (set) Token: 0x060003B1 RID: 945
		SrgsTagFormat TagFormat { get; set; }

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060003B4 RID: 948
		// (set) Token: 0x060003B3 RID: 947
		Collection<string> GlobalTags { get; set; }

		// Token: 0x1700006D RID: 109
		// (set) Token: 0x060003B5 RID: 949
		GrammarType Mode { set; }

		// Token: 0x1700006E RID: 110
		// (set) Token: 0x060003B6 RID: 950
		CultureInfo Culture { set; }

		// Token: 0x1700006F RID: 111
		// (set) Token: 0x060003B7 RID: 951
		Uri XmlBase { set; }

		// Token: 0x17000070 RID: 112
		// (set) Token: 0x060003B8 RID: 952
		AlphabetType PhoneticAlphabet { set; }

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060003BA RID: 954
		// (set) Token: 0x060003B9 RID: 953
		string Language { get; set; }

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060003BC RID: 956
		// (set) Token: 0x060003BB RID: 955
		string Namespace { get; set; }

		// Token: 0x17000073 RID: 115
		// (set) Token: 0x060003BD RID: 957
		bool Debug { set; }

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060003BE RID: 958
		// (set) Token: 0x060003BF RID: 959
		Collection<string> CodeBehind { get; set; }

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060003C0 RID: 960
		// (set) Token: 0x060003C1 RID: 961
		Collection<string> ImportNamespaces { get; set; }

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060003C2 RID: 962
		// (set) Token: 0x060003C3 RID: 963
		Collection<string> AssemblyReferences { get; set; }
	}
}
