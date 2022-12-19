using System;
using ThunderRoad;
using UnityEngine;

namespace TotT
{
	// Token: 0x02000010 RID: 16
	public class AmmoModule : ItemModule
	{
		// Token: 0x0600008B RID: 139 RVA: 0x000057FC File Offset: 0x000039FC
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			AmmoModuleParser.BoltID = this.BoltID;
			AmmoModuleParser.ModuleColor = this.ModuleColor;
			item.gameObject.AddComponent<AmmoModuleMono>();
		}

		// Token: 0x0400005A RID: 90
		public string BoltID;

		// Token: 0x0400005B RID: 91
		public Color ModuleColor;
	}
}
