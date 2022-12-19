using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sectory;
using ThunderRoad;
using UnityEngine;

namespace LightningReflex
{
	// Token: 0x02000002 RID: 2
	public class Entry : LevelModule
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public override IEnumerator OnLoadCoroutine()
		{
			Entry.inst = this;
			SectoryMenu.InjectClassIntoMenu(new SectoryMenu.InjectEditClass("LightningReflexes", this.options));
			return base.OnLoadCoroutine();
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002084 File Offset: 0x00000284
		public override void Update()
		{
			base.Update();
			bool flag = this.options == null || !Player.currentCreature;
			if (!flag)
			{
				bool on = this.options.on;
				if (on)
				{
					Transform playerHead = Player.currentCreature.ragdoll.headPart.transform;
					HashSet<Rigidbody> hashSet = new HashSet<Rigidbody>();
					bool flag2 = !this.forceSet && GameManager.slowMotionState != 2;
					if (flag2)
					{
						foreach (Item item in Item.allActive.Where(new Func<Item, bool>(this.MeetsCriteria)))
						{
							bool flag3 = (item.transform.position - playerHead.position).sqrMagnitude <= this.options.range * this.options.range && item.rb.velocity.sqrMagnitude > this.options.minimumReflexTrigger * this.options.minimumReflexTrigger && Vector3.Dot(item.rb.velocity, (playerHead.position - item.transform.position).normalized) > 0.5f;
							if (flag3)
							{
								hashSet.Add(item.rb);
							}
						}
						bool flag4 = hashSet.Any<Rigidbody>();
						if (flag4)
						{
							this.currentTrigger = hashSet.OrderBy((Rigidbody rb) => (playerHead.position - rb.transform.position).sqrMagnitude).First<Rigidbody>();
						}
					}
					bool flag5 = this.currentTrigger != null;
					if (flag5)
					{
						float sqrMagnitude = (this.currentTrigger.transform.position - playerHead.position).sqrMagnitude;
						bool flag6 = sqrMagnitude > this.options.range * this.options.range || this.currentTrigger.velocity.sqrMagnitude < this.options.minimumReflexTrigger * this.options.minimumReflexTrigger || Vector3.Dot(this.currentTrigger.velocity, (playerHead.position - this.currentTrigger.transform.position).normalized) < 0.5f;
						if (flag6)
						{
							this.currentTrigger = null;
							GameManager.options.physicTimeStep = (Entry.inst.generalInfo.lowPhysicsForFPSBoost ? 0 : 1);
							GameManager.SetSlowMotion(false, 1f, Catalog.GetData<SpellPowerSlowTime>("SlowTime", true).exitCurve, null, true);
							GameManager.SaveOptions(false);
							GameManager.LoadOptions();
							this.forceSet = false;
						}
						else
						{
							this.forceSet = true;
							bool flag7 = !this.options.stayInLowQuality;
							if (flag7)
							{
								GameManager.options.physicTimeStep = 0;
							}
							GameManager.SetSlowMotion(true, Mathf.Min(Mathf.Abs(sqrMagnitude / (this.options.reflexStrength * this.options.reflexStrength) - 1f) * this.options.reflexMultiplier, 0.5f), Catalog.GetData<SpellPowerSlowTime>("SlowTime", true).enterCurve, null, true);
						}
					}
				}
			}
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002404 File Offset: 0x00000604
		private bool MeetsCriteria(Item item)
		{
			RagdollHand mainHandler = item.mainHandler;
			bool flag = true;
			bool flag2 = ((mainHandler != null) ? mainHandler.creature : null) == Player.currentCreature;
			if (flag2)
			{
				flag = false;
			}
			bool flag3 = item.tkHandlers.Any<SpellCaster>();
			if (flag3)
			{
				flag = false;
			}
			bool flag4 = this.options.projectilesOnly && mainHandler;
			if (flag4)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x04000001 RID: 1
		public static Entry inst;

		// Token: 0x04000002 RID: 2
		public Entry.Options options;

		// Token: 0x04000003 RID: 3
		private Rigidbody currentTrigger;

		// Token: 0x04000004 RID: 4
		private bool forceSet;

		// Token: 0x02000003 RID: 3
		[Serializable]
		public class Options
		{
			// Token: 0x04000005 RID: 5
			[Range(0, 10)]
			public float range;

			// Token: 0x04000006 RID: 6
			[Range(0, 200)]
			public float reflexStrength;

			// Token: 0x04000007 RID: 7
			[Range(0, 1)]
			public float reflexMultiplier;

			// Token: 0x04000008 RID: 8
			[Range(0, 10)]
			public float minimumReflexTrigger;

			// Token: 0x04000009 RID: 9
			public bool projectilesOnly;

			// Token: 0x0400000A RID: 10
			public bool stayInLowQuality;

			// Token: 0x0400000B RID: 11
			public bool on;
		}
	}
}
