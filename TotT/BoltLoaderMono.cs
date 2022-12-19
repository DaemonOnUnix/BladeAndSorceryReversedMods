using System;
using ThunderRoad;
using UnityEngine;

namespace TotT
{
	// Token: 0x0200000F RID: 15
	public class BoltLoaderMono : MonoBehaviour
	{
		// Token: 0x06000088 RID: 136 RVA: 0x000056AF File Offset: 0x000038AF
		public void Start()
		{
			this.item = base.gameObject.GetComponentInParent<Item>();
			this.parentMono = this.item.gameObject.GetComponent<PhantomBladeMono>();
		}

		// Token: 0x06000089 RID: 137 RVA: 0x000056DC File Offset: 0x000038DC
		public void OnTriggerEnter(Collider collider)
		{
			bool flag = collider != null && this.parentMono != null;
			if (flag)
			{
				bool flag2 = !this.parentMono.isLoaded();
				if (flag2)
				{
					Item check = collider.gameObject.GetComponentInParent<Item>();
					bool flag3 = check != null;
					if (flag3)
					{
						PhantomBladeBolt check2 = check.gameObject.GetComponent<PhantomBladeBolt>();
						bool flag4 = check2 != null;
						if (flag4)
						{
							string id = check2.GetItemID();
							Color color = check.renderers[0].material.GetColor("_EmissionColor");
							this.parentMono.LoadBolt(id, color);
							bool flag5 = check.mainHandler != null;
							if (flag5)
							{
								RagdollHand r = check.mainHandler;
								r.TryRelease();
								r.ClearTouch();
								r.otherHand.ClearTouch();
							}
							check.mainHandleRight.gameObject.SetActive(false);
							check.gameObject.SetActive(false);
							check.Despawn();
						}
					}
				}
			}
		}

		// Token: 0x04000058 RID: 88
		private Item item;

		// Token: 0x04000059 RID: 89
		private PhantomBladeMono parentMono;
	}
}
