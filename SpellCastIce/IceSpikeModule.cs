using System;
using ThunderRoad;

namespace SpellCastIce
{
	// Token: 0x02000005 RID: 5
	public class IceSpikeModule : ItemModule
	{
		// Token: 0x0600001C RID: 28 RVA: 0x00002BC4 File Offset: 0x00000DC4
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			IceSpikeItem iceSpikeItem = item.gameObject.AddComponent<IceSpikeItem>();
			iceSpikeItem.item = item;
			iceSpikeItem.module = this;
			iceSpikeItem.Initialize();
		}
	}
}
