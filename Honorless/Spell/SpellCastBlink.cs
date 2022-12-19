using System;
using ThunderRoad;
using Wully.Mono;

namespace Wully.Spell
{
	// Token: 0x02000006 RID: 6
	public class SpellCastBlink : SpellCastCharge
	{
		// Token: 0x0600001C RID: 28 RVA: 0x00002844 File Offset: 0x00000A44
		public SpellCastBlink Clone()
		{
			return base.MemberwiseClone() as SpellCastBlink;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002854 File Offset: 0x00000A54
		public override void OnCatalogRefresh()
		{
			base.OnCatalogRefresh();
			if (!string.IsNullOrEmpty(this.targetEffectId))
			{
				this.targetEffectData = Catalog.GetData<EffectData>(this.targetEffectId, true);
			}
			if (!string.IsNullOrEmpty(this.markerEffectId))
			{
				this.markerEffectData = Catalog.GetData<EffectData>(this.markerEffectId, true);
			}
			if (!string.IsNullOrEmpty(this.ledgeEffectId))
			{
				this.ledgeEffectData = Catalog.GetData<EffectData>(this.ledgeEffectId, true);
			}
			if (!string.IsNullOrEmpty(this.teleportEffectId))
			{
				this.teleportEffectData = Catalog.GetData<EffectData>(this.teleportEffectId, true);
			}
			if (!string.IsNullOrEmpty(this.blinkStartEffectId))
			{
				this.blinkStartData = Catalog.GetData<EffectData>(this.blinkStartEffectId, true);
			}
			if (!string.IsNullOrEmpty(this.blinkTravelEffectId))
			{
				this.blinkTravelData = Catalog.GetData<EffectData>(this.blinkTravelEffectId, true);
			}
			if (!string.IsNullOrEmpty(this.blinkLoopEffectId))
			{
				this.blinkLoopData = Catalog.GetData<EffectData>(this.blinkLoopEffectId, true);
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002940 File Offset: 0x00000B40
		public override void Load(SpellCaster spellCaster, SpellData.Level level)
		{
			base.Load(spellCaster, level);
			this.side = spellCaster.ragdollHand.side;
			if (this.side == 1)
			{
				if (Blink.localLeft == null)
				{
					Blink.localLeft = spellCaster.ragdollHand.gameObject.AddComponent<Blink>();
				}
				this.blink = Blink.localLeft;
				this.blink.side = 1;
			}
			else
			{
				if (Blink.localRight == null)
				{
					Blink.localRight = spellCaster.ragdollHand.gameObject.AddComponent<Blink>();
				}
				this.blink = Blink.localRight;
				this.blink.side = 0;
			}
			this.blink.spellCaster = spellCaster;
			this.blink.range = this.range;
			this.blink.teleportSpeed = this.teleportSpeed;
			if (this.targetEffectData != null)
			{
				this.targetEffectInstance = this.targetEffectData.Spawn(this.spellCaster.magic, false, null, false, Array.Empty<Type>());
			}
			if (this.markerEffectData != null)
			{
				this.markerEffectInstance = this.markerEffectData.Spawn(this.spellCaster.magic, false, null, false, Array.Empty<Type>());
			}
			if (this.ledgeEffectData != null)
			{
				this.ledgeEffectInstance = this.ledgeEffectData.Spawn(this.spellCaster.magic, false, null, false, Array.Empty<Type>());
			}
			this.blink.groundMarker = this.markerEffectInstance;
			this.blink.climbMarker = this.ledgeEffectInstance;
			this.blink.targetMarker = this.targetEffectInstance;
			this.blink.teleportData = this.teleportEffectData;
			this.blink.blinkStartData = this.blinkStartData;
			this.blink.blinkTravelData = this.blinkTravelData;
			this.blink.blinkLoopData = this.blinkLoopData;
			this.blink.Load();
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002B18 File Offset: 0x00000D18
		public override void Unload()
		{
			this.blink.groundMarker = null;
			this.blink.climbMarker = null;
			this.blink.targetMarker = null;
			EffectInstance effectInstance = this.targetEffectInstance;
			if (effectInstance != null)
			{
				effectInstance.Despawn();
			}
			EffectInstance effectInstance2 = this.markerEffectInstance;
			if (effectInstance2 != null)
			{
				effectInstance2.Despawn();
			}
			EffectInstance effectInstance3 = this.ledgeEffectInstance;
			if (effectInstance3 != null)
			{
				effectInstance3.Despawn();
			}
			this.blink.SpellLoaded = false;
			this.blink.Unload();
			base.Unload();
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002B99 File Offset: 0x00000D99
		public override void Fire(bool active)
		{
			base.Fire(active);
			this.blink.FireActive = active;
		}

		// Token: 0x04000012 RID: 18
		public float range = 20f;

		// Token: 0x04000013 RID: 19
		public float teleportSpeed = 50f;

		// Token: 0x04000014 RID: 20
		public string targetEffectId;

		// Token: 0x04000015 RID: 21
		public string markerEffectId;

		// Token: 0x04000016 RID: 22
		public string ledgeEffectId;

		// Token: 0x04000017 RID: 23
		public string teleportEffectId;

		// Token: 0x04000018 RID: 24
		protected EffectData targetEffectData;

		// Token: 0x04000019 RID: 25
		protected EffectData markerEffectData;

		// Token: 0x0400001A RID: 26
		protected EffectData ledgeEffectData;

		// Token: 0x0400001B RID: 27
		protected EffectData teleportEffectData;

		// Token: 0x0400001C RID: 28
		private EffectInstance targetEffectInstance;

		// Token: 0x0400001D RID: 29
		private EffectInstance markerEffectInstance;

		// Token: 0x0400001E RID: 30
		private EffectInstance ledgeEffectInstance;

		// Token: 0x0400001F RID: 31
		private EffectInstance teleportEffectInstance;

		// Token: 0x04000020 RID: 32
		public string blinkStartEffectId;

		// Token: 0x04000021 RID: 33
		public string blinkTravelEffectId;

		// Token: 0x04000022 RID: 34
		public string blinkLoopEffectId;

		// Token: 0x04000023 RID: 35
		protected EffectData blinkStartData;

		// Token: 0x04000024 RID: 36
		protected EffectData blinkTravelData;

		// Token: 0x04000025 RID: 37
		protected EffectData blinkLoopData;

		// Token: 0x04000026 RID: 38
		private Blink blink;

		// Token: 0x04000027 RID: 39
		private Side side;
	}
}
