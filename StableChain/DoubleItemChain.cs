using System;
using ThunderRoad;

namespace StableChain
{
	// Token: 0x02000008 RID: 8
	public class DoubleItemChain : ItemModule
	{
		// Token: 0x06000013 RID: 19 RVA: 0x00003910 File Offset: 0x00001B10
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			bool flag = Level.current.data.id == "CharacterSelection";
			if (!flag)
			{
				this.item.gameObject.AddComponent<DoubleChainBehaviour>();
			}
		}
	}
}
