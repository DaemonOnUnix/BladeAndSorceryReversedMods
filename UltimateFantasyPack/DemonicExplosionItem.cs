using System;
using ThunderRoad;

namespace UltimateFantasyPack
{
	// Token: 0x0200001D RID: 29
	public class DemonicExplosionItem : ItemModule
	{
		// Token: 0x0600006F RID: 111 RVA: 0x00003EC1 File Offset: 0x000020C1
		public override void OnItemLoaded(Item item)
		{
			item.gameObject.AddComponent<DemonicExplosion>().Setup(this.explodePower, this.cooldownDuration, this.fireLerpDuration);
			base.OnItemLoaded(item);
		}

		// Token: 0x04000052 RID: 82
		public float explodePower;

		// Token: 0x04000053 RID: 83
		public float cooldownDuration;

		// Token: 0x04000054 RID: 84
		public float fireLerpDuration;
	}
}
