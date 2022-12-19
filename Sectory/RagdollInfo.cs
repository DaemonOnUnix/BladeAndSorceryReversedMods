using System;
using System.Linq;

namespace Sectory
{
	// Token: 0x02000009 RID: 9
	[Serialize("RagdollModifiers")]
	[Serializable]
	public class RagdollInfo
	{
		// Token: 0x06000012 RID: 18 RVA: 0x00002694 File Offset: 0x00000894
		public RagdollInfo.Info GetInfo(string partName)
		{
			return this.values.Where((RagdollInfo.Info i) => i.ragdollPart == partName).FirstOrDefault<RagdollInfo.Info>();
		}

		// Token: 0x04000031 RID: 49
		public bool sectoryChangeRagdollWeights;

		// Token: 0x04000032 RID: 50
		public RagdollInfo.Info[] values;

		// Token: 0x02000023 RID: 35
		[Serializable]
		public class Info
		{
			// Token: 0x06000086 RID: 134 RVA: 0x000075A0 File Offset: 0x000057A0
			public Info(string ragdollPart, float drag, float mass, float angularDrag)
			{
				this.ragdollPart = ragdollPart;
				this.drag = drag;
				this.mass = mass;
				this.angularDrag = angularDrag;
			}

			// Token: 0x040000C4 RID: 196
			public string ragdollPart;

			// Token: 0x040000C5 RID: 197
			public float drag;

			// Token: 0x040000C6 RID: 198
			public float mass;

			// Token: 0x040000C7 RID: 199
			public float angularDrag;
		}
	}
}
