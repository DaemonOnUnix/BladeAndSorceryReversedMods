using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ThunderRoad;
using UnityEngine;

namespace ExtensionMethods
{
	// Token: 0x02000002 RID: 2
	internal static class ExtensionMethods
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static bool IsGrabbingCreature(RagdollHand hand)
		{
			bool flag = hand.grabbedHandle != null;
			bool flag3;
			if (flag)
			{
				bool flag2 = hand.grabbedHandle.GetComponentInParent<Creature>() != null && hand.grabbedHandle.item == null;
				flag3 = flag2;
			}
			else
			{
				flag3 = false;
			}
			return flag3;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020AC File Offset: 0x000002AC
		public static bool IsEmpty(RagdollHand hand)
		{
			bool flag = hand.caster.isFiring || hand.caster.isMerging || hand.creature.mana.mergeActive || hand.grabbedHandle != null || hand.caster.telekinesis.catchedHandle != null;
			return !flag;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x0000211C File Offset: 0x0000031C
		public static IEnumerable<TOut> SelectNotNull<TIn, TOut>(this IEnumerable<TIn> enumerable, Func<TIn, TOut> func)
		{
			return enumerable.Where((TIn item) => func(item) != null).Select(func);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002153 File Offset: 0x00000353
		public static IEnumerable<T> NotNull<T>(this IEnumerable<T> enumerable)
		{
			return enumerable.Where((T item) => item != null);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x0000217A File Offset: 0x0000037A
		public static Vector3 Velocity(this RagdollHand hand)
		{
			return Player.local.transform.rotation * hand.playerHand.controlHand.GetHandVelocity();
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000021A0 File Offset: 0x000003A0
		public static float Abs(this float num)
		{
			return Mathf.Abs(num);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000021A8 File Offset: 0x000003A8
		public static bool MostlyX(this Vector3 vec)
		{
			return vec.x.Abs() > vec.y.Abs() && vec.x.Abs() > vec.z.Abs();
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000021DD File Offset: 0x000003DD
		public static bool MostlyY(this Vector3 vec)
		{
			return vec.y.Abs() > vec.x.Abs() && vec.y.Abs() > vec.z.Abs();
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002212 File Offset: 0x00000412
		public static bool MostlyZ(this Vector3 vec)
		{
			return vec.z.Abs() > vec.x.Abs() && vec.z.Abs() > vec.y.Abs();
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002248 File Offset: 0x00000448
		public static IEnumerable<Creature> SphereCastCreature(Vector3 origin, float maxRadius, Vector3 direction, float maxDistance, bool npc = true, bool live = true)
		{
			return from creature in Physics.SphereCastAll(origin, maxRadius, direction, maxDistance, -5, 1).SelectNotNull(delegate(RaycastHit hit)
				{
					Rigidbody rigidbody = hit.rigidbody;
					return (rigidbody != null) ? rigidbody.gameObject.GetComponent<Creature>() : null;
				})
				where (!npc || creature != Player.currentCreature) && (!live || creature.state > 0)
				select creature;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000022B4 File Offset: 0x000004B4
		public static IEnumerable<Creature> SphereCastDeadCreatures(Vector3 origin, float maxRadius, Vector3 direction, float maxDistance)
		{
			return from creature in (from hit in Physics.SphereCastAll(origin, maxRadius, direction, maxDistance, -5, 1)
					where !hit.collider.isTrigger
					select hit).SelectNotNull((RaycastHit hit) => hit.transform.root.GetComponent<Creature>())
				where creature != Player.currentCreature && creature.state == 0
				select creature;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x0000233E File Offset: 0x0000053E
		public static void Set<T>(this object source, string fieldName, T val)
		{
			source.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic).SetValue(source, val);
		}
	}
}
