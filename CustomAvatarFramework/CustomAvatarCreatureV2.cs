using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Chabuk.ManikinMono;
using CustomAvatarFramework.Editor.Items;
using CustomAvatarFramework.Extensions;
using RainyReignGames.MeshDismemberment;
using ThunderRoad;
using UnityEngine;

namespace CustomAvatarFramework
{
	// Token: 0x02000008 RID: 8
	public class CustomAvatarCreatureV2 : MonoBehaviour
	{
		// Token: 0x06000046 RID: 70 RVA: 0x0000377C File Offset: 0x0000197C
		private void Awake()
		{
			this._creature = base.GetComponent<Creature>();
			this._creature.ragdoll.OnSliceEvent += new Ragdoll.SliceEvent(this.RagdollOnOnSliceEvent);
			this._creature.OnDespawnEvent += new Creature.DespawnEvent(this.CreatureOnOnDespawnEvent);
		}

		// Token: 0x06000047 RID: 71 RVA: 0x000037C8 File Offset: 0x000019C8
		private void RagdollOnOnSliceEvent(RagdollPart ragdollpart, EventTime eventtime)
		{
			try
			{
				if (eventtime == 1)
				{
					this._severedRagdollParts.Enqueue(ragdollpart);
					base.StartCoroutine(this._customAvatarDismemberment.DoRip(this._avatarBones[this._ragdollPartNameAvatarBoneNameMapper[ragdollpart.name]], null, ragdollpart.sliceThreshold, ragdollpart.sliceFillMaterial, 1, null, null));
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000048 RID: 72 RVA: 0x0000383C File Offset: 0x00001A3C
		private Transform GetParentBone(Transform transformWithParent)
		{
			if (transformWithParent.parent == null)
			{
				return null;
			}
			if (transformWithParent.parent.parent != null)
			{
				return this.GetParentBone(transformWithParent.parent);
			}
			return transformWithParent.parent;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00003874 File Offset: 0x00001A74
		private void CreatureOnOnDespawnEvent(EventTime eventtime)
		{
			if (eventtime != 1)
			{
				return;
			}
			foreach (Transform transform in this._severedAvatarParts)
			{
				try
				{
					transform.SetParent(null);
					Object.Destroy(transform.gameObject);
				}
				catch (Exception)
				{
				}
			}
			Object.Destroy(this);
		}

		// Token: 0x0600004A RID: 74 RVA: 0x000038F0 File Offset: 0x00001AF0
		private void Update()
		{
			if (!this._initialized)
			{
				return;
			}
			foreach (Transform transform in this._creatureBones)
			{
				try
				{
					if (this._avatarBones.ContainsKey(transform.name))
					{
						Transform transform2 = this._avatarBones[transform.name];
						if (!(transform2 == null))
						{
							if (transform.name == "Hips_Mesh")
							{
								transform2.position = transform.position + transform.right * (this.extraHipPosition.y * this._creature.GetLocalScaleX()) + transform.forward * (this.extraHipPosition.x * this._creature.GetLocalScaleX());
							}
							if (this.enforceHandPosition && this._armBoneNames.Contains(transform.name))
							{
								transform2.position = transform.position;
							}
							transform2.rotation = transform.rotation * this.extraRotationBones[transform.name];
						}
					}
				}
				catch (Exception)
				{
				}
			}
			this.avatar.transform.localScale = this.avatarHeight * this._creature.GetCustomLocalScale(this.extraDimension);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00003E10 File Offset: 0x00002010
		public void Init(ItemModuleAvatar itemModuleAvatar)
		{
			Transform transform = this._creature.transform;
			Catalog.GetData<ItemData>(itemModuleAvatar.avatarItemId, true).SpawnAsync(delegate(Item rsItem)
			{
				rsItem.disallowDespawn = true;
				rsItem.disallowRoomDespawn = true;
				rsItem.rb.useGravity = false;
				rsItem.rb.isKinematic = true;
				rsItem.rb.detectCollisions = false;
				this.avatarHeight = itemModuleAvatar.avatarHeight;
				this.avatar = rsItem;
				if (this._creature.isPlayer)
				{
					this.avatar.DisableCullingDetection();
				}
				this._customAvatar = this.avatar.GetComponent<CustomAvatar>();
				if (this._customAvatar == null)
				{
					Debug.Log("No custom avatar found");
					return;
				}
				this._creature.ragdoll.physicTogglePlayerRadius *= 100f;
				this._creature.ragdoll.physicToggleRagdollRadius *= 100f;
				this.extraRotationBonesForEditor = itemModuleAvatar.bones;
				foreach (KeyValuePair<string, Vector3> keyValuePair in itemModuleAvatar.bones)
				{
					this.extraRotationBones[keyValuePair.Key] = Quaternion.Euler(keyValuePair.Value.x, keyValuePair.Value.y, keyValuePair.Value.z);
				}
				foreach (KeyValuePair<int, ManikinRig.BoneData> keyValuePair2 in this._creature.manikinParts.Rig.bones)
				{
					if (this.extraRotationBones.ContainsKey(keyValuePair2.Value.boneTransform.name))
					{
						this._creatureBones.Add(keyValuePair2.Value.boneTransform);
						this._avatarBones[keyValuePair2.Value.boneTransform.name] = this._customAvatar.GetType().GetField(keyValuePair2.Value.boneTransform.name).GetValue(this._customAvatar) as Transform;
					}
				}
				this.GenerateArmBoneNames();
				this.TryProcessingOptionalParts();
				this.TryProcessingWardrobeApparels(itemModuleAvatar);
				if (!itemModuleAvatar.disableDynamicBones)
				{
					List<DynamicBoneCollider> list = this.TryProcessingDynamicBoneColliders();
					this.TryProcessingDynamicBones(list);
				}
				this.enforceHandPosition = itemModuleAvatar.enforceHandPosition || this._creature.isPlayer;
				this.ProcessDismemberment();
				this.extraHipPosition = itemModuleAvatar.extraHipPosition;
				this.extraDimension = itemModuleAvatar.extraDimension;
				this.TryProcessingPlayerArmor(itemModuleAvatar.isGodMode);
				this.TryHidingHeadParts();
				this._creature.ChangeVisibility(false);
				this._initialized = true;
			}, new Vector3?(transform.position), new Quaternion?(transform.rotation), null, true, null);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00003E78 File Offset: 0x00002078
		private void TryProcessingPlayerArmor(bool isGodMode)
		{
			if (!this._creature.isPlayer)
			{
				return;
			}
			this.avatar.SetColliderLayer(GameManager.GetLayer(7));
			this.avatar.IgnoreRagdollCollision(this._creature.ragdoll);
			this.avatar.rb.detectCollisions = true;
			this.avatar.rb.isKinematic = false;
			this.DisableHandleCollision(this._creature.handLeft.grabbedHandle);
			this.DisableHandleCollision(this._creature.handRight.grabbedHandle);
			this._creature.handLeft.OnGrabEvent += new RagdollHand.GrabEvent(this.HandOnGrabEvent);
			this._creature.handRight.OnGrabEvent += new RagdollHand.GrabEvent(this.HandOnGrabEvent);
			if (!isGodMode)
			{
				return;
			}
			this._creature.handLeft.collisionHandler.OnCollisionStartEvent += new CollisionHandler.CollisionEvent(this.GodHandOnCollisionStartEvent);
			this._creature.handRight.collisionHandler.OnCollisionStartEvent += new CollisionHandler.CollisionEvent(this.GodHandOnCollisionStartEvent);
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00003F86 File Offset: 0x00002186
		private void HandOnGrabEvent(Side side, Handle handle, float axisposition, HandlePose orientation, EventTime eventtime)
		{
			if (eventtime != 1)
			{
				return;
			}
			this.DisableHandleCollision(handle);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00003F98 File Offset: 0x00002198
		private void GodHandOnCollisionStartEvent(CollisionInstance collisionInstance)
		{
			int num = 2;
			if (collisionInstance.impactVelocity.magnitude <= (float)num)
			{
				return;
			}
			RagdollPart component = collisionInstance.targetCollider.attachedRigidbody.GetComponent<RagdollPart>();
			if (component == null)
			{
				return;
			}
			if (component.ragdoll.creature.isPlayer || component.isSliced)
			{
				return;
			}
			component.ragdoll.creature.Damage(collisionInstance);
			component.TrySlice();
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00004005 File Offset: 0x00002205
		private void HandOnUnGrabEvent(Side side, Handle handle, bool throwing, EventTime eventtime)
		{
			if (eventtime != 1)
			{
				return;
			}
			base.StartCoroutine(this.EnableHandleCollisionCoroutine(handle));
		}

		// Token: 0x06000050 RID: 80 RVA: 0x0000401C File Offset: 0x0000221C
		private void FixedUpdate()
		{
			if (!this._creature.isPlayer)
			{
				return;
			}
			this.avatar.rb.detectCollisions = !this._creature.handLeft.climb.isGripping && !this._creature.handRight.climb.isGripping;
		}

		// Token: 0x06000051 RID: 81 RVA: 0x0000407C File Offset: 0x0000227C
		private void DisableHandleCollision(Handle handle)
		{
			if (handle == null)
			{
				return;
			}
			if (handle is HandleRagdoll)
			{
				HandleRagdoll handleRagdoll = handle as HandleRagdoll;
				this.avatar.IgnoreVanillaRagdollPartCollision(handleRagdoll.ragdollPart, true);
				return;
			}
			this.avatar.IgnoreVanillaItemCollision(handle.item, true);
		}

		// Token: 0x06000052 RID: 82 RVA: 0x000041A8 File Offset: 0x000023A8
		private IEnumerator EnableHandleCollisionCoroutine(Handle handle)
		{
			if (!(handle == null))
			{
				yield return new WaitForSeconds(0.5f);
				if (handle is HandleRagdoll)
				{
					HandleRagdoll handleRagdoll = handle as HandleRagdoll;
					this.avatar.IgnoreVanillaRagdollPartCollision(handleRagdoll.ragdollPart, false);
				}
				else
				{
					this.avatar.IgnoreVanillaItemCollision(handle.item, false);
				}
			}
			yield break;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x000041CC File Offset: 0x000023CC
		private void MainCollisionHandlerOnOnCollisionStartEvent(CollisionInstance collisioninstance)
		{
			RagdollPart component = collisioninstance.targetCollider.attachedRigidbody.GetComponent<RagdollPart>();
			if (component == null)
			{
				return;
			}
			EffectInstance effectInstance = Catalog.GetData<EffectData>("Effect_Hit" + collisioninstance.sourceCollider.material.name + "On" + collisioninstance.targetCollider.material.name, true).Spawn(collisioninstance.contactPoint, Quaternion.identity, null, null, true, null, false, new Type[0]);
			effectInstance.SetIntensity(1f);
			effectInstance.Play(0, false);
			component.ragdoll.creature.Damage(collisioninstance);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x000042A0 File Offset: 0x000024A0
		private void ProcessDismemberment()
		{
			GameObject gameObject = this._customAvatar.animator.gameObject;
			this._customAvatarDismemberment = new Dismemberment(gameObject);
			this._customAvatarDismemberment.Completed += this.CustomAvatarDismembermentOnCompleted;
			foreach (RagdollPart ragdollPart in this._creature.ragdoll.parts)
			{
				if (this._disallowSlicedRagdollPartNames.Contains(ragdollPart.name))
				{
					ragdollPart.sliceAllowed = false;
				}
			}
			BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
			Dismemberment dismemberment = this._creature.ragdoll.GetType().GetField("dismemberment", bindingFlags).GetValue(this._creature.ragdoll) as Dismemberment;
			dismemberment.Completed += delegate(object sender, Dismemberment.CompletedEventArgs args)
			{
				foreach (SkinnedMeshRenderer skinnedMeshRenderer in args.splitGameObject.GetComponentsInChildren<SkinnedMeshRenderer>())
				{
					skinnedMeshRenderer.enabled = false;
				}
			};
		}

		// Token: 0x06000055 RID: 85 RVA: 0x0000439C File Offset: 0x0000259C
		private void TryProcessingWardrobeApparels(ItemModuleAvatar itemModuleAvatar)
		{
			if (!itemModuleAvatar.hasWardrobe)
			{
				return;
			}
			WardrobeApparel wardrobeApparel = WardrobeApparel.randomizeWardrobe(itemModuleAvatar.wardrobeApparels);
			if (wardrobeApparel == null)
			{
				return;
			}
			foreach (CustomAvatarApparel customAvatarApparel in this.avatar.GetComponentsInChildren<CustomAvatarApparel>())
			{
				if (!wardrobeApparel.apparels.Contains(customAvatarApparel.itemName))
				{
					Object.Destroy(customAvatarApparel.gameObject);
				}
			}
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00004400 File Offset: 0x00002600
		private void TryProcessingOptionalParts()
		{
			CustomAvatarOptionalPart[] componentsInChildren = this.avatar.GetComponentsInChildren<CustomAvatarOptionalPart>();
			foreach (CustomAvatarOptionalPart customAvatarOptionalPart in componentsInChildren)
			{
				if (!(customAvatarOptionalPart.parentPart != null))
				{
					customAvatarOptionalPart.gameObject.SetActive((float)Random.Range(0, 100) <= customAvatarOptionalPart.percentage);
				}
			}
			foreach (CustomAvatarOptionalPart customAvatarOptionalPart2 in componentsInChildren)
			{
				if (!(customAvatarOptionalPart2.parentPart == null))
				{
					customAvatarOptionalPart2.gameObject.SetActive(customAvatarOptionalPart2.parentPart.gameObject.activeInHierarchy);
				}
			}
		}

		// Token: 0x06000057 RID: 87 RVA: 0x000044A4 File Offset: 0x000026A4
		private void TryProcessingDynamicBones(List<DynamicBoneCollider> dynamicBoneColliders)
		{
			CustomAvatarDynamicBone[] componentsInChildren = this.avatar.GetComponentsInChildren<CustomAvatarDynamicBone>();
			foreach (CustomAvatarDynamicBone customAvatarDynamicBone in componentsInChildren)
			{
				DynamicBone dynamicBone = this._creature.gameObject.AddComponent<DynamicBone>();
				dynamicBone.m_Root = customAvatarDynamicBone.transform;
				if (customAvatarDynamicBone.useGravity)
				{
					dynamicBone.m_UpdateMode = DynamicBone.UpdateMode.AnimatePhysics;
					dynamicBone.m_Gravity = new Vector3(0f, -9.81f, 0f);
				}
				dynamicBone.m_Colliders = new List<DynamicBoneColliderBase>();
				dynamicBone.m_Colliders.AddRange(dynamicBoneColliders);
				if (customAvatarDynamicBone is CustomAvatarDynamicBoneBreast)
				{
					dynamicBone.m_Damping = 0f;
				}
			}
		}

		// Token: 0x06000058 RID: 88 RVA: 0x0000454C File Offset: 0x0000274C
		private List<DynamicBoneCollider> TryProcessingDynamicBoneColliders()
		{
			CustomAvatarDynamicBoneCollider[] componentsInChildren = this.avatar.GetComponentsInChildren<CustomAvatarDynamicBoneCollider>();
			List<DynamicBoneCollider> list = new List<DynamicBoneCollider>();
			foreach (CustomAvatarDynamicBoneCollider customAvatarDynamicBoneCollider in componentsInChildren)
			{
				DynamicBoneCollider dynamicBoneCollider = customAvatarDynamicBoneCollider.gameObject.AddComponent<DynamicBoneCollider>();
				list.Add(dynamicBoneCollider);
			}
			return list;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x0000459C File Offset: 0x0000279C
		private void TryHidingHeadParts()
		{
			if (!this._creature.isPlayer)
			{
				return;
			}
			foreach (CustomAvatarHead customAvatarHead in this.avatar.GetComponentsInChildren<CustomAvatarHead>())
			{
				customAvatarHead.gameObject.layer = GameManager.GetLayer(11);
			}
		}

		// Token: 0x0600005A RID: 90 RVA: 0x000045E8 File Offset: 0x000027E8
		private void GenerateArmBoneNames()
		{
			foreach (KeyValuePair<string, HumanBodyBones?> keyValuePair in CustomAvatar.boneMappers)
			{
				if (keyValuePair.Key.Contains("Arm") || keyValuePair.Key.Contains("Hand"))
				{
					this._armBoneNames.Add(keyValuePair.Key);
				}
			}
		}

		// Token: 0x0600005B RID: 91 RVA: 0x0000466C File Offset: 0x0000286C
		private void CustomAvatarDismembermentOnCompleted(object sender, Dismemberment.CompletedEventArgs dismembermentArgs)
		{
			Transform splitBone = dismembermentArgs.splitBone;
			Transform parentBone = this.GetParentBone(splitBone);
			RagdollPart ragdollPart = this._severedRagdollParts.Dequeue();
			Transform transform = ragdollPart.transform;
			parentBone.transform.position += transform.position - splitBone.position;
			parentBone.SetParent(transform);
			this._severedAvatarParts.Add(parentBone);
		}

		// Token: 0x0600005C RID: 92 RVA: 0x000046D5 File Offset: 0x000028D5
		private void OnDestroy()
		{
			base.StopAllCoroutines();
			this._creature.ChangeVisibility(true);
			this.avatar.Despawn();
		}

		// Token: 0x04000011 RID: 17
		private Creature _creature;

		// Token: 0x04000012 RID: 18
		public Item avatar;

		// Token: 0x04000013 RID: 19
		private bool _initialized;

		// Token: 0x04000014 RID: 20
		public Dictionary<string, Quaternion> extraRotationBones = new Dictionary<string, Quaternion>();

		// Token: 0x04000015 RID: 21
		public Dictionary<string, Vector3> extraRotationBonesForEditor = new Dictionary<string, Vector3>();

		// Token: 0x04000016 RID: 22
		private List<Transform> _creatureBones = new List<Transform>();

		// Token: 0x04000017 RID: 23
		private Dismemberment _customAvatarDismemberment;

		// Token: 0x04000018 RID: 24
		private CustomAvatar _customAvatar;

		// Token: 0x04000019 RID: 25
		public bool enforceHandPosition;

		// Token: 0x0400001A RID: 26
		private readonly Dictionary<string, string> _ragdollPartNameAvatarBoneNameMapper = new Dictionary<string, string>
		{
			{ "RightFoot", "RightFoot_Mesh" },
			{ "LeftFoot", "LeftFoot_Mesh" },
			{ "RightLeg", "RightLeg_Mesh" },
			{ "LeftLeg", "LeftLeg_Mesh" },
			{ "RightUpLeg", "RightUpLeg_Mesh" },
			{ "LeftUpLeg", "LeftUpLeg_Mesh" },
			{ "Hips", "Hips_Mesh" },
			{ "Spine", "Spine_Mesh" },
			{ "Spine1", "Spine1_Mesh" },
			{ "RightArm", "RightArm_Mesh" },
			{ "LeftArm", "LeftArm_Mesh" },
			{ "RightForeArm", "RightForeArm_Mesh" },
			{ "LeftForeArm", "LeftForeArm_Mesh" },
			{ "RightHand", "RightHand_Mesh" },
			{ "LeftHand", "LeftHand_Mesh" },
			{ "Neck", "Neck_Mesh" },
			{ "Head", "Head_Mesh" }
		};

		// Token: 0x0400001B RID: 27
		private List<Transform> _severedAvatarParts = new List<Transform>();

		// Token: 0x0400001C RID: 28
		private Queue<RagdollPart> _severedRagdollParts = new Queue<RagdollPart>();

		// Token: 0x0400001D RID: 29
		private Dictionary<string, Transform> _avatarBones = new Dictionary<string, Transform>();

		// Token: 0x0400001E RID: 30
		public float avatarHeight;

		// Token: 0x0400001F RID: 31
		public Vector3 extraHipPosition;

		// Token: 0x04000020 RID: 32
		public Vector3 extraDimension;

		// Token: 0x04000021 RID: 33
		private List<string> _disallowSlicedRagdollPartNames = new List<string> { "RightUpLeg", "LeftUpLeg", "LeftArm", "RightArm" };

		// Token: 0x04000022 RID: 34
		private List<string> _armBoneNames = new List<string>();
	}
}
