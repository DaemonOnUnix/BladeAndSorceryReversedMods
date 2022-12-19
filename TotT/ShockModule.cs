using System;
using ThunderRoad;

namespace TotT
{
	// Token: 0x0200004B RID: 75
	public class ShockModule : ItemModule
	{
		// Token: 0x060001EF RID: 495 RVA: 0x0000D282 File Offset: 0x0000B482
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			ShockParser.toggleMethod = this.toggleMethod;
			ShockParser.RechargeMultiplier = this.RechargeMultiplier;
			item.gameObject.AddComponent<ShockModuleMono>();
		}

		// Token: 0x04000159 RID: 345
		public toggleMethod toggleMethod;

		// Token: 0x0400015A RID: 346
		public float RechargeMultiplier;
	}
}
