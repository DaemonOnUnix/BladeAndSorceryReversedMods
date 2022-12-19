using System;
using BloodMagic.UI;
using UnityEngine;

namespace BloodMagic.Quest.Rewards
{
	// Token: 0x02000013 RID: 19
	internal class LightXP : Reward
	{
		// Token: 0x06000071 RID: 113 RVA: 0x000033CC File Offset: 0x000015CC
		public override Reward CreateReward(int id, float totalCost, Random random)
		{
			base.CreateReward(id, totalCost, random);
			float num = Mathf.Lerp(0.5f, 1f, (float)random.NextDouble());
			this.lp = (int)Math.Round((double)totalCost + (double)num * 3.0 * (double)BookUIHandler.saveData.xpMultiplier, 0);
			this.rewardText = string.Format("{0} LP", this.lp);
			return this;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00003443 File Offset: 0x00001643
		public override void GiveReward()
		{
			base.GiveReward();
			BookUIHandler.saveData.lightPoints += this.lp;
		}

		// Token: 0x0400002E RID: 46
		private int lp;
	}
}
