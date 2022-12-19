using System;
using ThunderRoad;

namespace ShotgunShellHolder
{
	// Token: 0x02000003 RID: 3
	internal class BulletHolderModule : ItemModule
	{
		// Token: 0x06000006 RID: 6 RVA: 0x000021D8 File Offset: 0x000003D8
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			item.gameObject.AddComponent<BulletHolderSpawner>();
		}

		// Token: 0x04000004 RID: 4
		public string holderRef = "";

		// Token: 0x04000005 RID: 5
		public string ammoID = "";
	}
}
