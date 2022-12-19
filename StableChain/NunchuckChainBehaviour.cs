using System;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

namespace StableChain
{
	// Token: 0x02000005 RID: 5
	public class NunchuckChainBehaviour : MonoBehaviour
	{
		// Token: 0x06000009 RID: 9 RVA: 0x00002AF0 File Offset: 0x00000CF0
		protected void Start()
		{
			this.mainItem = base.GetComponent<Item>();
			this.mainItem.OnHeldActionEvent += new Item.HeldActionDelegate(this.HeldAction);
			try
			{
				this.root = this.mainItem.GetCustomReference("root", true).GetComponent<Transform>();
			}
			catch
			{
			}
			try
			{
				this.link1 = this.mainItem.GetCustomReference("1", true).GetComponent<Transform>();
			}
			catch
			{
			}
			try
			{
				this.link2 = this.mainItem.GetCustomReference("2", true).GetComponent<Transform>();
			}
			catch
			{
			}
			try
			{
				this.link3 = this.mainItem.GetCustomReference("3", true).GetComponent<Transform>();
			}
			catch
			{
			}
			try
			{
				this.link4 = this.mainItem.GetCustomReference("4", true).GetComponent<Transform>();
			}
			catch
			{
			}
			this.link1Dis = Vector3.Distance(this.link1.position, this.root.position) + 0.001f;
			this.link2Dis = Vector3.Distance(this.link2.position, this.link1.position) + 0.001f;
			this.link3Dis = Vector3.Distance(this.link3.position, this.root.position) + 0.001f;
			this.link4Dis = Vector3.Distance(this.link4.position, this.link3.position) + 0.001f;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002CBC File Offset: 0x00000EBC
		private void HeldAction(RagdollHand ragdollHand, Handle handle, Interactable.Action action)
		{
			bool flag = action == 0;
			if (flag)
			{
				bool flag2 = action != 1;
				if (flag2)
				{
				}
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002CE0 File Offset: 0x00000EE0
		private void FixedUpdate()
		{
			bool flag = this.link1 != null;
			if (flag)
			{
				float num = Vector3.Distance(this.link1.position, this.root.position);
				bool flag2 = num > this.link1Dis;
				if (flag2)
				{
					Vector3 vector = (this.link1.position - this.root.position).normalized;
					vector *= this.link1Dis - num;
					this.link1.position += vector;
				}
			}
			bool flag3 = this.link2 != null;
			if (flag3)
			{
				float num2 = Vector3.Distance(this.link2.position, this.link1.position);
				bool flag4 = num2 > this.link2Dis;
				if (flag4)
				{
					Vector3 vector2 = (this.link2.position - this.link1.position).normalized;
					vector2 *= this.link2Dis - num2;
					this.link2.position += vector2;
				}
			}
			bool flag5 = this.link3 != null;
			if (flag5)
			{
				float num3 = Vector3.Distance(this.link3.position, this.link2.position);
				bool flag6 = num3 > this.link3Dis;
				if (flag6)
				{
					Vector3 vector3 = (this.link3.position - this.link2.position).normalized;
					vector3 *= this.link3Dis - num3;
					this.link3.position += vector3;
				}
			}
			bool flag7 = this.link4 != null;
			if (flag7)
			{
				float num4 = Vector3.Distance(this.link4.position, this.link3.position);
				bool flag8 = num4 > this.link4Dis;
				if (flag8)
				{
					Vector3 vector4 = (this.link4.position - this.link3.position).normalized;
					vector4 *= this.link4Dis - num4;
					this.link4.position += vector4;
				}
			}
			new WaitForSeconds(0.2f);
		}

		// Token: 0x0400001C RID: 28
		private List<NunchuckChainBehaviour> others = new List<NunchuckChainBehaviour>();

		// Token: 0x0400001D RID: 29
		private Item item;

		// Token: 0x0400001E RID: 30
		private Item chainRigidbody;

		// Token: 0x0400001F RID: 31
		private Item mainItem;

		// Token: 0x04000020 RID: 32
		private Transform root;

		// Token: 0x04000021 RID: 33
		private Transform link1;

		// Token: 0x04000022 RID: 34
		private Transform link2;

		// Token: 0x04000023 RID: 35
		private Transform link3;

		// Token: 0x04000024 RID: 36
		private Transform link4;

		// Token: 0x04000025 RID: 37
		private float link1Dis;

		// Token: 0x04000026 RID: 38
		private float link2Dis;

		// Token: 0x04000027 RID: 39
		private float link3Dis;

		// Token: 0x04000028 RID: 40
		private float link4Dis;

		// Token: 0x04000029 RID: 41
		public List<Rigidbody> parts = new List<Rigidbody>();
	}
}
