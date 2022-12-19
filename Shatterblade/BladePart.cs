using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExtensionMethods;
using Shatterblade.Modes;
using ThunderRoad;
using UnityEngine;

namespace Shatterblade
{
	// Token: 0x0200000E RID: 14
	public class BladePart : MonoBehaviour
	{
		// Token: 0x060000DC RID: 220 RVA: 0x00006C50 File Offset: 0x00004E50
		public void Awake()
		{
			this.item = base.GetComponent<Item>();
			this.item.disallowDespawn = true;
			this.item.OnGrabEvent += delegate(Handle handle, RagdollHand hand)
			{
				this.Detach(false);
			};
			this.item.OnUngrabEvent += delegate(Handle handle, RagdollHand hand, bool throwing)
			{
				this.lastUngrab = Time.time;
			};
			this.item.OnDespawnEvent += delegate(EventTime time)
			{
				this.targetPoint = null;
				this.sword.parts.Remove(this);
			};
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00006CBE File Offset: 0x00004EBE
		public void DeInit()
		{
			this.Detach(false);
			Object.Destroy(this);
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00006CD0 File Offset: 0x00004ED0
		public void SetCollision(bool enable)
		{
			foreach (Collider collider in this.item.colliderGroups.First<ColliderGroup>().colliders)
			{
				collider.enabled = enable;
			}
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00006D38 File Offset: 0x00004F38
		public void IgnoreRagdoll(Ragdoll ragdoll, bool ignore)
		{
			foreach (RagdollPart part in ragdoll.parts)
			{
				foreach (ColliderGroup cg in this.item.colliderGroups)
				{
					foreach (Collider thisCollider in cg.colliders)
					{
						foreach (Collider collider in part.colliderGroup.colliders)
						{
							Physics.IgnoreCollision(thisCollider, collider, ignore);
						}
					}
				}
			}
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00006E60 File Offset: 0x00005060
		public void IgnoreHand(RagdollHand hand, bool ignore, float delay = 0f)
		{
			this.RunAfter(delegate
			{
				bool flag = this.item.mainHandler == hand;
				if (!flag)
				{
					foreach (Collider otherCollider in hand.colliderGroup.colliders)
					{
						foreach (ColliderGroup cg in this.item.colliderGroups)
						{
							foreach (Collider collider in cg.colliders)
							{
								Physics.IgnoreCollision(collider, otherCollider, ignore);
							}
						}
					}
					foreach (Collider otherCollider2 in hand.lowerArmPart.colliderGroup.colliders)
					{
						foreach (ColliderGroup cg2 in this.item.colliderGroups)
						{
							foreach (Collider collider2 in cg2.colliders)
							{
								Physics.IgnoreCollision(collider2, otherCollider2, ignore);
							}
						}
					}
					foreach (Collider otherCollider3 in hand.upperArmPart.colliderGroup.colliders)
					{
						foreach (ColliderGroup cg3 in this.item.colliderGroups)
						{
							foreach (Collider collider3 in cg3.colliders)
							{
								Physics.IgnoreCollision(collider3, otherCollider3, ignore);
							}
						}
					}
				}
			}, delay);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00006EA0 File Offset: 0x000050A0
		public void Init(Shatterblade sword, Rigidbody targetPoint, int index)
		{
			this.index = index;
			this.sword = sword;
			this.wasInHolder = sword.item.holder != null;
			this.targetPoint = targetPoint;
			this.SetCollision(false);
			for (int i = 0; i < this.item.mainHandleLeft.handlers.Count; i++)
			{
				RagdollHand handler = this.item.mainHandleLeft.handlers[i];
				if (handler != null)
				{
					handler.UnGrab(false);
				}
			}
			this.item.mainHandleLeft.SetTouch(false);
			this.lastTargetRotationDelta = Quaternion.Inverse(sword.transform.rotation) * targetPoint.rotation;
			this.lastTargetPositionDelta = sword.transform.InverseTransformPoint(targetPoint.position);
			this.item.collisionHandlers[0].enabled = false;
			this.item.collisionHandlers[0].OnCollisionStartEvent += delegate(CollisionInstance collision)
			{
				bool isLocked = this.IsLocked;
				if (isLocked)
				{
					sword.BladeHaptic(collision.impactVelocity.magnitude);
				}
			};
			for (int j = 0; j < this.item.colliderGroups.Count; j++)
			{
				ColliderGroup cg = this.item.colliderGroups[j];
				for (int k = 0; k < cg.data.modifiers.Count; k++)
				{
					ColliderGroupData.Modifier modifier = cg.data.modifiers[k];
					modifier.imbueRate = 2f;
				}
			}
			this.item.mainHandleLeft.touchRadius = 0.1f;
			for (int l = 0; l < this.item.collisionHandlers.Count; l++)
			{
				CollisionHandler ch = this.item.collisionHandlers[l];
				bool flag = ch.damagers == null;
				if (!flag)
				{
					for (int m = 0; m < ch.damagers.Count; m++)
					{
						Damager damager = ch.damagers[m];
						bool flag2;
						if (damager == null)
						{
							flag2 = false;
						}
						else
						{
							DamagerData data = damager.data;
							if (data == null)
							{
								flag2 = false;
							}
							else
							{
								float playerDamageMultiplier = data.playerDamageMultiplier;
								flag2 = true;
							}
						}
						bool flag3 = flag2;
						if (flag3)
						{
							damager.data.playerDamageMultiplier = 0.01f * sword.module.damageModifier;
						}
					}
				}
			}
			this.Show();
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00007148 File Offset: 0x00005348
		public void ForAllRenderers(Action<MeshRenderer> action)
		{
			foreach (MeshRenderer renderer in base.GetComponentsInChildren<MeshRenderer>())
			{
				action(renderer);
			}
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00007178 File Offset: 0x00005378
		public IEnumerator ShowCoroutine()
		{
			this.item.gameObject.SetActive(true);
			this.SetScale(0f);
			float startTime = Time.time;
			while (Time.time - startTime < 0.5f)
			{
				float ratio = ((Time.time - startTime) / 0.5f).Clamp(0f, 1f).Curve(new float[] { 0f, 0.25f, 1f });
				this.SetScale(ratio);
				yield return 0;
			}
			this.SetScale(1f);
			this.SetCollision(true);
			this.item.collisionHandlers.FirstOrDefault<CollisionHandler>().enabled = true;
			this.item.mainHandleLeft.SetTouch(true);
			yield break;
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00007188 File Offset: 0x00005388
		public void SetScale(float scale)
		{
			bool flag = base.transform.parent;
			if (flag)
			{
				Transform parent = base.transform.parent;
				base.transform.SetParent(null);
				base.transform.localScale = Vector3.one * scale;
				base.transform.SetParent(parent);
			}
			else
			{
				base.transform.localScale = Vector3.one * scale;
			}
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00007204 File Offset: 0x00005404
		public IEnumerator HideCoroutine()
		{
			float startTime = Time.time;
			this.SetScale(1f);
			while (Time.time - startTime < 0.5f)
			{
				float ratio = (1f - (Time.time - startTime) / 0.5f).Clamp(0f, 1f).Curve(new float[] { 0f, 0.25f, 1f });
				this.SetScale(ratio);
				yield return 0;
			}
			this.SetCollision(false);
			this.SetScale(0f);
			this.item.mainHandleLeft.Release();
			this.item.mainHandleLeft.SetTouch(false);
			yield break;
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x00007213 File Offset: 0x00005413
		public bool IsLocked
		{
			get
			{
				return this.state == PartState.Locked;
			}
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x0000721E File Offset: 0x0000541E
		public bool IsFree()
		{
			return this.state == PartState.Free;
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00007229 File Offset: 0x00005429
		public bool IsReforming()
		{
			return this.state == PartState.Reforming;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00007234 File Offset: 0x00005434
		public void Show()
		{
			base.StartCoroutine(this.ShowCoroutine());
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00007243 File Offset: 0x00005443
		public void Hide()
		{
			base.StartCoroutine(this.HideCoroutine());
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00007252 File Offset: 0x00005452
		public void Flash()
		{
			base.StartCoroutine(this.FlashCoroutine());
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00007261 File Offset: 0x00005461
		public IEnumerator FlashCoroutine()
		{
			bool flag = this.isFlashing;
			if (flag)
			{
				yield break;
			}
			this.isFlashing = true;
			float start = Time.time;
			Dictionary<Renderer, Color> initialEmissive = new Dictionary<Renderer, Color>();
			int num;
			for (int i = 0; i < this.item.renderers.Count; i = num + 1)
			{
				Renderer renderer = this.item.renderers[i];
				initialEmissive[renderer] = renderer.material.GetColor(BladePart.EmissionColor);
				renderer = null;
				num = i;
			}
			Color targetColor = Color.HSVToRGB(Random.Range(0f, 1f), 1f, 100f, true);
			while (Time.time - start < 0.3f)
			{
				float value = Mathf.Sin(3.1415927f * (Time.time - start) / 0.3f);
				for (int j = 0; j < this.item.renderers.Count; j = num + 1)
				{
					Renderer renderer2 = this.item.renderers[j];
					renderer2.material.SetColor(BladePart.EmissionColor, Color.Lerp(initialEmissive[renderer2], targetColor, Mathf.Sin(3.1415927f * (Time.time - start) / 0.3f)));
					renderer2 = null;
					num = j;
				}
				yield return 0;
			}
			this.isFlashing = false;
			for (int k = 0; k < this.item.renderers.Count; k = num + 1)
			{
				Renderer renderer3 = this.item.renderers[k];
				renderer3.material.SetColor(BladePart.EmissionColor, initialEmissive[renderer3]);
				renderer3 = null;
				num = k;
			}
			yield break;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00007270 File Offset: 0x00005470
		public void Update()
		{
			bool isCulled = this.item.isCulled;
			if (isCulled)
			{
				this.sword.parts.Remove(this);
				this.sword.CheckForMissingParts();
				Object.Destroy(this, 0f);
			}
			else
			{
				try
				{
					Shatterblade shatterblade = this.sword;
					bool flag = ((shatterblade != null) ? shatterblade.gameObject.transform : null) == null;
					if (flag)
					{
						this.sword = null;
					}
				}
				catch (NullReferenceException)
				{
					this.state = PartState.Free;
					this.item.mainHandleLeft.SetTelekinesis(true);
					this.item.RemovePhysicModifier(this);
					this.DestroyJoint();
					Object.Destroy(this);
				}
				bool flag2 = this.sword == null;
				if (!flag2)
				{
					bool flag3 = this.sword.item.holder == null && this.sword.item.transform.localScale != this.lastSwordScale;
					if (flag3)
					{
						this.SetScale(this.sword.item.transform.localScale.x);
						this.lastSwordScale = this.sword.item.transform.localScale;
						this.Detach(false);
						this.Reform();
					}
					bool flag4 = this.state == PartState.Reforming;
					if (flag4)
					{
						this.item.SetColliderAndMeshLayer(GameManager.GetLayer(7), false);
					}
					else
					{
						bool flag5 = Time.time - this.lastUngrab > 1f;
						if (flag5)
						{
							this.item.SetColliderLayer(GameManager.GetLayer(15));
						}
						this.item.SetMeshLayer(GameManager.GetLayer(5));
					}
					bool flag6 = !this.targetPoint;
					if (!flag6)
					{
						bool flag7 = this.joint && !this.IsLocked && this.sword.ShouldReform(this);
						if (flag7)
						{
							this.SetDriveFactor(Mathf.InverseLerp(2f, 0f, Vector3.Distance(base.transform.position, this.targetPoint.transform.position)));
							bool flag8 = Vector3.Distance(base.transform.position, this.targetPoint.transform.position) > 6f;
							if (flag8)
							{
								base.transform.position = Vector3.Lerp(base.transform.position, this.targetPoint.transform.position, Time.deltaTime * Mathf.Sqrt(Vector3.Distance(base.transform.position, this.targetPoint.transform.position) - 6f));
							}
							this.item.rb.velocity = Vector3.Lerp(this.item.rb.velocity, Vector3.Project(this.item.rb.velocity, base.transform.position - this.targetPoint.transform.position), Time.deltaTime * 6f);
						}
						bool flag9 = this.joint && this.IsLocked;
						if (flag9)
						{
							bool isLocked = this.IsLocked;
							if (isLocked)
							{
								bool flag10 = Vector3.Distance(base.transform.position, this.targetPoint.transform.position) > 0.5f;
								if (flag10)
								{
									base.transform.position = Vector3.Lerp(base.transform.position, this.targetPoint.transform.position, Time.deltaTime * Mathf.Sqrt(Vector3.Distance(base.transform.position, this.targetPoint.transform.position)));
								}
								bool flag11 = this.lastTargetRotationDelta != Quaternion.Inverse(this.sword.transform.rotation) * this.targetPoint.rotation;
								if (flag11)
								{
									this.RefreshJoint();
									this.lastTargetRotationDelta = Quaternion.Inverse(this.sword.transform.rotation) * this.targetPoint.rotation;
								}
								bool flag12 = this.lastTargetPositionDelta != this.sword.transform.InverseTransformPoint(this.targetPoint.position);
								if (flag12)
								{
									this.RefreshJoint();
									this.lastTargetPositionDelta = this.sword.transform.InverseTransformPoint(this.targetPoint.position);
								}
							}
						}
						bool flag13 = !this.sword.ShouldReform(this);
						if (!flag13)
						{
							bool flag14 = !this.item.holder;
							if (flag14)
							{
								bool flag15 = this.sword.item.holder != null && this.IsLocked && this.sword.ShouldHide(this);
								if (flag15)
								{
									bool flag16 = !this.wasInHolder;
									if (flag16)
									{
										this.wasInHolder = true;
										this.item.rb.isKinematic = true;
										this.item.transform.position = this.targetPoint.transform.position;
										this.item.transform.rotation = this.targetPoint.transform.rotation;
										this.item.transform.SetParent(this.targetPoint.transform);
										this.Hide();
									}
								}
								else
								{
									bool flag17 = this.wasInHolder;
									if (flag17)
									{
										this.wasInHolder = false;
										this.item.rb.isKinematic = false;
										this.item.transform.SetParent(null);
										this.Show();
										this.Reform();
									}
								}
							}
							bool flag18 = !this.IsLocked && Time.time - this.lastUnlockTime > 0.1f;
							if (flag18)
							{
								bool flag19 = !this.joint;
								if (flag19)
								{
									bool flag20 = !this.item.mainHandler;
									if (flag20)
									{
										this.Reform();
									}
								}
								bool flag21 = Vector3.Distance(base.transform.position, this.targetPoint.transform.position) >= 0.1f || Quaternion.Angle(base.transform.rotation, this.targetPoint.transform.rotation) >= 10f || !this.sword.ShouldPartLock(this);
								if (!flag21)
								{
									this.state = PartState.Locked;
									this.item.mainHandleLeft.SetTelekinesis(false);
									bool flag22 = !(this.sword.mode is CannonMode);
									if (flag22)
									{
										this.sword.item.handlers.ForEach(delegate(RagdollHand handler)
										{
											handler.HapticTick(1f, 10f);
										});
									}
									bool discoMode = this.sword.module.discoMode;
									if (discoMode)
									{
										this.Flash();
									}
									this.Attach();
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060000EE RID: 238 RVA: 0x000079CC File Offset: 0x00005BCC
		public void Depenetrate()
		{
			this.item.mainCollisionHandler.damagers.ForEach(delegate(Damager damager)
			{
				damager.UnPenetrateAll();
			});
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00007A04 File Offset: 0x00005C04
		public void RefreshJoint()
		{
			Quaternion orgRotation = base.transform.rotation;
			base.transform.rotation = this.targetPoint.transform.rotation;
			this.joint.autoConfigureConnectedAnchor = false;
			this.joint.targetRotation = Quaternion.identity;
			this.joint.anchor = Vector3.zero;
			bool shouldLock = this.sword.shouldLock;
			if (shouldLock)
			{
				this.joint.connectedAnchor = this.sword.transform.InverseTransformPoint(this.targetPoint.transform.position);
				this.joint.connectedBody = this.sword.item.rb;
				this.joint.massScale = 20f;
			}
			else
			{
				this.joint.connectedAnchor = Vector3.zero;
				this.joint.connectedBody = this.targetPoint;
			}
			base.transform.rotation = orgRotation;
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00007B08 File Offset: 0x00005D08
		public void Attach()
		{
			this.item.SetPhysicModifier(this, new float?(0f), 1f, 0.01f, 0.05f, -1f, null);
			this.Depenetrate();
			this.DestroyJoint();
			this.CreateJoint(true);
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00007B58 File Offset: 0x00005D58
		public void DisableItem()
		{
			this.item.enabled = false;
			this.item.rb.isKinematic = true;
			this.item.SetColliderLayer(this.sword.item.currentPhysicsLayer);
			this.sword.item.collisionHandlers.AddRange(this.item.collisionHandlers);
			this.sword.item.colliderGroups.AddRange(this.item.colliderGroups);
			this.sword.item.RefreshCollision(false);
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00007BF8 File Offset: 0x00005DF8
		public void EnableItem()
		{
			this.item.enabled = true;
			this.item.rb.isKinematic = false;
			this.sword.item.collisionHandlers.RemoveAll((CollisionHandler handler) => this.item.collisionHandlers.Contains(handler));
			this.sword.item.colliderGroups.RemoveAll((ColliderGroup group) => this.item.colliderGroups.Contains(group));
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00007C6C File Offset: 0x00005E6C
		public void Detach(bool shouldThrow = false)
		{
			this.state = PartState.Free;
			this.item.mainHandleLeft.SetTelekinesis(true);
			this.item.RemovePhysicModifier(this);
			if (shouldThrow)
			{
				this.item.rb.velocity = this.sword.item.rb.GetPointVelocity(this.targetPoint.transform.position) * 3f;
			}
			this.DestroyJoint();
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00007CEC File Offset: 0x00005EEC
		public void Reform()
		{
			this.Detach(false);
			this.state = PartState.Reforming;
			this.item.mainHandleLeft.SetTelekinesis(false);
			this.Depenetrate();
			this.item.SetPhysicModifier(this, new float?(0f), 1f, -1f, -1f, -1f, null);
			this.DestroyJoint();
			this.CreateJoint(false);
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00007D60 File Offset: 0x00005F60
		public void SetDriveFactor(float factor)
		{
			JointDrive posDrive = this.joint.xDrive;
			posDrive.positionSpring = 2000f;
			posDrive.positionDamper = Mathf.Lerp(20f, 100f, factor);
			posDrive.maximumForce = this.sword.module.jointMaxForce;
			this.joint.xDrive = posDrive;
			this.joint.yDrive = posDrive;
			this.joint.zDrive = posDrive;
			this.sword.ModifyJoint(this);
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00007DEC File Offset: 0x00005FEC
		public void CreateJoint(bool toSword = false)
		{
			bool flag = this.joint;
			if (!flag)
			{
				Quaternion orgRotation = base.transform.rotation;
				base.transform.rotation = this.targetPoint.transform.rotation;
				this.joint = base.gameObject.AddComponent<ConfigurableJoint>();
				this.joint.autoConfigureConnectedAnchor = false;
				this.joint.targetRotation = Quaternion.identity;
				this.joint.anchor = Vector3.zero;
				bool flag2 = toSword && this.sword.shouldLock;
				if (flag2)
				{
					this.joint.connectedAnchor = this.sword.transform.InverseTransformPoint(this.targetPoint.transform.position);
					this.joint.connectedBody = this.sword.item.rb;
					this.joint.massScale = 20f;
				}
				else
				{
					this.joint.connectedAnchor = Vector3.zero;
					this.joint.connectedBody = this.targetPoint;
				}
				JointDrive jointDrive = default(JointDrive);
				jointDrive.positionSpring = 2000f;
				jointDrive.positionDamper = 120f;
				jointDrive.maximumForce = this.sword.module.jointMaxForce;
				JointDrive posDrive = jointDrive;
				jointDrive = default(JointDrive);
				jointDrive.positionSpring = 1000f;
				jointDrive.positionDamper = 10f;
				jointDrive.maximumForce = this.sword.module.jointMaxForce;
				JointDrive rotDrive = jointDrive;
				this.joint.rotationDriveMode = 0;
				this.joint.xDrive = posDrive;
				this.joint.yDrive = posDrive;
				this.joint.zDrive = posDrive;
				this.joint.angularXDrive = rotDrive;
				this.joint.angularYZDrive = rotDrive;
				base.transform.rotation = orgRotation;
				this.joint.angularXMotion = 2;
				this.joint.angularYMotion = 2;
				this.joint.angularZMotion = 2;
				this.joint.xMotion = 2;
				this.joint.yMotion = 2;
				this.joint.zMotion = 2;
				this.sword.ModifyJoint(this);
			}
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00008040 File Offset: 0x00006240
		public void DestroyJoint()
		{
			bool flag = this.joint;
			if (flag)
			{
				Object.Destroy(this.joint);
				this.lastUnlockTime = Time.time;
				this.joint = null;
			}
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00008080 File Offset: 0x00006280
		public float DistanceToClosestHand()
		{
			return Mathf.Min(Vector3.Distance(base.transform.position, Player.currentCreature.GetHand(1).Palm()), Vector3.Distance(base.transform.position, Player.currentCreature.GetHand(1).Palm()));
		}

		// Token: 0x0400003E RID: 62
		private const float LOCKED_DRAG = 0.01f;

		// Token: 0x0400003F RID: 63
		private const float LOCKED_ANGULAR_DRAG = 0.05f;

		// Token: 0x04000040 RID: 64
		public ConfigurableJoint joint;

		// Token: 0x04000041 RID: 65
		public Shatterblade sword;

		// Token: 0x04000042 RID: 66
		public Rigidbody targetPoint;

		// Token: 0x04000043 RID: 67
		public Item item;

		// Token: 0x04000044 RID: 68
		private float lastUnlockTime;

		// Token: 0x04000045 RID: 69
		public bool wasInHolder;

		// Token: 0x04000046 RID: 70
		private Vector3 lastSwordScale;

		// Token: 0x04000047 RID: 71
		private float lastUngrab = 0f;

		// Token: 0x04000048 RID: 72
		public PartState state;

		// Token: 0x04000049 RID: 73
		private Quaternion lastTargetRotationDelta = Quaternion.identity;

		// Token: 0x0400004A RID: 74
		private Vector3 lastTargetPositionDelta = Vector3.zero;

		// Token: 0x0400004B RID: 75
		private bool isFlashing;

		// Token: 0x0400004C RID: 76
		private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

		// Token: 0x0400004D RID: 77
		public int index;
	}
}
