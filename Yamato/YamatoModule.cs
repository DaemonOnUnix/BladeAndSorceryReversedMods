using System;
using ThunderRoad;

namespace Yamato
{
	// Token: 0x02000007 RID: 7
	public class YamatoModule : ItemModule
	{
		// Token: 0x0600000F RID: 15 RVA: 0x00002358 File Offset: 0x00000558
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			item.gameObject.AddComponent<YamatoComponent>().Setup(this.SwordSpeed, this.BeamCooldown, this.SwapButtons, this.StopOnJudgementCut, this.ToggleAnimeSlice, this.ToggleSwordBeams, this.SwapJudgementCutActivation, this.NoJudgementCut);
		}

		// Token: 0x04000007 RID: 7
		public float BeamCooldown;

		// Token: 0x04000008 RID: 8
		public float SwordSpeed;

		// Token: 0x04000009 RID: 9
		public bool SwapButtons;

		// Token: 0x0400000A RID: 10
		public bool StopOnJudgementCut;

		// Token: 0x0400000B RID: 11
		public bool ToggleAnimeSlice;

		// Token: 0x0400000C RID: 12
		public bool ToggleSwordBeams;

		// Token: 0x0400000D RID: 13
		public bool SwapJudgementCutActivation;

		// Token: 0x0400000E RID: 14
		public bool NoJudgementCut;
	}
}
