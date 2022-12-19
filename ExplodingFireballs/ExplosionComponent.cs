using System;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

namespace ExplodingFireballs
{
	// Token: 0x02000004 RID: 4
	public class ExplosionComponent : MonoBehaviour
	{
		// Token: 0x06000005 RID: 5 RVA: 0x00002111 File Offset: 0x00000311
		public void Start()
		{
			this.item = base.GetComponent<Item>();
			this.item.mainCollisionHandler.OnCollisionStartEvent += new CollisionHandler.CollisionEvent(this.MainCollisionHandler_OnCollisionStartEvent);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x0000213D File Offset: 0x0000033D
		public void Setup(float radius, float force, float damage, string effect)
		{
			this.explosionRadius = radius;
			this.explosionForce = force;
			this.explosionDamage = damage;
			this.explosionEffectId = effect;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x0000215D File Offset: 0x0000035D
		private void MainCollisionHandler_OnCollisionStartEvent(CollisionInstance collisionInstance)
		{
			this.Impact(collisionInstance.contactPoint, collisionInstance.contactNormal, collisionInstance.sourceColliderGroup.transform.up);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002184 File Offset: 0x00000384
		private void Impact(Vector3 contactPoint, Vector3 contactNormal, Vector3 contactNormalUpward)
		{
			EffectInstance effectInstance = Catalog.GetData<EffectData>(this.explosionEffectId, true).Spawn(contactPoint, Quaternion.LookRotation(-contactNormal, contactNormalUpward), null, null, true, null, false, Array.Empty<Type>());
			effectInstance.SetIntensity(1f);
			effectInstance.Play(0, false);
			Collider[] array = Physics.OverlapSphere(contactPoint, this.explosionRadius, 232799233);
			List<Creature> list = new List<Creature>();
			List<Rigidbody> list2 = new List<Rigidbody>();
			list2.Add(this.item.rb);
			list.Add(Player.local.creature);
			foreach (Creature creature in Creature.allActive)
			{
				bool flag = !creature.isPlayer && !creature.isKilled && Vector3.Distance(contactPoint, creature.transform.position) < this.explosionRadius && !list.Contains(creature);
				if (flag)
				{
					CollisionInstance collisionInstance = new CollisionInstance(new DamageStruct(4, this.explosionDamage), null, null);
					collisionInstance.damageStruct.hitRagdollPart = creature.ragdoll.rootPart;
					creature.Damage(collisionInstance);
					creature.ragdoll.SetState(1);
					BurnComponent component = this.item.GetComponent<BurnComponent>();
					bool flag2 = component != null && creature.GetComponent<Burning>() == null;
					if (flag2)
					{
						creature.gameObject.AddComponent<Burning>().Setup(component.burnDamagePerSecond, component.burnDuration, component.burnEffectId);
					}
					Item item = this.item;
					Object @object;
					if (item == null)
					{
						@object = null;
					}
					else
					{
						RagdollHand lastHandler = item.lastHandler;
						@object = ((lastHandler != null) ? lastHandler.creature : null);
					}
					bool flag3 = @object != null;
					if (flag3)
					{
						creature.lastInteractionTime = Time.time;
						creature.lastInteractionCreature = this.item.lastHandler.creature;
					}
					list.Add(creature);
				}
			}
			foreach (Collider collider in array)
			{
				bool flag4 = collider.attachedRigidbody != null && !collider.attachedRigidbody.isKinematic && Vector3.Distance(contactPoint, collider.transform.position) < this.explosionRadius;
				if (flag4)
				{
					bool flag5 = collider.attachedRigidbody.gameObject.layer != GameManager.GetLayer(10) && !list2.Contains(collider.attachedRigidbody);
					if (flag5)
					{
						collider.attachedRigidbody.AddExplosionForce(this.explosionForce, contactPoint, this.explosionRadius, 0.5f, 2);
						list2.Add(collider.attachedRigidbody);
					}
				}
			}
		}

		// Token: 0x04000008 RID: 8
		private Item item;

		// Token: 0x04000009 RID: 9
		private float explosionRadius;

		// Token: 0x0400000A RID: 10
		private float explosionForce;

		// Token: 0x0400000B RID: 11
		private float explosionDamage;

		// Token: 0x0400000C RID: 12
		private string explosionEffectId;
	}
}
