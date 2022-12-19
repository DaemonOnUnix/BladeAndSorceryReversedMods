using System;
using System.Linq;
using ThunderRoad;
using UnityEngine;

namespace WandSpellss
{
	// Token: 0x02000002 RID: 2
	internal class Ascendio : MonoBehaviour
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public void Start()
		{
			this.item = base.GetComponent<Item>();
			this.player = Player.local.creature;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002070 File Offset: 0x00000270
		public void Ascend()
		{
			foreach (Rigidbody rigidbody in this.player.ragdoll.parts.Select((RagdollPart part) => part.rb))
			{
				bool flag = rigidbody != null;
				if (flag)
				{
					rigidbody.AddForce(this.item.flyDirRef.transform.forward * 2000f, 1);
				}
			}
			Player.fallDamage = false;
		}

		// Token: 0x04000001 RID: 1
		private Item item;

		// Token: 0x04000002 RID: 2
		private Creature player;
	}
}
