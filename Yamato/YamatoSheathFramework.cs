using System;
using ThunderRoad;

namespace Yamato
{
	// Token: 0x0200000F RID: 15
	public class YamatoSheathFramework : ItemModule
	{
		// Token: 0x06000046 RID: 70 RVA: 0x0000468E File Offset: 0x0000288E
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			item.gameObject.AddComponent<YamatoSheathFrameworkComponent>();
		}
	}
}
