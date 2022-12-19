using System;
using ThunderRoad;

namespace ExplodingFireballs
{
	// Token: 0x02000002 RID: 2
	public class ExplosionModule : ItemModule
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			item.gameObject.AddComponent<ExplosionComponent>().Setup(this.explosionRadius, this.explosionForce, this.explosionDamage, this.explosionEffectId);
		}

		// Token: 0x04000001 RID: 1
		public float explosionRadius = 10f;

		// Token: 0x04000002 RID: 2
		public float explosionForce = 25f;

		// Token: 0x04000003 RID: 3
		public float explosionDamage = 20f;

		// Token: 0x04000004 RID: 4
		public string explosionEffectId = "MeteorExplosion";
	}
}
