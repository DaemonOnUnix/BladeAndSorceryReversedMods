using System;
using ThunderRoad;

namespace TotT
{
	// Token: 0x02000024 RID: 36
	public class DishonoredFireBolt : ItemModule
	{
		// Token: 0x0600010A RID: 266 RVA: 0x00008A7F File Offset: 0x00006C7F
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			item.gameObject.AddComponent<FireBoltModule>();
		}
	}
}
