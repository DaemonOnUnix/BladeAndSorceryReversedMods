using System;
using ThunderRoad;
using UnityEngine;

namespace Shatterblade
{
	// Token: 0x0200000D RID: 13
	public abstract class BladeMode : ItemModule
	{
		// Token: 0x060000D0 RID: 208 RVA: 0x00006BD4 File Offset: 0x00004DD4
		public BladeMode Clone()
		{
			return (BladeMode)base.MemberwiseClone();
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00006BF4 File Offset: 0x00004DF4
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			Shatterblade shatterblade = item.GetComponent<Shatterblade>();
			bool flag = shatterblade != null;
			if (flag)
			{
				shatterblade.RegisterMode(this);
			}
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00006C23 File Offset: 0x00004E23
		public virtual float Priority()
		{
			return -1f;
		}

		// Token: 0x060000D3 RID: 211
		public abstract bool Test(Shatterblade sword);

		// Token: 0x060000D4 RID: 212 RVA: 0x00006C2A File Offset: 0x00004E2A
		public virtual void Enter(Shatterblade sword)
		{
			this.sword = sword;
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00006C33 File Offset: 0x00004E33
		public virtual void Update()
		{
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00006C36 File Offset: 0x00004E36
		public virtual void Exit()
		{
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00006C39 File Offset: 0x00004E39
		public virtual bool ShouldReform(BladePart part)
		{
			return true;
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00006C3C File Offset: 0x00004E3C
		public virtual bool ShouldLock(BladePart part)
		{
			return true;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00006C3F File Offset: 0x00004E3F
		public virtual bool ShouldHideWhenHolstered(BladePart part)
		{
			return false;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00006C42 File Offset: 0x00004E42
		public virtual void JointModifier(ConfigurableJoint joint, BladePart part)
		{
		}

		// Token: 0x0400003D RID: 61
		public Shatterblade sword;
	}
}
