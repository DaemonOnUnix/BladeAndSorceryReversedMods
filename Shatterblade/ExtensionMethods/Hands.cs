using System;
using System.Linq;
using ThunderRoad;
using UnityEngine;

namespace ExtensionMethods
{
	// Token: 0x02000009 RID: 9
	public static class Hands
	{
		// Token: 0x06000089 RID: 137 RVA: 0x00005350 File Offset: 0x00003550
		public static bool Empty(this RagdollHand hand)
		{
			return !hand.caster.isFiring && !hand.isGrabbed && !hand.caster.isMerging && !Player.currentCreature.mana.mergeActive && hand.grabbedHandle == null && hand.caster.telekinesis.catchedHandle == null;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000053BC File Offset: 0x000035BC
		public static void ForBothHands(Action<RagdollHand> action)
		{
			action(Hands.Left);
			action(Hands.Right);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x000053D7 File Offset: 0x000035D7
		public static bool Both(params Func<RagdollHand, bool>[] predicates)
		{
			return predicates.All((Func<RagdollHand, bool> pred) => pred(Hands.Left) && pred(Hands.Right));
		}

		// Token: 0x0600008C RID: 140 RVA: 0x000053FE File Offset: 0x000035FE
		public static bool Either(params Func<RagdollHand, bool>[] predicates)
		{
			return predicates.Any((Func<RagdollHand, bool> pred) => pred(Hands.Left) && pred(Hands.Right));
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00005425 File Offset: 0x00003625
		public static bool Gripping(this RagdollHand hand)
		{
			return hand.IsGripping();
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00005430 File Offset: 0x00003630
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

		// Token: 0x0600008F RID: 143 RVA: 0x00005486 File Offset: 0x00003686
		public static SpellCastCharge GetSpell(this RagdollHand hand)
		{
			return hand.caster.spellInstance as SpellCastCharge;
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00005498 File Offset: 0x00003698
		public static bool Selected<T>(this RagdollHand hand) where T : SpellCastCharge
		{
			return hand.caster.spellInstance is T;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x000054AD File Offset: 0x000036AD
		public static bool Selected(this RagdollHand hand, string id)
		{
			SpellCastData spellInstance = hand.caster.spellInstance;
			return ((spellInstance != null) ? spellInstance.id : null) == id;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x000054CC File Offset: 0x000036CC
		public static bool Casting<T>(this RagdollHand hand) where T : SpellCastCharge
		{
			return hand.caster.spellInstance is T && hand.caster.isFiring;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x000054EE File Offset: 0x000036EE
		public static RagdollHand GetHand(Side side)
		{
			return Player.currentCreature.GetHand(side);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x000054FB File Offset: 0x000036FB
		public static float Distance()
		{
			return Vector3.Distance(Hands.GetHand(1).Palm(), Hands.GetHand(0).Palm());
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00005518 File Offset: 0x00003718
		public static Vector3 Midpoint()
		{
			return Vector3.Lerp(Hands.GetHand(1).Palm(), Hands.GetHand(0).Palm(), 0.5f);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x0000553A File Offset: 0x0000373A
		public static Vector3 AveragePoint()
		{
			return Vector3.Lerp(Hands.Left.PointDir(), Hands.Right.PointDir(), 0.5f);
		}

		// Token: 0x06000097 RID: 151 RVA: 0x0000555A File Offset: 0x0000375A
		public static Vector3 AveragePalm()
		{
			return Vector3.Lerp(Hands.Left.PalmDir(), Hands.Right.PalmDir(), 0.5f);
		}

		// Token: 0x06000098 RID: 152 RVA: 0x0000557A File Offset: 0x0000377A
		public static Vector3 AverageThumb()
		{
			return Vector3.Lerp(Hands.Left.ThumbDir(), Hands.Right.ThumbDir(), 0.5f);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x0000559A File Offset: 0x0000379A
		public static Vector3 LeftToRight()
		{
			return Hands.Right.Palm() - Hands.Left.Palm();
		}

		// Token: 0x0600009A RID: 154 RVA: 0x000055B8 File Offset: 0x000037B8
		public static bool All(this RagdollHand hand, params Func<RagdollHand, bool>[] preds)
		{
			return preds.All((Func<RagdollHand, bool> pred) => pred(hand));
		}

		// Token: 0x0600009B RID: 155 RVA: 0x000055E4 File Offset: 0x000037E4
		public static bool Any(this RagdollHand hand, params Func<RagdollHand, bool>[] preds)
		{
			return preds.Any((Func<RagdollHand, bool> pred) => pred(hand));
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600009C RID: 156 RVA: 0x00005610 File Offset: 0x00003810
		public static RagdollHand Right
		{
			get
			{
				return Player.currentCreature.handRight;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600009D RID: 157 RVA: 0x0000561C File Offset: 0x0000381C
		public static RagdollHand Left
		{
			get
			{
				return Player.currentCreature.handLeft;
			}
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00005628 File Offset: 0x00003828
		public static bool FacingPos(this RagdollHand hand, Vector3 position, float angle = 50f)
		{
			return hand.FacingDir(position - hand.Palm(), angle);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x0000563D File Offset: 0x0000383D
		public static bool FacingDir(this RagdollHand hand, Vector3 direction, float angle = 50f)
		{
			return hand.PalmDir().IsFacing(direction, angle);
		}
	}
}
