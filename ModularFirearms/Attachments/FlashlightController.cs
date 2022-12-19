using System;
using ModularFirearms.Shared;
using ThunderRoad;
using UnityEngine;

namespace ModularFirearms.Attachments
{
	// Token: 0x02000021 RID: 33
	public class FlashlightController : MonoBehaviour
	{
		// Token: 0x060000EC RID: 236 RVA: 0x000091CC File Offset: 0x000073CC
		protected void Awake()
		{
			this.item = base.GetComponent<Item>();
			this.module = this.item.data.GetModule<AttachmentModule>();
			this.item.OnHeldActionEvent += new Item.HeldActionDelegate(this.OnHeldAction);
			if (this.module.flashlightRef != null)
			{
				this.attachedLight = this.item.GetCustomReference(this.module.flashlightRef, true).GetComponent<Light>();
				if (!string.IsNullOrEmpty(this.module.flashlightMeshRef))
				{
					this.flashlightMaterial = this.item.GetCustomReference(this.module.flashlightMeshRef, true).GetComponent<MeshRenderer>().material;
					this.flashlightEmissionColor = this.flashlightMaterial.GetColor("_EmissionColor");
					if (!this.attachedLight.enabled)
					{
						this.flashlightMaterial.SetColor("_EmissionColor", Color.black);
					}
				}
			}
			if (!string.IsNullOrEmpty(this.module.flashlightActivationSoundRef))
			{
				this.activationSound = this.item.GetCustomReference(this.module.flashlightActivationSoundRef, true).GetComponent<AudioSource>();
			}
			if (this.module.flashlightHandleRef != null)
			{
				this.attachmentHandle = this.item.GetCustomReference(this.module.flashlightHandleRef, true).GetComponent<Handle>();
			}
			this.lightCullingMask = 1048576;
			this.lightCullingMask = ~this.lightCullingMask;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00009330 File Offset: 0x00007530
		protected void Start()
		{
			if (this.attachedLight != null)
			{
				this.attachedLight.cullingMask = this.lightCullingMask;
			}
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00009351 File Offset: 0x00007551
		protected void StartLongPress()
		{
			this.checkForLongPress = true;
			this.lastSpellMenuPress = Time.time;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00009365 File Offset: 0x00007565
		public void CancelLongPress()
		{
			this.checkForLongPress = false;
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00009370 File Offset: 0x00007570
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
							this.ToggleLight();
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
						this.ToggleLight();
					}
				}
			}
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x000093D9 File Offset: 0x000075D9
		public void OnHeldAction(RagdollHand interactor, Handle handle, Interactable.Action action)
		{
			if (handle.Equals(this.attachmentHandle))
			{
				if (action == 2)
				{
					this.spellMenuPressed = true;
					this.StartLongPress();
				}
				if (action == 3)
				{
					this.spellMenuPressed = false;
				}
			}
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00009408 File Offset: 0x00007608
		private void ToggleLight()
		{
			if (this.activationSound != null)
			{
				this.activationSound.Play();
			}
			if (this.attachedLight != null)
			{
				this.attachedLight.enabled = !this.attachedLight.enabled;
				if (this.flashlightMaterial != null)
				{
					if (this.attachedLight.enabled)
					{
						this.flashlightMaterial.SetColor("_EmissionColor", this.flashlightEmissionColor);
						return;
					}
					this.flashlightMaterial.SetColor("_EmissionColor", Color.black);
				}
			}
		}

		// Token: 0x040001C8 RID: 456
		protected Item item;

		// Token: 0x040001C9 RID: 457
		protected AttachmentModule module;

		// Token: 0x040001CA RID: 458
		private Light attachedLight;

		// Token: 0x040001CB RID: 459
		private Material flashlightMaterial;

		// Token: 0x040001CC RID: 460
		private Color flashlightEmissionColor;

		// Token: 0x040001CD RID: 461
		private AudioSource activationSound;

		// Token: 0x040001CE RID: 462
		private Handle attachmentHandle;

		// Token: 0x040001CF RID: 463
		private int lightCullingMask;

		// Token: 0x040001D0 RID: 464
		public float lastSpellMenuPress;

		// Token: 0x040001D1 RID: 465
		public bool isLongPress;

		// Token: 0x040001D2 RID: 466
		public bool checkForLongPress;

		// Token: 0x040001D3 RID: 467
		public bool spellMenuPressed;
	}
}
