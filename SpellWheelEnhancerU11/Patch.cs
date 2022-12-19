using System;
using System.Collections.Generic;
using HarmonyLib;
using ThunderRoad;
using UnityEngine;

namespace SpellWheelEnhancer
{
	// Token: 0x02000006 RID: 6
	[HarmonyPatch(typeof(WheelMenu), "GetOrbLocalPosition")]
	internal class Patch
	{
		// Token: 0x06000008 RID: 8 RVA: 0x00002240 File Offset: 0x00000440
		private static bool Prefix(ref Vector3 __result, List<WheelMenu.Orb> ___orbs, WheelMenu __instance, int orbIndex, float startAngle, float maxAngle, bool uniform = true, bool revert = false)
		{
			float num2;
			if (uniform)
			{
				float num = maxAngle / (float)Mathf.Clamp(___orbs.Count, 0, 8);
				num2 = startAngle + num * (float)orbIndex;
			}
			else
			{
				float num3 = maxAngle / (float)(___orbs.Count + 1);
				num2 = (float)((double)startAngle - (double)maxAngle / 2.0 + (double)num3 * (double)(orbIndex + 1));
			}
			if (revert)
			{
				num2 = -num2;
			}
			float num4 = Mathf.Clamp((float)((double)Mathf.Floor((float)(orbIndex / 8)) * 0.5 + 1.0), 1f, float.PositiveInfinity);
			float num5 = (((double)Mathf.Floor((float)(orbIndex / 8)) % 2.0 == 0.0 || (double)Mathf.Floor((float)(orbIndex / 8)) == 0.0) ? 0f : 22.5f);
			__result = Quaternion.AngleAxis(num2 + num5, Vector3.forward) * Vector3.up * num4 * __instance.orbRadius;
			return false;
		}
	}
}
