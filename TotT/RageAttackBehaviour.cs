using System;
using ThunderRoad;
using UnityEngine;

namespace TotT
{
	// Token: 0x02000018 RID: 24
	public class RageAttackBehaviour : MonoBehaviour
	{
		// Token: 0x060000B6 RID: 182 RVA: 0x000063C8 File Offset: 0x000045C8
		public void Start()
		{
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x000063CC File Offset: 0x000045CC
		public void Setup(float Time)
		{
			this.creature = base.GetComponent<Creature>();
			this.oldFactionID = this.creature.factionId;
			this.creature.SetFaction(0);
			this.creature.brain.canDamage = true;
			this.creature.ragdoll.AddPhysicToggleModifier(this);
			this.TimerMax = Time;
			this.Timer = this.TimerMax;
			base.SendMessage("CheckDominoLink");
			this.go = true;
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00006450 File Offset: 0x00004650
		public void Update()
		{
			bool flag = this.go;
			if (flag)
			{
				this.Timer -= Time.deltaTime;
				bool flag2 = this.Timer <= 0f || this.creature.state == 0;
				if (flag2)
				{
					this.creature.SetFaction(this.oldFactionID);
					this.creature.ragdoll.RemovePhysicToggleModifier(this);
					Object.Destroy(this);
				}
				bool flag3 = this.creature.handLeft.grabbedHandle != null;
				if (flag3)
				{
					bool flag4 = !Item.allThrowed.Contains(this.creature.handLeft.grabbedHandle.item);
					if (flag4)
					{
						Item.allThrowed.Add(this.creature.handLeft.grabbedHandle.item);
					}
				}
				bool flag5 = this.creature.handRight.grabbedHandle != null;
				if (flag5)
				{
					bool flag6 = !Item.allThrowed.Contains(this.creature.handRight.grabbedHandle.item);
					if (flag6)
					{
						Item.allThrowed.Add(this.creature.handRight.grabbedHandle.item);
					}
				}
			}
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x0000659C File Offset: 0x0000479C
		public void DominoLinkedTo(Creature creature)
		{
			bool flag = creature.gameObject.GetComponent<RageAttackBehaviour>() == null;
			if (flag)
			{
				RageAttackBehaviour temp = creature.gameObject.AddComponent<RageAttackBehaviour>();
				temp.Setup(this.TimerMax);
			}
		}

		// Token: 0x04000086 RID: 134
		private Creature creature;

		// Token: 0x04000087 RID: 135
		private float Timer;

		// Token: 0x04000088 RID: 136
		private float TimerMax;

		// Token: 0x04000089 RID: 137
		private int oldFactionID;

		// Token: 0x0400008A RID: 138
		private bool go = false;
	}
}
