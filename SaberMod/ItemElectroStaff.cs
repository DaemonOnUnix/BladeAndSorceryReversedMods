using System;
using System.Collections;
using ThunderRoad;
using UnityEngine;

namespace SaberMod
{
	// Token: 0x02000005 RID: 5
	public class ItemElectroStaff : MonoBehaviour
	{
		// Token: 0x0600001C RID: 28 RVA: 0x000022E8 File Offset: 0x000004E8
		public void Awake()
		{
			this.item = base.GetComponent<Item>();
			this.item.OnGrabEvent += new Item.GrabDelegate(this.OnGrabEvent);
			this.item.OnUngrabEvent += new Item.ReleaseDelegate(this.OnUngrabEvent);
			this.item.OnTelekinesisGrabEvent += new Item.TelekinesisDelegate(this.OnTelekinesisGrabEvent);
			this.item.OnTelekinesisReleaseEvent += new Item.TelekinesisDelegate(this.OnTelekinesisReleaseEvent);
			this.item.OnSnapEvent += new Item.HolderDelegate(this.OnSnapEvent);
			this.item.OnHeldActionEvent += new Item.HeldActionDelegate(this.OnHeldActionEvent);
			this.isStaffOn = false;
			this.isRecalling = false;
			this.StaffDropped = false;
			this.StaffCycling = false;
			this.idleVol = 0.8f;
			this.ignitionOnVol = 1f;
			this.ignitionOffVol = 1f;
			this.recallAllowed = Configuration.RecallAllowed;
			this.recallStrength = Configuration.RecallStrength;
			this.recallTurnStaffOff = Configuration.RecallTurnSaberOff;
			this.recallMaxDistance = Configuration.RecallMaxDistance;
			this.ignitionSpeed = Configuration.IgnitionSpeed;
			this.ignitionDelay = Configuration.IgnitionDelay;
			this.whoosh = this.item.transform.Find("Whoosh").gameObject;
			this.ElectricityEmmiter = this.item.transform.Find("Electricity").gameObject;
			this.ElectricityEmmiter.SetActive(false);
			this.Blades = this.item.transform.Find("Blades");
			this.tip1 = this.Blades.Find("electric1").gameObject;
			this.tip2 = this.Blades.Find("electric2").gameObject;
			this.tip3 = this.Blades.Find("mesh1 (2)").gameObject;
			this.tip4 = this.Blades.Find("mesh1 (3)").gameObject;
			this.tip1.SetActive(false);
			this.tip2.SetActive(false);
			this.tip3.SetActive(true);
			this.tip4.SetActive(true);
			this.ignitionOnSound = this.item.GetCustomReference("ignitionOnSound", true).GetComponent<AudioSource>();
			this.ignitionOffSound = this.item.GetCustomReference("ignitionOffSound", true).GetComponent<AudioSource>();
			this.idleSound = this.item.GetCustomReference("idleSound", true).GetComponent<AudioSource>();
			this.ignitionOnSound.volume = this.ignitionOnVol;
			this.ignitionOffSound.volume = this.ignitionOffVol;
			this.idleSound.volume = this.idleVol;
			this.whoosh.GetComponent<WhooshPoint>().minVelocity = 9999f;
			this.whoosh.GetComponent<WhooshPoint>().maxVelocity = 9999f;
			this.colorCycling = false;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000025C8 File Offset: 0x000007C8
		private void OnHeldActionEvent(RagdollHand ragdollHand, Handle handle, Interactable.Action action)
		{
			if (action == 2)
			{
				if ((ragdollHand == Player.currentCreature.handLeft && this.item.IsHanded(PlayerControl.handLeft.side) && !PlayerControl.GetHand(PlayerControl.handLeft.side).castPressed) || (ragdollHand == Player.currentCreature.handRight && this.item.IsHanded(PlayerControl.handRight.side) && !PlayerControl.GetHand(PlayerControl.handRight.side).castPressed))
				{
					if (!this.isStaffOn)
					{
						if (!this.StaffCycling)
						{
							this.co = base.StartCoroutine(this.ToggleStaff("on", "button"));
							return;
						}
					}
					else if (!this.StaffCycling)
					{
						this.co = base.StartCoroutine(this.ToggleStaff("off", "button"));
						return;
					}
				}
				else if ((this.isStaffOn && ragdollHand == Player.currentCreature.handLeft && this.item.IsHanded(PlayerControl.handLeft.side) && PlayerControl.GetHand(PlayerControl.handLeft.side).castPressed) || (ragdollHand == Player.currentCreature.handRight && this.item.IsHanded(PlayerControl.handRight.side) && PlayerControl.GetHand(PlayerControl.handRight.side).castPressed))
				{
					bool flag = this.colorCycling;
				}
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002744 File Offset: 0x00000944
		protected void OnGrabEvent(Handle handle, RagdollHand hand)
		{
			if (hand.playerHand == Player.local.handLeft || hand.playerHand == Player.local.handRight)
			{
				if (this.StaffDropped && this.StaffCycling)
				{
					this.StaffDropped = false;
					base.StopCoroutine(this.co);
					this.StaffCycling = false;
					return;
				}
			}
			else if ((hand.playerHand != Player.local.handLeft || hand.playerHand != Player.local.handRight) && !this.StaffCycling)
			{
				this.co = base.StartCoroutine(this.ToggleStaff("on", "grab"));
			}
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000027FC File Offset: 0x000009FC
		protected void OnUngrabEvent(Handle handle, RagdollHand hand, bool throwing)
		{
			if (this.isStaffOn && !this.item.holder && (hand.playerHand == Player.local.handLeft || hand.playerHand == Player.local.handRight))
			{
				if (this.StaffCycling)
				{
					base.StopCoroutine(this.co);
					this.StaffCycling = false;
				}
				this.co = base.StartCoroutine(this.ToggleStaff("off", "drop"));
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002888 File Offset: 0x00000A88
		protected void OnTelekinesisGrabEvent(Handle handle, SpellTelekinesis teleGrabber)
		{
			if (this.StaffDropped && this.StaffCycling)
			{
				this.StaffDropped = false;
				base.StopCoroutine(this.co);
				this.StaffCycling = false;
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000028B4 File Offset: 0x00000AB4
		protected void OnTelekinesisReleaseEvent(Handle handle, SpellTelekinesis teleGrabber)
		{
			if (this.StaffCycling)
			{
				base.StopCoroutine(this.co);
				this.StaffCycling = false;
			}
			if (this.isStaffOn)
			{
				this.co = base.StartCoroutine(this.ToggleStaff("off", "drop"));
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002900 File Offset: 0x00000B00
		protected void OnSnapEvent(Holder holder)
		{
			if (this.isStaffOn)
			{
				if (this.StaffCycling)
				{
					base.StopCoroutine(this.co);
					this.StaffCycling = false;
				}
				this.co = base.StartCoroutine(this.ToggleStaff("off", "holster"));
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x0000294C File Offset: 0x00000B4C
		public void Update()
		{
			if (this.item.mainHandler == null && !this.isStaffOn && this.item.isTelekinesisGrabbed && ((PlayerControl.GetHand(PlayerControl.handLeft.side).alternateUsePressed && PlayerControl.GetHand(PlayerControl.handLeft.side).gripPressed) || (PlayerControl.GetHand(PlayerControl.handRight.side).alternateUsePressed && PlayerControl.GetHand(PlayerControl.handRight.side).gripPressed)) && !this.StaffCycling)
			{
				this.co = base.StartCoroutine(this.ToggleStaff("on", "tele"));
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002A04 File Offset: 0x00000C04
		public void FixedUpdate()
		{
			if (this.recallAllowed && this.item.lastHandler != null && !this.item.IsHanded(null) && !this.item.isTelekinesisGrabbed)
			{
				if (this.item.lastHandler.creature == Player.currentCreature && ((PlayerControl.GetHand(PlayerControl.handRight.side).gripPressed && !Player.currentCreature.handRight.grabbedHandle) || (PlayerControl.GetHand(PlayerControl.handLeft.side).gripPressed && !Player.currentCreature.handLeft.grabbedHandle)))
				{
					Handle mainHandleLeft = this.item.mainHandleLeft;
					Transform transform = Player.currentCreature.handLeft.transform;
					Transform transform2 = Player.currentCreature.handRight.transform;
					Rigidbody component = this.item.GetComponent<Rigidbody>();
					if (PlayerControl.GetHand(PlayerControl.handLeft.side).gripPressed)
					{
						float num = Vector3.Distance(this.item.transform.position, transform.position);
						if (num > this.recallMaxDistance && !this.isRecalling)
						{
							this.isRecalling = true;
							return;
						}
						if (num >= 0.3f && this.isRecalling)
						{
							if (this.isStaffOn && this.recallTurnStaffOff)
							{
								if (this.StaffCycling)
								{
									base.StopCoroutine(this.co);
									this.StaffCycling = false;
								}
								this.co = base.StartCoroutine(this.ToggleStaff("off", "recall"));
							}
							this.isRecalling = true;
							component.velocity = (transform.position - this.item.transform.position).normalized * this.recallStrength;
							return;
						}
						if (num < 0.3f && this.isRecalling && !Player.currentCreature.handLeft.grabbedHandle)
						{
							Player.currentCreature.handLeft.Grab(mainHandleLeft);
							this.isRecalling = false;
							return;
						}
					}
					else if (PlayerControl.GetHand(PlayerControl.handRight.side).gripPressed)
					{
						float num2 = Vector3.Distance(this.item.transform.position, transform2.position);
						if (num2 > this.recallMaxDistance && !this.isRecalling)
						{
							this.isRecalling = true;
							return;
						}
						if (num2 >= 0.3f && this.isRecalling)
						{
							if (this.isStaffOn && this.recallTurnStaffOff)
							{
								if (this.StaffCycling)
								{
									base.StopCoroutine(this.co);
									this.StaffCycling = false;
								}
								this.co = base.StartCoroutine(this.ToggleStaff("off", "recall"));
							}
							this.isRecalling = true;
							component.velocity = (transform2.position - this.item.transform.position).normalized * this.recallStrength;
							return;
						}
						if (num2 < 0.3f && this.isRecalling && !Player.currentCreature.handRight.grabbedHandle)
						{
							Player.currentCreature.handRight.Grab(mainHandleLeft);
							this.isRecalling = false;
							return;
						}
					}
				}
				else if (this.item.lastHandler.creature == Player.currentCreature && this.isRecalling)
				{
					this.isRecalling = false;
				}
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002D90 File Offset: 0x00000F90
		private IEnumerator SwapColor(GameObject blade, int counter)
		{
			this.colorCycling = true;
			string text = counter.ToString();
			if (!(text == "1"))
			{
				if (!(text == "2"))
				{
					if (!(text == "3"))
					{
						if (!(text == "4"))
						{
							if (!(text == "5"))
							{
								if (text == "6")
								{
									Color color;
									color..ctor(255f, 255f, 255f, 130f);
									Color color2;
									color2..ctor(191f, 191f, 191f);
									blade.GetComponent<Renderer>().material.SetColor("_Color", color);
									blade.GetComponent<Renderer>().material.SetColor("_EmissionColor", color2 * 0.03f);
									this.clickCounter = 0;
								}
							}
							else
							{
								Color color3;
								color3..ctor(255f, 0f, 0f, 130f);
								Color color4;
								color4..ctor(191f, 0f, 0f);
								blade.GetComponent<Renderer>().material.SetColor("_Color", color3);
								blade.GetComponent<Renderer>().material.SetColor("_EmissionColor", color4 * 0.05f);
							}
						}
						else
						{
							Color color5;
							color5..ctor(152f, 0f, 255f, 130f);
							Color color6;
							color6..ctor(60f, 0f, 191f);
							blade.GetComponent<Renderer>().material.SetColor("_Color", color5);
							blade.GetComponent<Renderer>().material.SetColor("_EmissionColor", color6 * 0.05f);
						}
					}
					else
					{
						Color color7;
						color7..ctor(255f, 229f, 0f, 130f);
						Color color8;
						color8..ctor(191f, 173f, 0f);
						blade.GetComponent<Renderer>().material.SetColor("_Color", color7);
						blade.GetComponent<Renderer>().material.SetColor("_EmissionColor", color8 * 0.03f);
					}
				}
				else
				{
					Color color9;
					color9..ctor(0f, 255f, 30f, 130f);
					Color color10;
					color10..ctor(0f, 191f, 0f);
					blade.GetComponent<Renderer>().material.SetColor("_Color", color9);
					blade.GetComponent<Renderer>().material.SetColor("_EmissionColor", color10 * 0.03f);
				}
			}
			else
			{
				Color color11;
				color11..ctor(0f, 40f, 255f, 130f);
				Color color12;
				color12..ctor(0f, 0f, 191f);
				blade.GetComponent<Renderer>().material.SetColor("_Color", color11);
				blade.GetComponent<Renderer>().material.SetColor("_EmissionColor", color12 * 0.05f);
			}
			yield return new WaitForSeconds(0.25f);
			this.colorCycling = false;
			yield break;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002DAD File Offset: 0x00000FAD
		private IEnumerator ToggleStaff(string state, string reason)
		{
			this.StaffCycling = true;
			if (state == "on")
			{
				this.isStaffOn = true;
				this.ElectricityEmmiter.SetActive(true);
				this.tip3.SetActive(false);
				this.tip4.SetActive(false);
				this.tip1.SetActive(true);
				this.tip2.SetActive(true);
				this.whoosh.GetComponent<WhooshPoint>().minVelocity = 2f;
				this.whoosh.GetComponent<WhooshPoint>().maxVelocity = 14f;
				this.ignitionOnSound.Play();
				this.idleSound.Play();
				yield return new WaitForSeconds(this.ignitionDelay);
				this.StaffCycling = false;
				this.StaffDropped = false;
			}
			else
			{
				if (reason == "drop")
				{
					this.StaffDropped = true;
					yield return new WaitForSeconds(2f);
				}
				this.isStaffOn = false;
				this.idleSound.Stop();
				this.ignitionOffSound.Play();
				this.whoosh.GetComponent<WhooshPoint>().minVelocity = 9999f;
				this.whoosh.GetComponent<WhooshPoint>().maxVelocity = 9999f;
				this.ElectricityEmmiter.SetActive(false);
				this.tip1.SetActive(false);
				this.tip2.SetActive(false);
				this.tip3.SetActive(true);
				this.tip4.SetActive(true);
				yield return new WaitForSeconds(this.ignitionDelay);
				this.StaffCycling = false;
				this.StaffDropped = false;
			}
			yield break;
		}

		// Token: 0x04000015 RID: 21
		private Item item;

		// Token: 0x04000016 RID: 22
		private AudioSource ignitionOnSound;

		// Token: 0x04000017 RID: 23
		private AudioSource ignitionOffSound;

		// Token: 0x04000018 RID: 24
		private AudioSource idleSound;

		// Token: 0x04000019 RID: 25
		private bool isStaffOn;

		// Token: 0x0400001A RID: 26
		private GameObject ElectricityEmmiter;

		// Token: 0x0400001B RID: 27
		private Transform Blades;

		// Token: 0x0400001C RID: 28
		private GameObject tip1;

		// Token: 0x0400001D RID: 29
		private GameObject tip2;

		// Token: 0x0400001E RID: 30
		private GameObject tip3;

		// Token: 0x0400001F RID: 31
		private GameObject tip4;

		// Token: 0x04000020 RID: 32
		private GameObject whoosh;

		// Token: 0x04000021 RID: 33
		private float ignitionOnVol;

		// Token: 0x04000022 RID: 34
		private float ignitionOffVol;

		// Token: 0x04000023 RID: 35
		private float idleVol;

		// Token: 0x04000024 RID: 36
		private bool StaffCycling;

		// Token: 0x04000025 RID: 37
		private bool StaffDropped;

		// Token: 0x04000026 RID: 38
		private Coroutine co;

		// Token: 0x04000027 RID: 39
		private string bladeOrigColor;

		// Token: 0x04000028 RID: 40
		private int clickCounter;

		// Token: 0x04000029 RID: 41
		private bool colorCycling;

		// Token: 0x0400002A RID: 42
		private bool recallAllowed;

		// Token: 0x0400002B RID: 43
		private bool recallTurnStaffOff;

		// Token: 0x0400002C RID: 44
		private float recallMaxDistance;

		// Token: 0x0400002D RID: 45
		private float recallStrength;

		// Token: 0x0400002E RID: 46
		private bool isRecalling;

		// Token: 0x0400002F RID: 47
		private float ignitionSpeed;

		// Token: 0x04000030 RID: 48
		private float ignitionDelay;
	}
}
