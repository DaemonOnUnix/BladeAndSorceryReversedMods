using System;
using ThunderRoad;

namespace LioScripts
{
	// Token: 0x02000004 RID: 4
	public class BearTrapItem : ItemModule
	{
		// Token: 0x0600000A RID: 10 RVA: 0x000023BE File Offset: 0x000005BE
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			item.gameObject.AddComponent<BearTrap>();
		}
	}
}
