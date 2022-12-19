using System;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

namespace UltimateFantasyPack
{
	// Token: 0x0200002B RID: 43
	public class MeteorCollision : MonoBehaviour
	{
		// Token: 0x060000AE RID: 174 RVA: 0x0000535C File Offset: 0x0000355C
		public void Start()
		{
			this.part = base.GetComponent<ParticleSystem>();
			this.collisionEvents = new List<ParticleCollisionEvent>();
			this.damageStruct = new DamageStruct(4, 50f);
			this.effect = Catalog.GetData<EffectData>("SpellGravityShockwave", true);
			this.effect2 = Catalog.GetData<EffectData>("ImbueGravityRagdoll", true);
		}

		// Token: 0x060000AF RID: 175 RVA: 0x000053B4 File Offset: 0x000035B4
		private void OnParticleCollision(GameObject other)
		{
			Debug.Log("Collision Added");
			int numCollisionEvents = ParticlePhysicsExtensions.GetCollisionEvents(this.part, other, this.collisionEvents);
			this.effect.Spawn(this.part.transform, true, null, false, Array.Empty<Type>()).Play(0, false);
			for (int i = 0; i < numCollisionEvents; i++)
			{
				foreach (Collider collider in Physics.OverlapSphere(this.part.transform.position, 10f))
				{
					bool flag = collider.attachedRigidbody != Player.local.locomotion.rb && collider.attachedRigidbody != VoidMono.item.rb;
					if (flag)
					{
						Rigidbody attachedRigidbody = collider.attachedRigidbody;
						if (attachedRigidbody != null)
						{
							attachedRigidbody.AddForce((collider.transform.position - VoidMono.item.transform.position).normalized * this.blastForce, 1);
						}
					}
					Creature creature = collider.gameObject.GetComponentInParent<Creature>();
					bool flag2 = creature != null && !creature.isPlayer;
					if (flag2)
					{
						bool flag3 = !creature.isKilled;
						if (flag3)
						{
							creature.ragdoll.SetState(1);
						}
						creature.Kill();
					}
				}
			}
		}

		// Token: 0x0400007B RID: 123
		public ParticleSystem part;

		// Token: 0x0400007C RID: 124
		public List<ParticleCollisionEvent> collisionEvents;

		// Token: 0x0400007D RID: 125
		private EffectData effect;

		// Token: 0x0400007E RID: 126
		private EffectData effect2;

		// Token: 0x0400007F RID: 127
		private DamageStruct damageStruct;

		// Token: 0x04000080 RID: 128
		public float blastForce = 20f;
	}
}
