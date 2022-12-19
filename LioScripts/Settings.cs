using System;
using System.Collections;
using ThunderRoad;

namespace LioScripts
{
	// Token: 0x02000011 RID: 17
	public class Settings : LevelModule
	{
		// Token: 0x06000033 RID: 51 RVA: 0x00002DB8 File Offset: 0x00000FB8
		public override IEnumerator OnLoadCoroutine()
		{
			EventManager.onPossess += new EventManager.PossessEvent(this.EventManager_onPossess);
			EventManager.onCreatureKill += new EventManager.CreatureKillEvent(this.EventManager_onCreatureKill);
			return base.OnLoadCoroutine();
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002DF4 File Offset: 0x00000FF4
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

		// Token: 0x06000035 RID: 53 RVA: 0x00002E94 File Offset: 0x00001094
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

		// Token: 0x04000029 RID: 41
		public bool EnemiesDropLoot;

		// Token: 0x0400002A RID: 42
		public bool ManaRegenDisabled;

		// Token: 0x0400002B RID: 43
		public bool FocusRegenDisabled;

		// Token: 0x0400002C RID: 44
		public bool EmptyHoldersAtSpawn;
	}
}
