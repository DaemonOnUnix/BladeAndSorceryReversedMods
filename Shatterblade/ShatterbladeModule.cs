using System;
using ThunderRoad;

namespace Shatterblade
{
	// Token: 0x0200000B RID: 11
	public class ShatterbladeModule : ItemModule
	{
		// Token: 0x060000AE RID: 174 RVA: 0x00005AD0 File Offset: 0x00003CD0
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			bool flag = Level.current.data.id == "CharacterSelection";
			if (!flag)
			{
				Shatterblade shatterblade = item.gameObject.AddComponent<Shatterblade>();
				shatterblade.isTutorialBlade = this.tutorial;
				shatterblade.module = this;
			}
		}

		// Token: 0x0400001D RID: 29
		public bool tutorial = false;

		// Token: 0x0400001E RID: 30
		public bool discoMode = false;

		// Token: 0x0400001F RID: 31
		public float damageModifier = 1f;

		// Token: 0x04000020 RID: 32
		public float jointMaxForce = 100f;
	}
}
