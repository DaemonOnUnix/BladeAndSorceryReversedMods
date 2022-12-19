using System;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

namespace StableChain
{
	// Token: 0x02000007 RID: 7
	public class ThalDarasChainBehaviour : MonoBehaviour
	{
		// Token: 0x0600000F RID: 15 RVA: 0x00002FB8 File Offset: 0x000011B8
		protected void Start()
		{
			this.mainItem = base.GetComponent<Item>();
			this.mainItem.OnHeldActionEvent += new Item.HeldActionDelegate(this.HeldAction);
			try
			{
				this.root1 = this.mainItem.GetCustomReference("root1", true).GetComponent<Transform>();
			}
			catch
			{
			}
			try
			{
				this.root2 = this.mainItem.GetCustomReference("root2", true).GetComponent<Transform>();
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
			try
			{
				this.link5 = this.mainItem.GetCustomReference("5", true).GetComponent<Transform>();
			}
			catch
			{
			}
			try
			{
				this.link6 = this.mainItem.GetCustomReference("6", true).GetComponent<Transform>();
			}
			catch
			{
			}
			try
			{
				this.link7 = this.mainItem.GetCustomReference("7", true).GetComponent<Transform>();
			}
			catch
			{
			}
			try
			{
				this.link8 = this.mainItem.GetCustomReference("8", true).GetComponent<Transform>();
			}
			catch
			{
			}
			try
			{
				this.link9 = this.mainItem.GetCustomReference("9", true).GetComponent<Transform>();
			}
			catch
			{
			}
			this.link1Dis = Vector3.Distance(this.link1.position, this.root1.position) + 0.001f;
			this.link2Dis = Vector3.Distance(this.link2.position, this.link1.position) + 0.001f;
			this.link3Dis = Vector3.Distance(this.link3.position, this.link2.position) + 0.001f;
			this.link4Dis = Vector3.Distance(this.link4.position, this.link3.position) + 0.001f;
			this.link5Dis = Vector3.Distance(this.link5.position, this.link4.position) + 0.001f;
			this.link6Dis = Vector3.Distance(this.link6.position, this.root2.position) + 0.001f;
			this.link7Dis = Vector3.Distance(this.link7.position, this.link6.position) + 0.001f;
			this.link8Dis = Vector3.Distance(this.link8.position, this.link7.position) + 0.001f;
			this.link9Dis = Vector3.Distance(this.link9.position, this.link8.position) + 0.001f;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00003374 File Offset: 0x00001574
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

		// Token: 0x06000011 RID: 17 RVA: 0x00003398 File Offset: 0x00001598
		private void FixedUpdate()
		{
			bool flag = this.link1 != null;
			if (flag)
			{
				float num = Vector3.Distance(this.link1.position, this.root1.position);
				bool flag2 = num > this.link1Dis;
				if (flag2)
				{
					Vector3 vector = (this.link1.position - this.root1.position).normalized;
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
			bool flag9 = this.link5 != null;
			if (flag9)
			{
				float num5 = Vector3.Distance(this.link5.position, this.link4.position);
				bool flag10 = num5 > this.link5Dis;
				if (flag10)
				{
					Vector3 vector5 = (this.link5.position - this.link4.position).normalized;
					vector5 *= this.link5Dis - num5;
					this.link5.position += vector5;
				}
			}
			bool flag11 = this.link6 != null;
			if (flag11)
			{
				float num6 = Vector3.Distance(this.link6.position, this.root2.position);
				bool flag12 = num6 > this.link6Dis;
				if (flag12)
				{
					Vector3 vector6 = (this.link6.position - this.root2.position).normalized;
					vector6 *= this.link6Dis - num6;
					this.link6.position += vector6;
				}
			}
			bool flag13 = this.link7 != null;
			if (flag13)
			{
				float num7 = Vector3.Distance(this.link7.position, this.link6.position);
				bool flag14 = num7 > this.link7Dis;
				if (flag14)
				{
					Vector3 vector7 = (this.link7.position - this.link6.position).normalized;
					vector7 *= this.link7Dis - num7;
					this.link7.position += vector7;
				}
			}
			bool flag15 = this.link8 != null;
			if (flag15)
			{
				float num8 = Vector3.Distance(this.link8.position, this.link7.position);
				bool flag16 = num8 > this.link8Dis;
				if (flag16)
				{
					Vector3 vector8 = (this.link8.position - this.link7.position).normalized;
					vector8 *= this.link8Dis - num8;
					this.link8.position += vector8;
				}
			}
			bool flag17 = this.link9 != null;
			if (flag17)
			{
				float num9 = Vector3.Distance(this.link9.position, this.link8.position);
				bool flag18 = num9 > this.link9Dis;
				if (flag18)
				{
					Vector3 vector9 = (this.link9.position - this.link8.position).normalized;
					vector9 *= this.link9Dis - num9;
					this.link9.position += vector9;
				}
			}
			new WaitForSeconds(0.2f);
		}

		// Token: 0x0400002A RID: 42
		private List<ThalDarasChainBehaviour> others = new List<ThalDarasChainBehaviour>();

		// Token: 0x0400002B RID: 43
		private Item mainItem;

		// Token: 0x0400002C RID: 44
		private Transform root1;

		// Token: 0x0400002D RID: 45
		private Transform root2;

		// Token: 0x0400002E RID: 46
		private Transform link1;

		// Token: 0x0400002F RID: 47
		private Transform link2;

		// Token: 0x04000030 RID: 48
		private Transform link3;

		// Token: 0x04000031 RID: 49
		private Transform link4;

		// Token: 0x04000032 RID: 50
		private Transform link5;

		// Token: 0x04000033 RID: 51
		private Transform link6;

		// Token: 0x04000034 RID: 52
		private Transform link7;

		// Token: 0x04000035 RID: 53
		private Transform link8;

		// Token: 0x04000036 RID: 54
		private Transform link9;

		// Token: 0x04000037 RID: 55
		private float link1Dis;

		// Token: 0x04000038 RID: 56
		private float link2Dis;

		// Token: 0x04000039 RID: 57
		private float link3Dis;

		// Token: 0x0400003A RID: 58
		private float link4Dis;

		// Token: 0x0400003B RID: 59
		private float link5Dis;

		// Token: 0x0400003C RID: 60
		private float link6Dis;

		// Token: 0x0400003D RID: 61
		private float link7Dis;

		// Token: 0x0400003E RID: 62
		private float link8Dis;

		// Token: 0x0400003F RID: 63
		private float link9Dis;

		// Token: 0x04000040 RID: 64
		public List<Rigidbody> parts = new List<Rigidbody>();
	}
}
