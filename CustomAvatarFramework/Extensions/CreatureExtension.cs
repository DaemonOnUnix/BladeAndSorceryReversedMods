using System;
using System.Collections;
using System.Reflection;
using ThunderRoad;
using UnityEngine;

namespace CustomAvatarFramework.Extensions
{
	// Token: 0x02000025 RID: 37
	public static class CreatureExtension
	{
		// Token: 0x060000A5 RID: 165 RVA: 0x00006F04 File Offset: 0x00005104
		public static CustomAvatarCreature CustomAvatarCreature(this Creature creature)
		{
			return creature.gameObject.GetComponent<CustomAvatarCreature>();
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00006F11 File Offset: 0x00005111
		public static CustomAvatarCreatureV2 CustomAvatarCreatureV2(this Creature creature)
		{
			return creature.gameObject.GetComponent<CustomAvatarCreatureV2>();
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00006F1E File Offset: 0x0000511E
		public static void SetExtraRotationBone(this Creature creature, string boneName, float x, float y, float z)
		{
			creature.CustomAvatarCreatureV2().extraRotationBones[boneName] = Quaternion.Euler(x, y, z);
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00006F3A File Offset: 0x0000513A
		public static Quaternion GetExtraRotationBone(this Creature creature, string boneName)
		{
			return creature.CustomAvatarCreatureV2().extraRotationBones[boneName];
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00006F4D File Offset: 0x0000514D
		public static void SetExtraRotationBoneForEditor(this Creature creature, string boneName, float x, float y, float z)
		{
			creature.CustomAvatarCreatureV2().extraRotationBonesForEditor[boneName] = new Vector3(x, y, z);
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00006F69 File Offset: 0x00005169
		public static Vector3 GetExtraRotationBoneForEditor(this Creature creature, string boneName)
		{
			return creature.CustomAvatarCreatureV2().extraRotationBonesForEditor[boneName];
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00006F7C File Offset: 0x0000517C
		public static Vector3 GetExtraHipPosition(this Creature creature)
		{
			return creature.CustomAvatarCreatureV2().extraHipPosition;
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00006F89 File Offset: 0x00005189
		public static Vector3 GetExtraDimension(this Creature creature)
		{
			return creature.CustomAvatarCreatureV2().extraDimension;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00006F98 File Offset: 0x00005198
		public static void SetExtraHipPosition(this Creature creature, float value, string axis)
		{
			Vector3 extraHipPosition = creature.GetExtraHipPosition();
			if (axis != null)
			{
				if (axis == "x")
				{
					creature.CustomAvatarCreatureV2().extraHipPosition = new Vector2(value, extraHipPosition.y);
					return;
				}
				if (!(axis == "y"))
				{
					return;
				}
				creature.CustomAvatarCreatureV2().extraHipPosition = new Vector2(extraHipPosition.x, value);
			}
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00007008 File Offset: 0x00005208
		public static void SetExtraDimension(this Creature creature, float value, string axis)
		{
			Vector3 extraDimension = creature.GetExtraDimension();
			if (axis != null)
			{
				if (axis == "x")
				{
					creature.CustomAvatarCreatureV2().extraDimension = new Vector3(value, extraDimension.y, extraDimension.z);
					return;
				}
				if (axis == "y")
				{
					creature.CustomAvatarCreatureV2().extraDimension = new Vector3(extraDimension.x, value, extraDimension.z);
					return;
				}
				if (!(axis == "z"))
				{
					return;
				}
				creature.CustomAvatarCreatureV2().extraDimension = new Vector3(extraDimension.x, extraDimension.y, value);
			}
		}

		// Token: 0x060000AF RID: 175 RVA: 0x000070D4 File Offset: 0x000052D4
		public static void ResetGrip(this Creature creature, string boneName, bool autoSpawnNewItem = false)
		{
			RagdollHand ragdollHand = creature.GetHand((boneName == "LeftHand_Mesh") ? 1 : 0);
			if (ragdollHand.grabbedHandle != null)
			{
				Handle grabbedHandle = ragdollHand.grabbedHandle;
				ragdollHand.UnGrab(false);
				ragdollHand.Grab(grabbedHandle);
				return;
			}
			if (!autoSpawnNewItem)
			{
				return;
			}
			Transform transform = ragdollHand.transform;
			Catalog.GetData<ItemData>("DaggerCommon", true).SpawnAsync(delegate(Item spawnedDagger)
			{
				ragdollHand.Grab(spawnedDagger.GetMainHandle(ragdollHand.side));
			}, new Vector3?(transform.position), new Quaternion?(transform.rotation), null, true, null);
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00007188 File Offset: 0x00005388
		public static bool IsTrulyVisible(this Creature creature)
		{
			foreach (Creature.RendererData rendererData in creature.renderers)
			{
				if (rendererData.renderer.forceRenderingOff)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00007300 File Offset: 0x00005500
		public static IEnumerator EquipApparelAvatarV2(this Creature creature, ItemData apparelItemData)
		{
			if (creature.HasCustomAvatar())
			{
				yield return creature.UnEquipCustomAvatar();
			}
			ItemModuleAvatar itemModuleAvatar = apparelItemData.GetModule<ItemModuleAvatar>();
			if (creature.gameObject.GetComponent<CustomAvatarCreatureV2>() != null)
			{
				Object.Destroy(creature.gameObject.GetComponent<CustomAvatarCreatureV2>());
			}
			CustomAvatarCreatureV2 customAvatarCreature = creature.gameObject.AddComponent<CustomAvatarCreatureV2>();
			customAvatarCreature.Init(itemModuleAvatar);
			CustomAvatarEventManager.InvokeOnCustomAvatarEquipped(creature, apparelItemData);
			yield return null;
			yield break;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00007323 File Offset: 0x00005523
		public static bool HasCustomAvatar(this Creature creature)
		{
			return creature.CustomAvatarCreatureV2() != null;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x000073EC File Offset: 0x000055EC
		public static IEnumerator UnEquipCustomAvatar(this Creature creature)
		{
			try
			{
				Object.Destroy(creature.CustomAvatarCreature());
			}
			catch (Exception)
			{
			}
			try
			{
				Object.Destroy(creature.CustomAvatarCreatureV2());
				CustomAvatarEventManager.InvokeOnCustomAvatarUnEquipped(creature);
			}
			catch (Exception)
			{
			}
			yield return null;
			yield break;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00007408 File Offset: 0x00005608
		public static void ProcessSpawnedInventoryItemData(this Creature creature, ItemData itemData)
		{
			ItemModuleAvatarEquipment module = itemData.GetModule<ItemModuleAvatarEquipment>();
			ItemModuleAvatarReset module2 = itemData.GetModule<ItemModuleAvatarReset>();
			if (module != null)
			{
				try
				{
					ItemData data = Catalog.GetData<ItemData>(module.avatarApparelId, true);
					GameManager.local.StartCoroutine(creature.EquipApparelAvatarV2(data));
				}
				catch (Exception)
				{
				}
			}
			if (module2 != null)
			{
				try
				{
					GameManager.local.StartCoroutine(creature.UnEquipCustomAvatar());
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x0000749C File Offset: 0x0000569C
		public static void SummonSpawnedInventoryItemData(this Creature creature, ItemData itemData)
		{
			ItemModuleAvatarEquipment module = itemData.GetModule<ItemModuleAvatarEquipment>();
			if (module != null)
			{
				try
				{
					CreatureData data = Catalog.GetData<CreatureData>(module.avatarCreatureId, true);
					Transform transform = creature.transform;
					GameManager.local.StartCoroutine(data.SpawnCoroutine(transform.position - 0.5f * transform.forward, 0f, null, delegate(Creature spawnedCreature)
					{
						spawnedCreature.SetFaction(creature.factionId);
					}, true, null));
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x0000753C File Offset: 0x0000573C
		public static Vector3 GetLocalScale(this Creature creature)
		{
			return creature.transform.localScale;
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00007549 File Offset: 0x00005749
		public static float GetLocalScaleX(this Creature creature)
		{
			return creature.GetLocalScale().x;
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00007558 File Offset: 0x00005758
		public static Vector3 GetCustomLocalScale(this Creature creature, Vector3 extraDimension)
		{
			float localScaleX = creature.GetLocalScaleX();
			return new Vector3(extraDimension.x * localScaleX, extraDimension.y * localScaleX, extraDimension.z * localScaleX);
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x0000758C File Offset: 0x0000578C
		public static void HideBody(this Creature creature, bool hide)
		{
			foreach (Creature.RendererData rendererData in creature.renderers)
			{
				if (rendererData.renderer.materials.Length != 0 && rendererData.renderer)
				{
					rendererData.renderer.enabled = !hide;
				}
			}
			creature.hidden = hide;
		}

		// Token: 0x060000BA RID: 186 RVA: 0x0000760C File Offset: 0x0000580C
		public static void Set<T>(this Creature creature, string fieldName, T val)
		{
			creature.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic).SetValue(creature, val);
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00007628 File Offset: 0x00005828
		public static void ChangeVisibility(this Creature creature, bool isVisible)
		{
			foreach (Creature.RendererData rendererData in creature.renderers)
			{
				rendererData.renderer.forceRenderingOff = !isVisible;
			}
		}
	}
}
