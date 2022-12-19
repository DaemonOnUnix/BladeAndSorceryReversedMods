using System;
using BloodMagic.Quest.Rewards;
using BloodMagic.Spell.Abilities;

namespace BloodMagic.Quest.Conditions
{
	// Token: 0x0200000C RID: 12
	public class Drain : MainCondition
	{
		// Token: 0x0600004A RID: 74 RVA: 0x00002958 File Offset: 0x00000B58
		public override MainCondition SetupMainCondition(int p_id, int p_level)
		{
			base.SetupMainCondition(p_id, p_level);
			this.progessGoal = (float)this.random.Next(50 * this.level, this.level * 60 * this.level);
			this.conditionText = string.Format("Drain {0} health from enemies", this.progessGoal);
			BloodDrain.OnDrain += this.OnDrainEvent;
			return this;
		}

		// Token: 0x0600004B RID: 75 RVA: 0x000029CC File Offset: 0x00000BCC
		public override void CompleteMainCondition()
		{
			base.CompleteMainCondition();
			BloodDrain.OnDrain -= this.OnDrainEvent;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x000029E8 File Offset: 0x00000BE8
		private void OnDrainEvent(float drainedHealth)
		{
			base.IsAllConditionsMet(base.GetType(), null, (float)Math.Round((double)drainedHealth, 5));
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002A04 File Offset: 0x00000C04
		public override void SetupRewards()
		{
			base.SetupRewards();
			this.rewards.Add(new LightXP());
			this.rewards[0].CreateReward(this.seed, (float)(this.totalCost + 1), this.random);
			bool flag = this.random.NextDouble() >= 0.3;
			if (!flag)
			{
				this.rewards.Add(new DrainPower().CreateReward(this.seed, (float)(this.totalCost + 1), this.random));
			}
		}
	}
}
