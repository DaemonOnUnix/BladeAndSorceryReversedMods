using System;
using System.Threading.Tasks;
using ThunderRoad;
using UnityEngine;

namespace SoulFireSpell
{
	// Token: 0x0200000A RID: 10
	public class SFGravityMerge : SpellMergeData
	{
		// Token: 0x06000016 RID: 22 RVA: 0x00002CA8 File Offset: 0x00000EA8
		public override void Merge(bool active)
		{
			base.Merge(active);
			Vector3 from = Player.local.transform.rotation * PlayerControl.GetHand(1).GetHandVelocity();
			Vector3 from2 = Player.local.transform.rotation * PlayerControl.GetHand(0).GetHandVelocity();
			bool flag = (double)from.magnitude <= (double)SpellCaster.throwMinHandVelocity || (double)from2.magnitude <= (double)SpellCaster.throwMinHandVelocity || ((double)Vector3.Angle(from, this.mana.casterLeft.magicSource.position - this.mana.mergePoint.position) >= 45.0 && (double)Vector3.Angle(from2, this.mana.casterRight.magicSource.position - this.mana.mergePoint.position) >= 45.0);
			if (!flag)
			{
				foreach (Creature creature in Creature.allActive)
				{
					this.GravityWeight(creature);
				}
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002DF8 File Offset: 0x00000FF8
		public async void GravityWeight(Creature creature)
		{
			bool flag = creature && !creature.isKilled && !creature.isPlayer;
			if (flag)
			{
				creature.ragdoll.SetState(1);
				creature.brain.AddNoStandUpModifier(creature);
				creature.ragdoll.rootPart.rb.mass = creature.ragdoll.rootPart.rb.mass * 5f;
				await Task.Delay(10000);
				creature.ragdoll.rootPart.rb.mass = creature.ragdoll.rootPart.rb.mass / 5f;
				creature.brain.RemoveNoStandUpModifier(creature);
			}
		}
	}
}
