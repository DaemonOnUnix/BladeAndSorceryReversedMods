using System;
using ThunderRoad;

namespace TotT
{
	// Token: 0x0200003E RID: 62
	public class DishonoredRopeBolt : ItemModule
	{
		// Token: 0x060001C2 RID: 450 RVA: 0x0000C9CA File Offset: 0x0000ABCA
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			item.gameObject.AddComponent<RopeBoltModule>();
		}
	}
}
