using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ThunderRoad;
using UnityEngine;

// Token: 0x02000002 RID: 2
public static class Snippet
{
	// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	public static float NegPow(this float input, float power)
	{
		return Mathf.Pow(input, power) * (input / Mathf.Abs(input));
	}

	// Token: 0x06000002 RID: 2 RVA: 0x00002062 File Offset: 0x00000262
	public static float Pow(this float input, float power)
	{
		return Mathf.Pow(input, power);
	}

	// Token: 0x06000003 RID: 3 RVA: 0x0000206B File Offset: 0x0000026B
	public static float Sqrt(this float input)
	{
		return Mathf.Sqrt(input);
	}

	// Token: 0x06000004 RID: 4 RVA: 0x00002073 File Offset: 0x00000273
	public static float Clamp(this float input, float low, float high)
	{
		return Mathf.Clamp(input, low, high);
	}

	// Token: 0x06000005 RID: 5 RVA: 0x0000207D File Offset: 0x0000027D
	public static float Remap(this float input, float inLow, float inHigh, float outLow, float outHigh)
	{
		return (input - inLow) / (inHigh - inLow) * (outHigh - outLow) + outLow;
	}

	// Token: 0x06000006 RID: 6 RVA: 0x0000208D File Offset: 0x0000028D
	public static float RemapClamp(this float input, float inLow, float inHigh, float outLow, float outHigh)
	{
		return (Mathf.Clamp(input, inLow, inHigh) - inLow) / (inHigh - inLow) * (outHigh - outLow) + outLow;
	}

	// Token: 0x06000007 RID: 7 RVA: 0x000020A4 File Offset: 0x000002A4
	public static float Remap01(this float input, float inLow, float inHigh)
	{
		return (input - inLow) / (inHigh - inLow);
	}

	// Token: 0x06000008 RID: 8 RVA: 0x000020AD File Offset: 0x000002AD
	public static float RemapClamp01(this float input, float inLow, float inHigh)
	{
		return (Mathf.Clamp(input, inLow, inHigh) - inLow) / (inHigh - inLow);
	}

	// Token: 0x06000009 RID: 9 RVA: 0x000020BD File Offset: 0x000002BD
	public static float OneMinus(this float input)
	{
		return Mathf.Clamp01(1f - input);
	}

	// Token: 0x0600000A RID: 10 RVA: 0x000020CB File Offset: 0x000002CB
	public static float Randomize(this float input, float range)
	{
		return input * Random.Range(1f - range, 1f + range);
	}

	// Token: 0x0600000B RID: 11 RVA: 0x000020E4 File Offset: 0x000002E4
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

	// Token: 0x0600000C RID: 12 RVA: 0x0000213C File Offset: 0x0000033C
	public static float MapOverCurve(this float time, params Tuple<float, float>[] points)
	{
		AnimationCurve curve = new AnimationCurve();
		foreach (Tuple<float, float> point in points)
		{
			curve.AddKey(new Keyframe(point.Item1, point.Item2));
		}
		return curve.Evaluate(time);
	}

	// Token: 0x0600000D RID: 13 RVA: 0x0000218C File Offset: 0x0000038C
	public static float MapOverCurve(this float time, params Tuple<float, float, float, float>[] points)
	{
		AnimationCurve curve = new AnimationCurve();
		foreach (Tuple<float, float, float, float> point in points)
		{
			curve.AddKey(new Keyframe(point.Item1, point.Item2, point.Item3, point.Item4));
		}
		return curve.Evaluate(time);
	}

	// Token: 0x0600000E RID: 14 RVA: 0x000021E8 File Offset: 0x000003E8
	public static RaycastHit[] ConeCastAll(this Vector3 origin, float maxRadius, Vector3 direction, float maxDistance, float coneAngle, int layer = -5)
	{
		RaycastHit[] sphereCastHits = Physics.SphereCastAll(origin, maxRadius, direction, maxDistance, layer, 1);
		List<RaycastHit> coneCastHitList = new List<RaycastHit>();
		bool flag = sphereCastHits.Length != 0;
		if (flag)
		{
			for (int i = 0; i < sphereCastHits.Length; i++)
			{
				Vector3 hitPoint = sphereCastHits[i].point;
				Vector3 directionToHit = hitPoint - origin;
				float angleToHit = Vector3.Angle(direction, directionToHit);
				float multiplier = 1f;
				bool flag2 = directionToHit.magnitude < 2f;
				if (flag2)
				{
					multiplier = 4f;
				}
				Rigidbody rb = sphereCastHits[i].rigidbody;
				bool hitRigidbody = rb != null && Vector3.Angle(direction, rb.transform.position - origin) < coneAngle * multiplier;
				bool flag3 = angleToHit < coneAngle * multiplier || hitRigidbody;
				if (flag3)
				{
					coneCastHitList.Add(sphereCastHits[i]);
				}
			}
		}
		return coneCastHitList.ToArray();
	}

	// Token: 0x0600000F RID: 15 RVA: 0x000022DC File Offset: 0x000004DC
	public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
	{
		T t;
		if ((t = obj.GetComponent<T>()) == null)
		{
			t = obj.AddComponent<T>();
		}
		return t;
	}

	// Token: 0x06000010 RID: 16 RVA: 0x00002303 File Offset: 0x00000503
	public static Vector3 PalmDir(this RagdollHand hand)
	{
		return -hand.transform.forward;
	}

	// Token: 0x06000011 RID: 17 RVA: 0x00002315 File Offset: 0x00000515
	public static Vector3 ThumbDir(this RagdollHand hand)
	{
		return (hand.side == null) ? hand.transform.up : (-hand.transform.up);
	}

	// Token: 0x06000012 RID: 18 RVA: 0x0000233C File Offset: 0x0000053C
	public static Vector3 PointDir(this RagdollHand hand)
	{
		return -hand.transform.right;
	}

	// Token: 0x06000013 RID: 19 RVA: 0x00002350 File Offset: 0x00000550
	public static Vector3 PosAboveBackOfHand(this RagdollHand hand, float factor = 1f)
	{
		return hand.transform.position - hand.transform.right * 0.1f * factor + hand.transform.forward * 0.2f * factor;
	}

	// Token: 0x06000014 RID: 20 RVA: 0x000023A8 File Offset: 0x000005A8
	public static Quaternion GetFlyDirRefLocalRotation(this Item item)
	{
		return Quaternion.Inverse(item.transform.rotation) * item.flyDirRef.rotation;
	}

	// Token: 0x06000015 RID: 21 RVA: 0x000023CC File Offset: 0x000005CC
	public static void SetVFXProperty<T>(this EffectInstance effect, string name, T data)
	{
		bool flag = effect == null;
		if (!flag)
		{
			Vector3 v;
			bool flag2;
			if (data is Vector3)
			{
				v = data as Vector3;
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
						EffectVfx effectVfx6 = fx as EffectVfx;
						return effectVfx6 != null && effectVfx6.vfx.HasVector3(name);
					});
				}
				foreach (Effect effect2 in effects.Where(func))
				{
					EffectVfx effectVfx = (EffectVfx)effect2;
					effectVfx.vfx.SetVector3(name, v);
				}
			}
			else
			{
				float f2;
				bool flag4;
				if (data is float)
				{
					f2 = data as float;
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
							EffectVfx effectVfx6 = fx as EffectVfx;
							return effectVfx6 != null && effectVfx6.vfx.HasFloat(name);
						});
					}
					foreach (Effect effect3 in effects2.Where(func2))
					{
						EffectVfx effectVfx2 = (EffectVfx)effect3;
						effectVfx2.vfx.SetFloat(name, f2);
					}
				}
				else
				{
					int i3;
					bool flag6;
					if (data is int)
					{
						i3 = data as int;
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
								EffectVfx effectVfx6 = fx as EffectVfx;
								return effectVfx6 != null && effectVfx6.vfx.HasInt(name);
							});
						}
						foreach (Effect effect4 in effects3.Where(func3))
						{
							EffectVfx effectVfx3 = (EffectVfx)effect4;
							effectVfx3.vfx.SetInt(name, i3);
						}
					}
					else
					{
						bool b4;
						bool flag8;
						if (data is bool)
						{
							b4 = data as bool;
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
									EffectVfx effectVfx6 = fx as EffectVfx;
									return effectVfx6 != null && effectVfx6.vfx.HasBool(name);
								});
							}
							foreach (Effect effect5 in effects4.Where(func4))
							{
								EffectVfx effectVfx4 = (EffectVfx)effect5;
								effectVfx4.vfx.SetBool(name, b4);
							}
						}
						else
						{
							Texture t5 = data as Texture;
							bool flag10 = t5 == null;
							if (!flag10)
							{
								IEnumerable<Effect> effects5 = effect.effects;
								Func<Effect, bool> <>9__4;
								Func<Effect, bool> func5;
								if ((func5 = <>9__4) == null)
								{
									func5 = (<>9__4 = delegate(Effect fx)
									{
										EffectVfx effectVfx6 = fx as EffectVfx;
										return effectVfx6 != null && effectVfx6.vfx.HasTexture(name);
									});
								}
								foreach (Effect effect6 in effects5.Where(func5))
								{
									EffectVfx effectVfx5 = (EffectVfx)effect6;
									effectVfx5.vfx.SetTexture(name, t5);
								}
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06000016 RID: 22 RVA: 0x00002744 File Offset: 0x00000944
	public static object GetVFXProperty(this EffectInstance effect, string name)
	{
		foreach (Effect effect2 in effect.effects)
		{
			EffectVfx effectVfx = effect2 as EffectVfx;
			bool flag = effectVfx != null;
			if (flag)
			{
				bool flag2 = effectVfx.vfx.HasFloat(name);
				if (flag2)
				{
					return effectVfx.vfx.GetFloat(name);
				}
				bool flag3 = effectVfx.vfx.HasVector3(name);
				if (flag3)
				{
					return effectVfx.vfx.GetVector3(name);
				}
				bool flag4 = effectVfx.vfx.HasBool(name);
				if (flag4)
				{
					return effectVfx.vfx.GetBool(name);
				}
				bool flag5 = effectVfx.vfx.HasInt(name);
				if (flag5)
				{
					return effectVfx.vfx.GetInt(name);
				}
			}
		}
		return null;
	}

	// Token: 0x06000017 RID: 23 RVA: 0x00002858 File Offset: 0x00000A58
	public static void HapticTick(this RagdollHand hand, float intensity = 1f, float frequency = 10f)
	{
		PlayerControl.input.Haptic(hand.side, intensity, frequency);
	}

	// Token: 0x06000018 RID: 24 RVA: 0x0000286D File Offset: 0x00000A6D
	public static void PlayHapticClipOver(this RagdollHand hand, AnimationCurve curve, float duration)
	{
		hand.StartCoroutine(Snippet.HapticPlayer(hand, curve, duration));
	}

	// Token: 0x06000019 RID: 25 RVA: 0x0000287F File Offset: 0x00000A7F
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

	// Token: 0x0600001A RID: 26 RVA: 0x0000289C File Offset: 0x00000A9C
	public static bool XBigger(this Vector3 vec)
	{
		return Mathf.Abs(vec.x) > Mathf.Abs(vec.y) && Mathf.Abs(vec.x) > Mathf.Abs(vec.z);
	}

	// Token: 0x0600001B RID: 27 RVA: 0x000028D1 File Offset: 0x00000AD1
	public static bool YBigger(this Vector3 vec)
	{
		return Mathf.Abs(vec.y) > Mathf.Abs(vec.x) && Mathf.Abs(vec.y) > Mathf.Abs(vec.z);
	}

	// Token: 0x0600001C RID: 28 RVA: 0x00002906 File Offset: 0x00000B06
	public static bool ZBigger(this Vector3 vec)
	{
		return Mathf.Abs(vec.z) > Mathf.Abs(vec.x) && Mathf.Abs(vec.z) > Mathf.Abs(vec.y);
	}

	// Token: 0x0600001D RID: 29 RVA: 0x0000293B File Offset: 0x00000B3B
	public static Vector3 Velocity(this RagdollHand hand)
	{
		return Player.local.transform.rotation * hand.playerHand.controlHand.GetHandVelocity();
	}

	// Token: 0x0600001E RID: 30 RVA: 0x00002964 File Offset: 0x00000B64
	public static IEnumerable<TOut> SelectNotNull<TIn, TOut>(this IEnumerable<TIn> enumerable, Func<TIn, TOut> func)
	{
		return enumerable.Where((TIn item) => func(item) != null).Select(func);
	}

	// Token: 0x0600001F RID: 31 RVA: 0x0000299C File Offset: 0x00000B9C
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

	// Token: 0x06000020 RID: 32 RVA: 0x000029E8 File Offset: 0x00000BE8
	public static bool IsImportant(this RagdollPart part)
	{
		RagdollPart.Type type = part.type;
		return type == 1 || type == 4 || type == 32 || type == 64 || type == 512 || type == 1024;
	}

	// Token: 0x06000021 RID: 33 RVA: 0x00002A26 File Offset: 0x00000C26
	public static RagdollPart GetPart(this Creature creature, RagdollPart.Type partType)
	{
		return creature.ragdoll.GetPart(partType);
	}

	// Token: 0x06000022 RID: 34 RVA: 0x00002A34 File Offset: 0x00000C34
	public static RagdollPart GetHead(this Creature creature)
	{
		return creature.ragdoll.headPart;
	}

	// Token: 0x06000023 RID: 35 RVA: 0x00002A41 File Offset: 0x00000C41
	public static RagdollPart GetTorso(this Creature creature)
	{
		return creature.GetPart(4);
	}

	// Token: 0x06000024 RID: 36 RVA: 0x00002A4A File Offset: 0x00000C4A
	public static Vector3 GetChest(this Creature creature)
	{
		return Vector3.Lerp(creature.GetTorso().transform.position, creature.GetHead().transform.position, 0.5f);
	}

	// Token: 0x06000025 RID: 37 RVA: 0x00002A78 File Offset: 0x00000C78
	public static IEnumerable<Creature> CreaturesInRadius(this Vector3 position, float radius, bool targetAliveCreature = true, bool targetDeadCreature = false, bool targetPlayer = false)
	{
		List<Creature> creatureDetected = new List<Creature>();
		for (int i = Creature.allActive.Count - 1; i >= 0; i--)
		{
			bool flag = (Creature.allActive[i].GetChest() - position).sqrMagnitude < radius * radius && (targetAliveCreature || Creature.allActive[i].state == 0) && (targetDeadCreature || Creature.allActive[i].state > 0) && (targetPlayer || !Creature.allActive[i].isPlayer);
			if (flag)
			{
				creatureDetected.Add(Creature.allActive[i]);
			}
		}
		return creatureDetected;
	}

	// Token: 0x06000026 RID: 38 RVA: 0x00002B48 File Offset: 0x00000D48
	public static IEnumerable<Creature> CreaturesInConeRadius(this Vector3 position, float radius, Vector3 directionOfCone, float angleOfCone, bool targetAliveCreature = true, bool targetDeadCreature = false, bool targetPlayer = false)
	{
		List<Creature> creatureDetected = new List<Creature>();
		for (int i = Creature.allActive.Count - 1; i >= 0; i--)
		{
			bool flag = (Creature.allActive[i].GetChest() - position).sqrMagnitude < radius * radius && (targetAliveCreature || Creature.allActive[i].state == 0) && (targetDeadCreature || Creature.allActive[i].state > 0) && (targetPlayer || !Creature.allActive[i].isPlayer) && Vector3.Angle(Creature.allActive[i].transform.position - position, directionOfCone) <= angleOfCone / 2f;
			if (flag)
			{
				creatureDetected.Add(Creature.allActive[i]);
			}
		}
		return creatureDetected.ToList<Creature>();
	}

	// Token: 0x06000027 RID: 39 RVA: 0x00002C50 File Offset: 0x00000E50
	public static Creature RandomCreatureInRadius(this Vector3 position, float radius, bool targetAliveCreature = true, bool targetDeadCreature = false, bool targetPlayer = false, Creature creatureToExclude = null, bool includeCreatureExcludedIfDefault = false)
	{
		List<Creature> creatureDetected = new List<Creature>();
		for (int i = Creature.allActive.Count - 1; i >= 0; i--)
		{
			bool flag = Creature.allActive[i] != creatureToExclude && !includeCreatureExcludedIfDefault && (Creature.allActive[i].GetChest() - position).sqrMagnitude < radius * radius && (targetAliveCreature || Creature.allActive[i].state == 0) && (targetDeadCreature || Creature.allActive[i].state > 0) && (targetPlayer || !Creature.allActive[i].isPlayer);
			if (flag)
			{
				creatureDetected.Add(Creature.allActive[i]);
			}
		}
		bool flag2 = creatureDetected.Count<Creature>() != 0;
		Creature creature;
		if (flag2)
		{
			creature = creatureDetected[Random.Range(0, creatureDetected.Count<Creature>())];
		}
		else
		{
			creature = null;
		}
		return creature;
	}

	// Token: 0x06000028 RID: 40 RVA: 0x00002D60 File Offset: 0x00000F60
	public static Creature ClosestCreatureInRadius(this Vector3 position, float radius, bool targetAliveCreature = true, bool targetDeadCreature = false, bool targetPlayer = false, Creature creatureToExclude = null)
	{
		List<Creature> creatureDetected = new List<Creature>();
		for (int i = Creature.allActive.Count - 1; i >= 0; i--)
		{
			bool flag = Creature.allActive[i] != creatureToExclude && (Creature.allActive[i].GetChest() - position).sqrMagnitude < radius * radius && (targetAliveCreature || Creature.allActive[i].state == 0) && (targetDeadCreature || Creature.allActive[i].state > 0) && (targetPlayer || !Creature.allActive[i].isPlayer);
			if (flag)
			{
				creatureDetected.Add(Creature.allActive[i]);
			}
		}
		bool flag2 = creatureDetected != null;
		Creature creature2;
		if (flag2)
		{
			float lastRadius = float.PositiveInfinity;
			Creature lastCreature = null;
			foreach (Creature creature in creatureDetected)
			{
				float thisRadius = (position - creature.transform.position).sqrMagnitude;
				bool flag3 = thisRadius <= lastRadius * lastRadius;
				if (flag3)
				{
					lastRadius = thisRadius;
					lastCreature = creature;
				}
			}
			creature2 = lastCreature;
		}
		else
		{
			creature2 = null;
		}
		return creature2;
	}

	// Token: 0x06000029 RID: 41 RVA: 0x00002ED8 File Offset: 0x000010D8
	public static Creature ClosestCreatureInListFromPosition(this List<Creature> creatures, Vector3 position)
	{
		float lastRadius = float.PositiveInfinity;
		Creature lastCreature = null;
		foreach (Creature creature in creatures)
		{
			float thisRadius = (position - creature.transform.position).sqrMagnitude;
			bool flag = thisRadius <= lastRadius * lastRadius;
			if (flag)
			{
				lastRadius = thisRadius;
				lastCreature = creature;
			}
		}
		return lastCreature;
	}

	// Token: 0x0600002A RID: 42 RVA: 0x00002F68 File Offset: 0x00001168
	public static Creature FarestCreatureInListFromPosition(this List<Creature> creatures, Vector3 position)
	{
		float lastRadius = 0f;
		Creature lastCreature = null;
		foreach (Creature creature in creatures)
		{
			float thisRadius = (position - creature.transform.position).sqrMagnitude;
			bool flag = thisRadius >= lastRadius * lastRadius;
			if (flag)
			{
				lastRadius = thisRadius;
				lastCreature = creature;
			}
		}
		return lastCreature;
	}

	// Token: 0x0600002B RID: 43 RVA: 0x00002FF8 File Offset: 0x000011F8
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

	// Token: 0x0600002C RID: 44 RVA: 0x00003090 File Offset: 0x00001290
	public static RagdollPart GetRandomRagdollPart(this Creature creature, int mask = 2047)
	{
		List<RagdollPart> ragdollParts = new List<RagdollPart>();
		foreach (RagdollPart part in creature.ragdoll.parts)
		{
			bool flag = (mask & part.type) > 0;
			if (flag)
			{
				ragdollParts.Add(part);
			}
		}
		return ragdollParts[Random.Range(0, ragdollParts.Count<RagdollPart>())];
	}

	// Token: 0x0600002D RID: 45 RVA: 0x0000311C File Offset: 0x0000131C
	public static bool returnWaveStarted()
	{
		int nbWaveStarted = 0;
		foreach (WaveSpawner waveSpawner in WaveSpawner.instances)
		{
			bool isRunning = waveSpawner.isRunning;
			if (isRunning)
			{
				nbWaveStarted++;
			}
		}
		return nbWaveStarted != 0;
	}

	// Token: 0x0600002E RID: 46 RVA: 0x0000318C File Offset: 0x0000138C
	public static Vector3 FromToDirection(this Vector3 from, Vector3 to)
	{
		return to - from;
	}

	// Token: 0x0600002F RID: 47 RVA: 0x000031A8 File Offset: 0x000013A8
	public static void Attraction_Repulsion_Force(this Rigidbody rigidbody, Vector3 origin, Vector3 attractedRb, bool useDistance, float coef)
	{
		Vector3 direction = attractedRb.FromToDirection(origin).normalized;
		if (useDistance)
		{
			float distance = attractedRb.FromToDirection(origin).magnitude;
			rigidbody.AddForce(direction * (coef / distance) / (rigidbody.mass / 2f), 2);
		}
		else
		{
			rigidbody.AddForce(direction * coef / (rigidbody.mass / 2f), 2);
		}
	}

	// Token: 0x06000030 RID: 48 RVA: 0x00003228 File Offset: 0x00001428
	public static void Attraction_Repulsion_ForceNoMass(this Rigidbody rigidbody, Vector3 origin, Vector3 attractedRb, bool useDistance, float coef)
	{
		Vector3 direction = attractedRb.FromToDirection(origin).normalized;
		if (useDistance)
		{
			float distance = attractedRb.FromToDirection(origin).magnitude;
			rigidbody.AddForce(direction * (coef / distance), 2);
		}
		else
		{
			rigidbody.AddForce(direction * coef, 2);
		}
	}

	// Token: 0x06000031 RID: 49 RVA: 0x00003284 File Offset: 0x00001484
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

	// Token: 0x06000032 RID: 50 RVA: 0x000032D0 File Offset: 0x000014D0
	public static Vector3 RotateCircle(this Vector3 origin, Vector3 forwardDirection, Vector3 upDirection, float radius, float speed, int nbElementsAroundCircle, int i)
	{
		return origin + Quaternion.AngleAxis((float)i * 360f / (float)nbElementsAroundCircle + speed, forwardDirection) * upDirection * radius;
	}

	// Token: 0x06000033 RID: 51 RVA: 0x0000330C File Offset: 0x0000150C
	public static Vector3 PosAroundCircle(this Vector3 origin, Vector3 forwardDirection, Vector3 upDirection, float radius, int nbElementsAroundCircle, int i)
	{
		return origin + Quaternion.AngleAxis((float)i * 360f / (float)nbElementsAroundCircle, forwardDirection) * upDirection * radius;
	}

	// Token: 0x06000034 RID: 52 RVA: 0x00003344 File Offset: 0x00001544
	public static ConfigurableJoint CreateSimpleJoint(Rigidbody source, Rigidbody target, float spring, float damper)
	{
		Quaternion orgRotation = source.transform.rotation;
		source.transform.rotation = target.transform.rotation;
		ConfigurableJoint joint = source.gameObject.AddComponent<ConfigurableJoint>();
		joint.autoConfigureConnectedAnchor = false;
		joint.targetRotation = Quaternion.identity;
		joint.anchor = source.centerOfMass;
		joint.connectedAnchor = target.centerOfMass;
		joint.connectedBody = target;
		JointDrive jointDrive = default(JointDrive);
		jointDrive.positionSpring = spring;
		jointDrive.positionDamper = damper;
		jointDrive.maximumForce = float.PositiveInfinity;
		JointDrive posDrive = jointDrive;
		jointDrive = default(JointDrive);
		jointDrive.positionSpring = 1000f;
		jointDrive.positionDamper = 10f;
		jointDrive.maximumForce = float.PositiveInfinity;
		JointDrive rotDrive = jointDrive;
		joint.rotationDriveMode = 0;
		joint.xDrive = posDrive;
		joint.yDrive = posDrive;
		joint.zDrive = posDrive;
		joint.angularXDrive = rotDrive;
		joint.angularYZDrive = rotDrive;
		source.transform.rotation = orgRotation;
		joint.angularXMotion = 2;
		joint.angularYMotion = 2;
		joint.angularZMotion = 2;
		joint.xMotion = 2;
		joint.yMotion = 2;
		joint.zMotion = 2;
		return joint;
	}

	// Token: 0x06000035 RID: 53 RVA: 0x00003488 File Offset: 0x00001688
	public static ConfigurableJoint CreateSlingshotJoint(Rigidbody source, Rigidbody target, float spring, float damper)
	{
		Quaternion orgRotation = source.transform.rotation;
		ConfigurableJoint joint = source.gameObject.AddComponent<ConfigurableJoint>();
		joint.autoConfigureConnectedAnchor = false;
		joint.targetRotation = Quaternion.identity;
		joint.anchor = Vector3.zero;
		joint.connectedAnchor = target.centerOfMass;
		joint.connectedBody = target;
		JointDrive jointDrive = default(JointDrive);
		jointDrive.positionSpring = spring;
		jointDrive.positionDamper = damper;
		jointDrive.maximumForce = float.PositiveInfinity;
		JointDrive posDrive = jointDrive;
		jointDrive = default(JointDrive);
		jointDrive.positionSpring = 0f;
		jointDrive.positionDamper = 0f;
		jointDrive.maximumForce = float.PositiveInfinity;
		JointDrive emptyDrive = jointDrive;
		SoftJointLimit softJointLimit2 = default(SoftJointLimit);
		softJointLimit2.limit = 0.76f;
		softJointLimit2.bounciness = 0f;
		softJointLimit2.contactDistance = 0f;
		SoftJointLimit softJointLimit = softJointLimit2;
		joint.linearLimit = softJointLimit;
		joint.rotationDriveMode = 0;
		joint.xDrive = emptyDrive;
		joint.yDrive = posDrive;
		joint.zDrive = emptyDrive;
		joint.angularXDrive = emptyDrive;
		joint.angularYZDrive = emptyDrive;
		joint.slerpDrive = emptyDrive;
		source.transform.rotation = orgRotation;
		joint.angularXMotion = 0;
		joint.angularYMotion = 0;
		joint.angularZMotion = 0;
		joint.xMotion = 0;
		joint.yMotion = 1;
		joint.zMotion = 0;
		joint.massScale = 15f;
		return joint;
	}

	// Token: 0x06000036 RID: 54 RVA: 0x00003604 File Offset: 0x00001804
	public static ConfigurableJoint StrongJointFixed(Rigidbody source, Rigidbody target, float massScale = 30f, bool limitMotion = false)
	{
		ConfigurableJoint joint = target.gameObject.AddComponent<ConfigurableJoint>();
		joint.autoConfigureConnectedAnchor = false;
		joint.targetRotation = Quaternion.identity;
		joint.anchor = Vector3.zero;
		joint.connectedBody = source;
		joint.connectedAnchor = Vector3.zero;
		joint.rotationDriveMode = 0;
		JointDrive jointDrive = default(JointDrive);
		jointDrive.positionSpring = 2000f;
		jointDrive.positionDamper = 40f;
		jointDrive.maximumForce = 100f;
		JointDrive posDrive = jointDrive;
		jointDrive = default(JointDrive);
		jointDrive.positionSpring = 1000f;
		jointDrive.positionDamper = 40f;
		jointDrive.maximumForce = 100f;
		JointDrive rotDrive = jointDrive;
		joint.xDrive = posDrive;
		joint.yDrive = posDrive;
		joint.zDrive = posDrive;
		joint.angularXDrive = rotDrive;
		joint.angularYZDrive = rotDrive;
		joint.massScale = massScale;
		joint.connectedMassScale = 1f / massScale;
		if (limitMotion)
		{
			joint.angularXMotion = 1;
			joint.angularYMotion = 1;
			joint.angularZMotion = 1;
			joint.xMotion = 1;
			joint.yMotion = 1;
			joint.zMotion = 1;
		}
		else
		{
			joint.angularXMotion = 2;
			joint.angularYMotion = 2;
			joint.angularZMotion = 2;
			joint.xMotion = 2;
			joint.yMotion = 2;
			joint.zMotion = 2;
		}
		return joint;
	}

	// Token: 0x06000037 RID: 55 RVA: 0x00003770 File Offset: 0x00001970
	public static ConfigurableJoint CreateJointToProjectileForCreatureAttraction(this Item projectile, RagdollPart attractedRagdollPart, ConfigurableJoint joint)
	{
		JointDrive jointDrive = default(JointDrive);
		jointDrive.positionSpring = 1f;
		jointDrive.positionDamper = 0.2f;
		SoftJointLimit softJointLimit = default(SoftJointLimit);
		softJointLimit.limit = 0.15f;
		SoftJointLimitSpring linearLimitSpring = default(SoftJointLimitSpring);
		linearLimitSpring.spring = 1f;
		linearLimitSpring.damper = 0.2f;
		joint = attractedRagdollPart.gameObject.AddComponent<ConfigurableJoint>();
		joint.autoConfigureConnectedAnchor = false;
		joint.targetRotation = Quaternion.identity;
		joint.anchor = Vector3.zero;
		joint.connectedBody = projectile.GetComponent<Rigidbody>();
		joint.connectedAnchor = Vector3.zero;
		joint.xMotion = 1;
		joint.yMotion = 1;
		joint.zMotion = 1;
		joint.angularXMotion = 1;
		joint.angularYMotion = 1;
		joint.angularZMotion = 1;
		joint.linearLimitSpring = linearLimitSpring;
		joint.linearLimit = softJointLimit;
		joint.angularXLimitSpring = linearLimitSpring;
		joint.xDrive = jointDrive;
		joint.yDrive = jointDrive;
		joint.zDrive = jointDrive;
		joint.massScale = 10000f;
		joint.connectedMassScale = 1E-05f;
		return joint;
	}

	// Token: 0x06000038 RID: 56 RVA: 0x0000389C File Offset: 0x00001A9C
	public static FixedJoint CreateStickyJointBetweenTwoRigidBodies(this Rigidbody connectedRB, Rigidbody targetRB, FixedJoint joint)
	{
		joint = targetRB.gameObject.AddComponent<FixedJoint>();
		joint.anchor = Vector3.zero;
		joint.connectedBody = connectedRB;
		joint.connectedAnchor = Vector3.zero;
		joint.massScale = 1E-05f;
		joint.connectedMassScale = 10000f;
		joint.breakForce = float.PositiveInfinity;
		joint.breakTorque = float.PositiveInfinity;
		return joint;
	}

	// Token: 0x06000039 RID: 57 RVA: 0x0000390C File Offset: 0x00001B0C
	public static void DestroyJoint(this Rigidbody rb)
	{
		bool flag = rb.gameObject.GetComponent<ConfigurableJoint>();
		if (flag)
		{
			Object.Destroy(rb.gameObject.GetComponent<ConfigurableJoint>());
		}
		bool flag2 = rb.gameObject.GetComponent<CharacterJoint>();
		if (flag2)
		{
			Object.Destroy(rb.gameObject.GetComponent<CharacterJoint>());
		}
		bool flag3 = rb.gameObject.GetComponent<SpringJoint>();
		if (flag3)
		{
			Object.Destroy(rb.gameObject.GetComponent<SpringJoint>());
		}
		bool flag4 = rb.gameObject.GetComponent<HingeJoint>();
		if (flag4)
		{
			Object.Destroy(rb.gameObject.GetComponent<HingeJoint>());
		}
	}

	// Token: 0x0600003A RID: 58 RVA: 0x000039B8 File Offset: 0x00001BB8
	public static void IgnoreCollider(this Ragdoll ragdoll, Collider collider, bool ignore = true)
	{
		foreach (RagdollPart part in ragdoll.parts)
		{
			part.IgnoreCollider(collider, ignore);
		}
	}

	// Token: 0x0600003B RID: 59 RVA: 0x00003A14 File Offset: 0x00001C14
	public static void IgnoreCollider(this RagdollPart part, Collider collider, bool ignore = true)
	{
		foreach (Collider itemCollider in part.colliderGroup.colliders)
		{
			Physics.IgnoreCollision(collider, itemCollider, ignore);
		}
	}

	// Token: 0x0600003C RID: 60 RVA: 0x00003A74 File Offset: 0x00001C74
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

	// Token: 0x0600003D RID: 61 RVA: 0x00003B10 File Offset: 0x00001D10
	public static void IgnoreColliderBetweenItem(this Item item, Item itemIgnored, bool ignore = true)
	{
		foreach (ColliderGroup colliderGroup in item.colliderGroups)
		{
			foreach (Collider collider in colliderGroup.colliders)
			{
				foreach (ColliderGroup colliderGroup2 in itemIgnored.colliderGroups)
				{
					foreach (Collider collider2 in colliderGroup2.colliders)
					{
						Physics.IgnoreCollision(collider, collider2, ignore);
					}
				}
			}
		}
		if (ignore)
		{
			item.ignoredItem = item;
		}
		else
		{
			item.ignoredItem = null;
		}
	}

	// Token: 0x0600003E RID: 62 RVA: 0x00003C50 File Offset: 0x00001E50
	public static void addIgnoreRagdollAndItemHoldingCollision(Item item, Creature creature)
	{
		foreach (ColliderGroup colliderGroup in item.colliderGroups)
		{
			foreach (Collider collider in colliderGroup.colliders)
			{
				creature.ragdoll.IgnoreCollision(collider, true, 0);
			}
		}
		item.ignoredRagdoll = creature.ragdoll;
		Handle grabbedHandle = creature.handLeft.grabbedHandle;
		bool flag = ((grabbedHandle != null) ? grabbedHandle.item : null) != null;
		if (flag)
		{
			foreach (ColliderGroup colliderGroup2 in item.colliderGroups)
			{
				foreach (Collider collider2 in colliderGroup2.colliders)
				{
					foreach (ColliderGroup colliderGroup3 in creature.handLeft.grabbedHandle.item.colliderGroups)
					{
						foreach (Collider collider3 in colliderGroup3.colliders)
						{
							Physics.IgnoreCollision(collider2, collider3, true);
						}
					}
				}
			}
			item.ignoredItem = creature.handLeft.grabbedHandle.item;
		}
		Handle grabbedHandle2 = creature.handRight.grabbedHandle;
		bool flag2 = ((grabbedHandle2 != null) ? grabbedHandle2.item : null) != null;
		if (flag2)
		{
			foreach (ColliderGroup colliderGroup4 in item.colliderGroups)
			{
				foreach (Collider collider4 in colliderGroup4.colliders)
				{
					foreach (ColliderGroup colliderGroup5 in creature.handRight.grabbedHandle.item.colliderGroups)
					{
						foreach (Collider collider5 in colliderGroup5.colliders)
						{
							Physics.IgnoreCollision(collider4, collider5, true);
						}
					}
				}
			}
			item.ignoredItem = creature.handRight.grabbedHandle.item;
		}
	}

	// Token: 0x0600003F RID: 63 RVA: 0x00003FCC File Offset: 0x000021CC
	public static void IgnoreCollision(this Item item, bool ignore = true)
	{
		foreach (ColliderGroup cg in item.colliderGroups)
		{
			foreach (Collider collider in cg.colliders)
			{
				collider.enabled = !ignore;
			}
		}
	}

	// Token: 0x06000040 RID: 64 RVA: 0x00004068 File Offset: 0x00002268
	public static List<RagdollPart> RagdollPartsImportantList(this Creature creature)
	{
		return new List<RagdollPart>
		{
			creature.GetPart(1),
			creature.GetPart(4),
			creature.GetPart(32),
			creature.GetPart(64),
			creature.GetPart(512),
			creature.GetPart(1024)
		};
	}

	// Token: 0x06000041 RID: 65 RVA: 0x000040E0 File Offset: 0x000022E0
	public static List<RagdollPart> RagdollPartsExtremitiesBodyList(this Creature creature)
	{
		return new List<RagdollPart>
		{
			creature.GetPart(32),
			creature.GetPart(64),
			creature.GetPart(512),
			creature.GetPart(1024)
		};
	}

	// Token: 0x06000042 RID: 66 RVA: 0x0000413C File Offset: 0x0000233C
	public static Vector3 RandomPositionAroundCreatureInRadius(this Creature creature, Vector3 offset, float radius)
	{
		return creature.transform.position + offset + new Vector3(Random.Range(-radius, radius), 0f, Random.Range(-radius, radius));
	}

	// Token: 0x06000043 RID: 67 RVA: 0x00004180 File Offset: 0x00002380
	public static Vector3 CalculatePositionFromAngleWithDistance(this Vector3 position, float angle, Vector3 axis, Vector3 upDir, float distance)
	{
		return position + Quaternion.AngleAxis(angle, axis) * upDir * distance;
	}

	// Token: 0x06000044 RID: 68 RVA: 0x000041AC File Offset: 0x000023AC
	public static Vector3 RandomPositionAroundCreatureInRadiusAngle(this Creature creature, Vector3 offset, float radius, float maxAngle, Vector3 direction, float distance)
	{
		return creature.GetHead().transform.position + offset + Quaternion.AngleAxis(Random.Range(-maxAngle, maxAngle), creature.transform.up) * direction * Random.Range(0f, radius) * distance;
	}

	// Token: 0x06000045 RID: 69 RVA: 0x00004210 File Offset: 0x00002410
	public static void DebugPosition(this Vector3 position, string textToDisplay)
	{
		Debug.Log(string.Concat(new string[]
		{
			"SnippetCode : ",
			textToDisplay,
			" : Position X : ",
			position.x.ToString(),
			"; Position Y : ",
			position.y.ToString(),
			"; Position Z : ",
			position.z.ToString()
		}));
	}

	// Token: 0x06000046 RID: 70 RVA: 0x00004280 File Offset: 0x00002480
	public static void DebugRotation(this Quaternion rotation, string textToDisplay)
	{
		Debug.Log(string.Concat(new string[]
		{
			"SnippetCode : ",
			textToDisplay,
			" : Rotation X : ",
			rotation.x.ToString(),
			"; Rotation Y : ",
			rotation.y.ToString(),
			"; Rotation Z : ",
			rotation.z.ToString()
		}));
	}

	// Token: 0x06000047 RID: 71 RVA: 0x000042F0 File Offset: 0x000024F0
	public static void DebugPositionAndRotation(this Transform transform, string textToDisplay)
	{
		string[] array = new string[8];
		array[0] = "SnippetCode : ";
		array[1] = textToDisplay;
		array[2] = " : Position X : ";
		int num = 3;
		Vector3 vector = transform.position;
		array[num] = vector.x.ToString();
		array[4] = "; Position Y : ";
		int num2 = 5;
		vector = transform.position;
		array[num2] = vector.y.ToString();
		array[6] = "; Position Z : ";
		int num3 = 7;
		vector = transform.position;
		array[num3] = vector.z.ToString();
		Debug.Log(string.Concat(array));
		string[] array2 = new string[8];
		array2[0] = "SnippetCode : ";
		array2[1] = textToDisplay;
		array2[2] = " : Rotation X : ";
		int num4 = 3;
		Quaternion quaternion = transform.rotation;
		array2[num4] = quaternion.x.ToString();
		array2[4] = "; Rotation Y : ";
		int num5 = 5;
		quaternion = transform.rotation;
		array2[num5] = quaternion.y.ToString();
		array2[6] = "; Rotation Z : ";
		int num6 = 7;
		quaternion = transform.rotation;
		array2[num6] = quaternion.z.ToString();
		Debug.Log(string.Concat(array2));
	}

	// Token: 0x06000048 RID: 72 RVA: 0x000043EC File Offset: 0x000025EC
	private static IEnumerator LerpMovement(this Vector3 positionToReach, Quaternion rotationToReach, Item itemToMove, float durationOfMvt)
	{
		foreach (ColliderGroup colliderGroup in itemToMove.colliderGroups)
		{
			foreach (Collider collider in colliderGroup.colliders)
			{
				collider.enabled = false;
				collider = null;
			}
			List<Collider>.Enumerator enumerator2 = default(List<Collider>.Enumerator);
			colliderGroup = null;
		}
		List<ColliderGroup>.Enumerator enumerator = default(List<ColliderGroup>.Enumerator);
		float time = 0f;
		Vector3 positionOrigin = itemToMove.transform.position;
		Quaternion orientationOrigin = itemToMove.transform.rotation;
		bool flag = positionToReach != positionOrigin;
		if (flag)
		{
			while (time < durationOfMvt)
			{
				itemToMove.transform.position = Vector3.Lerp(positionOrigin, positionToReach, time / durationOfMvt);
				itemToMove.transform.rotation = Quaternion.Lerp(orientationOrigin, rotationToReach, time / durationOfMvt);
				time += Time.deltaTime;
				yield return null;
			}
		}
		foreach (ColliderGroup colliderGroup2 in itemToMove.colliderGroups)
		{
			foreach (Collider collider2 in colliderGroup2.colliders)
			{
				collider2.enabled = true;
				collider2 = null;
			}
			List<Collider>.Enumerator enumerator4 = default(List<Collider>.Enumerator);
			colliderGroup2 = null;
		}
		List<ColliderGroup>.Enumerator enumerator3 = default(List<ColliderGroup>.Enumerator);
		yield break;
	}

	// Token: 0x06000049 RID: 73 RVA: 0x00004410 File Offset: 0x00002610
	private static IEnumerable<GameObject> GetGameObjectsChildrenOfGameObject(this GameObject gameObject, bool allInactive = true)
	{
		List<GameObject> gameObjects = new List<GameObject>();
		for (int i = 0; i < gameObject.transform.childCount; i++)
		{
			bool flag = gameObject.transform.GetChild(i).gameObject.activeSelf || allInactive;
			if (flag)
			{
				gameObjects.Add(gameObject.transform.GetChild(i).gameObject);
			}
		}
		return gameObjects;
	}

	// Token: 0x0600004A RID: 74 RVA: 0x00004480 File Offset: 0x00002680
	public static void listAllGameObjectsChildrenOfGameObject(this GameObject gameObject, bool allInactive = true)
	{
		int i = 0;
		foreach (GameObject go in gameObject.GetGameObjectsChildrenOfGameObject(allInactive))
		{
			Debug.Log(string.Format("Gameobject {0} {1} of parent : {2}", i, go.name, gameObject.name));
			i++;
		}
	}

	// Token: 0x0600004B RID: 75 RVA: 0x000044F4 File Offset: 0x000026F4
	private static IEnumerable<Component> GetComponentsOfGameObject(this GameObject gameObject, bool allInactive)
	{
		IEnumerable<Component> enumerable;
		if (!allInactive)
		{
			enumerable = from component in gameObject.GetComponents(typeof(Component))
				where component.gameObject.activeSelf
				select component;
		}
		else
		{
			IEnumerable<Component> components = gameObject.GetComponents(typeof(Component));
			enumerable = components;
		}
		return enumerable;
	}

	// Token: 0x0600004C RID: 76 RVA: 0x00004554 File Offset: 0x00002754
	public static void listAllComponentsOfGameObject(this GameObject gameObject, bool allInactive = true)
	{
		int i = 0;
		foreach (Component component in gameObject.GetComponentsOfGameObject(allInactive))
		{
			Debug.Log(string.Format("Component {0} of {1}; Type : {2}", i, component.name, component.GetType()));
			i++;
		}
	}

	// Token: 0x0600004D RID: 77 RVA: 0x000045C8 File Offset: 0x000027C8
	public static void listAllComponentsOfGameObjects()
	{
		List<GameObject> GOList = Object.FindObjectsOfType<GameObject>().ToList<GameObject>();
		foreach (GameObject gameObject in GOList)
		{
			Debug.Log("Gameobject Name : " + gameObject.name);
			Debug.Log("Gameobject Tag : " + gameObject.tag);
			gameObject.listAllComponentsOfGameObject(true);
		}
	}

	// Token: 0x0600004E RID: 78 RVA: 0x00004654 File Offset: 0x00002854
	public static IEnumerable<Light> LightInARadius(this Vector3 position, float radius)
	{
		return from item in Object.FindObjectsOfType<Light>()
			where (item.transform.position - position).sqrMagnitude < radius * radius
			select item;
	}

	// Token: 0x0600004F RID: 79 RVA: 0x00004690 File Offset: 0x00002890
	public static IEnumerable<Light> ListOfLightsInItems(this IEnumerable<Item> items)
	{
		List<Light> listLights = new List<Light>();
		foreach (Item item in items)
		{
			Light[] lights = item.GetComponents<Light>();
			listLights.AddRange(lights);
			lights = item.GetComponentsInChildren<Light>();
			listLights.AddRange(lights);
			lights = item.GetComponentsInParent<Light>();
			listLights.AddRange(lights);
		}
		return listLights;
	}

	// Token: 0x06000050 RID: 80 RVA: 0x00004710 File Offset: 0x00002910
	public static IEnumerable<Light> ListOfLightsInGameObject(this IEnumerable<GameObject> gameObjects)
	{
		List<Light> listLights = new List<Light>();
		foreach (GameObject gameObject in gameObjects)
		{
			Light[] lights = gameObject.GetComponents<Light>();
			listLights.AddRange(lights);
			lights = gameObject.GetComponentsInChildren<Light>();
			listLights.AddRange(lights);
			lights = gameObject.GetComponentsInParent<Light>();
			listLights.AddRange(lights);
		}
		return listLights;
	}

	// Token: 0x06000051 RID: 81 RVA: 0x00004790 File Offset: 0x00002990
	public static List<Item> GetItemsOnCreature(this Creature creature, ItemData.Type? dataType = null)
	{
		List<Item> list = new List<Item>();
		foreach (Holder holder in creature.holders)
		{
			foreach (Item item in holder.items)
			{
				bool flag = dataType != null;
				if (flag)
				{
					ItemData.Type type = item.data.type;
					ItemData.Type? type2 = dataType;
					bool flag2 = ((type == type2.GetValueOrDefault()) & (type2 != null)) && dataType != null;
					if (flag2)
					{
						list.Add(item);
					}
				}
				else
				{
					list.Add(item);
				}
			}
		}
		Handle grabbedHandle = creature.handLeft.grabbedHandle;
		bool flag3 = ((grabbedHandle != null) ? grabbedHandle.item : null) != null;
		if (flag3)
		{
			list.Add(creature.handLeft.grabbedHandle.item);
		}
		Handle grabbedHandle2 = creature.handRight.grabbedHandle;
		bool flag4 = ((grabbedHandle2 != null) ? grabbedHandle2.item : null) != null;
		if (flag4)
		{
			list.Add(creature.handRight.grabbedHandle.item);
		}
		Handle catchedHandle = creature.mana.casterLeft.telekinesis.catchedHandle;
		bool flag5 = ((catchedHandle != null) ? catchedHandle.item : null) != null;
		if (flag5)
		{
			List<Item> list2 = list;
			Handle catchedHandle2 = creature.mana.casterLeft.telekinesis.catchedHandle;
			list2.Add((catchedHandle2 != null) ? catchedHandle2.item : null);
		}
		Handle catchedHandle3 = creature.mana.casterRight.telekinesis.catchedHandle;
		bool flag6 = ((catchedHandle3 != null) ? catchedHandle3.item : null) != null;
		if (flag6)
		{
			List<Item> list3 = list;
			Handle catchedHandle4 = creature.mana.casterRight.telekinesis.catchedHandle;
			list3.Add((catchedHandle4 != null) ? catchedHandle4.item : null);
		}
		return list;
	}

	// Token: 0x06000052 RID: 82 RVA: 0x000049B4 File Offset: 0x00002BB4
	public static IEnumerable<Item> ItemsInRadiusAroundItem(this Vector3 position, Item thisItem, float radius)
	{
		List<Item> list = new List<Item>();
		for (int i = Item.allActive.Count<Item>() - 1; i >= 0; i--)
		{
			bool flag = (Item.allActive[i].transform.position - position).sqrMagnitude < radius * radius && !thisItem;
			if (flag)
			{
				list.Add(Item.allActive[i]);
			}
		}
		return list;
	}

	// Token: 0x06000053 RID: 83 RVA: 0x00004A3C File Offset: 0x00002C3C
	public static IEnumerable<Item> ItemsInRadius(Vector3 position, float radius, bool targetFlyingItem = true, bool targetThrownItem = true, Item itemToExclude = null)
	{
		Collider[] colliders = Physics.OverlapSphere(position, radius, -5, 1);
		List<Item> itemsList = new List<Item>();
		foreach (Collider collider in colliders)
		{
			Rigidbody attachedRigidbody = collider.attachedRigidbody;
			Item item2;
			if (attachedRigidbody == null)
			{
				item2 = null;
			}
			else
			{
				CollisionHandler component = attachedRigidbody.GetComponent<CollisionHandler>();
				item2 = ((component != null) ? component.item : null);
			}
			Item item = item2;
			bool flag = item != null;
			if (flag)
			{
				bool flag2 = !itemsList.Contains(item) && (!targetFlyingItem || item.isFlying) && (!targetThrownItem || item.isThrowed) && item != itemToExclude;
				if (flag2)
				{
					itemsList.Add(item);
				}
			}
		}
		return itemsList;
	}

	// Token: 0x06000054 RID: 84 RVA: 0x00004AF0 File Offset: 0x00002CF0
	public static IEnumerable<Item> ItemsInConeRadius(this Vector3 position, float radius, Vector3 directionOfCone, float angleOfCone, bool targetFlyingItem = true, bool targetThrownItem = true, Item itemToExclude = null)
	{
		Collider[] colliders = Physics.OverlapSphere(position, radius, -5, 1);
		List<Item> itemsList = new List<Item>();
		foreach (Collider collider in colliders)
		{
			Rigidbody attachedRigidbody = collider.attachedRigidbody;
			Item item2;
			if (attachedRigidbody == null)
			{
				item2 = null;
			}
			else
			{
				CollisionHandler component = attachedRigidbody.GetComponent<CollisionHandler>();
				item2 = ((component != null) ? component.item : null);
			}
			Item item = item2;
			bool flag = item != null;
			if (flag)
			{
				Vector3 directionTowardT = item.transform.position - position;
				float angleFromConeCenter = Vector3.Angle(directionTowardT, directionOfCone);
				bool flag2 = !itemsList.Contains(item) && (!targetFlyingItem || item.isFlying) && (!targetThrownItem || item.isThrowed) && item != itemToExclude && angleFromConeCenter <= angleOfCone / 2f;
				if (flag2)
				{
					itemsList.Add(item);
				}
			}
		}
		return itemsList;
	}

	// Token: 0x06000055 RID: 85 RVA: 0x00004BD8 File Offset: 0x00002DD8
	public static Item ClosestItemAroundItem(this Item thisItem, float radius)
	{
		float lastRadius = float.PositiveInfinity;
		Item lastItem = null;
		foreach (Item item in Item.allActive)
		{
			bool flag = item == thisItem;
			if (!flag)
			{
				float thisRadius = (item.transform.position - thisItem.transform.position).sqrMagnitude;
				bool flag2 = thisRadius < radius * radius && thisRadius < lastRadius;
				if (flag2)
				{
					lastRadius = thisRadius;
					lastItem = item;
				}
			}
		}
		return lastItem;
	}

	// Token: 0x06000056 RID: 86 RVA: 0x00004C88 File Offset: 0x00002E88
	public static Item ClosestItemAroundItemOverlapSphere(this Item thisItem, float radius)
	{
		float lastRadius = float.PositiveInfinity;
		Collider lastCollider = null;
		Collider[] colliders = Physics.OverlapSphere(thisItem.transform.position, radius, -5, 1);
		List<Item> itemsList = new List<Item>();
		foreach (Collider collider in colliders)
		{
			Rigidbody attachedRigidbody = collider.attachedRigidbody;
			Item item2;
			if (attachedRigidbody == null)
			{
				item2 = null;
			}
			else
			{
				CollisionHandler component = attachedRigidbody.GetComponent<CollisionHandler>();
				item2 = ((component != null) ? component.item : null);
			}
			Item item = item2;
			bool flag = item != null;
			if (flag)
			{
				bool flag2 = item != thisItem;
				if (flag2)
				{
					float thisRadius = (collider.ClosestPoint(thisItem.transform.position) - thisItem.transform.position).sqrMagnitude;
					bool flag3 = thisRadius < radius * radius && thisRadius < lastRadius;
					if (flag3)
					{
						lastRadius = thisRadius;
						lastCollider = collider;
					}
				}
			}
		}
		Object @object;
		if (lastCollider == null)
		{
			@object = null;
		}
		else
		{
			Rigidbody attachedRigidbody2 = lastCollider.attachedRigidbody;
			@object = ((attachedRigidbody2 != null) ? attachedRigidbody2.GetComponent<CollisionHandler>().item : null);
		}
		bool flag4 = @object == null;
		Item item3;
		if (flag4)
		{
			item3 = thisItem;
		}
		else
		{
			item3 = lastCollider.attachedRigidbody.GetComponent<CollisionHandler>().item;
		}
		return item3;
	}

	// Token: 0x06000057 RID: 87 RVA: 0x00004DAC File Offset: 0x00002FAC
	public static RagdollPart ClosestRagdollPartAroundItemOverlapSphere(this Item thisItem, float radius, bool targetPlayer = false)
	{
		float lastRadius = float.PositiveInfinity;
		Collider lastCollider = null;
		List<Collider> colliders = Physics.OverlapSphere(thisItem.transform.position, radius, -5, 1).Distinct<Collider>().Where(delegate(Collider coll)
		{
			Rigidbody attachedRigidbody2 = coll.attachedRigidbody;
			Object object2;
			if (attachedRigidbody2 == null)
			{
				object2 = null;
			}
			else
			{
				CollisionHandler component = attachedRigidbody2.GetComponent<CollisionHandler>();
				object2 = ((component != null) ? component.ragdollPart : null);
			}
			bool flag3;
			if (!((object2 != null) & targetPlayer))
			{
				List<RagdollPart> parts = Player.local.creature.ragdoll.parts;
				Rigidbody attachedRigidbody3 = coll.attachedRigidbody;
				RagdollPart ragdollPart2;
				if (attachedRigidbody3 == null)
				{
					ragdollPart2 = null;
				}
				else
				{
					CollisionHandler component2 = attachedRigidbody3.GetComponent<CollisionHandler>();
					ragdollPart2 = ((component2 != null) ? component2.ragdollPart : null);
				}
				flag3 = !parts.Contains(ragdollPart2);
			}
			else
			{
				flag3 = true;
			}
			return flag3;
		})
			.ToList<Collider>();
		foreach (Collider collider in colliders)
		{
			float thisRadius = (collider.ClosestPoint(thisItem.transform.position) - thisItem.transform.position).sqrMagnitude;
			bool flag = thisRadius < radius * radius && thisRadius < lastRadius;
			if (flag)
			{
				lastRadius = thisRadius;
				lastCollider = collider;
			}
		}
		Object @object;
		if (lastCollider == null)
		{
			@object = null;
		}
		else
		{
			Rigidbody attachedRigidbody = lastCollider.attachedRigidbody;
			@object = ((attachedRigidbody != null) ? attachedRigidbody.GetComponent<CollisionHandler>().ragdollPart : null);
		}
		bool flag2 = @object == null;
		RagdollPart ragdollPart;
		if (flag2)
		{
			ragdollPart = null;
		}
		else
		{
			ragdollPart = lastCollider.attachedRigidbody.GetComponent<CollisionHandler>().ragdollPart;
		}
		return ragdollPart;
	}

	// Token: 0x06000058 RID: 88 RVA: 0x00004ED0 File Offset: 0x000030D0
	public static RagdollPart ClosestRagdollPart(this Vector3 origin, Creature creature, int mask = 16383, RagdollPart partToExclude = null)
	{
		float lastRadius = float.PositiveInfinity;
		RagdollPart lastRagdollPart = null;
		foreach (RagdollPart part in creature.ragdoll.parts)
		{
			bool flag = (mask & part.type) > 0 && part != partToExclude;
			if (flag)
			{
				float thisRadius = Vector3.Distance(part.transform.position, origin);
				bool flag2 = thisRadius <= lastRadius;
				if (flag2)
				{
					lastRadius = thisRadius;
					lastRagdollPart = part;
				}
			}
		}
		return lastRagdollPart;
	}

	// Token: 0x06000059 RID: 89 RVA: 0x00004F7C File Offset: 0x0000317C
	public static RagdollPart FarestRagdollPart(this Vector3 origin, Creature creature, int mask = 16383, RagdollPart partToExclude = null)
	{
		float lastRadius = 0f;
		RagdollPart lastRagdollPart = null;
		foreach (RagdollPart part in creature.ragdoll.parts)
		{
			bool flag = (mask & part.type) > 0 && part != partToExclude;
			if (flag)
			{
				float thisRadius = Vector3.Distance(part.transform.position, origin);
				bool flag2 = thisRadius >= lastRadius;
				if (flag2)
				{
					lastRadius = thisRadius;
					lastRagdollPart = part;
				}
			}
		}
		return lastRagdollPart;
	}

	// Token: 0x0600005A RID: 90 RVA: 0x00005028 File Offset: 0x00003228
	public static int ReturnNbFreeSlotOnCreature(this Creature creature)
	{
		int nbFreeSlots = 0;
		foreach (Holder holder in creature.holders)
		{
			bool flag = holder.currentQuantity != 0;
			if (flag)
			{
				nbFreeSlots++;
			}
		}
		return nbFreeSlots;
	}

	// Token: 0x0600005B RID: 91 RVA: 0x00005098 File Offset: 0x00003298
	public static Vector3 HomingTarget(Rigidbody projectileRb, Vector3 targetPosition, float initialDistance, float forceFactor, float offSetInitialDistance = 0.25f, float distanceToStick = 0f)
	{
		return Vector3.Lerp(projectileRb.velocity, (targetPosition - projectileRb.position).normalized * Vector3.Distance(targetPosition, projectileRb.position) * forceFactor, Vector3.Distance(targetPosition, projectileRb.position).Remap01(initialDistance + offSetInitialDistance, distanceToStick));
	}

	// Token: 0x0600005C RID: 92 RVA: 0x000050F8 File Offset: 0x000032F8
	public static bool DidPlayerParry(CollisionInstance collisionInstance)
	{
		ColliderGroup sourceColliderGroup = collisionInstance.sourceColliderGroup;
		Object @object;
		if (sourceColliderGroup == null)
		{
			@object = null;
		}
		else
		{
			Item item = sourceColliderGroup.collisionHandler.item;
			if (item == null)
			{
				@object = null;
			}
			else
			{
				RagdollHand mainHandler = item.mainHandler;
				@object = ((mainHandler != null) ? mainHandler.creature.player : null);
			}
		}
		bool flag = @object;
		bool flag2;
		if (flag)
		{
			flag2 = true;
		}
		else
		{
			ColliderGroup targetColliderGroup = collisionInstance.targetColliderGroup;
			Object object2;
			if (targetColliderGroup == null)
			{
				object2 = null;
			}
			else
			{
				Item item2 = targetColliderGroup.collisionHandler.item;
				if (item2 == null)
				{
					object2 = null;
				}
				else
				{
					RagdollHand mainHandler2 = item2.mainHandler;
					object2 = ((mainHandler2 != null) ? mainHandler2.creature.player : null);
				}
			}
			bool flag3 = !object2;
			flag2 = !flag3;
		}
		return flag2;
	}

	// Token: 0x0600005D RID: 93 RVA: 0x00005190 File Offset: 0x00003390
	public static void ThrowFireball(this Vector3 origin, Vector3 directionToShoot, float forceOfThrow = 30f, float distanceToShootFrom = 1f)
	{
		Vector3 positionToSpawn = origin + directionToShoot.normalized * (distanceToShootFrom + 0.15f);
		Catalog.GetData<ItemData>("DynamicProjectile", true).SpawnAsync(delegate(Item projectile)
		{
			projectile.disallowDespawn = true;
			projectile.rb.useGravity = false;
			projectile.rb.velocity = Vector3.zero;
			foreach (CollisionHandler collisionHandler in projectile.collisionHandlers)
			{
				foreach (Damager damager in collisionHandler.damagers)
				{
					damager.Load(Catalog.GetData<DamagerData>("Fireball", true), collisionHandler);
				}
			}
			ItemMagicProjectile component = projectile.GetComponent<ItemMagicProjectile>();
			bool flag = component;
			if (flag)
			{
				component.guidance = 0;
				component.speed = 0f;
				component.allowDeflect = true;
				component.deflectEffectData = Catalog.GetData<EffectData>("HitFireBallDeflect", true);
				component.Fire(directionToShoot * forceOfThrow, Catalog.GetData<EffectData>("SpellFireball", true), null, null);
			}
			projectile.isThrowed = true;
			projectile.isFlying = true;
			projectile.Throw(1f, 2);
		}, new Vector3?(positionToSpawn), new Quaternion?(Quaternion.LookRotation(directionToShoot, Vector3.up)), null, true, null);
	}

	// Token: 0x0600005E RID: 94 RVA: 0x0000520C File Offset: 0x0000340C
	public static void ThrowMeteor(this Vector3 origin, Vector3 directionToShoot, Creature thrower, bool useGravity = true, float factorOfThrow = 1f, float distanceToShootFrom = 0.5f, bool ignoreCollision = false)
	{
		Item meteor = new Item();
		EffectData meteorEffectData = Catalog.GetData<EffectData>("Meteor", true);
		EffectData meteorExplosionEffectData = Catalog.GetData<EffectData>("MeteorExplosion", true);
		float meteorVelocity = 7f;
		float meteorExplosionDamage = 20f;
		float meteorExplosionPlayerDamage = 20f;
		float meteorExplosionRadius = 10f;
		AnimationCurve meteorIntensityCurve = AnimationCurve.EaseInOut(0f, 0f, 0.5f, 1f);
		SpellCastCharge meteorImbueSpellData = Catalog.GetData<SpellCastCharge>("Fire", true);
		Vector3 positionToSpawn = origin + directionToShoot.normalized * (distanceToShootFrom + 0.15f);
		ItemMagicAreaProjectile projectile;
		ItemMagicAreaProjectile.CreatureAreaHit <>9__3;
		ItemMagicAreaProjectile.Hit <>9__4;
		Catalog.GetData<ItemData>("Meteor", true).SpawnAsync(delegate(Item item)
		{
			item.disallowDespawn = true;
			item.rb.useGravity = useGravity;
			item.IgnoreCollision(ignoreCollision);
			ItemMagicAreaProjectile component = item.GetComponent<ItemMagicAreaProjectile>();
			bool flag = component != null;
			if (flag)
			{
				projectile = component;
				component.explosionEffectData = Catalog.GetData<EffectData>("MeteorExplosion", true);
				component.areaRadius = meteorExplosionRadius;
				component.OnHandlerHit += delegate(CollisionInstance hit, CollisionHandler handler)
				{
					bool flag2 = !handler.isItem;
					if (!flag2)
					{
						Snippet.MeteorImbueItem(hit.targetColliderGroup);
					}
				};
				component.OnHandlerAreaHit += delegate(Collider collider, CollisionHandler handler)
				{
					bool flag2 = !handler.isItem;
					if (!flag2)
					{
						Snippet.MeteorImbueItem(collider.GetComponentInParent<ColliderGroup>());
					}
				};
				ItemMagicAreaProjectile itemMagicAreaProjectile = component;
				ItemMagicAreaProjectile.CreatureAreaHit creatureAreaHit;
				if ((creatureAreaHit = <>9__3) == null)
				{
					creatureAreaHit = (<>9__3 = delegate(Collider collider, Creature creature)
					{
						creature.Damage(new CollisionInstance(new DamageStruct(4, creature.isPlayer ? meteorExplosionPlayerDamage : meteorExplosionDamage), null, null));
					});
				}
				itemMagicAreaProjectile.OnCreatureAreaHit += creatureAreaHit;
				ItemMagicAreaProjectile itemMagicAreaProjectile2 = component;
				ItemMagicAreaProjectile.Hit hit2;
				if ((hit2 = <>9__4) == null)
				{
					hit2 = (<>9__4 = delegate(CollisionInstance collision)
					{
						collision.contactPoint.MeteorExplosion(meteorExplosionRadius, thrower);
					});
				}
				itemMagicAreaProjectile2.OnHit += hit2;
				component.guidance = 0;
				component.guidanceAmount = 0f;
				component.speed = meteorVelocity;
				component.effectIntensityCurve = meteorIntensityCurve;
				item.rb.AddForce(directionToShoot * meteorVelocity * factorOfThrow, 1);
				component.Fire(directionToShoot, meteorEffectData, null, Player.currentCreature.ragdoll);
			}
			meteor = item;
		}, new Vector3?(positionToSpawn), new Quaternion?(Quaternion.LookRotation(directionToShoot, Vector3.up)), null, true, null);
	}

	// Token: 0x0600005F RID: 95 RVA: 0x0000531E File Offset: 0x0000351E
	private static void MeteorImbueItem(ColliderGroup group)
	{
		if (group != null)
		{
			Imbue imbue = group.imbue;
			if (imbue != null)
			{
				imbue.Transfer(Catalog.GetData<SpellCastCharge>("Fire", true), group.imbue.maxEnergy * 2f);
			}
		}
	}

	// Token: 0x06000060 RID: 96 RVA: 0x00005354 File Offset: 0x00003554
	private static void MeteorExplosion(this Vector3 position, float radius, Creature thrower)
	{
		HashSet<Rigidbody> rigidbodySet = new HashSet<Rigidbody>();
		HashSet<Creature> hitCreatures = new HashSet<Creature>();
		float meteorExplosionForce = 20f;
		float meteorExplosionPlayerForce = 5f;
		LayerMask explosionLayerMask = 232799233;
		foreach (Collider collider in Physics.OverlapSphere(position, radius, explosionLayerMask, 1))
		{
			bool flag = collider.attachedRigidbody && !rigidbodySet.Contains(collider.attachedRigidbody);
			if (flag)
			{
				float explosionForce = meteorExplosionForce;
				Creature componentInParent = collider.attachedRigidbody.GetComponentInParent<Creature>();
				bool flag2 = componentInParent != null && componentInParent != thrower && !componentInParent.isKilled && !componentInParent.isPlayer && !hitCreatures.Contains(componentInParent);
				if (flag2)
				{
					componentInParent.ragdoll.SetState(1);
					hitCreatures.Add(componentInParent);
				}
				bool flag3 = collider.attachedRigidbody.GetComponentInParent<Player>() != null;
				if (flag3)
				{
					explosionForce = meteorExplosionPlayerForce;
				}
				rigidbodySet.Add(collider.attachedRigidbody);
				collider.attachedRigidbody.AddExplosionForce(explosionForce, position, radius, 1f, 2);
			}
		}
	}

	// Token: 0x06000061 RID: 97 RVA: 0x0000548C File Offset: 0x0000368C
	public static float PingPongValue(float min, float max, float speed)
	{
		return Mathf.Lerp(min, max, Mathf.PingPong(Time.time * speed, 1f));
	}

	// Token: 0x06000062 RID: 98 RVA: 0x000054B8 File Offset: 0x000036B8
	public static AnimationCurve CurveSinSpinReverseRadius()
	{
		return new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f, 0f, 15f),
			new Keyframe(0.25f, 0.15f, -15f, -15f),
			new Keyframe(0.5f, 0f, 15f, 15f),
			new Keyframe(0.75f, -0.15f, -15f, -15f),
			new Keyframe(1f, 0f, 15f, 15f)
		});
	}

	// Token: 0x06000063 RID: 99 RVA: 0x00005578 File Offset: 0x00003778
	public static AnimationCurve CurveSinSpinReverseSpeed()
	{
		return new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f, 0f, 0f),
			new Keyframe(0.25f, 1f, 0f, 0f),
			new Keyframe(0.5f, 0f, 0.75f, 0.75f),
			new Keyframe(0.75f, -1f, 0f, 0f),
			new Keyframe(1f, 0f, 0f, 0f)
		});
	}

	// Token: 0x06000064 RID: 100 RVA: 0x00005638 File Offset: 0x00003838
	public static AnimationCurve CurveSlowDown()
	{
		return new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f, 0f, 5f),
			new Keyframe(0.25f, 0.75f, 1f, 1f),
			new Keyframe(1f, 1f, 0f, 0f)
		});
	}

	// Token: 0x06000065 RID: 101 RVA: 0x000056B8 File Offset: 0x000038B8
	public static void SlowDownFallCreature(Creature creature = null, float factor = 3f, float gravityValue = 9.81f)
	{
		AnimationCurve curve = Snippet.CurveSlowDown();
		bool flag = creature == null;
		if (flag)
		{
			Player.local.locomotion.rb.AddForce(new Vector3(0f, curve.Evaluate(Mathf.InverseLerp(0f, gravityValue, -Player.local.locomotion.velocity.y)) * gravityValue * factor, 0f), 5);
		}
		else
		{
			creature.locomotion.rb.AddForce(new Vector3(0f, curve.Evaluate(Mathf.InverseLerp(0f, gravityValue, -creature.locomotion.velocity.y)) * gravityValue * factor, 0f), 5);
		}
	}

	// Token: 0x06000066 RID: 102 RVA: 0x00005774 File Offset: 0x00003974
	public static bool MoveRightHandCloserToCenterOfBodyFast()
	{
		return Player.local.creature.handRight.rb.velocity.sqrMagnitude > 10f && Vector3.SignedAngle(Player.local.creature.transform.forward, Vector3.Cross(Player.local.creature.handRight.rb.velocity, Player.local.creature.transform.right), Player.local.transform.forward) < 90f;
	}

	// Token: 0x06000067 RID: 103 RVA: 0x00005814 File Offset: 0x00003A14
	public static bool MoveLeftHandCloserToCenterOfBodyFast()
	{
		return Player.local.creature.handLeft.rb.velocity.sqrMagnitude > 10f && Vector3.SignedAngle(Player.local.creature.transform.forward, Vector3.Cross(Player.local.creature.handLeft.rb.velocity, Player.local.creature.transform.right), Player.local.transform.forward) < 90f;
	}

	// Token: 0x06000068 RID: 104 RVA: 0x000058B4 File Offset: 0x00003AB4
	public static bool MoveBothHandCloserToCenterOfBodyFast()
	{
		return Snippet.MoveLeftHandCloserToCenterOfBodyFast() && Snippet.MoveRightHandCloserToCenterOfBodyFast();
	}

	// Token: 0x06000069 RID: 105 RVA: 0x000058D8 File Offset: 0x00003AD8
	public static bool BothHandAligned(float distance = 0.75f)
	{
		return Vector3.Dot(Player.local.handLeft.ragdollHand.PointDir(), Player.local.handRight.ragdollHand.PointDir()) > -1f && Vector3.Dot(Player.local.handLeft.ragdollHand.PointDir(), Player.local.handRight.ragdollHand.PointDir()) < -0.5f && Vector3.Distance(Player.local.handLeft.ragdollHand.transform.position, Player.local.handRight.ragdollHand.transform.position) > distance;
	}

	// Token: 0x0600006A RID: 106 RVA: 0x00005990 File Offset: 0x00003B90
	public static float SpeedSinSpinReverse(this float speed, float speedStrength = 480f, float timeLapse = 5f)
	{
		AnimationCurve curve = Snippet.CurveSinSpinReverseSpeed();
		float factorSpeed = curve.Evaluate(Time.time / timeLapse % 1f) * speedStrength;
		return speed += Time.fixedDeltaTime * factorSpeed;
	}

	// Token: 0x0600006B RID: 107 RVA: 0x000059CC File Offset: 0x00003BCC
	public static float RadiusSinSpinReverse(this float radius, float radiusStrength = 0.3f, float timeLapse = 5f)
	{
		AnimationCurve curve = Snippet.CurveSinSpinReverseRadius();
		float factorRadius = curve.Evaluate(Time.time / timeLapse % 1f) * radiusStrength;
		return radius += Time.fixedDeltaTime * factorRadius;
	}

	// Token: 0x0600006C RID: 108 RVA: 0x00005A08 File Offset: 0x00003C08
	public static void ImbueItem(this Item item, string ID)
	{
		SpellCastCharge magic = Catalog.GetData<SpellCastCharge>(ID, true);
		foreach (Imbue imbue in item.imbues)
		{
			bool flag = imbue.energy < imbue.maxEnergy;
			if (flag)
			{
				imbue.Transfer(magic, imbue.maxEnergy);
			}
		}
	}

	// Token: 0x0600006D RID: 109 RVA: 0x00005A84 File Offset: 0x00003C84
	public static string returnImbueId(this Item item)
	{
		string imbueId = null;
		foreach (Imbue imbue in item.imbues)
		{
			bool flag = imbue.energy > 0f;
			if (flag)
			{
				imbueId = imbue.spellCastBase.id;
			}
		}
		return imbueId;
	}

	// Token: 0x0600006E RID: 110 RVA: 0x00005B00 File Offset: 0x00003D00
	public static void UnImbueItem(this Item item)
	{
		foreach (Imbue imbue in item.imbues)
		{
			bool flag = imbue.energy < 0f;
			if (flag)
			{
				imbue.energy = 0f;
			}
		}
	}

	// Token: 0x0600006F RID: 111 RVA: 0x00005B70 File Offset: 0x00003D70
	public static bool imbueBelowLevelItem(this Item item, float level)
	{
		bool levelBelowOK = false;
		foreach (Imbue imbue in item.imbues)
		{
			bool flag = imbue.energy < level;
			if (flag)
			{
				levelBelowOK = true;
				break;
			}
		}
		return levelBelowOK;
	}

	// Token: 0x06000070 RID: 112 RVA: 0x00005BDC File Offset: 0x00003DDC
	public static Color HDRColor(Color color, float intensity)
	{
		return color * Mathf.Pow(2f, intensity);
	}

	// Token: 0x06000071 RID: 113 RVA: 0x00005C00 File Offset: 0x00003E00
	public static GameObject CreateDebugPoint(bool useLineRenderer = true, Light light = null)
	{
		GameObject gameObject = GameObject.CreatePrimitive(0);
		gameObject.transform.localScale = Vector3.one * 0.1f;
		if (useLineRenderer)
		{
			bool flag = light != null;
			Color color;
			if (flag)
			{
				bool flag2 = light.type == 0;
				if (flag2)
				{
					color..ctor(255f, 0f, 0f);
				}
				else
				{
					bool flag3 = light.type == 2;
					if (flag3)
					{
						color..ctor(255f, 127f, 0f);
					}
					else
					{
						bool flag4 = light.type == 3;
						if (flag4)
						{
							color..ctor(255f, 0f, 255f);
						}
						else
						{
							bool flag5 = light.type == 1;
							if (flag5)
							{
								color..ctor(0f, 0f, 255f);
							}
							else
							{
								color..ctor(0f, 127f, 255f);
							}
						}
					}
				}
			}
			else
			{
				color..ctor(255f, 255f, 255f);
			}
			LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
			lineRenderer.startWidth = 0.003f;
			lineRenderer.endWidth = 0.003f;
			lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
			lineRenderer.startColor = color;
			lineRenderer.endColor = color;
		}
		return gameObject;
	}

	// Token: 0x06000072 RID: 114 RVA: 0x00005D74 File Offset: 0x00003F74
	public static void AddDebugPointToGO(this GameObject gameObject, Color colorParam, bool useLineRenderer = true, Light light = null)
	{
		if (useLineRenderer)
		{
			bool flag = light != null;
			if (flag)
			{
				bool flag2 = light.type == 0;
				if (flag2)
				{
					Color color;
					color..ctor(255f, 0f, 0f);
				}
				else
				{
					bool flag3 = light.type == 2;
					if (flag3)
					{
						Color color;
						color..ctor(255f, 127f, 0f);
					}
					else
					{
						bool flag4 = light.type == 3;
						if (flag4)
						{
							Color color;
							color..ctor(255f, 0f, 255f);
						}
						else
						{
							bool flag5 = light.type == 1;
							if (flag5)
							{
								Color color;
								color..ctor(0f, 0f, 255f);
							}
							else
							{
								Color color;
								color..ctor(0f, 127f, 255f);
							}
						}
					}
				}
			}
			else
			{
				Color color;
				color..ctor(255f, 255f, 255f);
			}
			LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
			lineRenderer.startWidth = 0.003f;
			lineRenderer.endWidth = 0.003f;
			lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
			lineRenderer.startColor = colorParam;
			lineRenderer.endColor = colorParam;
		}
	}

	// Token: 0x06000073 RID: 115 RVA: 0x00005EC4 File Offset: 0x000040C4
	public static void RefreshDebugPointOfGO(this GameObject gameObject)
	{
		if (gameObject != null)
		{
			LineRenderer component = gameObject.GetComponent<LineRenderer>();
			if (component != null)
			{
				component.SetPosition(0, gameObject.transform.position);
			}
		}
		if (gameObject != null)
		{
			LineRenderer component2 = gameObject.GetComponent<LineRenderer>();
			if (component2 != null)
			{
				component2.SetPosition(1, Player.local.handRight.ragdollHand.fingerIndex.tip.position);
			}
		}
	}

	// Token: 0x06000074 RID: 116 RVA: 0x00005F2C File Offset: 0x0000412C
	public static void AddHolderPoint(Item item, Vector3 position)
	{
		GameObject GO = new GameObject("HolderPoint");
		GO.transform.SetParent(item.gameObject.transform);
		GO.transform.position = Vector3.zero;
		GO.transform.localPosition = position;
		GO.transform.rotation = Quaternion.Euler(90f, 180f, 0f);
		item.holderPoint = GO.transform;
	}

	// Token: 0x06000075 RID: 117 RVA: 0x00005FA8 File Offset: 0x000041A8
	public static Damager AddDamager(this Item item, string damagerName, string colliderGroupName)
	{
		GameObject GO = new GameObject(damagerName);
		GO.transform.SetParent(item.gameObject.transform);
		Damager damager = GO.gameObject.AddComponent<Damager>();
		return item.SetDamager(damager, damagerName, colliderGroupName, 0, 0f, 0f, false);
	}

	// Token: 0x06000076 RID: 118 RVA: 0x00005FFC File Offset: 0x000041FC
	private static Damager SetDamager(this Item item, Damager damager, string damagerName, string colliderGroupName, Damager.Direction direction = 0, float penetrationLength = 0f, float penetrationDepth = 0f, bool penetrationExitOnMaxDepth = false)
	{
		damager.name = damagerName;
		damager.colliderGroup = item.colliderGroups.Where((ColliderGroup colliderGroup) => colliderGroup.name == colliderGroupName).FirstOrDefault<ColliderGroup>();
		damager.direction = direction;
		damager.penetrationLength = penetrationLength;
		damager.penetrationDepth = penetrationDepth;
		damager.penetrationExitOnMaxDepth = penetrationExitOnMaxDepth;
		return damager;
	}

	// Token: 0x06000077 RID: 119 RVA: 0x00006068 File Offset: 0x00004268
	public static Holder AddHolderSlots(this Item item, string holderName, string interactableID, Vector3 touchCenter, Vector3 positionOnItem, Interactable.HandSide allowedHandSide = 0, float axisLength = 0f, Holder.DrawSlot drawSlot = 0, float touchRadius = 0.04f, bool useAnchor = true, int nbSlots = 1, float spacingSlots = 0.04f)
	{
		GameObject GO = new GameObject(holderName);
		GO.transform.SetParent(item.gameObject.transform);
		GO.transform.localPosition = positionOnItem;
		Holder holderSlot = GO.gameObject.AddComponent<Holder>();
		return holderSlot.SetHolderSlots(interactableID, touchCenter, allowedHandSide, axisLength, drawSlot, touchRadius, useAnchor, nbSlots, spacingSlots);
	}

	// Token: 0x06000078 RID: 120 RVA: 0x000060CC File Offset: 0x000042CC
	private static Holder SetHolderSlots(this Holder holder, string interactableID, Vector3 touchCenter, Interactable.HandSide allowedHandSide = 0, float axisLength = 0f, Holder.DrawSlot drawSlot = 0, float touchRadius = 0.04f, bool useAnchor = true, int nbSlots = 1, float spacingSlots = 0.04f)
	{
		holder.interactableId = interactableID;
		holder.allowedHandSide = allowedHandSide;
		holder.axisLength = axisLength;
		holder.touchRadius = touchRadius;
		holder.touchCenter = touchCenter;
		holder.drawSlot = drawSlot;
		holder.useAnchor = useAnchor;
		holder.SetSlots(nbSlots, spacingSlots, 2);
		return holder;
	}

	// Token: 0x06000079 RID: 121 RVA: 0x00006120 File Offset: 0x00004320
	private static Holder SetSlots(this Holder holder, int nbSlots, float spacing, int axe = 2)
	{
		holder.slots = new List<Transform>();
		for (int i = 0; i < nbSlots; i++)
		{
			GameObject slot = new GameObject(string.Format("Slot{0}", i + 1));
			slot.transform.SetParent(holder.gameObject.transform);
			slot.transform.localRotation = holder.gameObject.transform.rotation * Quaternion.Euler(90f, 180f, -180f);
			slot.transform.localPosition = new Vector3((axe == 1) ? (spacing * (float)i) : 0f, (axe == 2) ? (spacing * (float)i) : 0f, (axe == 3) ? (spacing * (float)i) : 0f);
			holder.slots.Add(slot.transform);
		}
		return holder;
	}

	// Token: 0x04000001 RID: 1
	public static Vector3 zero = Vector3.zero;

	// Token: 0x04000002 RID: 2
	public static Vector3 one = Vector3.one;

	// Token: 0x04000003 RID: 3
	public static Vector3 forward = Vector3.forward;

	// Token: 0x04000004 RID: 4
	public static Vector3 right = Vector3.right;

	// Token: 0x04000005 RID: 5
	public static Vector3 up = Vector3.up;

	// Token: 0x04000006 RID: 6
	public static Vector3 back = Vector3.back;

	// Token: 0x04000007 RID: 7
	public static Vector3 left = Vector3.left;

	// Token: 0x04000008 RID: 8
	public static Vector3 down = Vector3.down;

	// Token: 0x0200000A RID: 10
	public class PenetrateItem : CollisionHandler
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060000A6 RID: 166 RVA: 0x0000885C File Offset: 0x00006A5C
		// (remove) Token: 0x060000A7 RID: 167 RVA: 0x00008894 File Offset: 0x00006A94
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Snippet.PenetrateItem.PenetrateEvent OnPenetrateStart;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060000A8 RID: 168 RVA: 0x000088CC File Offset: 0x00006ACC
		// (remove) Token: 0x060000A9 RID: 169 RVA: 0x00008904 File Offset: 0x00006B04
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Snippet.PenetrateItem.PenetrateEvent OnPenetrateStop;

		// Token: 0x060000AA RID: 170 RVA: 0x0000893C File Offset: 0x00006B3C
		public void InvokePenetrateStart(CollisionInstance collisionInstance)
		{
			Snippet.PenetrateItem.PenetrateEvent penetrateStartEvent = this.OnPenetrateStart;
			bool flag = penetrateStartEvent == null;
			if (!flag)
			{
				penetrateStartEvent(collisionInstance);
				this.isPenetrating = true;
			}
		}

		// Token: 0x060000AB RID: 171 RVA: 0x0000896C File Offset: 0x00006B6C
		public void InvokePenetrateStop(CollisionInstance collisionInstance)
		{
			Snippet.PenetrateItem.PenetrateEvent penetrateStopEvent = this.OnPenetrateStop;
			bool flag = penetrateStopEvent == null;
			if (!flag)
			{
				penetrateStopEvent(collisionInstance);
				this.isPenetrating = false;
			}
		}

		// Token: 0x060000AC RID: 172 RVA: 0x0000899A File Offset: 0x00006B9A
		protected override void ManagedOnEnable()
		{
			base.ManagedOnEnable();
			this.isPenetrating = false;
			base.OnCollisionStartEvent += new CollisionHandler.CollisionEvent(this.PenetrateItem_OnCollisionStartEvent);
			base.OnCollisionStopEvent += new CollisionHandler.CollisionEvent(this.PenetrateItem_OnCollisionStopEvent);
		}

		// Token: 0x060000AD RID: 173 RVA: 0x000089D4 File Offset: 0x00006BD4
		private void PenetrateItem_OnCollisionStartEvent(CollisionInstance collisionInstance)
		{
			bool flag = !this.isPenetrating;
			if (flag)
			{
				this.InvokePenetrateStart(collisionInstance);
			}
		}

		// Token: 0x060000AE RID: 174 RVA: 0x000089FC File Offset: 0x00006BFC
		private void PenetrateItem_OnCollisionStopEvent(CollisionInstance collisionInstance)
		{
			bool flag = this.isPenetrating;
			if (flag)
			{
				this.InvokePenetrateStart(collisionInstance);
			}
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00008A1E File Offset: 0x00006C1E
		protected override void ManagedOnDisable()
		{
			base.ManagedOnDisable();
			this.isPenetrating = false;
			base.OnCollisionStartEvent -= new CollisionHandler.CollisionEvent(this.PenetrateItem_OnCollisionStartEvent);
			base.OnCollisionStopEvent -= new CollisionHandler.CollisionEvent(this.PenetrateItem_OnCollisionStopEvent);
		}

		// Token: 0x04000076 RID: 118
		private bool isPenetrating;

		// Token: 0x0200001C RID: 28
		// (Invoke) Token: 0x06000102 RID: 258
		public delegate void PenetrateEvent(CollisionInstance collisionInstance);
	}

	// Token: 0x0200000B RID: 11
	public enum Step
	{
		// Token: 0x04000078 RID: 120
		Enter,
		// Token: 0x04000079 RID: 121
		Update,
		// Token: 0x0400007A RID: 122
		Exit
	}

	// Token: 0x0200000C RID: 12
	public class Zone : MonoBehaviour
	{
		// Token: 0x060000B1 RID: 177 RVA: 0x00008A60 File Offset: 0x00006C60
		public void Start()
		{
			this.handlers = new HashSet<CollisionHandler>();
			this.creatures = new HashSet<Creature>();
			this.items = new HashSet<Item>();
			base.gameObject.layer = GameManager.GetLayer(7);
			this.collider = base.gameObject.AddComponent<CapsuleCollider>();
			this.collider.center = Vector3.forward * (this.distance / 2f - 0.3f);
			this.collider.radius = this.radius;
			this.collider.height = this.distance + 0.3f;
			this.collider.direction = 2;
			this.collider.isTrigger = true;
			this.Begin();
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00008B25 File Offset: 0x00006D25
		public virtual void Begin()
		{
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00008B28 File Offset: 0x00006D28
		public void Update()
		{
			foreach (CollisionHandler handler in this.handlers)
			{
				this.OnHandlerEvent(handler, Snippet.Step.Update);
			}
			foreach (Item item in this.items)
			{
				this.OnItemEvent(item, Snippet.Step.Update);
			}
			foreach (Creature creature in this.creatures)
			{
				this.CreatureEvent(creature, Snippet.Step.Update);
			}
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00008C10 File Offset: 0x00006E10
		public void Despawn()
		{
			foreach (CollisionHandler handler in this.handlers)
			{
				this.OnHandlerEvent(handler, Snippet.Step.Exit);
			}
			foreach (Item item in this.items)
			{
				this.OnItemEvent(item, Snippet.Step.Exit);
			}
			foreach (Creature creature in this.creatures)
			{
				this.CreatureEvent(creature, Snippet.Step.Exit);
			}
			this.OnDespawn();
			Object.Destroy(base.gameObject);
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00008D0C File Offset: 0x00006F0C
		public virtual void OnDespawn()
		{
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00008D10 File Offset: 0x00006F10
		public void OnTriggerEnter(Collider collider)
		{
			Rigidbody attachedRigidbody = collider.attachedRigidbody;
			CollisionHandler handler = ((attachedRigidbody != null) ? attachedRigidbody.GetComponent<CollisionHandler>() : null);
			bool flag = !handler || collider.attachedRigidbody.isKinematic;
			if (!flag)
			{
				bool flag2 = !this.handlers.Contains(handler);
				if (flag2)
				{
					this.handlers.Add(handler);
					RagdollPart ragdollPart = handler.ragdollPart;
					bool flag3 = ragdollPart == null || !ragdollPart.ragdoll.creature.isPlayer;
					if (flag3)
					{
						this.OnHandlerEvent(handler, Snippet.Step.Enter);
					}
				}
				Item item = handler.item;
				bool flag4 = item != null;
				if (flag4)
				{
					bool flag5 = !this.items.Contains(item);
					if (flag5)
					{
						this.OnItemEvent(item, Snippet.Step.Enter);
						this.items.Add(item);
					}
				}
				RagdollPart ragdollPart2 = handler.ragdollPart;
				Creature creature = ((ragdollPart2 != null) ? ragdollPart2.ragdoll.creature : null);
				bool flag6 = creature != null;
				if (flag6)
				{
					bool flag7 = !this.creatures.Contains(creature);
					if (flag7)
					{
						this.creatures.Add(creature);
						this.CreatureEvent(creature, Snippet.Step.Enter);
					}
				}
			}
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00008E38 File Offset: 0x00007038
		public bool HasItemHandlers(Item item)
		{
			return this.handlers.Any((CollisionHandler handler) => handler.item == item);
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00008E6C File Offset: 0x0000706C
		public bool HasCreatureHandlers(Creature creature)
		{
			return this.handlers.Any(delegate(CollisionHandler handler)
			{
				RagdollPart ragdollPart = handler.ragdollPart;
				return ((ragdollPart != null) ? ragdollPart.ragdoll.creature : null) == creature;
			});
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00008EA0 File Offset: 0x000070A0
		public void OnTriggerExit(Collider collider)
		{
			Rigidbody attachedRigidbody = collider.attachedRigidbody;
			CollisionHandler handler = ((attachedRigidbody != null) ? attachedRigidbody.GetComponent<CollisionHandler>() : null);
			bool flag = !handler || collider.attachedRigidbody.isKinematic;
			if (!flag)
			{
				bool flag2 = this.handlers.Contains(handler);
				if (flag2)
				{
					this.handlers.Remove(handler);
					RagdollPart ragdollPart = handler.ragdollPart;
					bool flag3 = ragdollPart == null || !ragdollPart.ragdoll.creature.isPlayer;
					if (flag3)
					{
						this.OnHandlerEvent(handler, Snippet.Step.Exit);
					}
				}
				Item item = handler.item;
				bool flag4 = item != null && this.items.Contains(item) && !this.HasItemHandlers(item);
				if (flag4)
				{
					this.items.Remove(item);
					this.OnItemEvent(item, Snippet.Step.Exit);
				}
				RagdollPart ragdollPart2 = handler.ragdollPart;
				Creature creature = ((ragdollPart2 != null) ? ragdollPart2.ragdoll.creature : null);
				bool flag5 = creature != null && this.creatures.Contains(creature) && !this.HasCreatureHandlers(creature);
				if (flag5)
				{
					this.creatures.Remove(creature);
					this.CreatureEvent(creature, Snippet.Step.Exit);
				}
			}
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00008FC8 File Offset: 0x000071C8
		public void CreatureEvent(Creature creature, Snippet.Step step)
		{
			this.OnCreatureEvent(creature, step);
			bool isPlayer = creature.isPlayer;
			if (isPlayer)
			{
				this.OnPlayerEvent(step);
			}
			else
			{
				this.OnNPCEvent(creature, step);
			}
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00009000 File Offset: 0x00007200
		public virtual void OnNPCEvent(Creature creature, Snippet.Step step)
		{
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00009003 File Offset: 0x00007203
		public virtual void OnPlayerEvent(Snippet.Step step)
		{
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00009006 File Offset: 0x00007206
		public virtual void OnCreatureEvent(Creature creature, Snippet.Step step)
		{
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00009009 File Offset: 0x00007209
		public virtual void OnItemEvent(Item item, Snippet.Step step)
		{
		}

		// Token: 0x060000BF RID: 191 RVA: 0x0000900C File Offset: 0x0000720C
		public virtual void OnHandlerEvent(CollisionHandler handler, Snippet.Step step)
		{
		}

		// Token: 0x0400007B RID: 123
		public float distance;

		// Token: 0x0400007C RID: 124
		public float radius;

		// Token: 0x0400007D RID: 125
		public CapsuleCollider collider;

		// Token: 0x0400007E RID: 126
		private HashSet<CollisionHandler> handlers;

		// Token: 0x0400007F RID: 127
		private HashSet<Creature> creatures;

		// Token: 0x04000080 RID: 128
		private HashSet<Item> items;
	}

	// Token: 0x0200000D RID: 13
	public class FreezeBehaviour : MonoBehaviour
	{
		// Token: 0x060000C1 RID: 193 RVA: 0x00009018 File Offset: 0x00007218
		public void Awake()
		{
			this.creature = base.GetComponent<Creature>();
			this.creature.OnDespawnEvent += delegate(EventTime time)
			{
				bool flag = time == 0;
				if (flag)
				{
					this.Disable();
				}
			};
			this.orgAnimatorSpeed = this.creature.animator.speed;
			this.orgSpeakPitchRange = this.creature.ragdoll.creature.brain.instance.GetModule<BrainModuleSpeak>(true).audioPitchRange;
			this.orgColorHair = this.creature.GetColor(0);
			this.orgColorHairSecondary = this.creature.GetColor(1);
			this.orgColorHairSpecular = this.creature.GetColor(2);
			this.orgColorEyesIris = this.creature.GetColor(3);
			this.orgColorEyesSclera = this.creature.GetColor(4);
			this.orgColorSkin = this.creature.GetColor(5);
			this.timerSlow = 1f;
			this.totalTimeOfFreezeRagdoll = Random.Range(7f, 10f);
			this.timerFreeze = this.totalTimeOfFreezeRagdoll - this.timeOfFreezing;
			this.targetColorHair = this.colorFreeze;
			this.targetColorHairSecondary = this.colorFreeze;
			this.targetColorHairSpecular = this.colorFreeze;
			this.targetColorEyesIris = this.colorFreeze;
			this.targetColorEyesSclera = this.colorFreeze;
			this.targetColorSkin = this.colorFreeze;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00009174 File Offset: 0x00007374
		public void UpdateSpeed()
		{
			this.creature.animator.speed = this.animatorSpeed;
			this.creature.ragdoll.creature.brain.instance.GetModule<BrainModuleSpeak>(true).audioPitchRange = this.speakPitchRange;
			this.creature.SetColor(this.colorHair, 0, true);
			this.creature.SetColor(this.colorHairSecondary, 1, true);
			this.creature.SetColor(this.colorHairSpecular, 2, true);
			this.creature.SetColor(this.colorEyesIris, 3, true);
			this.creature.SetColor(this.colorEyesSclera, 4, true);
			this.creature.SetColor(this.colorSkin, 5, true);
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x0000923C File Offset: 0x0000743C
		public void Update()
		{
			bool flag = !this.isFrozen;
			if (flag)
			{
				this.timerSlow -= Time.deltaTime / this.timeOfFreezing;
				this.timerSlow = Mathf.Clamp(this.timerSlow, 0f, this.timeOfFreezing);
				this.animatorSpeed = Mathf.Lerp(this.targetAnimatorSpeed, this.orgAnimatorSpeed, this.timerSlow);
				this.speakPitchRange = Vector2.Lerp(this.targetSpeakPitchRange, this.orgSpeakPitchRange, this.timerSlow);
				this.blendFreezeValue = Mathf.Lerp(this.targetBlendFreezeValue, this.orgBlendFreezeValue, this.timerSlow);
				this.colorHair = Color.Lerp(this.targetColorHair, this.orgColorHair, this.timerSlow);
				this.colorHairSecondary = Color.Lerp(this.targetColorHairSecondary, this.orgColorHairSecondary, this.timerSlow);
				this.colorHairSpecular = Color.Lerp(this.targetColorHairSpecular, this.orgColorHairSpecular, this.timerSlow);
				this.colorEyesIris = Color.Lerp(this.targetColorEyesIris, this.orgColorEyesIris, this.timerSlow);
				this.colorEyesSclera = Color.Lerp(this.targetColorEyesSclera, this.orgColorEyesSclera, this.timerSlow);
				this.colorSkin = Color.Lerp(this.targetColorSkin, this.orgColorSkin, this.timerSlow);
				this.UpdateSpeed();
			}
			bool flag2 = this.timerSlow <= 0f && !this.isFrozen;
			if (flag2)
			{
				this.brainId = this.creature.ragdoll.creature.brain.instance.id;
				this.isFrozen = true;
				this.creature.brain.Stop();
				this.creature.StopAnimation(false);
				this.creature.brain.StopAllCoroutines();
				this.creature.locomotion.MoveStop();
				foreach (RagdollPart ragdollPart in this.creature.ragdoll.parts)
				{
					ragdollPart.rb.constraints = 126;
				}
			}
			bool flag3 = this.isFrozen && !this.endOfFreeze;
			if (flag3)
			{
				this.timerFreeze = Mathf.Clamp(this.timerFreeze, 0f, this.timerFreeze);
				this.timerFreeze -= Time.deltaTime;
				bool flag4 = this.timerFreeze <= 0f;
				if (flag4)
				{
					this.endOfFreeze = true;
				}
			}
			bool flag5 = this.endOfFreeze;
			if (flag5)
			{
				this.Disable();
			}
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00009504 File Offset: 0x00007704
		public void Disable()
		{
			this.creature.animator.speed = this.orgAnimatorSpeed;
			this.creature.brain.Load(this.brainId);
			foreach (RagdollPart ragdollPart in this.creature.ragdoll.parts)
			{
				ragdollPart.rb.constraints = 0;
				ragdollPart.ragdoll.RemovePhysicModifier(this);
			}
			this.creature.ragdoll.creature.brain.instance.GetModule<BrainModuleSpeak>(true).audioPitchRange = this.orgSpeakPitchRange;
			this.creature.SetColor(this.orgColorHair, 0, true);
			this.creature.SetColor(this.orgColorHairSecondary, 1, true);
			this.creature.SetColor(this.orgColorHairSpecular, 2, true);
			this.creature.SetColor(this.orgColorEyesIris, 3, true);
			this.creature.SetColor(this.orgColorEyesSclera, 4, true);
			this.creature.SetColor(this.orgColorSkin, 5, true);
			Object.Destroy(this);
		}

		// Token: 0x04000081 RID: 129
		private string brainId;

		// Token: 0x04000082 RID: 130
		private float timeOfFreezing = 1.5f;

		// Token: 0x04000083 RID: 131
		private Creature creature;

		// Token: 0x04000084 RID: 132
		private float orgAnimatorSpeed;

		// Token: 0x04000085 RID: 133
		private float targetAnimatorSpeed = 0f;

		// Token: 0x04000086 RID: 134
		private float animatorSpeed;

		// Token: 0x04000087 RID: 135
		private Vector2 orgSpeakPitchRange;

		// Token: 0x04000088 RID: 136
		private Vector2 targetSpeakPitchRange = Vector2.zero;

		// Token: 0x04000089 RID: 137
		private Vector2 speakPitchRange;

		// Token: 0x0400008A RID: 138
		private float orgBlendFreezeValue = 1f;

		// Token: 0x0400008B RID: 139
		private float targetBlendFreezeValue = 0f;

		// Token: 0x0400008C RID: 140
		private float blendFreezeValue;

		// Token: 0x0400008D RID: 141
		private Color colorFreeze = new Color(0.24644f, 0.5971831f, 0.735849f);

		// Token: 0x0400008E RID: 142
		private Color orgColorHair;

		// Token: 0x0400008F RID: 143
		private Color targetColorHair = Color.cyan;

		// Token: 0x04000090 RID: 144
		private Color colorHair;

		// Token: 0x04000091 RID: 145
		private Color orgColorHairSecondary;

		// Token: 0x04000092 RID: 146
		private Color targetColorHairSecondary;

		// Token: 0x04000093 RID: 147
		private Color colorHairSecondary;

		// Token: 0x04000094 RID: 148
		private Color orgColorHairSpecular;

		// Token: 0x04000095 RID: 149
		private Color targetColorHairSpecular;

		// Token: 0x04000096 RID: 150
		private Color colorHairSpecular;

		// Token: 0x04000097 RID: 151
		private Color orgColorEyesIris;

		// Token: 0x04000098 RID: 152
		private Color targetColorEyesIris;

		// Token: 0x04000099 RID: 153
		private Color colorEyesIris;

		// Token: 0x0400009A RID: 154
		private Color orgColorEyesSclera;

		// Token: 0x0400009B RID: 155
		private Color targetColorEyesSclera;

		// Token: 0x0400009C RID: 156
		private Color colorEyesSclera;

		// Token: 0x0400009D RID: 157
		private Color orgColorSkin;

		// Token: 0x0400009E RID: 158
		private Color targetColorSkin;

		// Token: 0x0400009F RID: 159
		private Color colorSkin;

		// Token: 0x040000A0 RID: 160
		private float timerSlow;

		// Token: 0x040000A1 RID: 161
		private float timerFreeze;

		// Token: 0x040000A2 RID: 162
		private float timerFrozen;

		// Token: 0x040000A3 RID: 163
		private bool isFrozen = false;

		// Token: 0x040000A4 RID: 164
		private bool endOfFreeze = false;

		// Token: 0x040000A5 RID: 165
		private float totalTimeOfFreezeRagdoll;
	}

	// Token: 0x0200000E RID: 14
	public class SlowBehaviour : MonoBehaviour
	{
		// Token: 0x060000C7 RID: 199 RVA: 0x000096F4 File Offset: 0x000078F4
		public void Init(float start, float duration, float ratio, bool restoreVelocityAfterEffect = true, float blendDuration = 0f, bool playEffect = false)
		{
			this.timerStart = start;
			this.orgTimerStart = start;
			this.timerDuration = duration;
			this.orgTimerDuration = duration;
			this.ratioSlow = ratio;
			this.orgRatioSlow = ratio;
			this.timerBlend = blendDuration;
			this.orgTimerBlend = blendDuration;
			this.playVFX = playEffect;
			this.restoreVelocity = restoreVelocityAfterEffect;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x0000974C File Offset: 0x0000794C
		public void Awake()
		{
			this.creature = base.GetComponent<Creature>();
			this.creature.OnDespawnEvent += delegate(EventTime time)
			{
				bool flag = time == 0;
				if (flag)
				{
					this.Dispose();
				}
			};
			this.creature.OnKillEvent += new Creature.KillEvent(this.Creature_OnKillEvent);
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x0000978C File Offset: 0x0000798C
		private void Creature_OnKillEvent(CollisionInstance collisionInstance, EventTime eventTime)
		{
			bool flag = eventTime == 0;
			if (flag)
			{
				this.Dispose();
			}
		}

		// Token: 0x060000CA RID: 202 RVA: 0x000097AC File Offset: 0x000079AC
		public void Update()
		{
			bool flag = !this.hasStarted;
			if (flag)
			{
				this.timerStart -= Time.deltaTime;
				this.timerStart = Mathf.Clamp(this.timerStart, 0f, this.orgTimerStart);
				bool flag2 = this.timerStart <= 0f;
				if (flag2)
				{
					this.brainId = this.creature.ragdoll.creature.brain.instance.id;
					this.orgAnimatorSpeed = this.creature.animator.speed;
					this.orgCreatureVelocity = this.creature.locomotion.rb.velocity;
					this.orgCreatureAngularVelocity = this.creature.locomotion.rb.angularVelocity;
					this.orgCreatureVelocityPart = this.creature.ragdoll.parts.Select((RagdollPart part) => part.rb.velocity).ToList<Vector3>();
					this.orgCreatureAngularVelocityPart = this.creature.ragdoll.parts.Select((RagdollPart part) => part.rb.angularVelocity).ToList<Vector3>();
					this.orgCreatureDragPart = this.creature.ragdoll.parts.Select((RagdollPart part) => part.rb.drag).ToList<float>();
					this.orgCreatureAngularDragPart = this.creature.ragdoll.parts.Select((RagdollPart part) => part.rb.angularDrag).ToList<float>();
					this.orgLocomotionDrag = this.creature.locomotion.rb.drag;
					this.orgLocomotionAngularDrag = this.creature.locomotion.rb.angularDrag;
					this.hasStarted = true;
				}
			}
			bool flag3 = this.hasStarted && !this.isSlowed;
			if (flag3)
			{
				bool flag4 = this.orgTimerBlend != 0f;
				if (flag4)
				{
					this.timerBlend -= Time.deltaTime / this.orgTimerBlend;
					this.timerBlend = Mathf.Clamp(this.timerBlend, 0f, this.orgTimerBlend);
				}
				else
				{
					this.timerBlend = 0f;
				}
				this.creature.animator.speed = Mathf.Lerp(this.orgAnimatorSpeed * this.ratioSlow / this.factor, this.orgAnimatorSpeed, this.timerBlend);
				this.creature.locomotion.rb.velocity = new Vector3(Mathf.Lerp(this.orgCreatureVelocity.x * this.ratioSlow / this.factor, this.orgCreatureVelocity.x, this.timerBlend), Mathf.Lerp(this.orgCreatureVelocity.y * this.ratioSlow / this.factor, this.orgCreatureVelocity.y, this.timerBlend), Mathf.Lerp(this.orgCreatureVelocity.z * this.ratioSlow / this.factor, this.orgCreatureVelocity.z, this.timerBlend));
				this.creature.locomotion.rb.angularVelocity = new Vector3(Mathf.Lerp(this.orgCreatureAngularVelocity.x * this.ratioSlow / this.factor, this.orgCreatureAngularVelocity.x, this.timerBlend), Mathf.Lerp(this.orgCreatureAngularVelocity.y * this.ratioSlow / this.factor, this.orgCreatureAngularVelocity.y, this.timerBlend), Mathf.Lerp(this.orgCreatureAngularVelocity.z * this.ratioSlow / this.factor, this.orgCreatureAngularVelocity.z, this.timerBlend));
				this.creature.locomotion.rb.drag = Mathf.Lerp(this.factor * 100f, this.orgLocomotionDrag, this.timerBlend);
				this.creature.locomotion.rb.angularDrag = Mathf.Lerp(this.factor * 100f, this.orgLocomotionAngularDrag, this.timerBlend);
				for (int i = this.creature.ragdoll.parts.Count<RagdollPart>() - 1; i >= 0; i--)
				{
					this.creature.ragdoll.parts[i].ragdoll.SetPhysicModifier(this, 0f, 0f, this.factor * 100f, this.factor * 100f, null);
					this.creature.ragdoll.parts[i].rb.velocity = new Vector3(Mathf.Lerp(this.orgCreatureVelocityPart[i].x * this.ratioSlow / this.factor, this.orgCreatureVelocityPart[i].x, this.timerBlend), Mathf.Lerp(this.orgCreatureVelocityPart[i].x * this.ratioSlow / this.factor, this.orgCreatureVelocityPart[i].y, this.timerBlend), Mathf.Lerp(this.orgCreatureVelocityPart[i].x * this.ratioSlow / this.factor, this.orgCreatureVelocityPart[i].z, this.timerBlend));
					this.creature.ragdoll.parts[i].rb.angularVelocity = new Vector3(Mathf.Lerp(this.orgCreatureAngularVelocityPart[i].x * this.ratioSlow / this.factor, this.orgCreatureAngularVelocityPart[i].x, this.timerBlend), Mathf.Lerp(this.orgCreatureAngularVelocityPart[i].y * this.ratioSlow / this.factor, this.orgCreatureAngularVelocityPart[i].y, this.timerBlend), Mathf.Lerp(this.orgCreatureAngularVelocityPart[i].z * this.ratioSlow / this.factor, this.orgCreatureAngularVelocityPart[i].z, this.timerBlend));
					this.creature.ragdoll.parts[i].rb.drag = Mathf.Lerp(this.factor * 100f, this.orgCreatureDragPart[i], this.timerBlend);
					this.creature.ragdoll.parts[i].rb.angularDrag = Mathf.Lerp(this.factor * 100f, this.orgCreatureAngularDragPart[i], this.timerBlend);
				}
				bool flag5 = this.timerBlend <= 0f;
				if (flag5)
				{
					this.isSlowed = true;
					this.creature.GetPart(4).rb.freezeRotation = true;
				}
			}
			bool flag6 = this.isSlowed && !this.endOfSlow;
			if (flag6)
			{
				this.timerDuration = Mathf.Clamp(this.timerDuration, 0f, this.orgTimerDuration);
				this.timerDuration -= Time.deltaTime;
				bool flag7 = this.timerDuration <= 0f;
				if (flag7)
				{
					this.endOfSlow = true;
				}
			}
			bool flag8 = this.endOfSlow;
			if (flag8)
			{
				this.Dispose();
			}
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00009F9C File Offset: 0x0000819C
		public void Dispose()
		{
			bool flag = this.creature != null;
			if (flag)
			{
				this.creature.animator.speed = this.orgAnimatorSpeed;
				foreach (RagdollPart ragdollPart in this.creature.ragdoll.parts)
				{
					ragdollPart.ragdoll.RemovePhysicModifier(this);
				}
				this.creature.GetPart(4).rb.freezeRotation = false;
				bool flag2 = this.restoreVelocity && this.hasStarted;
				if (flag2)
				{
					this.creature.locomotion.rb.velocity = this.orgCreatureVelocity;
					this.creature.locomotion.rb.angularVelocity = this.orgCreatureAngularVelocity;
					this.creature.locomotion.rb.drag = this.orgLocomotionDrag;
					this.creature.locomotion.rb.angularDrag = this.orgLocomotionAngularDrag;
					for (int i = this.creature.ragdoll.parts.Count<RagdollPart>() - 1; i >= 0; i--)
					{
						this.creature.ragdoll.parts[i].rb.velocity = this.orgCreatureVelocityPart[i];
						this.creature.ragdoll.parts[i].rb.angularVelocity = this.orgCreatureAngularVelocityPart[i];
						this.creature.ragdoll.parts[i].rb.drag = this.orgCreatureDragPart[i];
						this.creature.ragdoll.parts[i].rb.angularDrag = this.orgCreatureAngularDragPart[i];
					}
				}
			}
			this.creature.OnKillEvent -= new Creature.KillEvent(this.Creature_OnKillEvent);
			Object.Destroy(this);
		}

		// Token: 0x040000A6 RID: 166
		private Creature creature;

		// Token: 0x040000A7 RID: 167
		private string brainId;

		// Token: 0x040000A8 RID: 168
		private float orgAnimatorSpeed;

		// Token: 0x040000A9 RID: 169
		private bool hasStarted = false;

		// Token: 0x040000AA RID: 170
		private bool isSlowed = false;

		// Token: 0x040000AB RID: 171
		private bool endOfSlow = false;

		// Token: 0x040000AC RID: 172
		private float timerStart;

		// Token: 0x040000AD RID: 173
		private float orgTimerStart;

		// Token: 0x040000AE RID: 174
		private float timerDuration;

		// Token: 0x040000AF RID: 175
		private float orgTimerDuration;

		// Token: 0x040000B0 RID: 176
		private float timerBlend;

		// Token: 0x040000B1 RID: 177
		private float orgTimerBlend;

		// Token: 0x040000B2 RID: 178
		private float ratioSlow;

		// Token: 0x040000B3 RID: 179
		private float orgRatioSlow;

		// Token: 0x040000B4 RID: 180
		private bool playVFX;

		// Token: 0x040000B5 RID: 181
		private bool restoreVelocity;

		// Token: 0x040000B6 RID: 182
		private Vector3 orgCreatureVelocity;

		// Token: 0x040000B7 RID: 183
		private Vector3 orgCreatureAngularVelocity;

		// Token: 0x040000B8 RID: 184
		private List<Vector3> orgCreatureVelocityPart;

		// Token: 0x040000B9 RID: 185
		private List<Vector3> orgCreatureAngularVelocityPart;

		// Token: 0x040000BA RID: 186
		private List<float> orgCreatureDragPart;

		// Token: 0x040000BB RID: 187
		private List<float> orgCreatureAngularDragPart;

		// Token: 0x040000BC RID: 188
		private float orgLocomotionDrag;

		// Token: 0x040000BD RID: 189
		private float orgLocomotionAngularDrag;

		// Token: 0x040000BE RID: 190
		private float factor = 10f;
	}
}
