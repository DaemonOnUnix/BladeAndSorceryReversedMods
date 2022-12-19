using System;
using ThunderRoad;
using UnityEngine;

namespace SoulFireSpell
{
	// Token: 0x02000004 RID: 4
	public class SoulFireMerge : SpellMergeData
	{
		// Token: 0x06000009 RID: 9 RVA: 0x00002538 File Offset: 0x00000738
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
					bool flag2 = Creature.allActive == null;
					if (flag2)
					{
						break;
					}
					bool isKilled = creature.isKilled;
					if (isKilled)
					{
						SoulFirePossess.SoulResurrect(creature);
					}
				}
			}
		}

		// Token: 0x04000006 RID: 6
		public bool updateProperties;
	}
}
