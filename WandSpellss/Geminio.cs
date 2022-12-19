using System;
using ThunderRoad;
using UnityEngine;

namespace WandSpellss
{
	// Token: 0x0200000B RID: 11
	public class Geminio : MonoBehaviour
	{
		// Token: 0x0600001E RID: 30 RVA: 0x000031D4 File Offset: 0x000013D4
		public void Start()
		{
			this.item = base.GetComponent<Item>();
			this.random = new Random();
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000031F0 File Offset: 0x000013F0
		internal void CastRay()
		{
			RaycastHit hit;
			bool flag = Physics.Raycast(this.item.flyDirRef.position, this.item.flyDirRef.forward, ref hit);
			if (flag)
			{
				Debug.Log(hit.collider.gameObject.transform.parent.name);
				Transform parent = hit.collider.gameObject.transform.parent;
				this.parentLocal = parent.gameObject;
				bool flag2 = this.parentLocal.GetComponent<Item>() != null;
				if (flag2)
				{
					this.duplicate = Object.Instantiate<GameObject>(this.parentLocal);
					double range = 0.4;
					double sample = this.random.NextDouble();
					double scaled = sample * range;
					this.duplicate.transform.position = new Vector3(parent.gameObject.transform.position.x + (float)scaled, parent.gameObject.transform.position.y + (float)scaled, parent.gameObject.transform.position.z);
				}
			}
		}

		// Token: 0x0400002F RID: 47
		private Item item;

		// Token: 0x04000030 RID: 48
		internal GameObject parentLocal;

		// Token: 0x04000031 RID: 49
		internal bool cantEvanesco;

		// Token: 0x04000032 RID: 50
		private GameObject duplicate;

		// Token: 0x04000033 RID: 51
		private Random random;
	}
}
