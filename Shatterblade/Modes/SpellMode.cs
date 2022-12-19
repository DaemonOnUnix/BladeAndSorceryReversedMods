using System;
using ThunderRoad;

namespace Shatterblade.Modes
{
	// Token: 0x02000016 RID: 22
	public abstract class SpellMode<T> : GrabbedShardMode where T : SpellCastCharge
	{
		// Token: 0x06000170 RID: 368 RVA: 0x0000A638 File Offset: 0x00008838
		public override int TargetPartNum()
		{
			return 11;
		}

		// Token: 0x06000171 RID: 369 RVA: 0x0000A63C File Offset: 0x0000883C
		public T Spell()
		{
			return this.Hand().caster.spellInstance as T;
		}

		// Token: 0x06000172 RID: 370 RVA: 0x0000A658 File Offset: 0x00008858
		public override bool Test(Shatterblade sword)
		{
			BladePart part = sword.GetPart(this.TargetPartNum());
			object obj;
			if (part == null)
			{
				obj = null;
			}
			else
			{
				Item item = part.item;
				if (item == null)
				{
					obj = null;
				}
				else
				{
					RagdollHand mainHandler = item.mainHandler;
					obj = ((mainHandler != null) ? mainHandler.caster.spellInstance : null);
				}
			}
			return obj is T;
		}
	}
}
