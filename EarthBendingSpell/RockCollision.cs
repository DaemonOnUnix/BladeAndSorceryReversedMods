using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ThunderRoad;
using UnityEngine;

namespace EarthBendingSpell
{
	// Token: 0x0200000A RID: 10
	public class RockCollision : MonoBehaviour
	{
		// Token: 0x06000036 RID: 54 RVA: 0x000043CC File Offset: 0x000025CC
		private void OnParticleCollision(GameObject other)
		{
			int num = ParticlePhysicsExtensions.GetCollisionEvents(this.part, other, this.collisionEvents);
			foreach (ParticleCollisionEvent particleCollisionEvent in this.collisionEvents)
			{
				EffectInstance effectInstance = this.rockCollisionEffectData.Spawn(particleCollisionEvent.intersection, Quaternion.identity, null, null, true, null, false, Array.Empty<Type>());
				effectInstance.Play(0, false);
				foreach (Collider collider in Physics.OverlapSphere(particleCollisionEvent.intersection, this.rockExplosionRadius))
				{
					bool flag = collider.attachedRigidbody;
					if (flag)
					{
						bool flag2 = collider.GetComponentInParent<Creature>();
						if (flag2)
						{
							Creature componentInParent = collider.GetComponentInParent<Creature>();
							bool flag3 = componentInParent != Player.currentCreature;
							if (flag3)
							{
								bool flag4 = componentInParent.state == 2;
								if (flag4)
								{
									componentInParent.ragdoll.SetState(1, false);
								}
								base.StartCoroutine(this.<OnParticleCollision>g__AddForceCoroutine|5_0(collider.attachedRigidbody, particleCollisionEvent.intersection));
							}
						}
						else
						{
							bool flag5 = collider.GetComponentInParent<Item>();
							if (flag5)
							{
								Item componentInParent2 = collider.GetComponentInParent<Item>();
								bool flag6 = componentInParent2.mainHandler;
								if (flag6)
								{
									bool flag7 = componentInParent2.mainHandler.creature != Player.currentCreature;
									if (flag7)
									{
										base.StartCoroutine(this.<OnParticleCollision>g__AddForceCoroutine|5_0(collider.attachedRigidbody, particleCollisionEvent.intersection));
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000045AC File Offset: 0x000027AC
		[CompilerGenerated]
		private IEnumerator <OnParticleCollision>g__AddForceCoroutine|5_0(Rigidbody rb, Vector3 expPos)
		{
			yield return new WaitForEndOfFrame();
			rb.AddExplosionForce(this.rockExplosionForce, expPos, this.rockExplosionForce, 1f, 1);
			yield break;
		}

		// Token: 0x04000088 RID: 136
		public EffectData rockCollisionEffectData;

		// Token: 0x04000089 RID: 137
		public ParticleSystem part;

		// Token: 0x0400008A RID: 138
		public List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

		// Token: 0x0400008B RID: 139
		public float rockExplosionRadius;

		// Token: 0x0400008C RID: 140
		public float rockExplosionForce;
	}
}
