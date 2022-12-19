using System;
using ThunderRoad;

namespace SheathFramework
{
	// Token: 0x02000003 RID: 3
	public class ContentCustomDataSheath : ContentCustomData
	{
		// Token: 0x06000002 RID: 2 RVA: 0x00002089 File Offset: 0x00000289
		public ContentCustomDataSheath(string itemId)
		{
			this.sheathedItemId = itemId;
		}

		// Token: 0x04000001 RID: 1
		public string sheathedItemId;
	}
}
