using System;
using ThunderRoad;
using UnityEngine;

namespace TotT
{
	// Token: 0x0200002F RID: 47
	public class PBBDefault : ItemModule
	{
		// Token: 0x0600016A RID: 362 RVA: 0x0000AFD7 File Offset: 0x000091D7
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			PBBDefaultParser.ammoMax = this.ammoMax;
			PBBDefaultParser.EmissionColor = this.EmissionColor;
			item.gameObject.AddComponent<PBBDefaultModule>();
		}

		// Token: 0x0400010E RID: 270
		public int ammoMax;

		// Token: 0x0400010F RID: 271
		public Color EmissionColor;
	}
}
