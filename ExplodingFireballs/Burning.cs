using System;
using ThunderRoad;
using UnityEngine;

namespace ExplodingFireballs
{
	// Token: 0x02000006 RID: 6
	public class Burning : MonoBehaviour
	{
		// Token: 0x0600000E RID: 14 RVA: 0x00002520 File Offset: 0x00000720
		public void Start()
		{
			this.creature = base.GetComponent<Creature>();
			this.instance = Catalog.GetData<EffectData>(this.burnEffectId, true).Spawn(this.creature.ragdoll.rootPart.transform, true, null, false, Array.Empty<Type>());
			this.instance.SetRenderer(this.creature.GetRendererForVFX(), false);
			this.instance.SetIntensity(1f);
			this.instance.Play(0, false);
			this.timer = Time.time;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000025B0 File Offset: 0x000007B0
		public void Setup(float dps, float duration, string effect)
		{
			this.burnDamagePerSecond = dps;
			this.burnDuration = duration;
			this.burnEffectId = effect;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000025C8 File Offset: 0x000007C8
		public void Update()
		{
			bool flag = Time.time - this.timer >= this.burnDuration;
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
					CollisionInstance collisionInstance = new CollisionInstance(new DamageStruct(4, this.burnDamagePerSecond * Time.deltaTime), null, null);
					collisionInstance.damageStruct.hitRagdollPart = this.creature.ragdoll.rootPart;
					this.creature.Damage(collisionInstance);
				}
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002660 File Offset: 0x00000860
		public void OnDestroy()
		{
			bool flag = this.instance != null && this.instance.isPlaying;
			if (flag)
			{
				this.instance.Stop(0);
			}
		}

		// Token: 0x04000011 RID: 17
		private Creature creature;

		// Token: 0x04000012 RID: 18
		private EffectInstance instance;

		// Token: 0x04000013 RID: 19
		private float timer;

		// Token: 0x04000014 RID: 20
		private float burnDamagePerSecond;

		// Token: 0x04000015 RID: 21
		private float burnDuration;

		// Token: 0x04000016 RID: 22
		private string burnEffectId;
	}
}
