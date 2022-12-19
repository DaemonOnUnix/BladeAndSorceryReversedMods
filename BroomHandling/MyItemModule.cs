using System;
using ThunderRoad;

namespace BroomHandling
{
	// Token: 0x02000002 RID: 2
	public class MyItemModule : ItemModule
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			item.gameObject.AddComponent<MyWeaponComponent>();
		}

		// Token: 0x04000001 RID: 1
		public float spellSpeed;

		// Token: 0x04000002 RID: 2
		public bool magicEffect;
	}
}
