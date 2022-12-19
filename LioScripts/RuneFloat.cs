using System;
using UnityEngine;

namespace LioScripts
{
	// Token: 0x02000010 RID: 16
	public class RuneFloat : MonoBehaviour
	{
		// Token: 0x06000030 RID: 48 RVA: 0x00002D12 File Offset: 0x00000F12
		private void Start()
		{
			this.posOffset = base.transform.position;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002D28 File Offset: 0x00000F28
		private void Update()
		{
			this.tempPos = this.posOffset;
			this.tempPos.y = this.tempPos.y + Mathf.Sin(Time.fixedTime * 3.1415927f * this.frequency) * this.amplitude;
			base.transform.position = this.tempPos;
		}

		// Token: 0x04000025 RID: 37
		private float amplitude = 0.2f;

		// Token: 0x04000026 RID: 38
		private float frequency = 0.2f;

		// Token: 0x04000027 RID: 39
		private Vector3 posOffset = default(Vector3);

		// Token: 0x04000028 RID: 40
		private Vector3 tempPos = default(Vector3);
	}
}
