using System;
using ThunderRoad;
using UnityEngine;

namespace MeteorFall
{
	// Token: 0x02000002 RID: 2
	public class MeteorSpell : SpellCastProjectile
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public override void Fire(bool active)
		{
			base.Fire(active);
			if (active)
			{
				this.spellCaster.SetMagicOffset(new Vector3(-0.04f, 0.2f, -0.07f));
			}
		}
	}
}
