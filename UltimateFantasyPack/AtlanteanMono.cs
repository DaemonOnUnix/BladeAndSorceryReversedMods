using System;
using System.Collections;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

namespace UltimateFantasyPack
{
	// Token: 0x02000003 RID: 3
	public class AtlanteanMono : MonoBehaviour
	{
		// Token: 0x06000003 RID: 3 RVA: 0x00002080 File Offset: 0x00000280
		public void Start()
		{
			this.item = base.GetComponent<Item>();
			this.item.OnHeldActionEvent += new Item.HeldActionDelegate(this.ItemOnOnHeldActionEvent);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020A8 File Offset: 0x000002A8
		private void ItemOnOnHeldActionEvent(RagdollHand ragdollhand, Handle handle, Interactable.Action action)
		{
			bool flag = action == 2 && !this.onCooldown;
			if (flag)
			{
				foreach (Collider collider in Physics.OverlapSphere(this.item.transform.position, 25f))
				{
					bool flag2 = collider.GetComponentInParent<Creature>();
					if (flag2)
					{
						this.creature = collider.GetComponentInParent<Creature>();
						bool flag3 = !this.creature.isPlayer && !this.creature.isKilled;
						if (flag3)
						{
							Catalog.GetData<ItemData>("WaterBubble", true).SpawnAsync(delegate(Item bubble)
							{
								bubble.transform.position = this.creature.ragdoll.headPart.transform.position;
								bubble.transform.SetParent(this.creature.ragdoll.headPart.transform);
								this.bubbleItem = bubble;
								base.StartCoroutine(this.Cooldown());
								base.StartCoroutine(this.FloatEnemy(this.creature, bubble));
								this.bubbleSpawned = true;
							}, null, null, null, true, null);
						}
					}
				}
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x0000217E File Offset: 0x0000037E
		public IEnumerator Cooldown()
		{
			this.onCooldown = true;
			yield return new WaitForSeconds(this.settings.cooldown);
			this.onCooldown = false;
			yield break;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x0000218D File Offset: 0x0000038D
		public IEnumerator FloatEnemy(Creature creatureFloat, Item itemBubble)
		{
			creatureFloat.ragdoll.SetPhysicModifier(this, 0f, 1f, -1f, -1f, null);
			creatureFloat.locomotion.SetPhysicModifier(this, 0f, -1f, -1f);
			creatureFloat.brain.AddNoStandUpModifier(this);
			creatureFloat.ragdoll.SetState(1);
			yield return new WaitForSeconds(2f);
			foreach (RagdollPart part in creatureFloat.ragdoll.parts)
			{
				part.rb.isKinematic = true;
				part = null;
			}
			List<RagdollPart>.Enumerator enumerator = default(List<RagdollPart>.Enumerator);
			yield return new WaitForSeconds(4f);
			foreach (RagdollPart part2 in creatureFloat.ragdoll.parts)
			{
				part2.rb.isKinematic = false;
				part2 = null;
			}
			List<RagdollPart>.Enumerator enumerator2 = default(List<RagdollPart>.Enumerator);
			creatureFloat.ragdoll.RemovePhysicModifier(this);
			creatureFloat.locomotion.RemovePhysicModifier(this);
			creatureFloat.brain.RemoveNoStandUpModifier(this);
			itemBubble.Despawn();
			this.bubbleSpawned = false;
			yield break;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000021AC File Offset: 0x000003AC
		public void Update()
		{
			bool flag = this.bubbleSpawned;
			if (flag)
			{
				this.bubbleItem.transform.position = this.creature.ragdoll.headPart.transform.position;
			}
		}

		// Token: 0x04000002 RID: 2
		private Item item;

		// Token: 0x04000003 RID: 3
		private Item bubbleItem;

		// Token: 0x04000004 RID: 4
		private bool onCooldown;

		// Token: 0x04000005 RID: 5
		public Atlantean settings;

		// Token: 0x04000006 RID: 6
		private bool bubbleSpawned;

		// Token: 0x04000007 RID: 7
		private Creature creature;
	}
}
