using System;
using System.Collections;
using ThunderRoad;

namespace ModularFirearms.Shared
{
	// Token: 0x02000014 RID: 20
	public class FrameworkSettings : LevelModule
	{
		// Token: 0x0600009E RID: 158 RVA: 0x00007AD4 File Offset: 0x00005CD4
		public override IEnumerator OnLoadCoroutine()
		{
			if (FrameworkSettings.local == null)
			{
				FrameworkSettings.local = this;
			}
			yield return base.OnLoadCoroutine();
			yield break;
		}

		// Token: 0x0400016D RID: 365
		public static FrameworkSettings local;

		// Token: 0x0400016E RID: 366
		public bool useHitscan;

		// Token: 0x0400016F RID: 367
		public float hitscanMaxDistance = 1f;

		// Token: 0x04000170 RID: 368
		public string customEffectID = "PenetrationFisherFirearmModular";
	}
}
