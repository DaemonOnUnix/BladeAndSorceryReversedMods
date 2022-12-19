using System;
using ThunderRoad;

namespace UltimateFantasyPack
{
	// Token: 0x02000017 RID: 23
	public class ItemReturnItem : ItemModule
	{
		// Token: 0x06000056 RID: 86 RVA: 0x00003653 File Offset: 0x00001853
		public override void OnItemLoaded(Item item)
		{
			item.gameObject.AddComponent<ItemReturn>().Setup(this.returnPower);
			base.OnItemLoaded(item);
		}

		// Token: 0x0400003D RID: 61
		public float returnPower;
	}
}
