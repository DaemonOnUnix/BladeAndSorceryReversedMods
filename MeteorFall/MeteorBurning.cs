using System;
using ThunderRoad;
using UnityEngine;

namespace MeteorFall
{
	// Token: 0x02000006 RID: 6
	public class MeteorBurning : MonoBehaviour
	{
		// Token: 0x0600000C RID: 12 RVA: 0x00002294 File Offset: 0x00000494
		public void Start()
		{
			this.creature = base.GetComponent<Creature>();
			this.instance = Catalog.GetData<EffectData>("MeteorRagdoll", true).Spawn(this.creature.ragdoll.rootPart.transform, true, null, false, Array.Empty<Type>());
			this.instance.SetRenderer(this.creature.GetRendererForVFX(), false);
			this.instance.SetIntensity(1f);
			this.instance.Play(0, false);
			this.timer = Time.time;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002324 File Offset: 0x00000524
		public void FixedUpdate()
		{
			bool flag = Time.time - this.timer >= 10f;
			if (flag)
			{
				this.instance.Stop(0);
				Object.Destroy(this);
			}
			else
			{
				bool flag2 = !this.creature.isKilled;
				if (flag2)
				{
					CollisionInstance collisionInstance = new CollisionInstance(new DamageStruct(4, 5f * Time.fixedDeltaTime), null, null);
					collisionInstance.damageStruct.hitRagdollPart = this.creature.ragdoll.rootPart;
					this.creature.Damage(collisionInstance);
				}
			}
		}

		// Token: 0x04000003 RID: 3
		private Creature creature;

		// Token: 0x04000004 RID: 4
		private EffectInstance instance;

		// Token: 0x04000005 RID: 5
		private float timer;
	}
}
