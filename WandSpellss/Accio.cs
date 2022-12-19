using System;
using ThunderRoad;
using UnityEngine;

namespace WandSpellss
{
	// Token: 0x02000005 RID: 5
	public class Accio : MonoBehaviour
	{
		// Token: 0x0600000A RID: 10 RVA: 0x00002174 File Offset: 0x00000374
		public void Start()
		{
			this.item = base.GetComponent<Item>();
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002184 File Offset: 0x00000384
		internal void CastRay()
		{
			RaycastHit hit;
			bool flag = Physics.Raycast(this.item.flyDirRef.position, this.item.flyDirRef.forward, ref hit);
			if (flag)
			{
				Debug.Log("Did hit.");
				Debug.Log(hit.collider.gameObject.transform.parent.name);
				Transform parent = hit.collider.gameObject.transform.parent;
				this.parentLocal = hit.collider.gameObject;
				bool flag2 = this.parentLocal.gameObject.GetComponent<Item>() != null && this.item.mainHandler.otherHand.grabbedHandle == null;
				if (flag2)
				{
					this.startPoint = parent.gameObject.transform.position;
					this.endPoint = Player.local.handLeft.transform.position;
					this.cantAccio = false;
				}
				else
				{
					bool flag3 = this.parentLocal.gameObject.GetComponentInParent<Item>() != null && this.item.mainHandler.otherHand.grabbedHandle == null;
					if (flag3)
					{
						this.startPoint = parent.gameObject.transform.position;
						this.endPoint = Player.local.handLeft.transform.position;
						this.cantAccio = false;
					}
					else
					{
						this.cantAccio = true;
					}
				}
			}
		}

		// Token: 0x0400000A RID: 10
		private Item item;

		// Token: 0x0400000B RID: 11
		private Item npcItem;

		// Token: 0x0400000C RID: 12
		internal Vector3 startPoint;

		// Token: 0x0400000D RID: 13
		internal Vector3 endPoint;

		// Token: 0x0400000E RID: 14
		internal GameObject parentLocal;

		// Token: 0x0400000F RID: 15
		internal bool cantAccio;
	}
}
