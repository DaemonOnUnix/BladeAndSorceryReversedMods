using System;
using ThunderRoad;
using UnityEngine;

namespace BloodMagic.Spell.Abilities
{
	// Token: 0x0200001D RID: 29
	public class BloodDaggerAbility
	{
		// Token: 0x06000099 RID: 153 RVA: 0x00004730 File Offset: 0x00002930
		public static bool TryToActivate(BloodSpell bloodSpell, Vector3 velocity, SaveData saveData)
		{
			bool flag = (double)Vector3.Dot(Player.currentCreature.transform.forward, bloodSpell.spellCaster.magic.up) <= (double)saveData.gesturePrescision || !SpellAbilityManager.SpendHealth(3f);
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				Catalog.GetData<ItemData>("BloodDagger", true).SpawnAsync(delegate(Item dagger)
				{
					dagger.IgnoreRagdollCollision(bloodSpell.spellCaster.mana.creature.ragdoll);
					Vector3 aimDir = BloodSpell.AimAssist(dagger.transform.position, velocity.normalized, 0.7f, 0.01f).aimDir;
					dagger.rb.AddForce(aimDir * velocity.magnitude * saveData.bulletSpeed, 1);
					dagger.transform.rotation = Quaternion.LookRotation(aimDir);
					dagger.Throw(1f, 2);
					dagger.gameObject.AddComponent<BloodDagger>().Initialize(dagger);
				}, new Vector3?(bloodSpell.spellCaster.magic.position), new Quaternion?(Quaternion.Euler(bloodSpell.spellCaster.magic.forward)), null, false, null);
				flag2 = true;
			}
			return flag2;
		}
	}
}
