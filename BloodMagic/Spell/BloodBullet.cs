using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThunderRoad;
using UnityEngine;

namespace BloodMagic.Spell
{
	// Token: 0x02000016 RID: 22
	internal class BloodBullet : MonoBehaviour
	{
		// Token: 0x0600007A RID: 122 RVA: 0x000034F4 File Offset: 0x000016F4
		public void Initialize(Item p_Item)
		{
			this.m_Item = p_Item;
			this.m_Item.collisionHandlers[0].OnCollisionStartEvent += new CollisionHandler.CollisionEvent(this.BloodBullet_OnCollisionStartEvent);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00003524 File Offset: 0x00001724
		private async void BloodBullet_OnCollisionStartEvent(CollisionInstance collisionInstance)
		{
			bool flag = collisionInstance.damageStruct.hitRagdollPart || collisionInstance.damageStruct.hitItem;
			if (flag)
			{
				Creature creature = null;
				bool flag2 = collisionInstance.damageStruct.hitItem;
				if (flag2)
				{
					Item item = collisionInstance.damageStruct.hitItem;
					RagdollHand mainHandler = item.mainHandler;
					bool flag3 = ((mainHandler != null) ? mainHandler.ragdoll.creature : null) != null;
					if (flag3)
					{
						creature = item.mainHandler.ragdoll.creature;
					}
					item = null;
					item = null;
				}
				else
				{
					creature = collisionInstance.damageStruct.hitRagdollPart.ragdoll.creature;
				}
				bool flag4 = creature != null && creature != Player.currentCreature && !creature.isKilled;
				if (flag4)
				{
					this.hitCreatures.Add(creature);
					BloodSpell.AimStruct aimStruct = BloodSpell.AimAssist(base.transform.position, collisionInstance.contactNormal, -1f, 0f);
					Creature bounceTo = aimStruct.toHit;
					bool flag5 = bounceTo != null && !this.hitCreatures.Contains(bounceTo);
					if (flag5)
					{
						this.hitCreatures.Add(bounceTo);
						this.m_Item.rb.AddForce(aimStruct.aimDir * 10f, 1);
						this.m_Item.transform.rotation = Quaternion.LookRotation(aimStruct.aimDir);
						this.m_Item.Throw(1f, 2);
						return;
					}
					aimStruct = default(BloodSpell.AimStruct);
					bounceTo = null;
					aimStruct = default(BloodSpell.AimStruct);
					bounceTo = null;
				}
				creature = null;
				creature = null;
			}
			this.m_Item.collisionHandlers[0].OnCollisionStartEvent -= new CollisionHandler.CollisionEvent(this.BloodBullet_OnCollisionStartEvent);
			await Task.Delay(2000);
			this.m_Item.Despawn();
		}

		// Token: 0x04000032 RID: 50
		private Item m_Item;

		// Token: 0x04000033 RID: 51
		private List<Creature> hitCreatures = new List<Creature>();
	}
}
