using System;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

namespace EarthBendingSpell
{
	// Token: 0x02000007 RID: 7
	public class SomeParticleCollisionDetectorClass : MonoBehaviour
	{
		// Token: 0x0600002D RID: 45 RVA: 0x00003F58 File Offset: 0x00002158
		private void OnParticleCollision(GameObject other)
		{
			foreach (ParticleCollisionEvent particleCollisionEvent in this.collisionEvents)
			{
				foreach (Collider collider in Physics.OverlapSphere(particleCollisionEvent.intersection, 0.2f))
				{
					bool flag = collider.attachedRigidbody;
					if (flag)
					{
						bool flag2 = collider.GetComponentInParent<Creature>();
						if (flag2)
						{
							Creature componentInParent = collider.GetComponentInParent<Creature>();
							componentInParent.brain.instance.GetModule<BrainModuleSpeak>(false).Play("death", true);
							bool flag3 = componentInParent != Player.currentCreature;
							if (flag3)
							{
								bool flag4 = componentInParent.state > 0;
								if (flag4)
								{
									CollisionInstance collisionInstance = new CollisionInstance(new DamageStruct(1, 0.5f), null, null);
									componentInParent.Damage(collisionInstance);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0400007B RID: 123
		public List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
	}
}
