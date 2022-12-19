using System;
using ThunderRoad;

namespace LioScripts
{
	// Token: 0x02000013 RID: 19
	internal class SmokeBombItem : ItemModule
	{
		// Token: 0x0600003C RID: 60 RVA: 0x00002FC1 File Offset: 0x000011C1
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			item.gameObject.AddComponent<SmokeBomb>();
		}
	}
}
