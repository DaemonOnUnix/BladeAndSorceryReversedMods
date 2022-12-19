using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using CustomAvatarFramework.Extensions;
using HarmonyLib;
using RainyReignGames.RevealMask;
using ThunderRoad;
using UnityEngine;

namespace CustomAvatarFramework
{
	// Token: 0x02000009 RID: 9
	public class CustomAvatarLevelModule : LevelModule
	{
		// Token: 0x0600005F RID: 95 RVA: 0x000048AC File Offset: 0x00002AAC
		public override IEnumerator OnLoadCoroutine()
		{
			try
			{
				this._harmony = new Harmony("CustomAvatar");
				this._harmony.PatchAll(Assembly.GetExecutingAssembly());
				Debug.Log("Custom Avatar V2 Loaded");
			}
			catch (Exception ex)
			{
				Debug.Log(ex.Message);
			}
			return base.OnLoadCoroutine();
		}

		// Token: 0x04000024 RID: 36
		private Harmony _harmony;

		// Token: 0x0200000A RID: 10
		[HarmonyPatch("EquipWardrobe")]
		[HarmonyPatch(typeof(Equipment))]
		private static class EquipmentEquipWardrobePatch
		{
			// Token: 0x06000061 RID: 97 RVA: 0x00004AD4 File Offset: 0x00002CD4
			private static IEnumerator ChangeAvatar(Equipment __instance, ContainerData.Content content)
			{
				yield return new WaitForSeconds(3f);
				ItemData itemData = content.itemData;
				if (itemData != null)
				{
					ItemModuleAvatar itemModuleAvatar = itemData.GetModule<ItemModuleAvatar>();
					if (itemModuleAvatar != null)
					{
						Debug.Log("Found item Module Avatar for item: " + itemData.id);
						Creature creature = __instance.creature;
						if (itemModuleAvatar.version == 2)
						{
							Debug.Log("Item data is v2");
							yield return creature.EquipApparelAvatarV2(itemData);
						}
						else
						{
							Debug.Log("Item data is v1");
							if (creature.gameObject.GetComponent<CustomAvatarCreature>() != null)
							{
								Object.Destroy(creature.gameObject.GetComponent<CustomAvatarCreature>());
								yield return new WaitForSeconds(1f);
							}
							CustomAvatarCreature customAvatarCreature = creature.gameObject.AddComponent<CustomAvatarCreature>();
							customAvatarCreature.Init(itemModuleAvatar);
							yield return null;
						}
					}
				}
				yield break;
			}

			// Token: 0x06000062 RID: 98 RVA: 0x00004AF8 File Offset: 0x00002CF8
			[HarmonyPostfix]
			private static void Postfix(Equipment __instance, ContainerData.Content content)
			{
				try
				{
					GameManager.local.StartCoroutine(CustomAvatarLevelModule.EquipmentEquipWardrobePatch.ChangeAvatar(__instance, content));
				}
				catch (Exception ex)
				{
					Debug.Log(ex.Message);
				}
			}
		}

		// Token: 0x0200000B RID: 11
		[HarmonyPatch("UnequipWardrobe")]
		[HarmonyPatch(typeof(Equipment))]
		private static class EquipmentUnequipWardrobePatch
		{
			// Token: 0x06000063 RID: 99 RVA: 0x00004C20 File Offset: 0x00002E20
			private static IEnumerator RemoveAvatar(Equipment __instance, ContainerData.Content content)
			{
				ItemData itemData = content.itemData;
				if (itemData != null && !(__instance.creature.CustomAvatarCreatureV2() == null) && !(__instance.creature.CustomAvatarCreatureV2().avatar.data.id != itemData.id))
				{
					Object.Destroy(__instance.creature.CustomAvatarCreatureV2());
					yield return null;
				}
				yield break;
			}

			// Token: 0x06000064 RID: 100 RVA: 0x00004C44 File Offset: 0x00002E44
			[HarmonyPostfix]
			private static void Postfix(Equipment __instance, ContainerData.Content content)
			{
				try
				{
					GameManager.local.StartCoroutine(CustomAvatarLevelModule.EquipmentUnequipWardrobePatch.RemoveAvatar(__instance, content));
				}
				catch (Exception ex)
				{
					Debug.Log(ex.Message);
				}
			}
		}

		// Token: 0x0200000C RID: 12
		[HarmonyPatch("Play")]
		[HarmonyPatch(typeof(EffectReveal))]
		private static class EffectDecalPlayPatch
		{
			// Token: 0x06000065 RID: 101 RVA: 0x00004C84 File Offset: 0x00002E84
			[HarmonyPrefix]
			private static void Prefix(EffectReveal __instance)
			{
				if (__instance.applyOn == 1)
				{
					Transform transform = __instance.transform;
					Vector3 vector = -transform.forward;
					try
					{
						RaycastHit raycastHit;
						if (Physics.Raycast(new Ray(transform.position, vector), ref raycastHit, 1f, 201334785))
						{
							Creature componentInParent = raycastHit.rigidbody.GetComponentInParent<Creature>();
							CustomAvatarCreatureV2 component = componentInParent.gameObject.GetComponent<CustomAvatarCreatureV2>();
							Item avatar = component.avatar;
							List<RevealMaterialController> list = new List<RevealMaterialController>();
							foreach (RevealDecal revealDecal in avatar.revealDecals)
							{
								if (!(revealDecal == null))
								{
									list.Add(revealDecal.revealMaterialController);
								}
							}
							GameManager.local.StartCoroutine(RevealMaskProjection.ProjectAsync(transform.position + -vector * __instance.offsetDistance, vector, transform.up, __instance.depth, __instance.currentSize, __instance.maskTexture, __instance.maxChannelMultiplier, list, __instance.revealData, null));
						}
					}
					catch (Exception)
					{
					}
				}
			}
		}

		// Token: 0x0200000D RID: 13
		[HarmonyPatch("IsVisible")]
		[HarmonyPatch(typeof(Creature))]
		private static class CreatureIsVisiblePatch
		{
			// Token: 0x06000066 RID: 102 RVA: 0x00004DC4 File Offset: 0x00002FC4
			[HarmonyPostfix]
			private static void Postfix(Creature __instance, ref bool __result)
			{
				if ((__instance.CustomAvatarCreatureV2() != null || __instance.CustomAvatarCreature() != null) && Time.time - __instance.lastInteractionTime <= (float)Catalog.gameData.cleanDeadLastInteractionDelay)
				{
					__result = true;
				}
			}
		}

		// Token: 0x0200000E RID: 14
		[HarmonyPatch("GrabSelectedItem")]
		[HarmonyPatch(typeof(UIItemSpawner))]
		private static class InventoryGrabSelectedItemPatch
		{
			// Token: 0x06000067 RID: 103 RVA: 0x00004E00 File Offset: 0x00003000
			[HarmonyPostfix]
			private static void Postfix(UIItemSpawner __instance)
			{
				ItemData itemData = __instance.SelectedItem.itemData;
				Player.local.creature.ProcessSpawnedInventoryItemData(itemData);
			}
		}

		// Token: 0x0200000F RID: 15
		[HarmonyPatch("SpawnSelectedItem")]
		[HarmonyPatch(typeof(UIItemSpawner))]
		private static class InventorySpawnSelectedItemPatch
		{
			// Token: 0x06000068 RID: 104 RVA: 0x00004E2C File Offset: 0x0000302C
			[HarmonyPostfix]
			private static void Postfix(UIItemSpawner __instance)
			{
				ItemData itemData = __instance.SelectedItem.itemData;
				Player.local.creature.SummonSpawnedInventoryItemData(itemData);
			}
		}

		// Token: 0x02000010 RID: 16
		[HarmonyPatch("SetCull")]
		[HarmonyPatch(typeof(Creature))]
		private static class CreatureSetCullPatch
		{
			// Token: 0x06000069 RID: 105 RVA: 0x00004E55 File Offset: 0x00003055
			[HarmonyPrefix]
			private static void Prefix(Creature __instance, bool cull)
			{
				if (!cull && __instance.isCulled)
				{
					CustomAvatarLevelModule.CreatureSetCullPatch._isStopCullingCreature = true;
				}
			}

			// Token: 0x0600006A RID: 106 RVA: 0x00004E68 File Offset: 0x00003068
			[HarmonyPostfix]
			private static void Postfix(Creature __instance, bool cull)
			{
				if (!CustomAvatarLevelModule.CreatureSetCullPatch._isStopCullingCreature)
				{
					return;
				}
				__instance.equipment.EquipAllWardrobes(false, true);
			}

			// Token: 0x04000025 RID: 37
			private static bool _isStopCullingCreature;
		}
	}
}
