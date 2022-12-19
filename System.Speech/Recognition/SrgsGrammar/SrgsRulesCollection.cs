using System;
using System.Collections.ObjectModel;
using System.Speech.Internal;

namespace System.Speech.Recognition.SrgsGrammar
{
	// Token: 0x02000124 RID: 292
	[Serializable]
	public sealed class SrgsRulesCollection : KeyedCollection<string, SrgsRule>
	{
		// Token: 0x060007C1 RID: 1985 RVA: 0x000224EC File Offset: 0x000214EC
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

		// Token: 0x060007C2 RID: 1986 RVA: 0x00022537 File Offset: 0x00021537
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
