using System;
using UnityEngine;

namespace Shatterblade.Modes
{
	// Token: 0x02000012 RID: 18
	internal class CircularSawMode : GrabbedShardMode
	{
		// Token: 0x06000109 RID: 265 RVA: 0x00008543 File Offset: 0x00006743
		public override int TargetPartNum()
		{
			return 12;
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00008547 File Offset: 0x00006747
		public override Vector3 Center()
		{
			return base.Center() + this.ForwardDir() * 0.4f;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00008564 File Offset: 0x00006764
		public override Vector3 GetPos(int index, Rigidbody rb, BladePart part)
		{
			bool flag = index > 9;
			Vector3 vector;
			if (flag)
			{
				vector = this.Center() + Quaternion.AngleAxis((float)(index - 10) * 360f / 5f - this.rotation / 3f, this.SideDir()) * this.UpDir() * this.sizeMultiplier * 0.1f;
			}
			else
			{
				vector = this.Center() + Quaternion.AngleAxis((float)index * 360f / 9f + this.rotation, this.SideDir()) * this.UpDir() * this.sizeMultiplier * (base.IsTriggerPressed() ? 0.2f : 0.25f);
			}
			return vector;
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00008630 File Offset: 0x00006830
		public override Quaternion GetRot(int index, Rigidbody rb, BladePart part)
		{
			return Quaternion.LookRotation(rb.transform.position - this.Center(), this.SideDir());
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00008653 File Offset: 0x00006853
		public override string GetUseAnnotation()
		{
			return "Press trigger to spin";
		}

		// Token: 0x0600010E RID: 270 RVA: 0x0000865A File Offset: 0x0000685A
		public override bool GetUseAnnotationShown()
		{
			return !base.IsTriggerPressed();
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00008668 File Offset: 0x00006868
		public override void Update()
		{
			base.Update();
			bool flag = base.IsTriggerPressed();
			if (flag)
			{
				this.rotation += Time.deltaTime * 700f * this.speedMultiplier;
			}
			else
			{
				this.rotation += Time.deltaTime * 80f * this.speedMultiplier;
			}
		}

		// Token: 0x04000052 RID: 82
		public float speedMultiplier = 1f;

		// Token: 0x04000053 RID: 83
		public float sizeMultiplier = 1f;

		// Token: 0x04000054 RID: 84
		private float rotation;
	}
}
