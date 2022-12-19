using System;
using ExtensionMethods;

namespace Shatterblade
{
	// Token: 0x02000010 RID: 16
	internal class ShieldMode : BladeMode
	{
		// Token: 0x06000100 RID: 256 RVA: 0x00008166 File Offset: 0x00006366
		public override bool Test(Shatterblade sword)
		{
			return false;
		}

		// Token: 0x06000101 RID: 257 RVA: 0x0000816C File Offset: 0x0000636C
		public override void Enter(Shatterblade sword)
		{
			base.Enter(sword);
			sword.animator.SetBool("IsExpanded", false);
			sword.animator.SetBool("IsLeft", sword.buttonHand.PalmDir().IsFacing(sword.item.transform.forward, 50f));
			sword.animator.SetBool("IsShield", true);
			sword.ReformParts();
			sword.handleAnnotationA.Hide();
			sword.handleAnnotationB.Hide();
			sword.imbueHandleAnnotation.Hide();
			sword.otherHandAnnotation.Hide();
			sword.gunShardAnnotation.Hide();
		}

		// Token: 0x06000102 RID: 258 RVA: 0x0000821F File Offset: 0x0000641F
		public override void Exit()
		{
			base.Exit();
			this.sword.animator.SetBool("IsShield", false);
		}
	}
}
