using System;
using ThunderRoad;
using UnityEngine;

namespace TotT
{
	// Token: 0x02000012 RID: 18
	public class AmmoModuleMono : MonoBehaviour
	{
		// Token: 0x06000092 RID: 146 RVA: 0x0000585C File Offset: 0x00003A5C
		public void Start()
		{
			this.item = base.GetComponent<Item>();
			this.BoltID = AmmoModuleParser.BoltID;
			this.ModuleColor = AmmoModuleParser.ModuleColor;
			this.item.renderers[1].material.SetColor("_EmissionColor", this.ModuleColor);
			this.item.renderers[2].material.SetColor("_EmissionColor", this.ModuleColor);
		}

		// Token: 0x06000093 RID: 147 RVA: 0x000058DA File Offset: 0x00003ADA
		public void Update()
		{
		}

		// Token: 0x0400005E RID: 94
		private Item item;

		// Token: 0x0400005F RID: 95
		public string BoltID;

		// Token: 0x04000060 RID: 96
		public Color ModuleColor;
	}
}
