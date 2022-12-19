using System;
using ThunderRoad;
using UnityEngine;

namespace TotT
{
	// Token: 0x02000034 RID: 52
	public class PBBStingModule : PhantomBladeBolt
	{
		// Token: 0x06000179 RID: 377 RVA: 0x0000B118 File Offset: 0x00009318
		public override void OnStart()
		{
			base.OnStart();
			this.ammoMax = PBBStingParser.ammoMax;
			this.itemID = this.item.data.id;
			this.eColor = PBBStingParser.EmissionColor;
			foreach (Renderer r in this.item.GetComponentsInChildren<Renderer>())
			{
				r.material.SetColor("_EmissionColor", this.eColor);
			}
		}

		// Token: 0x0600017A RID: 378 RVA: 0x0000B194 File Offset: 0x00009394
		public override void OnCreatureHit(Creature creature)
		{
			Damager dam = this.item.GetComponentInChildren<Damager>();
			dam.UnPenetrateAll();
			bool flag = creature.gameObject.GetComponent<RageAttackBehaviour>() == null;
			if (flag)
			{
				RageAttackBehaviour temp = creature.gameObject.AddComponent<RageAttackBehaviour>();
				temp.Setup(30f);
			}
			base.StartCoroutine(base.timeToDie(30f));
		}

		// Token: 0x0600017B RID: 379 RVA: 0x0000B1F8 File Offset: 0x000093F8
		public override string GetItemID()
		{
			return this.itemID;
		}

		// Token: 0x04000118 RID: 280
		private string itemID;
	}
}
