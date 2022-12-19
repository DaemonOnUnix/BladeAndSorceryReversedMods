using System;

namespace Sectory
{
	// Token: 0x02000018 RID: 24
	[AttributeUsage(AttributeTargets.Class)]
	internal class Serialize : Attribute
	{
		// Token: 0x06000039 RID: 57 RVA: 0x00003471 File Offset: 0x00001671
		public Serialize(string path)
		{
			this.path = path;
		}

		// Token: 0x0400008E RID: 142
		public string path;
	}
}
