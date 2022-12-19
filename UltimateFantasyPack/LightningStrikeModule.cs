using System;
using ThunderRoad;

namespace UltimateFantasyPack
{
	// Token: 0x02000019 RID: 25
	public class LightningStrikeModule : ItemModule
	{
		// Token: 0x0600005F RID: 95 RVA: 0x00003A72 File Offset: 0x00001C72
		public override void OnItemLoaded(Item item)
		{
			item.gameObject.AddComponent<LightningStrikeNew>().Setup(this.explosionForce, this.explosionRange, this.airDuration);
			base.OnItemLoaded(item);
		}

		// Token: 0x04000048 RID: 72
		public float explosionForce;

		// Token: 0x04000049 RID: 73
		public float explosionRange;

		// Token: 0x0400004A RID: 74
		public float airDuration;
	}
}
