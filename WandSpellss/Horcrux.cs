using System;
using ThunderRoad;
using UnityEngine;

namespace WandSpellss
{
	// Token: 0x0200000C RID: 12
	internal class Horcrux : MonoBehaviour
	{
		// Token: 0x06000021 RID: 33 RVA: 0x00003322 File Offset: 0x00001522
		public void Start()
		{
			this.item = base.GetComponent<Item>();
			this.percentageOfSoul = Player.local.creature.gameObject.GetComponent<Soul>().DivideSoul();
		}

		// Token: 0x04000034 RID: 52
		private Item item;

		// Token: 0x04000035 RID: 53
		private float percentageOfSoul;
	}
}
