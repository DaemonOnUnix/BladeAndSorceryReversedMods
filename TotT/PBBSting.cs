using System;
using ThunderRoad;
using UnityEngine;

namespace TotT
{
	// Token: 0x02000032 RID: 50
	public class PBBSting : ItemModule
	{
		// Token: 0x06000173 RID: 371 RVA: 0x0000B0BF File Offset: 0x000092BF
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			PBBStingParser.ammoMax = this.ammoMax;
			PBBStingParser.EmissionColor = this.EmissionColor;
			item.gameObject.AddComponent<PBBStingModule>();
		}

		// Token: 0x04000112 RID: 274
		public int ammoMax;

		// Token: 0x04000113 RID: 275
		public Color EmissionColor;

		// Token: 0x04000114 RID: 276
		public float KnockOutMinutes;

		// Token: 0x04000115 RID: 277
		public float CombatDelaySeconds;
	}
}
