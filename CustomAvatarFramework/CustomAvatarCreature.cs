using System;
using System.Collections.Generic;
using Chabuk.ManikinMono;
using CustomAvatarFramework.Extensions;
using ThunderRoad;
using UnityEngine;

namespace CustomAvatarFramework
{
	// Token: 0x02000003 RID: 3
	public class CustomAvatarCreature : MonoBehaviour
	{
		// Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
		private void Awake()
		{
			this._creature = base.GetComponent<Creature>();
			this._creature.OnDespawnEvent += new Creature.DespawnEvent(this.CreatureOnOnDespawnEvent);
		}

		// Token: 0x06000003 RID: 3 RVA: 0x0000207D File Offset: 0x0000027D
		private void CreatureOnOnDespawnEvent(EventTime eventtime)
		{
			if (this._avatar != null)
			{
				this._avatar.Hide(true);
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x0000209C File Offset: 0x0000029C
		private void FixedUpdate()
		{
			if (!this._initialized)
			{
				return;
			}
			foreach (Transform transform in this._creatureBones)
			{
				try
				{
					Transform transform2 = this._avatarBones[transform.name];
					if (!this.positionMatchingExemptionBoneNames.Contains(transform.name))
					{
						Vector3 up = this._creature.transform.up;
						transform2.position = transform.position + (this.extraPositionBones.ContainsKey(transform.name) ? new Vector3(up.x * this.extraPositionBones[transform.name].x, up.y * this.extraPositionBones[transform.name].y, up.z * this.extraPositionBones[transform.name].z) : Vector3.zero);
					}
					transform2.rotation = transform.rotation * this.extraRotationBones[transform.name];
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000023F0 File Offset: 0x000005F0
		public void Init(ItemModuleAvatar itemModuleAvatar)
		{
			this.extraPositionBones = itemModuleAvatar.extraPositionBones;
			this.extraRotationBones = new Dictionary<string, Quaternion>();
			foreach (KeyValuePair<string, Vector3> keyValuePair in itemModuleAvatar.bones)
			{
				this.extraRotationBones[keyValuePair.Key] = Quaternion.Euler(keyValuePair.Value.x, keyValuePair.Value.y, keyValuePair.Value.z);
			}
			this._creature.HideBody(true);
			this._creatureBones = new List<Transform>();
			this._avatarBones = new Dictionary<string, Transform>();
			foreach (RagdollPart ragdollPart in this._creature.ragdoll.parts)
			{
				ragdollPart.sliceAllowed = false;
			}
			this.positionMatchingExemptionBoneNames = itemModuleAvatar.positionMatchingExemptionBoneNames;
			Transform transform = this._creature.transform;
			Catalog.GetData<ItemData>(itemModuleAvatar.avatarItemId, true).SpawnAsync(delegate(Item rsItem)
			{
				rsItem.disallowDespawn = true;
				rsItem.disallowRoomDespawn = true;
				rsItem.rb.useGravity = false;
				rsItem.rb.isKinematic = true;
				rsItem.rb.detectCollisions = false;
				float num = this._creature.morphology.height * this._creature.transform.localScale.x;
				rsItem.transform.localScale = (this._creature.isPlayer ? itemModuleAvatar.playerScaleRatio : itemModuleAvatar.npcScaleRatio) * Mathf.Pow(num, 3f) / Mathf.Pow(itemModuleAvatar.avatarHeight, 3f) * Vector3.one;
				rsItem.transform.SetParent(this._creature.transform);
				this._avatar = rsItem;
				foreach (KeyValuePair<int, ManikinRig.BoneData> keyValuePair2 in this._creature.manikinParts.Rig.bones)
				{
					if (this.extraRotationBones.ContainsKey(keyValuePair2.Value.boneTransform.name))
					{
						this._creatureBones.Add(keyValuePair2.Value.boneTransform);
						this._avatarBones[keyValuePair2.Value.boneTransform.name] = this._avatar.GetCustomReference(keyValuePair2.Value.boneTransform.name, true);
					}
				}
				this._initialized = true;
			}, new Vector3?(transform.position), new Quaternion?(transform.rotation), null, true, null);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002574 File Offset: 0x00000774
		public void ChangeAvatarHeightRatio(float avatarHeight, float avatarRatio)
		{
			float num = this._creature.morphology.height * this._creature.transform.localScale.x;
			this._avatar.transform.localScale = avatarRatio * Mathf.Pow(num, 3f) / Mathf.Pow(avatarHeight, 3f) * Vector3.one;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000025DB File Offset: 0x000007DB
		public void AddPositionMatchingExemptionBoneNames(string boneName)
		{
			if (!this.positionMatchingExemptionBoneNames.Contains(boneName))
			{
				this.positionMatchingExemptionBoneNames.Add(boneName);
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000025F7 File Offset: 0x000007F7
		public void RemovePositionMatchingExemptionBoneNames(string boneName)
		{
			if (this.positionMatchingExemptionBoneNames.Contains(boneName))
			{
				this.positionMatchingExemptionBoneNames.Remove(boneName);
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002614 File Offset: 0x00000814
		private void OnDestroy()
		{
			this._creature.Hide(false);
			this._avatar.Despawn();
		}

		// Token: 0x04000001 RID: 1
		private Creature _creature;

		// Token: 0x04000002 RID: 2
		private Item _avatar;

		// Token: 0x04000003 RID: 3
		private bool _initialized;

		// Token: 0x04000004 RID: 4
		public Dictionary<string, Quaternion> extraRotationBones;

		// Token: 0x04000005 RID: 5
		public Dictionary<string, Vector3> extraPositionBones;

		// Token: 0x04000006 RID: 6
		public List<string> positionMatchingExemptionBoneNames;

		// Token: 0x04000007 RID: 7
		private List<Transform> _creatureBones;

		// Token: 0x04000008 RID: 8
		private Dictionary<string, Transform> _avatarBones;
	}
}
