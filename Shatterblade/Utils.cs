using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ExtensionMethods;
using ThunderRoad;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x02000005 RID: 5
internal static class Utils
{
	// Token: 0x0600000A RID: 10 RVA: 0x000024A0 File Offset: 0x000006A0
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

	// Token: 0x0600000B RID: 11 RVA: 0x000025E4 File Offset: 0x000007E4
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

	// Token: 0x0600000C RID: 12 RVA: 0x00002650 File Offset: 0x00000850
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

	// Token: 0x0600000D RID: 13 RVA: 0x000027A8 File Offset: 0x000009A8
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

	// Token: 0x0600000E RID: 14 RVA: 0x00002808 File Offset: 0x00000A08
	public static void Explosion(Vector3 origin, float force, float radius, bool massCompensation = false, bool disarm = false, bool dismemberIfKill = false, bool affectPlayer = false, float damage = 0f)
	{
		List<Rigidbody> seenRigidbodies = new List<Rigidbody>();
		List<Creature> seenCreatures = new List<Creature>();
		bool flag = !affectPlayer;
		if (flag)
		{
			seenCreatures.Add(Player.currentCreature);
		}
		foreach (Collider collider in Physics.OverlapSphere(origin, radius))
		{
			bool flag2 = collider.attachedRigidbody == null;
			if (!flag2)
			{
				bool flag3 = collider.attachedRigidbody.gameObject.layer == GameManager.GetLayer(15) || collider.attachedRigidbody.gameObject.layer == GameManager.GetLayer(2) || collider.attachedRigidbody.gameObject.layer == GameManager.GetLayer(16);
				if (!flag3)
				{
					bool flag4 = !seenRigidbodies.Contains(collider.attachedRigidbody);
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
								creature.TryPush(0, (creature.ragdoll.rootPart.transform.position - origin).normalized, 2, 0);
								bool flag9 = !creature.isPlayer && disarm;
								if (flag9)
								{
									creature.handLeft.TryRelease();
									creature.handRight.TryRelease();
								}
							}
							bool flag10 = damage > 0f;
							if (flag10)
							{
								creature.Damage(new CollisionInstance(new DamageStruct(4, damage), null, null));
							}
							bool flag11 = dismemberIfKill && creature.isKilled;
							if (flag11)
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

	// Token: 0x0600000F RID: 15 RVA: 0x00002B24 File Offset: 0x00000D24
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

	// Token: 0x06000010 RID: 16 RVA: 0x00002D24 File Offset: 0x00000F24
	public static Creature HomingCreature(Rigidbody rigidbody, Vector3 velocity, float homingAngle, Creature ignoredCreature = null)
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

	// Token: 0x06000011 RID: 17 RVA: 0x00002E18 File Offset: 0x00001018
	public static Vector3 HomingThrow(Rigidbody rigidbody, Vector3 velocity, float homingAngle, Creature ignoredCreature = null)
	{
		Creature target = Utils.HomingCreature(rigidbody, velocity, homingAngle, ignoredCreature);
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
			Vector3 vectorToTarget = targetPart.transform.position - rigidbody.transform.position;
			rigidbody.velocity = Vector3.zero;
			velocity = vectorToTarget.normalized * velocity.magnitude;
			vector = velocity;
		}
		return vector;
	}

	// Token: 0x06000012 RID: 18 RVA: 0x00002EF8 File Offset: 0x000010F8
	public static Transform GetPlayerChest()
	{
		return Player.currentCreature.ragdoll.GetPart(4).transform;
	}

	// Token: 0x06000013 RID: 19 RVA: 0x00002F20 File Offset: 0x00001120
	public static Vector3 UniqueVector(this GameObject obj, float min = -1f, float max = 1f, int salt = 0)
	{
		Random rand = new Random(obj.GetInstanceID() + salt);
		return new Vector3((float)rand.NextDouble() * (max - min) + min, (float)rand.NextDouble() * (max - min) + min, (float)rand.NextDouble() * (max - min) + min);
	}

	// Token: 0x06000014 RID: 20 RVA: 0x00002F6C File Offset: 0x0000116C
	public static float UniqueFloat(this GameObject obj, int salt = 0)
	{
		return (float)new Random(obj.GetInstanceID() + salt).NextDouble();
	}

	// Token: 0x06000015 RID: 21 RVA: 0x00002F84 File Offset: 0x00001184
	public static Vector3 RandomVector(float min = -1f, float max = 1f, int salt = 0)
	{
		return new Vector3(Random.Range(0f, 1f) * (max - min) + min, Random.Range(0f, 1f) * (max - min) + min, Random.Range(0f, 1f) * (max - min) + min);
	}

	// Token: 0x06000016 RID: 22 RVA: 0x00002FDC File Offset: 0x000011DC
	public static void SetField<T, U>(this T instance, string fieldName, U value)
	{
		BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
		FieldInfo field = instance.GetType().GetField(fieldName, bindFlags);
		field.SetValue(instance, value);
	}

	// Token: 0x06000017 RID: 23 RVA: 0x00003015 File Offset: 0x00001215
	public static IEnumerable<Creature> GetAliveNPCs()
	{
		return Creature.all.Where((Creature creature) => creature != Player.currentCreature && creature.state > 0);
	}

	// Token: 0x06000018 RID: 24 RVA: 0x00003040 File Offset: 0x00001240
	public static IEnumerable<CollisionHandler> ConeCastHandler(Vector3 origin, float maxRadius, Vector3 direction, float maxDistance, float coneAngle, bool npc = true, bool live = true)
	{
		return Utils.ConeCastAll(origin, maxRadius, direction, maxDistance, coneAngle).SelectNotNull(delegate(RaycastHit hit)
		{
			Rigidbody rigidbody = hit.rigidbody;
			return (rigidbody != null) ? rigidbody.gameObject.GetComponent<CollisionHandler>() : null;
		}).Where(delegate(CollisionHandler handler)
		{
			if (npc)
			{
				RagdollPart ragdollPart = handler.ragdollPart;
				if (!(((ragdollPart != null) ? ragdollPart.ragdoll.creature : null) != Player.currentCreature))
				{
					return false;
				}
			}
			bool flag;
			if (live)
			{
				RagdollPart ragdollPart2 = handler.ragdollPart;
				flag = ragdollPart2 == null || ragdollPart2.ragdoll.creature.state > 0;
			}
			else
			{
				flag = true;
			}
			return flag;
		})
			.Distinct<CollisionHandler>();
	}

	// Token: 0x06000019 RID: 25 RVA: 0x000030B0 File Offset: 0x000012B0
	public static IEnumerable<Creature> ConeCastCreature(Vector3 origin, float maxRadius, Vector3 direction, float maxDistance, float coneAngle, bool npc = true, bool live = true, Creature ignore = null)
	{
		return from creature in Utils.ConeCastAll(origin, maxRadius, direction, maxDistance, coneAngle).SelectNotNull(delegate(RaycastHit hit)
			{
				Rigidbody rigidbody = hit.rigidbody;
				Creature creature;
				if (rigidbody == null)
				{
					creature = null;
				}
				else
				{
					CollisionHandler component = rigidbody.gameObject.GetComponent<CollisionHandler>();
					if (component == null)
					{
						creature = null;
					}
					else
					{
						RagdollPart ragdollPart = component.ragdollPart;
						creature = ((ragdollPart != null) ? ragdollPart.ragdoll.creature : null);
					}
				}
				return creature;
			}).Distinct<Creature>()
			where (!npc || creature != Player.currentCreature) && creature != ignore && (!live || creature.state > 0)
			select creature;
	}

	// Token: 0x0600001A RID: 26 RVA: 0x00003128 File Offset: 0x00001328
	public static IEnumerable<RagdollPart> ConeCastRagdollPart(Vector3 origin, float maxRadius, Vector3 direction, float maxDistance, float coneAngle, bool npc = true, bool live = true, Creature ignoredCreature = null)
	{
		return from part in Utils.ConeCastAll(origin, maxRadius, direction, maxDistance, coneAngle).SelectNotNull(delegate(RaycastHit hit)
			{
				Rigidbody rigidbody = hit.rigidbody;
				return (rigidbody != null) ? rigidbody.gameObject.GetComponent<RagdollPart>() : null;
			})
			where (!npc || part.ragdoll.creature != Player.currentCreature) && part.ragdoll.creature != ignoredCreature && (!live || part.ragdoll.creature.state > 0)
			select part;
	}

	// Token: 0x0600001B RID: 27 RVA: 0x00003198 File Offset: 0x00001398
	public static IEnumerable<Item> ConeCastItem(Vector3 origin, float maxRadius, Vector3 direction, float maxDistance, float coneAngle)
	{
		return from item in Utils.ConeCastAll(origin, maxRadius, direction, maxDistance, coneAngle).SelectNotNull(delegate(RaycastHit hit)
			{
				Rigidbody rigidbody = hit.rigidbody;
				return (rigidbody != null) ? rigidbody.gameObject.GetComponent<Item>() : null;
			})
			where !item.isTelekinesisGrabbed && item.holder == null && item.mainHandler == null
			select item;
	}

	// Token: 0x0600001C RID: 28 RVA: 0x00003200 File Offset: 0x00001400
	public static RagdollPart PartCast(Vector3 origin, float maxRadius, Vector3 direction, float maxDistance, bool player = false, Creature ignoredCreature = null)
	{
		RaycastHit[] hits = Physics.SphereCastAll(origin, maxRadius, direction, maxDistance, -5, 1);
		foreach (RaycastHit hit in hits)
		{
			Rigidbody rigidbody = hit.rigidbody;
			RagdollPart ragdollPart;
			if (rigidbody == null)
			{
				ragdollPart = null;
			}
			else
			{
				CollisionHandler component = rigidbody.gameObject.GetComponent<CollisionHandler>();
				ragdollPart = ((component != null) ? component.ragdollPart : null);
			}
			RagdollPart part = ragdollPart;
			bool flag = part != null;
			if (flag)
			{
				Creature creature = part.ragdoll.creature;
				bool flag2 = creature != null && creature.isPlayer && !player;
				if (!flag2)
				{
					bool flag3 = part.ragdoll.creature == ignoredCreature;
					if (!flag3)
					{
						return part;
					}
				}
			}
		}
		return null;
	}

	// Token: 0x0600001D RID: 29 RVA: 0x000032C0 File Offset: 0x000014C0
	public static IEnumerable<CollisionHandler> SphereCastHandler(Vector3 origin, float maxRadius, Vector3 direction, float maxDistance, bool npc = true, bool live = true)
	{
		return Physics.SphereCastAll(origin, maxRadius, direction, maxDistance, -5, 1).SelectNotNull(delegate(RaycastHit hit)
		{
			Rigidbody rigidbody = hit.rigidbody;
			return (rigidbody != null) ? rigidbody.gameObject.GetComponent<CollisionHandler>() : null;
		});
	}

	// Token: 0x0600001E RID: 30 RVA: 0x00003304 File Offset: 0x00001504
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

	// Token: 0x0600001F RID: 31 RVA: 0x00003370 File Offset: 0x00001570
	public static IEnumerable<CollisionHandler> OverlapSphereHandler(Vector3 origin, float radius)
	{
		return Physics.OverlapSphere(origin, radius, -5, 1).SelectNotNull(delegate(Collider collider)
		{
			Rigidbody attachedRigidbody = collider.attachedRigidbody;
			return (attachedRigidbody != null) ? attachedRigidbody.GetComponent<CollisionHandler>() : null;
		});
	}

	// Token: 0x06000020 RID: 32 RVA: 0x000033B0 File Offset: 0x000015B0
	public static IEnumerable<Creature> OverlapSphereCreature(Vector3 origin, float radius, bool live = true)
	{
		return (from creature in Physics.OverlapSphere(origin, radius, -5, 1).SelectNotNull(delegate(Collider collider)
			{
				Rigidbody attachedRigidbody = collider.attachedRigidbody;
				Creature creature;
				if (attachedRigidbody == null)
				{
					creature = null;
				}
				else
				{
					CollisionHandler component = attachedRigidbody.GetComponent<CollisionHandler>();
					if (component == null)
					{
						creature = null;
					}
					else
					{
						RagdollPart ragdollPart = component.ragdollPart;
						creature = ((ragdollPart != null) ? ragdollPart.ragdoll.creature : null);
					}
				}
				return creature;
			})
			where !creature.isPlayer
			where !live || !creature.isKilled
			select creature).Distinct<Creature>();
	}

	// Token: 0x06000021 RID: 33 RVA: 0x00003438 File Offset: 0x00001638
	public static RagdollPart TargetPart(Vector3 position, Vector3 direction, float distance, float maxRadius, float angle, Creature ignoredCreature = null, bool player = false)
	{
		RaycastHit[] hits = Physics.SphereCastAll(position, maxRadius, direction, distance, -5, 1);
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
					if (attachedRigidbody == null)
					{
						creature = null;
					}
					else
					{
						CollisionHandler component = attachedRigidbody.GetComponent<CollisionHandler>();
						if (component == null)
						{
							creature = null;
						}
						else
						{
							RagdollPart ragdollPart2 = component.ragdollPart;
							creature = ((ragdollPart2 != null) ? ragdollPart2.ragdoll.creature : null);
						}
					}
				}
				return creature;
			})
			where creature != ignoredCreature && (player || !creature.isPlayer) && !creature.isKilled
			where Vector3.Angle(direction, creature.ragdoll.headPart.transform.position - position) < angle + 3f * Vector3.Distance(position, Player.currentCreature.GetChest())
			orderby Vector3.Angle(direction, creature.ragdoll.headPart.transform.position - position)
			select creature;
		bool flag = !targets.Any<Creature>();
		RagdollPart ragdollPart;
		if (flag)
		{
			ragdollPart = null;
		}
		else
		{
			IEnumerable<Creature> closeToAngle = targets.Where((Creature creature) => Vector3.Angle(direction, creature.ragdoll.headPart.transform.position - position) < 5f);
			bool flag2 = closeToAngle.Any<Creature>();
			if (flag2)
			{
				targets = closeToAngle.OrderBy((Creature creature) => Vector3.Distance(position, creature.ragdoll.headPart.transform.position));
			}
			Creature target = targets.FirstOrDefault<Creature>();
			Vector3 extendedPoint = position + direction.normalized * Vector3.Distance(position, target.ragdoll.GetPart(4).transform.position);
			ragdollPart = target.ragdoll.parts.MinBy((RagdollPart part) => Vector3.Distance(part.transform.position, extendedPoint));
		}
		return ragdollPart;
	}

	// Token: 0x06000022 RID: 34 RVA: 0x0000359C File Offset: 0x0000179C
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

	// Token: 0x06000023 RID: 35 RVA: 0x000035F8 File Offset: 0x000017F8
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

	// Token: 0x06000024 RID: 36 RVA: 0x000036EB File Offset: 0x000018EB
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
