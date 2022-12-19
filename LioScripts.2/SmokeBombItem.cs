using System;
using ThunderRoad;

namespace LioScripts
{
	// Token: 0x0200000F RID: 15
	internal class SmokeBombItem : ItemModule
	{
		// Token: 0x0600002A RID: 42 RVA: 0x00002B85 File Offset: 0x00000D85
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			item.gameObject.AddComponent<SmokeBomb>();
		}
	}
}
