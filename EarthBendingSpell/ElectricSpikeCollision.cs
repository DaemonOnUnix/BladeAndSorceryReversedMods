using System;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

namespace EarthBendingSpell
{
	// Token: 0x0200000D RID: 13
	public class ElectricSpikeCollision : MonoBehaviour
	{
		// Token: 0x06000044 RID: 68 RVA: 0x00004A00 File Offset: 0x00002C00
		private void OnParticleCollision(GameObject other)
		{
			int num = ParticlePhysicsExtensions.GetCollisionEvents(this.part, other, this.collisionEvents);
			foreach (ParticleCollisionEvent particleCollisionEvent in this.collisionEvents)
			{
				EffectInstance effectInstance = this.spikesCollisionEffectData.Spawn(particleCollisionEvent.intersection, Quaternion.identity, null, null, true, null, false, Array.Empty<Type>());
				effectInstance.Play(0, false);
				Collider[] array = Physics.OverlapSphere(particleCollisionEvent.intersection, 1f);
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
									componentInParent.Damage(new CollisionInstance(new DamageStruct(4, 5f), null, null));
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

		// Token: 0x0400009D RID: 157
		public EffectData spikesCollisionEffectData;

		// Token: 0x0400009E RID: 158
		public ParticleSystem part;

		// Token: 0x0400009F RID: 159
		public List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
	}
}
