using System;
using ThunderRoad;

namespace CustomAvatarFramework
{
	// Token: 0x0200002A RID: 42
	public class ItemModuleAvatarReset : ItemModule
	{
		// Token: 0x060000C8 RID: 200 RVA: 0x00007A8A File Offset: 0x00005C8A
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			item.gameObject.AddComponent<AvatarResetItem>();
		}
	}
}
