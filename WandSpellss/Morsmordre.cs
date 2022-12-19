using System;
using System.Timers;
using ThunderRoad;
using UnityEngine;

namespace WandSpellss
{
	// Token: 0x02000011 RID: 17
	internal class Morsmordre : MonoBehaviour
	{
		// Token: 0x0600002F RID: 47 RVA: 0x000037A1 File Offset: 0x000019A1
		public void Start()
		{
			this.item = base.GetComponent<Item>();
			this.SetTimer();
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000037B8 File Offset: 0x000019B8
		private void SetTimer()
		{
			this.aTimer = new Timer(2500.0);
			this.aTimer.Elapsed += this.OnTimedEvent;
			this.aTimer.AutoReset = false;
			this.aTimer.Enabled = true;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x0000380C File Offset: 0x00001A0C
		private void OnTimedEvent(object sender, ElapsedEventArgs e)
		{
			string text = "DarkMark ItemData: ";
			ItemData itemData = this.darkMark;
			Debug.Log(text + ((itemData != null) ? itemData.ToString() : null));
			this.darkMark.SpawnAsync(delegate(Item projectile)
			{
				projectile.gameObject.AddComponent<DarkMark>();
				projectile.transform.position = this.item.transform.position;
				projectile.rb.useGravity = false;
				projectile.rb.drag = 0f;
				this.item.Despawn();
			}, null, null, null, true, null);
		}

		// Token: 0x04000047 RID: 71
		private Item item;

		// Token: 0x04000048 RID: 72
		internal ItemData darkMark;

		// Token: 0x04000049 RID: 73
		private Timer aTimer;
	}
}
