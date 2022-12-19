using System;
using ModularFirearms.Items;
using ThunderRoad;

namespace ModularFirearms.Shared
{
	// Token: 0x02000011 RID: 17
	public class AmmoModule : ItemModule
	{
		// Token: 0x06000095 RID: 149 RVA: 0x0000755B File Offset: 0x0000575B
		public FrameworkCore.AmmoType GetSelectedType()
		{
			return (FrameworkCore.AmmoType)Enum.Parse(typeof(FrameworkCore.AmmoType), this.ammoType);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00007577 File Offset: 0x00005777
		public FrameworkCore.AmmoType GetAcceptedType()
		{
			return (FrameworkCore.AmmoType)Enum.Parse(typeof(FrameworkCore.AmmoType), this.acceptedAmmoType);
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00007594 File Offset: 0x00005794
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			this.selectedType = this.GetSelectedType();
			if (this.selectedType.Equals(FrameworkCore.AmmoType.Generic) || this.selectedType.Equals(FrameworkCore.AmmoType.SemiAuto) || this.selectedType.Equals(FrameworkCore.AmmoType.ShotgunShell))
			{
				item.gameObject.AddComponent<InteractiveAmmo>();
				return;
			}
			if (this.selectedType.Equals(FrameworkCore.AmmoType.Magazine))
			{
				item.gameObject.AddComponent<InteractiveMagazine>();
				return;
			}
			if (this.selectedType.Equals(FrameworkCore.AmmoType.Pouch))
			{
				item.gameObject.AddComponent<AmmoResupply>();
			}
		}

		// Token: 0x040000D4 RID: 212
		private FrameworkCore.AmmoType selectedType;

		// Token: 0x040000D5 RID: 213
		public string handleRef;

		// Token: 0x040000D6 RID: 214
		public string bulletMeshRef;

		// Token: 0x040000D7 RID: 215
		public string ammoType = "SemiAuto";

		// Token: 0x040000D8 RID: 216
		public string acceptedAmmoType = "SemiAuto";

		// Token: 0x040000D9 RID: 217
		public int ammoCapacity = 1;

		// Token: 0x040000DA RID: 218
		public float[] ejectionForceVector;

		// Token: 0x040000DB RID: 219
		public string magazineID = "MagazineBasicPistol";

		// Token: 0x040000DC RID: 220
		public bool despawnBagOnEmpty;

		// Token: 0x040000DD RID: 221
		public bool enableBulletHolder;
	}
}
