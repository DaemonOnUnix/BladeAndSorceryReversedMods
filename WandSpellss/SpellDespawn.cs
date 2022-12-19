using System;
using ThunderRoad;
using UnityEngine;

namespace WandSpellss
{
	// Token: 0x0200001C RID: 28
	internal class SpellDespawn : MonoBehaviour
	{
		// Token: 0x06000057 RID: 87 RVA: 0x00004733 File Offset: 0x00002933
		public void Start()
		{
			this.item = base.GetComponent<Item>();
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00004744 File Offset: 0x00002944
		public void OnCollisionEnter(Collision c)
		{
			Debug.Log("Before avada check");
			Debug.Log(c.gameObject.name);
			bool flag = this.item.GetComponent<AvadaKedavra>() != null;
			if (flag)
			{
				Debug.Log("After avada check");
				Debug.Log(c.gameObject);
				bool flag2 = c.gameObject.name == "Quad 1(Clone)";
				if (flag2)
				{
					Debug.Log("Colliders Hit this with avadaKedavra: " + c.gameObject.name);
					this.item.IgnoreObjectCollision(c.gameObject.GetComponent<Item>());
				}
				else
				{
					this.item.Despawn();
				}
			}
			else
			{
				this.item.Despawn();
			}
		}

		// Token: 0x04000072 RID: 114
		private Item item;
	}
}
