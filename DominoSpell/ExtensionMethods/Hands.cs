using System;
using System.Linq;
using ThunderRoad;
using UnityEngine;

namespace ExtensionMethods
{
	// Token: 0x02000006 RID: 6
	public static class Hands
	{
		// Token: 0x06000081 RID: 129 RVA: 0x00005018 File Offset: 0x00003218
		public static bool Empty(this RagdollHand hand)
		{
			return !hand.caster.isFiring && !hand.isGrabbed && !hand.caster.isMerging && !Player.currentCreature.mana.mergeActive && hand.grabbedHandle == null && hand.caster.telekinesis.catchedHandle == null;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00005084 File Offset: 0x00003284
		public static void ForBothHands(Action<RagdollHand> action)
		{
			action(Hands.Left);
			action(Hands.Right);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x0000509F File Offset: 0x0000329F
		public static bool Both(params Func<RagdollHand, bool>[] predicates)
		{
			return predicates.All((Func<RagdollHand, bool> pred) => pred(Hands.Left) && pred(Hands.Right));
		}

		// Token: 0x06000084 RID: 132 RVA: 0x000050C6 File Offset: 0x000032C6
		public static bool Either(params Func<RagdollHand, bool>[] predicates)
		{
			return predicates.Any((Func<RagdollHand, bool> pred) => pred(Hands.Left) && pred(Hands.Right));
		}

		// Token: 0x06000085 RID: 133 RVA: 0x000050ED File Offset: 0x000032ED
		public static bool Gripping(this RagdollHand hand)
		{
			return hand.IsGripping();
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000050F8 File Offset: 0x000032F8
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

		// Token: 0x06000087 RID: 135 RVA: 0x0000514E File Offset: 0x0000334E
		public static SpellCastCharge GetSpell(this RagdollHand hand)
		{
			return hand.caster.spellInstance as SpellCastCharge;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00005160 File Offset: 0x00003360
		public static bool Selected<T>(this RagdollHand hand) where T : SpellCastCharge
		{
			return hand.caster.spellInstance is T;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00005175 File Offset: 0x00003375
		public static bool Selected(this RagdollHand hand, string id)
		{
			SpellCastData spellInstance = hand.caster.spellInstance;
			return ((spellInstance != null) ? spellInstance.id : null) == id;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00005194 File Offset: 0x00003394
		public static bool Casting<T>(this RagdollHand hand) where T : SpellCastCharge
		{
			return hand.caster.spellInstance is T && hand.caster.isFiring;
		}

		// Token: 0x0600008B RID: 139 RVA: 0x000051B6 File Offset: 0x000033B6
		public static RagdollHand GetHand(Side side)
		{
			return Player.currentCreature.GetHand(side);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x000051C3 File Offset: 0x000033C3
		public static float Distance()
		{
			return Vector3.Distance(Hands.GetHand(1).Palm(), Hands.GetHand(0).Palm());
		}

		// Token: 0x0600008D RID: 141 RVA: 0x000051E0 File Offset: 0x000033E0
		public static Vector3 Midpoint()
		{
			return Vector3.Lerp(Hands.GetHand(1).Palm(), Hands.GetHand(0).Palm(), 0.5f);
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00005202 File Offset: 0x00003402
		public static Vector3 AveragePoint()
		{
			return Vector3.Lerp(Hands.Left.PointDir(), Hands.Right.PointDir(), 0.5f);
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00005222 File Offset: 0x00003422
		public static Vector3 AveragePalm()
		{
			return Vector3.Lerp(Hands.Left.PalmDir(), Hands.Right.PalmDir(), 0.5f);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00005242 File Offset: 0x00003442
		public static Vector3 AverageThumb()
		{
			return Vector3.Lerp(Hands.Left.ThumbDir(), Hands.Right.ThumbDir(), 0.5f);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00005262 File Offset: 0x00003462
		public static Vector3 LeftToRight()
		{
			return Hands.Right.Palm() - Hands.Left.Palm();
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00005280 File Offset: 0x00003480
		public static bool All(this RagdollHand hand, params Func<RagdollHand, bool>[] preds)
		{
			return preds.All((Func<RagdollHand, bool> pred) => pred(hand));
		}

		// Token: 0x06000093 RID: 147 RVA: 0x000052AC File Offset: 0x000034AC
		public static bool Any(this RagdollHand hand, params Func<RagdollHand, bool>[] preds)
		{
			return preds.Any((Func<RagdollHand, bool> pred) => pred(hand));
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000094 RID: 148 RVA: 0x000052D8 File Offset: 0x000034D8
		public static RagdollHand Right
		{
			get
			{
				return Player.currentCreature.handRight;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000095 RID: 149 RVA: 0x000052E4 File Offset: 0x000034E4
		public static RagdollHand Left
		{
			get
			{
				return Player.currentCreature.handLeft;
			}
		}

		// Token: 0x06000096 RID: 150 RVA: 0x000052F0 File Offset: 0x000034F0
		public static bool FacingPos(this RagdollHand hand, Vector3 position, float angle = 50f)
		{
			return hand.FacingDir(position - hand.Palm(), angle);
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00005305 File Offset: 0x00003505
		public static bool FacingDir(this RagdollHand hand, Vector3 direction, float angle = 50f)
		{
			return hand.PalmDir().IsFacing(direction, angle);
		}
	}
}
