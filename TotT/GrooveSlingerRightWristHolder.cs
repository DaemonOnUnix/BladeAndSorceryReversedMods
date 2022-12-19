using System;
using ThunderRoad;
using UnityEngine;

namespace TotT
{
	// Token: 0x0200002D RID: 45
	public class GrooveSlingerRightWristHolder : Holder
	{
		// Token: 0x0600014E RID: 334 RVA: 0x0000A3DC File Offset: 0x000085DC
		private void FixedUpdate()
		{
			bool flag = this.rotateAroundPart;
			if (flag)
			{
				float x = Vector3.SignedAngle(this.GetDir(this.part, this.holderDir), this.GetDir(this.rotatePart, this.relativeToDir), this.GetDir(this.rotatePart, this.relativePartAxis));
				base.gameObject.transform.localRotation = Quaternion.Euler(x, 0f, 0f);
			}
		}

		// Token: 0x0600014F RID: 335 RVA: 0x0000A453 File Offset: 0x00008653
		public void GrabOn()
		{
			this.canGrab = true;
		}

		// Token: 0x06000150 RID: 336 RVA: 0x0000A45D File Offset: 0x0000865D
		public void GrabOff()
		{
			this.canGrab = false;
		}

		// Token: 0x06000151 RID: 337 RVA: 0x0000A468 File Offset: 0x00008668
		private Vector3 GetDir(RagdollPart part, GrooveSlingerRightWristHolder.dir direction)
		{
			Vector3 vector;
			switch (direction)
			{
			case GrooveSlingerRightWristHolder.dir.up:
				vector = part.transform.up;
				break;
			case GrooveSlingerRightWristHolder.dir.down:
				vector = -part.transform.up;
				break;
			case GrooveSlingerRightWristHolder.dir.left:
				vector = -part.transform.right;
				break;
			case GrooveSlingerRightWristHolder.dir.right:
				vector = part.transform.right;
				break;
			case GrooveSlingerRightWristHolder.dir.forward:
				vector = part.transform.forward;
				break;
			case GrooveSlingerRightWristHolder.dir.backward:
				vector = -part.transform.forward;
				break;
			default:
				vector = part.transform.up;
				break;
			}
			return vector;
		}

		// Token: 0x06000152 RID: 338 RVA: 0x0000A50C File Offset: 0x0000870C
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

		// Token: 0x06000153 RID: 339 RVA: 0x0000A5AC File Offset: 0x000087AC
		protected override void Awake()
		{
			this.creature = base.gameObject.GetComponentInParent<Creature>();
			this.linkedContainer = this.creature.container;
			this.canGrab = true;
			base.Awake();
		}

		// Token: 0x06000154 RID: 340 RVA: 0x0000A5E0 File Offset: 0x000087E0
		public override void Snap(Item item, bool silent = false, bool addLinkedContainerContent = true)
		{
			foreach (Handle h in item.handles)
			{
				h.allowedHandSide = 2;
			}
			this.creature.handRight.ClearTouch();
			base.Snap(item, silent, addLinkedContainerContent);
		}

		// Token: 0x06000155 RID: 341 RVA: 0x0000A654 File Offset: 0x00008854
		public override void UnSnap(Item item, bool silent = false, bool removeLinkedContainerContent = true)
		{
			foreach (Handle h in item.handles)
			{
				h.allowedHandSide = 0;
			}
			base.UnSnap(item, silent, removeLinkedContainerContent);
		}

		// Token: 0x06000156 RID: 342 RVA: 0x0000A6B8 File Offset: 0x000088B8
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
					bool flag4 = this.items.Count >= 1;
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

		// Token: 0x06000157 RID: 343 RVA: 0x0000A798 File Offset: 0x00008998
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
					this.tapTimer = GrooveSlingerRightWristHolder.tapTimerMax;
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

		// Token: 0x06000158 RID: 344 RVA: 0x0000A8E8 File Offset: 0x00008AE8
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

		// Token: 0x040000F0 RID: 240
		public RagdollPart part;

		// Token: 0x040000F1 RID: 241
		private Creature creature;

		// Token: 0x040000F2 RID: 242
		private GrooveSlingerRightWristHolder.dir holderDir = GrooveSlingerRightWristHolder.dir.forward;

		// Token: 0x040000F3 RID: 243
		private GrooveSlingerRightWristHolder.dir relativeToDir = GrooveSlingerRightWristHolder.dir.up;

		// Token: 0x040000F4 RID: 244
		private GrooveSlingerRightWristHolder.dir relativePartAxis = GrooveSlingerRightWristHolder.dir.right;

		// Token: 0x040000F5 RID: 245
		private bool rotateAroundPart = true;

		// Token: 0x040000F6 RID: 246
		public RagdollPart rotatePart;

		// Token: 0x040000F7 RID: 247
		private Vector3 RotationOffset = new Vector3(0f, 0f, 180f);

		// Token: 0x040000F8 RID: 248
		private bool firstTap;

		// Token: 0x040000F9 RID: 249
		private bool secondTap;

		// Token: 0x040000FA RID: 250
		private bool Timer;

		// Token: 0x040000FB RID: 251
		private bool canGrab;

		// Token: 0x040000FC RID: 252
		private RagdollHand handCheck;

		// Token: 0x040000FD RID: 253
		private static float tapTimerMax = 0.14f;

		// Token: 0x040000FE RID: 254
		private float tapTimer = GrooveSlingerRightWristHolder.tapTimerMax;

		// Token: 0x02000085 RID: 133
		private enum dir
		{
			// Token: 0x0400027B RID: 635
			up,
			// Token: 0x0400027C RID: 636
			down,
			// Token: 0x0400027D RID: 637
			left,
			// Token: 0x0400027E RID: 638
			right,
			// Token: 0x0400027F RID: 639
			forward,
			// Token: 0x04000280 RID: 640
			backward
		}
	}
}
