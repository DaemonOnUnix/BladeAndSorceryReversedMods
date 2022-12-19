using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThunderRoad;
using UnityEngine;

namespace BloodMagic.Spell
{
	// Token: 0x02000015 RID: 21
	internal class BloodDagger : MonoBehaviour
	{
		// Token: 0x06000077 RID: 119 RVA: 0x00003471 File Offset: 0x00001671
		public void Initialize(Item p_Item)
		{
			this.m_Item = p_Item;
			this.m_Item.collisionHandlers[0].OnCollisionStartEvent += new CollisionHandler.CollisionEvent(this.BloodBullet_OnCollisionStartEvent);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x000034A0 File Offset: 0x000016A0
		private async void BloodBullet_OnCollisionStartEvent(CollisionInstance collisionInstance)
		{
			this.m_Item.collisionHandlers[0].OnCollisionStartEvent -= new CollisionHandler.CollisionEvent(this.BloodBullet_OnCollisionStartEvent);
			await Task.Delay(2000);
			this.m_Item.Despawn();
		}

		// Token: 0x04000030 RID: 48
		private Item m_Item;

		// Token: 0x04000031 RID: 49
		private List<Creature> hitCreatures = new List<Creature>();
	}
}
