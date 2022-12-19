using System;
using ThunderRoad;

namespace BloodMagic.Quest.Conditions
{
	// Token: 0x0200000B RID: 11
	public class InAir : Condition
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000045 RID: 69 RVA: 0x00002902 File Offset: 0x00000B02
		public override int conditionCost
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002908 File Offset: 0x00000B08
		public override Condition SetupCondition(int p_seed, int p_level)
		{
			base.SetupCondition(p_seed, p_level);
			this.conditionText = "while in the air";
			return this;
		}

		// Token: 0x06000047 RID: 71 RVA: 0x0000292F File Offset: 0x00000B2F
		public override bool IsConditionMet(Type mainType, object[] parseProperties = null)
		{
			return !Player.currentCreature.locomotion.isGrounded;
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002943 File Offset: 0x00000B43
		public override bool CanBeUsedWithType(Type mainType)
		{
			return !(mainType == typeof(Drain));
		}
	}
}
