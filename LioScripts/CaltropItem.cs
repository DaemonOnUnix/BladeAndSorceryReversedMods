using System;
using ThunderRoad;

namespace LioScripts
{
	// Token: 0x02000006 RID: 6
	public class CaltropItem : ItemModule
	{
		// Token: 0x0600000F RID: 15 RVA: 0x00002607 File Offset: 0x00000807
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			item.gameObject.AddComponent<Caltrop>();
		}
	}
}
