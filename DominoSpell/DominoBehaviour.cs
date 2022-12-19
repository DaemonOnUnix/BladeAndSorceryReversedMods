using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExtensionMethods;
using ThunderRoad;
using UnityEngine;

namespace DominoSpell
{
	// Token: 0x02000007 RID: 7
	internal class DominoBehaviour : MonoBehaviour
	{
		// Token: 0x06000098 RID: 152 RVA: 0x00005314 File Offset: 0x00003514
		public IEnumerator PulseLinkRoutine(EffectInstance effect)
		{
			bool flag = Time.time - this.lastPulseTime < 0.5f;
			if (flag)
			{
				yield break;
			}
			this.lastPulseTime = Time.time;
			Catalog.GetData<EffectData>("DominoTransfer", true).Spawn(this.EffectPos(), Quaternion.identity, null, null, true, null, false, Array.Empty<Type>()).Play(0, false);
			yield return Utils.LoopOver(delegate(float time)
			{
				EffectInstance effect2 = effect;
				float[] array = new float[3];
				array[1] = 1f;
				effect2.SetIntensity(time.Curve(array));
			}, 0.5f, delegate
			{
				effect.SetIntensity(0f);
			});
			yield break;
		}

		// Token: 0x06000099 RID: 153 RVA: 0x0000532A File Offset: 0x0000352A
		public void PulseLink(EffectInstance effect)
		{
			base.StartCoroutine(this.PulseLinkRoutine(effect));
		}

		// Token: 0x0600009A RID: 154 RVA: 0x0000533C File Offset: 0x0000353C
		public void Awake()
		{
			this.links = new Dictionary<DominoBehaviour, EffectInstance>();
			this.others = new List<DominoBehaviour>();
			this.slicedParts = new List<RagdollPart>();
			this.creature = base.GetComponent<Creature>();
			this.creature.OnDamageEvent += delegate(CollisionInstance instance)
			{
				bool flag = this.damaged;
				if (!flag)
				{
					this.damaged = true;
					foreach (DominoBehaviour other in this.others.ToList<DominoBehaviour>())
					{
						other.Damage(instance);
						bool flag2 = this.links.ContainsKey(other);
						if (flag2)
						{
							this.PulseLink(this.links[other]);
						}
					}
				}
			};
			this.creature.ragdoll.OnSliceEvent += delegate(RagdollPart part, EventTime time)
			{
				bool flag = this.slicedParts.Contains(part);
				if (!flag)
				{
					this.slicedParts.Add(part);
					foreach (DominoBehaviour other in this.others.ToList<DominoBehaviour>())
					{
						RagdollPart thisPart = other.creature.ragdoll.parts[this.creature.ragdoll.parts.IndexOf(part)];
						other.creature.ragdoll.Slice(thisPart);
						bool flag2 = this.links.ContainsKey(other);
						if (flag2)
						{
							this.PulseLink(this.links[other]);
						}
					}
				}
			};
			this.creature.OnKillEvent += delegate(CollisionInstance instance, EventTime time)
			{
				bool flag = this.dead;
				if (!flag)
				{
					this.dead = true;
					foreach (DominoBehaviour other in this.others.ToList<DominoBehaviour>())
					{
						bool flag2 = !other.creature.isKilled;
						if (flag2)
						{
							other.creature.Kill(instance);
							bool flag3 = this.links.ContainsKey(other);
							if (flag3)
							{
								this.PulseLink(this.links[other]);
							}
						}
					}
					Object.Destroy(this);
				}
			};
		}

		// Token: 0x0600009B RID: 155 RVA: 0x000053C4 File Offset: 0x000035C4
		public void Start()
		{
			this.targetEffect = SpellCastDomino.markerData.Spawn(this.creature.GetHead().transform.position + Vector3.up * 0.5f, this.creature.GetHead().transform.rotation, null, null, false, null, false, Array.Empty<Type>());
			this.targetEffect.Play(0, false);
			Creature creature = this.creature;
			if (creature != null)
			{
				Ragdoll ragdoll = creature.ragdoll;
				if (ragdoll != null)
				{
					ragdoll.AddPhysicToggleModifier(this);
				}
			}
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00005458 File Offset: 0x00003658
		public void OnDestroy()
		{
			foreach (KeyValuePair<DominoBehaviour, EffectInstance> effect in this.links)
			{
				EffectInstance value = effect.Value;
				if (value != null)
				{
					value.End(false, -1f);
				}
			}
			EffectInstance effectInstance = this.targetEffect;
			if (effectInstance != null)
			{
				effectInstance.End(false, -1f);
			}
			this.targetEffect = null;
			Creature creature = this.creature;
			if (creature != null)
			{
				Ragdoll ragdoll = creature.ragdoll;
				if (ragdoll != null)
				{
					ragdoll.RemovePhysicToggleModifier(this);
				}
			}
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00005500 File Offset: 0x00003700
		public void Update()
		{
			EffectInstance effectInstance = this.targetEffect;
			if (effectInstance != null)
			{
				effectInstance.SetPosition(this.EffectPos());
			}
			foreach (DominoBehaviour other in this.others)
			{
				EffectInstance effectInstance2 = this.links[other];
				if (effectInstance2 != null)
				{
					effectInstance2.SetPosition(Vector3.Lerp(this.EffectPos(), other.EffectPos(), 0.5f));
				}
				EffectInstance effectInstance3 = this.links[other];
				if (effectInstance3 != null)
				{
					effectInstance3.SetVFXProperty("Source", this.EffectPos());
				}
				EffectInstance effectInstance4 = this.links[other];
				if (effectInstance4 != null)
				{
					effectInstance4.SetVFXProperty("Target", other.EffectPos());
				}
			}
		}

		// Token: 0x0600009E RID: 158 RVA: 0x000055E0 File Offset: 0x000037E0
		public void FixedUpdate()
		{
			this.damaged = false;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x000055EC File Offset: 0x000037EC
		public void Link(DominoBehaviour other)
		{
			bool flag = !this.others.Contains(other) && other != this.creature;
			if (flag)
			{
				other.creature.OnKillEvent += delegate(CollisionInstance instance, EventTime time)
				{
					this.others.Remove(other);
					bool flag2 = this.links.ContainsKey(other);
					if (flag2)
					{
						EffectInstance effectInstance = this.links[other];
						if (effectInstance != null)
						{
							effectInstance.End(false, -1f);
						}
						this.links.Remove(other);
					}
				};
				other.creature.OnDespawnEvent += delegate(EventTime _)
				{
					this.others.Remove(other);
					bool flag2 = this.links.ContainsKey(other);
					if (flag2)
					{
						EffectInstance effectInstance = this.links[other];
						if (effectInstance != null)
						{
							effectInstance.End(false, -1f);
						}
						this.links.Remove(other);
					}
				};
				this.links[other] = SpellCastDomino.linkData.Spawn(Vector3.Lerp(this.EffectPos(), other.EffectPos(), 0.5f), Quaternion.identity, null, null, false, null, false, Array.Empty<Type>());
				this.others.Add(other);
				this.links[other].Play(0, false);
			}
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x000056EC File Offset: 0x000038EC
		public void CheckDominoLink()
		{
			foreach (DominoBehaviour other in this.links.Keys)
			{
				base.SendMessage("DominoLinkedTo", other.creature);
			}
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00005754 File Offset: 0x00003954
		public Vector3 EffectPos()
		{
			return this.creature.GetHead().transform.position + Vector3.up * 0.5f;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00005780 File Offset: 0x00003980
		public void Damage(CollisionInstance instance)
		{
			bool flag = instance.contactPoint == this.lastDamage;
			if (!flag)
			{
				RagdollPart otherPart = instance.damageStruct.hitRagdollPart;
				RagdollPart part = this.creature.ragdoll.parts[otherPart.ragdoll.creature.ragdoll.parts.IndexOf(otherPart)];
				CollisionInstance instance2 = instance;
				Vector3? vector;
				if (instance2 == null)
				{
					vector = null;
				}
				else
				{
					ColliderGroup sourceColliderGroup = instance2.sourceColliderGroup;
					vector = ((sourceColliderGroup != null) ? new Vector3?(sourceColliderGroup.collisionHandler.rb.velocity) : null);
				}
				Vector3 originalVel = vector ?? Vector3.zero;
				bool flag2 = instance.sourceColliderGroup;
				if (flag2)
				{
					instance.sourceColliderGroup.collisionHandler.rb.velocity = part.transform.TransformVector(otherPart.transform.InverseTransformVector(instance.sourceColliderGroup.collisionHandler.rb.velocity));
				}
				Vector3 contactPoint = part.transform.TransformPoint(otherPart.transform.InverseTransformPoint(instance.contactPoint));
				Vector3 contactNormal = part.transform.TransformDirection(otherPart.transform.InverseTransformDirection(instance.contactNormal));
				Vector3 impactVelocity = part.transform.TransformVector(otherPart.transform.InverseTransformVector(instance.impactVelocity));
				Transform transform = otherPart.transform;
				Transform transform2 = part.transform;
				Action action = delegate()
				{
					Collider targetCollider = part.colliderGroup.colliders[0];
					bool flag4 = instance.targetCollider;
					if (flag4)
					{
						int index = otherPart.colliderGroup.colliders.IndexOf(instance.targetCollider);
						bool flag5 = index == -1;
						if (flag5)
						{
							index = otherPart.colliderGroup.colliders.IndexOf(instance.sourceCollider);
						}
						bool flag6 = index > -1 && index < part.colliderGroup.colliders.Count;
						if (flag6)
						{
							targetCollider = part.colliderGroup.colliders[index];
						}
					}
					CollisionInstance clone = instance.Clone<CollisionInstance>();
					this.lastDamage = instance.contactPoint;
					clone.NewHit(instance.sourceCollider, targetCollider, instance.sourceColliderGroup, part.colliderGroup, impactVelocity, contactPoint, contactNormal, instance.intensity, instance.sourceMaterial, instance.targetMaterial);
					ColliderGroup sourceColliderGroup4 = instance.sourceColliderGroup;
					Rigidbody rigidbody;
					if (sourceColliderGroup4 == null)
					{
						rigidbody = null;
					}
					else
					{
						CollisionHandler collisionHandler = sourceColliderGroup4.collisionHandler;
						rigidbody = ((collisionHandler != null) ? collisionHandler.rb : null);
					}
					Rigidbody rb = rigidbody;
					bool flag7 = rb != null;
					if (flag7)
					{
						part.rb.AddForce(rb.velocity, 1);
					}
					bool flag8 = instance.damageStruct.damager;
					if (flag8)
					{
						clone.damageStruct.damager.UnPenetrate(clone);
					}
				};
				Transform[] array = new Transform[1];
				int num = 0;
				ColliderGroup sourceColliderGroup2 = instance.sourceColliderGroup;
				object obj;
				if (sourceColliderGroup2 == null)
				{
					obj = null;
				}
				else
				{
					Item item = sourceColliderGroup2.collisionHandler.item;
					obj = ((item != null) ? item.transform : null);
				}
				object obj2;
				if ((obj2 = obj) == null)
				{
					ColliderGroup sourceColliderGroup3 = instance.sourceColliderGroup;
					if (sourceColliderGroup3 == null)
					{
						obj2 = null;
					}
					else
					{
						RagdollPart ragdollPart = sourceColliderGroup3.collisionHandler.ragdollPart;
						obj2 = ((ragdollPart != null) ? ragdollPart.transform : null);
					}
				}
				array[num] = obj2;
				Utils.TempMove(transform, transform2, action, array);
				bool flag3 = instance.sourceColliderGroup;
				if (flag3)
				{
					instance.sourceColliderGroup.collisionHandler.rb.velocity = originalVel;
				}
			}
		}

		// Token: 0x0400000B RID: 11
		private Creature creature;

		// Token: 0x0400000C RID: 12
		private List<DominoBehaviour> others;

		// Token: 0x0400000D RID: 13
		public Vector3 lastDamage;

		// Token: 0x0400000E RID: 14
		public Dictionary<DominoBehaviour, EffectInstance> links;

		// Token: 0x0400000F RID: 15
		public EffectInstance targetEffect;

		// Token: 0x04000010 RID: 16
		private float lastPulseTime = 0f;

		// Token: 0x04000011 RID: 17
		public bool damaged = false;

		// Token: 0x04000012 RID: 18
		private bool dead;

		// Token: 0x04000013 RID: 19
		private List<RagdollPart> slicedParts;
	}
}
