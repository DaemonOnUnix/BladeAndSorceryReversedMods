using System;
using System.Collections;
using IngameDebugConsole;
using ThunderRoad;

namespace TimeStopSpell
{
	// Token: 0x02000003 RID: 3
	public class TimeStopSpellTestingLevelModule : LevelModule
	{
		// Token: 0x06000009 RID: 9 RVA: 0x00002619 File Offset: 0x00000819
		public override IEnumerator OnLoadCoroutine()
		{
			DebugLogConsole.AddCommandInstance("zw", "ZaWarudo", "ZaWarudo", this, new string[0]);
			return base.OnLoadCoroutine();
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000026B8 File Offset: 0x000008B8
		private IEnumerator ZaWarudoCoroutine()
		{
			GameManager.local.GetComponent<TimeStopData>().Toggle();
			yield return null;
			yield break;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000026D4 File Offset: 0x000008D4
		private void ZaWarudo()
		{
			GameManager.local.StartCoroutine(this.ZaWarudoCoroutine());
		}
	}
}
