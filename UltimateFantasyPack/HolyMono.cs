using System;
using System.Collections;
using ThunderRoad;
using UnityEngine;

namespace UltimateFantasyPack
{
	// Token: 0x02000010 RID: 16
	internal class HolyMono : MonoBehaviour
	{
		// Token: 0x06000038 RID: 56 RVA: 0x00002DEF File Offset: 0x00000FEF
		public void Awake()
		{
			this.item = base.GetComponent<Item>();
			this.item.OnHeldActionEvent += new Item.HeldActionDelegate(this.Item_OnHeldActionEvent);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002E18 File Offset: 0x00001018
		private void Item_OnHeldActionEvent(RagdollHand ragdollHand, Handle handle, Interactable.Action action)
		{
			bool flag = action == 2;
			if (flag)
			{
				foreach (Collider collider in Physics.OverlapSphere(Player.local.locomotion.transform.position, 50f))
				{
					Creature creature;
					collider.TryGetComponent<Creature>(ref creature);
					bool flag2 = creature != null && !creature.isKilled && !creature.isPlayer;
					if (flag2)
					{
						creature.locomotion.allowMove = false;
						GameManager.local.StartCoroutine(this.SpawnWeapon(creature, 1f));
					}
				}
			}
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002EBD File Offset: 0x000010BD
		public IEnumerator SpawnWeapon(Creature creature, float delay)
		{
			yield return new WaitForSeconds(delay);
			Catalog.GetData<ItemData>("HolyAbilitySword", true).SpawnAsync(delegate(Item item)
			{
				item.transform.position = creature.ragdoll.headPart.transform.position + Vector3.up * 10f;
				item.Throw(1f, 1);
				item.transform.LookAt(creature.ragdoll.headPart.transform.position);
				item.rb.AddForce(-(item.transform.position - creature.ragdoll.headPart.transform.position) * 5f, 1);
				item.Despawn(5f);
			}, null, null, null, true, null);
			yield return new WaitForSeconds(1f + delay);
			bool flag = !creature.isKilled;
			if (flag)
			{
				creature.locomotion.allowMove = true;
			}
			yield break;
		}

		// Token: 0x04000021 RID: 33
		public Item item;
	}
}
