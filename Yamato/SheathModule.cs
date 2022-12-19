using System;
using ThunderRoad;

namespace Yamato
{
	// Token: 0x0200000D RID: 13
	public class SheathModule : ItemModule
	{
		// Token: 0x06000039 RID: 57 RVA: 0x00003DA0 File Offset: 0x00001FA0
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			item.gameObject.AddComponent<SheathComponent>().Setup(this.DashSpeed, this.DashDirection, this.DisableGravity, this.DisableCollision, this.DashTime, this.StopOnEnd, this.StopOnStart, this.ThumbstickDash, this.SwapButtons);
		}

		// Token: 0x04000059 RID: 89
		public float DashSpeed;

		// Token: 0x0400005A RID: 90
		public string DashDirection;

		// Token: 0x0400005B RID: 91
		public bool DisableGravity;

		// Token: 0x0400005C RID: 92
		public bool DisableCollision;

		// Token: 0x0400005D RID: 93
		public float DashTime;

		// Token: 0x0400005E RID: 94
		public bool StopOnEnd;

		// Token: 0x0400005F RID: 95
		public bool StopOnStart;

		// Token: 0x04000060 RID: 96
		public bool ThumbstickDash;

		// Token: 0x04000061 RID: 97
		public bool SwapButtons;
	}
}
