using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ThunderRoad;
using UnityEngine;

namespace ExtensionMethods
{
	// Token: 0x02000003 RID: 3
	internal static class ExtensionMethods
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static bool IsEmpty(this RagdollHand hand)
		{
			bool flag = hand.grabbedHandle != null || hand.caster.telekinesis.catchedHandle != null;
			return !flag;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002094 File Offset: 0x00000294
		public static IEnumerable<TOut> SelectNotNull<TIn, TOut>(this IEnumerable<TIn> enumerable, Func<TIn, TOut> func)
		{
			return enumerable.Where((TIn item) => func(item) != null).Select(func);
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000020CB File Offset: 0x000002CB
		public static IEnumerable<T> NotNull<T>(this IEnumerable<T> enumerable)
		{
			return enumerable.Where((T item) => item != null);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020F2 File Offset: 0x000002F2
		public static Vector3 Velocity(this RagdollHand hand)
		{
			return Player.local.transform.rotation * hand.playerHand.controlHand.GetHandVelocity();
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002118 File Offset: 0x00000318
		public static float Abs(this float num)
		{
			return Mathf.Abs(num);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002120 File Offset: 0x00000320
		public static bool MostlyX(this Vector3 vec)
		{
			return vec.x.Abs() > vec.y.Abs() && vec.x.Abs() > vec.z.Abs();
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002155 File Offset: 0x00000355
		public static bool MostlyY(this Vector3 vec)
		{
			return vec.y.Abs() > vec.x.Abs() && vec.y.Abs() > vec.z.Abs();
		}

		// Token: 0x06000008 RID: 8 RVA: 0x0000218A File Offset: 0x0000038A
		public static bool MostlyZ(this Vector3 vec)
		{
			return vec.z.Abs() > vec.x.Abs() && vec.z.Abs() > vec.y.Abs();
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000021C0 File Offset: 0x000003C0
		public static Direction GetDirection(this Vector3 vec)
		{
			bool flag = vec.MostlyX();
			Direction direction;
			if (flag)
			{
				direction = Direction.X;
			}
			else
			{
				bool flag2 = vec.MostlyY();
				if (flag2)
				{
					direction = Direction.Y;
				}
				else
				{
					direction = Direction.Z;
				}
			}
			return direction;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000021F4 File Offset: 0x000003F4
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

		// Token: 0x0600000B RID: 11 RVA: 0x00002260 File Offset: 0x00000460
		public static IEnumerable<Creature> SphereCastDeadCreatures(Vector3 origin, float maxRadius, Vector3 direction, float maxDistance)
		{
			return from creature in (from hit in Physics.SphereCastAll(origin, maxRadius, direction, maxDistance, -5, 1)
					where !hit.collider.isTrigger
					select hit).SelectNotNull((RaycastHit hit) => hit.transform.root.GetComponent<Creature>())
				where creature != Player.currentCreature && creature.state == 0
				select creature;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000022EA File Offset: 0x000004EA
		public static void Set<T>(this object source, string fieldName, T val)
		{
			source.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic).SetValue(source, val);
		}
	}
}
