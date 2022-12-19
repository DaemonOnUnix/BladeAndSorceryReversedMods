using System;
using ThunderRoad;

namespace UltimateFantasyPack
{
	// Token: 0x02000027 RID: 39
	public class VampItem : ItemModule
	{
		// Token: 0x0600009B RID: 155 RVA: 0x00004E55 File Offset: 0x00003055
		public override void OnItemLoaded(Item item)
		{
			item.gameObject.AddComponent<Vampiric>();
			base.OnItemLoaded(item);
		}
	}
}
