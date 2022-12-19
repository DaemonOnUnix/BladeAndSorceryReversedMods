using System;
using UnityEngine;

namespace UltimateFantasyPack
{
	// Token: 0x02000025 RID: 37
	public class Cooldown : MonoBehaviour
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600008C RID: 140 RVA: 0x00004B49 File Offset: 0x00002D49
		public bool IsOnCooldown
		{
			get
			{
				return this.isOnCooldown;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600008D RID: 141 RVA: 0x00004B51 File Offset: 0x00002D51
		public float RemainingSeconds
		{
			get
			{
				return this.remainingSeconds;
			}
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00004B59 File Offset: 0x00002D59
		private void Awake()
		{
			this.remainingSeconds = this.cooldownLength;
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00004B68 File Offset: 0x00002D68
		private void Update()
		{
			bool flag = !this.isOnCooldown;
			if (!flag)
			{
				this.TickCooldown();
			}
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00004B8C File Offset: 0x00002D8C
		private void TickCooldown()
		{
			this.remainingSeconds -= Time.deltaTime;
			bool flag = (double)this.remainingSeconds > 0.0;
			if (!flag)
			{
				Action onCooldownEnd = this.onCooldownEnd;
				bool flag2 = onCooldownEnd != null;
				if (flag2)
				{
					onCooldownEnd();
				}
				this.isOnCooldown = false;
			}
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00004BE1 File Offset: 0x00002DE1
		public void StartCooldown()
		{
			this.isOnCooldown = true;
			this.remainingSeconds = this.cooldownLength;
		}

		// Token: 0x04000068 RID: 104
		private bool isOnCooldown = false;

		// Token: 0x04000069 RID: 105
		public float cooldownLength;

		// Token: 0x0400006A RID: 106
		private float remainingSeconds;

		// Token: 0x0400006B RID: 107
		public Action onCooldownEnd;
	}
}
