using System;
using ThunderRoad;
using UnityEngine;

namespace SpellCastIce
{
	// Token: 0x0200000D RID: 13
	public class ShurikenItem : MonoBehaviour
	{
		// Token: 0x06000044 RID: 68 RVA: 0x00003D40 File Offset: 0x00001F40
		private void OnCollisionEnter(Collision collision)
		{
			this.DespawnItem();
			bool flag = collision.gameObject.GetComponentInParent<Creature>();
			if (flag)
			{
				Creature componentInParent = collision.gameObject.GetComponentInParent<Creature>();
				componentInParent.TryElectrocute(1f, 0.2f, true, false, Catalog.GetData<EffectData>("ImbueLightningRagdoll", true));
			}
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00003D98 File Offset: 0x00001F98
		private void DespawnItem()
		{
			GameObject gameObject = this.item.GetCustomReference("Hit", true).gameObject;
			gameObject.transform.parent = null;
			gameObject.GetComponent<ParticleSystem>().Play();
			this.item.Despawn(0.1f);
			Object.Destroy(gameObject, 1f);
		}

		// Token: 0x04000037 RID: 55
		public Item item;
	}
}
