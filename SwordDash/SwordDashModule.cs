using System;
using ThunderRoad;

namespace SwordDash
{
	// Token: 0x02000002 RID: 2
	public class SwordDashModule : ItemModule
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			item.gameObject.AddComponent<SwordDashComponent>().Setup(this.DashSpeed, this.DashDirection, this.DisableGravity, this.DisableCollision, this.DashTime, this.ActivationButton);
		}

		// Token: 0x04000001 RID: 1
		public float DashSpeed;

		// Token: 0x04000002 RID: 2
		public string DashDirection;

		// Token: 0x04000003 RID: 3
		public bool DisableGravity;

		// Token: 0x04000004 RID: 4
		public bool DisableCollision;

		// Token: 0x04000005 RID: 5
		public float DashTime;

		// Token: 0x04000006 RID: 6
		public string ActivationButton;
	}
}
