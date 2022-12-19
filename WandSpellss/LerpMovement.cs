using System;
using UnityEngine;

namespace WandSpellss
{
	// Token: 0x0200000F RID: 15
	internal class LerpMovement : MonoBehaviour
	{
		// Token: 0x0600002A RID: 42 RVA: 0x0000360B File Offset: 0x0000180B
		public void Update()
		{
		}

		// Token: 0x0400003D RID: 61
		public Vector3 endPoint;

		// Token: 0x0400003E RID: 62
		public Vector3 startPoint;

		// Token: 0x0400003F RID: 63
		private float duration = 2f;

		// Token: 0x04000040 RID: 64
		private float elapsedTime;
	}
}
