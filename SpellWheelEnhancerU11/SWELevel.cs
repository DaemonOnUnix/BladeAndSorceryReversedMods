using System;
using System.Collections;
using HarmonyLib;
using ThunderRoad;
using UnityEngine;

namespace SpellWheelEnhancer
{
	// Token: 0x02000002 RID: 2
	public class SWELevel : LevelModule
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public override IEnumerator OnLoadCoroutine()
		{
			yield return base.OnLoadCoroutine();
			SpellWheelGlobalVariables.fontSize = this.fontSize;
			try
			{
				this.harmony = new Harmony("SpellWeelEnhancer");
				this.harmony.PatchAll();
				Debug.Log("Spell Enhancer Loaded");
				yield break;
			}
			catch (Exception ex2)
			{
				Exception ex = ex2;
				Debug.LogException(ex);
				yield break;
			}
			yield break;
		}

		// Token: 0x04000001 RID: 1
		public int fontSize = 120;

		// Token: 0x04000002 RID: 2
		public Harmony harmony;
	}
}
