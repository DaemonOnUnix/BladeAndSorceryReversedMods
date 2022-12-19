using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ThunderRoad;
using UnityEngine;

namespace LuckyBlocksLite
{
	// Token: 0x02000003 RID: 3
	public class LuckyBlocksMethods : MonoBehaviour
	{
		// Token: 0x06000003 RID: 3 RVA: 0x00002094 File Offset: 0x00000294
		public static async void LootBlock(Item item)
		{
			bool flag = !LuckyBlocksMethods.<LootBlock>g__IsPlayerTkHolding|0_0(item);
			if (flag)
			{
				LuckyBlocksMethods.<>c__DisplayClass0_0 CS$<>8__locals1 = new LuckyBlocksMethods.<>c__DisplayClass0_0();
				Transform parent = item.transform;
				Vector3 lootPos = item.transform.position;
				GameObject go = new GameObject();
				go.transform.position = parent.position;
				EffectInstance BreakSound = Catalog.GetData<EffectData>("LuckyBlockBreak", true).Spawn(go.transform, true, null, false, Array.Empty<Type>());
				BreakSound.Play(0, false);
				GOMono.DestroyGO(go);
				await Task.Delay(50);
				item.Despawn();
				Random random = new Random();
				CS$<>8__locals1.faction = Random.Range(2, 4);
				int lootType = random.Next(1, 5);
				if (lootType == 1)
				{
					List<string> catalog = Catalog.GetAllID(5);
					string ItemID = catalog[Random.Range(0, catalog.Count)];
					Catalog.GetData<ItemData>(ItemID, true).SpawnAsync(null, new Vector3?(lootPos), null, null, true, null);
					catalog = null;
					ItemID = null;
				}
				if (lootType == 2)
				{
					string creatureId = ((Random.Range(1, 101) <= GameManager.options.maleRatio) ? "HumanMale" : "HumanFemale");
					CreatureData creatureData = Catalog.GetData<CreatureData>(creatureId, true);
					float rotation = Player.local.transform.rotation.eulerAngles.y;
					Vector3 position = lootPos;
					GameManager.local.StartCoroutine(creatureData.SpawnCoroutine(position, rotation, null, delegate(Creature rsCreature)
					{
						rsCreature.SetFaction(CS$<>8__locals1.faction);
						rsCreature.brain.canDamage = true;
					}, true, null));
					creatureId = null;
					creatureData = null;
					position = default(Vector3);
				}
				if (lootType == 3)
				{
					int effect = Random.Range(1, 3);
					int target = Random.Range(1, 3);
					if (effect == 1)
					{
						if (target == 1)
						{
							LuckyBlocksMethods.DisplayMessage("You have received speed boost for 30 seconds!", lootPos);
							LuckyBlocksMethods.ApplyEffect("speed boost player", Player.currentCreature);
						}
						else
						{
							LuckyBlocksMethods.DisplayMessage("All NPCs received speed boost for 30 seconds!", lootPos);
							foreach (Creature creature in Creature.all)
							{
								if (creature != Player.currentCreature)
								{
									LuckyBlocksMethods.ApplyEffect("speed boost NPC", creature);
								}
								creature = null;
							}
							List<Creature>.Enumerator enumerator = default(List<Creature>.Enumerator);
						}
					}
					if (effect == 2)
					{
						if (target == 1)
						{
							LuckyBlocksMethods.DisplayMessage("You have been healed 100 HP!", lootPos);
							LuckyBlocksMethods.ApplyEffect("heal", Player.currentCreature);
						}
						else
						{
							LuckyBlocksMethods.DisplayMessage("All NPCs have been healed 100 HP!", lootPos);
							foreach (Creature creature2 in Creature.all)
							{
								if (creature2 != Player.currentCreature)
								{
									LuckyBlocksMethods.ApplyEffect("heal", creature2);
								}
								creature2 = null;
							}
							List<Creature>.Enumerator enumerator2 = default(List<Creature>.Enumerator);
						}
					}
				}
				if (lootType == 4)
				{
					int idk = Random.Range(1, 6);
					if (idk == 1)
					{
						LuckyBlocksMethods.DisplayMessage("All NPCs switched factions!", lootPos);
						foreach (Creature creature3 in Creature.all)
						{
							if (creature3 != Player.currentCreature)
							{
								if (creature3.factionId == 2)
								{
									creature3.SetFaction(3);
									creature3.brain.canDamage = true;
								}
								else
								{
									creature3.SetFaction(2);
									creature3.brain.canDamage = true;
								}
							}
							creature3 = null;
						}
						List<Creature>.Enumerator enumerator3 = default(List<Creature>.Enumerator);
					}
					if (idk == 2)
					{
						LuckyBlocksMethods.DisplayMessage("whoops", lootPos);
						Catalog.GetData<ItemData>("DaggerCommon", true).SpawnAsync(delegate(Item spawnedItem)
						{
							spawnedItem.rb.AddForce((Player.currentCreature.ragdoll.headPart.transform.position - spawnedItem.transform.position) * 12f, 1);
							spawnedItem.Throw(1f, 2);
						}, new Vector3?(lootPos), null, null, true, null);
					}
					if (idk == 3)
					{
						LuckyBlocksMethods.DisplayMessage("All the fallen came back from the dead!", lootPos);
						foreach (Creature creature4 in Creature.all)
						{
							if (creature4.isKilled)
							{
								creature4.Resurrect(50f, creature4);
								creature4.brain.Load(creature4.brain.instance.id);
								creature4.brain.canDamage = true;
							}
							creature4 = null;
						}
						List<Creature>.Enumerator enumerator4 = default(List<Creature>.Enumerator);
					}
					if (idk == 4)
					{
						LuckyBlocksMethods.DisplayMessage("Look up.", lootPos);
						Catalog.GetData<ItemData>("Anvil", true).SpawnAsync(delegate(Item Item)
						{
							Debug.Log(Item.name);
							Item.rb.AddForce(Item.transform.position - new Vector3(0f, -1f, 0f) * 3f, 1);
						}, new Vector3?(Player.currentCreature.transform.position + new Vector3(0f, 5f, 0f)), null, null, true, null);
					}
					if (idk == 5)
					{
						GameObject GO = new GameObject();
						GO.transform.position = Player.currentCreature.transform.position + new Vector3(0f, 100f, 0f);
						Player.local.Teleport(GO.transform, true, true);
						GOMono.DestroyGO(GO);
						GO = null;
					}
				}
				CS$<>8__locals1 = null;
				parent = null;
				lootPos = default(Vector3);
				go = null;
				BreakSound = null;
				random = null;
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020D0 File Offset: 0x000002D0
		public static async void DisplayMessage(string message, Vector3 position)
		{
			GameObject textGO = new GameObject();
			TextMesh textMesh = textGO.AddComponent<TextMesh>();
			textMesh.text = message;
			textMesh.fontSize = 25;
			textMesh.alignment = 1;
			textMesh.color = Color.yellow;
			textMesh.characterSize = 0.03f;
			textGO.transform.position = position;
			textGO.transform.LookAt(Player.currentCreature.transform);
			textGO.transform.Rotate(20f, 180f, 0f);
			await Task.Delay(4000);
			Object.Destroy(textMesh);
			Object.Destroy(textGO);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002110 File Offset: 0x00000310
		public static async void ApplyEffect(string effect, Creature target)
		{
			bool flag = effect == "speed boost NPC";
			if (flag)
			{
				float defaultSpeed = target.locomotion.forwardSpeed;
				target.locomotion.forwardSpeed = defaultSpeed * 2f;
				await Task.Delay(30000);
				target.locomotion.forwardSpeed = defaultSpeed;
			}
			if (effect == "speed boost player")
			{
				float defaultForwardSpeed = Player.local.locomotion.forwardSpeed;
				float defaultStrafeSpeed = Player.local.locomotion.strafeSpeed;
				float defaultRunSpeed = Player.local.locomotion.runSpeedAdd;
				float defaultBackSpeed = Player.local.locomotion.backwardSpeed;
				Player.local.locomotion.forwardSpeed = defaultForwardSpeed * 2f;
				Player.local.locomotion.strafeSpeed = defaultStrafeSpeed * 2f;
				Player.local.locomotion.runSpeedAdd = defaultRunSpeed * 2f;
				Player.local.locomotion.backwardSpeed = defaultBackSpeed * 2f;
				await Task.Delay(30000);
				Player.local.locomotion.forwardSpeed = defaultForwardSpeed;
				Player.local.locomotion.strafeSpeed = defaultStrafeSpeed;
				Player.local.locomotion.runSpeedAdd = defaultRunSpeed;
				Player.local.locomotion.backwardSpeed = defaultBackSpeed;
			}
			if (effect == "heal")
			{
				target.Heal(100f, target);
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x0000215C File Offset: 0x0000035C
		[CompilerGenerated]
		internal static bool <LootBlock>g__IsPlayerTkHolding|0_0(Item itemTK)
		{
			bool flag = itemTK != null;
			if (flag)
			{
				foreach (SpellCaster spellCaster in itemTK.tkHandlers)
				{
					bool isPlayer = spellCaster.ragdollHand.creature.isPlayer;
					if (isPlayer)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
