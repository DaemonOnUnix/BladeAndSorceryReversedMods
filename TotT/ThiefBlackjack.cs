using System;
using ThunderRoad;

namespace TotT
{
	// Token: 0x02000045 RID: 69
	public class ThiefBlackjack : ItemModule
	{
		// Token: 0x060001D9 RID: 473 RVA: 0x0000CD3F File Offset: 0x0000AF3F
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			BlackjackParser.KnockOutMinutes = this.KnockOutMinutes;
			item.gameObject.AddComponent<BlackjackModule>();
		}

		// Token: 0x0400014E RID: 334
		public float KnockOutMinutes;
	}
}
