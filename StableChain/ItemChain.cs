﻿using System;
using ThunderRoad;

namespace StableChain
{
	// Token: 0x02000002 RID: 2
	public class ItemChain : ItemModule
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			bool flag = Level.current.data.id == "CharacterSelection";
			if (!flag)
			{
				this.item.gameObject.AddComponent<ChainBehaviour>();
			}
		}
	}
}
