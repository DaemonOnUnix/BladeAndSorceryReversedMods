using System;
using ThunderRoad;

namespace TotT
{
	// Token: 0x0200001C RID: 28
	public class ClimbingPick : ItemModule
	{
		// Token: 0x060000D6 RID: 214 RVA: 0x00007236 File Offset: 0x00005436
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			ClimbingPickParser.TapDelay = this.TapDelay;
			ClimbingPickParser.KnockOutMinutes = this.KnockOutMinutes;
			item.gameObject.AddComponent<ClimbingPickModule>();
		}

		// Token: 0x04000098 RID: 152
		public float TapDelay;

		// Token: 0x04000099 RID: 153
		public float KnockOutMinutes;
	}
}
