using System;
using System.Collections.Generic;
using System.Linq;
using Shatterblade.Modes;
using ThunderRoad;
using UnityEngine;

namespace Shatterblade
{
	// Token: 0x0200000C RID: 12
	public class Shatterblade : MonoBehaviour
	{
		// Token: 0x060000B0 RID: 176 RVA: 0x00005B51 File Offset: 0x00003D51
		public void Awake()
		{
			this.modes = new List<BladeMode>();
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00005B5F File Offset: 0x00003D5F
		public void Start()
		{
			this.modes = this.modes.OrderBy((BladeMode mode) => mode.Priority()).ToList<BladeMode>();
			this.Init();
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00005B9E File Offset: 0x00003D9E
		public void RegisterMode(BladeMode newMode)
		{
			this.modes.Add(newMode);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00005BB0 File Offset: 0x00003DB0
		public void Init()
		{
			bool flag;
			if (base.gameObject)
			{
				Creature currentCreature = Player.currentCreature;
				flag = ((currentCreature != null) ? currentCreature.handLeft : null) == null;
			}
			else
			{
				flag = true;
			}
			bool flag2 = flag;
			if (!flag2)
			{
				this.item = base.GetComponent<Item>();
				this.orgAxisLength = this.item.mainHandleLeft.axisLength;
				this.parts = new List<BladePart>();
				this.item.ResetCenterOfMass();
				this.animator = base.GetComponentInChildren<Animator>();
				this.item.OnDespawnEvent += new Item.SpawnEvent(this.OnDespawnEvent);
				this.ListenForHand(Player.currentCreature.handRight);
				this.ListenForHand(Player.currentCreature.handLeft);
				this.wasLocking = true;
				this.handleAnnotationA = Annotation.CreateAnnotation(this, this.item.mainHandleLeft.transform, base.transform, new Vector3(0f, 1f, -1f));
				this.handleAnnotationB = Annotation.CreateAnnotation(this, this.item.mainHandleLeft.transform, base.transform, new Vector3(0f, -1f, 1f));
				this.handleAnnotationA.Hide();
				this.handleAnnotationB.Hide();
				this.jointRBs = this.animator.GetComponentsInChildren<Rigidbody>().ToList<Rigidbody>();
				for (int i = 0; i < 15; i++)
				{
					this.isSpawning[i + 1] = true;
				}
				bool flag3 = this.item.handlers.Any<RagdollHand>();
				if (flag3)
				{
					this.SpawnAllParts();
				}
				else
				{
					this.item.OnGrabEvent += new Item.GrabDelegate(this.PickUpEvent);
				}
				this.ChangeMode<SwordMode>();
			}
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00005D71 File Offset: 0x00003F71
		public void GrabEvent(Side side, Handle handle, float axisPosition, HandlePose orientation, EventTime time)
		{
			this.IgnoreCollider(Player.currentCreature.GetHand(side), true, 0f);
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00005D8B File Offset: 0x00003F8B
		public void UnGrabEvent(Side side, Handle handle, bool throwing, EventTime time)
		{
			this.IgnoreCollider(Player.currentCreature.GetHand(side), false, 0.5f);
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00005DA5 File Offset: 0x00003FA5
		public void ListenForHand(RagdollHand hand)
		{
			hand.OnGrabEvent += new RagdollHand.GrabEvent(this.GrabEvent);
			hand.OnUnGrabEvent += new RagdollHand.UnGrabEvent(this.UnGrabEvent);
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00005DCE File Offset: 0x00003FCE
		public void StopListening(RagdollHand hand)
		{
			hand.OnGrabEvent -= new RagdollHand.GrabEvent(this.GrabEvent);
			hand.OnUnGrabEvent -= new RagdollHand.UnGrabEvent(this.UnGrabEvent);
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00005DF8 File Offset: 0x00003FF8
		public void OnDespawnEvent(EventTime time)
		{
			this.isDespawned = true;
			BladeMode bladeMode = this.mode;
			if (bladeMode != null)
			{
				bladeMode.Exit();
			}
			this.mode = null;
			this.StopListening(Player.currentCreature.handRight);
			this.StopListening(Player.currentCreature.handLeft);
			this.DetachParts(false);
			Annotation annotation = this.handleAnnotationA;
			if (annotation != null)
			{
				annotation.Destroy();
			}
			Annotation annotation2 = this.handleAnnotationB;
			if (annotation2 != null)
			{
				annotation2.Destroy();
			}
			Annotation annotation3 = this.otherHandAnnotation;
			if (annotation3 != null)
			{
				annotation3.Destroy();
			}
			Annotation annotation4 = this.imbueHandleAnnotation;
			if (annotation4 != null)
			{
				annotation4.Destroy();
			}
			Annotation annotation5 = this.gunShardAnnotation;
			if (annotation5 != null)
			{
				annotation5.Destroy();
			}
			Annotation annotation6 = this.sawShardAnnotation;
			if (annotation6 != null)
			{
				annotation6.Destroy();
			}
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00005EBC File Offset: 0x000040BC
		public void SpawnAllParts()
		{
			for (int i = 0; i < 15; i++)
			{
				this.SpawnPart(i + 1);
			}
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00005EE7 File Offset: 0x000040E7
		public void PickUpEvent(Handle handle, RagdollHand hand)
		{
			this.SpawnAllParts();
			this.item.OnGrabEvent -= new Item.GrabDelegate(this.PickUpEvent);
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00005F0C File Offset: 0x0000410C
		public void ShowAll()
		{
			foreach (BladePart part in this.parts)
			{
				part.Show();
			}
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00005F64 File Offset: 0x00004164
		public void SpawnPart(int i)
		{
			this.isSpawning[i] = true;
			Func<Rigidbody, bool> <>9__1;
			Catalog.GetData<ItemData>(string.Format("ShatterbladePart{0}", i), true).SpawnAsync(delegate(Item item)
			{
				IEnumerable<Rigidbody> enumerable = this.jointRBs;
				Func<Rigidbody, bool> func;
				if ((func = <>9__1) == null)
				{
					func = (<>9__1 = (Rigidbody obj) => obj.name == string.Format("Blade_{0}", i));
				}
				Rigidbody rigidbody = enumerable.FirstOrDefault(func);
				Rigidbody targetRB = ((rigidbody != null) ? rigidbody.gameObject.GetComponent<Rigidbody>() : null);
				item.transform.position = targetRB.transform.position;
				item.transform.rotation = targetRB.transform.rotation;
				BladePart part = item.gameObject.AddComponent<BladePart>();
				this.parts.Add(part);
				part.Init(this, targetRB, i);
				this.rbMap[targetRB] = part;
				this.partMap[part] = targetRB;
				this.isSpawning[i] = false;
				int i2 = i;
				int num = i2;
				if (num != 1)
				{
					switch (num)
					{
					case 10:
					{
						bool flag = this.gunShardAnnotation;
						if (flag)
						{
							Object.Destroy(this.gunShardAnnotation);
							this.gunShardAnnotation = null;
						}
						this.gunShardAnnotation = Annotation.CreateAnnotation(this, item.transform, this.item.transform, new Vector3(1f, -2f, 0f));
						break;
					}
					case 11:
					{
						bool flag2 = this.imbueHandleAnnotation;
						if (flag2)
						{
							Object.Destroy(this.imbueHandleAnnotation);
							this.imbueHandleAnnotation = null;
						}
						this.imbueHandleAnnotation = Annotation.CreateAnnotation(this, item.transform, this.item.transform, new Vector3(-1f, -2f, 0f));
						break;
					}
					case 12:
					{
						bool flag3 = this.sawShardAnnotation;
						if (flag3)
						{
							Object.Destroy(this.sawShardAnnotation);
							this.sawShardAnnotation = null;
						}
						this.sawShardAnnotation = Annotation.CreateAnnotation(this, item.transform, this.item.transform, new Vector3(-1f, 0.5f, 0f));
						break;
					}
					}
				}
				else
				{
					bool flag4 = this.otherHandAnnotation;
					if (flag4)
					{
						Object.Destroy(this.otherHandAnnotation);
						this.otherHandAnnotation = null;
					}
					this.otherHandAnnotation = Annotation.CreateAnnotation(this, item.transform, this.item.transform, new Vector3(1f, -1f, 0f));
				}
			}, null, null, null, true, null);
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00005FDC File Offset: 0x000041DC
		public void HideAllAnnotations()
		{
			this.imbueHandleAnnotation.Hide();
			this.otherHandAnnotation.Hide();
			this.gunShardAnnotation.Hide();
			this.sawShardAnnotation.Hide();
			this.handleAnnotationA.Hide();
			this.handleAnnotationB.Hide();
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00006034 File Offset: 0x00004234
		public void IgnoreOtherBladeParts(BladePart part)
		{
			foreach (BladePart otherPart in this.parts)
			{
				bool flag = part != otherPart;
				if (flag)
				{
					foreach (Collider collider in part.item.colliderGroups.First<ColliderGroup>().colliders)
					{
						foreach (Collider otherCollider in otherPart.item.colliderGroups.First<ColliderGroup>().colliders)
						{
							Physics.IgnoreCollision(collider, otherCollider);
						}
					}
				}
			}
			foreach (Collider collider2 in part.item.colliderGroups.First<ColliderGroup>().colliders)
			{
				foreach (Collider otherCollider2 in this.item.GetComponentsInChildren<Collider>())
				{
					Physics.IgnoreCollision(collider2, otherCollider2);
				}
			}
		}

		// Token: 0x060000BF RID: 191 RVA: 0x000061C8 File Offset: 0x000043C8
		public void PostInit()
		{
			this.isReady = true;
			foreach (BladePart part in this.parts)
			{
				this.IgnoreOtherBladeParts(part);
			}
			foreach (RagdollHand handler in this.item.handlers)
			{
				this.IgnoreCollider(handler, true, 0f);
			}
			this.locking = true;
			this.ReformParts();
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00006288 File Offset: 0x00004488
		public void IgnoreRagdoll(Ragdoll ragdoll, bool ignore)
		{
			foreach (BladePart part in this.parts)
			{
				part.IgnoreRagdoll(ragdoll, ignore);
			}
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x000062E4 File Offset: 0x000044E4
		public void IgnoreCollider(RagdollHand hand, bool ignore, float delay = 0f)
		{
			foreach (BladePart part in this.parts)
			{
				part.IgnoreHand(hand, ignore, delay);
			}
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00006340 File Offset: 0x00004540
		public void ChangeMode<T>() where T : BladeMode, new()
		{
			bool flag = this.mode is T;
			if (!flag)
			{
				BladeMode bladeMode = this.mode;
				if (bladeMode != null)
				{
					bladeMode.Exit();
				}
				foreach (RagdollHand hand in this.item.handlers)
				{
					this.IgnoreCollider(hand, true, 0f);
				}
				this.mode = new T();
				this.mode.Enter(this);
			}
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x000063E8 File Offset: 0x000045E8
		public void ChangeMode(BladeMode newMode)
		{
			BladeMode newModeInstance = newMode.Clone();
			bool flag = this.mode.GetType() == newModeInstance.GetType();
			if (!flag)
			{
				BladeMode bladeMode = this.mode;
				if (bladeMode != null)
				{
					bladeMode.Exit();
				}
				foreach (RagdollHand hand in this.item.handlers)
				{
					this.IgnoreCollider(hand, true, 0f);
				}
				this.mode = newModeInstance;
				this.mode.Enter(this);
			}
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00006494 File Offset: 0x00004694
		public void CheckForMissingParts()
		{
			List<BladePart> newParts = new List<BladePart>();
			foreach (BladePart part in this.parts)
			{
				bool flag = ((part != null) ? part.gameObject.transform : null) == null;
				if (!flag)
				{
					newParts.Add(part);
				}
			}
			this.parts = newParts;
			int i = 0;
			while (i < 15)
			{
				bool flag2 = this.parts[i].index == i + 1;
				if (flag2)
				{
					BladePart part2 = this.parts[i];
					bool flag3 = !part2.item.isCulled;
					if (!flag3)
					{
						part2.item.Despawn();
						bool flag4 = this.parts.Contains(part2);
						if (flag4)
						{
							this.parts.Remove(part2);
						}
						this.SpawnPart(i + 1);
						this.isReady = false;
					}
				}
				else
				{
					bool flag5 = !this.isSpawning[i + 1];
					if (flag5)
					{
						this.SpawnPart(i + 1);
						this.isReady = false;
					}
				}
				IL_126:
				i++;
				continue;
				goto IL_126;
			}
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x000065EC File Offset: 0x000047EC
		public void Update()
		{
			bool flag = this.item == null;
			if (flag)
			{
				this.Init();
				bool flag2 = this.item == null;
				if (flag2)
				{
					return;
				}
			}
			bool flag3 = !this.isReady && this.parts.Count == 15;
			if (flag3)
			{
				this.PostInit();
			}
			else
			{
				bool flag4 = !this.isReady;
				if (flag4)
				{
					return;
				}
			}
			this.CheckForMissingParts();
			bool flag5 = this.item.holder == null;
			if (flag5)
			{
				this.HideAllAnnotations();
			}
			bool changedModes = false;
			foreach (BladeMode mode in this.modes)
			{
				bool flag6 = mode.Test(this);
				if (flag6)
				{
					this.ChangeMode(mode);
					changedModes = true;
				}
			}
			bool flag7 = !changedModes;
			if (flag7)
			{
				bool flag8 = this.item.handlers.Any((RagdollHand handler) => handler.playerHand.controlHand.alternateUsePressed);
				if (flag8)
				{
					bool flag9 = !this.buttonWasPressed;
					if (flag9)
					{
						this.buttonWasPressed = true;
						this.lastButtonPress = Time.time;
						this.orgAxisLength = this.item.mainHandleLeft.axisLength;
						this.item.mainHandleLeft.axisLength = 0f;
						foreach (RagdollHand handler2 in this.item.handlers)
						{
							this.item.mainHandleLeft.SetSliding(handler2, false);
						}
					}
					this.locking = true;
					bool flag10 = this.item.handlers.Any((RagdollHand handler) => handler.playerHand.controlHand.usePressed);
					if (flag10)
					{
						RagdollHand ragdollHand;
						if (this.item.handlers.Count((RagdollHand hand) => hand.playerHand.controlHand.usePressed) <= 1)
						{
							ragdollHand = this.item.handlers.FirstOrDefault((RagdollHand hand) => hand.playerHand.controlHand.usePressed);
						}
						else
						{
							ragdollHand = this.item.mainHandler;
						}
						this.buttonHand = ragdollHand;
						this.ChangeMode<ShieldMode>();
					}
					else
					{
						this.ChangeMode<ExpandedMode>();
					}
				}
				else
				{
					this.ChangeMode<SwordMode>();
					bool flag11 = this.buttonWasPressed;
					if (flag11)
					{
						bool flag12 = Time.time - this.lastButtonPress < 0.3f;
						if (flag12)
						{
							this.locking = !this.wasLocking;
							bool flag13 = this.locking;
							if (flag13)
							{
								this.ReformParts();
							}
							else
							{
								this.DetachParts(true);
							}
							this.wasLocking = this.locking;
						}
						else
						{
							this.wasLocking = true;
						}
						this.item.mainHandleLeft.axisLength = this.orgAxisLength;
					}
					this.buttonWasPressed = false;
				}
			}
			BladeMode bladeMode = this.mode;
			if (bladeMode != null)
			{
				bladeMode.Update();
			}
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00006954 File Offset: 0x00004B54
		public void BladeHaptic(float velocity)
		{
			bool flag = this.mode is CannonMode;
			if (!flag)
			{
				bool flag2 = Time.time - this.lastBladeHaptic > 0.01f;
				if (flag2)
				{
					this.lastBladeHaptic = Time.time;
					this.item.handlers.ForEach(delegate(RagdollHand handler)
					{
						handler.HapticTick(Mathf.InverseLerp(0f, 10f, velocity) * 0.5f, 10f);
					});
				}
			}
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x000069C4 File Offset: 0x00004BC4
		public void DetachParts(bool shouldThrow = true)
		{
			foreach (BladePart part in this.parts)
			{
				part.Detach(shouldThrow);
			}
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00006A20 File Offset: 0x00004C20
		public void ReformParts()
		{
			bool flag = !this.isReady;
			if (!flag)
			{
				this.locking = true;
				foreach (BladePart part in this.parts)
				{
					BladeMode bladeMode = this.mode;
					bool flag2 = bladeMode == null || bladeMode.ShouldReform(part);
					if (flag2)
					{
						part.Reform();
					}
				}
			}
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00006AA8 File Offset: 0x00004CA8
		public bool ShouldHide(BladePart part)
		{
			BladeMode bladeMode = this.mode;
			return bladeMode == null || bladeMode.ShouldHideWhenHolstered(part);
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00006ABD File Offset: 0x00004CBD
		public bool ShouldReform(BladePart part)
		{
			bool flag;
			if (this.locking)
			{
				BladeMode bladeMode = this.mode;
				flag = bladeMode == null || bladeMode.ShouldReform(part);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00006ADD File Offset: 0x00004CDD
		public bool ShouldPartLock(BladePart part)
		{
			bool flag;
			if (this.locking)
			{
				BladeMode bladeMode = this.mode;
				flag = bladeMode == null || bladeMode.ShouldLock(part);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00006AFD File Offset: 0x00004CFD
		public void ModifyJoint(BladePart part)
		{
			BladeMode bladeMode = this.mode;
			if (bladeMode != null)
			{
				bladeMode.JointModifier(part.joint, part);
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00006B18 File Offset: 0x00004D18
		public BladePart GetPart(int index)
		{
			return this.parts.FirstOrDefault((BladePart part) => part.item.itemId == string.Format("ShatterbladePart{0}", index));
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00006B4C File Offset: 0x00004D4C
		public Rigidbody GetRB(int index)
		{
			return this.jointRBs.FirstOrDefault((Rigidbody rb) => rb.name == string.Format("Blade_{0}", index));
		}

		// Token: 0x04000021 RID: 33
		public ShatterbladeModule module;

		// Token: 0x04000022 RID: 34
		private const float BUTTON_TAP_THRESHOLD = 0.3f;

		// Token: 0x04000023 RID: 35
		public bool isTutorialBlade;

		// Token: 0x04000024 RID: 36
		public bool locking;

		// Token: 0x04000025 RID: 37
		public bool wasLocking;

		// Token: 0x04000026 RID: 38
		public List<BladePart> parts = new List<BladePart>();

		// Token: 0x04000027 RID: 39
		private bool buttonWasPressed;

		// Token: 0x04000028 RID: 40
		private bool isReady;

		// Token: 0x04000029 RID: 41
		public List<Rigidbody> jointRBs = new List<Rigidbody>();

		// Token: 0x0400002A RID: 42
		private float lastButtonPress;

		// Token: 0x0400002B RID: 43
		public Item item;

		// Token: 0x0400002C RID: 44
		private float lastBladeHaptic;

		// Token: 0x0400002D RID: 45
		public BladeMode mode;

		// Token: 0x0400002E RID: 46
		public Animator animator;

		// Token: 0x0400002F RID: 47
		public RagdollHand buttonHand;

		// Token: 0x04000030 RID: 48
		public Dictionary<Rigidbody, BladePart> rbMap = new Dictionary<Rigidbody, BladePart>();

		// Token: 0x04000031 RID: 49
		public Dictionary<BladePart, Rigidbody> partMap = new Dictionary<BladePart, Rigidbody>();

		// Token: 0x04000032 RID: 50
		public bool shouldLock = true;

		// Token: 0x04000033 RID: 51
		public Dictionary<int, bool> isSpawning = new Dictionary<int, bool>();

		// Token: 0x04000034 RID: 52
		public bool isDespawned;

		// Token: 0x04000035 RID: 53
		public Annotation handleAnnotationA;

		// Token: 0x04000036 RID: 54
		public Annotation handleAnnotationB;

		// Token: 0x04000037 RID: 55
		public Annotation otherHandAnnotation;

		// Token: 0x04000038 RID: 56
		public Annotation gunShardAnnotation;

		// Token: 0x04000039 RID: 57
		public Annotation sawShardAnnotation;

		// Token: 0x0400003A RID: 58
		public Annotation imbueHandleAnnotation;

		// Token: 0x0400003B RID: 59
		private float orgAxisLength;

		// Token: 0x0400003C RID: 60
		public List<BladeMode> modes;
	}
}
