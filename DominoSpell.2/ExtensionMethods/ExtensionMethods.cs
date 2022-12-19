﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ThunderRoad;
using UnityEngine;

namespace ExtensionMethods
{
	// Token: 0x02000005 RID: 5
	internal static class ExtensionMethods
	{
		// Token: 0x06000022 RID: 34 RVA: 0x0000382C File Offset: 0x00001A2C
		public static int Capacity(this Holder holder)
		{
			return holder.data.maxQuantity;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00003839 File Offset: 0x00001A39
		public static Vector3 LocalAngularVelocity(this RagdollHand hand)
		{
			return hand.transform.InverseTransformDirection(hand.rb.angularVelocity);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00003854 File Offset: 0x00001A54
		public static Task<TOutput> Then<TInput, TOutput>(this Task<TInput> task, Func<TInput, TOutput> func)
		{
			return task.ContinueWith<TOutput>((Task<TInput> input) => func(input.Result));
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00003888 File Offset: 0x00001A88
		public static Task Then(this Task task, Action<Task> func)
		{
			return task.ContinueWith(func);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000038A4 File Offset: 0x00001AA4
		public static Task Then<TInput>(this Task<TInput> task, Action<TInput> func)
		{
			return task.ContinueWith(delegate(Task<TInput> input)
			{
				func(input.Result);
			});
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000038D8 File Offset: 0x00001AD8
		public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
		{
			T t;
			if ((t = obj.GetComponent<T>()) == null)
			{
				t = obj.AddComponent<T>();
			}
			return t;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00003900 File Offset: 0x00001B00
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

		// Token: 0x06000029 RID: 41 RVA: 0x0000397C File Offset: 0x00001B7C
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

		// Token: 0x0600002A RID: 42 RVA: 0x000039FC File Offset: 0x00001BFC
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

		// Token: 0x0600002B RID: 43 RVA: 0x00003B2C File Offset: 0x00001D2C
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

		// Token: 0x0600002C RID: 44 RVA: 0x00003B82 File Offset: 0x00001D82
		public static void HapticTick(this RagdollHand hand, float intensity = 1f, float frequency = 10f)
		{
			PlayerControl.input.Haptic(hand.side, intensity, frequency);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00003B97 File Offset: 0x00001D97
		public static void PlayHapticClipOver(this RagdollHand hand, AnimationCurve curve, float duration)
		{
			hand.StartCoroutine(ExtensionMethods.HapticPlayer(hand, curve, duration));
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00003BA9 File Offset: 0x00001DA9
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

		// Token: 0x0600002F RID: 47 RVA: 0x00003BC8 File Offset: 0x00001DC8
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

		// Token: 0x06000030 RID: 48 RVA: 0x00003C14 File Offset: 0x00001E14
		public static IEnumerable<TOut> SelectNotNull<TIn, TOut>(this IEnumerable<TIn> enumerable, Func<TIn, TOut> func)
		{
			return enumerable.Where((TIn item) => func(item) != null).Select(func);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00003C4B File Offset: 0x00001E4B
		public static IEnumerable<T> NotNull<T>(this IEnumerable<T> enumerable)
		{
			return enumerable.Where((T item) => item != null);
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00003C74 File Offset: 0x00001E74
		public static Vector3 PosAboveBackOfHand(this RagdollHand hand)
		{
			return hand.transform.position - hand.transform.right * 0.1f + hand.transform.forward * 0.2f;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00003CC0 File Offset: 0x00001EC0
		public static Vector3 PalmDir(this RagdollHand hand)
		{
			return hand.transform.forward * -1f;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00003CD7 File Offset: 0x00001ED7
		public static Vector3 PointDir(this RagdollHand hand)
		{
			return -hand.transform.right;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00003CE9 File Offset: 0x00001EE9
		public static Vector3 ThumbDir(this RagdollHand hand)
		{
			return (hand.side == null) ? hand.transform.up : (-hand.transform.up);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00003D10 File Offset: 0x00001F10
		public static Transform IndexTip(this RagdollHand hand)
		{
			return hand.fingerIndex.distal.collider.transform;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00003D27 File Offset: 0x00001F27
		public static Vector3 Palm(this RagdollHand hand)
		{
			return hand.transform.position + hand.PointDir() * 0.1f;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00003D49 File Offset: 0x00001F49
		public static Vector3 Velocity(this RagdollHand hand)
		{
			return Player.local.transform.rotation * hand.playerHand.controlHand.GetHandVelocity();
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00003D6F File Offset: 0x00001F6F
		public static void ForBothColliderGroups(this CollisionInstance hit, Action<ColliderGroup> func)
		{
			func(hit.targetColliderGroup);
			func(hit.sourceColliderGroup);
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00003D8C File Offset: 0x00001F8C
		public static float NegPow(this float input, float power)
		{
			return Mathf.Pow(input, power) * (input / Mathf.Abs(input));
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00003D9E File Offset: 0x00001F9E
		public static float Pow(this float input, float power)
		{
			return Mathf.Pow(input, power);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00003DA7 File Offset: 0x00001FA7
		public static float Sqrt(this float input)
		{
			return Mathf.Sqrt(input);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00003DAF File Offset: 0x00001FAF
		public static float Clamp(this float input, float low, float high)
		{
			return Mathf.Clamp(input, low, high);
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00003DB9 File Offset: 0x00001FB9
		public static float Remap(this float input, float inLow, float inHigh, float outLow, float outHigh)
		{
			return (input - inLow) / (inHigh - inLow) * (outHigh - outLow) + outLow;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00003DC9 File Offset: 0x00001FC9
		public static float RemapClamp(this float input, float inLow, float inHigh, float outLow, float outHigh)
		{
			return (Mathf.Clamp(input, inLow, inHigh) - inLow) / (inHigh - inLow) * (outHigh - outLow) + outLow;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00003DE0 File Offset: 0x00001FE0
		public static float Remap01(this float input, float inLow, float inHigh)
		{
			return (input - inLow) / (inHigh - inLow);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00003DE9 File Offset: 0x00001FE9
		public static float RemapClamp01(this float input, float inLow, float inHigh)
		{
			return (Mathf.Clamp(input, inLow, inHigh) - inLow) / (inHigh - inLow);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00003DF9 File Offset: 0x00001FF9
		public static float Randomize(this float input, float range)
		{
			return input * Random.Range(1f - range, 1f + range);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00003E10 File Offset: 0x00002010
		public static float Curve(this float time, params float[] values)
		{
			AnimationCurve curve = new AnimationCurve();
			int i = 0;
			foreach (float value in values)
			{
				curve.AddKey((float)i / ((float)values.Length - 1f), value);
				i++;
			}
			return curve.Evaluate(time);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00003E68 File Offset: 0x00002068
		public static float MapOverCurve(this float time, params Tuple<float, float>[] points)
		{
			AnimationCurve curve = new AnimationCurve();
			foreach (Tuple<float, float> point in points)
			{
				curve.AddKey(new Keyframe(point.Item1, point.Item2));
			}
			return curve.Evaluate(time);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00003EB8 File Offset: 0x000020B8
		public static float MapOverCurve(this float time, params Tuple<float, float, float, float>[] points)
		{
			AnimationCurve curve = new AnimationCurve();
			foreach (Tuple<float, float, float, float> point in points)
			{
				curve.AddKey(new Keyframe(point.Item1, point.Item2, point.Item3, point.Item4));
			}
			return curve.Evaluate(time);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00003F14 File Offset: 0x00002114
		public static float SafetyClamp(this float num)
		{
			return Mathf.Clamp(num, -1000f, 1000f);
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00003F26 File Offset: 0x00002126
		public static float Abs(this float num)
		{
			return Mathf.Abs(num);
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00003F2E File Offset: 0x0000212E
		public static Vector3 SafetyClamp(this Vector3 vec)
		{
			return vec.normalized * vec.magnitude.SafetyClamp();
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00003F48 File Offset: 0x00002148
		public static bool MostlyX(this Vector3 vec)
		{
			return vec.x.Abs() > vec.y.Abs() && vec.x.Abs() > vec.z.Abs();
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00003F7D File Offset: 0x0000217D
		public static bool MostlyY(this Vector3 vec)
		{
			return vec.y.Abs() > vec.x.Abs() && vec.y.Abs() > vec.z.Abs();
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00003FB2 File Offset: 0x000021B2
		public static bool MostlyZ(this Vector3 vec)
		{
			return vec.z.Abs() > vec.x.Abs() && vec.z.Abs() > vec.y.Abs();
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00003FE7 File Offset: 0x000021E7
		public static RagdollPart GetPart(this Creature creature, RagdollPart.Type partType)
		{
			return creature.ragdoll.GetPart(partType);
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00003FF5 File Offset: 0x000021F5
		public static RagdollPart GetHead(this Creature creature)
		{
			return creature.ragdoll.headPart;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00004002 File Offset: 0x00002202
		public static RagdollPart GetTorso(this Creature creature)
		{
			return creature.GetPart(4);
		}

		// Token: 0x0600004F RID: 79 RVA: 0x0000400B File Offset: 0x0000220B
		public static Vector3 GetChest(this Creature creature)
		{
			return Vector3.Lerp(creature.GetTorso().transform.position, creature.GetHead().transform.position, 0.5f);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00004038 File Offset: 0x00002238
		public static float HandVelocityInLocalDirection(this RagdollHand hand, Vector3 direction)
		{
			return Vector3.Dot(hand.Velocity(), hand.transform.TransformDirection(direction));
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00004064 File Offset: 0x00002264
		public static float HandVelocityInDirection(this RagdollHand hand, Vector3 direction)
		{
			return Vector3.Dot(hand.Velocity(), direction);
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00004084 File Offset: 0x00002284
		public static Vector3 Rotated(this Vector3 vector, Quaternion rotation, Vector3 pivot = default(Vector3))
		{
			return rotation * (vector - pivot) + pivot;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x000040AC File Offset: 0x000022AC
		public static Side Other(this Side side)
		{
			return (side == 1) ? 0 : 1;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x000040C8 File Offset: 0x000022C8
		public static Vector3 Rotated(this Vector3 vector, Vector3 rotation, Vector3 pivot = default(Vector3))
		{
			return vector.Rotated(Quaternion.Euler(rotation), pivot);
		}

		// Token: 0x06000055 RID: 85 RVA: 0x000040E8 File Offset: 0x000022E8
		public static Vector3 Rotated(this Vector3 vector, float x, float y, float z, Vector3 pivot = default(Vector3))
		{
			return vector.Rotated(Quaternion.Euler(x, y, z), pivot);
		}

		// Token: 0x06000056 RID: 86 RVA: 0x0000410A File Offset: 0x0000230A
		public static bool IsFacing(this Vector3 source, Vector3 other, float angle = 50f)
		{
			return Vector3.Angle(source, other) < angle;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00004118 File Offset: 0x00002318
		public static void SetPosition(this EffectInstance instance, Vector3 position)
		{
			instance.effects.ForEach(delegate(Effect effect)
			{
				effect.transform.position = position;
			});
		}

		// Token: 0x06000058 RID: 88 RVA: 0x0000414C File Offset: 0x0000234C
		public static void SetRotation(this EffectInstance instance, Quaternion rotation)
		{
			instance.effects.ForEach(delegate(Effect effect)
			{
				effect.transform.rotation = rotation;
			});
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00004180 File Offset: 0x00002380
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

		// Token: 0x0600005A RID: 90 RVA: 0x000041F8 File Offset: 0x000023F8
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

		// Token: 0x0600005B RID: 91 RVA: 0x00004228 File Offset: 0x00002428
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

		// Token: 0x0600005C RID: 92 RVA: 0x00004258 File Offset: 0x00002458
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

		// Token: 0x0600005D RID: 93 RVA: 0x00004285 File Offset: 0x00002485
		public static IEnumerator RunAfterCoroutine(Func<IEnumerator> function, float delay)
		{
			yield return new WaitForSeconds(delay);
			yield return function();
			yield break;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x0000429B File Offset: 0x0000249B
		public static IEnumerator RunAfterCoroutine(Action action, float delay)
		{
			yield return new WaitForSeconds(delay);
			action();
			yield break;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x000042B1 File Offset: 0x000024B1
		public static IEnumerator RunNextFrameCoroutine(Action action)
		{
			yield return 0;
			action();
			yield break;
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000042C0 File Offset: 0x000024C0
		public static GameObject AddComponents<T>(this GameObject obj, Action<T> callback) where T : Component
		{
			callback(obj.AddComponent<T>());
			return obj;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x000042E0 File Offset: 0x000024E0
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

		// Token: 0x06000062 RID: 98 RVA: 0x00004340 File Offset: 0x00002540
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

		// Token: 0x06000063 RID: 99 RVA: 0x000043A4 File Offset: 0x000025A4
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

		// Token: 0x06000064 RID: 100 RVA: 0x000043DC File Offset: 0x000025DC
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

		// Token: 0x06000065 RID: 101 RVA: 0x00004418 File Offset: 0x00002618
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

		// Token: 0x06000066 RID: 102 RVA: 0x00004450 File Offset: 0x00002650
		public static void SetSpring(this ConfigurableJoint joint, float spring)
		{
			joint.xDrive.positionSpring = spring;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00004470 File Offset: 0x00002670
		public static void SetDamping(this ConfigurableJoint joint, float damper)
		{
			joint.xDrive.positionDamper = damper;
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00004490 File Offset: 0x00002690
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

		// Token: 0x06000069 RID: 105 RVA: 0x000044CC File Offset: 0x000026CC
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

		// Token: 0x0600006A RID: 106 RVA: 0x00004518 File Offset: 0x00002718
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

		// Token: 0x0600006B RID: 107 RVA: 0x0000454C File Offset: 0x0000274C
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

		// Token: 0x0600006C RID: 108 RVA: 0x000045A0 File Offset: 0x000027A0
		public static Vector3 MultiplyComponents(this Vector3 a, Vector3 b)
		{
			return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x000045D0 File Offset: 0x000027D0
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

		// Token: 0x0600006E RID: 110 RVA: 0x00004684 File Offset: 0x00002884
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

		// Token: 0x0600006F RID: 111 RVA: 0x0000471C File Offset: 0x0000291C
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

		// Token: 0x06000070 RID: 112 RVA: 0x00004830 File Offset: 0x00002A30
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

		// Token: 0x06000071 RID: 113 RVA: 0x00004BB0 File Offset: 0x00002DB0
		public static Quaternion GetFlyDirRefLocalRotation(this Item item)
		{
			return Quaternion.Inverse(item.transform.rotation) * item.flyDirRef.rotation;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00004BD2 File Offset: 0x00002DD2
		public static void AddModifier(this Rigidbody rb, object handler, int priority, float? gravity = null, float? drag = null, float? mass = null)
		{
			rb.gameObject.GetOrAddComponent<RigidbodyModifier>().AddModifier(handler, priority, gravity, drag, mass);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00004BED File Offset: 0x00002DED
		public static void RemoveModifier(this Rigidbody rb, object handler)
		{
			rb.gameObject.GetOrAddComponent<RigidbodyModifier>().RemoveModifier(handler);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00004C02 File Offset: 0x00002E02
		public static string ListString<T>(this IEnumerable<T> list)
		{
			return string.Join(", ", list.Select((T e) => e.ToString()));
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00004C33 File Offset: 0x00002E33
		public static T RandomChoice<T>(this IEnumerable<T> list)
		{
			return list.ElementAtOrDefault(Random.Range(0, list.Count<T>() - 1));
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00004C4C File Offset: 0x00002E4C
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

		// Token: 0x06000077 RID: 119 RVA: 0x00004CA0 File Offset: 0x00002EA0
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

		// Token: 0x06000078 RID: 120 RVA: 0x00004D34 File Offset: 0x00002F34
		public static void IgnoreCollider(this Ragdoll ragdoll, Collider collider, bool ignore = true)
		{
			foreach (RagdollPart part in ragdoll.parts)
			{
				part.IgnoreCollider(collider, ignore);
			}
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00004D90 File Offset: 0x00002F90
		public static void IgnoreCollider(this RagdollPart part, Collider collider, bool ignore = true)
		{
			foreach (Collider itemCollider in part.colliderGroup.colliders)
			{
				Physics.IgnoreCollision(collider, itemCollider, ignore);
			}
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00004DF0 File Offset: 0x00002FF0
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

		// Token: 0x0600007B RID: 123 RVA: 0x00004E8C File Offset: 0x0000308C
		public static bool Free(this Item item)
		{
			return item.mainHandler == null && item.holder == null && !item.isGripped && !item.isTelekinesisGrabbed;
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00004EC0 File Offset: 0x000030C0
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

		// Token: 0x0600007D RID: 125 RVA: 0x00004F35 File Offset: 0x00003135
		public static IEnumerable<string> Chop(this string str, int chunkSize)
		{
			for (int i = 0; i < str.Length; i += chunkSize)
			{
				yield return str.Substring(i, chunkSize);
			}
			yield break;
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00004F4C File Offset: 0x0000314C
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

		// Token: 0x0600007F RID: 127 RVA: 0x00004F98 File Offset: 0x00003198
		public static bool IsImportant(this RagdollPart part)
		{
			RagdollPart.Type type = part.type;
			return type == 2 || type == 32 || type == 64 || type == 512 || type == 1024;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00004FD4 File Offset: 0x000031D4
		public static T Clone<T>(this T obj)
		{
			MethodInfo inst = obj.GetType().GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic);
			return (T)((object)((inst != null) ? inst.Invoke(obj, null) : null));
		}
	}
}
