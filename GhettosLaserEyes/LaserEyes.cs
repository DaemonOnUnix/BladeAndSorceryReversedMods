using System;
using ThunderRoad;
using UnityEngine;

namespace GhettosLaserEyes
{
	// Token: 0x02000004 RID: 4
	public class LaserEyes : SpellCastCharge
	{
		// Token: 0x0600000A RID: 10 RVA: 0x00002FE8 File Offset: 0x000011E8
		public override void Fire(bool active)
		{
			bool flag = Player.local.gameObject.GetComponent<PlayerModule>() == null;
			if (flag)
			{
				PlayerModule module = Player.local.gameObject.AddComponent<PlayerModule>();
				module.damage = this.damage;
				module.dismember = this.dismember;
				module.framesBetweenDecals = this.framesBetweenDecals;
				module.showVignette = this.showVignette;
				module.overrideColor = this.overrideColor;
				module.color = this.color;
				module.playLaserSound = this.playLaserSound;
				module.useV1Sound = this.useV1Sound;
				module.useV2Sound = this.useV2Sound;
				module.useV3Sound = this.useV3Sound;
			}
			Player.local.gameObject.GetComponent<PlayerModule>().Toggle(active);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000030B2 File Offset: 0x000012B2
		public override void UpdateCaster()
		{
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000030B5 File Offset: 0x000012B5
		public override void Throw(Vector3 velocity)
		{
		}

		// Token: 0x0400001C RID: 28
		public float damage;

		// Token: 0x0400001D RID: 29
		public bool dismember;

		// Token: 0x0400001E RID: 30
		public int framesBetweenDecals;

		// Token: 0x0400001F RID: 31
		public bool showVignette;

		// Token: 0x04000020 RID: 32
		public bool overrideColor;

		// Token: 0x04000021 RID: 33
		public Color color;

		// Token: 0x04000022 RID: 34
		public bool playLaserSound;

		// Token: 0x04000023 RID: 35
		public bool useV1Sound;

		// Token: 0x04000024 RID: 36
		public bool useV2Sound;

		// Token: 0x04000025 RID: 37
		public bool useV3Sound;
	}
}
