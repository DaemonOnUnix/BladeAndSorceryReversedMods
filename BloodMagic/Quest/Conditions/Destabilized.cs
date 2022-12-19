using System;
using ThunderRoad;

namespace BloodMagic.Quest.Conditions
{
	// Token: 0x02000008 RID: 8
	public class Destabilized : Condition
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000036 RID: 54 RVA: 0x000026FD File Offset: 0x000008FD
		public override int conditionCost
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x000027B0 File Offset: 0x000009B0
		public override Condition SetupCondition(int p_seed, int p_level)
		{
			base.SetupCondition(p_seed, p_level);
			this.conditionText = "while enemy is knocked out";
			return this;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000027D7 File Offset: 0x000009D7
		public override bool CanBeUsedWithType(Type mainType)
		{
			return mainType == typeof(Kill) || mainType == typeof(Dismember);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000027FE File Offset: 0x000009FE
		public override bool IsConditionMet(Type mainType, object[] parseProperties = null)
		{
			return (parseProperties[0] as Creature).state == 1;
		}
	}
}
