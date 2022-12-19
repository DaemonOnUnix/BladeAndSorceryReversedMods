using System;

namespace Sectory
{
	// Token: 0x0200000E RID: 14
	[Serialize("AiSettings")]
	[Serializable]
	public class AiSettings
	{
		// Token: 0x04000061 RID: 97
		[Range(0, 20)]
		public float npcParryMultiplier;

		// Token: 0x04000062 RID: 98
		[Range(0, 100)]
		public float dodgeChance;

		// Token: 0x04000063 RID: 99
		[Range(0, 3)]
		public float attackAnimationSpeedMult;

		// Token: 0x04000064 RID: 100
		public bool canDodgeWhenGrabbed;

		// Token: 0x04000065 RID: 101
		public bool recoilOnParry;
	}
}
