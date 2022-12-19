using System;
using System.Collections;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

namespace MeteorFall
{
	// Token: 0x02000005 RID: 5
	public class MeteorComponent : MonoBehaviour
	{
		// Token: 0x06000008 RID: 8 RVA: 0x00002177 File Offset: 0x00000377
		public void Awake()
		{
			this.item = base.GetComponent<Item>();
			this.item.mainCollisionHandler.OnCollisionStartEvent += new CollisionHandler.CollisionEvent(this.MainCollisionHandler_OnCollisionStartEvent);
			this.item.disallowDespawn = true;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000021B0 File Offset: 0x000003B0
		private void MainCollisionHandler_OnCollisionStartEvent(CollisionInstance collisionInstance)
		{
			base.StartCoroutine(this.Impact(collisionInstance.contactPoint, collisionInstance.contactNormal, collisionInstance.sourceColliderGroup.transform.up));
			this.item.Hide(true);
			this.item.colliderGroups[0].colliders[0].isTrigger = true;
			this.item.rb.velocity = Vector3.zero;
			this.item.rb.Sleep();
			this.item.disallowDespawn = false;
			this.item.mainCollisionHandler.OnCollisionStartEvent -= new CollisionHandler.CollisionEvent(this.MainCollisionHandler_OnCollisionStartEvent);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002267 File Offset: 0x00000467
		private IEnumerator Impact(Vector3 contactPoint, Vector3 contactNormal, Vector3 contactNormalUpward)
		{
			EffectInstance effectInstance = Catalog.GetData<EffectData>("MeteorShockwave", true).Spawn(contactPoint, Quaternion.LookRotation(-contactNormal, contactNormalUpward), null, null, true, null, false, Array.Empty<Type>());
			effectInstance.SetIntensity(100f);
			effectInstance.Play(0, false);
			Collider[] sphereContacts = Physics.OverlapSphere(contactPoint, 100f, 218119169);
			List<Creature> creaturesPushed = new List<Creature>();
			List<Rigidbody> rigidbodiesPushed = new List<Rigidbody>();
			rigidbodiesPushed.Add(this.item.rb);
			creaturesPushed.Add(Player.local.creature);
			float waveDistance = 0f;
			yield return new WaitForEndOfFrame();
			while (waveDistance < 100f)
			{
				waveDistance += 7f;
				foreach (Creature creature in Creature.allActive)
				{
					bool flag = !creature.isPlayer && !creature.isKilled && Vector3.Distance(contactPoint, creature.transform.position) < waveDistance && !creaturesPushed.Contains(creature);
					if (flag)
					{
						CollisionInstance collision = new CollisionInstance(new DamageStruct(4, 200f - Vector3.Distance(contactPoint, creature.transform.position) * 2f), null, null);
						collision.damageStruct.hitRagdollPart = creature.ragdoll.rootPart;
						creature.Damage(collision);
						creature.ragdoll.SetState(1);
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
						bool flag2 = @object != null;
						if (flag2)
						{
							creature.lastInteractionTime = Time.time;
							creature.lastInteractionCreature = this.item.lastHandler.creature;
						}
						creature.gameObject.AddComponent<MeteorBurning>();
						creaturesPushed.Add(creature);
						collision = null;
					}
					creature = null;
				}
				List<Creature>.Enumerator enumerator = default(List<Creature>.Enumerator);
				foreach (Collider collider in sphereContacts)
				{
					bool flag3 = collider.attachedRigidbody != null && !collider.attachedRigidbody.isKinematic && Vector3.Distance(contactPoint, collider.transform.position) < waveDistance;
					if (flag3)
					{
						bool flag4 = collider.attachedRigidbody.gameObject.layer != GameManager.GetLayer(10) && !rigidbodiesPushed.Contains(collider.attachedRigidbody);
						if (flag4)
						{
							collider.attachedRigidbody.AddExplosionForce(75f, contactPoint, 100f, 0.5f, 2);
							rigidbodiesPushed.Add(collider.attachedRigidbody);
						}
					}
					collider = null;
				}
				Collider[] array = null;
				yield return new WaitForSeconds(0.05f);
			}
			this.item.Despawn();
			yield break;
		}

		// Token: 0x04000002 RID: 2
		private Item item;
	}
}
