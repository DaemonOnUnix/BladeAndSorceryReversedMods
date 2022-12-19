using System;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

namespace Heartbreaker
{
	// Token: 0x02000002 RID: 2
	public class HeartBehaviour : ThunderBehaviour
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		protected override ManagedLoops ManagedLoops
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002054 File Offset: 0x00000254
		public void Awake()
		{
			this.creature.ragdoll.SetState(3);
			this.item = base.GetComponent<Item>();
			this.item.rb.isKinematic = true;
			this.item.transform.SetParent(this.creature.ragdoll.targetPart.bone.animation);
			this.item.transform.localPosition = new Vector3(-0.06f, -0.04f, 0f);
			this.item.transform.localRotation = Quaternion.Euler(-120f, 180f, 0f);
			this.item.transform.localScale = Vector3.one;
			this.joint = this.item.gameObject.AddComponent<FixedJoint>();
			bool flag = this.creature.ragdoll.state != 5 && this.creature.ragdoll.state != 6 && this.creature.ragdoll.state != 4;
			if (flag)
			{
				base.transform.SetParent(this.creature.ragdoll.targetPart.transform);
				this.item.transform.localPosition = new Vector3(-0.06f, -0.04f, 0f);
				this.item.transform.localRotation = Quaternion.Euler(-120f, 180f, 0f);
				this.joint.connectedBody = this.creature.ragdoll.targetPart.rb;
				base.transform.SetParent(null);
				this.item.rb.isKinematic = false;
			}
			this.item.disallowDespawn = true;
			this.item.disallowRoomDespawn = true;
			foreach (Handle handle in this.item.handles)
			{
				handle.data.allowTelekinesis = false;
				handle.touchRadius = 0.01f;
			}
			this.item.RefreshAllowTelekinesis();
			this.creature.OnDespawnEvent += new Creature.DespawnEvent(this.Creature_OnDespawnEvent);
			this.creature.ragdoll.OnStateChange += new Ragdoll.StateChange(this.Ragdoll_OnStateChange);
			this.item.OnDespawnEvent += new Item.SpawnEvent(this.Item_OnDespawnEvent);
			this.item.OnGrabEvent += new Item.GrabDelegate(this.Item_OnGrabEvent);
			this.item.OnUngrabEvent += new Item.ReleaseDelegate(this.Item_OnUngrabEvent);
			this.item.mainCollisionHandler.OnCollisionStartEvent += new CollisionHandler.CollisionEvent(this.MainCollisionHandler_OnCollisionStartEvent);
			bool flag2 = HeartBehaviour.squashEffect == null;
			if (flag2)
			{
				HeartBehaviour.squashEffect = Catalog.GetData<EffectData>("SquashHeart", true);
			}
			bool flag3 = HeartBehaviour.ripOutEffect == null;
			if (flag3)
			{
				HeartBehaviour.ripOutEffect = Catalog.GetData<EffectData>("RipOutHeart", true);
			}
			bool flag4 = HeartBehaviour.squeezeEffect == null;
			if (flag4)
			{
				HeartBehaviour.squeezeEffect = Catalog.GetData<EffectData>("SqueezeHeart", true);
			}
			bool flag5 = HeartBehaviour.bleedingEffect == null;
			if (flag5)
			{
				HeartBehaviour.bleedingEffect = Catalog.GetData<EffectData>("SliceFleshChild", true);
			}
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000023BC File Offset: 0x000005BC
		protected override void ManagedUpdate()
		{
			this.time += Time.deltaTime;
			this.UpdateHeartBeat();
			this.UpdateSquashHeart();
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000023E0 File Offset: 0x000005E0
		public void OnDestroy()
		{
			this.creature.OnDespawnEvent -= new Creature.DespawnEvent(this.Creature_OnDespawnEvent);
			this.creature.ragdoll.OnStateChange -= new Ragdoll.StateChange(this.Ragdoll_OnStateChange);
			this.item.OnDespawnEvent -= new Item.SpawnEvent(this.Item_OnDespawnEvent);
			this.item.OnGrabEvent -= new Item.GrabDelegate(this.Item_OnGrabEvent);
			this.item.OnUngrabEvent -= new Item.ReleaseDelegate(this.Item_OnUngrabEvent);
			this.item.mainCollisionHandler.OnCollisionStartEvent -= new CollisionHandler.CollisionEvent(this.MainCollisionHandler_OnCollisionStartEvent);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002488 File Offset: 0x00000688
		private void Creature_OnDespawnEvent(EventTime eventTime)
		{
			bool flag = this.item && this.isHeartInside;
			if (flag)
			{
				this.item.Despawn();
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000024BC File Offset: 0x000006BC
		private void Ragdoll_OnStateChange(Ragdoll.State previousState, Ragdoll.State newState, Ragdoll.PhysicStateChange physicStateChange, EventTime eventTime)
		{
			bool flag = !this.isHeartInside;
			if (!flag)
			{
				bool flag2 = eventTime == 1;
				if (flag2)
				{
					if (physicStateChange != 1)
					{
						if (physicStateChange == 2)
						{
							this.item.rb.isKinematic = true;
							this.joint.connectedBody = null;
							this.item.transform.SetParent(this.creature.ragdoll.targetPart.bone.animation);
							this.item.transform.localPosition = new Vector3(-0.06f, -0.04f, 0f);
							this.item.transform.localRotation = Quaternion.Euler(-120f, 180f, 0f);
						}
					}
					else
					{
						base.transform.SetParent(this.creature.ragdoll.targetPart.transform);
						this.item.transform.localPosition = new Vector3(-0.06f, -0.04f, 0f);
						this.item.transform.localRotation = Quaternion.Euler(-120f, 180f, 0f);
						this.joint.connectedBody = this.creature.ragdoll.targetPart.rb;
						base.transform.SetParent(null);
						this.item.rb.isKinematic = false;
					}
				}
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002646 File Offset: 0x00000846
		private void Item_OnDespawnEvent(EventTime eventTime)
		{
			Object.Destroy(this.joint);
			Object.Destroy(this);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x0000265C File Offset: 0x0000085C
		private void OnJointBreak(float breakForce)
		{
			this.RipOutHeart();
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002668 File Offset: 0x00000868
		private void Item_OnGrabEvent(Handle handle, RagdollHand ragdollHand)
		{
			bool flag = !this.joint;
			if (!flag)
			{
				this.joint.breakForce = 333f * HeartbreakerLevelModule.local.minRipVelocity;
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000026A8 File Offset: 0x000008A8
		private void Item_OnUngrabEvent(Handle handle, RagdollHand ragdollHand, bool throwing)
		{
			bool flag = !this.joint;
			if (!flag)
			{
				this.joint.breakForce = float.PositiveInfinity;
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000026DC File Offset: 0x000008DC
		private void MainCollisionHandler_OnCollisionStartEvent(CollisionInstance collisionInstance)
		{
			bool flag = this.isHeartInside && collisionInstance.damageStruct.penetrationJoint;
			if (flag)
			{
				Object.Destroy(collisionInstance.damageStruct.penetrationJoint);
			}
			bool ignoreHeartCollisions = HeartbreakerLevelModule.local.ignoreHeartCollisions;
			if (!ignoreHeartCollisions)
			{
				ColliderGroup sourceColliderGroup = collisionInstance.sourceColliderGroup;
				bool flag2 = ((sourceColliderGroup != null) ? sourceColliderGroup.collisionHandler.item : null);
				if (flag2)
				{
					ColliderGroup sourceColliderGroup2 = collisionInstance.sourceColliderGroup;
					List<RagdollHand> list;
					if (sourceColliderGroup2 == null)
					{
						list = null;
					}
					else
					{
						Item item = sourceColliderGroup2.collisionHandler.item;
						list = ((item != null) ? item.handlers : null);
					}
					foreach (RagdollHand hand in list)
					{
						bool flag3 = hand.creature == this.creature;
						if (flag3)
						{
							return;
						}
					}
				}
				bool flag4 = collisionInstance.damageStruct.damageType == 2;
				if (flag4)
				{
					this.HeartDeath(HeartDeathType.Slash, collisionInstance);
				}
				else
				{
					bool flag5 = collisionInstance.damageStruct.damageType == 1;
					if (flag5)
					{
						this.HeartDeath(HeartDeathType.Pierce, collisionInstance);
					}
				}
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002804 File Offset: 0x00000A04
		public void UpdateHeartBeat()
		{
			bool flag = this.creature.state == 0;
			if (!flag)
			{
				bool flag2 = this.item.handlers.Count != 1 || !this.item.handlers[0];
				if (!flag2)
				{
					RagdollHand hand = this.item.handlers[0];
					float speed = 9.424778f;
					bool flag3 = Mathf.Sin(this.time * speed) >= 0.95f;
					if (flag3)
					{
						PlayerControl.GetHand(hand.side).HapticShort(1f);
					}
					bool flag4 = Mathf.Sin(this.time * speed - 1f) >= 0.95f;
					if (flag4)
					{
						PlayerControl.GetHand(hand.side).HapticShort(0.5f);
					}
				}
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000028E8 File Offset: 0x00000AE8
		public void HeartDeath(HeartDeathType type, CollisionInstance collisionInstance = null)
		{
			bool flag = this.creature.state > 0;
			if (flag)
			{
				Debug.Log("[Heartbreaker] death by " + type.ToString());
				bool flag2 = collisionInstance == null;
				if (flag2)
				{
					DamageStruct damageStruct;
					damageStruct..ctor(4, 99999f);
					HeartDeathType heartDeathType = type;
					HeartDeathType heartDeathType2 = heartDeathType;
					if (heartDeathType2 > HeartDeathType.Slash)
					{
						if (heartDeathType2 == HeartDeathType.Squash)
						{
							damageStruct.damageType = 3;
						}
					}
					else
					{
						damageStruct.damageType = 1;
					}
					damageStruct.penetration = 1;
					collisionInstance = new CollisionInstance(damageStruct, null, null)
					{
						targetColliderGroup = this.creature.ragdoll.targetPart.colliderGroup
					};
					this.creature.lastInteractionCreature = Player.local.creature;
				}
				collisionInstance.damageStruct.hitRagdollPart = this.creature.ragdoll.targetPart;
				this.creature.Kill(collisionInstance);
				bool flag3 = !this.isHeartInside && HeartBehaviour.bleedingEffect != null;
				if (flag3)
				{
					EffectInstance effectInstance = HeartBehaviour.bleedingEffect.Spawn(this.item.transform, true, null, false, Array.Empty<Type>());
					effectInstance.Play(0, false);
				}
			}
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002A18 File Offset: 0x00000C18
		private void RipOutHeart()
		{
			this.isHeartInside = false;
			this.item.disallowDespawn = false;
			this.item.disallowRoomDespawn = false;
			foreach (Handle handle in this.item.handles)
			{
				handle.data.allowTelekinesis = true;
				handle.touchRadius = 0.1f;
			}
			this.item.RefreshAllowTelekinesis();
			bool flag = HeartBehaviour.ripOutEffect != null;
			if (flag)
			{
				List<Type> ignoredEffects = new List<Type>();
				bool noSoundForRippingOutHeart = HeartbreakerLevelModule.local.noSoundForRippingOutHeart;
				if (noSoundForRippingOutHeart)
				{
					ignoredEffects.Add(typeof(EffectModuleAudio));
				}
				EffectInstance effectInstance = HeartBehaviour.ripOutEffect.Spawn(this.item.transform, true, null, false, ignoredEffects.ToArray());
				effectInstance.Play(0, false);
			}
			bool flag2 = !HeartbreakerLevelModule.local.surviveHeartRippedOut;
			if (flag2)
			{
				this.HeartDeath(HeartDeathType.Rip, null);
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002B2C File Offset: 0x00000D2C
		public void UpdateSquashHeart()
		{
			bool flag = this.item.handlers.Count == 1 && this.item.handlers[0];
			if (flag)
			{
				RagdollHand hand = this.item.handlers[0];
				float gripForce = PlayerControl.GetHand(hand.side).gripAxis;
				float size = this.item.transform.localScale.x;
				float sizeNew = Mathf.Lerp(size, size * 0.8f, gripForce);
				this.item.transform.localScale = new Vector3(size, sizeNew, sizeNew);
				bool flag2 = gripForce >= HeartbreakerLevelModule.local.minGripIntensity;
				if (flag2)
				{
					bool flag3 = HeartBehaviour.squeezeEffect != null && this.squeezeEffectInstance == null;
					if (flag3)
					{
						this.squeezeEffectInstance = HeartBehaviour.squeezeEffect.Spawn(base.transform, true, null, false, Array.Empty<Type>());
						this.squeezeEffectInstance.Play(0, false);
					}
					bool flag4 = this.timeSqueezeStart <= 0f;
					if (flag4)
					{
						this.timeSqueezeStart = this.time;
					}
					else
					{
						bool flag5 = this.time - this.timeSqueezeStart >= HeartbreakerLevelModule.local.timeTillSqueezedHeartExplodes;
						if (flag5)
						{
							bool flag6 = HeartBehaviour.squashEffect != null;
							if (flag6)
							{
								EffectInstance effectInstance = HeartBehaviour.squashEffect.Spawn(this.item.transform.position, this.item.transform.rotation, null, null, true, null, false, Array.Empty<Type>());
								effectInstance.Play(0, false);
							}
							this.HeartDeath(HeartDeathType.Squash, null);
							this.item.Despawn();
						}
					}
					return;
				}
				bool flag7 = this.timeSqueezeStart > 0f;
				if (flag7)
				{
					this.timeSqueezeStart += Time.deltaTime;
				}
			}
			bool flag8 = this.squeezeEffectInstance != null;
			if (flag8)
			{
				this.squeezeEffectInstance.End(false, -1f);
				this.squeezeEffectInstance = null;
			}
		}

		// Token: 0x04000001 RID: 1
		public static EffectData squashEffect;

		// Token: 0x04000002 RID: 2
		public static EffectData ripOutEffect;

		// Token: 0x04000003 RID: 3
		public static EffectData squeezeEffect;

		// Token: 0x04000004 RID: 4
		public static EffectData bleedingEffect;

		// Token: 0x04000005 RID: 5
		public Item item;

		// Token: 0x04000006 RID: 6
		public Creature creature;

		// Token: 0x04000007 RID: 7
		private bool isHeartInside = true;

		// Token: 0x04000008 RID: 8
		private FixedJoint joint;

		// Token: 0x04000009 RID: 9
		private float time = 0f;

		// Token: 0x0400000A RID: 10
		private float timeSqueezeStart = 0f;

		// Token: 0x0400000B RID: 11
		private EffectInstance squeezeEffectInstance;
	}
}
