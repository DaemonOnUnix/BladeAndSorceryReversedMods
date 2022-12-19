using System;
using System.Collections;
using ThunderRoad;
using UnityEngine;

namespace Yamato
{
	// Token: 0x02000006 RID: 6
	public class DaggerDespawn : MonoBehaviour
	{
		// Token: 0x0600000B RID: 11 RVA: 0x00002296 File Offset: 0x00000496
		public void Start()
		{
			this.item = base.GetComponent<Item>();
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000022A8 File Offset: 0x000004A8
		public void OnCollisionEnter(Collision c)
		{
			bool flag = c.collider.gameObject.GetComponentInParent<YamatoComponent>() != null || c.collider.gameObject.GetComponentInParent<SheathComponent>() != null;
			if (flag)
			{
				this.item.IgnoreObjectCollision(c.collider.gameObject.GetComponentInParent<Item>());
			}
			else
			{
				bool flag2 = !this.item.IsHanded(null);
				if (flag2)
				{
					base.StartCoroutine(this.BeginDespawn());
					this.item.rb.useGravity = true;
				}
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x0000233D File Offset: 0x0000053D
		public IEnumerator BeginDespawn()
		{
			yield return new WaitForSeconds(0.3f);
			bool flag = this.item.IsHanded(null);
			if (flag)
			{
				yield break;
			}
			foreach (Damager damager in this.item.GetComponentsInChildren<Damager>())
			{
				damager.UnPenetrateAll();
				damager = null;
			}
			Damager[] array = null;
			this.item.Despawn();
			yield break;
		}

		// Token: 0x04000006 RID: 6
		private Item item;
	}
}
