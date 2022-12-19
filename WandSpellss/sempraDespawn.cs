using System;
using ThunderRoad;
using UnityEngine;

namespace WandSpellss
{
	// Token: 0x02000014 RID: 20
	internal class sempraDespawn : MonoBehaviour
	{
		// Token: 0x0600003E RID: 62 RVA: 0x00003D6D File Offset: 0x00001F6D
		public void Start()
		{
			this.item = base.GetComponent<Item>();
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00003D7C File Offset: 0x00001F7C
		public void OnCollisionEnter(Collision c)
		{
			this.item.Despawn();
		}

		// Token: 0x04000058 RID: 88
		private Item item;
	}
}
