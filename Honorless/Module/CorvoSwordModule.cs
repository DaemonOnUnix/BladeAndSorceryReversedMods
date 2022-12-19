using System;
using ThunderRoad;
using Wully.Mono;

namespace Wully.Module
{
	// Token: 0x0200000E RID: 14
	public class CorvoSwordModule : ItemModule
	{
		// Token: 0x0600005A RID: 90 RVA: 0x00004B58 File Offset: 0x00002D58
		public override void OnItemDataRefresh(ItemData data)
		{
			base.OnItemDataRefresh(data);
			if (!string.IsNullOrEmpty(this.spinHandPoseId))
			{
				this.spinHandPoseData = Catalog.GetData<HandPoseData>(this.spinHandPoseId, true);
			}
			this.handleLarge = Catalog.GetData<HandPoseData>("HandleLarge", true);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00004B94 File Offset: 0x00002D94
		public override void OnItemLoaded(Item loadedItem)
		{
			base.OnItemLoaded(loadedItem);
			this.corvoSword = loadedItem.gameObject.AddComponent<CorvoSword>();
			this.corvoSword.spinHandPoseData = this.spinHandPoseData;
			this.corvoSword.handleData = this.handleLarge;
			this.corvoSword.animationSpeed = this.animationSpeed;
			this.corvoSword.Load();
		}

		// Token: 0x04000078 RID: 120
		private CorvoSword corvoSword;

		// Token: 0x04000079 RID: 121
		public float animationSpeed = 2f;

		// Token: 0x0400007A RID: 122
		public string spinHandPoseId;

		// Token: 0x0400007B RID: 123
		private HandPoseData spinHandPoseData;

		// Token: 0x0400007C RID: 124
		private HandPoseData handleLarge;
	}
}
