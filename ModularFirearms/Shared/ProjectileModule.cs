using System;
using ModularFirearms.Projectiles;
using ThunderRoad;

namespace ModularFirearms.Shared
{
	// Token: 0x02000015 RID: 21
	public class ProjectileModule : ItemModule
	{
		// Token: 0x060000A1 RID: 161 RVA: 0x00007B09 File Offset: 0x00005D09
		public FrameworkCore.ProjectileType GetSelectedType(string projectileType)
		{
			return (FrameworkCore.ProjectileType)Enum.Parse(typeof(FrameworkCore.ProjectileType), projectileType);
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00007B20 File Offset: 0x00005D20
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			FrameworkCore.ProjectileType selectedType = this.GetSelectedType(this.projectileType);
			if (selectedType.Equals(FrameworkCore.ProjectileType.Pierce) || selectedType.Equals(FrameworkCore.ProjectileType.Blunt))
			{
				item.gameObject.AddComponent<BasicProjectile>();
				return;
			}
			if (selectedType.Equals(FrameworkCore.ProjectileType.Explosive))
			{
				item.gameObject.AddComponent<ExplosiveProjectile>();
				return;
			}
			item.gameObject.AddComponent<BasicProjectile>();
		}

		// Token: 0x04000171 RID: 369
		public string CustomSplatterEffect;

		// Token: 0x04000172 RID: 370
		public float lifetime = 2f;

		// Token: 0x04000173 RID: 371
		public string projectileType = "Pierce";

		// Token: 0x04000174 RID: 372
		public float flyingAcceleration = 1f;

		// Token: 0x04000175 RID: 373
		public float throwMult = 1f;

		// Token: 0x04000176 RID: 374
		public bool allowFlyTime = true;

		// Token: 0x04000177 RID: 375
		public bool useHitScanning;

		// Token: 0x04000178 RID: 376
		public float explosiveForce = 1f;

		// Token: 0x04000179 RID: 377
		public float blastRadius = 10f;

		// Token: 0x0400017A RID: 378
		public float liftMult = 1f;

		// Token: 0x0400017B RID: 379
		public string particleEffectRef;

		// Token: 0x0400017C RID: 380
		public string soundRef;

		// Token: 0x0400017D RID: 381
		public string shellMeshRef;

		// Token: 0x0400017E RID: 382
		public string forceMode = "Impulse";
	}
}
