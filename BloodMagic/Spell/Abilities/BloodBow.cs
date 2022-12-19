using System;
using ThunderRoad;
using UnityEngine;

namespace BloodMagic.Spell.Abilities
{
	// Token: 0x0200001A RID: 26
	public class BloodBow
	{
		// Token: 0x06000091 RID: 145 RVA: 0x00003F98 File Offset: 0x00002198
		public static bool TryToActivate(BloodSpell bloodSpell, Vector3 velocity, SaveData saveData)
		{
			bool flag = !SpellAbilityManager.HasEnoughHealth(10f) || (double)Math.Abs(Vector3.Dot(Vector3.up, bloodSpell.spellCaster.magic.right)) <= (double)saveData.gesturePrescision;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				Side side = bloodSpell.spellCaster.other.ragdollHand.side;
				Vector3 vector = Player.local.transform.rotation * PlayerControl.GetHand(side).GetHandVelocity();
				Vector3 normalized = vector.normalized;
				bool flag3 = !bloodSpell.spellCaster.other.isFiring || (double)Vector3.Dot(bloodSpell.spellCaster.magic.up, bloodSpell.spellCaster.other.magic.up) <= (double)saveData.gesturePrescision;
				if (flag3)
				{
					flag2 = false;
				}
				else
				{
					Vector3 normalized2 = (bloodSpell.spellCaster.magic.position - bloodSpell.spellCaster.other.magic.position).normalized;
					bool flag4 = (double)Vector3.Dot(bloodSpell.spellCaster.magic.up, normalized2) <= (double)saveData.gesturePrescision || (double)Vector3.Distance(bloodSpell.spellCaster.magic.position, bloodSpell.spellCaster.other.magic.position) <= 0.150000005960464 || (double)Vector3.Dot(-bloodSpell.spellCaster.magic.up, normalized) <= (double)saveData.gesturePrescision || (double)Vector3.Project(vector, -bloodSpell.spellCaster.magic.up).magnitude <= 1.0;
					if (flag4)
					{
						flag2 = false;
					}
					else
					{
						SpellAbilityManager.SpendHealth(10f);
						flag2 = true;
					}
				}
			}
			return flag2;
		}
	}
}
