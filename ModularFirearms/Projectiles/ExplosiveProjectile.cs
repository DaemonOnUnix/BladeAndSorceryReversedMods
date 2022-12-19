using System;
using ModularFirearms.Shared;
using ThunderRoad;
using UnityEngine;

namespace ModularFirearms.Projectiles
{
	// Token: 0x02000017 RID: 23
	public class ExplosiveProjectile : MonoBehaviour
	{
		// Token: 0x060000AC RID: 172 RVA: 0x00007DEC File Offset: 0x00005FEC
		protected void Awake()
		{
			this.item = base.GetComponent<Item>();
			this.module = this.item.data.GetModule<ProjectileModule>();
			if (!string.IsNullOrEmpty(this.module.shellMeshRef))
			{
				this.meshObject = this.item.GetCustomReference(this.module.shellMeshRef, true).gameObject;
			}
			if (!string.IsNullOrEmpty(this.module.particleEffectRef))
			{
				this.explosiveEffect = this.item.GetCustomReference(this.module.particleEffectRef, true).GetComponent<ParticleSystem>();
			}
			if (!string.IsNullOrEmpty(this.module.soundRef))
			{
				this.explosiveSound = this.item.GetCustomReference(this.module.soundRef, true).GetComponent<AudioSource>();
			}
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00007EB8 File Offset: 0x000060B8
		protected void Start()
		{
			this.item.Throw(this.module.throwMult, 2);
			if (this.module.allowFlyTime)
			{
				this.item.rb.useGravity = false;
				this.isFlying = true;
			}
			this.item.Despawn(this.module.lifetime);
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00007F17 File Offset: 0x00006117
		private void LateUpdate()
		{
			if (this.isFlying)
			{
				this.item.rb.velocity = this.item.rb.velocity * this.module.flyingAcceleration;
			}
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00007F51 File Offset: 0x00006151
		public void IgnoreItem(Item interactiveObject)
		{
			this.item.IgnoreObjectCollision(interactiveObject);
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00007F60 File Offset: 0x00006160
		private void Explode()
		{
			if (this.explosiveSound != null)
			{
				this.explosiveSound.transform.parent = null;
				this.explosiveSound.Play();
			}
			if (this.meshObject != null)
			{
				this.meshObject.SetActive(false);
			}
			if (this.explosiveEffect != null)
			{
				this.explosiveEffect.transform.parent = null;
				FrameworkCore.HitscanExplosion(this.explosiveEffect.transform.position, this.module.explosiveForce, this.module.blastRadius, this.module.liftMult, (ForceMode)Enum.Parse(typeof(ForceMode), this.module.forceMode), true);
				this.explosiveEffect.Play();
			}
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00008031 File Offset: 0x00006231
		private void OnCollisionEnter(Collision hit)
		{
			if (!this.item.rb.useGravity)
			{
				this.item.rb.useGravity = true;
				this.isFlying = false;
			}
			this.Explode();
			this.item.Despawn();
		}

		// Token: 0x04000185 RID: 389
		protected Item item;

		// Token: 0x04000186 RID: 390
		protected ProjectileModule module;

		// Token: 0x04000187 RID: 391
		private ParticleSystem explosiveEffect;

		// Token: 0x04000188 RID: 392
		private AudioSource explosiveSound;

		// Token: 0x04000189 RID: 393
		private GameObject meshObject;

		// Token: 0x0400018A RID: 394
		protected bool isFlying;
	}
}
