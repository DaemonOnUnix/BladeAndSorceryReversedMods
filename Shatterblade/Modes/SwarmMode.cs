using System;
using System.Collections.Generic;
using ExtensionMethods;
using UnityEngine;

namespace Shatterblade.Modes
{
	// Token: 0x0200001A RID: 26
	internal class SwarmMode : GrabbedShardMode
	{
		// Token: 0x060001A7 RID: 423 RVA: 0x0000B885 File Offset: 0x00009A85
		public override Vector3 Center()
		{
			return base.Center() + this.ForwardDir() * 1.5f * (1f + this.Hand().playerHand.controlHand.useAxis);
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x0000B8C4 File Offset: 0x00009AC4
		public override void Enter(Shatterblade sword)
		{
			base.Enter(sword);
			this.offsets = new Dictionary<int, Vector3>();
			this.axes = new Dictionary<int, Vector3>();
			for (int i = 1; i < 16; i++)
			{
				this.offsets[i] = Utils.RandomVector(-1f, 1f, 0);
				this.axes[i] = Utils.RandomVector(-1f, 1f, 0);
			}
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x0000B93D File Offset: 0x00009B3D
		public override int TargetPartNum()
		{
			return 13;
		}

		// Token: 0x060001AA RID: 426 RVA: 0x0000B944 File Offset: 0x00009B44
		public override Vector3 GetPos(int index, Rigidbody rb, BladePart part)
		{
			float size = (base.IsButtonPressed() ? 0.5f : 1f);
			Vector3 pos = rb.gameObject.UniqueVector(-size, size, 0);
			Vector3 normal = part.item.gameObject.UniqueVector(-size, size, 1);
			Quaternion handAngle = Quaternion.LookRotation(this.SideDir());
			return this.Center() + handAngle * pos.Rotated(Quaternion.AngleAxis(Time.time * 120f, normal), default(Vector3));
		}

		// Token: 0x060001AB RID: 427 RVA: 0x0000B9D0 File Offset: 0x00009BD0
		public override Quaternion GetRot(int index, Rigidbody rb, BladePart part)
		{
			float size = (base.IsButtonPressed() ? 0.5f : 1f);
			Vector3 pos = rb.gameObject.UniqueVector(-size, size, 0);
			pos = pos.normalized * (pos.magnitude + 0.2f);
			Vector3 normal = part.item.gameObject.UniqueVector(-size, size, 1);
			Quaternion handAngle = Quaternion.LookRotation(this.SideDir());
			Vector3 facingDir = this.Center() + handAngle * pos.Rotated(Quaternion.AngleAxis(this.rotation, normal), default(Vector3)) - rb.transform.position;
			return Quaternion.LookRotation((this.Hand().Velocity() + facingDir).normalized);
		}

		// Token: 0x060001AC RID: 428 RVA: 0x0000BAA3 File Offset: 0x00009CA3
		public override bool ShouldLock(BladePart part)
		{
			return false;
		}

		// Token: 0x060001AD RID: 429 RVA: 0x0000BAA8 File Offset: 0x00009CA8
		public override void JointModifier(ConfigurableJoint joint, BladePart part)
		{
			JointDrive posDrive = joint.xDrive;
			posDrive.positionSpring = 100f;
			posDrive.positionDamper = 10f;
			posDrive.maximumForce = 1000f;
			joint.xDrive = posDrive;
			joint.yDrive = posDrive;
			joint.zDrive = posDrive;
		}

		// Token: 0x060001AE RID: 430 RVA: 0x0000BAFC File Offset: 0x00009CFC
		public override void Update()
		{
			base.Update();
			this.rotation += Time.deltaTime * 200f * (1f + this.Hand().Velocity().magnitude / 3f);
		}

		// Token: 0x04000087 RID: 135
		private Dictionary<int, Vector3> offsets;

		// Token: 0x04000088 RID: 136
		private Dictionary<int, Vector3> axes;

		// Token: 0x04000089 RID: 137
		private float rotation;
	}
}
