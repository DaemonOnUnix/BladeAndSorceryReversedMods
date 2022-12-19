using System;
using ThunderRoad;

namespace WandSpellss
{
	// Token: 0x0200001D RID: 29
	public class MyItemModule : ItemModule
	{
		// Token: 0x0600005A RID: 90 RVA: 0x0000480E File Offset: 0x00002A0E
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			item.gameObject.AddComponent<MyWeaponComponent>().Setup(this.spellSpeed, this.magicEffect, this.expelliarmusPower);
		}

		// Token: 0x04000073 RID: 115
		public float spellSpeed;

		// Token: 0x04000074 RID: 116
		public bool magicEffect;

		// Token: 0x04000075 RID: 117
		public float expelliarmusPower;
	}
}
