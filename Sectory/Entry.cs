using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using ThunderRoad;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Sectory
{
	// Token: 0x02000013 RID: 19
	public class Entry : LevelModule
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000022 RID: 34 RVA: 0x00002816 File Offset: 0x00000A16
		public static string GetSettingsPath
		{
			get
			{
				return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Settings");
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002831 File Offset: 0x00000A31
		public string ClipFamilyPath(string path)
		{
			return path.Split(new char[] { Path.DirectorySeparatorChar }).Last<string>().Split(new char[] { '.' })
				.First<string>();
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000024 RID: 36 RVA: 0x00002861 File Offset: 0x00000A61
		public static int[] GetNNInit
		{
			get
			{
				return new int[] { 40, 45, 20, 15, 19, 20, 12, 6, 11 };
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002878 File Offset: 0x00000A78
		public override IEnumerator OnLoadCoroutine()
		{
			Entry.inst = this;
			foreach (CreatureData creatureData in from id in Catalog.GetAllID<CreatureData>()
				select Catalog.GetData<CreatureData>(id, true))
			{
				this.creatureToHealth.Add(creatureData, creatureData.health);
			}
			this.Jsons();
			this.Events();
			this.Init();
			Debug.Log("If you see this: Sectory has successfully loaded.");
			return base.OnLoadCoroutine();
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002928 File Offset: 0x00000B28
		private void Events()
		{
			EventManager.onCreatureSpawn += new EventManager.CreatureSpawnedEvent(this.Spawn);
			EventManager.onPossess += new EventManager.PossessEvent(this.PlayerSpawn);
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002950 File Offset: 0x00000B50
		private void Jsons()
		{
			this.ragdollInfo = JsonConvert.DeserializeObject<RagdollInfo>(File.ReadAllText(Path.Combine(Entry.GetSettingsPath, "RagdollModifiers.json")));
			this.injuryInfo = JsonConvert.DeserializeObject<InjuryInfo>(File.ReadAllText(Path.Combine(Entry.GetSettingsPath, "InjurySettings.json")));
			this.bleedInfo = JsonConvert.DeserializeObject<BleedInfo>(File.ReadAllText(Path.Combine(Entry.GetSettingsPath, "BleedSettings.json")));
			this.generalInfo = JsonConvert.DeserializeObject<GeneralInfo>(File.ReadAllText(Path.Combine(Entry.GetSettingsPath, "GeneralSettings.json")));
			this.stealthInfo = JsonConvert.DeserializeObject<StealthInfo>(File.ReadAllText(Path.Combine(Entry.GetSettingsPath, "StealthSettings.json")));
			this.lootInfo = JsonConvert.DeserializeObject<LootInfo>(File.ReadAllText(Path.Combine(Entry.GetSettingsPath, "LootSettings.json")));
			this.unconsciousInfo = JsonConvert.DeserializeObject<UnconsciousInfo>(File.ReadAllText(Path.Combine(Entry.GetSettingsPath, "UnconsciousSettings.json")));
			this.internalsInfo = JsonConvert.DeserializeObject<InternalsInfo>(File.ReadAllText(Path.Combine(Entry.GetSettingsPath, "InternalsSettings.json")));
			this.aiSettings = JsonConvert.DeserializeObject<AiSettings>(File.ReadAllText(Path.Combine(Entry.GetSettingsPath, "AiSettings.json")));
			this.twistInfo = JsonConvert.DeserializeObject<TwistInjuriesInfo>(File.ReadAllText(Path.Combine(Entry.GetSettingsPath, "TwistInjurySettings.json")));
			this.bleedData = (Catalog.GetAllID<EffectData>().Any((string i) => i == "SushinBleedVfx") ? Catalog.GetData<EffectData>("SushinBleedVfx", true) : Catalog.GetData<EffectData>("Bleed", true));
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002AE0 File Offset: 0x00000CE0
		private void Init()
		{
			SectoryMenu.InjectClassIntoMenu(new SectoryMenu.InjectEditClass("Bleed", this.bleedInfo));
			SectoryMenu.InjectClassIntoMenu(new SectoryMenu.InjectEditClass("General", this.generalInfo));
			SectoryMenu.InjectClassIntoMenu(new SectoryMenu.InjectEditClass("Injury", this.injuryInfo));
			SectoryMenu.InjectClassIntoMenu(new SectoryMenu.InjectEditClass("Ragdoll", this.ragdollInfo));
			SectoryMenu.InjectClassIntoMenu(new SectoryMenu.InjectEditClass("Stealth", this.stealthInfo));
			SectoryMenu.InjectClassIntoMenu(new SectoryMenu.InjectEditClass("Unconscious", this.unconsciousInfo));
			SectoryMenu.InjectClassIntoMenu(new SectoryMenu.InjectEditClass("AI", this.aiSettings));
			SectoryMenu.InjectClassIntoMenu(new SectoryMenu.InjectEditClass("Internals", this.internalsInfo));
			SectoryMenu.InjectClassIntoMenu(new SectoryMenu.InjectEditClass("InternalsHelper", new InternalHelper()));
			SectoryMenu.InjectClassIntoMenu(new SectoryMenu.InjectEditClass("SnapHelper", new SnapHelper()));
			GameManager.options.stuckBehaviour = 0;
			GameManager.options.Apply();
			this.generalInfo.ScriptDifficulty = this.generalInfo.Difficulty;
			foreach (CreatureData creatureData in from i in Catalog.GetAllID<CreatureData>()
				select Catalog.GetData<CreatureData>(i, true) into s
				where !s.name.Contains("Player")
				select s)
			{
				creatureData.health = (short)Mathf.Clamp((float)Entry.inst.creatureToHealth[creatureData] + this.generalInfo.CreatureAddHealth, -32767f, 32767f);
			}
			this.ConfigureDynamicLoot();
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002CB4 File Offset: 0x00000EB4
		private void ConfigureDynamicLoot()
		{
			bool dynamicLoot = this.lootInfo.dynamicLoot;
			if (dynamicLoot)
			{
				Entry.<>c__DisplayClass24_0 CS$<>8__locals1;
				CS$<>8__locals1.suitableData = (from id in Catalog.GetAllID<ItemData>()
					select Catalog.GetData<ItemData>(id, true)).ToList<ItemData>();
				CS$<>8__locals1.random = new Random(this.lootInfo.dynamicLootRandomPerSession ? DateTime.Now.Millisecond : this.lootInfo.dynamicLootSeed.GetHashCode());
				CS$<>8__locals1.dynamicallyAddedLoot = new HashSet<ItemData>();
				Dictionary<string, LootInfo.TableInfo> dictionary = new Dictionary<string, LootInfo.TableInfo>();
				foreach (LootInfo.TableInfo tableInfo in this.lootInfo.tableInfos)
				{
					dictionary.Add(tableInfo.lootTableID, tableInfo);
				}
				foreach (string text in dictionary.Keys)
				{
					LootTable data = Catalog.GetData<LootTable>(text, true);
					for (int j = 0; j < this.lootInfo.dynamicItemAdditionAmount; j++)
					{
						ItemData itemData = Entry.<ConfigureDynamicLoot>g__GetRandom|24_1(dictionary[text], ref CS$<>8__locals1);
						bool flag = itemData == null;
						if (flag)
						{
							break;
						}
						LootTable.Drop drop = new LootTable.Drop();
						drop.lootTable = data;
						drop.reference = 0;
						drop.referenceID = itemData.id;
						drop.itemData = itemData;
						drop.probabilityWeight = (float)this.lootInfo.dynamicLootChance;
						data.drops.Add(drop);
						Debug.Log("Added " + itemData.displayName + " to table " + data.id);
					}
				}
			}
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002EA0 File Offset: 0x000010A0
		private void PlayerSpawn(Creature player, EventTime time)
		{
			bool flag = this.generalInfo.playerEffected && !player.gameObject.GetComponent<Systems>();
			if (flag)
			{
				player.gameObject.AddComponent<Systems>();
			}
			GameManager.local.StartCoroutine(this.FallDamageFix());
			bool flag2 = this.hardcoreCoroutine == null && this.generalInfo.hardcoreMode;
			if (flag2)
			{
				this.hardcoreCoroutine = GameManager.local.StartCoroutine(this.Hardcore());
			}
			bool flag3 = this.debugCoroutine == null && this.generalInfo.debugMode;
			if (flag3)
			{
				this.debugCoroutine = GameManager.local.StartCoroutine(this.DebugMode());
			}
			player.data.health = (short)this.generalInfo.playerHealth;
			player.maxHealth = (float)player.data.health;
			player.currentHealth = this.generalInfo.playerHealth;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002F8C File Offset: 0x0000118C
		private IEnumerator FallDamageFix()
		{
			bool _ = Player.fallDamage;
			GameManager.SetPlayerFallDamage(false);
			yield return new WaitForSeconds(1f);
			bool flag = _;
			if (flag)
			{
				GameManager.SetPlayerFallDamage(true);
			}
			yield break;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002F9C File Offset: 0x0000119C
		private void Spawn(Creature creature)
		{
			bool flag = creature == Player.currentCreature;
			if (!flag)
			{
				Systems component = creature.gameObject.GetComponent<Systems>();
				bool flag2 = component != null;
				if (flag2)
				{
					component.Reset();
				}
				else
				{
					creature.gameObject.AddComponent<Systems>();
				}
				bool flag3 = creature.data.name.Contains("Human") && this.injuryInfo.necksnapTwist && !creature.gameObject.GetComponent<NeckSnapSystem>();
				if (flag3)
				{
					creature.gameObject.AddComponent<NeckSnapSystem>().Setup(this.injuryInfo.neckSnapTwistEase);
				}
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00003041 File Offset: 0x00001241
		private IEnumerator Hardcore()
		{
			for (;;)
			{
				bool flag = Player.currentCreature && Player.currentCreature.currentHealth > 0.25f;
				if (flag)
				{
					Player.currentCreature.currentHealth = 0.25f;
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00003050 File Offset: 0x00001250
		private IEnumerator DebugMode()
		{
			for (;;)
			{
				RaycastHit info;
				bool flag = Keyboard.current.dKey.wasPressedThisFrame && Physics.Raycast(Spectator.local.cam.ScreenPointToRay(Mouse.current.position.ReadValue()), ref info);
				if (flag)
				{
					Creature creature = info.collider.GetComponentInParent<Creature>();
					bool flag2 = creature != null && !creature.isKilled && !creature.isPlayer;
					if (flag2)
					{
						creature.ragdoll.SetState(1);
					}
					creature = null;
				}
				RaycastHit info2;
				bool flag3 = Mouse.current.middleButton.wasPressedThisFrame && Physics.Raycast(Spectator.local.cam.ScreenPointToRay(Mouse.current.position.ReadValue()), ref info2);
				if (flag3)
				{
					Player.local.transform.position = info2.point;
				}
				RaycastHit info3;
				bool flag4 = Keyboard.current.mKey.wasPressedThisFrame && Physics.Raycast(Spectator.local.cam.ScreenPointToRay(Mouse.current.position.ReadValue()), ref info3);
				if (flag4)
				{
					this.lastHitPoint = info3.point;
				}
				RaycastHit info4;
				bool flag5 = Keyboard.current.mKey.wasReleasedThisFrame && Physics.Raycast(Spectator.local.cam.ScreenPointToRay(Mouse.current.position.ReadValue()), ref info4);
				if (flag5)
				{
					Debug.Log((this.lastHitPoint - info4.point).magnitude);
				}
				RaycastHit info5;
				bool flag6 = Keyboard.current.kKey.wasPressedThisFrame && Physics.Raycast(Spectator.local.cam.ScreenPointToRay(Mouse.current.position.ReadValue()), ref info5);
				if (flag6)
				{
					Creature creature2 = info5.collider.GetComponentInParent<Creature>();
					bool flag7 = creature2 != null && !creature2.isKilled && !creature2.isPlayer;
					if (flag7)
					{
						creature2.Kill();
					}
					creature2 = null;
				}
				yield return null;
				info = default(RaycastHit);
				info2 = default(RaycastHit);
				info3 = default(RaycastHit);
				info4 = default(RaycastHit);
				info5 = default(RaycastHit);
			}
			yield break;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00003074 File Offset: 0x00001274
		[CompilerGenerated]
		internal static ItemData <ConfigureDynamicLoot>g__GetRandom|24_1(LootInfo.TableInfo info, ref Entry.<>c__DisplayClass24_0 A_1)
		{
			foreach (ItemData itemData in A_1.suitableData)
			{
				bool flag = itemData.type == 1;
				bool flag2 = !flag;
				if (!flag2)
				{
					bool flag3 = A_1.random.Next(0, 100) > 20;
					if (flag3)
					{
						flag = false;
					}
					bool flag4 = itemData.mass > info.massBelowThis;
					if (flag4)
					{
						flag = false;
					}
					bool flag5 = itemData.mass < info.massAboveThis;
					if (flag5)
					{
						flag = false;
					}
					bool flag6 = info.oneHanded && itemData.moduleAI != null && itemData.moduleAI.weaponHandling != 1;
					if (flag6)
					{
						flag = false;
					}
					bool flag7 = info.twoHanded && itemData.moduleAI != null && itemData.moduleAI.weaponHandling != 2;
					if (flag7)
					{
						flag = false;
					}
					bool flag8 = A_1.dynamicallyAddedLoot.Contains(itemData);
					if (flag8)
					{
						flag = false;
					}
					bool flag9 = info.mustBeAvailableInBook && !itemData.purchasable;
					if (flag9)
					{
						flag = false;
					}
					bool flag10 = info.nameMustContain.Length != 0 && !info.nameMustContain.Any(new Func<string, bool>(itemData.displayName.Contains));
					if (flag10)
					{
						flag = false;
					}
					bool flag11 = info.idMustContain.Length != 0 && !info.idMustContain.Any(new Func<string, bool>(itemData.id.Contains));
					if (flag11)
					{
						flag = false;
					}
					bool flag12 = info.nameCannotContain.Length != 0 && info.nameCannotContain.Any(new Func<string, bool>(itemData.displayName.Contains));
					if (flag12)
					{
						flag = false;
					}
					bool flag13 = info.idCannotContain.Length != 0 && info.idCannotContain.Any(new Func<string, bool>(itemData.id.Contains));
					if (flag13)
					{
						flag = false;
					}
					bool flag14 = A_1.dynamicallyAddedLoot.Contains(itemData);
					if (flag14)
					{
						flag = false;
					}
					bool flag15 = flag;
					if (flag15)
					{
						A_1.dynamicallyAddedLoot.Add(itemData);
						return itemData;
					}
				}
			}
			Debug.Log("Returning null because could not find a suited item.");
			return null;
		}

		// Token: 0x04000072 RID: 114
		public BleedInfo bleedInfo;

		// Token: 0x04000073 RID: 115
		public InjuryInfo injuryInfo;

		// Token: 0x04000074 RID: 116
		public RagdollInfo ragdollInfo;

		// Token: 0x04000075 RID: 117
		public EffectData bleedData;

		// Token: 0x04000076 RID: 118
		public GeneralInfo generalInfo;

		// Token: 0x04000077 RID: 119
		public StealthInfo stealthInfo;

		// Token: 0x04000078 RID: 120
		public LootInfo lootInfo;

		// Token: 0x04000079 RID: 121
		public UnconsciousInfo unconsciousInfo;

		// Token: 0x0400007A RID: 122
		public InternalsInfo internalsInfo;

		// Token: 0x0400007B RID: 123
		public TwistInjuriesInfo twistInfo;

		// Token: 0x0400007C RID: 124
		public AiSettings aiSettings;

		// Token: 0x0400007D RID: 125
		public Dictionary<CreatureData, short> creatureToHealth = new Dictionary<CreatureData, short>();

		// Token: 0x0400007E RID: 126
		public static Entry inst;

		// Token: 0x0400007F RID: 127
		private Coroutine hardcoreCoroutine;

		// Token: 0x04000080 RID: 128
		private Coroutine debugCoroutine;

		// Token: 0x04000081 RID: 129
		private Vector3 lastHitPoint;
	}
}
