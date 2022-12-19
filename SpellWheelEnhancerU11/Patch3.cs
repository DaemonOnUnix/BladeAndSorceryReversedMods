using System;
using HarmonyLib;
using ThunderRoad;
using UnityEngine;

namespace SpellWheelEnhancer
{
	// Token: 0x02000004 RID: 4
	[HarmonyPatch(typeof(WheelMenu), "Hide")]
	internal class Patch3
	{
		// Token: 0x06000004 RID: 4 RVA: 0x00002078 File Offset: 0x00000278
		private static bool Prefix(WheelMenu __instance)
		{
			TextMesh componentInChildren = __instance.gameObject.GetComponentInChildren<TextMesh>();
			bool flag = componentInChildren;
			if (flag)
			{
				Object.Destroy(componentInChildren);
			}
			return true;
		}
	}
}
