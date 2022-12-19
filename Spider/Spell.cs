using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ThunderRoad;
using UnityEngine;
using UnityEngine.AI;

namespace Spider
{
	// Token: 0x02000002 RID: 2
	public class Spell : SpellCastCharge
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public override void Load(SpellCaster spellCaster, SpellData.Level level)
		{
			base.Load(spellCaster, level);
			Spell.latestInstance = this;
			this.activeWebHolder = new GameObject();
			this.activeWebHolder.transform.position = spellCaster.ragdollHand.transform.position;
			this.activeWebHolder.transform.parent = spellCaster.transform;
			this.activeWeb = this.activeWebHolder.AddComponent<LineRenderer>();
			this.activeWeb.material = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
			this.activeWeb.material.SetColor("_BaseColor", this.webColor);
			this.activeWeb.widthMultiplier = this.webSize * 0.003f;
			this.activeWeb.enabled = false;
			bool flag = this.visibleStringAim;
			if (flag)
			{
				this.reticleHolder = new GameObject();
				this.reticle = this.reticleHolder.AddComponent<LineRenderer>();
				Catalog.LoadAssetAsync<Material>("Spider.ReticleMaterial", delegate(Material material)
				{
					this.reticle.material = material;
				}, "Spider");
				this.reticle.widthMultiplier = 0.0015f;
				this.reticle.generateLightingData = false;
				this.reticle.lightProbeUsage = 0;
				this.reticle.receiveShadows = false;
				this.reticle.enabled = true;
			}
			bool flag2 = spellCaster.ragdollHand.side == 1;
			if (flag2)
			{
				Spell.leftInstance = this;
			}
			bool flag3 = spellCaster.ragdollHand.side == 0;
			if (flag3)
			{
				Spell.rightInstance = this;
			}
			spellCaster.ragdollHand.gameObject.AddComponent<Spell.HandTetherAbility>();
			bool flag4 = !Spell.bindedEvent;
			if (flag4)
			{
				Spell.bindedEvent = true;
				EventManager.CreatureSpawnedEvent creatureSpawnedEvent;
				if ((creatureSpawnedEvent = Spell.<>O.<0>__Spawn) == null)
				{
					creatureSpawnedEvent = (Spell.<>O.<0>__Spawn = new EventManager.CreatureSpawnedEvent(Spell.Spawn));
				}
				EventManager.onCreatureSpawn += creatureSpawnedEvent;
				EventManager.LevelLoadEvent levelLoadEvent;
				if ((levelLoadEvent = Spell.<>O.<1>__UnLoad) == null)
				{
					levelLoadEvent = (Spell.<>O.<1>__UnLoad = new EventManager.LevelLoadEvent(Spell.UnLoad));
				}
				EventManager.onLevelUnload += levelLoadEvent;
				foreach (Keyframe keyframe in Player.currentCreature.data.playerFallDamageCurve.keys)
				{
					keyframe.time *= 50f;
					keyframe.value /= 50f;
				}
				Keyframe[] keys;
				Player.currentCreature.data.playerFallDamageCurve.keys = keys;
				Player.currentCreature.data.playerFallDamageCurve.preWrapMode = 1;
				Player.currentCreature.data.playerFallDamageCurve.postWrapMode = 4;
			}
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000022E7 File Offset: 0x000004E7
		public static void UnLoad(LevelData levelData, EventTime eventTime)
		{
			Spell.ClearTethers();
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000022F0 File Offset: 0x000004F0
		public static void ClearTethers()
		{
			HashSet<Spell.Tether> hashSet = Spell.tetherToTime.Keys.ToHashSet<Spell.Tether>();
			foreach (Spell.Tether tether in hashSet)
			{
				tether.Destroy();
			}
			Spell.tetherToTime.Clear();
		}

		// Token: 0x06000004 RID: 4 RVA: 0x0000235C File Offset: 0x0000055C
		public static void Spawn(Creature creature)
		{
			bool flag = creature == null;
			if (!flag)
			{
				bool flag2 = Spell.leftInstance == null && Spell.rightInstance == null;
				if (!flag2)
				{
					bool flag3;
					if (Spell.leftInstance != null && Spell.leftInstance.attachedRB)
					{
						Creature componentInParent = Spell.leftInstance.attachedRB.GetComponentInParent<Creature>();
						if (componentInParent != null)
						{
							flag3 = componentInParent == creature;
							goto IL_63;
						}
					}
					flag3 = false;
					IL_63:
					bool flag4 = flag3;
					if (flag4)
					{
						Spell.leftInstance.CancelWeb();
					}
					bool flag5;
					if (Spell.rightInstance != null && Spell.rightInstance.attachedRB)
					{
						Creature componentInParent2 = Spell.rightInstance.attachedRB.GetComponentInParent<Creature>();
						if (componentInParent2 != null)
						{
							flag5 = componentInParent2 == creature;
							goto IL_AB;
						}
					}
					flag5 = false;
					IL_AB:
					bool flag6 = flag5;
					if (flag6)
					{
						Spell.rightInstance.CancelWeb();
					}
					HashSet<Spell.Tether> hashSet = Spell.tetherToTime.Keys.ToHashSet<Spell.Tether>();
					foreach (Spell.Tether tether in hashSet)
					{
						Creature componentInParent3 = tether.rb.gameObject.GetComponentInParent<Creature>();
						bool flag7 = componentInParent3 != null && componentInParent3 == creature;
						if (flag7)
						{
							tether.Destroy();
							Spell.tetherToTime.Remove(tether);
						}
						if (!tether.rb2)
						{
							goto IL_154;
						}
						Creature componentInParent4 = tether.rb2.gameObject.GetComponentInParent<Creature>();
						if (componentInParent4 == null)
						{
							goto IL_154;
						}
						bool flag8 = componentInParent4 == creature;
						IL_155:
						bool flag9 = flag8;
						if (flag9)
						{
							tether.Destroy();
							Spell.tetherToTime.Remove(tether);
						}
						continue;
						IL_154:
						flag8 = false;
						goto IL_155;
					}
				}
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x0000250C File Offset: 0x0000070C
		public override void Fire(bool active)
		{
			base.Fire(active);
			if (active)
			{
				this.Fire(false);
				this.currentCharge = 0f;
				this.spellCaster.isFiring = false;
				this.acting = true;
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002550 File Offset: 0x00000750
		public override void UpdateCaster()
		{
			base.UpdateCaster();
			bool flag = this.visibleStringAim;
			if (flag)
			{
				this.reticle.SetPositions(new Vector3[]
				{
					this.spellCaster.ragdollHand.transform.position,
					this.spellCaster.ragdollHand.transform.position + -(this.spellCaster.ragdollHand.transform.right * 75f)
				});
			}
			bool flag2 = this.acting;
			if (flag2)
			{
				bool castPressed = PlayerControl.GetHand(this.spellCaster.ragdollHand.side).castPressed;
				if (castPressed)
				{
					bool flag3 = !this.attachedRB && this.webbedPoint == Vector3.zero;
					if (flag3)
					{
						RaycastHit hit;
						bool flag4 = Physics.Raycast(this.spellCaster.ragdollHand.transform.position + this.spellCaster.ragdollHand.transform.right * -0.175f, -this.spellCaster.ragdollHand.transform.right, ref hit, this.maxRange);
						if (flag4)
						{
							bool flag5 = hit.rigidbody && !hit.rigidbody.isKinematic;
							if (flag5)
							{
								this.attachedRB = hit.rigidbody;
								this.offset = this.attachedRB.transform.InverseTransformPoint(hit.point);
								Creature componentInParent = this.attachedRB.GetComponentInParent<Creature>();
								bool flag6 = componentInParent != null;
								if (flag6)
								{
									this.webbedCreature = componentInParent;
									bool flag7 = !this.attachedRB.GetComponent<RagdollPart>();
									if (flag7)
									{
										RagdollPart ragdollPart = componentInParent.ragdoll.parts.OrderBy((RagdollPart i) => Vector3.Distance(i.transform.position, hit.point)).First<RagdollPart>();
										this.attachedRB = ragdollPart.rb;
										this.offset = this.attachedRB.transform.InverseTransformPoint(ragdollPart.transform.position);
									}
								}
								Item componentInParent2 = this.attachedRB.GetComponentInParent<Item>();
								bool flag8 = componentInParent2 != null && componentInParent2.data.purchasable;
								if (flag8)
								{
									this.webbedItem = componentInParent2;
								}
							}
							else
							{
								this.webbedPoint = hit.point;
							}
							this.webDistance = hit.distance - 0.25f;
							this.activeWeb.enabled = true;
							bool flag9 = this.reticle;
							if (flag9)
							{
								this.reticle.enabled = false;
							}
						}
					}
					bool flag10 = this.attachedRB || this.webbedPoint != Vector3.zero;
					if (flag10)
					{
						Vector3 vector = (this.attachedRB ? this.attachedRB.transform.TransformPoint(this.offset) : this.webbedPoint);
						float num = Vector3.Distance(vector, this.spellCaster.ragdollHand.transform.position);
						Locomotion locomotion = ((this.spellCaster.ragdollHand.creature == Player.currentCreature) ? Player.local.locomotion : this.spellCaster.ragdollHand.creature.locomotion);
						Vector3 normalized = ((this.attachedRB ? this.spellCaster.ragdollHand.transform.position : this.webbedPoint) - (this.attachedRB ? this.attachedRB.transform.position : this.spellCaster.ragdollHand.creature.transform.position)).normalized;
						this.activeWeb.enabled = true;
						this.activeWeb.SetPositions(new Vector3[]
						{
							vector,
							this.spellCaster.ragdollHand.transform.position
						});
						bool flag11 = num > this.webDistance && !PlayerControl.GetHand(this.spellCaster.ragdollHand.side).gripPressed;
						if (flag11)
						{
							Rigidbody rigidbody = (this.attachedRB ? this.attachedRB : locomotion.rb);
							Vector3 vector2 = normalized;
							float num2 = Spell.webPower * Mathf.Min(num * 1.25f, 30f);
							float num3;
							if (!this.webbedCreature)
							{
								num3 = 1.25f;
							}
							else
							{
								num3 = (float)(Mathf.Abs(this.webbedCreature.ragdoll.parts.Where((RagdollPart part) => !part.isSliced).Count<RagdollPart>() / this.webbedCreature.ragdoll.parts.Count - 1) + 2);
							}
							rigidbody.AddForce(vector2 * (num2 * num3 * ((this.attachedRB == null) ? 0.75f : 1f)), this.attachedRB ? 1 : 5);
						}
						bool gripPressed = PlayerControl.GetHand(this.spellCaster.ragdollHand.side).gripPressed;
						if (gripPressed)
						{
							Rigidbody rigidbody2 = (this.attachedRB ? this.attachedRB : locomotion.rb);
							Vector3 vector3 = normalized;
							float num4 = Spell.webPower * Mathf.Min(num * 1.35f, 40f);
							float num5;
							if (!this.webbedCreature)
							{
								num5 = 1.25f;
							}
							else
							{
								num5 = (float)(Mathf.Abs(this.webbedCreature.ragdoll.parts.Where((RagdollPart part) => !part.isSliced).Count<RagdollPart>() / this.webbedCreature.ragdoll.parts.Count - 1) + 2);
							}
							rigidbody2.AddForce(vector3 * (num4 * num5 * ((this.attachedRB == null) ? 0.85f : 1f)), this.attachedRB ? 1 : 5);
						}
						bool flag12 = num + 0.25f < this.webDistance;
						if (flag12)
						{
							Rigidbody rigidbody3 = (this.attachedRB ? this.attachedRB : locomotion.rb);
							Vector3 vector4 = -normalized;
							float num6 = Mathf.Abs(Spell.webElasticity / 100f - 1f) * Spell.webPower * Mathf.Min(num * 1.35f, 40f);
							float num7;
							if (!this.webbedCreature)
							{
								num7 = 1.25f;
							}
							else
							{
								num7 = (float)(Mathf.Abs(this.webbedCreature.ragdoll.parts.Where((RagdollPart part) => !part.isSliced).Count<RagdollPart>() / this.webbedCreature.ragdoll.parts.Count - 1) + 2);
							}
							rigidbody3.AddForce(vector4 * (num6 * num7 * ((this.attachedRB == null) ? 0.85f : 1f)), this.attachedRB ? 1 : 5);
						}
						bool flag13 = num - 0.25f > this.webDistance;
						if (flag13)
						{
							Rigidbody rigidbody4 = (this.attachedRB ? this.attachedRB : locomotion.rb);
							Vector3 vector5 = normalized;
							float num8 = Mathf.Abs(Spell.webElasticity / 100f - 1f) * Spell.webPower * Mathf.Min(num * 1.35f, 40f);
							float num9;
							if (!this.webbedCreature)
							{
								num9 = 1.25f;
							}
							else
							{
								num9 = (float)(Mathf.Abs(this.webbedCreature.ragdoll.parts.Where((RagdollPart part) => !part.isSliced).Count<RagdollPart>() / this.webbedCreature.ragdoll.parts.Count - 1) + 2);
							}
							rigidbody4.AddForce(vector5 * (num8 * num9 * ((this.attachedRB == null) ? 0.85f : 1f)), this.attachedRB ? 1 : 5);
						}
						bool flag14 = Vector3.Dot(locomotion.velocity.normalized, Vector3.down) > 0.8f && this.webbedPoint != Vector3.zero && this.spellCaster.ragdollHand.transform.position.y < vector.y - this.webDistance * 0.9f;
						if (flag14)
						{
							locomotion.rb.AddForce(Vector3.up * Spell.webPower, 5);
						}
						bool flag15 = this.webbedCreature && this.attachedRB && Vector3.Distance(vector, this.spellCaster.ragdollHand.transform.position) > 20f / (Spell.webPower * 1.5f) && !this.webbedCreature.isKilled && this.webbedCreature.ragdoll.state != 1;
						if (flag15)
						{
							this.webbedCreature.ragdoll.SetState(1);
						}
						bool flag16 = this.attachedRB;
						if (flag16)
						{
							Rigidbody rb = locomotion.rb;
							Vector3 vector6 = (this.attachedRB.transform.position - locomotion.transform.position).normalized * Mathf.Min(num, 20f);
							float num10;
							if (!this.webbedCreature)
							{
								num10 = (float)2;
							}
							else
							{
								num10 = (float)(this.webbedCreature.ragdoll.parts.Where((RagdollPart part) => !part.isSliced).Count<RagdollPart>() / 3);
							}
							rb.AddForce(vector6 * num10 * (PlayerControl.GetHand(this.spellCaster.ragdollHand.side).gripPressed ? 2f : 0.5f), 5);
						}
						bool flag17 = this.webbedItem && (this.webbedItem.transform.position - this.spellCaster.ragdollHand.transform.position).sqrMagnitude < 1.44f;
						if (flag17)
						{
							this.spellCaster.ragdollHand.Grab(this.webbedItem.GetMainHandle(this.spellCaster.ragdollHand.side));
							this.CancelWeb();
						}
					}
					else
					{
						this.activeWeb.enabled = false;
					}
				}
				else
				{
					this.CancelWeb();
				}
			}
			HashSet<Spell.Tether> hashSet = Spell.tetherToTime.Keys.ToHashSet<Spell.Tether>();
			foreach (Spell.Tether tether in hashSet)
			{
				bool flag18 = Spell.tetherToTime[tether] <= 0f;
				if (flag18)
				{
					tether.Destroy();
					Spell.tetherToTime.Remove(tether);
				}
				else
				{
					tether.Pull();
					Dictionary<Spell.Tether, float> dictionary = Spell.tetherToTime;
					Spell.Tether tether2 = tether;
					dictionary[tether2] -= Time.deltaTime;
				}
			}
			HashSet<Spell.Web> hashSet2 = Spell.webToTime.Keys.ToHashSet<Spell.Web>();
			foreach (Spell.Web web in hashSet2)
			{
				bool flag19 = Spell.webToTime[web] <= 0f;
				if (flag19)
				{
					web.Destroy();
					Spell.webToTime.Remove(web);
				}
				else
				{
					Dictionary<Spell.Web, float> dictionary2 = Spell.webToTime;
					Spell.Web web2 = web;
					dictionary2[web2] -= Time.deltaTime;
				}
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000031B0 File Offset: 0x000013B0
		public void CancelWeb()
		{
			this.acting = false;
			this.activeWeb.enabled = false;
			this.attachedRB = null;
			this.webbedPoint = Vector3.zero;
			bool flag = this.reticle;
			if (flag)
			{
				this.reticle.enabled = true;
			}
			this.webbedItem = null;
			this.webbedCreature = null;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00003210 File Offset: 0x00001410
		public override void Unload()
		{
			base.Unload();
			HashSet<Spell.Tether> hashSet = Spell.tetherToTime.Keys.ToHashSet<Spell.Tether>();
			foreach (Spell.Tether tether in hashSet)
			{
				tether.Destroy();
			}
			Spell.tetherToTime.Clear();
			Object.Destroy(this.spellCaster.ragdollHand.gameObject.GetComponent<Spell.HandTetherAbility>());
			Object.Destroy(this.activeWebHolder);
			bool flag = this.reticleHolder;
			if (flag)
			{
				Object.Destroy(this.reticleHolder);
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000032C8 File Offset: 0x000014C8
		public static void AttemptTether()
		{
			bool flag = Spell.leftInstance == null || Spell.rightInstance == null;
			if (!flag)
			{
				bool flag2 = Spell.leftInstance.webbedPoint != Vector3.zero && Spell.rightInstance.webbedPoint != Vector3.zero;
				if (flag2)
				{
					Spell.webToTime.Add(new Spell.Web(Spell.leftInstance.webbedPoint, Spell.rightInstance.webbedPoint), Spell.tetherTime);
				}
				bool flag3 = Spell.leftInstance.attachedRB && Spell.rightInstance.attachedRB;
				if (flag3)
				{
					Spell.tetherToTime.Add(new Spell.Tether(Spell.leftInstance.attachedRB, Spell.rightInstance.attachedRB, Vector3.zero, Spell.leftInstance.offset, Spell.rightInstance.offset), Spell.tetherTime);
				}
				bool flag4 = Spell.rightInstance.attachedRB && !Spell.leftInstance.attachedRB && Spell.leftInstance.webbedPoint != Vector3.zero;
				if (flag4)
				{
					Spell.tetherToTime.Add(new Spell.Tether(Spell.rightInstance.attachedRB, null, Spell.leftInstance.webbedPoint, Spell.rightInstance.offset, Vector3.zero), Spell.tetherTime);
				}
				bool flag5 = !Spell.rightInstance.attachedRB && Spell.leftInstance.attachedRB && Spell.rightInstance.webbedPoint != Vector3.zero;
				if (flag5)
				{
					Spell.tetherToTime.Add(new Spell.Tether(Spell.leftInstance.attachedRB, null, Spell.rightInstance.webbedPoint, Spell.leftInstance.offset, Vector3.zero), Spell.tetherTime);
				}
				Spell.leftInstance.CancelWeb();
				Spell.rightInstance.CancelWeb();
			}
		}

		// Token: 0x04000001 RID: 1
		public static Spell leftInstance;

		// Token: 0x04000002 RID: 2
		public static Spell rightInstance;

		// Token: 0x04000003 RID: 3
		public static float webPower = 2f;

		// Token: 0x04000004 RID: 4
		public static float tetherPower = 60f;

		// Token: 0x04000005 RID: 5
		public static float tetherTime = 30f;

		// Token: 0x04000006 RID: 6
		public static float webElasticity = 100f;

		// Token: 0x04000007 RID: 7
		public float maxRange;

		// Token: 0x04000008 RID: 8
		private static bool bindedEvent;

		// Token: 0x04000009 RID: 9
		public bool acting;

		// Token: 0x0400000A RID: 10
		private GameObject activeWebHolder;

		// Token: 0x0400000B RID: 11
		private GameObject reticleHolder;

		// Token: 0x0400000C RID: 12
		private Item webbedItem;

		// Token: 0x0400000D RID: 13
		private Creature webbedCreature;

		// Token: 0x0400000E RID: 14
		public Vector3 offset;

		// Token: 0x0400000F RID: 15
		public Vector3 webbedPoint;

		// Token: 0x04000010 RID: 16
		private Rigidbody attachedRB;

		// Token: 0x04000011 RID: 17
		private LineRenderer activeWeb;

		// Token: 0x04000012 RID: 18
		private LineRenderer reticle;

		// Token: 0x04000013 RID: 19
		private float webDistance;

		// Token: 0x04000014 RID: 20
		public static Dictionary<Spell.Tether, float> tetherToTime = new Dictionary<Spell.Tether, float>();

		// Token: 0x04000015 RID: 21
		public static Dictionary<Spell.Web, float> webToTime = new Dictionary<Spell.Web, float>();

		// Token: 0x04000016 RID: 22
		public static Spell latestInstance;

		// Token: 0x04000017 RID: 23
		public Color webColor;

		// Token: 0x04000018 RID: 24
		public bool visibleStringAim;

		// Token: 0x04000019 RID: 25
		public float webSize;

		// Token: 0x02000004 RID: 4
		public class Web
		{
			// Token: 0x06000015 RID: 21 RVA: 0x0000385C File Offset: 0x00001A5C
			public Web(Vector3 point1, Vector3 point2)
			{
				this.lineRenderer = (this.gameObject = GameObject.CreatePrimitive(3)).AddComponent<LineRenderer>();
				this.firstPoint = point1;
				this.secondPoint = point2;
				this.gameObject.transform.position = Vector3.Lerp(point1, point2, 0.5f);
				this.gameObject.transform.up = (point1 - point2).normalized;
				this.gameObject.transform.localScale = new Vector3(0.03f, (point1 - point2).magnitude, 0.03f);
				NavMeshObstacle navMeshObstacle = this.gameObject.AddComponent<NavMeshObstacle>();
				navMeshObstacle.shape = 1;
				navMeshObstacle.center = Vector3.zero;
				navMeshObstacle.size = Vector3.one;
				navMeshObstacle.carving = true;
				navMeshObstacle.carveOnlyStationary = false;
				this.lineRenderer.material = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
				this.lineRenderer.material.SetColor("_BaseColor", Spell.latestInstance.webColor);
				this.lineRenderer.widthMultiplier = 0.0075f;
				Object.Destroy(this.gameObject.GetComponent<MeshRenderer>());
				this.lineRenderer.SetPositions(new Vector3[] { point1, point2 });
			}

			// Token: 0x06000016 RID: 22 RVA: 0x000039C0 File Offset: 0x00001BC0
			public void Destroy()
			{
				Object.Destroy(this.gameObject);
			}

			// Token: 0x04000024 RID: 36
			public GameObject gameObject;

			// Token: 0x04000025 RID: 37
			public LineRenderer lineRenderer;

			// Token: 0x04000026 RID: 38
			public Vector3 firstPoint;

			// Token: 0x04000027 RID: 39
			public Vector3 secondPoint;
		}

		// Token: 0x02000005 RID: 5
		public class Tether
		{
			// Token: 0x06000017 RID: 23 RVA: 0x000039D0 File Offset: 0x00001BD0
			public Tether(Rigidbody rb, Rigidbody rb2, Vector3 point, Vector3 offset1, Vector3 offset2)
			{
				this.rb = rb;
				this.rb2 = rb2;
				bool flag = this.rb2 != null;
				if (flag)
				{
					this.rb2Exists = true;
				}
				this.point = point;
				this.offset1 = offset1;
				this.offset2 = offset2;
				this.possibleCreature = rb.GetComponentInParent<Creature>();
				bool flag2 = rb2 != null;
				if (flag2)
				{
					this.secondPossibleCreature = rb2.GetComponentInParent<Creature>();
				}
				bool flag3 = this.possibleCreature;
				if (flag3)
				{
					this.firstCreatureExists = true;
				}
				bool flag4 = this.secondCreatureExists;
				if (flag4)
				{
					this.secondCreatureExists = true;
				}
				this.holder = new GameObject();
				this.web = this.holder.AddComponent<LineRenderer>();
				this.web.material = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
				this.web.material.SetColor("_BaseColor", Spell.rightInstance.webColor);
				this.web.widthMultiplier = 0.0032f * Spell.leftInstance.webSize;
				this.web.enabled = true;
				bool flag5 = this.possibleCreature != null && this.secondPossibleCreature != null && this.possibleCreature == this.secondPossibleCreature;
				if (flag5)
				{
					RagdollPart.Type type = 4;
					RagdollPart.Type type2 = 4;
					foreach (Rigidbody rigidbody in this.possibleCreature.ragdoll.parts.Select((RagdollPart part) => part.rb))
					{
						bool flag6 = rigidbody == rb;
						if (flag6)
						{
							type = rigidbody.GetComponent<RagdollPart>().type;
						}
					}
					foreach (Rigidbody rigidbody2 in this.possibleCreature.ragdoll.parts.Select((RagdollPart part) => part.rb))
					{
						bool flag7 = rigidbody2 == rb2;
						if (flag7)
						{
							type2 = rigidbody2.GetComponent<RagdollPart>().type;
						}
					}
					bool flag8 = (type == 128 || type == 256 || type == 1024 || type == 512) && (type2 == 128 || type2 == 256 || type2 == 1024 || type2 == 512);
					if (flag8)
					{
						this.shouldTrip = true;
					}
					bool flag9 = (type == 8 || type == 32 || type == 16 || type == 64) && (type2 == 8 || type2 == 32 || type2 == 16 || type2 == 64);
					if (flag9)
					{
						this.shouldDisarm = true;
					}
					bool flag10 = (type == 8 || type == 32 || type == 16 || type == 64) && (type2 == 128 || type2 == 256 || type2 == 1024 || type2 == 512);
					if (flag10)
					{
						this.shouldDisarm = true;
						this.shouldTrip = true;
					}
					bool flag11 = (type == 128 || type == 256 || type == 1024 || type == 512) && (type2 == 8 || type2 == 32 || type2 == 16 || type2 == 64);
					if (flag11)
					{
						this.shouldDisarm = true;
						this.shouldTrip = true;
					}
				}
			}

			// Token: 0x06000018 RID: 24 RVA: 0x00003D90 File Offset: 0x00001F90
			public void Destroy()
			{
				Object.Destroy(this.holder);
			}

			// Token: 0x06000019 RID: 25 RVA: 0x00003DA0 File Offset: 0x00001FA0
			public void Pull()
			{
				bool flag = this.rb == null;
				if (!flag)
				{
					bool flag2 = this.rb2Exists && this.rb2 == null;
					if (!flag2)
					{
						bool flag3 = !this.possibleCreature && this.firstCreatureExists;
						if (!flag3)
						{
							bool flag4 = !this.secondPossibleCreature && this.secondCreatureExists;
							if (!flag4)
							{
								try
								{
									this.web.SetPositions(new Vector3[]
									{
										this.rb.transform.TransformPoint(this.offset1),
										(this.rb2 != null) ? this.rb2.transform.TransformPoint(this.offset2) : this.point
									});
								}
								catch
								{
								}
								float num = Vector3.Distance(this.rb.position, (this.rb2 != null) ? this.rb2.position : this.point);
								bool flag5 = this.rb && this.rb2 != null && this.point == Vector3.zero;
								if (flag5)
								{
									this.rb.AddForce((this.rb2.transform.position - this.rb.transform.position).normalized * Spell.tetherPower * Spell.tetherPower * Mathf.Min(num, 10f) * Time.timeScale);
									this.rb2.AddForce((this.rb.transform.position - this.rb2.transform.position).normalized * Spell.tetherPower * Spell.tetherPower * Mathf.Min(num, 10f) * Time.timeScale);
								}
								bool flag6 = this.rb && this.point != Vector3.zero && this.rb2 == null;
								if (flag6)
								{
									this.rb.AddForce((this.point - this.rb.transform.position).normalized * Spell.tetherPower * Spell.tetherPower * Mathf.Min(num, 10f) * Time.timeScale);
								}
								bool flag7 = this.possibleCreature && !this.possibleCreature.isKilled && this.point != Vector3.zero;
								if (flag7)
								{
									this.possibleCreature.ragdoll.SetState(1);
								}
								bool flag8 = this.secondPossibleCreature && !this.secondPossibleCreature.isKilled && this.point != Vector3.zero;
								if (flag8)
								{
									this.secondPossibleCreature.ragdoll.SetState(1);
								}
								bool flag9 = this.shouldTrip && !this.possibleCreature.isKilled;
								if (flag9)
								{
									this.possibleCreature.ragdoll.SetState(1);
								}
								bool flag10 = this.shouldDisarm && this.possibleCreature.handRight.grabbedHandle;
								if (flag10)
								{
									this.possibleCreature.handRight.UnGrab(false);
								}
								bool flag11 = this.shouldDisarm && this.possibleCreature.handLeft.grabbedHandle;
								if (flag11)
								{
									this.possibleCreature.handLeft.UnGrab(false);
								}
							}
						}
					}
				}
			}

			// Token: 0x04000028 RID: 40
			public Rigidbody rb;

			// Token: 0x04000029 RID: 41
			public Rigidbody rb2;

			// Token: 0x0400002A RID: 42
			public Vector3 point;

			// Token: 0x0400002B RID: 43
			public Vector3 offset1;

			// Token: 0x0400002C RID: 44
			public Vector3 offset2;

			// Token: 0x0400002D RID: 45
			public Creature possibleCreature;

			// Token: 0x0400002E RID: 46
			public Creature secondPossibleCreature;

			// Token: 0x0400002F RID: 47
			private bool rb2Exists;

			// Token: 0x04000030 RID: 48
			private bool firstCreatureExists;

			// Token: 0x04000031 RID: 49
			private bool secondCreatureExists;

			// Token: 0x04000032 RID: 50
			private bool shouldTrip;

			// Token: 0x04000033 RID: 51
			private bool shouldDisarm;

			// Token: 0x04000034 RID: 52
			private GameObject holder;

			// Token: 0x04000035 RID: 53
			private LineRenderer web;
		}

		// Token: 0x02000006 RID: 6
		private class HandTetherAbility : MonoBehaviour
		{
			// Token: 0x0600001A RID: 26 RVA: 0x0000419C File Offset: 0x0000239C
			private void OnCollisionEnter(Collision collision)
			{
				bool flag = collision.collider.transform.name == "Palm";
				if (flag)
				{
					Spell.AttemptTether();
				}
			}
		}

		// Token: 0x02000007 RID: 7
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000036 RID: 54
			public static EventManager.CreatureSpawnedEvent <0>__Spawn;

			// Token: 0x04000037 RID: 55
			public static EventManager.LevelLoadEvent <1>__UnLoad;
		}
	}
}
