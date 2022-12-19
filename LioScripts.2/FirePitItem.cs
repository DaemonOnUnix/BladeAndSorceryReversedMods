using System;
using ThunderRoad;

namespace LioScripts
{
	// Token: 0x02000008 RID: 8
	internal class FirePitItem : ItemModule
	{
		// Token: 0x06000014 RID: 20 RVA: 0x0000272A File Offset: 0x0000092A
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			item.gameObject.AddComponent<FirePit>();
		}
	}
}
