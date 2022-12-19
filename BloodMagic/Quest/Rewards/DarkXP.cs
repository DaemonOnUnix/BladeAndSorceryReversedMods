using System;
using BloodMagic.UI;
using UnityEngine;

namespace BloodMagic.Quest.Rewards
{
	// Token: 0x02000011 RID: 17
	internal class DarkXP : Reward
	{
		// Token: 0x0600006B RID: 107 RVA: 0x0000329C File Offset: 0x0000149C
		public override Reward CreateReward(int id, float totalCost, Random random)
		{
			base.CreateReward(id, totalCost, random);
			float num = Mathf.Lerp(0.5f, 1f, (float)random.NextDouble());
			this.dp = (int)Math.Round((double)totalCost + (double)num * 3.0 * (double)BookUIHandler.saveData.xpMultiplier, 0);
			this.rewardText = string.Format("{0} DP", this.dp);
			return this;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00003313 File Offset: 0x00001513
		public override void GiveReward()
		{
			base.GiveReward();
			BookUIHandler.saveData.darkPoints += this.dp;
		}

		// Token: 0x0400002C RID: 44
		private int dp;
	}
}
