using System;
using System.Linq;
using ThunderRoad;
using UnityEngine;

namespace WandSpellss
{
	// Token: 0x02000019 RID: 25
	public class Expelliarmus : MonoBehaviour
	{
		// Token: 0x0600004F RID: 79 RVA: 0x00004502 File Offset: 0x00002702
		public void Start()
		{
			this.item = base.GetComponent<Item>();
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00004514 File Offset: 0x00002714
		public void OnCollisionEnter(Collision c)
		{
			bool flag = c.gameObject.GetComponentInParent<Creature>() != null;
			if (flag)
			{
				c.gameObject.GetComponentInParent<Creature>().handRight.UnGrab(false);
				c.gameObject.GetComponentInParent<Creature>().handLeft.UnGrab(false);
				foreach (Rigidbody rigidbody in c.gameObject.GetComponentInParent<Creature>().ragdoll.parts.Select((RagdollPart part) => part.rb))
				{
					Debug.Log("Rigidbody name: " + rigidbody.name);
					c.gameObject.GetComponentInParent<Creature>().ragdoll.SetState(1);
					rigidbody.AddForce(this.item.flyDirRef.transform.forward * this.power, 1);
				}
			}
			else
			{
				bool flag2 = c.gameObject.GetComponentInParent<Item>() != null;
				if (flag2)
				{
					Item tempItem = c.gameObject.GetComponentInParent<Item>();
					tempItem.mainHandler.otherHand.otherHand.UnGrab(false);
					tempItem.mainHandler.otherHand.creature.ragdoll.SetState(1);
				}
			}
		}

		// Token: 0x0400006B RID: 107
		private Item item;

		// Token: 0x0400006C RID: 108
		private Item npcItem;

		// Token: 0x0400006D RID: 109
		internal AudioSource source;

		// Token: 0x0400006E RID: 110
		internal float power;
	}
}
