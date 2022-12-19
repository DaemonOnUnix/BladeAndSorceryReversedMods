using System;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

namespace UltimateFantasyPack
{
	// Token: 0x02000005 RID: 5
	public class ArcWraithMono : MonoBehaviour
	{
		// Token: 0x0600000C RID: 12 RVA: 0x000022A0 File Offset: 0x000004A0
		public void Start()
		{
			this.item = base.GetComponent<Item>();
			this.ArchWraithItem = Catalog.GetData<ItemData>("ArchWraithPower", true);
			this.effect = Catalog.GetData<EffectData>("ImbueLightningRagdoll", true);
			this.item.OnHeldActionEvent += new Item.HeldActionDelegate(this.Item_OnHeldActionEvent);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000022F4 File Offset: 0x000004F4
		private void Item_OnHeldActionEvent(RagdollHand ragdollHand, Handle handle, Interactable.Action action)
		{
			bool flag = action == 2;
			if (flag)
			{
				Player.currentCreature.mana.currentMana -= 1f;
				Catalog.GetData<ItemData>("ArchWraithPower", true).SpawnAsync(delegate(Item Arch)
				{
					Arch.transform.position = this.item.flyDirRef.position;
					Arch.transform.rotation = Quaternion.identity;
					Arch.rb.useGravity = false;
					this.fixedJoint = Arch.gameObject.AddComponent<FixedJoint>();
					this.fixedJoint.connectedBody = this.item.rb;
					this.ArchInstance = Arch;
				}, null, null, null, true, null);
				foreach (Creature creature in Creature.allActive)
				{
					bool flag2 = Vector3.Distance(creature.transform.position, this.item.flyDirRef.position) < 5f && !creature.isPlayer;
					if (flag2)
					{
						creature.TryElectrocute(10f, 10f, true, true, this.effect);
						creature.ragdoll.SetState(1);
						this.affectedCreatures.Add(creature);
					}
				}
				this.isShocking = true;
			}
			else
			{
				bool flag3 = action == 3;
				if (flag3)
				{
					bool flag4 = this.ArchInstance != null;
					if (flag4)
					{
						this.ArchInstance.Despawn();
					}
					foreach (Creature creature2 in this.affectedCreatures)
					{
						creature2.StopShock();
						this.affectedCreatures.Remove(creature2);
					}
					this.isShocking = false;
				}
			}
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000024A8 File Offset: 0x000006A8
		private void Update()
		{
			bool flag = this.isShocking;
			if (flag)
			{
				foreach (Creature creature in this.affectedCreatures)
				{
					creature.TryElectrocute(10f, 10f, true, true, this.effect);
				}
			}
		}

		// Token: 0x04000008 RID: 8
		private Item item;

		// Token: 0x04000009 RID: 9
		private Item ArchInstance;

		// Token: 0x0400000A RID: 10
		private EffectData effect;

		// Token: 0x0400000B RID: 11
		private bool isShocking;

		// Token: 0x0400000C RID: 12
		private List<Creature> affectedCreatures = new List<Creature>();

		// Token: 0x0400000D RID: 13
		private ItemData ArchWraithItem;

		// Token: 0x0400000E RID: 14
		private FixedJoint fixedJoint;
	}
}
