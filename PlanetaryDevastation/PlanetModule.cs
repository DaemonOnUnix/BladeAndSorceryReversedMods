using System;
using ThunderRoad;

namespace PlanetaryDevastation
{
	// Token: 0x02000005 RID: 5
	internal class PlanetModule : ItemModule
	{
		// Token: 0x06000015 RID: 21 RVA: 0x00002CE2 File Offset: 0x00000EE2
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			item.gameObject.AddComponent<Planet>();
		}
	}
}
