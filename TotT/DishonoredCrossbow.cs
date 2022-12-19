using System;
using ThunderRoad;
using UnityEngine;

namespace TotT
{
	// Token: 0x02000020 RID: 32
	public class DishonoredCrossbow : ItemModule
	{
		// Token: 0x060000EB RID: 235 RVA: 0x0000789C File Offset: 0x00005A9C
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			BoltAmount.NormalBoltMax = this.NormalBoltMax;
			BoltAmount.SleepDartMax = this.SleepDartMax;
			BoltAmount.StingBoltMax = this.StingBoltMax;
			Debug.Log(string.Format("Normal Bolt Max found for {0}", this.NormalBoltMax));
			Debug.Log(string.Format("Sleep Dart Max found for {0}", this.SleepDartMax));
			Debug.Log(string.Format("Sting Bolt Max found for {0}", this.StingBoltMax));
			item.gameObject.AddComponent<CrossbowModule>();
		}

		// Token: 0x040000B5 RID: 181
		public int NormalBoltMax;

		// Token: 0x040000B6 RID: 182
		public int SleepDartMax;

		// Token: 0x040000B7 RID: 183
		public int StingBoltMax;
	}
}
