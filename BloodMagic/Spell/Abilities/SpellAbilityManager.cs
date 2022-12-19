using System;
using BloodMagic.UI;
using ThunderRoad;
using UnityEngine;

namespace BloodMagic.Spell.Abilities
{
	// Token: 0x0200001F RID: 31
	public static class SpellAbilityManager
	{
		// Token: 0x0600009D RID: 157 RVA: 0x0000490D File Offset: 0x00002B0D
		public static bool HasEnoughHealth(float health)
		{
			return !BookUIHandler.saveData.useHealth || (double)Player.currentCreature.currentHealth > (double)health;
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00004930 File Offset: 0x00002B30
		public static bool SpendHealth(float health)
		{
			bool flag = !BookUIHandler.saveData.useHealth;
			bool flag2;
			if (flag)
			{
				flag2 = true;
			}
			else
			{
				bool flag3 = (double)Player.currentCreature.currentHealth > (double)health;
				if (flag3)
				{
					Player.currentCreature.currentHealth -= health;
					CameraEffects.RefreshHealth();
					flag2 = true;
				}
				else
				{
					CameraEffects.DoTimedEffect(new Color(1f, 0.1f, 0.1f), 0, 0.5f);
					flag2 = false;
				}
			}
			return flag2;
		}
	}
}
