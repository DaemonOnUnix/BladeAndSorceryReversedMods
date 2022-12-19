using System;
using ThunderRoad;

namespace SaberMod
{
	// Token: 0x02000003 RID: 3
	public class ItemModuleElectroStaff : ItemModule
	{
		// Token: 0x06000018 RID: 24 RVA: 0x0000226D File Offset: 0x0000046D
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			if (item.gameObject.GetComponent<ItemElectroStaff>() == null)
			{
				item.gameObject.AddComponent<ItemElectroStaff>();
			}
		}
	}
}
