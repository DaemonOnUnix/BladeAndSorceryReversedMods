using System;
using ThunderRoad;
using UnityEngine;

namespace WandSpellss
{
	// Token: 0x0200001B RID: 27
	public class StunDamager : MonoBehaviour
	{
		// Token: 0x06000054 RID: 84 RVA: 0x000046BE File Offset: 0x000028BE
		public void Start()
		{
			this.item = base.GetComponent<Item>();
		}

		// Token: 0x06000055 RID: 85 RVA: 0x000046D0 File Offset: 0x000028D0
		public void OnCollisionEnter(Collision c)
		{
			bool flag = c.gameObject.GetComponentInParent<Creature>() != null;
			if (flag)
			{
				c.gameObject.GetComponentInParent<Creature>().ragdoll.SetState(1);
				c.gameObject.GetComponentInParent<Creature>().TryElectrocute(1f, 3f, true, false, null);
			}
		}

		// Token: 0x04000071 RID: 113
		private Item item;
	}
}
