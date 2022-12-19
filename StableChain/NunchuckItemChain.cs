using System;
using ThunderRoad;

namespace StableChain
{
	// Token: 0x02000004 RID: 4
	public class NunchuckItemChain : ItemModule
	{
		// Token: 0x06000007 RID: 7 RVA: 0x00002AA0 File Offset: 0x00000CA0
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			bool flag = Level.current.data.id == "CharacterSelection";
			if (!flag)
			{
				this.item.gameObject.AddComponent<NunchuckChainBehaviour>();
			}
		}
	}
}
