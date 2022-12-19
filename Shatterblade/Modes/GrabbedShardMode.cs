using System;
using System.Collections.Generic;
using ExtensionMethods;
using ThunderRoad;
using UnityEngine;

namespace Shatterblade.Modes
{
	// Token: 0x02000014 RID: 20
	public abstract class GrabbedShardMode : BladeMode
	{
		// Token: 0x0600012C RID: 300 RVA: 0x000090B0 File Offset: 0x000072B0
		public override bool Test(Shatterblade sword)
		{
			BladePart part = sword.GetPart(this.TargetPartNum());
			Object @object;
			if (part == null)
			{
				@object = null;
			}
			else
			{
				Item item = part.item;
				@object = ((item != null) ? item.mainHandler : null);
			}
			return @object != null;
		}

		// Token: 0x0600012D RID: 301
		public abstract int TargetPartNum();

		// Token: 0x0600012E RID: 302 RVA: 0x000090DC File Offset: 0x000072DC
		public virtual RagdollHand Hand()
		{
			return this.GetPart().item.mainHandler;
		}

		// Token: 0x0600012F RID: 303 RVA: 0x000090EE File Offset: 0x000072EE
		public BladePart GetPart()
		{
			return this.sword.GetPart(this.TargetPartNum());
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00009101 File Offset: 0x00007301
		public virtual Vector3 Center()
		{
			return this.Hand().transform.position + this.Hand().PointDir() * 0.2f;
		}

		// Token: 0x06000131 RID: 305 RVA: 0x0000912D File Offset: 0x0000732D
		public virtual Vector3 UpDir()
		{
			return this.Hand().ThumbDir();
		}

		// Token: 0x06000132 RID: 306 RVA: 0x0000913A File Offset: 0x0000733A
		public virtual Vector3 ForwardDir()
		{
			return this.Hand().PointDir();
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00009147 File Offset: 0x00007347
		public virtual Vector3 SideDir()
		{
			return this.Hand().PalmDir();
		}

		// Token: 0x06000134 RID: 308
		public abstract Vector3 GetPos(int index, Rigidbody rb, BladePart part);

		// Token: 0x06000135 RID: 309
		public abstract Quaternion GetRot(int index, Rigidbody rb, BladePart part);

		// Token: 0x06000136 RID: 310 RVA: 0x00009154 File Offset: 0x00007354
		public Vector3 GetUseAnnotationPosition()
		{
			return (this.Hand().side == 1) ? new Vector3(1f, -1f, 1.5f) : new Vector3(1f, -1f, -1.5f);
		}

		// Token: 0x06000137 RID: 311 RVA: 0x0000918E File Offset: 0x0000738E
		public Vector3 GetAltUseAnnotationPosition()
		{
			return (this.Hand().side == 1) ? new Vector3(1f, 1f, 1.5f) : new Vector3(1f, 1f, -1.5f);
		}

		// Token: 0x06000138 RID: 312 RVA: 0x000091C8 File Offset: 0x000073C8
		public virtual float Cooldown()
		{
			return 0f;
		}

		// Token: 0x06000139 RID: 313 RVA: 0x000091CF File Offset: 0x000073CF
		public virtual bool GetUseAnnotationShown()
		{
			return false;
		}

		// Token: 0x0600013A RID: 314 RVA: 0x000091D2 File Offset: 0x000073D2
		public virtual bool GetAltUseAnnotationShown()
		{
			return false;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x000091D5 File Offset: 0x000073D5
		public virtual string GetUseAnnotation()
		{
			return "";
		}

		// Token: 0x0600013C RID: 316 RVA: 0x000091DC File Offset: 0x000073DC
		public virtual string GetAltUseAnnotation()
		{
			return "";
		}

		// Token: 0x0600013D RID: 317 RVA: 0x000091E4 File Offset: 0x000073E4
		public override void Enter(Shatterblade sword)
		{
			base.Enter(sword);
			this.jointParts = new List<Rigidbody>();
			sword.shouldLock = false;
			sword.animator.enabled = false;
			sword.jointRBs.ForEach(delegate(Rigidbody rb)
			{
				rb.transform.parent = null;
			});
			for (int i = 1; i < 16; i++)
			{
				bool flag = i != this.TargetPartNum();
				if (flag)
				{
					this.jointParts.Add(sword.GetRB(i));
				}
			}
			foreach (RagdollHand hand in this.GetPart().item.handlers)
			{
				sword.IgnoreCollider(hand, true, 0f);
			}
			sword.ReformParts();
			this.GetPart().Detach(false);
			this.useAnnotation = Annotation.CreateAnnotation(sword, this.GetPart().transform, this.GetPart().transform, this.GetUseAnnotationPosition());
			this.altUseAnnotation = Annotation.CreateAnnotation(sword, this.GetPart().transform, sword.GetPart(this.TargetPartNum()).transform, this.GetAltUseAnnotationPosition());
			sword.HideAllAnnotations();
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00009348 File Offset: 0x00007548
		public override void Exit()
		{
			base.Exit();
			this.useAnnotation.Destroy();
			this.altUseAnnotation.Destroy();
			this.sword.jointRBs.ForEach(delegate(Rigidbody rb)
			{
				rb.transform.parent = this.sword.animator.transform;
			});
			this.sword.animator.enabled = true;
			this.sword.shouldLock = true;
		}

		// Token: 0x0600013F RID: 319 RVA: 0x000093B0 File Offset: 0x000075B0
		public bool IsButtonPressed()
		{
			return this.Hand().playerHand.controlHand.alternateUsePressed;
		}

		// Token: 0x06000140 RID: 320 RVA: 0x000093C7 File Offset: 0x000075C7
		public bool IsTriggerPressed()
		{
			return this.Hand().playerHand.controlHand.usePressed && Time.time - this.lastTriggerReleased > this.Cooldown();
		}

		// Token: 0x06000141 RID: 321 RVA: 0x000093F7 File Offset: 0x000075F7
		public virtual void OnButtonPressed()
		{
			this.lastButtonPress = Time.time;
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00009404 File Offset: 0x00007604
		public virtual void OnButtonHeld()
		{
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00009407 File Offset: 0x00007607
		public virtual void OnButtonNotHeld()
		{
		}

		// Token: 0x06000144 RID: 324 RVA: 0x0000940A File Offset: 0x0000760A
		public virtual void OnButtonReleased()
		{
			this.lastButtonReleased = Time.time;
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00009417 File Offset: 0x00007617
		public virtual void OnTriggerPressed()
		{
			this.lastTriggerPress = Time.time;
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00009424 File Offset: 0x00007624
		public virtual void OnTriggerHeld()
		{
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00009427 File Offset: 0x00007627
		public virtual void OnTriggerNotHeld()
		{
		}

		// Token: 0x06000148 RID: 328 RVA: 0x0000942A File Offset: 0x0000762A
		public virtual void OnTriggerReleased()
		{
			this.lastTriggerReleased = Time.time;
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00009438 File Offset: 0x00007638
		private void CheckInputs()
		{
			bool flag = this.IsTriggerPressed();
			if (flag)
			{
				bool flag2 = !this.wasTriggerPressed;
				if (flag2)
				{
					this.wasTriggerPressed = true;
					this.OnTriggerPressed();
				}
				this.OnTriggerHeld();
			}
			else
			{
				bool flag3 = this.wasTriggerPressed;
				if (flag3)
				{
					this.wasTriggerPressed = false;
					this.OnTriggerReleased();
				}
				this.OnTriggerNotHeld();
			}
			bool flag4 = this.IsButtonPressed();
			if (flag4)
			{
				bool flag5 = !this.wasButtonPressed;
				if (flag5)
				{
					this.wasButtonPressed = true;
					this.OnButtonPressed();
				}
				this.OnButtonHeld();
			}
			else
			{
				bool flag6 = this.wasButtonPressed;
				if (flag6)
				{
					this.wasButtonPressed = false;
					this.OnButtonReleased();
				}
				this.OnButtonNotHeld();
			}
		}

		// Token: 0x0600014A RID: 330 RVA: 0x000094F4 File Offset: 0x000076F4
		public override void Update()
		{
			base.Update();
			this.CheckInputs();
			this.useAnnotation.offset = this.GetUseAnnotationPosition();
			this.altUseAnnotation.offset = this.GetAltUseAnnotationPosition();
			bool useAnnotationShown = this.GetUseAnnotationShown();
			if (useAnnotationShown)
			{
				bool flag = this.lastUseText != this.GetUseAnnotation();
				if (flag)
				{
					this.useAnnotation.SetText(this.GetUseAnnotation(), null);
					this.lastUseText = this.GetUseAnnotation();
				}
			}
			else
			{
				this.useAnnotation.Hide();
				this.lastUseText = "";
			}
			bool altUseAnnotationShown = this.GetAltUseAnnotationShown();
			if (altUseAnnotationShown)
			{
				bool flag2 = this.lastAltUseText != this.GetAltUseAnnotation();
				if (flag2)
				{
					this.altUseAnnotation.SetText(this.GetAltUseAnnotation(), null);
					this.lastAltUseText = this.GetAltUseAnnotation();
				}
			}
			else
			{
				this.altUseAnnotation.Hide();
				this.lastAltUseText = "";
			}
			int i = 1;
			foreach (Rigidbody jointPart in this.jointParts)
			{
				BladePart part = this.sword.rbMap[jointPart];
				jointPart.transform.position = this.GetPos(i, jointPart, part);
				jointPart.transform.rotation = this.GetRot(i, jointPart, part) * Quaternion.Inverse(part.item.GetFlyDirRefLocalRotation());
				i++;
			}
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00009694 File Offset: 0x00007894
		public override bool ShouldReform(BladePart part)
		{
			return part != this.sword.GetPart(this.TargetPartNum());
		}

		// Token: 0x0600014C RID: 332 RVA: 0x000096AD File Offset: 0x000078AD
		public override bool ShouldLock(BladePart part)
		{
			return part != this.sword.GetPart(this.TargetPartNum());
		}

		// Token: 0x0400005F RID: 95
		public List<Rigidbody> jointParts;

		// Token: 0x04000060 RID: 96
		private string lastUseText = "";

		// Token: 0x04000061 RID: 97
		private string lastAltUseText = "";

		// Token: 0x04000062 RID: 98
		private bool wasButtonPressed;

		// Token: 0x04000063 RID: 99
		private bool wasTriggerPressed;

		// Token: 0x04000064 RID: 100
		protected float lastButtonPress;

		// Token: 0x04000065 RID: 101
		protected float lastTriggerPress;

		// Token: 0x04000066 RID: 102
		protected float lastButtonReleased;

		// Token: 0x04000067 RID: 103
		protected float lastTriggerReleased;

		// Token: 0x04000068 RID: 104
		private Annotation useAnnotation;

		// Token: 0x04000069 RID: 105
		private Annotation altUseAnnotation;
	}
}
