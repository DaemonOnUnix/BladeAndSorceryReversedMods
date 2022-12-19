using System;
using ThunderRoad;
using UnityEngine;

namespace WandSpellss
{
	// Token: 0x02000003 RID: 3
	public class AvadaKedavra : MonoBehaviour
	{
		// Token: 0x06000004 RID: 4 RVA: 0x0000212D File Offset: 0x0000032D
		public void Start()
		{
			this.item = base.GetComponent<Item>();
		}

		// Token: 0x06000005 RID: 5 RVA: 0x0000213C File Offset: 0x0000033C
		public void OnCollisionEnter(Collision c)
		{
			c.gameObject.GetComponentInParent<Creature>().Kill();
		}

		// Token: 0x04000003 RID: 3
		private Item item;

		// Token: 0x04000004 RID: 4
		internal ItemData avadaLightning;

		// Token: 0x04000005 RID: 5
		internal AudioSource source;

		// Token: 0x04000006 RID: 6
		private Item lightningItem;

		// Token: 0x04000007 RID: 7
		public Creature hitCreatures;
	}
}
