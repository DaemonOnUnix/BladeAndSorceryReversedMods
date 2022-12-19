using System;
using ThunderRoad;
using UnityEngine;

namespace BloodMagic.Spell.Abilities
{
	// Token: 0x0200001E RID: 30
	public class BloodBulletAbility
	{
		// Token: 0x0600009B RID: 155 RVA: 0x00004808 File Offset: 0x00002A08
		public static bool TryToActivate(BloodSpell bloodSpell, Vector3 velocity, SaveData saveData)
		{
			bool flag = (double)Vector3.Dot(Vector3.up, bloodSpell.spellCaster.magic.up) <= (double)saveData.gesturePrescision || (double)Vector3.Dot(Player.currentCreature.transform.forward, bloodSpell.spellCaster.magic.forward) <= (double)saveData.gesturePrescision || !SpellAbilityManager.SpendHealth(2f);
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				Catalog.GetData<ItemData>("BloodBullet", true).SpawnAsync(delegate(Item bullet)
				{
					bullet.IgnoreRagdollCollision(bloodSpell.spellCaster.mana.creature.ragdoll);
					Vector3 aimDir = BloodSpell.AimAssist(bullet.transform.position, velocity.normalized, 0.7f, 0.01f).aimDir;
					bullet.rb.AddForce(aimDir * velocity.magnitude * saveData.bulletSpeed, 1);
					bullet.transform.rotation = Quaternion.LookRotation(aimDir);
					bullet.Throw(1f, 2);
					bullet.gameObject.AddComponent<BloodBullet>().Initialize(bullet);
				}, new Vector3?(bloodSpell.spellCaster.magic.position), new Quaternion?(Quaternion.Euler(bloodSpell.spellCaster.magic.forward)), null, false, null);
				flag2 = true;
			}
			return flag2;
		}
	}
}
