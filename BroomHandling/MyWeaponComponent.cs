using System;
using ThunderRoad;
using UnityEngine;

namespace BroomHandling
{
	// Token: 0x02000003 RID: 3
	internal class MyWeaponComponent : MonoBehaviour
	{
		// Token: 0x06000003 RID: 3 RVA: 0x00002070 File Offset: 0x00000270
		private void Start()
		{
			this.item = base.GetComponent<Item>();
			this.multiplier = 2.5f;
			this.anchors[0] = GameObject.Find("Nimbus2000/FrontAnchor1").GetComponent<Transform>();
			this.anchors[1] = GameObject.Find("/FrontAnchor2").GetComponent<Transform>();
			this.anchors[3] = GameObject.Find("/BackAnchor1").GetComponent<Transform>();
			this.anchors[3] = GameObject.Find("/BackAnchor2").GetComponent<Transform>();
			bool flag = GameObject.Find("Nimbus2000/FrontAnchor1") != null;
			if (flag)
			{
				Debug.Log("Found: FrontAnchor1");
			}
			for (int i = 0; i < 4; i++)
			{
				Debug.Log(this.anchors[i]);
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002132 File Offset: 0x00000332
		private void Update()
		{
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002138 File Offset: 0x00000338
		private void FixedUpdate()
		{
			for (int i = 0; i < 4; i++)
			{
				this.ApplyForce(this.anchors[i], this.hits[i]);
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002174 File Offset: 0x00000374
		private void ApplyForce(Transform anchors, RaycastHit hit)
		{
			bool flag = Physics.Raycast(anchors.position, -anchors.up, ref hit);
			if (flag)
			{
				float num = Mathf.Abs(1f / (hit.point.y - anchors.position.y));
				this.item.rb.AddForceAtPosition(this.item.transform.up * num * this.multiplier, anchors.position, 5);
			}
		}

		// Token: 0x04000003 RID: 3
		private Item item;

		// Token: 0x04000004 RID: 4
		private float hoverHeight;

		// Token: 0x04000005 RID: 5
		private bool isMounted;

		// Token: 0x04000006 RID: 6
		public float multiplier;

		// Token: 0x04000007 RID: 7
		public float moveForce;

		// Token: 0x04000008 RID: 8
		public float turnTorque;

		// Token: 0x04000009 RID: 9
		public Transform[] anchors;

		// Token: 0x0400000A RID: 10
		public RaycastHit[] hits = new RaycastHit[4];
	}
}
