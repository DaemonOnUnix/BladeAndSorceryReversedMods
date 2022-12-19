using System;
using ThunderRoad;
using UnityEngine;

namespace TotT
{
	// Token: 0x02000037 RID: 55
	public class PBBSleepModule : PhantomBladeBolt
	{
		// Token: 0x06000187 RID: 391 RVA: 0x0000B2B4 File Offset: 0x000094B4
		public override void OnStart()
		{
			base.OnStart();
			this.ammoMax = PBBSleepParser.ammoMax;
			this.itemID = this.item.data.id;
			this.eColor = PBBSleepParser.EmissionColor;
			this.KnockOutMinutes = PBBSleepParser.KnockOutMinutes;
			this.CombatDelaySeconds = PBBSleepParser.CombatDelaySeconds;
			foreach (Renderer r in this.item.GetComponentsInChildren<Renderer>())
			{
				r.material.SetColor("_EmissionColor", this.eColor);
			}
		}

		// Token: 0x06000188 RID: 392 RVA: 0x0000B344 File Offset: 0x00009544
		public override void OnCreatureHit(Creature creature)
		{
			Damager dam = this.item.GetComponentInChildren<Damager>();
			dam.UnPenetrateAll();
			bool flag = creature.gameObject.GetComponent<KnockOutBehaviour>() == null;
			if (flag)
			{
				KnockOutBehaviour temp = creature.gameObject.AddComponent<KnockOutBehaviour>();
				bool flag2 = creature.brain.state == 5 && this.CombatDelaySeconds > 0f;
				if (flag2)
				{
					temp.SetupWithDelay(this.KnockOutMinutes, this.CombatDelaySeconds);
				}
				else
				{
					temp.Setup(this.KnockOutMinutes);
				}
			}
			base.StartCoroutine(base.timeToDie(30f));
		}

		// Token: 0x06000189 RID: 393 RVA: 0x0000B3E4 File Offset: 0x000095E4
		public override string GetItemID()
		{
			return this.itemID;
		}

		// Token: 0x04000121 RID: 289
		private float KnockOutMinutes;

		// Token: 0x04000122 RID: 290
		private float CombatDelaySeconds;

		// Token: 0x04000123 RID: 291
		private string itemID;
	}
}
