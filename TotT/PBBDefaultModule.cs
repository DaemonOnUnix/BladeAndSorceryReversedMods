using System;
using UnityEngine;

namespace TotT
{
	// Token: 0x02000031 RID: 49
	public class PBBDefaultModule : PhantomBladeBolt
	{
		// Token: 0x06000170 RID: 368 RVA: 0x0000B030 File Offset: 0x00009230
		public override void OnStart()
		{
			base.OnStart();
			this.ammoMax = PBBDefaultParser.ammoMax;
			this.eColor = PBBDefaultParser.EmissionColor;
			foreach (Renderer r in this.item.GetComponentsInChildren<Renderer>())
			{
				r.material.SetColor("_EmissionColor", this.eColor);
			}
		}

		// Token: 0x06000171 RID: 369 RVA: 0x0000B094 File Offset: 0x00009294
		public override string GetItemID()
		{
			return this.item.data.id;
		}
	}
}
