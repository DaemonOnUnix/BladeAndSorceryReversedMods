using System;
using ThunderRoad;

namespace TotT
{
	// Token: 0x02000003 RID: 3
	public class GrapplingHook : ItemModule
	{
		// Token: 0x0600000D RID: 13 RVA: 0x0000235C File Offset: 0x0000055C
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			GrapplingHookParser.toggleMethod = this.toggleMethod;
			item.gameObject.AddComponent<GrapplingHookMono>();
		}

		// Token: 0x04000001 RID: 1
		public toggleMethod toggleMethod;
	}
}
