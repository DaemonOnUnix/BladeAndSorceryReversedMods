using System;
using ExtensionMethods;
using ThunderRoad;
using UnityEngine;

namespace MysticHands
{
	// Token: 0x0200000C RID: 12
	public class SpellMysticHands : SpellCastCharge
	{
		// Token: 0x060000DD RID: 221 RVA: 0x00007A04 File Offset: 0x00005C04
		public override void OnCatalogRefresh()
		{
			base.OnCatalogRefresh();
			SpellMysticHands.configHandColor = this.handColor;
			SpellMysticHands.configLightColor = this.lightColor;
			SpellMysticHands.configShowTrail = this.showTrail;
			SpellMysticHands.configScale = this.scale;
			SpellMysticHands.configDistanceMult = this.distanceMult;
			SpellMysticHands.configLengthMult = this.lengthMult;
			SpellMysticHands.configShockwaveRadius = this.shockwaveRadius;
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00007A68 File Offset: 0x00005C68
		public override void Load(SpellCaster spellCaster, SpellData.Level level)
		{
			base.Load(spellCaster, level);
			this.claw = this.defaultMode == Mode.Claws;
			this.controller = Player.currentCreature.mana.gameObject.GetOrAddComponent<HandController>();
			this.controller.Init();
			this.state = new StateTracker();
			this.state.Toggle(() => spellCaster.ragdollHand.Gripping() && spellCaster.ragdollHand.Empty() && this.controller.hands[spellCaster.ragdollHand.side].active, delegate()
			{
				spellCaster.DisableSpellWheel(this);
			}, delegate()
			{
				spellCaster.AllowSpellWheel(this);
			}, 0f).On(() => spellCaster.ragdollHand.Gripping() && spellCaster.ragdollHand.Empty() && this.controller.hands[spellCaster.ragdollHand.side].active && spellCaster.ragdollHand.playerHand.controlHand.alternateUsePressed, delegate()
			{
				this.claw = !this.claw;
				spellCaster.ragdollHand.HapticTick(1f, 10f, this.claw ? 2 : 1);
				this.controller.SwitchMode(spellCaster.ragdollHand.side, this.claw);
			}, 0f);
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00007B30 File Offset: 0x00005D30
		public override void Fire(bool active)
		{
			base.Fire(active);
			if (active)
			{
				this.controller.Toggle(this.spellCaster.ragdollHand.side, this.claw);
			}
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00007B6F File Offset: 0x00005D6F
		public override void UpdateCaster()
		{
			base.UpdateCaster();
			this.state.Update();
		}

		// Token: 0x04000029 RID: 41
		public static Color configHandColor;

		// Token: 0x0400002A RID: 42
		public static Color configLightColor;

		// Token: 0x0400002B RID: 43
		public static bool configShowTrail;

		// Token: 0x0400002C RID: 44
		public static float configScale = 1f;

		// Token: 0x0400002D RID: 45
		public static float configDistanceMult = 1f;

		// Token: 0x0400002E RID: 46
		public static float configLengthMult;

		// Token: 0x0400002F RID: 47
		public static float configShockwaveRadius = 4f;

		// Token: 0x04000030 RID: 48
		public Color handColor;

		// Token: 0x04000031 RID: 49
		public Color lightColor;

		// Token: 0x04000032 RID: 50
		public bool showTrail;

		// Token: 0x04000033 RID: 51
		public float scale = 1f;

		// Token: 0x04000034 RID: 52
		public float distanceMult = 1f;

		// Token: 0x04000035 RID: 53
		public float lengthMult;

		// Token: 0x04000036 RID: 54
		public float shockwaveRadius = 4f;

		// Token: 0x04000037 RID: 55
		public Mode defaultMode;

		// Token: 0x04000038 RID: 56
		public HandController controller;

		// Token: 0x04000039 RID: 57
		private bool claw = true;

		// Token: 0x0400003A RID: 58
		public StateTracker state;
	}
}
