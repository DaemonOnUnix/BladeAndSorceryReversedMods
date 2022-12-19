using System;
using BloodMagic.Quest.Rewards;
using ThunderRoad;
using UnityEngine;

namespace BloodMagic.Quest.Conditions
{
	// Token: 0x02000010 RID: 16
	public class Kill : MainCondition
	{
		// Token: 0x06000066 RID: 102 RVA: 0x0000315C File Offset: 0x0000135C
		public override MainCondition SetupMainCondition(int p_id, int p_level)
		{
			base.SetupMainCondition(p_id, p_level);
			this.progessGoal = (float)this.random.Next(this.level, (this.level + 4) * this.level);
			this.conditionText = string.Format("Kill {0} enemies", this.progessGoal);
			EventManager.onCreatureKill += new EventManager.CreatureKillEvent(this.OnCreatureKill);
			return this;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x000031CC File Offset: 0x000013CC
		private void OnCreatureKill(Creature creature, Player player, CollisionInstance collisionInstance, EventTime eventTime)
		{
			bool flag = eventTime > 0;
			if (!flag)
			{
				Debug.Log(base.GetType().FullName + " :: On Creature Killed");
				base.IsAllConditionsMet(base.GetType(), new object[] { creature, player, collisionInstance, eventTime }, 1f);
			}
		}

		// Token: 0x06000068 RID: 104 RVA: 0x0000322E File Offset: 0x0000142E
		public override void CompleteMainCondition()
		{
			base.CompleteMainCondition();
			EventManager.onCreatureKill -= new EventManager.CreatureKillEvent(this.OnCreatureKill);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x0000324C File Offset: 0x0000144C
		public override void SetupRewards()
		{
			base.SetupRewards();
			this.rewards.Add(new DarkXP());
			this.rewards[0].CreateReward(this.seed, (float)(this.totalCost + 1), this.random);
		}
	}
}
