using System;
using BloodMagic.UI;
using UnityEngine;

namespace BloodMagic.Quest.Rewards
{
	// Token: 0x02000012 RID: 18
	internal class DrainPower : Reward
	{
		// Token: 0x0600006E RID: 110 RVA: 0x00003340 File Offset: 0x00001540
		public override Reward CreateReward(int id, float totalCost, Random random)
		{
			base.CreateReward(id, totalCost, random);
			this.power = (float)Math.Round((double)Mathf.Lerp(0.5f, 1f, (float)random.NextDouble()) * (double)totalCost * 0.25, 1);
			this.rewardText = string.Format("{0} Drain power", this.power);
			return this;
		}

		// Token: 0x0600006F RID: 111 RVA: 0x000033A9 File Offset: 0x000015A9
		public override void GiveReward()
		{
			base.GiveReward();
			BookUIHandler.saveData.drainPower += this.power;
		}

		// Token: 0x0400002D RID: 45
		private float power;
	}
}
