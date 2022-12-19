using System;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

namespace LioScripts
{
	// Token: 0x02000005 RID: 5
	public class Caltrop : MonoBehaviour
	{
		// Token: 0x0600000C RID: 12 RVA: 0x000023E0 File Offset: 0x000005E0
		private void Awake()
		{
			this.item = base.GetComponent<Item>();
			this.item.mainCollisionHandler.OnCollisionStartEvent += new CollisionHandler.CollisionEvent(this.CaltropCollide);
			this.item.disallowRoomDespawn = true;
			bool flag = Caltrop.EnemiesHitIDs == null;
			if (flag)
			{
				Caltrop.EnemiesHitIDs = new List<int>();
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x0000243C File Offset: 0x0000063C
		private void CaltropCollide(CollisionInstance collisionInstance)
		{
			bool flag = collisionInstance.targetCollider.transform.root.GetComponent<Creature>();
			if (flag)
			{
				bool flag2 = !collisionInstance.targetCollider.transform.root.GetComponent<Creature>().isPlayer;
				if (flag2)
				{
					Creature component = collisionInstance.targetCollider.transform.root.GetComponent<Creature>();
					bool flag3 = !component.isKilled;
					if (flag3)
					{
						bool flag4 = !Caltrop.EnemiesHitIDs.Contains(component.GetInstanceID());
						if (flag4)
						{
							Caltrop.EnemiesHitIDs.Add(component.GetInstanceID());
							component.locomotion.angularSpeed /= this.speedSlowModifier;
							component.locomotion.turnSpeed /= this.speedSlowModifier;
							component.locomotion.backwardSpeed /= this.speedSlowModifier;
							component.locomotion.forwardSpeed /= this.speedSlowModifier;
							component.locomotion.runSpeedAdd /= this.speedSlowModifier;
							component.locomotion.horizontalSpeed /= this.speedSlowModifier;
							component.locomotion.strafeSpeed /= this.speedSlowModifier;
							bool flag5 = Random.value > 0.5f;
							if (flag5)
							{
								component.ragdoll.SetState(1);
							}
						}
						component.Damage(new CollisionInstance(new DamageStruct(1, this.damage), null, null));
						bool flag6 = component.currentHealth <= 0f;
						if (flag6)
						{
							component.Kill();
						}
					}
				}
			}
		}

		// Token: 0x0400000B RID: 11
		public static List<int> EnemiesHitIDs;

		// Token: 0x0400000C RID: 12
		private Item item;

		// Token: 0x0400000D RID: 13
		private float speedSlowModifier = 1.5f;

		// Token: 0x0400000E RID: 14
		private float damage = 2f;
	}
}
