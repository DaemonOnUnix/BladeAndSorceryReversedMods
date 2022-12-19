using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ThunderRoad;
using UnityEngine;

namespace ExtensionMethods
{
	// Token: 0x02000007 RID: 7
	internal static class ExtensionMethods
	{
		// Token: 0x06000040 RID: 64 RVA: 0x0000437C File Offset: 0x0000257C
		public static int Capacity(this Holder holder)
		{
			return holder.data.maxQuantity;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00004389 File Offset: 0x00002589
		public static Vector3 LocalAngularVelocity(this RagdollHand hand)
		{
			return hand.transform.InverseTransformDirection(hand.rb.angularVelocity);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000043A4 File Offset: 0x000025A4
		public static Task<TOutput> Then<TInput, TOutput>(this Task<TInput> task, Func<TInput, TOutput> func)
		{
			return task.ContinueWith<TOutput>((Task<TInput> input) => func(input.Result));
		}

		// Token: 0x06000043 RID: 67 RVA: 0x000043D8 File Offset: 0x000025D8
		public static Task Then(this Task task, Action<Task> func)
		{
			return task.ContinueWith(func);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x000043F4 File Offset: 0x000025F4
		public static Task Then<TInput>(this Task<TInput> task, Action<TInput> func)
		{
			return task.ContinueWith(delegate(Task<TInput> input)
			{
				func(input.Result);
			});
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00004428 File Offset: 0x00002628
		public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
		{
			T t;
			if ((t = obj.GetComponent<T>()) == null)
			{
				t = obj.AddComponent<T>();
			}
			return t;
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00004450 File Offset: 0x00002650
		public static void Play(this WhooshPoint point)
		{
			object field = point.GetField("trigger");
			bool flag;
			if (field is WhooshPoint.Trigger)
			{
				WhooshPoint.Trigger trigger = (WhooshPoint.Trigger)field;
				if (trigger != 1)
				{
					flag = point.GetField("effectInstance") != null;
					goto IL_31;
				}
			}
			flag = false;
			IL_31:
			bool flag2 = flag;
			if (flag2)
			{
				EffectInstance effectInstance = point.GetField("effectInstance") as EffectInstance;
				if (effectInstance != null)
				{
					effectInstance.Play(0, false);
				}
			}
			point.SetField("effectActive", true);
			point.SetField("dampenedIntensity", 0);
		}

		// Token: 0x06000047 RID: 71 RVA: 0x000044CC File Offset: 0x000026CC
		public static void Stop(this WhooshPoint point)
		{
			object field = point.GetField("trigger");
			bool flag;
			if (field is WhooshPoint.Trigger)
			{
				WhooshPoint.Trigger trigger = (WhooshPoint.Trigger)field;
				if (trigger != 1)
				{
					flag = point.GetField("effectInstance") != null;
					goto IL_31;
				}
			}
			flag = false;
			IL_31:
			bool flag2 = flag;
			if (flag2)
			{
				EffectInstance effectInstance = point.GetField("effectInstance") as EffectInstance;
				if (effectInstance != null)
				{
					effectInstance.SetIntensity(0f);
				}
			}
			point.SetField("effectActive", false);
			point.SetField("dampenedIntensity", 0);
		}

		// Token: 0x06000048 RID: 72 RVA: 0x0000454C File Offset: 0x0000274C
		public static void PointItemFlyRefAtTarget(this Item item, Vector3 target, float lerpFactor, Vector3? upDir = null)
		{
			Vector3 up = upDir ?? Vector3.up;
			bool flag = item.flyDirRef;
			if (flag)
			{
				item.transform.rotation = Quaternion.Slerp(item.transform.rotation * item.flyDirRef.localRotation, Quaternion.LookRotation(target, up), lerpFactor) * Quaternion.Inverse(item.flyDirRef.localRotation);
			}
			else
			{
				bool flag2 = item.holderPoint;
				if (flag2)
				{
					item.transform.rotation = Quaternion.Slerp(item.transform.rotation * item.holderPoint.localRotation, Quaternion.LookRotation(target, up), lerpFactor) * Quaternion.Inverse(item.holderPoint.localRotation);
				}
				else
				{
					Quaternion pointDir = Quaternion.LookRotation(item.transform.up, up);
					item.transform.rotation = Quaternion.Slerp(item.transform.rotation * pointDir, Quaternion.LookRotation(target, up), lerpFactor) * Quaternion.Inverse(pointDir);
				}
			}
		}

		// Token: 0x06000049 RID: 73 RVA: 0x0000467C File Offset: 0x0000287C
		public static bool IsGripping(this RagdollHand hand)
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
					flag = ((controlHand != null) ? new bool?(controlHand.gripPressed) : null);
				}
			}
			bool? flag2 = flag;
			return flag2.GetValueOrDefault();
		}

		// Token: 0x0600004A RID: 74 RVA: 0x000046D4 File Offset: 0x000028D4
		public static void HapticTick(this RagdollHand hand, float intensity = 1f, float frequency = 10f, int count = 1)
		{
			PlayerControl.input.Haptic(hand.side, intensity, frequency);
			bool flag = count > 1;
			if (flag)
			{
				Action <>9__0;
				for (int i = 0; i < count - 1; i++)
				{
					MonoBehaviour local = PlayerControl.local;
					Action action;
					if ((action = <>9__0) == null)
					{
						action = (<>9__0 = delegate()
						{
							PlayerControl.input.Haptic(hand.side, intensity, frequency);
						});
					}
					local.RunAfter(action, 0.07f * (float)count);
				}
			}
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00004771 File Offset: 0x00002971
		public static void PlayHapticClipOver(this RagdollHand hand, AnimationCurve curve, float duration)
		{
			hand.StartCoroutine(ExtensionMethods.HapticPlayer(hand, curve, duration));
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00004783 File Offset: 0x00002983
		public static IEnumerator HapticPlayer(RagdollHand hand, AnimationCurve curve, float duration)
		{
			float time = Time.time;
			while (Time.time - time < duration)
			{
				hand.HapticTick(curve.Evaluate((Time.time - time) / duration), 10f);
				yield return 0;
			}
			yield break;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x000047A0 File Offset: 0x000029A0
		public static T MinBy<T>(this IEnumerable<T> enumerable, Func<T, IComparable> comparator)
		{
			bool flag = !enumerable.Any<T>();
			T t;
			if (flag)
			{
				t = default(T);
			}
			else
			{
				t = enumerable.Aggregate((T curMin, T x) => (curMin == null || comparator(x).CompareTo(comparator(curMin)) < 0) ? x : curMin);
			}
			return t;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x000047EC File Offset: 0x000029EC
		public static IEnumerable<TOut> SelectNotNull<TIn, TOut>(this IEnumerable<TIn> enumerable, Func<TIn, TOut> func)
		{
			return enumerable.Where((TIn item) => func(item) != null).Select(func);
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00004823 File Offset: 0x00002A23
		public static IEnumerable<T> NotNull<T>(this IEnumerable<T> enumerable)
		{
			return enumerable.Where((T item) => item != null);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x0000484C File Offset: 0x00002A4C
		public static Vector3 PosAboveBackOfHand(this RagdollHand hand)
		{
			return hand.transform.position - hand.transform.right * 0.1f + hand.transform.forward * 0.2f;
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00004898 File Offset: 0x00002A98
		public static Vector3 PalmDir(this RagdollHand hand)
		{
			return hand.transform.forward * -1f;
		}

		// Token: 0x06000052 RID: 82 RVA: 0x000048AF File Offset: 0x00002AAF
		public static Vector3 PointDir(this RagdollHand hand)
		{
			return -hand.transform.right;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x000048C1 File Offset: 0x00002AC1
		public static Vector3 ThumbDir(this RagdollHand hand)
		{
			return (hand.side == null) ? hand.transform.up : (-hand.transform.up);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x000048E8 File Offset: 0x00002AE8
		public static Transform IndexTip(this RagdollHand hand)
		{
			return hand.fingerIndex.distal.collider.transform;
		}

		// Token: 0x06000055 RID: 85 RVA: 0x000048FF File Offset: 0x00002AFF
		public static Vector3 Palm(this RagdollHand hand)
		{
			return hand.transform.position + hand.PointDir() * 0.1f;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00004924 File Offset: 0x00002B24
		public static Vector3 Velocity(this RagdollHand hand)
		{
			Vector3 vector;
			try
			{
				vector = Player.local.transform.rotation * hand.playerHand.controlHand.GetHandVelocity();
			}
			catch (NullReferenceException)
			{
				vector = Vector3.zero;
			}
			return vector;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00004978 File Offset: 0x00002B78
		public static void ForBothColliderGroups(this CollisionInstance hit, Action<ColliderGroup> func)
		{
			func(hit.targetColliderGroup);
			func(hit.sourceColliderGroup);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00004995 File Offset: 0x00002B95
		public static float NegPow(this float input, float power)
		{
			return Mathf.Pow(input, power) * (input / Mathf.Abs(input));
		}

		// Token: 0x06000059 RID: 89 RVA: 0x000049A7 File Offset: 0x00002BA7
		public static float Pow(this float input, float power)
		{
			return Mathf.Pow(input, power);
		}

		// Token: 0x0600005A RID: 90 RVA: 0x000049B0 File Offset: 0x00002BB0
		public static float Sqrt(this float input)
		{
			return Mathf.Sqrt(input);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x000049B8 File Offset: 0x00002BB8
		public static float Clamp(this float input, float low, float high)
		{
			return Mathf.Clamp(input, low, high);
		}

		// Token: 0x0600005C RID: 92 RVA: 0x000049C2 File Offset: 0x00002BC2
		public static float Sign(this float input)
		{
			return (float)((input < 0f) ? (-1) : ((input > 0f) ? 1 : 0));
		}

		// Token: 0x0600005D RID: 93 RVA: 0x000049DC File Offset: 0x00002BDC
		public static float Remap(this float input, float inLow, float inHigh, float outLow, float outHigh)
		{
			return (input - inLow) / (inHigh - inLow) * (outHigh - outLow) + outLow;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000049EC File Offset: 0x00002BEC
		public static float RemapClamp(this float input, float inLow, float inHigh, float outLow, float outHigh)
		{
			return (Mathf.Clamp(input, inLow, inHigh) - inLow) / (inHigh - inLow) * (outHigh - outLow) + outLow;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00004A03 File Offset: 0x00002C03
		public static float Remap01(this float input, float inLow, float inHigh)
		{
			return (input - inLow) / (inHigh - inLow);
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00004A0C File Offset: 0x00002C0C
		public static float RemapClamp01(this float input, float inLow, float inHigh)
		{
			return (Mathf.Clamp(input, inLow, inHigh) - inLow) / (inHigh - inLow);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00004A1C File Offset: 0x00002C1C
		public static float OneMinus(this float input)
		{
			return Mathf.Clamp01(1f - input);
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00004A2A File Offset: 0x00002C2A
		public static float Randomize(this float input, float range)
		{
			return input * Random.Range(1f - range, 1f + range);
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00004A44 File Offset: 0x00002C44
		public static float Curve(this float time, params float?[] values)
		{
			AnimationCurve curve = new AnimationCurve();
			int i = 0;
			foreach (float value in values)
			{
				float num;
				bool flag;
				if (value != null)
				{
					num = value.GetValueOrDefault();
					flag = true;
				}
				else
				{
					flag = false;
				}
				bool flag2 = flag;
				if (flag2)
				{
					curve.AddKey((float)i / ((float)values.Length - 1f), num);
				}
				i++;
			}
			return curve.Evaluate(time);
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00004ABC File Offset: 0x00002CBC
		public static float MapOverCurve(this float time, params Tuple<float, float>[] points)
		{
			AnimationCurve curve = new AnimationCurve();
			foreach (Tuple<float, float> point in points)
			{
				curve.AddKey(new Keyframe(point.Item1, point.Item2));
			}
			return curve.Evaluate(time);
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00004B0C File Offset: 0x00002D0C
		public static float MapOverCurve(this float time, params Tuple<float, float, float, float>[] points)
		{
			AnimationCurve curve = new AnimationCurve();
			foreach (Tuple<float, float, float, float> point in points)
			{
				curve.AddKey(new Keyframe(point.Item1, point.Item2, point.Item3, point.Item4));
			}
			return curve.Evaluate(time);
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00004B68 File Offset: 0x00002D68
		public static Vector3 BezierMap(this float time, Vector3 A, Vector3 B, Vector3 C, Vector3 D)
		{
			Vector3 Q = Vector3.Lerp(A, B, time);
			Vector3 R = Vector3.Lerp(B, C, time);
			Vector3 S = Vector3.Lerp(C, D, time);
			Vector3 P = Vector3.Lerp(Q, R, time);
			Vector3 T = Vector3.Lerp(R, S, time);
			return Vector3.Lerp(P, T, time);
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00004BB4 File Offset: 0x00002DB4
		public static float SafetyClamp(this float num)
		{
			return Mathf.Clamp(num, -1000f, 1000f);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00004BC6 File Offset: 0x00002DC6
		public static float Abs(this float num)
		{
			return Mathf.Abs(num);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00004BCE File Offset: 0x00002DCE
		public static Vector3 SafetyClamp(this Vector3 vec)
		{
			return vec.normalized * vec.magnitude.SafetyClamp();
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00004BE8 File Offset: 0x00002DE8
		public static Vector3 WithX(this Vector3 vec, float value)
		{
			return new Vector3(value, vec.y, vec.z);
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00004BFC File Offset: 0x00002DFC
		public static Vector3 WithY(this Vector3 vec, float value)
		{
			return new Vector3(vec.x, value, vec.z);
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00004C10 File Offset: 0x00002E10
		public static Vector3 WithZ(this Vector3 vec, float value)
		{
			return new Vector3(vec.x, vec.y, value);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00004C24 File Offset: 0x00002E24
		public static bool MostlyX(this Vector3 vec)
		{
			return vec.x.Abs() > vec.y.Abs() && vec.x.Abs() > vec.z.Abs();
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00004C59 File Offset: 0x00002E59
		public static bool MostlyY(this Vector3 vec)
		{
			return vec.y.Abs() > vec.x.Abs() && vec.y.Abs() > vec.z.Abs();
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00004C8E File Offset: 0x00002E8E
		public static bool MostlyZ(this Vector3 vec)
		{
			return vec.z.Abs() > vec.x.Abs() && vec.z.Abs() > vec.y.Abs();
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00004CC3 File Offset: 0x00002EC3
		public static RagdollPart GetPart(this Creature creature, RagdollPart.Type partType)
		{
			return creature.ragdoll.GetPart(partType);
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00004CD1 File Offset: 0x00002ED1
		public static RagdollPart GetHead(this Creature creature)
		{
			return creature.ragdoll.headPart;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00004CDE File Offset: 0x00002EDE
		public static RagdollPart GetTorso(this Creature creature)
		{
			return creature.GetPart(4);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00004CE7 File Offset: 0x00002EE7
		public static Vector3 GetChest(this Creature creature)
		{
			return Vector3.Lerp(creature.GetTorso().transform.position, creature.GetHead().transform.position, 0.5f);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00004D14 File Offset: 0x00002F14
		public static float HandVelocityInLocalDirection(this RagdollHand hand, Vector3 direction)
		{
			return Vector3.Dot(hand.Velocity(), hand.transform.TransformDirection(direction));
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00004D40 File Offset: 0x00002F40
		public static float HandVelocityInDirection(this RagdollHand hand, Vector3 direction)
		{
			return Vector3.Dot(hand.Velocity(), direction);
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00004D60 File Offset: 0x00002F60
		public static Vector3 Rotated(this Vector3 vector, Quaternion rotation, Vector3 pivot = default(Vector3))
		{
			return rotation * (vector - pivot) + pivot;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00004D88 File Offset: 0x00002F88
		public static Side Other(this Side side)
		{
			return (side == 1) ? 0 : 1;
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00004DA4 File Offset: 0x00002FA4
		public static Vector3 Rotated(this Vector3 vector, Vector3 rotation, Vector3 pivot = default(Vector3))
		{
			return vector.Rotated(Quaternion.Euler(rotation), pivot);
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00004DC4 File Offset: 0x00002FC4
		public static Vector3 Rotated(this Vector3 vector, float x, float y, float z, Vector3 pivot = default(Vector3))
		{
			return vector.Rotated(Quaternion.Euler(x, y, z), pivot);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00004DE6 File Offset: 0x00002FE6
		public static bool IsFacing(this Vector3 source, Vector3 other, float angle = 50f)
		{
			return Vector3.Angle(source, other) < angle;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00004DF4 File Offset: 0x00002FF4
		public static void SetPosition(this EffectInstance instance, Vector3 position)
		{
			instance.effects.ForEach(delegate(Effect effect)
			{
				effect.transform.position = position;
			});
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00004E28 File Offset: 0x00003028
		public static void SetRotation(this EffectInstance instance, Quaternion rotation)
		{
			instance.effects.ForEach(delegate(Effect effect)
			{
				effect.transform.rotation = rotation;
			});
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00004E5C File Offset: 0x0000305C
		public static void SetScale(this EffectInstance instance, Vector3 scale)
		{
			foreach (Effect effect in instance.effects)
			{
				EffectMesh mesh = effect as EffectMesh;
				bool flag = mesh != null;
				if (flag)
				{
					mesh.transform.localScale = scale;
					mesh.meshSize = scale;
				}
			}
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00004ED4 File Offset: 0x000030D4
		public static Coroutine RunCoroutine(this MonoBehaviour mono, Func<IEnumerator> function, float delay = 0f)
		{
			bool isActiveAndEnabled = mono.isActiveAndEnabled;
			Coroutine coroutine;
			if (isActiveAndEnabled)
			{
				coroutine = mono.StartCoroutine(ExtensionMethods.RunAfterCoroutine(function, delay));
			}
			else
			{
				coroutine = null;
			}
			return coroutine;
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00004F04 File Offset: 0x00003104
		public static Coroutine RunAfter(this MonoBehaviour mono, Action action, float delay = 0f)
		{
			bool isActiveAndEnabled = mono.isActiveAndEnabled;
			Coroutine coroutine;
			if (isActiveAndEnabled)
			{
				coroutine = mono.StartCoroutine(ExtensionMethods.RunAfterCoroutine(action, delay));
			}
			else
			{
				coroutine = null;
			}
			return coroutine;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00004F34 File Offset: 0x00003134
		public static Coroutine RunNextFrame(this MonoBehaviour mono, Action action)
		{
			bool isActiveAndEnabled = mono.isActiveAndEnabled;
			Coroutine coroutine;
			if (isActiveAndEnabled)
			{
				coroutine = mono.StartCoroutine(ExtensionMethods.RunNextFrameCoroutine(action));
			}
			else
			{
				coroutine = null;
			}
			return coroutine;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00004F61 File Offset: 0x00003161
		public static IEnumerator RunAfterCoroutine(Func<IEnumerator> function, float delay)
		{
			yield return new WaitForSeconds(delay);
			yield return function();
			yield break;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00004F77 File Offset: 0x00003177
		public static IEnumerator RunAfterCoroutine(Action action, float delay)
		{
			yield return new WaitForSeconds(delay);
			action();
			yield break;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00004F8D File Offset: 0x0000318D
		public static IEnumerator RunNextFrameCoroutine(Action action)
		{
			yield return 0;
			action();
			yield break;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00004F9C File Offset: 0x0000319C
		public static GameObject AddComponents<T>(this GameObject obj, Action<T> callback) where T : Component
		{
			callback(obj.AddComponent<T>());
			return obj;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00004FBC File Offset: 0x000031BC
		public static RagdollHand.Finger GetFinger(this RagdollHand hand, Finger finger)
		{
			RagdollHand.Finger finger2;
			switch (finger)
			{
			case 0:
				finger2 = hand.fingerThumb;
				break;
			case 1:
				finger2 = hand.fingerIndex;
				break;
			case 2:
				finger2 = hand.fingerMiddle;
				break;
			case 3:
				finger2 = hand.fingerRing;
				break;
			case 4:
				finger2 = hand.fingerLittle;
				break;
			default:
				finger2 = null;
				break;
			}
			return finger2;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x0000501C File Offset: 0x0000321C
		public static Transform GetFingerPart(this RagdollHand.Finger finger, FingerPart part)
		{
			Transform transform;
			switch (part)
			{
			case FingerPart.Proximal:
				transform = finger.proximal.collider.transform;
				break;
			case FingerPart.Intermediate:
				transform = finger.intermediate.collider.transform;
				break;
			case FingerPart.Distal:
				transform = finger.distal.collider.transform;
				break;
			default:
				transform = null;
				break;
			}
			return transform;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00005080 File Offset: 0x00003280
		public static object Call(this object o, string methodName, params object[] args)
		{
			MethodInfo mi = o.GetType().GetMethod(methodName, BindingFlags.Instance);
			bool flag = mi != null;
			object obj;
			if (flag)
			{
				obj = mi.Invoke(o, args);
			}
			else
			{
				obj = null;
			}
			return obj;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000050B8 File Offset: 0x000032B8
		public static object CallPrivate(this object o, string methodName, params object[] args)
		{
			MethodInfo mi = o.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
			bool flag = mi != null;
			object obj;
			if (flag)
			{
				obj = mi.Invoke(o, args);
			}
			else
			{
				obj = null;
			}
			return obj;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x000050F4 File Offset: 0x000032F4
		public static object GetField(this object instance, string fieldName)
		{
			bool flag = instance == null;
			object obj;
			if (flag)
			{
				obj = null;
			}
			else
			{
				BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
				FieldInfo field = instance.GetType().GetField(fieldName, bindFlags);
				obj = field.GetValue(instance);
			}
			return obj;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x0000512C File Offset: 0x0000332C
		public static void SetSpring(this ConfigurableJoint joint, float spring)
		{
			bool flag = joint != null;
			if (!flag)
			{
				JointDrive drive = joint.xDrive;
				drive.positionSpring = spring;
				joint.xDrive = drive;
				joint.yDrive = drive;
				joint.zDrive = drive;
			}
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00005170 File Offset: 0x00003370
		public static void SetDamping(this ConfigurableJoint joint, float damper)
		{
			bool flag = joint != null;
			if (!flag)
			{
				JointDrive drive = joint.xDrive;
				drive.positionDamper = damper;
				joint.xDrive = drive;
				joint.yDrive = drive;
				joint.zDrive = drive;
			}
		}

		// Token: 0x0600008C RID: 140 RVA: 0x000051B4 File Offset: 0x000033B4
		public static float GetMassModifier(this Rigidbody rb)
		{
			bool flag = rb.mass < 1f;
			float num;
			if (flag)
			{
				num = rb.mass * 3f;
			}
			else
			{
				num = rb.mass;
			}
			return num;
		}

		// Token: 0x0600008D RID: 141 RVA: 0x000051F0 File Offset: 0x000033F0
		public static float GetMassModifier(this Item item)
		{
			bool flag = item.rb.mass < 1f;
			float num;
			if (flag)
			{
				num = item.rb.mass * 3f;
			}
			else
			{
				num = item.rb.mass;
			}
			return num;
		}

		// Token: 0x0600008E RID: 142 RVA: 0x0000523C File Offset: 0x0000343C
		public static Item UnSnapOne(this Holder holder, bool silent)
		{
			Item obj = holder.items.LastOrDefault<Item>();
			bool flag = obj;
			if (flag)
			{
				holder.UnSnap(obj, silent, true);
			}
			return obj;
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00005270 File Offset: 0x00003470
		public static Vector3 GetScaleRelativeTo(this Transform transform, Transform target)
		{
			Vector3 output = Vector3.one;
			Transform parent = transform;
			while (parent.parent != target && parent.parent != null)
			{
				output = output.MultiplyComponents(parent.localScale);
				parent = parent.parent;
			}
			return output;
		}

		// Token: 0x06000090 RID: 144 RVA: 0x000052C4 File Offset: 0x000034C4
		public static Vector3 MultiplyComponents(this Vector3 a, Vector3 b)
		{
			return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x000052F4 File Offset: 0x000034F4
		public static float GetRadius(this Item item)
		{
			Item item2 = item;
			bool flag;
			if (item2 == null)
			{
				flag = false;
			}
			else
			{
				List<Renderer> renderers = item2.renderers;
				bool? flag2 = ((renderers != null) ? new bool?(renderers.Any<Renderer>()) : null);
				bool flag3 = true;
				flag = (flag2.GetValueOrDefault() == flag3) & (flag2 != null);
			}
			float num;
			if (!flag)
			{
				num = 0.5f;
			}
			else
			{
				num = item.renderers.Select((Renderer renderer) => renderer.gameObject.GetComponent<MeshFilter>()).Max((MeshFilter meshFilter) => meshFilter.transform.GetScaleRelativeTo(item.transform).MultiplyComponents(meshFilter.transform.position - item.transform.position + meshFilter.mesh.bounds.extents).magnitude).Clamp(0f, 1f);
			}
			return num;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x000053A8 File Offset: 0x000035A8
		public static void Depenetrate(this Item item)
		{
			foreach (CollisionHandler handler in item.collisionHandlers)
			{
				foreach (Damager damager in handler.damagers)
				{
					damager.UnPenetrateAll();
				}
			}
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00005440 File Offset: 0x00003640
		public static object GetVFXProperty(this EffectInstance effect, string name)
		{
			foreach (Effect fx in effect.effects)
			{
				EffectVfx vfx = fx as EffectVfx;
				bool flag = vfx != null;
				if (flag)
				{
					bool flag2 = vfx.vfx.HasFloat(name);
					if (flag2)
					{
						return vfx.vfx.GetFloat(name);
					}
					bool flag3 = vfx.vfx.HasVector3(name);
					if (flag3)
					{
						return vfx.vfx.GetVector3(name);
					}
					bool flag4 = vfx.vfx.HasBool(name);
					if (flag4)
					{
						return vfx.vfx.GetBool(name);
					}
					bool flag5 = vfx.vfx.HasInt(name);
					if (flag5)
					{
						return vfx.vfx.GetInt(name);
					}
				}
			}
			return null;
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00005554 File Offset: 0x00003754
		public static void SetVFXProperty<T>(this EffectInstance effect, string name, T data)
		{
			bool flag = effect == null;
			if (!flag)
			{
				Vector3 vec3;
				bool flag2;
				if (data is Vector3)
				{
					vec3 = data as Vector3;
					flag2 = true;
				}
				else
				{
					flag2 = false;
				}
				bool flag3 = flag2;
				if (flag3)
				{
					IEnumerable<Effect> effects = effect.effects;
					Func<Effect, bool> <>9__0;
					Func<Effect, bool> func;
					if ((func = <>9__0) == null)
					{
						func = (<>9__0 = delegate(Effect fx)
						{
							EffectVfx vfx = fx as EffectVfx;
							return vfx != null && vfx.vfx.HasVector3(name);
						});
					}
					foreach (Effect effect2 in effects.Where(func))
					{
						EffectVfx fx6 = (EffectVfx)effect2;
						fx6.vfx.SetVector3(name, vec3);
					}
				}
				else
				{
					float flt;
					bool flag4;
					if (data is float)
					{
						flt = data as float;
						flag4 = true;
					}
					else
					{
						flag4 = false;
					}
					bool flag5 = flag4;
					if (flag5)
					{
						IEnumerable<Effect> effects2 = effect.effects;
						Func<Effect, bool> <>9__1;
						Func<Effect, bool> func2;
						if ((func2 = <>9__1) == null)
						{
							func2 = (<>9__1 = delegate(Effect fx)
							{
								EffectVfx vfx = fx as EffectVfx;
								return vfx != null && vfx.vfx.HasFloat(name);
							});
						}
						foreach (Effect effect3 in effects2.Where(func2))
						{
							EffectVfx fx2 = (EffectVfx)effect3;
							fx2.vfx.SetFloat(name, flt);
						}
					}
					else
					{
						int integer;
						bool flag6;
						if (data is int)
						{
							integer = data as int;
							flag6 = true;
						}
						else
						{
							flag6 = false;
						}
						bool flag7 = flag6;
						if (flag7)
						{
							IEnumerable<Effect> effects3 = effect.effects;
							Func<Effect, bool> <>9__2;
							Func<Effect, bool> func3;
							if ((func3 = <>9__2) == null)
							{
								func3 = (<>9__2 = delegate(Effect fx)
								{
									EffectVfx vfx = fx as EffectVfx;
									return vfx != null && vfx.vfx.HasInt(name);
								});
							}
							foreach (Effect effect4 in effects3.Where(func3))
							{
								EffectVfx fx3 = (EffectVfx)effect4;
								fx3.vfx.SetInt(name, integer);
							}
						}
						else
						{
							bool boolean;
							bool flag8;
							if (data is bool)
							{
								boolean = data as bool;
								flag8 = true;
							}
							else
							{
								flag8 = false;
							}
							bool flag9 = flag8;
							if (flag9)
							{
								IEnumerable<Effect> effects4 = effect.effects;
								Func<Effect, bool> <>9__3;
								Func<Effect, bool> func4;
								if ((func4 = <>9__3) == null)
								{
									func4 = (<>9__3 = delegate(Effect fx)
									{
										EffectVfx vfx = fx as EffectVfx;
										return vfx != null && vfx.vfx.HasBool(name);
									});
								}
								foreach (Effect effect5 in effects4.Where(func4))
								{
									EffectVfx fx4 = (EffectVfx)effect5;
									fx4.vfx.SetBool(name, boolean);
								}
							}
							else
							{
								Texture texture = data as Texture;
								bool flag10 = texture != null;
								if (flag10)
								{
									IEnumerable<Effect> effects5 = effect.effects;
									Func<Effect, bool> <>9__4;
									Func<Effect, bool> func5;
									if ((func5 = <>9__4) == null)
									{
										func5 = (<>9__4 = delegate(Effect fx)
										{
											EffectVfx vfx = fx as EffectVfx;
											return vfx != null && vfx.vfx.HasTexture(name);
										});
									}
									foreach (Effect effect6 in effects5.Where(func5))
									{
										EffectVfx fx5 = (EffectVfx)effect6;
										fx5.vfx.SetTexture(name, texture);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000095 RID: 149 RVA: 0x000058D4 File Offset: 0x00003AD4
		public static Quaternion GetFlyDirRefLocalRotation(this Item item)
		{
			return Quaternion.Inverse(item.transform.rotation) * item.flyDirRef.rotation;
		}

		// Token: 0x06000096 RID: 150 RVA: 0x000058F6 File Offset: 0x00003AF6
		public static void AddModifier(this Rigidbody rb, object handler, int priority, float? gravity = null, float? drag = null, float? mass = null)
		{
			rb.gameObject.GetOrAddComponent<RigidbodyModifier>().AddModifier(handler, priority, gravity, drag, mass);
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00005911 File Offset: 0x00003B11
		public static void RemoveModifier(this Rigidbody rb, object handler)
		{
			rb.gameObject.GetOrAddComponent<RigidbodyModifier>().RemoveModifier(handler);
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00005926 File Offset: 0x00003B26
		public static string ListString<T>(this IEnumerable<T> list)
		{
			return string.Join(", ", list.Select((T e) => e.ToString()));
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00005957 File Offset: 0x00003B57
		public static T RandomChoice<T>(this IEnumerable<T> list)
		{
			return list.ElementAtOrDefault(Random.Range(0, list.Count<T>() - 1));
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00005970 File Offset: 0x00003B70
		public static int AffectedHandlers(this Creature creature, IEnumerable<CollisionHandler> handlers)
		{
			int num;
			if (creature == null)
			{
				num = 1;
			}
			else
			{
				num = creature.ragdoll.parts.SelectNotNull((RagdollPart part) => part.collisionHandler).Intersect(handlers).Count<CollisionHandler>();
			}
			return Mathf.Max(num, 1);
		}

		// Token: 0x0600009B RID: 155 RVA: 0x000059C4 File Offset: 0x00003BC4
		public static int AffectedHandlers(this CollisionHandler handler, IEnumerable<CollisionHandler> handlers)
		{
			RagdollPart ragdollPart = handler.ragdollPart;
			int? num;
			if (ragdollPart == null)
			{
				num = null;
			}
			else
			{
				Creature creature = ragdollPart.ragdoll.creature;
				if (creature == null)
				{
					num = null;
				}
				else
				{
					num = new int?(creature.ragdoll.parts.SelectNotNull((RagdollPart part) => part.collisionHandler).Intersect(handlers).Count<CollisionHandler>());
				}
			}
			return Mathf.Max(num ?? 1, 1);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00005A58 File Offset: 0x00003C58
		public static void IgnoreCollider(this Ragdoll ragdoll, Collider collider, bool ignore = true)
		{
			foreach (RagdollPart part in ragdoll.parts)
			{
				part.IgnoreCollider(collider, ignore);
			}
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00005AB4 File Offset: 0x00003CB4
		public static void IgnoreCollider(this RagdollPart part, Collider collider, bool ignore = true)
		{
			foreach (Collider itemCollider in part.colliderGroup.colliders)
			{
				Physics.IgnoreCollision(collider, itemCollider, ignore);
			}
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00005B14 File Offset: 0x00003D14
		public static bool Active(this Creature creature)
		{
			return !creature.isKilled && !creature.isCulled;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00005B2C File Offset: 0x00003D2C
		public static void IgnoreCollider(this Item item, Collider collider, bool ignore)
		{
			foreach (ColliderGroup cg in item.colliderGroups)
			{
				foreach (Collider itemCollider in cg.colliders)
				{
					Physics.IgnoreCollision(collider, itemCollider, ignore);
				}
			}
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00005BC8 File Offset: 0x00003DC8
		public static bool Free(this Item item)
		{
			return item.mainHandler == null && item.holder == null && !item.isGripped && !item.isTelekinesisGrabbed;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00005BFC File Offset: 0x00003DFC
		public static void SafeDespawn(this Item item)
		{
			item.handlers.ToList<RagdollHand>().ForEach(delegate(RagdollHand handler)
			{
				handler.UnGrab(false);
			});
			item.handles.ToList<Handle>().ForEach(delegate(Handle handle)
			{
				handle.SetTouch(false);
				handle.SetTelekinesis(false);
			});
			item.Despawn();
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00005C71 File Offset: 0x00003E71
		public static IEnumerable<string> Chop(this string str, int chunkSize)
		{
			for (int i = 0; i < str.Length; i += chunkSize)
			{
				yield return str.Substring(i, chunkSize);
			}
			yield break;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00005C88 File Offset: 0x00003E88
		public static bool IsPlayer(this RagdollPart part)
		{
			bool flag;
			if (part == null)
			{
				flag = false;
			}
			else
			{
				Ragdoll ragdoll = part.ragdoll;
				bool? flag2 = ((ragdoll != null) ? new bool?(ragdoll.creature.isPlayer) : null);
				bool flag3 = true;
				flag = (flag2.GetValueOrDefault() == flag3) & (flag2 != null);
			}
			return flag;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00005CD4 File Offset: 0x00003ED4
		public static bool IsImportant(this RagdollPart part)
		{
			RagdollPart.Type type = part.type;
			return type == 1 || type == 4 || type == 32 || type == 64 || type == 512 || type == 1024;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00005D14 File Offset: 0x00003F14
		public static T Clone<T>(this T obj)
		{
			MethodInfo inst = obj.GetType().GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic);
			return (T)((object)((inst != null) ? inst.Invoke(obj, null) : null));
		}
	}
}
