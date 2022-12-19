using System;
using ThunderRoad;
using UnityEngine;

namespace WandSpellss
{
	// Token: 0x02000007 RID: 7
	public class Engorgio : MonoBehaviour
	{
		// Token: 0x06000012 RID: 18 RVA: 0x000024C0 File Offset: 0x000006C0
		public void Start()
		{
			this.item = base.GetComponent<Item>();
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000024D0 File Offset: 0x000006D0
		internal void CastRay()
		{
			RaycastHit hit;
			bool flag = Physics.Raycast(this.item.flyDirRef.position, this.item.flyDirRef.forward, ref hit);
			if (flag)
			{
				Debug.Log("Did hit.");
				Debug.Log(hit.collider.gameObject.transform.parent.name);
				Transform parent = hit.collider.gameObject.transform.parent;
				this.ogScale = parent.gameObject.transform.localScale;
				this.parentLocal = hit.collider.gameObject;
				bool flag2 = this.parentLocal.GetComponentInParent<Item>() != null;
				if (flag2)
				{
					this.cantEngorgio = false;
					this.parentLocal.AddComponent<EngorgioPerItem>();
				}
				else
				{
					bool flag3 = this.parentLocal.GetComponent<Item>() != null;
					if (flag3)
					{
						this.cantEngorgio = false;
						this.parentLocal.AddComponent<EngorgioPerItem>();
					}
					else
					{
						this.cantEngorgio = true;
					}
				}
			}
		}

		// Token: 0x04000013 RID: 19
		private Item item;

		// Token: 0x04000014 RID: 20
		private Item npcItem;

		// Token: 0x04000015 RID: 21
		internal Vector3 startPoint;

		// Token: 0x04000016 RID: 22
		internal Vector3 endPoint;

		// Token: 0x04000017 RID: 23
		internal GameObject parentLocal;

		// Token: 0x04000018 RID: 24
		internal Vector3 ogScale;

		// Token: 0x04000019 RID: 25
		internal bool cantEngorgio;

		// Token: 0x0400001A RID: 26
		private float elapsedTime;

		// Token: 0x0400001B RID: 27
		private float duration;

		// Token: 0x0400001C RID: 28
		private Vector3 engorgioMaxSize;
	}
}
