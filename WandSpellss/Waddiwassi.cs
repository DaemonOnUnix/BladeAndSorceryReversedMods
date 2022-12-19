using System;
using ThunderRoad;
using UnityEngine;

namespace WandSpellss
{
	// Token: 0x02000020 RID: 32
	public class Waddiwassi : MonoBehaviour
	{
		// Token: 0x0600007E RID: 126 RVA: 0x00007426 File Offset: 0x00005626
		public void Start()
		{
			this.item = base.GetComponent<Item>();
			this.castCounter = 0;
		}

		// Token: 0x0600007F RID: 127 RVA: 0x0000743C File Offset: 0x0000563C
		internal void CastRay()
		{
			this.castCounter++;
			RaycastHit hit;
			bool flag = Physics.Raycast(this.item.flyDirRef.position, this.item.flyDirRef.forward, ref hit);
			if (flag)
			{
				Debug.Log("Did hit.");
				Debug.Log(hit.collider.gameObject.transform.parent.name);
				Transform parent = hit.collider.gameObject.transform.parent;
				this.parentLocal = parent.gameObject;
				bool flag2 = this.parentLocal.GetComponent<Item>() != null;
				if (flag2)
				{
					this.shootItem = this.parentLocal.GetComponent<Item>();
				}
				else
				{
					bool flag3 = this.parentLocal.GetComponentInParent<Creature>() != null;
					if (flag3)
					{
						this.target = this.parentLocal.GetComponentInParent<Creature>();
					}
				}
				bool flag4 = this.shootItem != null && this.target != null;
				if (flag4)
				{
					this.shootItem.gameObject.AddComponent<WaddiwassiPerItem>();
					this.shootItem.gameObject.GetComponent<WaddiwassiPerItem>().target = this.target;
					this.castCounter = 0;
				}
			}
		}

		// Token: 0x040000ED RID: 237
		private Item item;

		// Token: 0x040000EE RID: 238
		private Creature target;

		// Token: 0x040000EF RID: 239
		private Item shootItem;

		// Token: 0x040000F0 RID: 240
		internal Vector3 startPoint;

		// Token: 0x040000F1 RID: 241
		internal Vector3 endPoint;

		// Token: 0x040000F2 RID: 242
		internal GameObject parentLocal;

		// Token: 0x040000F3 RID: 243
		private int castCounter;
	}
}
