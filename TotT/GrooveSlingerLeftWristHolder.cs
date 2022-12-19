using System;
using ThunderRoad;
using UnityEngine;

namespace TotT
{
	// Token: 0x0200002E RID: 46
	public class GrooveSlingerLeftWristHolder : Holder
	{
		// Token: 0x0600015C RID: 348 RVA: 0x0000A9D4 File Offset: 0x00008BD4
		private void FixedUpdate()
		{
			bool flag = this.rotateAroundPart;
			if (flag)
			{
				float x = Vector3.SignedAngle(this.GetDir(this.part, this.holderDir), this.GetDir(this.rotatePart, this.relativeToDir), this.GetDir(this.rotatePart, this.relativePartAxis));
				base.gameObject.transform.localRotation = Quaternion.Euler(x + 25f, 0f, 0f);
			}
		}

		// Token: 0x0600015D RID: 349 RVA: 0x0000AA51 File Offset: 0x00008C51
		public void GrabOn()
		{
			this.canGrab = true;
		}

		// Token: 0x0600015E RID: 350 RVA: 0x0000AA5B File Offset: 0x00008C5B
		public void GrabOff()
		{
			this.canGrab = false;
		}

		// Token: 0x0600015F RID: 351 RVA: 0x0000AA68 File Offset: 0x00008C68
		private Vector3 GetDir(RagdollPart part, GrooveSlingerLeftWristHolder.dir direction)
		{
			Vector3 vector;
			switch (direction)
			{
			case GrooveSlingerLeftWristHolder.dir.up:
				vector = part.transform.up;
				break;
			case GrooveSlingerLeftWristHolder.dir.down:
				vector = -part.transform.up;
				break;
			case GrooveSlingerLeftWristHolder.dir.left:
				vector = -part.transform.right;
				break;
			case GrooveSlingerLeftWristHolder.dir.right:
				vector = part.transform.right;
				break;
			case GrooveSlingerLeftWristHolder.dir.forward:
				vector = part.transform.forward;
				break;
			case GrooveSlingerLeftWristHolder.dir.backward:
				vector = -part.transform.forward;
				break;
			default:
				vector = part.transform.up;
				break;
			}
			return vector;
		}

		// Token: 0x06000160 RID: 352 RVA: 0x0000AB0C File Offset: 0x00008D0C
		protected override void Awake()
		{
			this.creature = base.gameObject.GetComponentInParent<Creature>();
			this.linkedContainer = this.creature.container;
			this.canGrab = true;
			base.Awake();
		}

		// Token: 0x06000161 RID: 353 RVA: 0x0000AB40 File Offset: 0x00008D40
		public void LoadContent()
		{
			foreach (ContainerData.Content content in this.linkedContainer.contents)
			{
				ContentStateHolder contentState;
				bool flag = content.TryGetState<ContentStateHolder>(ref contentState) && contentState.holderName == base.name && content.itemData != null;
				if (flag)
				{
					this.spawningItem = true;
					content.Spawn(delegate(Item item)
					{
						bool flag2 = item;
						if (flag2)
						{
							this.Snap(item, true, false);
						}
						this.spawningItem = false;
					}, true);
				}
			}
		}

		// Token: 0x06000162 RID: 354 RVA: 0x0000ABE0 File Offset: 0x00008DE0
		public override void Snap(Item item, bool silent = false, bool addLinkedContainerContent = true)
		{
			foreach (Handle h in item.handles)
			{
				h.allowedHandSide = 1;
			}
			this.creature.handLeft.ClearTouch();
			base.Snap(item, silent, true);
		}

		// Token: 0x06000163 RID: 355 RVA: 0x0000AC54 File Offset: 0x00008E54
		public override void UnSnap(Item item, bool silent = false, bool addLinkedContainerContent = true)
		{
			foreach (Handle h in item.handles)
			{
				h.allowedHandSide = 0;
			}
			base.UnSnap(item, silent, true);
		}

		// Token: 0x06000164 RID: 356 RVA: 0x0000ACB8 File Offset: 0x00008EB8
		public override bool TryTouchAction(RagdollHand ragdollHand, Interactable.Action action)
		{
			bool flag = action == 4 && !this.canGrab;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				bool flag3 = action == 1;
				if (flag3)
				{
					bool flag4 = this.items[0] != null;
					if (flag4)
					{
						Item item = this.items[0];
						if (item != null)
						{
							item.gameObject.SendMessage("TriggerAction");
						}
						return true;
					}
				}
				else
				{
					bool flag5 = action == 2 && !this.firstTap;
					if (flag5)
					{
						this.firstTap = true;
						this.Timer = true;
						this.handCheck = ragdollHand;
						return true;
					}
					bool flag6 = action == 2 && this.firstTap && this.Timer;
					if (flag6)
					{
						this.secondTap = true;
						this.tapTimer = 0f;
						return true;
					}
				}
				flag2 = base.TryTouchAction(ragdollHand, action);
			}
			return flag2;
		}

		// Token: 0x06000165 RID: 357 RVA: 0x0000AD9C File Offset: 0x00008F9C
		private Vector3 getHeightScale()
		{
			bool flag = this.creature;
			Vector3 vector;
			if (flag)
			{
				vector = Vector3.one * (this.creature.GetHeight() / this.creature.morphology.height);
			}
			else
			{
				Debug.Log("Can't get height because creature is null");
				vector = Vector3.one;
			}
			return vector;
		}

		// Token: 0x06000166 RID: 358 RVA: 0x0000ADF8 File Offset: 0x00008FF8
		public void Update()
		{
			bool flag = this.Timer && this.items[0] != null;
			if (flag)
			{
				bool flag2 = this.tapTimer >= 0f;
				if (flag2)
				{
					this.tapTimer -= Time.deltaTime;
				}
				else
				{
					this.Timer = false;
					this.tapTimer = GrooveSlingerLeftWristHolder.tapTimerMax;
					bool flag3 = this.firstTap && this.secondTap;
					if (flag3)
					{
						Item item = this.items[0];
						if (item != null)
						{
							item.gameObject.SendMessage("DoubleTapAction");
						}
					}
					else
					{
						bool flag4 = this.firstTap && this.handCheck.playerHand.controlHand.alternateUsePressed;
						if (flag4)
						{
							Item item2 = this.items[0];
							if (item2 != null)
							{
								item2.gameObject.SendMessage("HoldAction");
							}
						}
						else
						{
							bool flag5 = this.firstTap && !this.secondTap;
							if (flag5)
							{
								Item item3 = this.items[0];
								if (item3 != null)
								{
									item3.gameObject.SendMessage("AltUseAction");
								}
							}
						}
					}
					this.firstTap = false;
					this.secondTap = false;
					this.handCheck = null;
				}
			}
		}

		// Token: 0x040000FF RID: 255
		public RagdollPart part;

		// Token: 0x04000100 RID: 256
		private Creature creature;

		// Token: 0x04000101 RID: 257
		private GrooveSlingerLeftWristHolder.dir holderDir = GrooveSlingerLeftWristHolder.dir.backward;

		// Token: 0x04000102 RID: 258
		private GrooveSlingerLeftWristHolder.dir relativeToDir = GrooveSlingerLeftWristHolder.dir.down;

		// Token: 0x04000103 RID: 259
		private GrooveSlingerLeftWristHolder.dir relativePartAxis = GrooveSlingerLeftWristHolder.dir.right;

		// Token: 0x04000104 RID: 260
		private bool rotateAroundPart = true;

		// Token: 0x04000105 RID: 261
		public RagdollPart rotatePart;

		// Token: 0x04000106 RID: 262
		private Vector3 RotationOffset = new Vector3(0f, 0f, 180f);

		// Token: 0x04000107 RID: 263
		private bool firstTap;

		// Token: 0x04000108 RID: 264
		private bool secondTap;

		// Token: 0x04000109 RID: 265
		private bool Timer;

		// Token: 0x0400010A RID: 266
		private bool canGrab;

		// Token: 0x0400010B RID: 267
		private RagdollHand handCheck;

		// Token: 0x0400010C RID: 268
		private static float tapTimerMax = 0.14f;

		// Token: 0x0400010D RID: 269
		private float tapTimer = GrooveSlingerLeftWristHolder.tapTimerMax;

		// Token: 0x02000086 RID: 134
		private enum dir
		{
			// Token: 0x04000282 RID: 642
			up,
			// Token: 0x04000283 RID: 643
			down,
			// Token: 0x04000284 RID: 644
			left,
			// Token: 0x04000285 RID: 645
			right,
			// Token: 0x04000286 RID: 646
			forward,
			// Token: 0x04000287 RID: 647
			backward
		}
	}
}
