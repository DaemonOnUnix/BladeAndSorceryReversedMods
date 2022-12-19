using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using ThunderRoad;
using UnityEngine;
using UnityEngine.Events;

namespace SpellCastIce
{
	// Token: 0x02000003 RID: 3
	public static class IceManager
	{
		// Token: 0x0600000A RID: 10 RVA: 0x000020A8 File Offset: 0x000002A8
		public static void LoadFromJSON()
		{
			List<IceManager.Ability> list = new List<IceManager.Ability>();
			list.Add(new IceManager.Ability(5, "AimButton", "Ice Aim", "Your aim has never been better, ice spikes will now more easily hit the target", IceManager.AbilitiesEnum.iceSpikeAim, null));
			list.Add(new IceManager.Ability(2, "GrabButton", "Grabable Ice", "Ever just wanted to pick up that ice spike you just launched? Well now you can! Unlocks the ability to wield and manipulate ice spikes.", IceManager.AbilitiesEnum.pickUpIceSpikes, null));
			list.Add(new IceManager.Ability(3, "NoGravButton", "Zero Gravity Ice", "Makes the ice spikes fly without falling, simply no gravity on spikes", IceManager.AbilitiesEnum.noGravity, null));
			list.Add(new IceManager.Ability(1, "ImbueButton", "Frost Imbuement", "Allows you to imbue any weapon with frost, enemies hit will be slowed by the power of the imbuement", IceManager.AbilitiesEnum.IceImbue, delegate()
			{
				Catalog.GetData<SpellCastCharge>("IceSpell", true).imbueEnabled = true;
			}));
			list.Add(new IceManager.Ability(3, "MergeIceButton", "Ice Blast", "Merge ice to shoot out a bunch of spikes around you", IceManager.AbilitiesEnum.IceMergeIce, delegate()
			{
				Debug.Log("MergeIce");
				bool flag = Player.local;
				if (flag)
				{
					IceManager.playerCreature.container.AddContent(Catalog.GetData<ItemData>("SpellIceMergeItem", true), null, null);
					IceManager.playerCreature.mana.AddSpell(Catalog.GetData<SpellData>("IceMergeIce", true), Catalog.GetData<SpellData>("IceMergeIce", true).level);
				}
			}));
			list.Add(new IceManager.Ability(5, "MergeFireButton", "FireIce Beam", "Merge ice and fire to shoot out a burning cold beam of ice", IceManager.AbilitiesEnum.IceMergeFire, delegate()
			{
				Debug.Log("MergeFire");
				bool flag = Player.local;
				if (flag)
				{
					IceManager.playerCreature.container.AddContent(Catalog.GetData<ItemData>("SpellIceFireMergeItem", true), null, null);
					IceManager.playerCreature.mana.AddSpell(Catalog.GetData<SpellData>("IceFireMerge", true), Catalog.GetData<SpellData>("IceFireMerge", true).level);
				}
			}));
			list.Add(new IceManager.Ability(5, "MergeGravButton", "Ice stasis dome", "Merge ice and gravity to create a freezing sphere around stopping anyone near you", IceManager.AbilitiesEnum.IceMergeGrav, delegate()
			{
				Debug.Log("MergeGrav");
				bool flag = Player.local;
				if (flag)
				{
					IceManager.playerCreature.container.AddContent(Catalog.GetData<ItemData>("SpellIceMergeGravItem", true), null, null);
					IceManager.playerCreature.mana.AddSpell(Catalog.GetData<SpellData>("IceGravMerge", true), Catalog.GetData<SpellData>("IceGravMerge", true).level);
				}
			}));
			list.Add(new IceManager.Ability(5, "MergeLightningButton", "Charged Ice Shurikens", "Merge to spray out a ton of electric ice shuricens", IceManager.AbilitiesEnum.IceMergeLightning, delegate()
			{
				Debug.Log("MergeLightning");
				bool flag = Player.local;
				if (flag)
				{
					IceManager.playerCreature.container.AddContent(Catalog.GetData<ItemData>("IceLightningMergeItem", true), null, null);
					IceManager.playerCreature.mana.AddSpell(Catalog.GetData<SpellData>("IceLightningMerge", true), Catalog.GetData<SpellData>("IceLightningMerge", true).level);
				}
			}));
			IceManager.abilities = list;
			EventManager.onCreatureHit += new EventManager.CreatureHitEvent(IceManager.EventManager_onCreatureHit);
			EventManager.onCreatureKill += new EventManager.CreatureKillEvent(IceManager.EventManager_onCreatureKill);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x0000226C File Offset: 0x0000046C
		public static void LoadFromSave(Creature creature)
		{
			IceManager.playerCreature = creature;
			bool flag = File.Exists(Path.Combine(Application.streamingAssetsPath, "Mods/IceSpell/Saves/IceStatSave.json"));
			if (flag)
			{
				string text = File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "Mods/IceSpell/Saves/IceStatSave.json"));
				IceStatsJSON iceStatsJSON = JsonConvert.DeserializeObject<IceStatsJSON>(text);
				IceManager.level = iceStatsJSON.level;
				IceManager.levelPoints = iceStatsJSON.levelPoints;
				IceManager.xp = iceStatsJSON.xp;
				foreach (IceManager.Ability ability in IceManager.abilities)
				{
					bool flag2 = iceStatsJSON.unlocked.Contains(ability.abilityEnum);
					if (flag2)
					{
						ability.Unlock();
					}
				}
				IceManager.chargeSpeed = (float)(1.0 / Math.Pow(1.100000023841858, (double)IceManager.level));
				IceManager.spikeSpeed = (float)(5.0 + Math.Sqrt((double)IceManager.level) / 4.0);
			}
			else
			{
				IceManager.SaveToJSON();
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002394 File Offset: 0x00000594
		public static void EventManager_onCreatureKill(Creature creature, Player player, CollisionInstance collisionStruct, EventTime eventTime)
		{
			int num = 0;
			ColliderGroup sourceColliderGroup = collisionStruct.sourceColliderGroup;
			string text;
			if (sourceColliderGroup == null)
			{
				text = null;
			}
			else
			{
				CollisionHandler collisionHandler = sourceColliderGroup.collisionHandler;
				if (collisionHandler == null)
				{
					text = null;
				}
				else
				{
					Item item = collisionHandler.item;
					text = ((item != null) ? item.itemId : null);
				}
			}
			bool flag = text == "IceSpike";
			if (flag)
			{
				num += 10;
				bool flag2 = collisionStruct.damageStruct.hitRagdollPart.type == 1;
				if (flag2)
				{
					num += 5;
				}
				num *= 1 + (int)(Vector3.Distance(creature.transform.position, Player.local.transform.position) / 5f);
				IceManager.GainXP(num);
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002434 File Offset: 0x00000634
		public static void EventManager_onCreatureHit(Creature creature, CollisionInstance collisionStruct)
		{
			int num = 0;
			ColliderGroup sourceColliderGroup = collisionStruct.sourceColliderGroup;
			string text;
			if (sourceColliderGroup == null)
			{
				text = null;
			}
			else
			{
				CollisionHandler collisionHandler = sourceColliderGroup.collisionHandler;
				if (collisionHandler == null)
				{
					text = null;
				}
				else
				{
					Item item = collisionHandler.item;
					text = ((item != null) ? item.itemId : null);
				}
			}
			bool flag = text == "IceSpike";
			if (flag)
			{
				num += 3;
				bool flag2 = collisionStruct.damageStruct.hitRagdollPart.type == 1;
				if (flag2)
				{
					num += 5;
				}
				bool flag3 = creature != Player.currentCreature && !creature.isKilled;
				if (flag3)
				{
					bool flag4 = creature.ragdoll.state != 2;
					if (flag4)
					{
						bool flag5 = !creature.GetComponent<IceSpellMWE>();
						if (flag5)
						{
							creature.gameObject.AddComponent<IceSpellMWE>();
						}
						IceSpellMWE component = creature.GetComponent<IceSpellMWE>();
						component.SlowStartCoroutine(creature, 100f, 0f, 0f, 8f);
					}
				}
				num *= 1 + (int)(Vector3.Distance(creature.transform.position, Player.local.transform.position) / 5f);
				IceManager.GainXP(num);
			}
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002554 File Offset: 0x00000754
		public static void GainXP(int _xp)
		{
			IceManager.xp += (float)_xp;
			bool flag = IceManager.xp >= IceManager.XpForNextLevel(IceManager.level);
			if (flag)
			{
				float num = IceManager.xp - IceManager.XpForNextLevel(IceManager.level);
				IceManager.level++;
				IceManager.levelPoints++;
				IceManager.xp = num;
				IceManager.chargeSpeed = (float)(1.0 / Math.Pow(1.100000023841858, (double)IceManager.level));
				IceManager.spikeSpeed = (float)(5.0 + Math.Sqrt((double)IceManager.level) / 4.0);
			}
			IceManager.SaveToJSON();
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002608 File Offset: 0x00000808
		public static void SaveToJSON()
		{
			IceStatsJSON iceStatsJSON = new IceStatsJSON
			{
				level = IceManager.level,
				levelPoints = IceManager.levelPoints,
				xp = IceManager.xp
			};
			foreach (KeyValuePair<IceManager.AbilitiesEnum, IceManager.Ability> keyValuePair in IceManager.abilityDict)
			{
				bool unlocked = keyValuePair.Value.unlocked;
				if (unlocked)
				{
					iceStatsJSON.unlocked.Add(keyValuePair.Key);
				}
			}
			string text = JsonConvert.SerializeObject(iceStatsJSON, 1);
			File.WriteAllText(Path.Combine(Application.streamingAssetsPath, "Mods/IceSpell/Saves/IceStatSave.json"), text);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000026C8 File Offset: 0x000008C8
		public static float XpForNextLevel(int lvl)
		{
			return (float)(5.0 * Math.Pow((double)(lvl + 1), 2.0) + 50.0);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002704 File Offset: 0x00000904
		public static bool IsAbilityUnlocked(IceManager.AbilitiesEnum ab)
		{
			IceManager.Ability ability;
			IceManager.abilityDict.TryGetValue(ab, out ability);
			return ability.unlocked;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002738 File Offset: 0x00000938
		public static bool UnlockAbility(IceManager.AbilitiesEnum ability)
		{
			IceManager.Ability ability2;
			IceManager.abilityDict.TryGetValue(ability, out ability2);
			bool flag = !ability2.unlocked;
			if (flag)
			{
				bool flag2 = IceManager.levelPoints >= ability2.levelPointCost;
				if (flag2)
				{
					IceManager.levelPoints -= ability2.levelPointCost;
					ability2.Unlock();
					IceManager.SaveToJSON();
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000005 RID: 5
		public static int level = 0;

		// Token: 0x04000006 RID: 6
		public static int levelPoints = 0;

		// Token: 0x04000007 RID: 7
		public static float xp = 0f;

		// Token: 0x04000008 RID: 8
		public static Creature playerCreature;

		// Token: 0x04000009 RID: 9
		public static float chargeSpeed = 1f;

		// Token: 0x0400000A RID: 10
		public static float spikeSpeed = 5f;

		// Token: 0x0400000B RID: 11
		public static readonly Dictionary<IceManager.AbilitiesEnum, IceManager.Ability> abilityDict = new Dictionary<IceManager.AbilitiesEnum, IceManager.Ability>();

		// Token: 0x0400000C RID: 12
		public static List<IceManager.Ability> abilities = new List<IceManager.Ability>();

		// Token: 0x0200000F RID: 15
		public enum AbilitiesEnum
		{
			// Token: 0x04000039 RID: 57
			noGravity,
			// Token: 0x0400003A RID: 58
			pickUpIceSpikes,
			// Token: 0x0400003B RID: 59
			iceSpikeAim,
			// Token: 0x0400003C RID: 60
			IceMergeIce,
			// Token: 0x0400003D RID: 61
			IceMergeFire,
			// Token: 0x0400003E RID: 62
			IceMergeGrav,
			// Token: 0x0400003F RID: 63
			IceMergeLightning,
			// Token: 0x04000040 RID: 64
			IceImbue
		}

		// Token: 0x02000010 RID: 16
		public class Ability
		{
			// Token: 0x06000048 RID: 72 RVA: 0x00003E05 File Offset: 0x00002005
			public void Unlock()
			{
				this.unlocked = true;
				this.onUnlockEvent.Invoke();
			}

			// Token: 0x06000049 RID: 73 RVA: 0x00003E1C File Offset: 0x0000201C
			public Ability(int cost, string customRefName, string uiTitle, string uiDescript, IceManager.AbilitiesEnum abilityEnum, UnityAction onUnlockAction = null)
			{
				Debug.Log(IceManager.abilityDict.Count);
				this.levelPointCost = cost;
				this.unlocked = false;
				this.customRefName = customRefName;
				this.uiTitle = uiTitle;
				this.uiDescript = uiDescript;
				this.abilityEnum = abilityEnum;
				bool flag = onUnlockAction != null;
				if (flag)
				{
					this.onUnlockEvent.AddListener(onUnlockAction);
				}
				try
				{
					IceManager.abilityDict.Add(abilityEnum, this);
				}
				catch
				{
				}
			}

			// Token: 0x04000041 RID: 65
			public UnityEvent onUnlockEvent = new UnityEvent();

			// Token: 0x04000042 RID: 66
			public int levelPointCost;

			// Token: 0x04000043 RID: 67
			public bool unlocked;

			// Token: 0x04000044 RID: 68
			public string customRefName;

			// Token: 0x04000045 RID: 69
			public string uiTitle;

			// Token: 0x04000046 RID: 70
			public string uiDescript;

			// Token: 0x04000047 RID: 71
			public IceManager.AbilitiesEnum abilityEnum;
		}

		// Token: 0x02000011 RID: 17
		public static class Abilities
		{
		}
	}
}
