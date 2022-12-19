using System;
using ThunderRoad;

namespace TotT
{
	// Token: 0x02000019 RID: 25
	public class PhantomBladeBoltHolder : ItemModule
	{
		// Token: 0x060000BB RID: 187 RVA: 0x000065EA File Offset: 0x000047EA
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			item.gameObject.AddComponent<PBBHolderModule>();
		}
	}
}
