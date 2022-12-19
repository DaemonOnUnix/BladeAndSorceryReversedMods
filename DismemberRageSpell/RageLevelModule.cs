using System;
using System.Collections;
using System.Reflection;
using HarmonyLib;
using ThunderRoad;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace DismemberRageSpell
{
	// Token: 0x02000003 RID: 3
	public class RageLevelModule : LevelModule
	{
		// Token: 0x06000004 RID: 4 RVA: 0x00002228 File Offset: 0x00000428
		public override void Update()
		{
			base.Update();
			ColorAdjustments colorAdjustments;
			if (this.enableRageVision && this.ds_active && CameraEffects.local.defaultPostProcessVolume.profile.TryGet<ColorAdjustments>(ref colorAdjustments) && !colorAdjustments.colorFilter.value.Equals(this.new_filter_color))
			{
				colorAdjustments.colorFilter.Override(this.new_filter_color);
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x0000228F File Offset: 0x0000048F
		public override IEnumerator OnLoadCoroutine()
		{
			try
			{
				if (!Harmony.HasAnyPatches("Fisher.U11.DismemberRageSpell"))
				{
					Debug.Log("[Harmony][Fisher.U11.DismemberRageSpell] Loading Patches ... ");
					this.harmony = new Harmony("Fisher.U11.DismemberRageSpell");
					this.harmony.PatchAll(Assembly.GetExecutingAssembly());
					Debug.Log("[Harmony][Fisher.U11.DismemberRageSpell] Patches Loaded !!! ");
				}
			}
			catch (Exception ex)
			{
				Debug.Log("[Harmony][Fisher.U11.DismemberRageSpell] [Exception] ERROR with patches: ");
				Debug.Log(ex.StackTrace);
			}
			if (RageLevelModule.local == null)
			{
				RageLevelModule.local = this;
			}
			this.new_filter_color = new Color(this.visionColor[0] / 255f, this.visionColor[1] / 255f, this.visionColor[2] / 255f, this.visionAlpha);
			yield return base.OnLoadCoroutine();
			yield break;
		}

		// Token: 0x04000003 RID: 3
		public static RageLevelModule local;

		// Token: 0x04000004 RID: 4
		public float[] visionColor = new float[] { 255f, 255f, 255f };

		// Token: 0x04000005 RID: 5
		public float visionAlpha = 1f;

		// Token: 0x04000006 RID: 6
		public float distortionAmount = 5f;

		// Token: 0x04000007 RID: 7
		public bool enableRageVision = true;

		// Token: 0x04000008 RID: 8
		public bool enableRageDistortion = true;

		// Token: 0x04000009 RID: 9
		public bool ds_active;

		// Token: 0x0400000A RID: 10
		public Color prev_filter_color = Color.white;

		// Token: 0x0400000B RID: 11
		public Color new_filter_color = Color.white;

		// Token: 0x0400000C RID: 12
		private Harmony harmony;

		// Token: 0x0400000D RID: 13
		private const string fleshMatID = "Flesh";

		// Token: 0x0400000E RID: 14
		private const string activationSpellID = "SpellDismember";

		// Token: 0x0400000F RID: 15
		private const string harmonyPatchName = "Fisher.U11.DismemberRageSpell";

		// Token: 0x02000005 RID: 5
		[HarmonyPatch(typeof(Damager))]
		[HarmonyPatch("CheckSlice")]
		private static class DamagerCheckSlicePrefixPatch
		{
			// Token: 0x06000008 RID: 8 RVA: 0x0000230C File Offset: 0x0000050C
			[HarmonyPrefix]
			public static bool Prefix(ref bool __result, ref Damager __instance, Vector3 contactPoint, RagdollPart ragdollPart)
			{
				bool flag3;
				try
				{
					bool flag = Player.local.handLeft.ragdollHand.caster.spellInstance == null || !Player.local.handLeft.ragdollHand.caster.spellInstance.id.Contains("SpellDismember");
					bool flag2 = Player.local.handRight.ragdollHand.caster.spellInstance == null || !Player.local.handRight.ragdollHand.caster.spellInstance.id.Contains("SpellDismember");
					if (flag && flag2)
					{
						flag3 = true;
					}
					else
					{
						__result = true;
						flag3 = false;
					}
				}
				catch
				{
					flag3 = true;
				}
				return flag3;
			}
		}

		// Token: 0x02000006 RID: 6
		[HarmonyPatch(typeof(Damager))]
		[HarmonyPatch("CheckPenetration")]
		private static class DamagerCheckPenetrationPrefixPatch
		{
			// Token: 0x06000009 RID: 9 RVA: 0x000023D4 File Offset: 0x000005D4
			[HarmonyPrefix]
			public static bool Prefix(ref bool __result, ref Damager __instance, CollisionInstance collisionInstance)
			{
				bool flag3;
				try
				{
					bool flag = Player.local.handLeft.ragdollHand.caster.spellInstance == null || !Player.local.handLeft.ragdollHand.caster.spellInstance.id.Contains("SpellDismember");
					bool flag2 = Player.local.handRight.ragdollHand.caster.spellInstance == null || !Player.local.handRight.ragdollHand.caster.spellInstance.id.Contains("SpellDismember");
					if (flag && flag2)
					{
						flag3 = true;
					}
					else if (!collisionInstance.sourceMaterial.id.Equals("Flesh") || !collisionInstance.targetMaterial.id.Equals("Flesh"))
					{
						flag3 = true;
					}
					else if (((collisionInstance != null) ? collisionInstance.sourceColliderGroup : null) != null && ((collisionInstance != null) ? collisionInstance.targetColliderGroup : null) != null && (collisionInstance.targetColliderGroup.GetComponentInParent<Creature>() == Player.currentCreature || collisionInstance.sourceColliderGroup.GetComponentInParent<Creature>() == Player.currentCreature))
					{
						flag3 = true;
					}
					else
					{
						__result = __instance.data.penetrationAllowed;
						flag3 = false;
					}
				}
				catch
				{
					flag3 = true;
				}
				return flag3;
			}
		}
	}
}
