using System;
using ThunderRoad;

namespace TotT
{
	// Token: 0x02000040 RID: 64
	public class DishonoredSleepDart : ItemModule
	{
		// Token: 0x060001C9 RID: 457 RVA: 0x0000CABB File Offset: 0x0000ACBB
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			SleepDartParser.KnockOutMinutes = this.KnockOutMinutes;
			SleepDartParser.CombatDelaySeconds = this.CombatDelaySeconds;
			item.gameObject.AddComponent<SleepDartModule>();
		}

		// Token: 0x04000145 RID: 325
		public float KnockOutMinutes;

		// Token: 0x04000146 RID: 326
		public float CombatDelaySeconds;
	}
}
