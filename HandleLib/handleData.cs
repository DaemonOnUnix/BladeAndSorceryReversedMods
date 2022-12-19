using System;
using System.Collections.Generic;
using ThunderRoad;

namespace HandleLib
{
	// Token: 0x02000003 RID: 3
	public class handleData : ItemModule
	{
		// Token: 0x06000005 RID: 5 RVA: 0x0000213E File Offset: 0x0000033E
		public override void OnItemLoaded(Item item)
		{
			item.gameObject.AddComponent<handleClass>().data = this;
			base.OnItemLoaded(item);
		}

		// Token: 0x04000004 RID: 4
		public bool useSpellMenu;

		// Token: 0x04000005 RID: 5
		public List<string> flippables;
	}
}
