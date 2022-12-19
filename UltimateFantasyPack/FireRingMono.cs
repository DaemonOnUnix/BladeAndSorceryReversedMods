using System;
using ThunderRoad;
using UnityEngine;

namespace UltimateFantasyPack
{
	// Token: 0x02000008 RID: 8
	internal class FireRingMono : MonoBehaviour
	{
		// Token: 0x06000018 RID: 24 RVA: 0x00002792 File Offset: 0x00000992
		public void Awake()
		{
			this.item = base.GetComponent<Item>();
			this.item.OnHeldActionEvent += new Item.HeldActionDelegate(this.Item_OnHeldActionEvent);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000027BC File Offset: 0x000009BC
		private void Item_OnHeldActionEvent(RagdollHand ragdollHand, Handle handle, Interactable.Action action)
		{
			bool flag = action == 2;
			if (flag)
			{
				Catalog.LoadAssetAsync<GameObject>("Rave.FireRing", delegate(GameObject obj)
				{
					obj.transform.position = Player.currentCreature.transform.position;
					foreach (ParticleSystem particleSystem in obj.GetComponentsInChildren<ParticleSystem>())
					{
						particleSystem.gameObject.AddComponent<ParticleCollisionHandler>();
					}
					Object.Destroy(obj, 10f);
				}, "FireRing");
			}
		}

		// Token: 0x04000016 RID: 22
		public Item item;
	}
}
