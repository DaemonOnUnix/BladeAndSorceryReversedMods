using System;
using ThunderRoad;

namespace StableChain
{
	// Token: 0x02000006 RID: 6
	public class ThalDarasItemChain : ItemModule
	{
		// Token: 0x0600000D RID: 13 RVA: 0x00002F68 File Offset: 0x00001168
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			bool flag = Level.current.data.id == "CharacterSelection";
			if (!flag)
			{
				this.item.gameObject.AddComponent<ThalDarasChainBehaviour>();
			}
		}
	}
}
