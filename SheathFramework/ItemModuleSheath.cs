using System;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

namespace SheathFramework
{
	// Token: 0x02000004 RID: 4
	public class ItemModuleSheath : ItemModule
	{
		// Token: 0x06000003 RID: 3 RVA: 0x0000209C File Offset: 0x0000029C
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			bool flag = this.grindingEffectId != null && this.grindingEffectId != "";
			if (flag)
			{
				this.grindingEffectData = Catalog.GetData<EffectData>(this.grindingEffectId, true);
			}
			bool flag2 = this.holderTransformName != null && this.holderTransformName != "";
			if (flag2)
			{
				foreach (Holder holder in item.GetComponentsInChildren<Holder>())
				{
					bool flag3 = holder.name == this.holderTransformName;
					if (flag3)
					{
						Object.Destroy(holder);
					}
				}
			}
			foreach (Collider collider in item.GetComponentsInChildren<Collider>())
			{
				bool flag4 = collider.name == this.transformName && collider.isTrigger;
				if (flag4)
				{
					collider.gameObject.SetActive(false);
					SheathHolder sheath = collider.gameObject.AddComponent<SheathHolder>();
					sheath.module = this;
					collider.gameObject.SetActive(true);
				}
			}
		}

		// Token: 0x04000002 RID: 2
		public string transformName = "Sheath";

		// Token: 0x04000003 RID: 3
		public FilterLogic tagFilter = 0;

		// Token: 0x04000004 RID: 4
		public List<string> items;

		// Token: 0x04000005 RID: 5
		public float maxDepth;

		// Token: 0x04000006 RID: 6
		public float snapDepth;

		// Token: 0x04000007 RID: 7
		public float damper = 100000f;

		// Token: 0x04000008 RID: 8
		public float damperHeld = 100f;

		// Token: 0x04000009 RID: 9
		public bool lockUnlessHeld;

		// Token: 0x0400000A RID: 10
		public string audioContainerLocation;

		// Token: 0x0400000B RID: 11
		public string spawnItemId;

		// Token: 0x0400000C RID: 12
		public float badAngle;

		// Token: 0x0400000D RID: 13
		public float badDepth;

		// Token: 0x0400000E RID: 14
		public string holderTransformName;

		// Token: 0x0400000F RID: 15
		public string grindingEffectId;

		// Token: 0x04000010 RID: 16
		public bool allowReverse;

		// Token: 0x04000011 RID: 17
		public bool toggleHolsteredInteraction;

		// Token: 0x04000012 RID: 18
		[NonSerialized]
		public EffectData grindingEffectData;
	}
}
