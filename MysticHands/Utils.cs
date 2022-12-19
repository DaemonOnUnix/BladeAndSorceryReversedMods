using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ExtensionMethods;
using ThunderRoad;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x02000003 RID: 3
internal static class Utils
{
	// Token: 0x06000013 RID: 19 RVA: 0x000024B4 File Offset: 0x000006B4
	public static ConfigurableJoint CreateGrabJoint(Rigidbody source, Transform gripPoint, Side side, Item target)
	{
		bool flag = source == null || target == null;
		ConfigurableJoint configurableJoint;
		if (flag)
		{
			configurableJoint = null;
		}
		else
		{
			Handle handle = target.GetMainHandle(side);
			Vector3 targetPos = Vector3.zero;
			Quaternion targetRot = Quaternion.identity;
			bool flag2 = handle && handle.orientations.Count<HandlePose>() > 1;
			if (flag2)
			{
				HandlePose orientation = handle.GetNearestOrientation(gripPoint, side);
				float axisPosition = (((double)handle.axisLength > 0.0) ? handle.GetNearestAxisPosition(gripPoint.position) : 0f);
				targetPos = orientation.transform.position + orientation.handle.transform.up * axisPosition * orientation.handle.transform.lossyScale.y;
				targetRot = orientation.transform.rotation * Quaternion.Inverse(gripPoint.localRotation);
			}
			source.transform.position = targetPos;
			source.transform.rotation = targetRot;
			ConfigurableJoint joint = source.gameObject.AddComponent<ConfigurableJoint>();
			joint.connectedBody = target.rb;
			joint.anchor = source.transform.InverseTransformPoint(gripPoint.position);
			joint.autoConfigureConnectedAnchor = false;
			joint.connectedAnchor = target.rb.transform.InverseTransformPoint(targetPos);
			joint.enableCollision = false;
			joint.xMotion = 0;
			joint.yMotion = 0;
			joint.zMotion = 0;
			joint.angularXMotion = 0;
			joint.angularYMotion = 0;
			joint.angularZMotion = 0;
			configurableJoint = joint;
		}
		return configurableJoint;
	}

	// Token: 0x06000014 RID: 20 RVA: 0x00002658 File Offset: 0x00000858
	public static ConfigurableJoint CreateGrabJoint(Rigidbody source, Transform gripPoint, Side side, Creature target)
	{
		bool flag = source == null || target == null;
		ConfigurableJoint configurableJoint;
		if (flag)
		{
			configurableJoint = null;
		}
		else
		{
			Transform torso = target.GetTorso().transform;
			Vector3 downVector = -(target.GetChest() - torso.position).normalized;
			Vector3 targetPos = target.GetChest() + downVector * 0.4f + ((side == 1) ? (-gripPoint.right) : gripPoint.right) * 0.2f;
			Quaternion targetRot = source.transform.rotation * Quaternion.FromToRotation((side == 1) ? (-source.transform.up) : source.transform.up, target.GetChest() - torso.transform.position);
			source.transform.position = targetPos;
			source.transform.rotation = targetRot;
			ConfigurableJoint joint = source.gameObject.AddComponent<ConfigurableJoint>();
			joint.connectedBody = target.GetTorso().rb;
			joint.anchor = source.transform.InverseTransformPoint(gripPoint.position);
			joint.autoConfigureConnectedAnchor = false;
			joint.connectedAnchor = target.GetTorso().rb.transform.InverseTransformPoint(targetPos);
			joint.enableCollision = false;
			joint.xMotion = 0;
			joint.yMotion = 0;
			joint.zMotion = 0;
			joint.angularXMotion = 0;
			joint.angularYMotion = 0;
			joint.angularZMotion = 0;
			configurableJoint = joint;
		}
		return configurableJoint;
	}

	// Token: 0x06000015 RID: 21 RVA: 0x00002800 File Offset: 0x00000A00
	public static ConfigurableJoint CreateTKJoint(Rigidbody source, Rigidbody target)
	{
		ConfigurableJoint joint = source.gameObject.AddComponent<ConfigurableJoint>();
		joint.autoConfigureConnectedAnchor = false;
		joint.anchor = Vector3.zero;
		joint.connectedBody = target;
		joint.connectedAnchor = Vector3.zero;
		joint.rotationDriveMode = 0;
		joint.enableCollision = false;
		JointDrive jointDrive = default(JointDrive);
		JointDrive jointDrive2 = default(JointDrive);
		jointDrive.positionSpring = 10000f;
		jointDrive.positionDamper = 1000f;
		jointDrive.maximumForce = 100000f;
		jointDrive2.positionSpring = 10000f;
		jointDrive2.positionDamper = 2000f;
		jointDrive2.maximumForce = 100000f;
		joint.xDrive = jointDrive;
		joint.yDrive = jointDrive;
		joint.zDrive = jointDrive;
		joint.angularXDrive = jointDrive2;
		joint.angularYZDrive = jointDrive2;
		joint.xMotion = 2;
		joint.yMotion = 2;
		joint.zMotion = 2;
		joint.angularXMotion = 2;
		joint.angularYMotion = 2;
		joint.angularZMotion = 2;
		return joint;
	}

	// Token: 0x06000016 RID: 22 RVA: 0x00002910 File Offset: 0x00000B10
	public static ConfigurableJoint CreateSimpleJoint(Rigidbody source, Rigidbody target, float spring, float damper, float maxForce = float.PositiveInfinity, bool rotation = true)
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
		jointDrive.maximumForce = maxForce;
		JointDrive posDrive = jointDrive;
		jointDrive = default(JointDrive);
		jointDrive.positionSpring = 1000f;
		jointDrive.positionDamper = 10f;
		jointDrive.maximumForce = float.PositiveInfinity;
		JointDrive rotDrive = jointDrive;
		joint.rotationDriveMode = 1;
		joint.xDrive = posDrive;
		joint.yDrive = posDrive;
		joint.zDrive = posDrive;
		if (rotation)
		{
			joint.slerpDrive = rotDrive;
		}
		source.transform.rotation = orgRotation;
		joint.angularXMotion = 2;
		joint.angularYMotion = 2;
		joint.angularZMotion = 2;
		joint.xMotion = 2;
		joint.yMotion = 2;
		joint.zMotion = 2;
		return joint;
	}

	// Token: 0x06000017 RID: 23 RVA: 0x00002A54 File Offset: 0x00000C54
	public static void Teleport(Item item, Vector3 position, float range)
	{
		NavMeshHit hit;
		bool flag = NavMesh.SamplePosition(position, ref hit, range, -1);
		if (flag)
		{
			item.transform.position = hit.position + item.GetRadius() * Vector3.up;
		}
		else
		{
			item.transform.position = position + item.GetRadius() * Vector3.up;
		}
	}

	// Token: 0x06000018 RID: 24 RVA: 0x00002AC0 File Offset: 0x00000CC0
	public static void TempMove(Transform parent, Transform newParent, Action func, params Transform[] objs)
	{
		List<Vector3> positions = new List<Vector3>();
		List<Quaternion> rotation = new List<Quaternion>();
		for (int i = 0; i < objs.Length; i++)
		{
			List<Vector3> list = positions;
			Transform transform = objs[i];
			list.Add((transform != null) ? transform.position : Vector3.zero);
			List<Quaternion> list2 = rotation;
			Transform transform2 = objs[i];
			list2.Add((transform2 != null) ? transform2.rotation : Quaternion.identity);
			bool flag = objs[i];
			if (flag)
			{
				objs[i].position = newParent.transform.TransformPoint(parent.transform.InverseTransformPoint(objs[i].position));
				objs[i].rotation = newParent.transform.rotation * (Quaternion.Inverse(parent.transform.rotation) * objs[i].rotation);
			}
		}
		try
		{
			func();
		}
		catch (Exception e)
		{
			Debug.Log(string.Format("TempMove caught exception: {0}", e));
		}
		for (int j = 0; j < objs.Length; j++)
		{
			bool flag2 = objs[j];
			if (flag2)
			{
				objs[j].position = positions[j];
				objs[j].rotation = rotation[j];
			}
		}
	}

	// Token: 0x06000019 RID: 25 RVA: 0x00002C18 File Offset: 0x00000E18
	public static AnimationCurve Curve(params float[] values)
	{
		AnimationCurve curve = new AnimationCurve();
		int i = 0;
		foreach (float value in values)
		{
			curve.AddKey((float)i / ((float)values.Length - 1f), value);
			i++;
		}
		return curve;
	}

	// Token: 0x0600001A RID: 26 RVA: 0x00002C68 File Offset: 0x00000E68
	public static void Teleport(Creature creature, Vector3 position, float range)
	{
		NavMeshHit hit;
		Vector3 target = (NavMesh.SamplePosition(position, ref hit, range, -1) ? hit.position : position);
		bool isPlayer = creature.isPlayer;
		if (isPlayer)
		{
			Player.local.locomotion.transform.position = target;
		}
		else
		{
			creature.locomotion.transform.position = target;
		}
	}

	// Token: 0x0600001B RID: 27 RVA: 0x00002CC8 File Offset: 0x00000EC8
	public static void Explosion(Vector3 origin, float force, float radius, bool massCompensation = false, bool disarm = false, bool dismemberIfKill = false, bool affectPlayer = false, int pushLevel = 3, float damage = 0f, params Rigidbody[] ignoredRBs)
	{
		List<Rigidbody> seenRigidbodies = new List<Rigidbody>();
		List<Creature> seenCreatures = new List<Creature>();
		bool flag = !affectPlayer;
		if (flag)
		{
			seenCreatures.Add(Player.currentCreature);
		}
		object handler = new object();
		foreach (Collider collider in Physics.OverlapSphere(origin, radius))
		{
			bool flag2 = collider.attachedRigidbody == null;
			if (!flag2)
			{
				bool flag3 = collider.attachedRigidbody.gameObject.layer == GameManager.GetLayer(15) || collider.attachedRigidbody.gameObject.layer == GameManager.GetLayer(2) || collider.attachedRigidbody.gameObject.layer == GameManager.GetLayer(16);
				if (!flag3)
				{
					bool flag4 = !seenRigidbodies.Contains(collider.attachedRigidbody) && !ignoredRBs.Contains(collider.attachedRigidbody);
					if (flag4)
					{
						seenRigidbodies.Add(collider.attachedRigidbody);
						float modifier = 1f;
						bool flag5 = collider.attachedRigidbody.mass < 1f;
						if (flag5)
						{
							modifier *= collider.attachedRigidbody.mass * 2f;
						}
						else
						{
							modifier *= collider.attachedRigidbody.mass;
						}
						bool flag6 = !massCompensation;
						if (flag6)
						{
							modifier = 1f;
						}
						modifier *= Random.Range(0.9f, 1.1f);
						collider.attachedRigidbody.AddExplosionForce(force * modifier, origin, radius, 1f, 1);
					}
					else
					{
						Creature creature = collider.GetComponentInParent<Creature>();
						bool flag7 = creature != null && !seenCreatures.Contains(creature);
						if (flag7)
						{
							seenCreatures.Add(creature);
							bool flag8 = !creature.isPlayer && !creature.isKilled;
							if (flag8)
							{
								bool flag9 = pushLevel > 2;
								if (flag9)
								{
									creature.ragdoll.SetState(1);
								}
								creature.TryPush(0, (creature.ragdoll.rootPart.transform.position - origin).normalized, pushLevel, 0);
								bool flag10 = !creature.isPlayer && disarm;
								if (flag10)
								{
									creature.handLeft.TryRelease();
									creature.handRight.TryRelease();
								}
							}
							bool flag11 = damage > 0f;
							if (flag11)
							{
								try
								{
									creature.Damage(new CollisionInstance(new DamageStruct(4, damage), null, null));
								}
								catch
								{
								}
							}
							bool flag12 = dismemberIfKill && creature.isKilled;
							if (flag12)
							{
								foreach (RagdollPart ragdollPart in (from thisPart in creature.ragdoll.parts
									where thisPart.sliceAllowed
									orderby Random.Range(0f, 1f)
									select thisPart).Take(Random.Range(0, 2)))
								{
									ragdollPart.TrySlice();
								}
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x0600001C RID: 28 RVA: 0x00003030 File Offset: 0x00001230
	public static void PushForce(Vector3 origin, Vector3 direction, float radius, float distance, Vector3 force, bool massCompensation = false, bool disarm = false)
	{
		List<Rigidbody> seenRigidbodies = new List<Rigidbody>();
		List<Creature> seenCreatures = new List<Creature> { Player.currentCreature };
		foreach (RaycastHit hit in Physics.SphereCastAll(origin, radius, direction, distance, -5, 1))
		{
			Collider collider = hit.collider;
			bool flag = collider.attachedRigidbody == null;
			if (!flag)
			{
				bool flag2 = collider.attachedRigidbody.gameObject.layer == GameManager.GetLayer(15) || collider.attachedRigidbody.gameObject.layer == GameManager.GetLayer(2) || collider.attachedRigidbody.gameObject.layer == GameManager.GetLayer(16);
				if (!flag2)
				{
					bool flag3 = !seenRigidbodies.Contains(collider.attachedRigidbody);
					if (flag3)
					{
						seenRigidbodies.Add(collider.attachedRigidbody);
						float modifier = 1f;
						bool flag4 = collider.attachedRigidbody.mass < 1f;
						if (flag4)
						{
							modifier *= collider.attachedRigidbody.mass * 2f;
						}
						else
						{
							modifier *= collider.attachedRigidbody.mass;
						}
						bool flag5 = !massCompensation;
						if (flag5)
						{
							modifier = 1f;
						}
						collider.attachedRigidbody.AddForce(force * modifier, 1);
					}
					else
					{
						Creature npc = collider.GetComponentInParent<Creature>();
						bool flag6 = npc != null && npc != null && !seenCreatures.Contains(npc);
						if (flag6)
						{
							seenCreatures.Add(npc);
							npc.TryPush(0, (npc.ragdoll.rootPart.transform.position - origin).normalized, 2, 0);
							if (disarm)
							{
								npc.handLeft.TryRelease();
								npc.handRight.TryRelease();
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x0600001D RID: 29 RVA: 0x00003230 File Offset: 0x00001430
	public static Creature GetAimedAtCreatureOld(Rigidbody rigidbody, Vector3 velocity, float homingAngle, Creature ignoredCreature = null)
	{
		RaycastHit[] hits = Physics.SphereCastAll(rigidbody.transform.position, 10f, velocity, 10f, -5, 1);
		IOrderedEnumerable<Creature> targets = from creature in hits.SelectNotNull(delegate(RaycastHit hit)
			{
				Collider collider = hit.collider;
				Creature creature;
				if (collider == null)
				{
					creature = null;
				}
				else
				{
					Rigidbody attachedRigidbody = collider.attachedRigidbody;
					creature = ((attachedRigidbody != null) ? attachedRigidbody.GetComponentInParent<Creature>() : null);
				}
				return creature;
			})
			where creature != ignoredCreature && creature != Player.currentCreature && creature.state > 0
			where Vector3.Angle(velocity, creature.ragdoll.headPart.transform.position - rigidbody.transform.position) < homingAngle + 3f * Vector3.Distance(rigidbody.transform.position, Player.currentCreature.transform.position)
			orderby Vector3.Angle(velocity, creature.ragdoll.headPart.transform.position - rigidbody.transform.position)
			select creature;
		IEnumerable<Creature> closeToAngle = targets.Where((Creature creature) => Vector3.Angle(velocity, creature.ragdoll.headPart.transform.position - rigidbody.transform.position) < 5f);
		bool flag = closeToAngle.Any<Creature>();
		if (flag)
		{
			targets = closeToAngle.OrderBy((Creature creature) => Vector3.Distance(rigidbody.transform.position, creature.ragdoll.headPart.transform.position));
		}
		return targets.FirstOrDefault<Creature>();
	}

	// Token: 0x0600001E RID: 30 RVA: 0x00003324 File Offset: 0x00001524
	public static Creature GetAimedAtCreature(Rigidbody rigidbody, Vector3 velocity, float homingAngle, Creature ignoredCreature = null)
	{
		RaycastHit[] hits = Physics.SphereCastAll(rigidbody.transform.position, 10f, velocity, 10f, LayerMask.GetMask(new string[] { "BodyLocomotion", "Avatar" }), 1);
		float smallestAngle = float.PositiveInfinity;
		float smallestDistance = float.PositiveInfinity;
		Creature closestCreature = null;
		HashSet<Creature> hitCreatures = new HashSet<Creature>();
		RaycastHit[] array = hits;
		int i = 0;
		while (i < array.Length)
		{
			RaycastHit hit = array[i];
			Collider collider = hit.collider;
			Creature creature2;
			if (collider == null)
			{
				creature2 = null;
			}
			else
			{
				Rigidbody attachedRigidbody = collider.attachedRigidbody;
				creature2 = ((attachedRigidbody != null) ? attachedRigidbody.GetComponentInParent<Creature>() : null);
			}
			Creature creature = creature2;
			bool flag = creature != null;
			if (flag)
			{
				bool flag2 = hitCreatures.Contains(creature) || creature == ignoredCreature || creature.isPlayer || creature.isKilled;
				if (!flag2)
				{
					Vector3 point = hit.point;
					float angle = Vector3.Angle(velocity, point - rigidbody.transform.position);
					float distance = (point - rigidbody.transform.position).sqrMagnitude;
					bool flag3 = (angle < smallestAngle || (angle < 5f && distance < smallestDistance)) && angle < homingAngle + 3f * Vector3.Distance(rigidbody.transform.position, Player.currentCreature.transform.position);
					if (flag3)
					{
						smallestAngle = angle;
						smallestDistance = distance;
						closestCreature = creature;
					}
				}
			}
			IL_15D:
			i++;
			continue;
			goto IL_15D;
		}
		return closestCreature;
	}

	// Token: 0x0600001F RID: 31 RVA: 0x000034A8 File Offset: 0x000016A8
	public static Vector3 AimAssist(this Rigidbody rigidbody, Vector3 velocity, float homingAngle, Creature ignoredCreature = null)
	{
		Creature target = Utils.GetAimedAtCreature(rigidbody, velocity, homingAngle, ignoredCreature);
		bool flag = !target;
		Vector3 vector;
		if (flag)
		{
			vector = velocity;
		}
		else
		{
			Vector3 extendedPoint = rigidbody.transform.position + velocity.normalized * Vector3.Distance(rigidbody.transform.position, target.ragdoll.GetPart(4).transform.position);
			RagdollPart targetPart = target.ragdoll.parts.MinBy((RagdollPart part) => Vector3.Distance(part.transform.position, extendedPoint));
			velocity = (targetPart.transform.position - rigidbody.transform.position).normalized * velocity.magnitude;
			vector = velocity;
		}
		return vector;
	}

	// Token: 0x06000020 RID: 32 RVA: 0x0000357C File Offset: 0x0000177C
	public static Transform GetPlayerChest()
	{
		return Player.currentCreature.ragdoll.GetPart(4).transform;
	}

	// Token: 0x06000021 RID: 33 RVA: 0x000035A4 File Offset: 0x000017A4
	public static Vector3 UniqueVector(this GameObject obj, float min = -1f, float max = 1f, int salt = 0)
	{
		Random rand = new Random(obj.GetInstanceID() + salt);
		return new Vector3((float)rand.NextDouble() * (max - min) + min, (float)rand.NextDouble() * (max - min) + min, (float)rand.NextDouble() * (max - min) + min);
	}

	// Token: 0x06000022 RID: 34 RVA: 0x000035F0 File Offset: 0x000017F0
	public static float UniqueFloat(this GameObject obj, int salt = 0)
	{
		return (float)new Random(obj.GetInstanceID() + salt).NextDouble();
	}

	// Token: 0x06000023 RID: 35 RVA: 0x00003608 File Offset: 0x00001808
	public static Vector3 RandomVector(float min = -1f, float max = 1f, int salt = 0)
	{
		return new Vector3(Random.Range(0f, 1f) * (max - min) + min, Random.Range(0f, 1f) * (max - min) + min, Random.Range(0f, 1f) * (max - min) + min);
	}

	// Token: 0x06000024 RID: 36 RVA: 0x00003660 File Offset: 0x00001860
	public static void SetField<T, U>(this T instance, string fieldName, U value)
	{
		BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
		FieldInfo field = instance.GetType().GetField(fieldName, bindFlags);
		field.SetValue(instance, value);
	}

	// Token: 0x06000025 RID: 37 RVA: 0x00003699 File Offset: 0x00001899
	public static IEnumerable<Creature> GetAliveNPCs()
	{
		return Creature.allActive.Where((Creature creature) => creature != Player.currentCreature && creature.state > 0);
	}

	// Token: 0x06000026 RID: 38 RVA: 0x000036C4 File Offset: 0x000018C4
	public static IEnumerable<Item> ItemsInRadius(Vector3 position, float radius)
	{
		return Physics.OverlapSphere(position, radius, -5, 1).SelectNotNull(delegate(Collider collider)
		{
			Rigidbody attachedRigidbody = collider.attachedRigidbody;
			Item item;
			if (attachedRigidbody == null)
			{
				item = null;
			}
			else
			{
				CollisionHandler component = attachedRigidbody.GetComponent<CollisionHandler>();
				item = ((component != null) ? component.item : null);
			}
			return item;
		}).Distinct<Item>();
	}

	// Token: 0x06000027 RID: 39 RVA: 0x0000370C File Offset: 0x0000190C
	public static IEnumerable<Creature> CreaturesInRadius(Vector3 position, float radius)
	{
		return Creature.allActive.Where((Creature creature) => (creature.GetChest() - position).sqrMagnitude < radius * radius);
	}

	// Token: 0x06000028 RID: 40 RVA: 0x00003748 File Offset: 0x00001948
	public static Creature ClosestCreatureInRadius(Vector3 position, float radius, bool live = true)
	{
		float lastRadius = float.PositiveInfinity;
		Creature lastCreature = null;
		foreach (Creature creature in Creature.allActive)
		{
			bool flag = creature.isCulled || creature.isPlayer || (live && creature.isKilled);
			if (!flag)
			{
				float thisRadius = (creature.GetChest() - position).sqrMagnitude;
				bool flag2 = thisRadius < radius * radius && thisRadius < lastRadius;
				if (flag2)
				{
					lastRadius = thisRadius;
					lastCreature = creature;
				}
			}
		}
		return lastCreature;
	}

	// Token: 0x06000029 RID: 41 RVA: 0x00003804 File Offset: 0x00001A04
	public static Creature TargetCreature(Vector3 position, Vector3 direction, float distance, float angle, Creature toIgnore = null)
	{
		float lastAngle = float.PositiveInfinity;
		float sqrDistance = distance * distance;
		Creature target = null;
		foreach (Creature creature in Creature.allActive)
		{
			bool flag = creature.isKilled || creature.isCulled || creature.isPlayer || !creature.initialized || creature == target || creature == toIgnore || creature.faction.id == 2 || creature.ragdoll.state == 6;
			if (!flag)
			{
				Vector3 handToCreature = Vector3.Lerp(creature.ragdoll.GetPart(4).transform.position, creature.ragdoll.headPart.transform.position, 0.5f) - position;
				float creatureDistance = handToCreature.sqrMagnitude;
				bool flag2 = creatureDistance < sqrDistance;
				if (flag2)
				{
					float angleToCreature = Vector3.Angle(direction, handToCreature);
					bool flag3 = angleToCreature < angle && angleToCreature < lastAngle;
					if (flag3)
					{
						lastAngle = angleToCreature;
						target = creature;
					}
				}
			}
		}
		return target;
	}

	// Token: 0x0600002A RID: 42 RVA: 0x0000394C File Offset: 0x00001B4C
	public static Creature TargetCreature(Ray ray, float distance, float angle, Creature toIgnore = null)
	{
		return Utils.TargetCreature(ray.origin, ray.direction, distance, angle, toIgnore);
	}

	// Token: 0x0600002B RID: 43 RVA: 0x00003964 File Offset: 0x00001B64
	public static RagdollPart TargetPart(Vector3 position, Vector3 direction, float distance, float angle, Creature toIgnore = null)
	{
		return Utils.TargetPart(new Ray(position, direction), distance, angle, toIgnore);
	}

	// Token: 0x0600002C RID: 44 RVA: 0x00003978 File Offset: 0x00001B78
	public static RagdollPart TargetPart(Ray ray, float distance, float angle, Creature toIgnore = null)
	{
		Creature creature = Utils.TargetCreature(ray, distance, angle, toIgnore);
		Vector3 extendedPoint = ray.GetPoint(Vector3.Distance(creature.GetChest(), ray.origin));
		float lastPartDistance = float.PositiveInfinity;
		RagdollPart lastPart = null;
		foreach (RagdollPart part in creature.ragdoll.parts)
		{
			float thisDistance = (part.transform.position - extendedPoint).sqrMagnitude;
			bool flag = thisDistance < lastPartDistance;
			if (flag)
			{
				lastPartDistance = thisDistance;
				lastPart = part;
			}
		}
		return lastPart;
	}

	// Token: 0x0600002D RID: 45 RVA: 0x00003A34 File Offset: 0x00001C34
	public static bool HandlerCone(Ray ray, float distance, float angle, out CollisionHandler outHandler, out RaycastHit outHit, bool live = true)
	{
		float lastAngle = float.PositiveInfinity;
		bool found = false;
		outHandler = null;
		outHit = default(RaycastHit);
		RaycastHit[] array = Physics.SphereCastAll(ray, 5f, distance, LayerMask.GetMask(new string[] { "Default", "BodyLocomotion", "MovingItem", "DroppedItem" }), 1);
		int i = 0;
		while (i < array.Length)
		{
			RaycastHit hit = array[i];
			Rigidbody rigidbody = hit.rigidbody;
			CollisionHandler handler = ((rigidbody != null) ? rigidbody.GetComponent<CollisionHandler>() : null);
			bool flag = handler != null;
			if (flag)
			{
				RagdollPart part = handler.ragdollPart;
				bool flag2 = part != null;
				if (flag2)
				{
					bool flag3 = part.ragdoll.creature.isCulled || part.ragdoll.creature.isPlayer || (part.ragdoll.creature.isKilled && live);
					if (flag3)
					{
						goto IL_14D;
					}
				}
				else
				{
					Item item = handler.item;
					bool flag4 = item != null;
					if (flag4)
					{
						bool isCulled = item.isCulled;
						if (isCulled)
						{
							goto IL_14D;
						}
					}
				}
				float thisAngle = Vector3.Angle(ray.direction, hit.point - ray.origin);
				bool flag5 = thisAngle < angle && thisAngle < lastAngle;
				if (flag5)
				{
					outHandler = handler;
					outHit = hit;
					found = true;
				}
			}
			IL_14D:
			i++;
			continue;
			goto IL_14D;
		}
		return found;
	}

	// Token: 0x0600002E RID: 46 RVA: 0x00003BA2 File Offset: 0x00001DA2
	public static Item TargetItem(Vector3 position, Vector3 direction, float distance, float angle)
	{
		return Utils.TargetItem(new Ray(position, direction), distance, angle);
	}

	// Token: 0x0600002F RID: 47 RVA: 0x00003BB4 File Offset: 0x00001DB4
	public static Item TargetItem(Ray ray, float distance, float angle)
	{
		float lastAngle = float.PositiveInfinity;
		float sqrDistance = distance * distance;
		Item target = null;
		foreach (Item item in Item.allActive)
		{
			bool flag = item.isCulled || item.isCulled || item == target;
			if (!flag)
			{
				Vector3 handToItem = Vector3.Lerp(item.transform.position, item.transform.position, 0.5f) - ray.origin;
				float itemDistance = handToItem.sqrMagnitude;
				bool flag2 = itemDistance < sqrDistance;
				if (flag2)
				{
					float angleToItem = Vector3.Angle(ray.direction, handToItem);
					bool flag3 = angleToItem < angle && angleToItem < lastAngle;
					if (flag3)
					{
						lastAngle = angleToItem;
						target = item;
					}
				}
			}
		}
		return target;
	}

	// Token: 0x06000030 RID: 48 RVA: 0x00003CB4 File Offset: 0x00001EB4
	public static void Update(this ConfigurableJoint joint, float spring, float damp)
	{
		bool flag = joint == null;
		if (!flag)
		{
			JointDrive posDrive = default(JointDrive);
			posDrive.positionSpring = spring;
			posDrive.positionDamper = damp;
			posDrive.maximumForce = 1000f;
			joint.xDrive = posDrive;
			joint.yDrive = posDrive;
			joint.zDrive = posDrive;
		}
	}

	// Token: 0x06000031 RID: 49 RVA: 0x00003D10 File Offset: 0x00001F10
	public static RaycastHit[] ConeCastAll(Vector3 origin, float maxRadius, Vector3 direction, float maxDistance, float coneAngle)
	{
		RaycastHit[] sphereCastHits = Physics.SphereCastAll(origin, maxRadius, direction, maxDistance, -5, 1);
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

	// Token: 0x06000032 RID: 50 RVA: 0x00003E03 File Offset: 0x00002003
	public static IEnumerator LoopOver(Action<float> action, float time, Action after = null)
	{
		float startTime = Time.time;
		float elapsed;
		while ((elapsed = Time.time - startTime) <= time)
		{
			action(elapsed / time);
			yield return 0;
		}
		if (after != null)
		{
			after();
		}
		yield break;
	}
}
