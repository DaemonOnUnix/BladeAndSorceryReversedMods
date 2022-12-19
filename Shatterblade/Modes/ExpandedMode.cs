using System;
using UnityEngine;

namespace Shatterblade.Modes
{
	// Token: 0x02000019 RID: 25
	internal class ExpandedMode : BladeMode
	{
		// Token: 0x060001A1 RID: 417 RVA: 0x0000B7AD File Offset: 0x000099AD
		public override bool Test(Shatterblade sword)
		{
			return false;
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x0000B7B0 File Offset: 0x000099B0
		public override void Enter(Shatterblade sword)
		{
			base.Enter(sword);
			sword.animator.SetBool(ExpandedMode.IsExpanded, true);
			sword.ReformParts();
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x0000B7D4 File Offset: 0x000099D4
		public override void Update()
		{
			base.Update();
			this.sword.handleAnnotationA.SetText("Release [[BUTTON]] to retract the blade", null);
			this.sword.handleAnnotationB.SetText("Hold Trigger to form a shield", null);
			this.sword.imbueHandleAnnotation.Hide();
			this.sword.otherHandAnnotation.Hide();
			this.sword.gunShardAnnotation.Hide();
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x0000B84A File Offset: 0x00009A4A
		public override void Exit()
		{
			base.Exit();
			this.sword.animator.SetBool("IsExpanded", false);
		}

		// Token: 0x04000086 RID: 134
		private static readonly int IsExpanded = Animator.StringToHash("IsExpanded");
	}
}
