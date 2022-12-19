using System;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

namespace SpellCastIce
{
	// Token: 0x02000009 RID: 9
	internal class SpellMergeIce : SpellMergeData
	{
		// Token: 0x0600002F RID: 47 RVA: 0x00003350 File Offset: 0x00001550
		public override void OnCatalogRefresh()
		{
			base.OnCatalogRefresh();
			this.shotEffectData = Catalog.GetData<EffectData>(this.shotEffectId, true);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x0000336C File Offset: 0x0000156C
		public override void Merge(bool active)
		{
			base.Merge(active);
			bool flag = !active;
			if (flag)
			{
				Vector3 vector = Player.local.transform.rotation * PlayerControl.GetHand(1).GetHandVelocity();
				Vector3 vector2 = Player.local.transform.rotation * PlayerControl.GetHand(0).GetHandVelocity();
				bool flag2 = vector.magnitude > SpellCaster.throwMinHandVelocity && vector2.magnitude > SpellCaster.throwMinHandVelocity;
				if (flag2)
				{
					bool flag3 = Vector3.Angle(vector, this.mana.casterLeft.magicSource.position - this.mana.mergePoint.position) < 45f || Vector3.Angle(vector2, this.mana.casterRight.magicSource.position - this.mana.mergePoint.position) < 45f;
					if (flag3)
					{
						bool flag4 = this.currentCharge >= this.shotMinCharge;
						if (flag4)
						{
							this.FireSpikes();
						}
					}
				}
			}
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00003490 File Offset: 0x00001690
		private void FireSpikes()
		{
			EffectInstance effectInstance = this.shotEffectData.Spawn(this.mana.mergePoint, true, null, false, Array.Empty<Type>());
			effectInstance.SetIntensity(0f);
			effectInstance.Play(0, false);
			int spikeAmount = Mathf.RoundToInt(Mathf.Clamp(this.maxSpawnAmount * this.currentCharge, this.minSpawnAmount, this.maxSpawnAmount));
			List<Item> spikes = new List<Item>();
			Action<Item> <>9__0;
			for (int i = 1; i <= spikeAmount; i++)
			{
				ItemData data = Catalog.GetData<ItemData>("IceSpike", true);
				Action<Item> action;
				if ((action = <>9__0) == null)
				{
					action = (<>9__0 = delegate(Item iceSpike)
					{
						iceSpike.transform.position = this.mana.mergePoint.position;
						iceSpike.transform.rotation = Quaternion.Euler(0f, (float)(360 / spikeAmount * spikes.Count), 0f);
						foreach (Item item in spikes)
						{
							item.IgnoreObjectCollision(iceSpike);
						}
						iceSpike.IgnoreRagdollCollision(Player.currentCreature.ragdoll);
						iceSpike.rb.AddForce(iceSpike.transform.forward * 35f, 1);
						iceSpike.Throw(1f, 2);
						spikes.Add(iceSpike);
					});
				}
				data.SpawnAsync(action, null, null, null, false, null);
			}
			this.currentCharge = 0f;
		}

		// Token: 0x0400001D RID: 29
		public float minSpawnAmount;

		// Token: 0x0400001E RID: 30
		public float maxSpawnAmount;

		// Token: 0x0400001F RID: 31
		public float shotMinCharge;

		// Token: 0x04000020 RID: 32
		public string shotEffectId;

		// Token: 0x04000021 RID: 33
		protected EffectData shotEffectData;
	}
}
