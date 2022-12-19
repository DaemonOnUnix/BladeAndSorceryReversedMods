using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using ThunderRoad;
using UnityEngine;

namespace Honour
{
	// Token: 0x02000003 RID: 3
	public class HonourLevelModule : LevelModule
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public override IEnumerator OnLoadCoroutine()
		{
			this.harmony = new Harmony("com.hujohner.honour");
			this.harmony.PatchAll(Assembly.GetExecutingAssembly());
			EventManager.onLevelLoad += new EventManager.LevelLoadEvent(this.EventManager_onLevelLoad);
			Debug.Log(string.Format("[Honour] Mod v{0} loaded.", Assembly.GetExecutingAssembly().GetName().Version));
			return base.OnLoadCoroutine();
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020BA File Offset: 0x000002BA
		public override void OnUnload()
		{
			EventManager.onLevelLoad -= new EventManager.LevelLoadEvent(this.EventManager_onLevelLoad);
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000020D0 File Offset: 0x000002D0
		private void EventManager_onLevelLoad(LevelData levelData, EventTime eventTime)
		{
			bool flag = eventTime != 1;
			if (!flag)
			{
				FieldInfo scoreFields = typeof(MenuModuleScores).GetField("scoreFields", BindingFlags.Instance | BindingFlags.NonPublic);
				foreach (UITextScores uiTextScores in ((List<UITextScores>)scoreFields.GetValue(MenuModuleScores.current)))
				{
					FieldInfo levelModuleXP = typeof(UITextScores).GetField("levelModuleXP", BindingFlags.Instance | BindingFlags.NonPublic);
					levelModuleXP.SetValue(uiTextScores, null);
				}
			}
		}

		// Token: 0x04000001 RID: 1
		private Harmony harmony;
	}
}
