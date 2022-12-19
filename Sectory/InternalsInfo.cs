using System;
using UnityEngine;

namespace Sectory
{
	// Token: 0x0200000F RID: 15
	[Serialize("InternalsSettings")]
	[Serializable]
	public class InternalsInfo
	{
		// Token: 0x0600001F RID: 31 RVA: 0x000027F5 File Offset: 0x000009F5
		public InternalsInfo(InternalsInfo.Internal[] internals)
		{
			this.internals = internals;
		}

		// Token: 0x04000066 RID: 102
		[Range(0, 60)]
		public float criticalBleedingDuration;

		// Token: 0x04000067 RID: 103
		[Range(0, 60)]
		public float respirationOxygenCutOffTimer;

		// Token: 0x04000068 RID: 104
		[Range(0, 1)]
		public float bluntTriggerInternalRatio;

		// Token: 0x04000069 RID: 105
		public bool internalsEnabled;

		// Token: 0x0400006A RID: 106
		public InternalsInfo.Internal[] internals;

		// Token: 0x02000027 RID: 39
		public class Internal
		{
			// Token: 0x0600008E RID: 142 RVA: 0x00007698 File Offset: 0x00005898
			public Internal(string name, string host, Vector3 offset, float size, float penetrationDepth, float bleedMultiplier, bool pierceAllowed, bool slashAllowed, InternalInjuryAction action, float durability)
			{
				this.name = name;
				this.host = host;
				this.offset = offset;
				this.size = size;
				this.penetrationDepth = penetrationDepth;
				this.pierceAllowed = pierceAllowed;
				this.slashAllowed = slashAllowed;
				this.bleedMultiplier = bleedMultiplier;
				this.action = action;
				this.durability = durability;
			}

			// Token: 0x040000D6 RID: 214
			public string name;

			// Token: 0x040000D7 RID: 215
			public string host;

			// Token: 0x040000D8 RID: 216
			public Vector3 offset;

			// Token: 0x040000D9 RID: 217
			public float size;

			// Token: 0x040000DA RID: 218
			public float penetrationDepth;

			// Token: 0x040000DB RID: 219
			public float bleedMultiplier;

			// Token: 0x040000DC RID: 220
			public bool pierceAllowed;

			// Token: 0x040000DD RID: 221
			public bool slashAllowed;

			// Token: 0x040000DE RID: 222
			public bool bluntAllowed;

			// Token: 0x040000DF RID: 223
			public InternalInjuryAction action;

			// Token: 0x040000E0 RID: 224
			public float durability;
		}
	}
}
