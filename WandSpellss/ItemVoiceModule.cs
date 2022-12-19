using System;
using ThunderRoad;

namespace WandSpellss
{
	// Token: 0x0200000E RID: 14
	public class ItemVoiceModule : ItemModule
	{
		// Token: 0x06000028 RID: 40 RVA: 0x000035B4 File Offset: 0x000017B4
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			item.gameObject.AddComponent<VoiceWeaponComponent>().Setup(this.spellSpeed, this.magicEffect, this.expelliarmusPower);
			Player.local.creature.gameObject.AddComponent<Soul>();
		}

		// Token: 0x0400003A RID: 58
		public float spellSpeed;

		// Token: 0x0400003B RID: 59
		public bool magicEffect;

		// Token: 0x0400003C RID: 60
		public float expelliarmusPower;
	}
}
