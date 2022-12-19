using System;
using ThunderRoad;

namespace LioScripts
{
	// Token: 0x0200000B RID: 11
	internal class FirePitItem : ItemModule
	{
		// Token: 0x06000023 RID: 35 RVA: 0x00002AC2 File Offset: 0x00000CC2
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			item.gameObject.AddComponent<FirePit>();
		}
	}
}
