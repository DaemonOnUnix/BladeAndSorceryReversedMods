using System;
using System.Reflection;
using ThunderRoad;
using UnityEngine;

namespace CustomAvatarFramework.Extensions
{
	// Token: 0x02000026 RID: 38
	public static class ItemExtension
	{
		// Token: 0x060000BC RID: 188 RVA: 0x00007684 File Offset: 0x00005884
		public static void Set<T>(this Item item, string fieldName, T val)
		{
			item.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic).SetValue(item, val);
		}

		// Token: 0x060000BD RID: 189 RVA: 0x000076A0 File Offset: 0x000058A0
		public static bool IsCustomAvatarItem(this Item item)
		{
			return item.data.HasModule<ItemModuleAvatar>();
		}

		// Token: 0x060000BE RID: 190 RVA: 0x000076AD File Offset: 0x000058AD
		public static void DisableCullingDetection(this Item item)
		{
			item.Set("cullingDetectionEnabled", false);
		}

		// Token: 0x060000BF RID: 191 RVA: 0x000076BC File Offset: 0x000058BC
		public static void IgnoreVanillaRagdollPartCollision(this Item item, RagdollPart ragdollPart, bool active)
		{
			foreach (ColliderGroup colliderGroup in item.colliderGroups)
			{
				foreach (Collider collider in colliderGroup.colliders)
				{
					foreach (Collider collider2 in ragdollPart.colliderGroup.colliders)
					{
						Physics.IgnoreCollision(collider2, collider, active);
					}
				}
			}
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00007790 File Offset: 0x00005990
		public static void IgnoreVanillaItemCollision(this Item item, Item otherItem, bool active)
		{
			foreach (ColliderGroup colliderGroup in item.colliderGroups)
			{
				foreach (Collider collider in colliderGroup.colliders)
				{
					foreach (ColliderGroup colliderGroup2 in otherItem.colliderGroups)
					{
						foreach (Collider collider2 in colliderGroup2.colliders)
						{
							Physics.IgnoreCollision(collider, collider2, active);
						}
					}
				}
			}
		}
	}
}
