using System;
using ThunderRoad;
using UnityEngine;

namespace MeteorFall
{
	// Token: 0x02000004 RID: 4
	public class MeteorProjectileComponent : MonoBehaviour
	{
		// Token: 0x06000005 RID: 5 RVA: 0x000020B4 File Offset: 0x000002B4
		public void Start()
		{
			this.item = base.GetComponent<Item>();
			this.item.mainCollisionHandler.OnCollisionStartEvent += new CollisionHandler.CollisionEvent(this.MainCollisionHandler_OnCollisionStartEvent);
			this.item.disallowDespawn = true;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000020EC File Offset: 0x000002EC
		private void MainCollisionHandler_OnCollisionStartEvent(CollisionInstance collisionInstance)
		{
			collisionInstance.ignoreDamage = true;
			Catalog.GetData<ItemData>("MeteorFall", true).SpawnAsync(delegate(Item meteor)
			{
				meteor.gameObject.AddComponent<MeteorComponent>();
				meteor.rb.AddForce(Vector3.down * 500f, 2);
				meteor.Throw(1f, 1);
			}, new Vector3?(new Vector3(collisionInstance.contactPoint.x, collisionInstance.contactPoint.y + 2000f, collisionInstance.contactPoint.z)), null, null, true, null);
		}

		// Token: 0x04000001 RID: 1
		private Item item;
	}
}
