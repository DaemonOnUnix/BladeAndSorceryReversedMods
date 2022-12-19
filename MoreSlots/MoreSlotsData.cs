using System;
using ThunderRoad;
using UnityEngine;

namespace Wully.MoreSlots
{
	// Token: 0x02000003 RID: 3
	[Serializable]
	public class MoreSlotsData : CustomData
	{
		// Token: 0x06000008 RID: 8 RVA: 0x00002408 File Offset: 0x00000608
		public override string ToString()
		{
			return string.Format("{0}: {1}, {2}: {3}, {4}: {5}, {6}: {7}, {8}: {9}, {10}: {11}, {12}: {13}", new object[]
			{
				"holderDataId", this.holderDataId, "handSide", this.handSide, "localPosition", this.localPosition, "localRotation", this.localRotation, "ragdollPartName", this.ragdollPartName,
				"triggerColliderRadius", this.triggerColliderRadius, "enabled", this.enabled
			});
		}

		// Token: 0x04000006 RID: 6
		public string holderDataId;

		// Token: 0x04000007 RID: 7
		public Interactable.HandSide handSide;

		// Token: 0x04000008 RID: 8
		public Vector3 localPosition;

		// Token: 0x04000009 RID: 9
		public Vector3 localRotation;

		// Token: 0x0400000A RID: 10
		public string ragdollPartName;

		// Token: 0x0400000B RID: 11
		public float triggerColliderRadius = 0.15f;

		// Token: 0x0400000C RID: 12
		public bool enabled = true;
	}
}
