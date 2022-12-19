using System;
using BloodMagic.Quest.Rewards;
using ThunderRoad;

namespace BloodMagic.Quest.Conditions
{
	// Token: 0x0200000E RID: 14
	public class Parry : MainCondition
	{
		// Token: 0x06000055 RID: 85 RVA: 0x00002BF4 File Offset: 0x00000DF4
		public override MainCondition SetupMainCondition(int p_id, int p_level)
		{
			base.SetupMainCondition(p_id, p_level);
			this.progessGoal = (float)this.random.Next(this.level, (this.level + 4) * this.level);
			this.conditionText = string.Format("Parry {0} attacks", this.progessGoal);
			EventManager.onCreatureParry += new EventManager.CreatureHitEvent(this.EventManager_onCreatureParry);
			return this;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00002C64 File Offset: 0x00000E64
		private void EventManager_onCreatureParry(Creature creature, CollisionInstance collisionInstance)
		{
			bool flag = !(creature != null) || creature.isPlayer || collisionInstance.IsDoneByPlayer();
			if (!flag)
			{
				base.UpdateProgress(1f);
			}
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00002C9E File Offset: 0x00000E9E
		public override void CompleteMainCondition()
		{
			base.CompleteMainCondition();
			EventManager.onCreatureParry -= new EventManager.CreatureHitEvent(this.EventManager_onCreatureParry);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00002CBC File Offset: 0x00000EBC
		public override void SetupRewards()
		{
			base.SetupRewards();
			this.rewards.Add((this.random.NextDouble() > 0.5) ? new DarkXP() : new LightXP());
			this.rewards[0].CreateReward(this.seed, (float)(this.totalCost + 1), this.random);
		}
	}
}
