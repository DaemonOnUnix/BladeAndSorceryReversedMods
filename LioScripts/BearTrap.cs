using System;
using ThunderRoad;
using UnityEngine;

namespace LioScripts
{
	// Token: 0x02000003 RID: 3
	internal class BearTrap : MonoBehaviour
	{
		// Token: 0x06000005 RID: 5 RVA: 0x00002198 File Offset: 0x00000398
		private void Awake()
		{
			this.item = base.GetComponent<Item>();
			this.anim = base.GetComponentInChildren<Animator>();
			this.item.mainCollisionHandler.OnCollisionStartEvent += new CollisionHandler.CollisionEvent(this.BearTrapCollide);
			this.item.disallowRoomDespawn = true;
			this.isClosed = false;
			this.item.OnSpawnEvent += new Item.SpawnEvent(this.Item_OnSpawnEvent);
			this.item.isPooled = false;
			this.anim.keepAnimatorControllerStateOnDisable = true;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x0000221F File Offset: 0x0000041F
		private void OnDisable()
		{
			this.anim.Play("Idle", 0, 0f);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002239 File Offset: 0x00000439
		private void Item_OnSpawnEvent(EventTime eventTime)
		{
			this.isClosed = false;
			this.item.disallowRoomDespawn = true;
			this.anim.SetBool("Close", false);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002264 File Offset: 0x00000464
		private void BearTrapCollide(CollisionInstance collisionInstance)
		{
			Collider targetCollider = collisionInstance.targetCollider;
			bool flag = targetCollider.transform.root.GetComponent<Creature>() && !this.isClosed;
			if (flag)
			{
				bool flag2 = !targetCollider.transform.root.GetComponent<Creature>().isPlayer && !this.item.IsHanded(1) && !this.item.IsHanded(0) && !this.item.isTelekinesisGrabbed;
				if (flag2)
				{
					Creature component = targetCollider.transform.root.GetComponent<Creature>();
					bool flag3 = component.state > 0;
					if (flag3)
					{
						RagdollPart partByName = component.ragdoll.GetPartByName(targetCollider.GetComponentInParent<RagdollPart>().type.ToString());
						partByName.TrySlice();
						partByName.gameObject.transform.position = base.gameObject.transform.position;
						partByName.transform.SetParent(base.gameObject.transform);
						component.Kill(collisionInstance);
						this.anim.SetBool("Close", true);
						this.item.disallowRoomDespawn = false;
						this.isClosed = true;
						base.GetComponent<AudioSource>().Play();
					}
				}
			}
		}

		// Token: 0x04000008 RID: 8
		private Item item;

		// Token: 0x04000009 RID: 9
		private Animator anim;

		// Token: 0x0400000A RID: 10
		private bool isClosed;
	}
}
