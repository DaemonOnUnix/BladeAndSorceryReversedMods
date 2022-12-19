using System;
using ThunderRoad;
using UnityEngine;

namespace SwordBeam
{
	// Token: 0x02000002 RID: 2
	public class SwordBeamModule : ItemModule
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			item.gameObject.AddComponent<SwordBeamComponent>().Setup(this.ProjectileID, this.ActivationButton, this.BeamDismember, this.BeamSpeed, this.SwordSpeed, this.DespawnTime, this.BeamDamage, this.BeamCooldown, this.BeamColor, this.BeamEmission, this.BeamSize, this.BeamScaleIncrease);
		}

		// Token: 0x04000001 RID: 1
		public Color BeamColor;

		// Token: 0x04000002 RID: 2
		public Color BeamEmission;

		// Token: 0x04000003 RID: 3
		public Vector3 BeamSize;

		// Token: 0x04000004 RID: 4
		public Vector3 BeamScaleIncrease;

		// Token: 0x04000005 RID: 5
		public float BeamCooldown;

		// Token: 0x04000006 RID: 6
		public float DespawnTime;

		// Token: 0x04000007 RID: 7
		public float SwordSpeed;

		// Token: 0x04000008 RID: 8
		public float BeamSpeed;

		// Token: 0x04000009 RID: 9
		public string ProjectileID;

		// Token: 0x0400000A RID: 10
		public float BeamDamage;

		// Token: 0x0400000B RID: 11
		public bool BeamDismember;

		// Token: 0x0400000C RID: 12
		public string ActivationButton;
	}
}
