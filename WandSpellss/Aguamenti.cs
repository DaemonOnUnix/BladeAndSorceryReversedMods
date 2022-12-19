using System;
using ThunderRoad;
using UnityEngine;

namespace WandSpellss
{
	// Token: 0x02000004 RID: 4
	public class Aguamenti : MonoBehaviour
	{
		// Token: 0x06000007 RID: 7 RVA: 0x00002159 File Offset: 0x00000359
		public void Start()
		{
			this.item = base.GetComponent<Item>();
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002168 File Offset: 0x00000368
		public void OnParticleCollision(Collision c)
		{
		}

		// Token: 0x04000008 RID: 8
		private Item item;

		// Token: 0x04000009 RID: 9
		private Item npcItem;
	}
}
