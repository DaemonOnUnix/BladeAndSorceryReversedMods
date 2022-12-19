using System;
using UnityEngine;

namespace WandSpellss
{
	// Token: 0x02000015 RID: 21
	internal class Soul : MonoBehaviour
	{
		// Token: 0x06000041 RID: 65 RVA: 0x00003D94 File Offset: 0x00001F94
		public void Start()
		{
			this.currentSoul = this.maxSoul;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00003DA4 File Offset: 0x00001FA4
		public float DivideSoul()
		{
			this.currentSoul /= 2f;
			return this.currentSoul;
		}

		// Token: 0x04000059 RID: 89
		internal float maxSoul = 100f;

		// Token: 0x0400005A RID: 90
		internal float currentSoul;
	}
}
