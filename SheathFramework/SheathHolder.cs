using System;
using System.Collections.Generic;
using System.Linq;
using ThunderRoad;
using UnityEngine;

namespace SheathFramework
{
	// Token: 0x02000005 RID: 5
	public class SheathHolder : Holder
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000005 RID: 5 RVA: 0x000021EF File Offset: 0x000003EF
		protected override ManagedLoops ManagedLoops
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000021F2 File Offset: 0x000003F2
		public override void SetTouch(bool active)
		{
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000021F8 File Offset: 0x000003F8
		public void FillWithDefault(string itemId)
		{
			bool flag = this.sheathedItem;
			if (flag)
			{
				this.UnSheatheItem(this.sheathedItem);
			}
			ItemData data = Catalog.GetData<ItemData>(itemId, true);
			data.SpawnAsync(delegate(Item item)
			{
				this.SheatheItem(item, item.mainCollisionHandler.damagers.FirstOrDefault((Damager d) => d.penetrationDepth > 0f), true, true, true);
			}, null, null, null, true, null);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002254 File Offset: 0x00000454
		protected override void Awake()
		{
			this.interactableId = "HolderSheath";
			bool flag = this.slots.Count == 0;
			if (flag)
			{
				this.slots.Add(base.transform);
			}
			this.creature = base.GetComponentInParent<Creature>();
			this.parentItem = base.GetComponentInParent<Item>();
			this.parentItem.OnDespawnEvent += new Item.SpawnEvent(this.Sheath_OnDespawnEvent);
			this.parentItem.OnSnapEvent += new Item.HolderDelegate(this.Sheath_OnSnapEvent);
			this.parentItem.OnUnSnapEvent += new Item.HolderDelegate(this.Sheath_OnUnSnapEvent);
			this.parentItem.OnGrabEvent += new Item.GrabDelegate(this.Sheath_OnGrabEvent);
			this.parentItem.OnUngrabEvent += new Item.ReleaseDelegate(this.Sheath_OnUngrabEvent);
			this.dummyJoint = new GameObject("DummyJoint");
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002334 File Offset: 0x00000534
		protected override void Start()
		{
			base.Start();
			base.data.delegateToParentHolder = this.module.toggleHolsteredInteraction;
			this.path = base.transform.Cast<Transform>().ToArray<Transform>();
			this.parentItem.childHolders.Add(this);
			base.RefreshChildAndParentHolder();
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002390 File Offset: 0x00000590
		public override void Load(InteractableData interactableData)
		{
			bool flag = !(interactableData is HolderData);
			if (flag)
			{
				Debug.LogError("Trying to load wrong data type");
			}
			else
			{
				base.Load(interactableData as HolderData);
				bool flag2 = this.audioContainer;
				if (flag2)
				{
					Catalog.ReleaseAsset<AudioContainer>(this.audioContainer);
				}
				Catalog.LoadAssetAsync<AudioContainer>(this.module.audioContainerLocation, delegate(AudioContainer value)
				{
					this.audioContainer = value;
				}, interactableData.id);
				ContentCustomDataSheath customData;
				bool flag3 = this.parentItem.TryGetCustomData<ContentCustomDataSheath>(ref customData) && customData.sheathedItemId != null && customData.sheathedItemId != "";
				if (flag3)
				{
					this.FillWithDefault(customData.sheathedItemId);
				}
				else
				{
					bool flag4 = !string.IsNullOrEmpty(this.module.spawnItemId);
					if (flag4)
					{
						this.FillWithDefault(this.module.spawnItemId);
					}
				}
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002475 File Offset: 0x00000675
		public override bool CanTouch(RagdollHand ragdollHand)
		{
			return false;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002478 File Offset: 0x00000678
		public override bool GrabFromHandle()
		{
			return true;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x0000247B File Offset: 0x0000067B
		public override bool ObjectAllowed(Item item)
		{
			return this.SlotAllowed(item.itemId);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x0000248C File Offset: 0x0000068C
		public override bool SlotAllowed(string itemId)
		{
			return (this.module.tagFilter == null) ? (!this.module.items.Contains(itemId)) : (this.module.tagFilter == 1 && this.module.items.Contains(itemId));
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000024DE File Offset: 0x000006DE
		public override void OnTouchStart(RagdollHand ragdollHand)
		{
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000024E1 File Offset: 0x000006E1
		public override void OnTouchEnd(RagdollHand ragdollHand)
		{
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000024E4 File Offset: 0x000006E4
		public override void ShowHint(RagdollHand ragdollHand)
		{
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000024E7 File Offset: 0x000006E7
		public override bool TryTouchAction(RagdollHand ragdollHand, Interactable.Action action)
		{
			return false;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000024EC File Offset: 0x000006EC
		public override void Snap(Item item, bool silent = false, bool addLinkedContainerContent = true)
		{
			bool flag = !this.HasSlotFree();
			if (!flag)
			{
				item.waterHandler.Reset();
				this.currentQuantity++;
				item.RefreshCollision(false);
				item.rb.collisionDetectionMode = 0;
				this.items.Add(item);
				this.SetSheathedItemCollision(this.parentItem.holder == null);
				this.parentHolder = this.parentItem.holder;
				foreach (Item obj in this.items)
				{
					foreach (Holder childHolder in obj.childHolders)
					{
						bool flag2 = !this.GrabFromHandle();
						if (flag2)
						{
							bool delegateToParentHolder = childHolder.data.delegateToParentHolder;
							if (delegateToParentHolder)
							{
								this.grabChildHolder = true;
							}
							childHolder.SetTouch(false);
						}
					}
				}
				item.OnSnap(this, silent);
				bool flag3 = base.data.hideFromFpv && this.creature && this.creature.player;
				if (flag3)
				{
					item.SetMeshLayer(GameManager.GetLayer(11));
				}
				item.contentState = new ContentStateHolder(base.name);
				bool flag4 = base.GetRootHolder().creature && base.GetRootHolder().creature.hidden;
				if (flag4)
				{
					foreach (Item obj2 in this.items)
					{
						obj2.Hide(true);
					}
				}
				this.RaiseSnapped(item);
			}
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002700 File Offset: 0x00000900
		public override void UnSnap(Item item, bool silent = false, bool removeLinkedContainerContent = true)
		{
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002704 File Offset: 0x00000904
		private void OnDestroy()
		{
			this.parentItem.OnDespawnEvent -= new Item.SpawnEvent(this.Sheath_OnDespawnEvent);
			this.parentItem.OnSnapEvent -= new Item.HolderDelegate(this.Sheath_OnSnapEvent);
			this.parentItem.OnUnSnapEvent -= new Item.HolderDelegate(this.Sheath_OnUnSnapEvent);
			this.parentItem.OnGrabEvent -= new Item.GrabDelegate(this.Sheath_OnGrabEvent);
			this.parentItem.OnUngrabEvent -= new Item.ReleaseDelegate(this.Sheath_OnUngrabEvent);
			Object.Destroy(this.dummyJoint);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002798 File Offset: 0x00000998
		private void OnTriggerEnter(Collider other)
		{
			bool flag = this.sheathedItem;
			if (!flag)
			{
				bool flag2 = Time.time - this.lastTimeUnSheathed <= 0.1f;
				if (!flag2)
				{
					Item item = other.GetComponentInParent<Item>();
					bool flag3 = item && this.ObjectAllowed(item);
					if (flag3)
					{
						ColliderGroup colliderGroup = other.GetComponentInParent<ColliderGroup>();
						Damager damager = item.mainCollisionHandler.damagers.FirstOrDefault((Damager d) => d.penetrationDepth > 0f && ((colliderGroup != null && d.colliderGroup == colliderGroup) || d.colliderOnly == other));
						this.SheatheItem(item, damager, false, false, false);
					}
				}
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x0000285C File Offset: 0x00000A5C
		private void Sheath_OnDespawnEvent(EventTime eventTime)
		{
			bool flag = !this.sheathedItem;
			if (!flag)
			{
				Item item = this.sheathedItem;
				this.UnSheatheItem(this.sheathedItem);
				item.Despawn();
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x0000289C File Offset: 0x00000A9C
		private void Sheath_OnSnapEvent(Holder holder)
		{
			this.parentItem.rb.isKinematic = false;
			this.parentItem.transform.SetParent(null, true);
			this.parentHolder = holder;
			bool flag = this.sheathedItem;
			if (flag)
			{
				Common.MoveAlign(this.sheathedItem.transform, this.tip, base.transform.TransformPoint(new Vector3(0f, 0f, this.currentDepth)), Common.TransformRotation(base.transform, Quaternion.Euler(0f, 0f, this.isReversed ? 180f : 0f)), null);
				foreach (Handle handle in this.sheathedItem.handles)
				{
					handle.SetTelekinesis(false);
				}
			}
			this.snapJoint = this.parentItem.rb.gameObject.AddComponent<ConfigurableJoint>();
			this.snapJoint.connectedBody = holder.GetComponentInParent<Rigidbody>();
			this.snapJoint.anchor = this.parentItem.rb.transform.InverseTransformPoint(this.parentItem.holderPoint.position);
			this.snapJoint.axis = this.parentItem.rb.transform.InverseTransformDirection(this.parentItem.holderPoint.right);
			this.snapJoint.secondaryAxis = this.parentItem.rb.transform.InverseTransformDirection(this.parentItem.holderPoint.up);
			this.snapJoint.xMotion = 0;
			this.snapJoint.yMotion = 0;
			this.snapJoint.zMotion = 0;
			this.snapJoint.angularXMotion = 1;
			this.snapJoint.angularYMotion = 1;
			this.snapJoint.angularZMotion = 1;
			SoftJointLimit jointLimit = this.snapJoint.highAngularXLimit;
			jointLimit.limit = 90f;
			this.snapJoint.highAngularXLimit = jointLimit;
			this.snapJoint.lowAngularXLimit = jointLimit;
			this.snapJoint.angularYLimit = jointLimit;
			this.snapJoint.angularZLimit = jointLimit;
			this.snapJoint.rotationDriveMode = 1;
			JointDrive drive = this.snapJoint.slerpDrive;
			drive.positionSpring = 1000f;
			drive.positionDamper = 100f;
			this.snapJoint.slerpDrive = drive;
			this.SetSheathedItemCollision(false);
			this.CheckLock();
			this.RemovePlayerJoint();
			bool toggleHolsteredInteraction = this.module.toggleHolsteredInteraction;
			if (toggleHolsteredInteraction)
			{
				this.parentHolder.grabChildHolder = this.sheathedItem;
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002B78 File Offset: 0x00000D78
		private void Sheath_OnUnSnapEvent(Holder holder)
		{
			bool flag = this.snapJoint;
			if (flag)
			{
				Object.Destroy(this.snapJoint);
			}
			this.SetSheathedItemCollision(true);
			this.CheckLock();
			bool flag2 = this.sheathedItem;
			if (flag2)
			{
				List<RagdollHand> handlers = new List<RagdollHand>(this.sheathedItem.handlers);
				foreach (RagdollHand hand in handlers)
				{
					this.AddPlayerJoint(hand);
				}
				this.sheathedItem.RefreshAllowTelekinesis();
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002C24 File Offset: 0x00000E24
		private void Sheath_OnGrabEvent(Handle handle, RagdollHand ragdollHand)
		{
			bool flag = !this.sheathedItem;
			if (!flag)
			{
				this.RefreshCollision();
				this.CheckLock();
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002C54 File Offset: 0x00000E54
		private void Sheath_OnUngrabEvent(Handle handle, RagdollHand ragdollHand, bool throwing)
		{
			bool flag = !this.sheathedItem;
			if (!flag)
			{
				this.RefreshCollision();
				this.CheckLock();
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002C84 File Offset: 0x00000E84
		private void SheathedItem_OnDespawnEvent(EventTime eventTime)
		{
			this.UnSheatheItem(this.sheathedItem);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002C94 File Offset: 0x00000E94
		private void SheathedItem_OnGrabEvent(Handle handle, RagdollHand ragdollHand)
		{
			this.RefreshCollision();
			this.CheckLock();
			this.RemovePlayerJoint();
			bool flag = this.sheathedItem && this.parentItem.holder;
			if (flag)
			{
				foreach (Handle handle2 in this.sheathedItem.handles)
				{
					handle2.SetTelekinesis(false);
				}
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002D2C File Offset: 0x00000F2C
		private void SheathedItem_OnUngrabEvent(Handle handle, RagdollHand ragdollHand, bool throwing)
		{
			this.RefreshCollision();
			this.CheckLock();
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002D40 File Offset: 0x00000F40
		private void Handle_UnGrabbed(RagdollHand ragdollHand, Handle handle, EventTime eventTime)
		{
			bool flag = eventTime != 1;
			if (!flag)
			{
				bool flag2 = this.sheathedItem && this.parentItem.holder;
				if (flag2)
				{
					handle.SetTelekinesis(false);
				}
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002D88 File Offset: 0x00000F88
		protected override void ManagedUpdate()
		{
			bool flag = !this.sheathedItem;
			if (!flag)
			{
				bool flag2 = !this.sheathJoint || !this.sheathJoint.gameObject.activeInHierarchy || !this.sheathJoint.connectedBody || !this.sheathJoint.connectedBody.gameObject.activeInHierarchy;
				if (flag2)
				{
					bool isQuitting = GameManager.isQuitting;
					if (!isQuitting)
					{
						this.UnSnap(false);
						this.UnSheatheItem(this.sheathedItem);
					}
				}
				else
				{
					Vector3 depth = base.transform.InverseTransformPoint(this.tip.position);
					this.currentDepth = depth.z;
					bool flag3 = !this.isLocked;
					if (flag3)
					{
						this.SetTarget(this.currentDepth);
					}
					this.RefreshJointDrive();
					bool flag4 = this.effectInstance != null;
					if (flag4)
					{
						this.effectInstance.SetSpeed(Mathf.Abs(this.currentDepth - this.lastDepth) / 0.02f);
					}
					bool flag5 = this.sheathedItem.leftPlayerHand || this.sheathedItem.rightPlayerHand;
					if (flag5)
					{
						float penetrationPeriod = Catalog.gameData.haptics.penetrationPeriod;
						float penetrationIntensity = Catalog.gameData.haptics.penetrationIntensity;
						bool flag6 = Mathf.Abs(this.currentDepth - this.lastRumbleDepth) > penetrationPeriod;
						if (flag6)
						{
							bool flag7 = this.sheathedItem.leftPlayerHand;
							if (flag7)
							{
								PlayerControl.handLeft.HapticShort(penetrationIntensity);
							}
							else
							{
								bool flag8 = this.sheathedItem.rightPlayerHand;
								if (flag8)
								{
									PlayerControl.handRight.HapticShort(penetrationIntensity);
								}
							}
							this.lastRumbleDepth = this.currentDepth;
						}
					}
					bool flag9 = this.currentDepth > this.module.snapDepth && this.currentDepth > this.lastDepth;
					if (flag9)
					{
						this.Snap(false);
					}
					else
					{
						bool flag10 = this.currentDepth <= this.module.snapDepth && this.currentDepth < this.lastDepth;
						if (flag10)
						{
							this.UnSnap(false);
						}
					}
					bool flag11 = this.currentDepth <= 0f && this.currentDepth < this.lastDepth;
					if (flag11)
					{
						this.UnSheatheItem(this.sheathedItem);
					}
					this.lastDepth = this.currentDepth;
				}
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x0000300C File Offset: 0x0000120C
		public void SheatheItem(Item item, Damager damager, bool force = false, bool fullSheath = false, bool silent = false)
		{
			bool flag = !damager;
			if (!flag)
			{
				this.tip = damager.transform;
				bool flag2 = !force && this.module.badAngle > 0f && Vector3.Angle(this.tip.forward, base.transform.forward) > this.module.badAngle;
				if (!flag2)
				{
					this.sheathDepth = ((this.module.maxDepth > 0f) ? Mathf.Min(this.module.maxDepth, damager.penetrationDepth) : damager.penetrationDepth);
					Vector3 depth = base.transform.InverseTransformPoint(this.tip.position);
					this.currentDepth = (this.lastDepth = (fullSheath ? this.sheathDepth : Mathf.Max(depth.z, 0f)));
					bool flag3 = !force && this.module.badDepth > 0f && this.currentDepth > this.module.badDepth;
					if (!flag3)
					{
						this.lastTimeUnSheathed = Time.time;
						this.sheathedItem = item;
						this.parentItem.AddCustomData<ContentCustomDataSheath>(new ContentCustomDataSheath(item.itemId));
						bool allowReverse = this.module.allowReverse;
						if (allowReverse)
						{
							float num = Vector3.Dot(base.transform.right, this.tip.right) + Vector3.Dot(base.transform.up, this.tip.up);
							float num2 = Vector3.Dot(base.transform.right, -this.tip.right) + Vector3.Dot(base.transform.up, -this.tip.up);
							this.isReversed = num2 > num;
						}
						bool lockUnlessHeld = this.module.lockUnlessHeld;
						if (lockUnlessHeld)
						{
							this.isLocked = item.handlers.Count <= 0 || (this.parentItem.handlers.Count <= 0 && !this.parentItem.holder);
						}
						Common.MoveAlign(item.transform, this.tip, base.transform.TransformPoint(new Vector3(0f, 0f, this.currentDepth)), Common.TransformRotation(base.transform, Quaternion.Euler(0f, 0f, this.isReversed ? 180f : 0f)), null);
						this.sheathJoint = this.parentItem.rb.gameObject.AddComponent<ConfigurableJoint>();
						this.sheathJoint.connectedBody = item.rb;
						this.sheathJoint.anchor = this.parentItem.rb.transform.InverseTransformPoint(base.transform.TransformPoint(new Vector3(0f, 0f, this.isLocked ? this.currentDepth : 0f)));
						this.sheathJoint.axis = this.parentItem.rb.transform.InverseTransformDirection((this.isReversed ? (-1f) : 1f) * base.transform.right);
						this.sheathJoint.secondaryAxis = this.parentItem.rb.transform.InverseTransformDirection((this.isReversed ? (-1f) : 1f) * base.transform.up);
						this.sheathJoint.autoConfigureConnectedAnchor = false;
						this.sheathJoint.connectedAnchor = item.rb.transform.InverseTransformPoint(this.tip.position);
						this.sheathJoint.xMotion = 0;
						this.sheathJoint.yMotion = 0;
						this.sheathJoint.zMotion = (this.isLocked ? 0 : 1);
						SoftJointLimit linearLimit = this.sheathJoint.linearLimit;
						linearLimit.limit = this.sheathDepth;
						this.sheathJoint.linearLimit = linearLimit;
						bool flag4 = this.path.Length == 0;
						if (flag4)
						{
							this.sheathJoint.angularXMotion = 0;
							this.sheathJoint.angularYMotion = 0;
							this.sheathJoint.angularZMotion = 0;
						}
						else
						{
							this.sheathJoint.rotationDriveMode = 1;
							JointDrive jointDrive = this.sheathJoint.slerpDrive;
							jointDrive.positionSpring = 10000000f;
							this.sheathJoint.slerpDrive = jointDrive;
						}
						this.RefreshJointDrive();
						this.SetTarget(this.currentDepth);
						item.disableSnap = true;
						foreach (Handle handle in item.handles)
						{
							handle.data.ignoreSnap = true;
							handle.UnGrabbed += new Handle.GrabEvent(this.Handle_UnGrabbed);
							bool flag5 = this.parentItem.holder;
							if (flag5)
							{
								handle.SetTelekinesis(false);
							}
						}
						item.disallowDespawn = true;
						item.disallowRoomDespawn = true;
						item.OnDespawnEvent += new Item.SpawnEvent(this.SheathedItem_OnDespawnEvent);
						foreach (RagdollHand hand in this.parentItem.handlers)
						{
							this.Sheath_OnGrabEvent(hand.grabbedHandle, hand);
						}
						item.OnGrabEvent += new Item.GrabDelegate(this.SheathedItem_OnGrabEvent);
						item.OnUngrabEvent += new Item.ReleaseDelegate(this.SheathedItem_OnUngrabEvent);
						item.RefreshCollision(false);
						if (fullSheath)
						{
							this.Snap(silent);
						}
						this.RemovePlayerJoint();
						bool flag6 = this.module.grindingEffectData != null;
						if (flag6)
						{
							bool flag7 = this.effectInstance == null;
							if (flag7)
							{
								this.effectInstance = this.module.grindingEffectData.Spawn(item.transform, true, null, false, Array.Empty<Type>());
							}
							this.effectInstance.Play(0, false);
						}
					}
				}
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00003664 File Offset: 0x00001864
		public void Snap(bool silent = false)
		{
			bool flag = !this.sheathedItem || this.sheathedItem.holder;
			if (!flag)
			{
				this.Snap(this.sheathedItem, silent, true);
				bool flag2 = this.module.toggleHolsteredInteraction && this.parentHolder;
				if (flag2)
				{
					this.parentHolder.grabChildHolder = true;
				}
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000036D4 File Offset: 0x000018D4
		public void UnSheatheItem(Item item)
		{
			bool flag = !this.sheathedItem;
			if (!flag)
			{
				this.tip = null;
				this.sheathedItem = null;
				this.parentItem.RemoveCustomData<ContentCustomDataSheath>();
				Object.Destroy(this.sheathJoint);
				List<RagdollHand> handlers = new List<RagdollHand>(item.handlers);
				foreach (RagdollHand hand in handlers)
				{
					this.AddPlayerJoint(hand);
				}
				item.disableSnap = false;
				foreach (Handle handle in item.handles)
				{
					handle.data.ignoreSnap = false;
					handle.UnGrabbed -= new Handle.GrabEvent(this.Handle_UnGrabbed);
				}
				item.RefreshAllowTelekinesis();
				item.disallowDespawn = false;
				item.disallowRoomDespawn = false;
				item.OnDespawnEvent -= new Item.SpawnEvent(this.SheathedItem_OnDespawnEvent);
				foreach (RagdollHand hand2 in this.parentItem.handlers)
				{
					this.Sheath_OnUngrabEvent(hand2.grabbedHandle, hand2, false);
				}
				item.OnGrabEvent -= new Item.GrabDelegate(this.SheathedItem_OnGrabEvent);
				item.OnUngrabEvent -= new Item.ReleaseDelegate(this.SheathedItem_OnUngrabEvent);
				bool flag2 = this.effectInstance != null;
				if (flag2)
				{
					this.effectInstance.Despawn();
					this.effectInstance = null;
				}
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000038A0 File Offset: 0x00001AA0
		public void UnSnap(bool silent = false)
		{
			bool flag = !this.sheathedItem || !this.sheathedItem.holder || Time.time - this.lastTimeUnSnapped <= 0.1f;
			if (!flag)
			{
				bool flag2 = this.module.toggleHolsteredInteraction && this.parentHolder;
				if (flag2)
				{
					this.parentHolder.grabChildHolder = false;
				}
				base.UnSnap(this.sheathedItem, silent, true);
				this.lastTimeUnSnapped = Time.time;
				bool flag3 = base.GetRootHolder().creature && base.GetRootHolder().creature.hidden;
				if (flag3)
				{
					this.sheathedItem.Hide(true);
				}
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00003968 File Offset: 0x00001B68
		private void RefreshJointDrive()
		{
			bool held = this.sheathedItem.handlers.Count > 0 && (this.parentItem.handlers.Count > 0 || this.parentItem.holder);
			JointDrive jointDrive = this.sheathJoint.zDrive;
			jointDrive.positionDamper = (held ? this.module.damperHeld : this.module.damper);
			this.sheathJoint.zDrive = jointDrive;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000039F0 File Offset: 0x00001BF0
		private void SetTarget(float currentDepth)
		{
			bool flag = this.path.Length == 0;
			if (!flag)
			{
				float value = Mathf.Clamp01(currentDepth / this.sheathDepth) * (float)this.path.Length;
				int fromIndex = Mathf.FloorToInt(value) - 1;
				Transform from = ((fromIndex < 0) ? base.transform : this.path[fromIndex]);
				Transform to = ((fromIndex >= this.path.Length - 1) ? from : this.path[fromIndex + 1]);
				float t = value - (float)Math.Truncate((double)value);
				Vector3 localAnchor = base.transform.InverseTransformPoint(Vector3.Lerp(from.position, to.position, t));
				this.sheathJoint.anchor = this.parentItem.rb.transform.InverseTransformPoint(base.transform.TransformPoint(new Vector3(localAnchor.x, localAnchor.y, this.isLocked ? currentDepth : 0f)));
				this.sheathJoint.targetRotation = Common.InverseTransformRotation(base.transform, Quaternion.Lerp(from.rotation, to.rotation, t));
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00003B0C File Offset: 0x00001D0C
		private void SetSheathedItemCollision(bool active)
		{
			bool flag = !this.sheathedItem;
			if (!flag)
			{
				foreach (ColliderGroup colliderGroup in this.sheathedItem.colliderGroups)
				{
					foreach (Collider collider in colliderGroup.colliders)
					{
						bool flag2 = collider != null;
						if (flag2)
						{
							bool flag3 = !collider.isTrigger;
							if (flag3)
							{
								collider.enabled = active;
							}
						}
					}
				}
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00003BE4 File Offset: 0x00001DE4
		private void RefreshCollision()
		{
			PlayerHand leftPlayerHand = null;
			PlayerHand rightPlayerHand = null;
			this.parentItem.ResetRagdollCollision();
			this.parentItem.ResetObjectCollision();
			Item item = this.sheathedItem;
			if (item != null)
			{
				item.ResetRagdollCollision();
			}
			Item item2 = this.sheathedItem;
			if (item2 != null)
			{
				item2.ResetObjectCollision();
			}
			foreach (RagdollHand handler in this.parentItem.handlers)
			{
				bool flag = handler.playerHand;
				if (flag)
				{
					bool flag2 = handler.playerHand.side == 1;
					if (flag2)
					{
						leftPlayerHand = handler.playerHand;
					}
					else
					{
						rightPlayerHand = handler.playerHand;
					}
				}
			}
			bool flag3 = this.sheathedItem;
			if (flag3)
			{
				foreach (RagdollHand handler2 in this.sheathedItem.handlers)
				{
					bool flag4 = handler2.playerHand;
					if (flag4)
					{
						bool flag5 = handler2.playerHand.side == 1;
						if (flag5)
						{
							leftPlayerHand = handler2.playerHand;
						}
						else
						{
							rightPlayerHand = handler2.playerHand;
						}
					}
				}
			}
			bool flag6 = rightPlayerHand || leftPlayerHand;
			if (flag6)
			{
				Item item3 = this.sheathedItem;
				if (item3 != null)
				{
					item3.SetColliderAndMeshLayer(GameManager.GetLayer(5), false);
				}
				bool selfCollision = Player.selfCollision;
				if (selfCollision)
				{
					bool flag7 = rightPlayerHand && leftPlayerHand;
					if (flag7)
					{
						this.parentItem.IgnoreRagdollCollision(rightPlayerHand.ragdollHand.creature.ragdoll, 1935);
						Item item4 = this.sheathedItem;
						if (item4 != null)
						{
							item4.IgnoreRagdollCollision(rightPlayerHand.ragdollHand.creature.ragdoll, 1935);
						}
					}
					else
					{
						bool flag8 = rightPlayerHand;
						if (flag8)
						{
							this.parentItem.IgnoreRagdollCollision(rightPlayerHand.ragdollHand.creature.ragdoll, 1967);
							Item item5 = this.sheathedItem;
							if (item5 != null)
							{
								item5.IgnoreRagdollCollision(rightPlayerHand.ragdollHand.creature.ragdoll, 1967);
							}
						}
						else
						{
							bool flag9 = leftPlayerHand;
							if (flag9)
							{
								this.parentItem.IgnoreRagdollCollision(leftPlayerHand.ragdollHand.creature.ragdoll, 2007);
								Item item6 = this.sheathedItem;
								if (item6 != null)
								{
									item6.IgnoreRagdollCollision(leftPlayerHand.ragdollHand.creature.ragdoll, 2007);
								}
							}
						}
					}
				}
				else
				{
					bool flag10 = rightPlayerHand && leftPlayerHand;
					if (flag10)
					{
						this.parentItem.IgnoreRagdollCollision(rightPlayerHand.ragdollHand.creature.ragdoll, 1536);
						Item item7 = this.sheathedItem;
						if (item7 != null)
						{
							item7.IgnoreRagdollCollision(rightPlayerHand.ragdollHand.creature.ragdoll, 1536);
						}
					}
					else
					{
						bool flag11 = rightPlayerHand;
						if (flag11)
						{
							this.parentItem.IgnoreRagdollCollision(rightPlayerHand.ragdollHand.creature.ragdoll, 1568);
							Item item8 = this.sheathedItem;
							if (item8 != null)
							{
								item8.IgnoreRagdollCollision(rightPlayerHand.ragdollHand.creature.ragdoll, 1568);
							}
						}
						else
						{
							bool flag12 = leftPlayerHand;
							if (flag12)
							{
								this.parentItem.IgnoreRagdollCollision(leftPlayerHand.ragdollHand.creature.ragdoll, 1600);
								Item item9 = this.sheathedItem;
								if (item9 != null)
								{
									item9.IgnoreRagdollCollision(leftPlayerHand.ragdollHand.creature.ragdoll, 1600);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00003FB8 File Offset: 0x000021B8
		private void CheckLock()
		{
			bool flag = !this.module.lockUnlessHeld || !this.sheathedItem;
			if (!flag)
			{
				bool held = this.sheathedItem.handlers.Count > 0 && (this.parentItem.handlers.Count > 0 || this.parentItem.holder);
				bool flag2 = this.isLocked != held;
				if (!flag2)
				{
					this.isLocked = !held;
					bool flag3 = this.isLocked;
					if (flag3)
					{
						this.sheathJoint.zMotion = 0;
						this.sheathJoint.anchor = this.parentItem.rb.transform.InverseTransformPoint(base.transform.TransformPoint(new Vector3(0f, 0f, this.currentDepth)));
						this.SetTarget(this.currentDepth);
					}
					else
					{
						this.sheathJoint.zMotion = 1;
						this.sheathJoint.anchor = this.parentItem.rb.transform.InverseTransformPoint(base.transform.position);
					}
				}
			}
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000040F0 File Offset: 0x000022F0
		private void AddPlayerJoint(RagdollHand hand)
		{
			Handle handle = hand.grabbedHandle;
			bool flag = hand.gripInfo.playerJoint.gameObject == this.dummyJoint && handle;
			if (flag)
			{
				float axisPosition = hand.gripInfo.axisPosition;
				HandlePose handlePose = hand.gripInfo.orientation;
				hand.UnGrab(false);
				hand.Grab(handle, handlePose, axisPosition, false);
			}
		}

		// Token: 0x0600002B RID: 43 RVA: 0x0000415C File Offset: 0x0000235C
		private void RemovePlayerJoint()
		{
			bool flag = this.parentItem.holder && this.sheathedItem;
			if (flag)
			{
				foreach (RagdollHand hand in this.sheathedItem.handlers)
				{
					hand.gripInfo.hasPlayerJoint = false;
					bool flag2 = hand.gripInfo.playerJoint;
					if (flag2)
					{
						Object.Destroy(hand.gripInfo.playerJoint);
					}
					hand.gripInfo.playerJoint = this.dummyJoint.AddComponent<ConfigurableJoint>();
				}
			}
		}

		// Token: 0x04000013 RID: 19
		[NonSerialized]
		public ItemModuleSheath module;

		// Token: 0x04000014 RID: 20
		[NonSerialized]
		public Item sheathedItem;

		// Token: 0x04000015 RID: 21
		protected Transform[] path;

		// Token: 0x04000016 RID: 22
		protected Transform tip;

		// Token: 0x04000017 RID: 23
		private ConfigurableJoint sheathJoint;

		// Token: 0x04000018 RID: 24
		private ConfigurableJoint snapJoint;

		// Token: 0x04000019 RID: 25
		private float sheathDepth;

		// Token: 0x0400001A RID: 26
		private float currentDepth;

		// Token: 0x0400001B RID: 27
		private float lastDepth;

		// Token: 0x0400001C RID: 28
		private float lastRumbleDepth;

		// Token: 0x0400001D RID: 29
		private float lastTimeUnSheathed;

		// Token: 0x0400001E RID: 30
		private float lastTimeUnSnapped;

		// Token: 0x0400001F RID: 31
		private bool isReversed;

		// Token: 0x04000020 RID: 32
		private bool isLocked;

		// Token: 0x04000021 RID: 33
		private EffectInstance effectInstance;

		// Token: 0x04000022 RID: 34
		private GameObject dummyJoint;
	}
}
