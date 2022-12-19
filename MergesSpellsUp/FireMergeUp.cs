using System;
using ThunderRoad;

namespace MergesSpellsUp
{
	// Token: 0x02000004 RID: 4
	public class FireMergeUp : SpellCastCharge
	{
		// Token: 0x0600007B RID: 123 RVA: 0x0000626C File Offset: 0x0000446C
		public override void Fire(bool active)
		{
			base.Fire(active);
			bool flag = !active && this.currentCharge >= 0.5f && this.spellCaster.ragdollHand.Velocity().magnitude > 1.5f;
			if (flag)
			{
				this.spellCaster.ragdollHand.transform.position.ThrowMeteor(this.spellCaster.ragdollHand.Velocity(), Player.local.creature, true, 1f, 0.5f, false);
				this.Throw(this.spellCaster.ragdollHand.Velocity());
			}
		}

		// Token: 0x04000009 RID: 9
		public bool isCasting;
	}
}
