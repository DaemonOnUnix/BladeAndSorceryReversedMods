using System;
using ThunderRoad;

namespace CustomAvatarFramework
{
	// Token: 0x02000028 RID: 40
	public class ItemModuleAvatarEquipment : ItemModule
	{
		// Token: 0x060000C2 RID: 194 RVA: 0x000078D0 File Offset: 0x00005AD0
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			item.gameObject.AddComponent<AvatarEquipmentItem>();
		}

		// Token: 0x040000D2 RID: 210
		public string avatarApparelId;

		// Token: 0x040000D3 RID: 211
		public string avatarCreatureId;
	}
}
