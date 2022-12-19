using System;

namespace Sectory
{
	// Token: 0x02000017 RID: 23
	[AttributeUsage(AttributeTargets.Field)]
	public class ActionImage : Attribute
	{
		// Token: 0x06000038 RID: 56 RVA: 0x00003461 File Offset: 0x00001661
		public ActionImage(string address)
		{
			this.address = address;
		}

		// Token: 0x0400008D RID: 141
		public string address;
	}
}
