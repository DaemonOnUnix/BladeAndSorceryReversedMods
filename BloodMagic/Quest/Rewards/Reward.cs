using System;

namespace BloodMagic.Quest.Rewards
{
	// Token: 0x02000014 RID: 20
	public class Reward
	{
		// Token: 0x06000074 RID: 116 RVA: 0x00003465 File Offset: 0x00001665
		public virtual Reward CreateReward(int id, float totalCost, Random random)
		{
			return null;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00002E8B File Offset: 0x0000108B
		public virtual void GiveReward()
		{
		}

		// Token: 0x0400002F RID: 47
		public string rewardText;
	}
}
