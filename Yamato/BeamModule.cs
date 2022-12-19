using System;
using ThunderRoad;
using UnityEngine;

namespace Yamato
{
	// Token: 0x0200000B RID: 11
	public class BeamModule : ItemModule
	{
		// Token: 0x06000032 RID: 50 RVA: 0x000038A4 File Offset: 0x00001AA4
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			item.gameObject.AddComponent<BeamCustomization>().Setup(this.BeamDismember, this.BeamSpeed, this.DespawnTime, this.BeamDamage, this.BeamColor, this.BeamEmission, this.BeamSize, this.BeamScaleIncrease);
		}

		// Token: 0x04000047 RID: 71
		public Color BeamColor;

		// Token: 0x04000048 RID: 72
		public Color BeamEmission;

		// Token: 0x04000049 RID: 73
		public Vector3 BeamSize;

		// Token: 0x0400004A RID: 74
		public float BeamSpeed;

		// Token: 0x0400004B RID: 75
		public float DespawnTime;

		// Token: 0x0400004C RID: 76
		public float BeamDamage;

		// Token: 0x0400004D RID: 77
		public bool BeamDismember;

		// Token: 0x0400004E RID: 78
		public Vector3 BeamScaleIncrease;
	}
}
