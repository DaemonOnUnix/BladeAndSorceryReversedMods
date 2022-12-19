using System;
using ThunderRoad;
using UnityEngine;

namespace SpellCastIce
{
	// Token: 0x02000006 RID: 6
	public class IceSpikeItem : MonoBehaviour
	{
		// Token: 0x0600001E RID: 30 RVA: 0x00002C04 File Offset: 0x00000E04
		public void Initialize()
		{
			foreach (CollisionHandler collisionHandler in this.item.collisionHandlers)
			{
				bool flag = IceManager.IsAbilityUnlocked(IceManager.AbilitiesEnum.noGravity);
				if (flag)
				{
					collisionHandler.SetPhysicModifier(this, new float?(0f), 1f, 0f, 0f, -1f, null);
				}
			}
			bool flag2 = !IceManager.IsAbilityUnlocked(IceManager.AbilitiesEnum.pickUpIceSpikes);
			if (flag2)
			{
				Handle handle = this.item.handles[0];
				handle.enabled = false;
				handle.gameObject.SetActive(false);
				this.item.handles.Clear();
			}
			this.spawnTime = Time.time;
			this.item.OnUngrabEvent += new Item.ReleaseDelegate(this.Item_OnUngrabEvent1);
			this.item.OnTelekinesisGrabEvent += new Item.TelekinesisDelegate(this.Item_OnTelekinesisGrabEvent);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002D14 File Offset: 0x00000F14
		private void Item_OnTelekinesisGrabEvent(Handle handle, SpellTelekinesis teleGrabber)
		{
			this.spawnTime = Time.time;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002D22 File Offset: 0x00000F22
		private void Item_OnUngrabEvent1(Handle handle, RagdollHand ragdollHand, bool throwing)
		{
			this.spawnTime = Time.time;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002D30 File Offset: 0x00000F30
		private void Update()
		{
			bool flag = Time.time - this.spawnTime > 5f && !this.despawning;
			if (flag)
			{
				bool flag2 = !this.item.IsHanded(null) && !this.item.isTelekinesisGrabbed;
				if (flag2)
				{
					this.item.GetComponentInChildren<Animation>().Play();
					this.despawning = true;
					this.item.Despawn(1f);
				}
			}
		}

		// Token: 0x04000016 RID: 22
		public Item item;

		// Token: 0x04000017 RID: 23
		public IceSpikeModule module;

		// Token: 0x04000018 RID: 24
		private float spawnTime;

		// Token: 0x04000019 RID: 25
		private bool despawning;
	}
}
