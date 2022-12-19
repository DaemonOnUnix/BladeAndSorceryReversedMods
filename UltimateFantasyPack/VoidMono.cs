using System;
using System.Collections;
using ThunderRoad;
using UnityEngine;

namespace UltimateFantasyPack
{
	// Token: 0x0200002A RID: 42
	public class VoidMono : MonoBehaviour
	{
		// Token: 0x060000AA RID: 170 RVA: 0x0000527C File Offset: 0x0000347C
		public void Start()
		{
			Debug.Log("Meteor Item Spawned");
			VoidMono.item = base.GetComponent<Item>();
			VoidMono.item.OnHeldActionEvent += new Item.HeldActionDelegate(this.Item_OnHeldActionEvent);
			this.Cooldown = false;
		}

		// Token: 0x060000AB RID: 171 RVA: 0x000052B3 File Offset: 0x000034B3
		private IEnumerator voidSmash()
		{
			this.Cooldown = true;
			yield return new WaitForSeconds(15f);
			this.Cooldown = false;
			yield break;
		}

		// Token: 0x060000AC RID: 172 RVA: 0x000052C4 File Offset: 0x000034C4
		private void Item_OnHeldActionEvent(RagdollHand ragdollHand, Handle handle, Interactable.Action action)
		{
			bool flag = action == 2 && !this.Cooldown;
			if (flag)
			{
				Debug.Log("Alt Use Pressed");
				Catalog.GetData<ItemData>("VoidMeteor", true).SpawnAsync(delegate(Item Meteor)
				{
					Meteor.transform.position = VoidMono.item.transform.position;
					Meteor.transform.rotation = Player.currentCreature.transform.rotation;
					Meteor.rb.useGravity = false;
					Meteor.GetCustomReference("Meteor", true).GetComponent<ParticleSystem>().gameObject.AddComponent<MeteorCollision>();
					Meteor.Despawn(6f);
				}, null, null, null, true, null);
				Debug.Log("Meteor spawned in the sky");
				base.StartCoroutine(this.voidSmash());
			}
		}

		// Token: 0x04000079 RID: 121
		public static Item item;

		// Token: 0x0400007A RID: 122
		private bool Cooldown;
	}
}
