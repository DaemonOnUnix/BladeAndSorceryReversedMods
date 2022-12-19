using System;
using System.Diagnostics;
using BloodMagic.UI;
using ThunderRoad;
using UnityEngine;
using UnityEngine.VFX;

namespace BloodMagic.Spell.Abilities
{
	// Token: 0x0200001C RID: 28
	public static class BloodDrain
	{
		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000095 RID: 149 RVA: 0x0000442C File Offset: 0x0000262C
		// (remove) Token: 0x06000096 RID: 150 RVA: 0x00004460 File Offset: 0x00002660
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event BloodDrain.DrainAction OnDrain;

		// Token: 0x06000097 RID: 151 RVA: 0x00004494 File Offset: 0x00002694
		public static bool TryToActivate(BloodSpell bloodSpell, Vector3 velocity, SaveData saveData)
		{
			RaycastHit raycastHit;
			bool flag = Physics.Raycast(bloodSpell.spellCaster.magic.position, bloodSpell.spellCaster.magic.forward, ref raycastHit) && raycastHit.collider.GetComponentInParent<Creature>() && (double)raycastHit.distance < (double)saveData.drainDistance;
			if (flag)
			{
				Creature componentInParent = raycastHit.collider.GetComponentInParent<Creature>();
				bool flag2 = !componentInParent.isPlayer && componentInParent.isKilled && (double)Player.currentCreature.currentHealth < (double)Player.currentCreature.maxHealth;
				if (flag2)
				{
					CreatureDrainComponent component = componentInParent.GetComponent<CreatureDrainComponent>();
					bool flag3 = component && (double)component.health > 0.0;
					if (flag3)
					{
						component.health -= BookUIHandler.saveData.drainPower * Time.deltaTime;
						BloodDrain.DrainHealth(BookUIHandler.saveData.drainPower * Time.deltaTime, bloodSpell, componentInParent);
						return true;
					}
					componentInParent.Despawn();
				}
			}
			return false;
		}

		// Token: 0x06000098 RID: 152 RVA: 0x000045B4 File Offset: 0x000027B4
		public static void DrainHealth(float health, BloodSpell bloodSpell, Creature creature)
		{
			bool flag = BloodDrain.OnDrain != null;
			if (flag)
			{
				BloodDrain.OnDrain(health);
			}
			bool flag2 = !BloodDrain.drainEffectLeft;
			if (flag2)
			{
				BloodDrain.drainEffectLeft = Catalog.GetData<EffectData>("BloodDrain", true).Spawn(Vector3.zero, Quaternion.identity, null, null, false, Array.Empty<Type>()).effects[0].GetComponent<VisualEffect>();
				BloodDrain.drainEffectLeft.transform.SetParent(null);
			}
			bool flag3 = !BloodDrain.drainEffectRight;
			if (flag3)
			{
				BloodDrain.drainEffectRight = Catalog.GetData<EffectData>("BloodDrain", true).Spawn(Vector3.zero, Quaternion.identity, null, null, false, Array.Empty<Type>()).effects[0].GetComponent<VisualEffect>();
				BloodDrain.drainEffectRight.transform.SetParent(null);
			}
			bool flag4 = !(creature != null);
			if (!flag4)
			{
				bool flag5 = bloodSpell.spellCaster.ragdollHand.side == 1;
				if (flag5)
				{
					BloodDrain.drainEffectLeft.transform.position = creature.ragdoll.GetPart(new RagdollPart.Type[] { 2 }).transform.position;
				}
				else
				{
					BloodDrain.drainEffectRight.transform.position = creature.ragdoll.GetPart(new RagdollPart.Type[] { 2 }).transform.position;
				}
				Player.currentCreature.Heal(health, Player.currentCreature);
			}
		}

		// Token: 0x0400003C RID: 60
		public static VisualEffect drainEffectLeft;

		// Token: 0x0400003D RID: 61
		public static VisualEffect drainEffectRight;

		// Token: 0x02000035 RID: 53
		// (Invoke) Token: 0x06000103 RID: 259
		public delegate void DrainAction(float drainedHealth);
	}
}
