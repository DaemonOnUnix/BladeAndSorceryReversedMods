using System;
using System.Collections;
using ThunderRoad;
using UnityEngine;

namespace Wully.Mono
{
	// Token: 0x0200000A RID: 10
	public class CorvoSword : MonoBehaviour
	{
		// Token: 0x06000046 RID: 70 RVA: 0x00004108 File Offset: 0x00002308
		public void Load()
		{
			this.item = base.gameObject.GetComponent<Item>();
			this.rb = this.item.GetComponent<Rigidbody>();
			this.animator = this.item.GetComponentInChildren<Animator>();
			this.bladeCollider = this.item.GetCustomReference("BladeCollider", true).gameObject;
			this.closedHiltCollider = this.item.GetCustomReference("ClosedHiltCollider", true).gameObject;
			this.openHiltCollider = this.item.GetCustomReference("OpenHiltCollider", true).gameObject;
			this.imbueMesh = this.item.GetCustomReference("ImbueMesh", true).gameObject;
			this.item.OnHeldActionEvent += new Item.HeldActionDelegate(this.Item_OnHeldActionEvent);
			this.item.OnDespawnEvent += new Item.SpawnEvent(this.Item_OnDespawnEvent);
			this.item.OnGrabEvent += new Item.GrabDelegate(this.Item_OnGrabEvent);
			this.unfoldData = Catalog.GetData<EffectData>("UnfoldBlade", true);
			this.foldData = Catalog.GetData<EffectData>("FoldBlade", true);
			this.animator.SetTrigger("Extend");
			this.animator.SetFloat("Speed", this.animationSpeed);
			this.isExtended = true;
			Vector3 tmp;
			tmp..ctor(0f, 1f, 1f);
			this.imbueMesh.transform.localScale = Vector3.one;
			this.bladeCollider.transform.localScale = Vector3.one;
			this.closedHiltCollider.transform.localScale = tmp;
			this.openHiltCollider.transform.localScale = Vector3.one;
			if (this.item.IsHanded(null))
			{
				this.item.handlers[0].poser.SetTargetPose(this.handleData, false, false, false, false, false);
				this.item.handlers[0].poser.SetDefaultPose(this.spinHandPoseData);
				this.item.handlers[0].poser.SetTargetWeight(0f);
			}
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00004327 File Offset: 0x00002527
		private void Item_OnGrabEvent(Handle handle, RagdollHand ragdollHand)
		{
			ragdollHand.poser.SetTargetPose(this.handleData, false, false, false, false, false);
			ragdollHand.poser.SetDefaultPose(this.spinHandPoseData);
			ragdollHand.poser.SetTargetWeight(1f);
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00004360 File Offset: 0x00002560
		private void Item_OnDespawnEvent(EventTime eventtime)
		{
			if (eventtime == 1)
			{
				return;
			}
			if (this.unfoldEffectInstance != null)
			{
				this.unfoldEffectInstance.Despawn();
			}
			if (this.foldEffectInstance != null)
			{
				this.foldEffectInstance.Despawn();
			}
		}

		// Token: 0x06000049 RID: 73 RVA: 0x0000438D File Offset: 0x0000258D
		private void Item_OnHeldActionEvent(RagdollHand ragdollHand, Handle handle, Interactable.Action action)
		{
			if (action == 2)
			{
				this.AffectBlade(ragdollHand, handle);
			}
		}

		// Token: 0x0600004A RID: 74 RVA: 0x0000439C File Offset: 0x0000259C
		private void AffectBlade(RagdollHand hand, Handle handle)
		{
			if (this.isExtended)
			{
				if (this.item.isPenetrating)
				{
					foreach (CollisionHandler collisionHandler in this.item.collisionHandlers)
					{
						foreach (Damager damager in collisionHandler.damagers)
						{
							damager.UnPenetrateAll();
						}
					}
				}
				if (this.item.imbues != null)
				{
					foreach (Imbue imbue in this.item.imbues)
					{
						if (imbue != null)
						{
							SpellCastCharge spellCastBase = imbue.spellCastBase;
							if (spellCastBase != null)
							{
								EffectInstance imbueEffect = spellCastBase.imbueEffect;
								if (imbueEffect != null)
								{
									imbueEffect.Stop(0);
								}
							}
						}
					}
				}
				this.animator.SetTrigger("Collapse");
				this.isExtended = false;
				if (this.foldEffectInstance != null)
				{
					this.foldEffectInstance.Stop(0);
				}
				if (this.foldData != null)
				{
					this.foldEffectInstance = this.foldData.Spawn(this.item.transform, true, null, false, Array.Empty<Type>());
					this.foldEffectInstance.Play(0, false);
				}
				this.startValue = this.endValue;
				this.endValue = 0f;
			}
			else
			{
				if (this.item.imbues != null)
				{
					foreach (Imbue imbue2 in this.item.imbues)
					{
						if (imbue2 != null)
						{
							SpellCastCharge spellCastBase2 = imbue2.spellCastBase;
							if (spellCastBase2 != null)
							{
								EffectInstance imbueEffect2 = spellCastBase2.imbueEffect;
								if (imbueEffect2 != null)
								{
									imbueEffect2.Play(0, false);
								}
							}
						}
					}
				}
				this.animator.SetTrigger("Extend");
				this.isExtended = true;
				if (this.unfoldEffectInstance != null)
				{
					this.unfoldEffectInstance.Stop(0);
				}
				if (this.unfoldData != null)
				{
					this.unfoldEffectInstance = this.unfoldData.Spawn(this.item.transform, true, null, false, Array.Empty<Type>());
					this.unfoldEffectInstance.Play(0, false);
				}
				this.startValue = this.endValue;
				this.endValue = 1f;
			}
			this.item.RefreshCollision(false);
			this.rb.ResetInertiaTensor();
			this.rb.ResetCenterOfMass();
			this.item.customCenterOfMass = this.rb.centerOfMass;
			this.item.useCustomCenterOfMass = true;
			if (this.poseHandCoroutine != null)
			{
				this.item.StopCoroutine(this.poseHandCoroutine);
			}
			this.poseHandCoroutine = this.item.StartCoroutine(this.PoseHand(hand));
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00004690 File Offset: 0x00002890
		private IEnumerator PoseHand(RagdollHand hand)
		{
			float timeElapsed = 0f;
			float duration = this.lerpDuration * 1f / this.animationSpeed;
			while (timeElapsed < duration)
			{
				this.valueToLerp = Mathf.Lerp(this.startValue, this.endValue, timeElapsed / duration);
				timeElapsed += Time.deltaTime;
				this.tmp.x = this.valueToLerp;
				this.tmp2.x = 1f - this.valueToLerp;
				this.bladeCollider.transform.localScale = this.tmp;
				this.imbueMesh.transform.localScale = this.tmp;
				this.closedHiltCollider.transform.localScale = this.tmp;
				this.openHiltCollider.transform.localScale = this.tmp2;
				float weight;
				if (this.valueToLerp <= 0.5f)
				{
					weight = 1f - this.valueToLerp * 2f;
				}
				else
				{
					weight = this.valueToLerp * 2f - 1f;
				}
				hand.poser.SetTargetWeight(weight);
				yield return null;
			}
			this.valueToLerp = this.endValue;
			hand.poser.SetTargetWeight(1f);
			yield break;
		}

		// Token: 0x04000058 RID: 88
		private Rigidbody rb;

		// Token: 0x04000059 RID: 89
		private Item item;

		// Token: 0x0400005A RID: 90
		private Animator animator;

		// Token: 0x0400005B RID: 91
		private bool isExtended;

		// Token: 0x0400005C RID: 92
		private GameObject bladeCollider;

		// Token: 0x0400005D RID: 93
		private GameObject closedHiltCollider;

		// Token: 0x0400005E RID: 94
		private GameObject openHiltCollider;

		// Token: 0x0400005F RID: 95
		private GameObject imbueMesh;

		// Token: 0x04000060 RID: 96
		public HandPoseData spinHandPoseData;

		// Token: 0x04000061 RID: 97
		public HandPoseData handleData;

		// Token: 0x04000062 RID: 98
		public float animationSpeed;

		// Token: 0x04000063 RID: 99
		protected EffectData unfoldData;

		// Token: 0x04000064 RID: 100
		protected EffectData foldData;

		// Token: 0x04000065 RID: 101
		private EffectInstance unfoldEffectInstance;

		// Token: 0x04000066 RID: 102
		private EffectInstance foldEffectInstance;

		// Token: 0x04000067 RID: 103
		private float lerpDuration = 1f;

		// Token: 0x04000068 RID: 104
		private float startValue;

		// Token: 0x04000069 RID: 105
		private float endValue = 1f;

		// Token: 0x0400006A RID: 106
		private float valueToLerp;

		// Token: 0x0400006B RID: 107
		private Coroutine poseHandCoroutine;

		// Token: 0x0400006C RID: 108
		private Vector3 tmp = new Vector3(1f, 1f, 1f);

		// Token: 0x0400006D RID: 109
		private Vector3 tmp2 = new Vector3(0f, 0f, 0f);
	}
}
