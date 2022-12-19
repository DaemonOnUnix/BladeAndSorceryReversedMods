using System;
using System.Collections;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

namespace TotT
{
	// Token: 0x0200002C RID: 44
	public class LevelModuleTotT : LevelModule
	{
		// Token: 0x06000148 RID: 328 RVA: 0x00009F65 File Offset: 0x00008165
		public override IEnumerator OnLoadCoroutine()
		{
			EventManager.onPossess += new EventManager.PossessEvent(this.onPossessEvent);
			yield break;
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00009F74 File Offset: 0x00008174
		private void onPossessEvent(Creature creature, EventTime eventTime)
		{
			bool flag = eventTime == 1;
			if (flag)
			{
				bool flag2 = Player.currentCreature == null;
				if (!flag2)
				{
					Creature playerCreature = Player.currentCreature;
					bool RightWrist_Test = false;
					bool LeftWrist_Test = false;
					foreach (Holder h in Player.currentCreature.holders)
					{
						bool flag3 = h.data.id == "GrooveSlinger.RightWristHolderID";
						if (flag3)
						{
							RightWrist_Test = true;
						}
						bool flag4 = h.data.id == "GrooveSlinger.LeftWristHolderID";
						if (flag4)
						{
							LeftWrist_Test = true;
						}
					}
					bool flag5 = !RightWrist_Test;
					if (flag5)
					{
						GrooveSlingerRightWristHolder RightWristHolder = new GrooveSlingerRightWristHolder();
						Debug.Log("Attempting to add Groove's RightWrist Holder");
						this.createRightWristHolder();
						Player.currentCreature.holders = new List<Holder>(Player.currentCreature.GetComponentsInChildren<Holder>());
					}
					bool flag6 = !LeftWrist_Test;
					if (flag6)
					{
						GrooveSlingerLeftWristHolder LeftWristHolder = new GrooveSlingerLeftWristHolder();
						Debug.Log("Attempting to add Groove's LeftWrist Holder");
						this.createLeftWristHolder();
						Player.currentCreature.holders = new List<Holder>(Player.currentCreature.GetComponentsInChildren<Holder>());
					}
					Debug.Log("Items loaded from container from TotT's Level Module:");
					foreach (ContainerData.Content c in Player.currentCreature.container.contents)
					{
						Debug.Log(c.itemData.displayName ?? "");
					}
					this.RightWristHolder.LoadContent();
					this.LeftWristHolder.LoadContent();
				}
			}
		}

		// Token: 0x0600014A RID: 330 RVA: 0x0000A140 File Offset: 0x00008340
		private void createRightWristHolder()
		{
			RagdollPart part = this.GetRagdollPart("RightForeArm");
			HolderData hData = Catalog.GetData<HolderData>("GrooveSlinger.RightWristHolderID", true);
			bool flag = part == null || hData == null;
			if (!flag)
			{
				GrooveSlingerRightWristHolder slHolder = new GameObject("GrooveSlinger.RightWristHolderID-Holder")
				{
					transform = 
					{
						parent = part.transform,
						localPosition = new Vector3(-0.2059f, 0.0001f, 0f),
						localEulerAngles = new Vector3(0f, 0f, 0f)
					}
				}.AddComponent<GrooveSlingerRightWristHolder>();
				slHolder.part = part;
				this.RightWristHolder = slHolder;
				slHolder.Load(hData);
				slHolder.data.forceAllowTouchOnPlayer = false;
				slHolder.data.disableTouch = false;
				slHolder.allowedHandSide = 2;
				slHolder.rotatePart = this.GetRagdollPart("RightHand");
				slHolder.RefreshChildAndParentHolder();
				Debug.Log("Added Groove's Right Wrist Holder");
			}
		}

		// Token: 0x0600014B RID: 331 RVA: 0x0000A23C File Offset: 0x0000843C
		private void createLeftWristHolder()
		{
			RagdollPart part = this.GetRagdollPart("LeftForeArm");
			HolderData hData = Catalog.GetData<HolderData>("GrooveSlinger.LeftWristHolderID", true);
			bool flag = part == null || hData == null;
			if (!flag)
			{
				GrooveSlingerLeftWristHolder slHolder = new GameObject("GrooveSlinger.LeftWristHolderID-Holder")
				{
					transform = 
					{
						parent = part.transform,
						localPosition = new Vector3(-0.2059f, 0.0001f, 0f),
						localEulerAngles = new Vector3(180f, 0f, 0f)
					}
				}.AddComponent<GrooveSlingerLeftWristHolder>();
				slHolder.part = part;
				this.LeftWristHolder = slHolder;
				slHolder.Load(hData);
				slHolder.data.forceAllowTouchOnPlayer = false;
				slHolder.data.disableTouch = false;
				slHolder.allowedHandSide = 1;
				slHolder.rotatePart = this.GetRagdollPart("LeftHand");
				slHolder.RefreshChildAndParentHolder();
				Debug.Log("Added Groove's Left Wrist Holder");
			}
		}

		// Token: 0x0600014C RID: 332 RVA: 0x0000A338 File Offset: 0x00008538
		public RagdollPart GetRagdollPart(string name)
		{
			bool flag = Player.currentCreature;
			if (flag)
			{
				foreach (RagdollPart part in Player.currentCreature.ragdoll.parts)
				{
					bool flag2 = part.name == name;
					if (flag2)
					{
						return part;
					}
				}
			}
			Debug.Log("TotT Error: couldn't find " + name);
			return null;
		}

		// Token: 0x040000EE RID: 238
		private GrooveSlingerRightWristHolder RightWristHolder;

		// Token: 0x040000EF RID: 239
		private GrooveSlingerLeftWristHolder LeftWristHolder;
	}
}
