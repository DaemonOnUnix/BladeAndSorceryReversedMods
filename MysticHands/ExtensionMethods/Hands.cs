using System;
using System.Linq;
using ThunderRoad;
using UnityEngine;

namespace ExtensionMethods
{
	// Token: 0x02000008 RID: 8
	public static class Hands
	{
		// Token: 0x060000A6 RID: 166 RVA: 0x00005D58 File Offset: 0x00003F58
		public static bool Empty(this RagdollHand hand)
		{
			return !hand.caster.isFiring && !hand.isGrabbed && !hand.caster.isMerging && !Player.currentCreature.mana.mergeActive && hand.grabbedHandle == null && hand.caster.telekinesis.catchedHandle == null;
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00005DC4 File Offset: 0x00003FC4
		public static void ForBothHands(Action<RagdollHand> action)
		{
			action(Hands.Left);
			action(Hands.Right);
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00005DDF File Offset: 0x00003FDF
		public static bool Both(params Func<RagdollHand, bool>[] predicates)
		{
			return predicates.All((Func<RagdollHand, bool> pred) => pred(Hands.Left) && pred(Hands.Right));
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00005E06 File Offset: 0x00004006
		public static bool Either(params Func<RagdollHand, bool>[] predicates)
		{
			return predicates.Any((Func<RagdollHand, bool> pred) => pred(Hands.Left) && pred(Hands.Right));
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00005E2D File Offset: 0x0000402D
		public static bool Gripping(this RagdollHand hand)
		{
			return hand.IsGripping();
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00005E35 File Offset: 0x00004035
		public static Ray PointRay(this RagdollHand hand)
		{
			return new Ray(hand.IndexTip().position, hand.PointDir());
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00005E50 File Offset: 0x00004050
		public static bool Triggering(this RagdollHand hand)
		{
			bool? flag;
			if (hand == null)
			{
				flag = null;
			}
			else
			{
				PlayerHand playerHand = hand.playerHand;
				if (playerHand == null)
				{
					flag = null;
				}
				else
				{
					PlayerControl.Hand controlHand = playerHand.controlHand;
					flag = ((controlHand != null) ? new bool?(controlHand.usePressed) : null);
				}
			}
			bool? flag2 = flag;
			return flag2.GetValueOrDefault();
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00005EA8 File Offset: 0x000040A8
		public static bool AltFire(this RagdollHand hand)
		{
			bool? flag;
			if (hand == null)
			{
				flag = null;
			}
			else
			{
				PlayerHand playerHand = hand.playerHand;
				if (playerHand == null)
				{
					flag = null;
				}
				else
				{
					PlayerControl.Hand controlHand = playerHand.controlHand;
					flag = ((controlHand != null) ? new bool?(controlHand.alternateUsePressed) : null);
				}
			}
			bool? flag2 = flag;
			return flag2.GetValueOrDefault();
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00005EFE File Offset: 0x000040FE
		public static SpellCastCharge GetSpell(this RagdollHand hand)
		{
			return hand.caster.spellInstance as SpellCastCharge;
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00005F10 File Offset: 0x00004110
		public static bool Selected<T>(this RagdollHand hand) where T : SpellCastCharge
		{
			return hand.caster.spellInstance is T;
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00005F25 File Offset: 0x00004125
		public static bool Selected(this RagdollHand hand, string id)
		{
			SpellCastData spellInstance = hand.caster.spellInstance;
			return ((spellInstance != null) ? spellInstance.id : null) == id;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00005F44 File Offset: 0x00004144
		public static bool Casting<T>(this RagdollHand hand) where T : SpellCastCharge
		{
			return hand.caster.spellInstance is T && hand.caster.isFiring;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00005F66 File Offset: 0x00004166
		public static RagdollHand GetHand(Side side)
		{
			return Player.currentCreature.GetHand(side);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00005F73 File Offset: 0x00004173
		public static float Distance()
		{
			return Vector3.Distance(Hands.GetHand(1).Palm(), Hands.GetHand(0).Palm());
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00005F90 File Offset: 0x00004190
		public static Vector3 Midpoint()
		{
			return Vector3.Lerp(Hands.GetHand(1).Palm(), Hands.GetHand(0).Palm(), 0.5f);
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00005FB2 File Offset: 0x000041B2
		public static Vector3 AveragePoint()
		{
			return Vector3.Lerp(Hands.Left.PointDir(), Hands.Right.PointDir(), 0.5f);
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00005FD2 File Offset: 0x000041D2
		public static Vector3 AveragePalm()
		{
			return Vector3.Lerp(Hands.Left.PalmDir(), Hands.Right.PalmDir(), 0.5f);
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00005FF2 File Offset: 0x000041F2
		public static Vector3 AverageThumb()
		{
			return Vector3.Lerp(Hands.Left.ThumbDir(), Hands.Right.ThumbDir(), 0.5f);
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00006012 File Offset: 0x00004212
		public static Vector3 LeftToRight()
		{
			return Hands.Right.Palm() - Hands.Left.Palm();
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00006030 File Offset: 0x00004230
		public static bool All(this RagdollHand hand, params Func<RagdollHand, bool>[] preds)
		{
			return preds.All((Func<RagdollHand, bool> pred) => pred(hand));
		}

		// Token: 0x060000BA RID: 186 RVA: 0x0000605C File Offset: 0x0000425C
		public static bool Any(this RagdollHand hand, params Func<RagdollHand, bool>[] preds)
		{
			return preds.Any((Func<RagdollHand, bool> pred) => pred(hand));
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x060000BB RID: 187 RVA: 0x00006088 File Offset: 0x00004288
		public static RagdollHand Right
		{
			get
			{
				return Player.currentCreature.handRight;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x060000BC RID: 188 RVA: 0x00006094 File Offset: 0x00004294
		public static RagdollHand Left
		{
			get
			{
				return Player.currentCreature.handLeft;
			}
		}

		// Token: 0x060000BD RID: 189 RVA: 0x000060A0 File Offset: 0x000042A0
		public static bool FacingPos(this RagdollHand hand, Vector3 position, float angle = 50f)
		{
			return hand.FacingDir(position - hand.Palm(), angle);
		}

		// Token: 0x060000BE RID: 190 RVA: 0x000060B5 File Offset: 0x000042B5
		public static bool FacingDir(this RagdollHand hand, Vector3 direction, float angle = 50f)
		{
			return hand.PalmDir().IsFacing(direction, angle);
		}
	}
}
