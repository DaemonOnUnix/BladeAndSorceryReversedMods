using System;
using ThunderRoad;
using UnityEngine;

namespace BloodMagic.Spell.Abilities
{
	// Token: 0x02000019 RID: 25
	public class BloodWave
	{
		// Token: 0x0600008F RID: 143 RVA: 0x00003DCC File Offset: 0x00001FCC
		public static bool TryToActivate(BloodSpell bloodSpell, Vector3 velocity, SaveData saveData)
		{
			bool flag = !SpellAbilityManager.HasEnoughHealth(20f) || BloodWave.waveCreated || !PlayerControl.GetHand(0).gripPressed || !PlayerControl.GetHand(1).gripPressed || (double)Vector3.Dot(Player.currentCreature.transform.forward, bloodSpell.spellCaster.magic.forward) <= (double)saveData.gesturePrescision;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				bool flag3 = (double)Vector3.Dot(Player.currentCreature.transform.forward, bloodSpell.spellCaster.other.magic.forward) > (double)saveData.gesturePrescision;
				if (flag3)
				{
					Vector3 vector = Player.local.transform.rotation * PlayerControl.GetHand(1).GetHandVelocity();
					Vector3 vector2 = Player.local.transform.rotation * PlayerControl.GetHand(0).GetHandVelocity();
					bool flag4 = (double)Vector3.Dot(Player.currentCreature.transform.forward, vector) > (double)saveData.gesturePrescision * 1.5 && (double)Vector3.Dot(Player.currentCreature.transform.forward, vector2) > (double)saveData.gesturePrescision * 1.5;
					if (flag4)
					{
						SpellAbilityManager.SpendHealth(20f);
						BloodWave.waveCreated = true;
						return true;
					}
				}
				Side side = bloodSpell.spellCaster.other.ragdollHand.side;
				Vector3 normalized = (Player.local.transform.rotation * PlayerControl.GetHand(side).GetHandVelocity()).normalized;
				flag2 = !bloodSpell.spellCaster.other.isFiring && false;
			}
			return flag2;
		}

		// Token: 0x0400003B RID: 59
		public static bool waveCreated;
	}
}
