using System;
using ThunderRoad;
using UnityEngine;

namespace WandSpellss
{
	// Token: 0x0200001A RID: 26
	internal class Stupefy : MonoBehaviour
	{
		// Token: 0x06000052 RID: 82 RVA: 0x00004695 File Offset: 0x00002895
		public void Start()
		{
			this.item = base.GetComponent<Item>();
			this.item.gameObject.AddComponent<StunDamager>();
		}

		// Token: 0x0400006F RID: 111
		private Item item;

		// Token: 0x04000070 RID: 112
		internal AudioSource source;
	}
}
