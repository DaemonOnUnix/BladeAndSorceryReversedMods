using System;
using System.Collections;
using ThunderRoad;
using UnityEngine;

namespace TimeStopSpell
{
	// Token: 0x0200000B RID: 11
	public class TimeStopSpellLevelModule : LevelModule
	{
		// Token: 0x06000031 RID: 49 RVA: 0x0000350C File Offset: 0x0000170C
		public override IEnumerator OnLoadCoroutine()
		{
			EventManager.onCreatureSpawn += new EventManager.CreatureSpawnedEvent(this.EventManagerOnonCreatureSpawn);
			EventManager.onLevelLoad += new EventManager.LevelLoadEvent(this.EventManagerOnonLevelLoad);
			this._timeStopData = GameManager.local.gameObject.GetComponent<TimeStopData>();
			return base.OnLoadCoroutine();
		}

		// Token: 0x06000032 RID: 50 RVA: 0x0000354B File Offset: 0x0000174B
		private void EventManagerOnonLevelLoad(LevelData leveldata, EventTime eventtime)
		{
			if (this._timeStopData != null)
			{
				this._timeStopData.isTimeStopped = false;
			}
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00003568 File Offset: 0x00001768
		private void EventManagerOnonCreatureSpawn(Creature creature)
		{
			if (creature.isPlayer)
			{
				return;
			}
			creature.ragdoll.physicTogglePlayerRadius = 1000f;
			creature.ragdoll.physicToggleRagdollRadius = 1000f;
			try
			{
				Object.Destroy(creature.gameObject.GetComponent<FrozenRagdollCreature>());
			}
			catch (Exception ex)
			{
				Debug.Log(ex.Message);
			}
			try
			{
				Object.Destroy(creature.gameObject.GetComponent<FrozenAnimationCreature>());
			}
			catch (Exception ex2)
			{
				Debug.Log(ex2.Message);
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x000035FC File Offset: 0x000017FC
		public override void Update()
		{
			if (this._timeStopData == null)
			{
				this._timeStopData = GameManager.local.gameObject.GetComponent<TimeStopData>();
				return;
			}
			if (!this._timeStopData.isTimeStopped)
			{
				return;
			}
			foreach (Creature creature in Creature.allActive)
			{
				if (!creature.isPlayer)
				{
					if (creature.gameObject.GetComponent<FrozenRagdollCreature>() == null)
					{
						creature.gameObject.AddComponent<FrozenRagdollCreature>();
					}
					if (creature.gameObject.GetComponent<FrozenAnimationCreature>() == null)
					{
						creature.gameObject.AddComponent<FrozenAnimationCreature>();
					}
				}
			}
			foreach (Item item in Item.allActive)
			{
				if ((item.isThrowed || item.isFlying) && item.gameObject.GetComponent<FrozenItem>() == null)
				{
					item.gameObject.AddComponent<FrozenItem>();
				}
			}
		}

		// Token: 0x04000020 RID: 32
		private TimeStopData _timeStopData;
	}
}
