using System;
using System.Linq;
using ThunderRoad;
using UnityEngine;

namespace Shatterblade
{
	// Token: 0x02000011 RID: 17
	internal class SwordMode : BladeMode
	{
		// Token: 0x06000104 RID: 260 RVA: 0x00008249 File Offset: 0x00006449
		public override bool Test(Shatterblade sword)
		{
			return false;
		}

		// Token: 0x06000105 RID: 261 RVA: 0x0000824C File Offset: 0x0000644C
		public override void Enter(Shatterblade sword)
		{
			base.Enter(sword);
			sword.ReformParts();
		}

		// Token: 0x06000106 RID: 262 RVA: 0x0000825E File Offset: 0x0000645E
		public override bool ShouldHideWhenHolstered(BladePart part)
		{
			return true;
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00008264 File Offset: 0x00006464
		public override void Update()
		{
			base.Update();
			bool locking = this.sword.locking;
			if (locking)
			{
				this.sword.handleAnnotationA.SetText("Hold [[BUTTON]] to expand\nthe blade", null);
				this.sword.gunShardAnnotation.SetText("Grab this shard to\nmake a handgun!", null);
				this.sword.sawShardAnnotation.SetText("Grab this shard to\nmake a buzz saw!", null);
				bool flag = this.sword.item.handlers.Count<RagdollHand>() == 1;
				if (flag)
				{
					this.sword.otherHandAnnotation.SetTarget(this.sword.item.mainHandler.otherHand.transform);
					this.sword.otherHandAnnotation.offset = new Vector3(-1f, (float)((this.sword.item.mainHandler.otherHand.side == null) ? 1 : (-1)), 0f);
					bool flag2 = this.sword.item.mainHandler.otherHand.caster.spellInstance is SpellCastCharge;
					if (flag2)
					{
						bool flag3 = this.sword.item.mainHandler.otherHand.caster.spellInstance is SpellCastLightning;
						if (flag3)
						{
							this.sword.otherHandAnnotation.SetText("Arc Cannon (Lightning spell) selected!\nLook back at the blade", null);
							this.sword.imbueHandleAnnotation.SetText("Grab this shard for Arc Cannon mode", null);
						}
						else
						{
							bool flag4 = this.sword.item.mainHandler.otherHand.caster.spellInstance is SpellCastProjectile;
							if (flag4)
							{
								this.sword.otherHandAnnotation.SetText("Flamethrower (Fire spell) selected!\nLook back at the blade", null);
								this.sword.imbueHandleAnnotation.SetText("Grab this shard for Flamethrower mode", null);
							}
							else
							{
								bool flag5 = this.sword.item.mainHandler.otherHand.caster.spellInstance is SpellCastGravity;
								if (flag5)
								{
									this.sword.otherHandAnnotation.SetText("Gravity Gun (Gravity spell) selected!\nLook back at the blade", null);
									this.sword.imbueHandleAnnotation.SetText("Grab this shard for Gravity Gun mode", null);
								}
							}
						}
					}
					else
					{
						this.sword.imbueHandleAnnotation.Hide();
						this.sword.otherHandAnnotation.SetText("Select Fire, Lightning, or Gravity with\nthis hand to form a weapon", null);
					}
				}
				else
				{
					this.sword.otherHandAnnotation.Hide();
				}
			}
			else
			{
				this.sword.handleAnnotationA.Hide();
			}
			this.sword.handleAnnotationB.SetText("Tap [[BUTTON]] to " + (this.sword.locking ? "Shatter" : "Reform") + "\n the blade", null);
		}
	}
}
