using System;
using System.Collections.ObjectModel;
using System.Speech.Internal;

namespace System.Speech.Recognition.SrgsGrammar
{
	// Token: 0x02000081 RID: 129
	[Serializable]
	public sealed class SrgsRulesCollection : KeyedCollection<string, SrgsRule>
	{
		// Token: 0x06000465 RID: 1125 RVA: 0x00011A48 File Offset: 0x0000FC48
		public void Add(params SrgsRule[] rules)
		{
			Helpers.ThrowIfNull(rules, "rules");
			for (int i = 0; i < rules.Length; i++)
			{
				if (rules[i] == null)
				{
					throw new ArgumentNullException("rules", SR.Get(SRID.ParamsEntryNullIllegal, new object[0]));
				}
				base.Add(rules[i]);
			}
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x00011A93 File Offset: 0x0000FC93
		protected override string GetKeyForItem(SrgsRule rule)
		{
			if (rule == null)
			{
				throw new ArgumentNullException("rule");
			}
			return rule.Id;
		}
	}
}
