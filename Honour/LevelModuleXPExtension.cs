using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using ThunderRoad;
using UnityEngine;

namespace Honour
{
	// Token: 0x02000004 RID: 4
	public static class LevelModuleXPExtension
	{
		// Token: 0x06000005 RID: 5 RVA: 0x00002180 File Offset: 0x00000380
		public static bool IsDoneByPlayer(CollisionInstance collisionInstance)
		{
			bool flag = collisionInstance.sourceColliderGroup;
			if (flag)
			{
				Item item = collisionInstance.sourceColliderGroup.collisionHandler.item;
				Object @object;
				if (item == null)
				{
					@object = null;
				}
				else
				{
					RagdollHand lastHandler = item.lastHandler;
					@object = ((lastHandler != null) ? lastHandler.creature.player : null);
				}
				bool flag2;
				if (!@object)
				{
					RagdollPart ragdollPart = collisionInstance.sourceColliderGroup.collisionHandler.ragdollPart;
					Object object2;
					if (ragdollPart == null)
					{
						object2 = null;
					}
					else
					{
						Creature lastInteractionCreature = ragdollPart.ragdoll.creature.lastInteractionCreature;
						object2 = ((lastInteractionCreature != null) ? lastInteractionCreature.player : null);
					}
					flag2 = object2;
				}
				else
				{
					flag2 = true;
				}
				bool flag3 = flag2;
				if (flag3)
				{
					return true;
				}
			}
			else
			{
				SpellCaster casterHand = collisionInstance.casterHand;
				bool flag4;
				if (!((casterHand != null) ? casterHand.mana.creature.player : null))
				{
					ColliderGroup targetColliderGroup = collisionInstance.targetColliderGroup;
					Object object3;
					if (targetColliderGroup == null)
					{
						object3 = null;
					}
					else
					{
						RagdollPart ragdollPart2 = targetColliderGroup.collisionHandler.ragdollPart;
						object3 = ((ragdollPart2 != null) ? ragdollPart2.ragdoll.creature.lastInteractionCreature.player : null);
					}
					flag4 = object3;
				}
				else
				{
					flag4 = true;
				}
				bool flag5 = flag4;
				if (flag5)
				{
					return true;
				}
				RagdollPart hitRagdollPart = collisionInstance.damageStruct.hitRagdollPart;
				Object object4;
				if (hitRagdollPart == null)
				{
					object4 = null;
				}
				else
				{
					Creature lastInteractionCreature2 = hitRagdollPart.ragdoll.creature.lastInteractionCreature;
					object4 = ((lastInteractionCreature2 != null) ? lastInteractionCreature2.player : null);
				}
				bool flag6 = object4;
				if (flag6)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000022C8 File Offset: 0x000004C8
		public static LevelModuleXP.Action GetAction(Creature creature, CollisionInstance collisionInstance, out int bonusMalus, out int honor)
		{
			FieldInfo field = typeof(Ragdoll).GetField("sliceRunning", BindingFlags.Instance | BindingFlags.NonPublic);
			bool flag = collisionInstance.damageStruct.hitRagdollPart && (collisionInstance.damageStruct.hitRagdollPart.isSliced || (collisionInstance.damageStruct.hitRagdollPart.sliceChildAndDisableSelf && collisionInstance.damageStruct.hitRagdollPart.sliceChildAndDisableSelf.isSliced) || (bool)field.GetValue(collisionInstance.damageStruct.hitRagdollPart.ragdoll));
			LevelModuleXP.Action action;
			if (flag)
			{
				bool flag2 = !collisionInstance.sourceColliderGroup;
				if (flag2)
				{
					bonusMalus = Catalog.gameData.experience.actions.magicDismemberment;
					honor = Catalog.gameData.honor.actions.magicDismemberment;
					action = 3;
				}
				else
				{
					bool flag3 = collisionInstance.damageStruct.hitRagdollPart.type == 1 || collisionInstance.damageStruct.hitRagdollPart.type == 2 || (collisionInstance.damageStruct.hitRagdollPart.sliceChildAndDisableSelf && (collisionInstance.damageStruct.hitRagdollPart.sliceChildAndDisableSelf.type == 1 || collisionInstance.damageStruct.hitRagdollPart.sliceChildAndDisableSelf.type == 2));
					if (flag3)
					{
						bool flag4 = collisionInstance.sourceColliderGroup.collisionHandler.item && collisionInstance.sourceColliderGroup.collisionHandler.item.isThrowed;
						if (flag4)
						{
							bonusMalus = Catalog.gameData.experience.actions.throwDecapitation;
							honor = Catalog.gameData.honor.actions.throwDecapitation;
							action = 19;
						}
						else
						{
							bonusMalus = Catalog.gameData.experience.actions.decapitation;
							honor = Catalog.gameData.honor.actions.decapitation;
							action = 1;
						}
					}
					else
					{
						bool flag5 = collisionInstance.sourceColliderGroup.collisionHandler.item && collisionInstance.sourceColliderGroup.collisionHandler.item.isThrowed;
						if (flag5)
						{
							bonusMalus = Catalog.gameData.experience.actions.throwDismemberment;
							honor = Catalog.gameData.honor.actions.throwDismemberment;
							action = 20;
						}
						else
						{
							bonusMalus = Catalog.gameData.experience.actions.dismemberment;
							honor = Catalog.gameData.honor.actions.dismemberment;
							action = 2;
						}
					}
				}
			}
			else
			{
				bool flag6 = !collisionInstance.sourceColliderGroup;
				if (flag6)
				{
					bool flag7 = collisionInstance.sourceMaterial != null;
					if (flag7)
					{
						bool flag8 = collisionInstance.sourceMaterial.xpReference == "Drain";
						if (flag8)
						{
							bonusMalus = Catalog.gameData.experience.actions.drained;
							honor = Catalog.gameData.honor.actions.drained;
							return 18;
						}
						bool flag9 = collisionInstance.sourceMaterial.xpReference == "Frost";
						if (flag9)
						{
							bonusMalus = Catalog.gameData.experience.actions.frozen;
							honor = Catalog.gameData.honor.actions.frozen;
							return 16;
						}
					}
					bool flag10 = collisionInstance.damageStruct.damageType == 4 && creature.brain.isElectrocuted;
					if (flag10)
					{
						bonusMalus = Catalog.gameData.experience.actions.electrocuted;
						honor = Catalog.gameData.honor.actions.electrocuted;
						action = 14;
					}
					else
					{
						bool flag11 = collisionInstance.damageStruct.damageType == 2;
						if (flag11)
						{
							bonusMalus = Catalog.gameData.experience.actions.staticSlash;
							honor = Catalog.gameData.honor.actions.staticSlash;
							action = 12;
						}
						else
						{
							bool flag12 = collisionInstance.damageStruct.damageType == 1;
							if (flag12)
							{
								bonusMalus = Catalog.gameData.experience.actions.staticPierce;
								honor = Catalog.gameData.honor.actions.staticPierce;
								action = 13;
							}
							else
							{
								bool flag13 = collisionInstance.damageStruct.damageType == 3;
								if (flag13)
								{
									bonusMalus = Catalog.gameData.experience.actions.staticBlunt;
									honor = Catalog.gameData.honor.actions.staticBlunt;
									action = 11;
								}
								else
								{
									bonusMalus = 0;
									honor = 0;
									action = 0;
								}
							}
						}
					}
				}
				else
				{
					bool isRagdollPart = collisionInstance.sourceColliderGroup.collisionHandler.isRagdollPart;
					if (isRagdollPart)
					{
						bonusMalus = Catalog.gameData.experience.actions.ragdollHit;
						honor = Catalog.gameData.honor.actions.ragdollHit;
						action = 10;
					}
					else
					{
						bool flag14 = collisionInstance.sourceColliderGroup.collisionHandler.item && collisionInstance.sourceColliderGroup.collisionHandler.item.data.type == 5;
						if (flag14)
						{
							bonusMalus = Catalog.gameData.experience.actions.punchOrKick;
							honor = Catalog.gameData.honor.actions.punchOrKick;
							action = 9;
						}
						else
						{
							bool flag15 = collisionInstance.damageStruct.penetration == 1 && collisionInstance.damageStruct.hitRagdollPart.type == 2;
							if (flag15)
							{
								bonusMalus = Catalog.gameData.experience.actions.cutThroat;
								honor = Catalog.gameData.honor.actions.cutThroat;
								action = 4;
							}
							else
							{
								bool flag16 = collisionInstance.damageStruct.damageType == 2 && (collisionInstance.damageStruct.hitRagdollPart.type == 1 || collisionInstance.damageStruct.hitRagdollPart.type == 2);
								if (flag16)
								{
									bool flag17 = collisionInstance.sourceColliderGroup.collisionHandler.item && collisionInstance.sourceColliderGroup.collisionHandler.item.isThrowed;
									if (flag17)
									{
										bonusMalus = Catalog.gameData.experience.actions.throwSlashHeadOrNeck;
										honor = Catalog.gameData.honor.actions.throwSlashHeadOrNeck;
										action = 21;
									}
									else
									{
										bonusMalus = Catalog.gameData.experience.actions.slashHeadOrNeck;
										honor = Catalog.gameData.honor.actions.slashHeadOrNeck;
										action = 5;
									}
								}
								else
								{
									bool flag18 = collisionInstance.damageStruct.damageType == 1;
									if (flag18)
									{
										bool flag19 = collisionInstance.sourceColliderGroup.collisionHandler.item && collisionInstance.sourceColliderGroup.collisionHandler.item.isThrowed;
										if (flag19)
										{
											bool flag20 = collisionInstance.damageStruct.hitRagdollPart.type == 1 || collisionInstance.damageStruct.hitRagdollPart.type == 2;
											if (flag20)
											{
												bonusMalus = Catalog.gameData.experience.actions.throwPierceHeadOrNeck;
												honor = Catalog.gameData.honor.actions.throwPierceHeadOrNeck;
												return 22;
											}
											bonusMalus = Catalog.gameData.experience.actions.throwPierce;
											honor = Catalog.gameData.honor.actions.throwPierce;
											return 23;
										}
										else
										{
											bool flag21 = collisionInstance.damageStruct.hitRagdollPart.type == 1 || collisionInstance.damageStruct.hitRagdollPart.type == 2;
											if (flag21)
											{
												bonusMalus = Catalog.gameData.experience.actions.pierceHeadOrNeck;
												honor = Catalog.gameData.honor.actions.pierceHeadOrNeck;
												return 6;
											}
											bool flag22 = collisionInstance.damageStruct.hitRagdollPart.type == 4;
											if (flag22)
											{
												bonusMalus = Catalog.gameData.experience.actions.pierceTorso;
												honor = Catalog.gameData.honor.actions.pierceTorso;
												return 7;
											}
										}
									}
									bool flag23 = collisionInstance.damageStruct.damageType == 3;
									if (flag23)
									{
										bool flag24 = collisionInstance.sourceColliderGroup.collisionHandler.item && collisionInstance.sourceColliderGroup.collisionHandler.item.isThrowed;
										if (flag24)
										{
											bonusMalus = Catalog.gameData.experience.actions.throwBlunt;
											honor = Catalog.gameData.honor.actions.throwBlunt;
											return 24;
										}
										bool flag25 = collisionInstance.damageStruct.hitRagdollPart.type == 1;
										if (flag25)
										{
											bonusMalus = Catalog.gameData.experience.actions.bluntHead;
											honor = Catalog.gameData.honor.actions.bluntHead;
											return 8;
										}
									}
									bool flag26;
									if (collisionInstance.damageStruct.damageType == 4)
									{
										Item item = collisionInstance.sourceColliderGroup.collisionHandler.item;
										flag26 = ((item != null) ? item.itemId : null) == "DynamicProjectile";
									}
									else
									{
										flag26 = false;
									}
									bool flag27 = flag26;
									if (flag27)
									{
										bonusMalus = Catalog.gameData.experience.actions.burned;
										honor = Catalog.gameData.honor.actions.burned;
										action = 15;
									}
									else
									{
										bonusMalus = 0;
										honor = 0;
										action = 0;
									}
								}
							}
						}
					}
				}
			}
			return action;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002C50 File Offset: 0x00000E50
		public static LevelModuleXP.Modifier GetModifier(Creature creature, CollisionInstance collisionInstance, out int bonusMalus, out int honor)
		{
			BrainModuleDetection brainModuleDetection = creature.brain.instance.GetModule<BrainModuleDetection>(true);
			bool flag = creature.brain.currentTarget == null || !creature.brain.currentTarget.isPlayer;
			LevelModuleXP.Modifier modifier;
			if (flag)
			{
				bonusMalus = 0;
				honor = 0;
				modifier = 10;
			}
			else
			{
				bool flag2 = !creature.equipment.GetHeldWeapon(1) && !creature.equipment.GetHeldWeapon(0);
				if (flag2)
				{
					bonusMalus = Catalog.gameData.experience.modifiers.unarmed;
					honor = Catalog.gameData.honor.modifiers.unarmed;
					modifier = 5;
				}
				else
				{
					bool flag3 = creature.ragdoll.isTkGrabbed && creature.ragdoll.state == 1;
					if (flag3)
					{
						bonusMalus = Catalog.gameData.experience.modifiers.onGround;
						honor = Catalog.gameData.honor.modifiers.onGround;
						modifier = 9;
					}
					else
					{
						bool flag4 = creature.ragdoll.standingUp || (creature.ragdoll.state == 1 && creature.fallState == 3);
						if (flag4)
						{
							bonusMalus = Catalog.gameData.experience.modifiers.onGround;
							honor = Catalog.gameData.honor.modifiers.onGround;
							modifier = 4;
						}
						else
						{
							bool isElectrocuted = creature.brain.isElectrocuted;
							if (isElectrocuted)
							{
								bonusMalus = Catalog.gameData.experience.modifiers.electrocuted;
								honor = Catalog.gameData.honor.modifiers.electrocuted;
								modifier = 3;
							}
							else
							{
								bool isGrabbed = creature.ragdoll.isGrabbed;
								if (isGrabbed)
								{
									bonusMalus = Catalog.gameData.experience.modifiers.grabbed;
									honor = Catalog.gameData.honor.modifiers.grabbed;
									modifier = 1;
								}
								else
								{
									bool flag5 = creature.ragdoll.state == 1;
									if (flag5)
									{
										bonusMalus = Catalog.gameData.experience.modifiers.flying;
										honor = Catalog.gameData.honor.modifiers.flying;
										modifier = 2;
									}
									else
									{
										honor = 0;
										bonusMalus = 0;
										modifier = 0;
									}
								}
							}
						}
					}
				}
			}
			return modifier;
		}

		// Token: 0x02000006 RID: 6
		[HarmonyPatch("OnCreatureKill")]
		[HarmonyPatch(typeof(LevelModuleXP))]
		private static class LevelModuleXPOnCreatureKillPatch
		{
			// Token: 0x06000009 RID: 9 RVA: 0x00002F2C File Offset: 0x0000112C
			[HarmonyPrefix]
			private static bool Prefix(LevelModuleXP __instance, Creature creature, Player player, CollisionInstance collisionInstance, EventTime eventTime)
			{
				bool flag = eventTime != null || player || !LevelModuleXPExtension.IsDoneByPlayer(collisionInstance);
				bool flag2;
				if (flag)
				{
					flag2 = false;
				}
				else
				{
					int bonusMalus;
					int honor;
					int bonusMalus2;
					int honor2;
					LevelModuleXP.HistoryEvent historyEvent = new LevelModuleXP.HistoryEvent
					{
						time = Time.time,
						action = LevelModuleXPExtension.GetAction(creature, collisionInstance, out bonusMalus, out honor),
						modifier = LevelModuleXPExtension.GetModifier(creature, collisionInstance, out bonusMalus2, out honor2),
						honor = honor + honor2,
						creatureName = creature.data.name,
						baseXP = creature.data.baseXP,
						aiXP = (int)((double)creature.data.baseXP * (((creature.brain.instance != null) ? ((double)creature.brain.instance.difficulty) : 1.0) * ((double)Catalog.gameData.experience.aiDifficulty / 100.0)))
					};
					historyEvent.creatureXP = creature.data.baseXP + historyEvent.aiXP;
					bool flag3 = creature.equipment;
					if (flag3)
					{
						historyEvent.apparelsXP = (int)((double)historyEvent.creatureXP * ((double)creature.equipment.totalTierApparels * ((double)Catalog.gameData.experience.apparelTier / 100.0)));
					}
					float totalTierWeapons = 0f;
					float totalTierShields = 0f;
					foreach (Holder holder in creature.holders)
					{
						foreach (Item obj in holder.items)
						{
							bool flag4 = obj.data.type == 1;
							if (flag4)
							{
								totalTierWeapons += (float)obj.data.tier;
							}
							bool flag5 = obj.data.type == 6;
							if (flag5)
							{
								totalTierShields += (float)obj.data.tier;
							}
						}
					}
					historyEvent.weaponsXP = (int)((double)historyEvent.creatureXP * ((double)totalTierWeapons * ((double)Catalog.gameData.experience.weaponTier / 100.0)));
					historyEvent.shieldsXP = (int)((double)historyEvent.creatureXP * ((double)totalTierShields * ((double)Catalog.gameData.experience.shieldTier / 100.0)));
					historyEvent.creatureTotalXP = historyEvent.creatureXP + historyEvent.apparelsXP + historyEvent.weaponsXP + historyEvent.shieldsXP;
					historyEvent.actionModifierXP = (int)((double)historyEvent.creatureTotalXP * ((double)bonusMalus / 100.0 + (double)bonusMalus2 / 100.0));
					historyEvent.totalXP = historyEvent.creatureTotalXP + historyEvent.actionModifierXP;
					historyEvent.history = new List<LevelModuleXP.HistoryEvent.History>();
					for (int index = __instance.history.Count - 1; index >= 0; index--)
					{
						float time = historyEvent.time - __instance.history[index].time;
						bool flag6 = time > Catalog.gameData.experience.historyMaxTime;
						if (flag6)
						{
							bool flag7 = __instance.history.Count - index > Catalog.gameData.experience.historyMaxCount;
							if (flag7)
							{
								__instance.history.RemoveAt(index);
							}
						}
						else
						{
							bool flag8 = time < Catalog.gameData.experience.historyMinTime;
							if (flag8)
							{
								int XP = (int)((double)historyEvent.totalXP * ((double)Catalog.gameData.experience.historyMultiKill / 100.0));
								historyEvent.history.Add(new LevelModuleXP.HistoryEvent.History(time, 2, XP));
								historyEvent.totalXP += XP;
							}
							else
							{
								bool flag9 = __instance.history[index].action == historyEvent.action;
								if (flag9)
								{
									float num = Mathf.InverseLerp(Catalog.gameData.experience.historyMaxTime, Catalog.gameData.experience.historyMinTime, time);
									bool flag10 = __instance.history[index].modifier == historyEvent.modifier;
									if (flag10)
									{
										int XP2 = (int)((double)num * ((double)historyEvent.totalXP * ((double)Catalog.gameData.experience.historyMaxSameActionAndModifier / 100.0)));
										historyEvent.history.Add(new LevelModuleXP.HistoryEvent.History(time, 1, XP2));
										historyEvent.totalXP += XP2;
									}
									else
									{
										int XP3 = (int)((double)num * ((double)historyEvent.totalXP * ((double)Catalog.gameData.experience.historyMaxSameAction / 100.0)));
										historyEvent.history.Add(new LevelModuleXP.HistoryEvent.History(time, 0, XP3));
										historyEvent.totalXP += XP3;
									}
								}
							}
						}
					}
					__instance.history.Add(historyEvent);
					__instance.currentKill++;
					__instance.currentXP += historyEvent.totalXP;
					__instance.currentHonor += historyEvent.honor;
					bool flag11 = MenuModuleScores.current != null;
					if (flag11)
					{
						MenuModuleScores.current.RefreshPage();
					}
					flag2 = false;
				}
				return flag2;
			}
		}
	}
}
