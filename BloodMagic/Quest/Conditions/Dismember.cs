using System;
using BloodMagic.Quest.Rewards;
using ThunderRoad;

namespace BloodMagic.Quest.Conditions
{
	// Token: 0x0200000D RID: 13
	public class Dismember : MainCondition
	{
		// Token: 0x0600004F RID: 79 RVA: 0x00002AA4 File Offset: 0x00000CA4
		public override MainCondition SetupMainCondition(int p_id, int p_level)
		{
			base.SetupMainCondition(p_id, p_level);
			this.progessGoal = (float)this.random.Next(this.level * 2, (this.level + 4) * this.level);
			this.conditionText = string.Format("Dismember {0} limbs", this.progessGoal);
			EventManager.onCreatureSpawn += new EventManager.CreatureSpawnedEvent(this.EventManager_onCreatureSpawn);
			return this;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002B18 File Offset: 0x00000D18
		private void EventManager_onCreatureSpawn(Creature creature)
		{
			bool isPlayer = creature.isPlayer;
			if (!isPlayer)
			{
				creature.ragdoll.OnSliceEvent += new Ragdoll.SliceEvent(this.Ragdoll_OnSliceEvent);
			}
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002B4C File Offset: 0x00000D4C
		private void Ragdoll_OnSliceEvent(RagdollPart ragdollPart, EventTime eventTime)
		{
			bool flag = eventTime > 0;
			if (!flag)
			{
				ragdollPart.ragdoll.OnSliceEvent -= new Ragdoll.SliceEvent(this.Ragdoll_OnSliceEvent);
				base.UpdateProgress(1f);
			}
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00002B88 File Offset: 0x00000D88
		public override void CompleteMainCondition()
		{
			base.CompleteMainCondition();
			EventManager.onCreatureSpawn -= new EventManager.CreatureSpawnedEvent(this.EventManager_onCreatureSpawn);
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00002BA4 File Offset: 0x00000DA4
		public override void SetupRewards()
		{
			base.SetupRewards();
			this.rewards.Add(new DarkXP());
			this.rewards[0].CreateReward(this.seed, (float)(this.totalCost + 1), this.random);
		}
	}
}
