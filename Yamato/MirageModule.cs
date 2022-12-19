using System;
using ThunderRoad;

namespace Yamato
{
	// Token: 0x02000009 RID: 9
	public class MirageModule : ItemModule
	{
		// Token: 0x06000025 RID: 37 RVA: 0x00002F14 File Offset: 0x00001114
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			item.gameObject.AddComponent<MirageComponent>().Setup(this.DashSpeed, this.DashDirection, this.DisableGravity, this.DisableCollision, this.DashTime, this.SwordSpeed, this.BeamCooldown, this.RotateDegreesPerSecond, this.ReturnSpeed, this.StopOnEnd, this.StopOnStart, this.ThumbstickDash, this.SwapButtons, this.ToggleSwordBeams);
		}

		// Token: 0x04000024 RID: 36
		public float DashSpeed;

		// Token: 0x04000025 RID: 37
		public string DashDirection;

		// Token: 0x04000026 RID: 38
		public bool DisableGravity;

		// Token: 0x04000027 RID: 39
		public bool DisableCollision;

		// Token: 0x04000028 RID: 40
		public float DashTime;

		// Token: 0x04000029 RID: 41
		public float BeamCooldown;

		// Token: 0x0400002A RID: 42
		public float SwordSpeed;

		// Token: 0x0400002B RID: 43
		public float RotateDegreesPerSecond;

		// Token: 0x0400002C RID: 44
		public float ReturnSpeed;

		// Token: 0x0400002D RID: 45
		public bool StopOnEnd;

		// Token: 0x0400002E RID: 46
		public bool StopOnStart;

		// Token: 0x0400002F RID: 47
		public bool ThumbstickDash;

		// Token: 0x04000030 RID: 48
		public bool SwapButtons;

		// Token: 0x04000031 RID: 49
		public bool ToggleSwordBeams;
	}
}
