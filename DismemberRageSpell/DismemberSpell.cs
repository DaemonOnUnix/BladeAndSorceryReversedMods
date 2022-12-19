using System;
using ThunderRoad;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace DismemberRageSpell
{
	// Token: 0x02000002 RID: 2
	internal class DismemberSpell : SpellCastData
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public override void Load(SpellCaster spellCaster, SpellData.Level level)
		{
			base.Load(spellCaster, level);
			try
			{
				if (spellCaster.ragdollHand.name.Contains("Left"))
				{
					Player.local.handRight.ragdollHand.caster.spellInstance = null;
				}
				else if (spellCaster.ragdollHand.name.Contains("Right"))
				{
					Player.local.handLeft.ragdollHand.caster.spellInstance = null;
				}
				VolumeProfile profile = CameraEffects.local.defaultPostProcessVolume.profile;
				if (RageLevelModule.local.enableRageDistortion)
				{
					ChromaticAberration chromaticAberration;
					if (!profile.TryGet<ChromaticAberration>(ref chromaticAberration))
					{
						chromaticAberration = profile.Add<ChromaticAberration>(false);
					}
					chromaticAberration.active = true;
					chromaticAberration.intensity.overrideState = true;
					chromaticAberration.intensity.Override(RageLevelModule.local.distortionAmount);
				}
				if (RageLevelModule.local.enableRageVision)
				{
					ColorAdjustments colorAdjustments;
					if (!profile.TryGet<ColorAdjustments>(ref colorAdjustments))
					{
						colorAdjustments = profile.Add<ColorAdjustments>(false);
					}
					colorAdjustments.active = true;
					colorAdjustments.colorFilter.overrideState = true;
					colorAdjustments.colorFilter.Override(RageLevelModule.local.new_filter_color);
				}
				RageLevelModule.local.ds_active = true;
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002194 File Offset: 0x00000394
		public override void Unload()
		{
			base.Unload();
			try
			{
				VolumeProfile profile = CameraEffects.local.defaultPostProcessVolume.profile;
				ChromaticAberration chromaticAberration;
				if (RageLevelModule.local.enableRageDistortion && profile.TryGet<ChromaticAberration>(ref chromaticAberration))
				{
					chromaticAberration.active = false;
				}
				ColorAdjustments colorAdjustments;
				if (RageLevelModule.local.enableRageVision && profile.TryGet<ColorAdjustments>(ref colorAdjustments))
				{
					colorAdjustments.colorFilter.Override(RageLevelModule.local.prev_filter_color);
				}
				RageLevelModule.local.ds_active = false;
			}
			catch
			{
			}
		}

		// Token: 0x04000001 RID: 1
		private const string handLeft = "Left";

		// Token: 0x04000002 RID: 2
		private const string handRight = "Right";
	}
}
