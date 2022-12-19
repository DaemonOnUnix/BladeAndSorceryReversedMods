using System;
using System.Timers;
using ThunderRoad;
using UnityEngine;

namespace WandSpellss
{
	// Token: 0x02000017 RID: 23
	internal class PetrificusTotalus : MonoBehaviour
	{
		// Token: 0x06000047 RID: 71 RVA: 0x00003EF4 File Offset: 0x000020F4
		public void Start()
		{
			this.item = base.GetComponent<Item>();
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00003F04 File Offset: 0x00002104
		public void OnCollisionEnter(Collision c)
		{
			bool flag = c.gameObject.GetComponentInParent<Creature>() != null;
			if (flag)
			{
				this.SetTimer();
				c.gameObject.GetComponentInParent<Creature>().StopAnimation(false);
				c.gameObject.GetComponentInParent<Creature>().ToogleTPose();
				this.enemy = c.gameObject;
			}
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00003F60 File Offset: 0x00002160
		private void SetTimer()
		{
			this.aTimer = new Timer(7500.0);
			this.aTimer.Elapsed += this.OnTimedEvent;
			this.aTimer.AutoReset = false;
			this.aTimer.Enabled = true;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00003FB4 File Offset: 0x000021B4
		private void OnTimedEvent(object source, ElapsedEventArgs e)
		{
			this.enemy.GetComponentInParent<Creature>().StopAnimation(false);
			this.enemy.GetComponentInParent<Creature>().ToogleTPose();
		}

		// Token: 0x0400005F RID: 95
		private Item item;

		// Token: 0x04000060 RID: 96
		private Item npcItem;

		// Token: 0x04000061 RID: 97
		private GameObject enemy;

		// Token: 0x04000062 RID: 98
		internal AudioSource source;

		// Token: 0x04000063 RID: 99
		public Timer aTimer;
	}
}
