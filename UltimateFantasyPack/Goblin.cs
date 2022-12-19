using System;
using ThunderRoad;

namespace UltimateFantasyPack
{
	// Token: 0x0200000D RID: 13
	public class Goblin : ItemModule
	{
		// Token: 0x06000026 RID: 38 RVA: 0x000029AC File Offset: 0x00000BAC
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			item.OnSnapEvent += new Item.HolderDelegate(this.Snap);
			item.OnUnSnapEvent += new Item.HolderDelegate(this.Unsnap);
			item.OnHeldActionEvent += new Item.HeldActionDelegate(this.Item_OnHeldActionEvent);
			item.OnGrabEvent += new Item.GrabDelegate(this.Item_OnGrabEvent);
			item.OnUngrabEvent += new Item.ReleaseDelegate(this.Item_OnUngrabEvent);
			bool flag = item.mainHandler != null && item.mainHandler.creature.isPlayer;
			if (flag)
			{
				RagdollHandClimb.climbFree = true;
			}
			else
			{
				RagdollHandClimb.climbFree = false;
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002A56 File Offset: 0x00000C56
		private void Item_OnUngrabEvent(Handle handle, RagdollHand ragdollHand, bool throwing)
		{
			RagdollHandClimb.climbFree = false;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002A5F File Offset: 0x00000C5F
		private void Item_OnGrabEvent(Handle handle, RagdollHand ragdollHand)
		{
			RagdollHandClimb.climbFree = true;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002A68 File Offset: 0x00000C68
		private void Item_OnHeldActionEvent(RagdollHand ragdollHand, Handle handle, Interactable.Action action)
		{
			bool flag = action == 4;
			if (flag)
			{
				RagdollHandClimb.climbFree = true;
			}
			bool flag2 = action == 5;
			if (flag2)
			{
				RagdollHandClimb.climbFree = false;
			}
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002A94 File Offset: 0x00000C94
		private void Snap(Holder holder)
		{
			Creature creature = holder.creature;
			bool flag = ((creature != null) ? creature.player : null);
			if (flag)
			{
				RagdollHandClimb.climbFree = true;
			}
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002AC8 File Offset: 0x00000CC8
		private void Unsnap(Holder holder)
		{
			Creature creature = holder.creature;
			bool flag = ((creature != null) ? creature.player : null);
			if (flag)
			{
				RagdollHandClimb.climbFree = false;
			}
		}
	}
}
