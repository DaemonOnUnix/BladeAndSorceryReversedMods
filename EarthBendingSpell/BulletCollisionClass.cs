using System;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

namespace EarthBendingSpell
{
	// Token: 0x02000008 RID: 8
	public class BulletCollisionClass : MonoBehaviour
	{
		// Token: 0x0600002F RID: 47 RVA: 0x00004088 File Offset: 0x00002288
		private void OnParticleCollision(GameObject other)
		{
			int num = ParticlePhysicsExtensions.GetCollisionEvents(this.part, other, this.collisionEvents);
			foreach (ParticleCollisionEvent particleCollisionEvent in this.collisionEvents)
			{
				this.bulletColData.Spawn(particleCollisionEvent.intersection, Quaternion.identity, null, null, true, null, false, Array.Empty<Type>()).Play(0, false);
				Collider[] array = Physics.OverlapSphere(particleCollisionEvent.intersection, 0.2f);
				int i = 0;
				while (i < array.Length)
				{
					Collider collider = array[i];
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
								bool flag4 = componentInParent.state > 0;
								if (flag4)
								{
									componentInParent.TryElectrocute(10f, 12f, true, false, null);
									CollisionInstance collisionInstance = new CollisionInstance(new DamageStruct(1, 0.5f), null, null);
									componentInParent.Damage(collisionInstance);
								}
							}
						}
					}
					IL_10A:
					i++;
					continue;
					goto IL_10A;
				}
			}
		}

		// Token: 0x0400007C RID: 124
		public ParticleSystem part;

		// Token: 0x0400007D RID: 125
		public List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

		// Token: 0x0400007E RID: 126
		public EffectData bulletColData;
	}
}
