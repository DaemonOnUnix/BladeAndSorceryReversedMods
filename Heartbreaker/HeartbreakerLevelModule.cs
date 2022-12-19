using System;
using System.Collections;
using System.Reflection;
using HarmonyLib;
using ThunderRoad;
using UnityEngine;

namespace Heartbreaker
{
	// Token: 0x02000003 RID: 3
	public class HeartbreakerLevelModule : LevelModule
	{
		// Token: 0x06000011 RID: 17 RVA: 0x00002D60 File Offset: 0x00000F60
		public override IEnumerator OnLoadCoroutine()
		{
			HeartbreakerLevelModule.local = this;
			this.harmony = new Harmony("com.hujohner.heartbreaker");
			this.harmony.PatchAll(Assembly.GetExecutingAssembly());
			this.heartData = Catalog.GetData<ItemData>("Heart", true);
			Debug.Log(string.Format("[Heartbreaker] Mod v{0} loaded.", Assembly.GetExecutingAssembly().GetName().Version));
			EventManager.onCreatureSpawn += new EventManager.CreatureSpawnedEvent(this.EventManager_onCreatureSpawn);
			this.AddTips();
			return base.OnLoadCoroutine();
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002DE8 File Offset: 0x00000FE8
		public override void OnUnload()
		{
			EventManager.onCreatureSpawn -= new EventManager.CreatureSpawnedEvent(this.EventManager_onCreatureSpawn);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002E00 File Offset: 0x00001000
		private void EventManager_onCreatureSpawn(Creature creature)
		{
			bool isPlayer = creature.isPlayer;
			if (!isPlayer)
			{
				bool flag = Array.Exists<string>(HeartbreakerLevelModule.creatureWithHeartList, (string elem) => elem == creature.creatureId);
				if (flag)
				{
					this.heartData.SpawnAsync(delegate(Item item)
					{
						item.gameObject.SetActive(false);
						HeartBehaviour heartBehaviour = item.gameObject.AddComponent<HeartBehaviour>();
						heartBehaviour.creature = creature;
						item.gameObject.SetActive(true);
					}, null, null, null, true, null);
				}
			}
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002E74 File Offset: 0x00001074
		private void AddTips()
		{
			TextData.TextGroup tips = LocalizationManager.Instance.GetGroup("Tips");
			bool flag = tips != null;
			if (flag)
			{
				int id = tips.texts.Count + 1;
				TextData.TextID tip = new TextData.TextID
				{
					id = id.ToString(),
					text = string.Format("[Heartbreaker] Squeezing the heart of an opponent for {0} seconds will make their heart explode.", this.timeTillSqueezedHeartExplodes)
				};
				tips.texts.Add(tip);
				int id2 = tips.texts.Count + 1;
				TextData.TextID tip2 = new TextData.TextID
				{
					id = id2.ToString(),
					text = "[Heartbreaker] You can use the Phase spell to phase through solid objects."
				};
				tips.texts.Add(tip2);
				string willwont = (this.surviveHeartRippedOut ? "will" : "wont");
				string endis = (this.surviveHeartRippedOut ? "dis" : "en");
				int id3 = tips.texts.Count + 1;
				TextData.TextID tip3 = new TextData.TextID
				{
					id = id3.ToString(),
					text = string.Concat(new string[] { "[Heartbreaker] Enemies ", willwont, " survive ripping their heart out unless you ", endis, "able it in the mod settings." })
				};
				tips.texts.Add(tip3);
			}
		}

		// Token: 0x0400000C RID: 12
		public static HeartbreakerLevelModule local;

		// Token: 0x0400000D RID: 13
		[Range(0f, 1f)]
		public float minGripIntensity;

		// Token: 0x0400000E RID: 14
		public float timeTillSqueezedHeartExplodes;

		// Token: 0x0400000F RID: 15
		public float minRipVelocity;

		// Token: 0x04000010 RID: 16
		public bool noSoundForRippingOutHeart;

		// Token: 0x04000011 RID: 17
		public bool noHandVibrationWhenPhasing;

		// Token: 0x04000012 RID: 18
		public bool surviveHeartRippedOut;

		// Token: 0x04000013 RID: 19
		public bool ignoreHeartCollisions;

		// Token: 0x04000014 RID: 20
		public static string[] creatureWithHeartList = new string[] { "HumanFemale", "HumanMale" };

		// Token: 0x04000015 RID: 21
		private Harmony harmony;

		// Token: 0x04000016 RID: 22
		private ItemData heartData;
	}
}
