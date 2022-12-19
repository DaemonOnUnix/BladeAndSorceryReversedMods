using System;
using ThunderRoad;
using UnityEngine;

namespace TotT
{
	// Token: 0x02000035 RID: 53
	public class PBBSleep : ItemModule
	{
		// Token: 0x0600017D RID: 381 RVA: 0x0000B21C File Offset: 0x0000941C
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			PBBSleepParser.ammoMax = this.ammoMax;
			PBBSleepParser.EmissionColor = this.EmissionColor;
			PBBSleepParser.KnockOutMinutes = this.KnockOutMinutes;
			PBBSleepParser.CombatDelaySeconds = this.CombatDelaySeconds;
			item.gameObject.AddComponent<PBBSleepModule>();
		}

		// Token: 0x04000119 RID: 281
		public int ammoMax;

		// Token: 0x0400011A RID: 282
		public Color EmissionColor;

		// Token: 0x0400011B RID: 283
		public float KnockOutMinutes;

		// Token: 0x0400011C RID: 284
		public float CombatDelaySeconds;
	}
}
