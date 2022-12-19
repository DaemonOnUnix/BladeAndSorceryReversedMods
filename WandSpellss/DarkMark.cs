using System;
using System.Timers;
using ThunderRoad;
using UnityEngine;

namespace WandSpellss
{
	// Token: 0x02000006 RID: 6
	internal class DarkMark : MonoBehaviour
	{
		// Token: 0x0600000D RID: 13 RVA: 0x00002318 File Offset: 0x00000518
		public void Start()
		{
			this.item = base.GetComponent<Item>();
			this.dissolveVal = 1f;
			foreach (Renderer renderer in this.item.gameObject.GetComponentsInChildren<Renderer>())
			{
				foreach (Material mat in renderer.materials)
				{
					mat.SetFloat("_dissolve", this.dissolveVal);
				}
			}
			this.SetTimer();
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000023A0 File Offset: 0x000005A0
		private void Update()
		{
			bool flag = this.dissolveVal > 0f;
			if (flag)
			{
				this.dissolveVal -= 0.01f;
				foreach (Renderer renderer in this.item.gameObject.GetComponentsInChildren<Renderer>())
				{
					foreach (Material mat in renderer.materials)
					{
						mat.SetFloat("_dissolve", this.dissolveVal);
					}
				}
			}
			this.item.gameObject.transform.LookAt(Player.local.transform);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002454 File Offset: 0x00000654
		private void SetTimer()
		{
			this.aTimer = new Timer(45000.0);
			this.aTimer.Elapsed += this.OnTimedEvent;
			this.aTimer.AutoReset = false;
			this.aTimer.Enabled = true;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000024A8 File Offset: 0x000006A8
		private void OnTimedEvent(object sender, ElapsedEventArgs e)
		{
			this.item.Despawn();
		}

		// Token: 0x04000010 RID: 16
		private Item item;

		// Token: 0x04000011 RID: 17
		private Timer aTimer;

		// Token: 0x04000012 RID: 18
		private float dissolveVal;
	}
}
