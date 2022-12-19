using System;
using UnityEngine;

namespace UltimateFantasyPack
{
	// Token: 0x02000024 RID: 36
	public class Timer : MonoBehaviour
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000085 RID: 133 RVA: 0x00004A85 File Offset: 0x00002C85
		public float SecondsRemaining
		{
			get
			{
				return this._timeRemaining;
			}
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00004A8D File Offset: 0x00002C8D
		private void Awake()
		{
			this.ResetTimer();
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00004A98 File Offset: 0x00002C98
		private void Update()
		{
			bool flag = !this.AlwaysActive;
			if (!flag)
			{
				this.TickTimer();
			}
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00004ABC File Offset: 0x00002CBC
		public void TickTimer()
		{
			this._timeRemaining -= Time.deltaTime;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00004AD0 File Offset: 0x00002CD0
		public void HandleTimerEnd()
		{
			bool flag = (double)this._timeRemaining > 0.0;
			if (!flag)
			{
				Action onTimerEnd = this.OnTimerEnd;
				bool flag2 = onTimerEnd != null;
				if (flag2)
				{
					onTimerEnd();
				}
				bool destroyOnFinish = this.DestroyOnFinish;
				if (destroyOnFinish)
				{
					Object.Destroy(this);
				}
				this.ResetTimer();
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00004B24 File Offset: 0x00002D24
		public void ResetTimer()
		{
			this._timeRemaining = this.TimerDuration;
		}

		// Token: 0x04000063 RID: 99
		public float TimerDuration;

		// Token: 0x04000064 RID: 100
		public Action OnTimerEnd;

		// Token: 0x04000065 RID: 101
		public bool DestroyOnFinish = true;

		// Token: 0x04000066 RID: 102
		public bool AlwaysActive = true;

		// Token: 0x04000067 RID: 103
		private float _timeRemaining;
	}
}
