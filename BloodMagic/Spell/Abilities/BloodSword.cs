using System;
using ThunderRoad;
using UnityEngine;

namespace BloodMagic.Spell.Abilities
{
	// Token: 0x0200001B RID: 27
	public class BloodSword
	{
		// Token: 0x06000093 RID: 147 RVA: 0x0000418C File Offset: 0x0000238C
		public static bool TryToActivate(BloodSpell bloodSpell, Vector3 velocity, SaveData saveData)
		{
			bool flag = !SpellAbilityManager.HasEnoughHealth(15f) || (double)Vector3.Dot(Vector3.down, bloodSpell.spellCaster.magic.forward) <= (double)saveData.gesturePrescision;
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
				bool flag3 = !bloodSpell.spellCaster.other.isFiring || (double)Vector3.Distance(bloodSpell.spellCaster.other.magic.position, bloodSpell.spellCaster.magic.position) > 0.200000002980232;
				if (flag3)
				{
					flag2 = false;
				}
				else
				{
					bool flag4 = side == 1;
					if (flag4)
					{
						bool flag5 = (double)Vector3.Dot(bloodSpell.spellCaster.magic.right, bloodSpell.spellCaster.other.magic.forward) > (double)saveData.gesturePrescision;
						if (flag5)
						{
							Vector3 vector2 = Vector3.Project(vector, -bloodSpell.spellCaster.magic.right);
							Debug.Log(string.Format("BloodSword :: projection magnitude {0}", vector2.magnitude));
							bool flag6 = (double)Vector3.Dot(-bloodSpell.spellCaster.magic.right, vector2.normalized) > (double)saveData.gesturePrescision && (double)vector2.magnitude > 1.0;
							if (flag6)
							{
								SpellAbilityManager.SpendHealth(15f);
								return true;
							}
						}
					}
					else
					{
						bool flag7 = (double)Vector3.Dot(-bloodSpell.spellCaster.magic.right, bloodSpell.spellCaster.other.magic.forward) > (double)saveData.gesturePrescision;
						if (flag7)
						{
							Vector3 vector3 = Vector3.Project(vector, bloodSpell.spellCaster.magic.right);
							Debug.Log(string.Format("BloodSword :: projection magnitude {0}", vector3.magnitude));
							bool flag8 = (double)Vector3.Dot(bloodSpell.spellCaster.magic.right, vector3.normalized) > (double)saveData.gesturePrescision && (double)vector3.magnitude > 1.0;
							if (flag8)
							{
								SpellAbilityManager.SpendHealth(15f);
								return true;
							}
						}
					}
					flag2 = false;
				}
			}
			return flag2;
		}
	}
}
