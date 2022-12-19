using System;
using ThunderRoad;

namespace LioScripts
{
	// Token: 0x02000009 RID: 9
	internal class DragonicFireRuneItem : ItemModule
	{
		// Token: 0x0600001E RID: 30 RVA: 0x0000299F File Offset: 0x00000B9F
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			item.gameObject.AddComponent<DragonicFireRune>();
		}
	}
}
