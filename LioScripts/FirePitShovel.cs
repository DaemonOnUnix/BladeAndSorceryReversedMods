using System;
using ThunderRoad;
using UnityEngine;

namespace LioScripts
{
	// Token: 0x0200000C RID: 12
	internal class FirePitShovel : MonoBehaviour
	{
		// Token: 0x06000025 RID: 37 RVA: 0x00002AE2 File Offset: 0x00000CE2
		private void Awake()
		{
			this.item = base.GetComponent<Item>();
			this.item.mainCollisionHandler.OnCollisionStartEvent += new CollisionHandler.CollisionEvent(this.MainCollisionHandler_OnCollisionStartEvent);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002B10 File Offset: 0x00000D10
		private void MainCollisionHandler_OnCollisionStartEvent(CollisionInstance collisionInstance)
		{
			Collider targetCollider = collisionInstance.targetCollider;
			bool flag = collisionInstance.impactVelocity.magnitude >= 7f && collisionInstance.sourceColliderGroup.name == "ColliderBlade";
			if (flag)
			{
				bool flag2 = collisionInstance.targetMaterial.id == "Dirt" || collisionInstance.targetMaterial.id == "Sand";
				if (flag2)
				{
					Catalog.GetData<ItemData>("FirePit", true).SpawnAsync(delegate(Item it)
					{
						Catalog.GetData<EffectData>("FirePitShovelDig", true).Spawn(it.transform, true, null, false, Array.Empty<Type>());
						it.transform.position = collisionInstance.contactPoint;
						it.transform.rotation = Quaternion.FromToRotation(Vector3.up, collisionInstance.contactNormal);
					}, null, null, null, true, null);
					base.GetComponent<AudioSource>().Play();
				}
			}
		}

		// Token: 0x04000021 RID: 33
		private Item item;
	}
}
