using System;
using ThunderRoad;
using UnityEngine;

namespace ExplodingFireballs
{
	// Token: 0x02000005 RID: 5
	public class BurnComponent : MonoBehaviour
	{
		// Token: 0x0600000A RID: 10 RVA: 0x0000246D File Offset: 0x0000066D
		public void Start()
		{
			this.item = base.GetComponent<Item>();
			this.item.mainCollisionHandler.OnCollisionStartEvent += new CollisionHandler.CollisionEvent(this.MainCollisionHandler_OnCollisionStartEvent);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002499 File Offset: 0x00000699
		public void Setup(float dps, float duration, string effect)
		{
			this.burnDamagePerSecond = dps;
			this.burnDuration = duration;
			this.burnEffectId = effect;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000024B4 File Offset: 0x000006B4
		private void MainCollisionHandler_OnCollisionStartEvent(CollisionInstance collisionInstance)
		{
			Creature componentInParent = collisionInstance.targetCollider.GetComponentInParent<Creature>();
			bool flag = componentInParent != null;
			if (flag)
			{
				bool flag2 = componentInParent.GetComponent<Burning>() != null;
				if (flag2)
				{
					Object.Destroy(componentInParent.GetComponent<Burning>());
				}
				componentInParent.gameObject.AddComponent<Burning>().Setup(this.burnDamagePerSecond, this.burnDuration, this.burnEffectId);
			}
		}

		// Token: 0x0400000D RID: 13
		private Item item;

		// Token: 0x0400000E RID: 14
		public float burnDamagePerSecond;

		// Token: 0x0400000F RID: 15
		public float burnDuration;

		// Token: 0x04000010 RID: 16
		public string burnEffectId;
	}
}
