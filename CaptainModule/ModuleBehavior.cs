using System;
using System.Collections;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

namespace CaptainModule
{
	// Token: 0x02000003 RID: 3
	public class ModuleBehavior : MonoBehaviour
	{
		// Token: 0x06000003 RID: 3 RVA: 0x00002078 File Offset: 0x00000278
		private void Start()
		{
			this.item = base.GetComponent<Item>();
			this.item.OnUngrabEvent += new Item.ReleaseDelegate(this.OnUnGrab);
			this.item.mainCollisionHandler.OnCollisionStartEvent += new CollisionHandler.CollisionEvent(this.OnCollision);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020C8 File Offset: 0x000002C8
		private void OnCollision(CollisionInstance collisioninstance)
		{
			bool flag;
			if (this.currentBounces < this.Data.MaximumBounces && !this.Attacking && this.Thrown)
			{
				ColliderGroup targetColliderGroup = collisioninstance.targetColliderGroup;
				Object @object;
				if (targetColliderGroup == null)
				{
					@object = null;
				}
				else
				{
					CollisionHandler collisionHandler = targetColliderGroup.collisionHandler;
					@object = ((collisionHandler != null) ? collisionHandler.rb : null);
				}
				flag = @object != null;
			}
			else
			{
				flag = true;
			}
			bool flag2 = flag;
			if (!flag2)
			{
				List<Creature> list = new List<Creature>();
				foreach (Collider collider in Physics.OverlapSphere(base.transform.position, this.Data.DetectionRadius))
				{
					RagdollPart ragdollPart;
					bool flag3 = collider.attachedRigidbody != null && collider.attachedRigidbody.TryGetComponent<RagdollPart>(ref ragdollPart);
					if (flag3)
					{
						bool flag4 = !ragdollPart.ragdoll.creature.isPlayer && !list.Contains(ragdollPart.ragdoll.creature);
						if (flag4)
						{
							list.Add(ragdollPart.ragdoll.creature);
						}
					}
				}
				bool flag5 = list.Count > 0;
				if (flag5)
				{
					Creature creature = list[0];
					Vector3 normalized = (creature.ragdoll.GetPart(4).transform.position - base.transform.position).normalized;
					this.item.transform.rotation = Quaternion.LookRotation(normalized);
					this.item.rb.AddForce(normalized * this.Data.AttackForce, 1);
				}
				else
				{
					Vector3 vector = Vector3.Reflect(this.item.rb.velocity, collisioninstance.contactNormal);
					this.item.transform.rotation = Quaternion.LookRotation(vector);
					this.item.rb.AddForce(vector * this.Data.BounceForce, 1);
					this.currentBounces++;
				}
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000022C0 File Offset: 0x000004C0
		private void Update()
		{
			bool flag = this.item.handlers.Count > 0;
			if (flag)
			{
				this.bindedHand = this.item.handlers[0].playerHand;
				this.currentBounces = 0;
			}
			bool flag2 = this.bindedHand != null && this.bindedHand.controlHand.usePressed && this.bindedHand.ragdollHand.caster.telekinesis.catchedHandle == null && this.bindedHand.ragdollHand.grabbedHandle == null;
			if (flag2)
			{
				bool flag3 = Vector3.Distance(this.bindedHand.ragdollHand.transform.position, base.transform.position) < this.Data.GrabDistance;
				if (flag3)
				{
					this.item.rb.velocity = Vector3.zero;
					this.item.transform.position = this.bindedHand.ragdollHand.transform.position;
					this.bindedHand.ragdollHand.Grab(this.item.handles[0]);
				}
				bool flag4 = !this.Returning;
				if (flag4)
				{
					base.StartCoroutine(this.DisableCol());
					base.StartCoroutine(this.InitRotation());
				}
				Vector3 normalized = (this.bindedHand.ragdollHand.transform.position - base.transform.position).normalized;
				this.item.rb.AddForce(normalized * this.Data.ReturnForce, 1);
			}
			else
			{
				this.Returning = false;
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x0000248A File Offset: 0x0000068A
		private IEnumerator InitRotation()
		{
			this.Returning = true;
			while (this.Returning)
			{
				base.transform.Rotate(this.Data.RotationSpeed * Time.deltaTime);
				yield return null;
			}
			yield break;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002499 File Offset: 0x00000699
		private IEnumerator DisableCol()
		{
			foreach (ColliderGroup colliderGroup in this.item.colliderGroups)
			{
				foreach (Collider collider in colliderGroup.colliders)
				{
					collider.enabled = false;
					collider = null;
				}
				List<Collider>.Enumerator enumerator2 = default(List<Collider>.Enumerator);
				colliderGroup = null;
			}
			List<ColliderGroup>.Enumerator enumerator = default(List<ColliderGroup>.Enumerator);
			while (this.item.handlers.Count < 0)
			{
				yield return null;
			}
			foreach (ColliderGroup colliderGroup2 in this.item.colliderGroups)
			{
				foreach (Collider collider2 in colliderGroup2.colliders)
				{
					collider2.enabled = true;
					collider2 = null;
				}
				List<Collider>.Enumerator enumerator4 = default(List<Collider>.Enumerator);
				colliderGroup2 = null;
			}
			List<ColliderGroup>.Enumerator enumerator3 = default(List<ColliderGroup>.Enumerator);
			yield break;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000024A8 File Offset: 0x000006A8
		private void OnUnGrab(Handle handle, RagdollHand ragdollhand, bool throwing)
		{
			this.Thrown = throwing;
		}

		// Token: 0x04000008 RID: 8
		public LoadModule Data;

		// Token: 0x04000009 RID: 9
		private Item item;

		// Token: 0x0400000A RID: 10
		private PlayerHand bindedHand;

		// Token: 0x0400000B RID: 11
		private bool Thrown = false;

		// Token: 0x0400000C RID: 12
		private bool Returning = false;

		// Token: 0x0400000D RID: 13
		private bool Attacking = false;

		// Token: 0x0400000E RID: 14
		private int currentBounces;
	}
}
