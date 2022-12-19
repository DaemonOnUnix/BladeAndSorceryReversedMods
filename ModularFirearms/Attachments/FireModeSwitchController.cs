using System;
using System.Collections.Generic;
using ModularFirearms.Shared;
using ModularFirearms.Weapons;
using ThunderRoad;
using UnityEngine;

namespace ModularFirearms.Attachments
{
	// Token: 0x0200001D RID: 29
	public class FireModeSwitchController : MonoBehaviour
	{
		// Token: 0x060000D5 RID: 213 RVA: 0x00008784 File Offset: 0x00006984
		public void NextFireMode()
		{
			if (this.parentFirearm != null)
			{
				int num = this.switchModes.IndexOf(this.parentFirearm.GetCurrentFireMode());
				num++;
				if (num == -1 || num >= this.switchModes.Count)
				{
					num = 0;
				}
				this.parentFirearm.SetNextFireMode(this.switchModes[num]);
				if (this.activationSound != null)
				{
					this.activationSound.Play();
				}
				try
				{
					if (this.pivotTransform != null)
					{
						this.pivotTransform.position = this.switchPositions[num].position;
						this.pivotTransform.rotation = Quaternion.Euler(this.switchPositions[num].rotation.eulerAngles);
					}
					return;
				}
				catch (Exception ex)
				{
					Debug.LogError(string.Format("[ModularFirearms][Exception] NextFireMode(): {0} \n {1}", ex.Message, ex.StackTrace));
					return;
				}
			}
			Debug.LogError("[ModularFirearms][ERROR] NextFireMode(): no parent firearm was found");
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00008890 File Offset: 0x00006A90
		private void Awake()
		{
			this.item = base.GetComponent<Item>();
			this.module = this.item.data.GetModule<AttachmentModule>();
			this.item.OnHeldActionEvent += new Item.HeldActionDelegate(this.OnHeldAction);
			this.switchPositions = new List<Transform>();
			this.switchModes = new List<FrameworkCore.FireMode>();
			this.parentFirearm = base.GetComponent<BaseFirearmGenerator>();
			if (!string.IsNullOrEmpty(this.module.attachmentHandleRef))
			{
				this.attachmentHandle = this.item.GetCustomReference(this.module.attachmentHandleRef, true).GetComponent<Handle>();
			}
			if (!string.IsNullOrEmpty(this.module.activationSoundRef))
			{
				this.activationSound = this.item.GetCustomReference(this.module.activationSoundRef, true).GetComponent<AudioSource>();
			}
			if (!string.IsNullOrEmpty(this.module.swtichRef))
			{
				this.pivotTransform = this.item.GetCustomReference(this.module.swtichRef, true);
			}
			foreach (string text in this.module.switchPositionRefs)
			{
				Transform customReference = this.item.GetCustomReference(text, true);
				this.switchPositions.Add(customReference);
			}
			foreach (string text2 in this.module.allowedFireModes)
			{
				FrameworkCore.FireMode fireMode = (FrameworkCore.FireMode)Enum.Parse(typeof(FrameworkCore.FireMode), text2);
				this.switchModes.Add(fireMode);
			}
			if (this.pivotTransform != null && this.switchPositions.Count != this.switchModes.Count)
			{
				Debug.LogWarning("WARNING, FireModeSwtich switchPositions and switchModes have different lengths!!!");
			}
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00008A3B File Offset: 0x00006C3B
		protected void StartLongPress()
		{
			this.checkForLongPress = true;
			this.lastSpellMenuPress = Time.time;
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00008A4F File Offset: 0x00006C4F
		public void CancelLongPress()
		{
			this.checkForLongPress = false;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00008A58 File Offset: 0x00006C58
		public void LateUpdate()
		{
			if (this.checkForLongPress)
			{
				if (this.spellMenuPressed)
				{
					if (Time.time - this.lastSpellMenuPress > this.module.longPressTime)
					{
						if (this.module.longPressToActivate)
						{
							this.NextFireMode();
						}
						this.CancelLongPress();
						return;
					}
				}
				else
				{
					this.CancelLongPress();
					if (!this.module.longPressToActivate)
					{
						this.NextFireMode();
					}
				}
			}
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00008AC1 File Offset: 0x00006CC1
		public void OnHeldAction(RagdollHand interactor, Handle handle, Interactable.Action action)
		{
			if (handle.Equals(this.attachmentHandle))
			{
				if (action == 2)
				{
					this.spellMenuPressed = true;
					this.StartLongPress();
					return;
				}
				if (action == 3)
				{
					this.spellMenuPressed = false;
				}
			}
		}

		// Token: 0x040001A7 RID: 423
		protected Item item;

		// Token: 0x040001A8 RID: 424
		protected AttachmentModule module;

		// Token: 0x040001A9 RID: 425
		private BaseFirearmGenerator parentFirearm;

		// Token: 0x040001AA RID: 426
		private Handle attachmentHandle;

		// Token: 0x040001AB RID: 427
		private AudioSource activationSound;

		// Token: 0x040001AC RID: 428
		private Transform pivotTransform;

		// Token: 0x040001AD RID: 429
		private List<Transform> switchPositions;

		// Token: 0x040001AE RID: 430
		private List<FrameworkCore.FireMode> switchModes;

		// Token: 0x040001AF RID: 431
		public float lastSpellMenuPress;

		// Token: 0x040001B0 RID: 432
		public bool isLongPress;

		// Token: 0x040001B1 RID: 433
		public bool checkForLongPress;

		// Token: 0x040001B2 RID: 434
		public bool spellMenuPressed;
	}
}
