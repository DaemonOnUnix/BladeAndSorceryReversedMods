using System;
using System.Collections;
using ThunderRoad;

namespace LioScripts
{
	// Token: 0x0200000D RID: 13
	public class Settings : LevelModule
	{
		// Token: 0x06000021 RID: 33 RVA: 0x0000297C File Offset: 0x00000B7C
		public override IEnumerator OnLoadCoroutine()
		{
			EventManager.onPossess += new EventManager.PossessEvent(this.EventManager_onPossess);
			EventManager.onCreatureKill += new EventManager.CreatureKillEvent(this.EventManager_onCreatureKill);
			return base.OnLoadCoroutine();
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000029B8 File Offset: 0x00000BB8
		private void EventManager_onCreatureKill(Creature creature, Player player, CollisionInstance collisionInstance, EventTime eventTime)
		{
			bool flag = eventTime != 1 || !this.EnemiesDropLoot || creature.isPlayer;
			if (!flag)
			{
				Catalog.GetData<LootTable>("EnemyDropLoot", true).Pick(0).SpawnAsync(delegate(Item loot)
				{
					loot.transform.position = creature.transform.position;
					loot.transform.rotation = creature.transform.rotation;
				}, null, null, null, true, null);
				Catalog.GetData<EffectData>("DL.Effect.LootDrop", true).Spawn(Player.local.transform, true, null, false, Array.Empty<Type>()).Play(0, false);
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002A58 File Offset: 0x00000C58
		private void EventManager_onPossess(Creature creature, EventTime eventTime)
		{
			bool flag = eventTime != 1;
			if (!flag)
			{
				bool manaRegenDisabled = this.ManaRegenDisabled;
				if (manaRegenDisabled)
				{
					creature.mana.manaRegen = 0f;
				}
				bool focusRegenDisabled = this.FocusRegenDisabled;
				if (focusRegenDisabled)
				{
					creature.mana.focusRegen = 0f;
				}
			}
		}

		// Token: 0x04000015 RID: 21
		public bool EnemiesDropLoot;

		// Token: 0x04000016 RID: 22
		public bool ManaRegenDisabled;

		// Token: 0x04000017 RID: 23
		public bool FocusRegenDisabled;

		// Token: 0x04000018 RID: 24
		public bool EmptyHoldersAtSpawn;
	}
}
