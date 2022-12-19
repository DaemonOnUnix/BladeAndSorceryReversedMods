using System;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

namespace DynamicModule.Modules
{
	// Token: 0x02000006 RID: 6
	public class GroundPoundModule : MonoBehaviour
	{
		// Token: 0x0600000A RID: 10 RVA: 0x000022A0 File Offset: 0x000004A0
		private void Start()
		{
			this.Weapon.Item.mainCollisionHandler.OnCollisionStartEvent += new CollisionHandler.CollisionEvent(this.OnCollision);
			this.threshHold = this.Weapon.Data.GroundPoundActivationForce * this.Weapon.Data.GroundPoundActivationForce;
			bool flag = !string.IsNullOrEmpty(this.Weapon.Data.GroundPoundVFXRef);
			if (flag)
			{
				this.ps = this.Weapon.Item.GetCustomReference(this.Weapon.Data.GroundPoundVFXRef).GetComponent<ParticleSystem>();
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002340 File Offset: 0x00000540
		private void OnCollision(CollisionInstance collisionInstance)
		{
			bool flag;
			if (!this.isCooldown && !this.Weapon.Data.ColliderTriggerNames.Contains(collisionInstance.sourceCollider.name))
			{
				if (collisionInstance.impactVelocity.sqrMagnitude <= this.threshHold)
				{
					ColliderGroup targetColliderGroup = collisionInstance.targetColliderGroup;
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
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			if (!flag2)
			{
				bool flag3 = this.ps != null;
				if (flag3)
				{
					this.ps.Play();
				}
				List<Creature> list = new List<Creature>();
				Collider[] array = Physics.OverlapSphere(base.transform.position, this.Weapon.Data.GroundPoundRadius);
				int i = 0;
				while (i < array.Length)
				{
					Collider collider = array[i];
					RagdollPart ragdollPart;
					bool flag4 = collider.attachedRigidbody != null && collider.attachedRigidbody.TryGetComponent<RagdollPart>(ref ragdollPart);
					if (flag4)
					{
						Creature creature = ragdollPart.ragdoll.creature;
						bool flag5 = creature.isPlayer || list.Contains(creature);
						if (!flag5)
						{
							Vector3 normalized = (creature.transform.position - base.transform.position).normalized;
							creature.TryPush(0, normalized, this.Weapon.Data.GroundPoundPushLevel, 0);
							list.Add(creature);
							this.timer = this.Weapon.Data.GroundPoundCooldown;
						}
					}
					IL_17C:
					i++;
					continue;
					goto IL_17C;
				}
				this.isCooldown = true;
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000024E0 File Offset: 0x000006E0
		private void Update()
		{
			bool flag = this.isCooldown;
			if (flag)
			{
				this.timer -= Time.deltaTime;
				bool flag2 = this.timer <= 0f;
				if (flag2)
				{
					this.isCooldown = false;
				}
			}
		}

		// Token: 0x04000019 RID: 25
		public InitiateBehavior Weapon;

		// Token: 0x0400001A RID: 26
		private float threshHold;

		// Token: 0x0400001B RID: 27
		private float timer = 0f;

		// Token: 0x0400001C RID: 28
		private bool isCooldown = false;

		// Token: 0x0400001D RID: 29
		private ParticleSystem ps;
	}
}
