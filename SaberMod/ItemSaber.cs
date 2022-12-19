using System;
using System.Collections;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

namespace SaberMod
{
	// Token: 0x02000006 RID: 6
	public class ItemSaber : MonoBehaviour
	{
		// Token: 0x06000028 RID: 40 RVA: 0x00002DD4 File Offset: 0x00000FD4
		public void Awake()
		{
			this.item = base.GetComponent<Item>();
			this.item.OnGrabEvent += new Item.GrabDelegate(this.OnGrabEvent);
			this.item.OnUngrabEvent += new Item.ReleaseDelegate(this.OnUngrabEvent);
			this.item.OnTelekinesisGrabEvent += new Item.TelekinesisDelegate(this.OnTelekinesisGrabEvent);
			this.item.OnTelekinesisReleaseEvent += new Item.TelekinesisDelegate(this.OnTelekinesisReleaseEvent);
			this.item.OnSnapEvent += new Item.HolderDelegate(this.OnSnapEvent);
			this.item.OnHeldActionEvent += new Item.HeldActionDelegate(this.OnHeldActionEvent);
			this.isSaberOn = false;
			this.isRecalling = false;
			this.saberDropped = false;
			this.saberCycling = false;
			this.idleVol = Configuration.HumVolume;
			this.ignitionOnVol = Configuration.IgnitionVolume;
			this.ignitionOffVol = Configuration.IgnitionVolume;
			this.recallAllowed = Configuration.RecallAllowed;
			this.recallStrength = Configuration.RecallStrength;
			this.recallTurnSaberOff = Configuration.RecallTurnSaberOff;
			this.recallMaxDistance = Configuration.RecallMaxDistance;
			this.ignitionSpeed = Configuration.IgnitionSpeed;
			this.ignitionDelay = Configuration.IgnitionDelay;
			this.saberTrailsActive = Configuration.SaberTrailsActive;
			this.saberTrailsSpeedDiff = Configuration.SaberTrailsSpeedDiff;
			this.saberBladesHolder = this.item.transform.Find("SaberBlade");
			this.whoosh = this.item.transform.Find("Whoosh").gameObject;
			this.pierce = this.item.transform.Find("Pierce").gameObject;
			this.pierce2t = this.item.transform.Find("Pierce2");
			if (this.pierce2t != null)
			{
				this.pierce2 = this.pierce2t.gameObject;
			}
			this.item.distantGrabSafeDistance = 3f;
			this.ignitionOnSound = this.item.GetCustomReference("ignitionOnSound", true).GetComponent<AudioSource>();
			this.ignitionOffSound = this.item.GetCustomReference("ignitionOffSound", true).GetComponent<AudioSource>();
			this.idleSound = this.item.GetCustomReference("idleSound", true).GetComponent<AudioSource>();
			this.ignitionOnSound.volume = this.ignitionOnVol;
			this.ignitionOffSound.volume = this.ignitionOffVol;
			this.idleSound.volume = this.idleVol;
			if (this.item.gameObject.name.Contains("Darksaber"))
			{
				this.isDarksaber = true;
				this.darksaberCutout = this.item.transform.Find("darksaber").GetComponent<MeshRenderer>().materials[2];
			}
			for (int i = 0; i < this.saberBladesHolder.childCount; i++)
			{
				GameObject gameObject = this.saberBladesHolder.GetChild(i).gameObject;
				this.saberBlades.Add(gameObject);
				this.bladeOnScales.Add(new Vector3(0f, gameObject.transform.localScale.y, 0f));
				this.bladeOffScales.Add(new Vector3(0f, -gameObject.transform.localScale.y, 0f));
				gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, 0f, gameObject.transform.localScale.z);
				gameObject.GetComponent<Collider>().enabled = false;
				this.bladeOrigColor = gameObject.GetComponent<Renderer>().material.name;
				this.saberGlows.Add(gameObject.transform.Find("Glow").transform.gameObject);
				this.saberTrails.Add(gameObject.transform.Find("Trail").transform.gameObject);
				gameObject.SetActive(false);
			}
			this.whoosh.GetComponent<WhooshPoint>().minVelocity = 9999f;
			this.whoosh.GetComponent<WhooshPoint>().maxVelocity = 9999f;
			if (!this.saberTrailsActive)
			{
				foreach (GameObject gameObject2 in this.saberTrails)
				{
					gameObject2.SetActive(false);
				}
			}
			this.colorCycling = false;
			string text = this.bladeOrigColor.ToString();
			uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
			if (num <= 1154258222U)
			{
				if (num != 136278576U)
				{
					if (num != 608259217U)
					{
						if (num != 1154258222U)
						{
							goto IL_740;
						}
						if (!(text == "WhiteBlade (Instance)"))
						{
							goto IL_740;
						}
						goto IL_6FC;
					}
					else if (!(text == "YellowBlade (Instance)"))
					{
						goto IL_740;
					}
				}
				else
				{
					if (!(text == "RedBlade (Instance)"))
					{
						goto IL_740;
					}
					goto IL_6B8;
				}
			}
			else
			{
				if (num <= 2802604845U)
				{
					if (num != 2031736080U)
					{
						if (num != 2802604845U)
						{
							goto IL_740;
						}
						if (!(text == "BlueBlade (Instance)"))
						{
							goto IL_740;
						}
						this.clickCounter = 1;
						using (List<GameObject>.Enumerator enumerator = this.saberGlows.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								GameObject gameObject3 = enumerator.Current;
								gameObject3.GetComponent<Light>().color = Color.blue;
							}
							goto IL_740;
						}
					}
					else
					{
						if (!(text == "GreenBlade (Instance)"))
						{
							goto IL_740;
						}
						goto IL_5E3;
					}
				}
				else if (num != 3963538275U)
				{
					if (num != 4089824122U)
					{
						goto IL_740;
					}
					if (!(text == "CyanBlade (Instance)"))
					{
						goto IL_740;
					}
				}
				else
				{
					if (!(text == "PurpleBlade (Instance)"))
					{
						goto IL_740;
					}
					goto IL_671;
				}
				this.clickCounter = 2;
				using (List<GameObject>.Enumerator enumerator = this.saberGlows.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						GameObject gameObject4 = enumerator.Current;
						gameObject4.GetComponent<Light>().color = Color.green;
					}
					goto IL_740;
				}
				IL_5E3:
				this.clickCounter = 3;
				using (List<GameObject>.Enumerator enumerator = this.saberGlows.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						GameObject gameObject5 = enumerator.Current;
						gameObject5.GetComponent<Light>().color = Color.green;
					}
					goto IL_740;
				}
			}
			this.clickCounter = 4;
			using (List<GameObject>.Enumerator enumerator = this.saberGlows.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GameObject gameObject6 = enumerator.Current;
					gameObject6.GetComponent<Light>().color = Color.yellow;
				}
				goto IL_740;
			}
			IL_671:
			this.clickCounter = 5;
			using (List<GameObject>.Enumerator enumerator = this.saberGlows.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GameObject gameObject7 = enumerator.Current;
					gameObject7.GetComponent<Light>().color = Color.magenta;
				}
				goto IL_740;
			}
			IL_6B8:
			this.clickCounter = 6;
			using (List<GameObject>.Enumerator enumerator = this.saberGlows.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GameObject gameObject8 = enumerator.Current;
					gameObject8.GetComponent<Light>().color = Color.red;
				}
				goto IL_740;
			}
			IL_6FC:
			this.clickCounter = 0;
			foreach (GameObject gameObject9 in this.saberGlows)
			{
				gameObject9.GetComponent<Light>().color = Color.white;
			}
			IL_740:
			foreach (GameObject gameObject10 in this.saberGlows)
			{
				gameObject10.SetActive(false);
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000035C8 File Offset: 0x000017C8
		private void OnHeldActionEvent(RagdollHand ragdollHand, Handle handle, Interactable.Action action)
		{
			if (action == 2)
			{
				if ((ragdollHand == Player.currentCreature.handLeft && this.item.IsHanded(PlayerControl.handLeft.side) && !PlayerControl.GetHand(PlayerControl.handLeft.side).castPressed) || (ragdollHand == Player.currentCreature.handRight && this.item.IsHanded(PlayerControl.handRight.side) && !PlayerControl.GetHand(PlayerControl.handRight.side).castPressed))
				{
					if (!this.isSaberOn)
					{
						if (!this.saberCycling)
						{
							this.co = base.StartCoroutine(this.ToggleSaber("on", "button"));
							return;
						}
					}
					else if (!this.saberCycling)
					{
						this.co = base.StartCoroutine(this.ToggleSaber("off", "button"));
						return;
					}
				}
				else if (((this.isSaberOn && ragdollHand == Player.currentCreature.handLeft && this.item.IsHanded(PlayerControl.handLeft.side) && PlayerControl.GetHand(PlayerControl.handLeft.side).castPressed) || (ragdollHand == Player.currentCreature.handRight && this.item.IsHanded(PlayerControl.handRight.side) && PlayerControl.GetHand(PlayerControl.handRight.side).castPressed)) && !this.colorCycling)
				{
					this.clickCounter++;
					foreach (GameObject gameObject in this.saberBlades)
					{
						base.StartCoroutine(this.SwapColor(gameObject, this.clickCounter));
					}
				}
			}
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000037A8 File Offset: 0x000019A8
		protected void OnGrabEvent(Handle handle, RagdollHand hand)
		{
			if (hand.playerHand == Player.local.handLeft || hand.playerHand == Player.local.handRight)
			{
				if (this.saberDropped && this.saberCycling)
				{
					this.saberDropped = false;
					base.StopCoroutine(this.co);
					this.saberCycling = false;
					return;
				}
			}
			else if (hand.playerHand != Player.local.handLeft && hand.playerHand != Player.local.handRight && !this.saberCycling)
			{
				this.co = base.StartCoroutine(this.ToggleSaber("on", "grab"));
			}
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00003860 File Offset: 0x00001A60
		protected void OnUngrabEvent(Handle handle, RagdollHand hand, bool throwing)
		{
			if (this.isSaberOn && !this.item.holder && (hand.playerHand == Player.local.handLeft || hand.playerHand == Player.local.handRight))
			{
				if (this.saberCycling)
				{
					base.StopCoroutine(this.co);
					this.saberCycling = false;
				}
				this.co = base.StartCoroutine(this.ToggleSaber("off", "drop"));
			}
			else if (this.isSaberOn && !this.item.holder && hand.playerHand != Player.local.handLeft && hand.playerHand != Player.local.handRight && !this.saberCycling)
			{
				this.co = base.StartCoroutine(this.ToggleSaber("off", "npcdrop"));
			}
			if (this.isSaberOn && !this.item.holder && this.item.isPenetrating && !this.saberCycling)
			{
				this.co = base.StartCoroutine(this.ToggleSaber("off", "drop"));
			}
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000039A5 File Offset: 0x00001BA5
		protected void OnTelekinesisGrabEvent(Handle handle, SpellTelekinesis teleGrabber)
		{
			if (this.saberDropped && this.saberCycling)
			{
				this.saberDropped = false;
				base.StopCoroutine(this.co);
				this.saberCycling = false;
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000039D4 File Offset: 0x00001BD4
		protected void OnTelekinesisReleaseEvent(Handle handle, SpellTelekinesis teleGrabber)
		{
			if (this.saberCycling)
			{
				base.StopCoroutine(this.co);
				this.saberCycling = false;
			}
			if (this.isSaberOn)
			{
				this.co = base.StartCoroutine(this.ToggleSaber("off", "drop"));
			}
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00003A20 File Offset: 0x00001C20
		protected void OnSnapEvent(Holder holder)
		{
			if (this.isSaberOn)
			{
				if (this.saberCycling)
				{
					base.StopCoroutine(this.co);
					this.saberCycling = false;
				}
				this.co = base.StartCoroutine(this.ToggleSaber("off", "holster"));
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00003A6C File Offset: 0x00001C6C
		public void Update()
		{
			if (this.item.mainHandler == null && !this.isSaberOn && this.item.isTelekinesisGrabbed && ((PlayerControl.GetHand(PlayerControl.handLeft.side).alternateUsePressed && PlayerControl.GetHand(PlayerControl.handLeft.side).gripPressed) || (PlayerControl.GetHand(PlayerControl.handRight.side).alternateUsePressed && PlayerControl.GetHand(PlayerControl.handRight.side).gripPressed)) && !this.saberCycling)
			{
				this.co = base.StartCoroutine(this.ToggleSaber("on", "tele"));
			}
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00003B24 File Offset: 0x00001D24
		public void FixedUpdate()
		{
			if (this.saberTrailsActive && this.isSaberOn && this.item.IsHanded(null) && this.item.handlers[0].creature != null)
			{
				float magnitude = this.item.handlers[0].creature.GetComponent<Rigidbody>().velocity.magnitude;
				if (this.item.GetComponent<Rigidbody>().velocity.magnitude - magnitude > this.saberTrailsSpeedDiff)
				{
					using (List<GameObject>.Enumerator enumerator = this.saberTrails.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							GameObject gameObject = enumerator.Current;
							if (!gameObject.activeSelf)
							{
								gameObject.SetActive(true);
							}
						}
						goto IL_112;
					}
				}
				foreach (GameObject gameObject2 in this.saberTrails)
				{
					if (gameObject2.activeSelf)
					{
						gameObject2.SetActive(false);
					}
				}
			}
			IL_112:
			if (this.recallAllowed && this.item.lastHandler != null && !this.item.IsHanded(null) && !this.item.isTelekinesisGrabbed)
			{
				if (this.item.lastHandler.creature == Player.currentCreature && ((PlayerControl.GetHand(PlayerControl.handRight.side).gripPressed && !Player.currentCreature.handRight.grabbedHandle) || (PlayerControl.GetHand(PlayerControl.handLeft.side).gripPressed && !Player.currentCreature.handLeft.grabbedHandle)))
				{
					Handle mainHandleLeft = this.item.mainHandleLeft;
					Transform transform = Player.currentCreature.handLeft.transform;
					Transform transform2 = Player.currentCreature.handRight.transform;
					Rigidbody component = this.item.GetComponent<Rigidbody>();
					if (PlayerControl.GetHand(PlayerControl.handLeft.side).gripPressed && !Player.currentCreature.handLeft.grabbedHandle)
					{
						float num = Vector3.Distance(this.item.transform.position, transform.position);
						if (num > this.recallMaxDistance && !this.isRecalling)
						{
							this.isRecalling = true;
							return;
						}
						if (num >= 0.3f && this.isRecalling)
						{
							if (this.isSaberOn && this.recallTurnSaberOff)
							{
								if (this.saberCycling)
								{
									base.StopCoroutine(this.co);
									this.saberCycling = false;
								}
								this.co = base.StartCoroutine(this.ToggleSaber("off", "recall"));
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
					else if (PlayerControl.GetHand(PlayerControl.handRight.side).gripPressed && !Player.currentCreature.handRight.grabbedHandle)
					{
						float num2 = Vector3.Distance(this.item.transform.position, transform2.position);
						if (num2 > this.recallMaxDistance && !this.isRecalling)
						{
							this.isRecalling = true;
							return;
						}
						if (num2 >= 0.3f && this.isRecalling)
						{
							if (this.isSaberOn && this.recallTurnSaberOff)
							{
								if (this.saberCycling)
								{
									base.StopCoroutine(this.co);
									this.saberCycling = false;
								}
								this.co = base.StartCoroutine(this.ToggleSaber("off", "recall"));
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

		// Token: 0x06000031 RID: 49 RVA: 0x0000401C File Offset: 0x0000221C
		private IEnumerator SwapColor(GameObject blade, int counter)
		{
			this.colorCycling = true;
			float num = 0.5f;
			string text = counter.ToString();
			uint num2 = <PrivateImplementationDetails>.ComputeStringHash(text);
			if (num2 <= 839689206U)
			{
				if (num2 != 806133968U)
				{
					if (num2 != 822911587U)
					{
						if (num2 == 839689206U)
						{
							if (text == "7")
							{
								Color color;
								color..ctor(255f, 255f, 255f, 130f);
								Color color2;
								color2..ctor(191f, 191f, 191f);
								blade.GetComponent<Renderer>().material.SetColor("_Color", color);
								blade.GetComponent<Renderer>().material.SetColor("_EmissionColor", color2 * (num - 0.25f));
								foreach (GameObject gameObject in this.saberGlows)
								{
									gameObject.GetComponent<Light>().color = Color.white;
								}
								if (this.isDarksaber)
								{
									this.darksaberCutout.SetColor("_Color", color);
									this.darksaberCutout.SetColor("_EmissionColor", color2 * num);
								}
								if (this.saberBlades.Count > 1)
								{
									if (this.saberBlades.IndexOf(blade) == 1)
									{
										this.clickCounter = 0;
									}
								}
								else
								{
									this.clickCounter = 0;
								}
							}
						}
					}
					else if (text == "4")
					{
						Color color3;
						color3..ctor(255f, 229f, 0f, 130f);
						Color color4;
						color4..ctor(191f, 173f, 0f);
						blade.GetComponent<Renderer>().material.SetColor("_Color", color3);
						blade.GetComponent<Renderer>().material.SetColor("_EmissionColor", color4 * (num - 0.25f));
						foreach (GameObject gameObject2 in this.saberGlows)
						{
							gameObject2.GetComponent<Light>().color = Color.yellow;
						}
						if (this.isDarksaber)
						{
							this.darksaberCutout.SetColor("_Color", color3);
							this.darksaberCutout.SetColor("_EmissionColor", color4 * num);
						}
					}
				}
				else if (text == "5")
				{
					Color color5;
					color5..ctor(152f, 0f, 255f, 130f);
					Color color6;
					color6..ctor(60f, 0f, 191f);
					blade.GetComponent<Renderer>().material.SetColor("_Color", color5);
					blade.GetComponent<Renderer>().material.SetColor("_EmissionColor", color6 * num);
					foreach (GameObject gameObject3 in this.saberGlows)
					{
						gameObject3.GetComponent<Light>().color = Color.magenta;
					}
					if (this.isDarksaber)
					{
						this.darksaberCutout.SetColor("_Color", color5);
						this.darksaberCutout.SetColor("_EmissionColor", color6 * num);
					}
				}
			}
			else if (num2 <= 873244444U)
			{
				if (num2 != 856466825U)
				{
					if (num2 == 873244444U)
					{
						if (text == "1")
						{
							Color color7;
							color7..ctor(0f, 40f, 255f, 130f);
							Color color8;
							color8..ctor(0f, 0f, 191f);
							blade.GetComponent<Renderer>().material.SetColor("_Color", color7);
							blade.GetComponent<Renderer>().material.SetColor("_EmissionColor", color8 * num);
							foreach (GameObject gameObject4 in this.saberGlows)
							{
								gameObject4.GetComponent<Light>().color = Color.blue;
							}
							if (this.isDarksaber)
							{
								this.darksaberCutout.SetColor("_Color", color7);
								this.darksaberCutout.SetColor("_EmissionColor", color8 * num);
							}
						}
					}
				}
				else if (text == "6")
				{
					Color color9;
					color9..ctor(255f, 0f, 0f, 130f);
					Color color10;
					color10..ctor(191f, 0f, 0f);
					blade.GetComponent<Renderer>().material.SetColor("_Color", color9);
					blade.GetComponent<Renderer>().material.SetColor("_EmissionColor", color10 * num);
					foreach (GameObject gameObject5 in this.saberGlows)
					{
						gameObject5.GetComponent<Light>().color = Color.red;
					}
					if (this.isDarksaber)
					{
						this.darksaberCutout.SetColor("_Color", color9);
						this.darksaberCutout.SetColor("_EmissionColor", color10 * num);
					}
				}
			}
			else if (num2 != 906799682U)
			{
				if (num2 == 923577301U)
				{
					if (text == "2")
					{
						Color color11;
						color11..ctor(0f, 255f, 255f, 130f);
						Color color12;
						color12..ctor(0f, 191f, 191f);
						blade.GetComponent<Renderer>().material.SetColor("_Color", color11);
						blade.GetComponent<Renderer>().material.SetColor("_EmissionColor", color12 * (num - 0.25f));
						foreach (GameObject gameObject6 in this.saberGlows)
						{
							gameObject6.GetComponent<Light>().color = Color.cyan;
						}
						if (this.isDarksaber)
						{
							this.darksaberCutout.SetColor("_Color", color11);
							this.darksaberCutout.SetColor("_EmissionColor", color12 * num);
						}
					}
				}
			}
			else if (text == "3")
			{
				Color color13;
				color13..ctor(0f, 255f, 30f, 130f);
				Color color14;
				color14..ctor(0f, 191f, 0f);
				blade.GetComponent<Renderer>().material.SetColor("_Color", color13);
				blade.GetComponent<Renderer>().material.SetColor("_EmissionColor", color14 * (num - 0.25f));
				foreach (GameObject gameObject7 in this.saberGlows)
				{
					gameObject7.GetComponent<Light>().color = Color.green;
				}
				if (this.isDarksaber)
				{
					this.darksaberCutout.SetColor("_Color", color13);
					this.darksaberCutout.SetColor("_EmissionColor", color14 * num);
				}
			}
			yield return new WaitForSeconds(0.25f);
			this.colorCycling = false;
			yield break;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00004039 File Offset: 0x00002239
		private IEnumerator ToggleSaber(string state, string reason)
		{
			this.saberCycling = true;
			if (state == "on")
			{
				this.isSaberOn = true;
				for (int i = 0; i < this.saberBlades.Count; i++)
				{
					GameObject gameObject = this.saberBlades[i];
					base.StartCoroutine(this.ScaleOverTime(gameObject.transform, this.bladeOnScales[i], this.ignitionSpeed));
				}
				this.whoosh.GetComponent<WhooshPoint>().minVelocity = 2f;
				this.whoosh.GetComponent<WhooshPoint>().maxVelocity = 14f;
				foreach (GameObject gameObject2 in this.saberGlows)
				{
					gameObject2.SetActive(true);
				}
				this.ignitionOnSound.Play();
				this.idleSound.Play();
				yield return new WaitForSeconds(this.ignitionDelay);
				this.saberCycling = false;
				this.saberDropped = false;
			}
			else
			{
				if (reason == "drop")
				{
					this.saberDropped = true;
					yield return new WaitForSeconds(2f);
				}
				this.isSaberOn = false;
				this.idleSound.Stop();
				this.ignitionOffSound.Play();
				this.whoosh.GetComponent<WhooshPoint>().minVelocity = 9999f;
				this.whoosh.GetComponent<WhooshPoint>().maxVelocity = 9999f;
				for (int j = 0; j < this.saberBlades.Count; j++)
				{
					GameObject gameObject3 = this.saberBlades[j];
					base.StartCoroutine(this.ScaleOverTime(gameObject3.transform, this.bladeOffScales[j], this.ignitionSpeed));
				}
				foreach (GameObject gameObject4 in this.saberGlows)
				{
					gameObject4.SetActive(false);
				}
				yield return new WaitForSeconds(this.ignitionDelay);
				this.saberCycling = false;
				this.saberDropped = false;
			}
			yield break;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00004056 File Offset: 0x00002256
		private IEnumerator ScaleOverTime(Transform blade, Vector3 d, float t)
		{
			if (!blade.gameObject.activeSelf)
			{
				blade.gameObject.SetActive(true);
			}
			blade.GetComponent<Collider>().enabled = false;
			if (this.saberTrailsActive)
			{
				foreach (GameObject gameObject in this.saberTrails)
				{
					gameObject.SetActive(false);
				}
			}
			float rate = 1f / t;
			float index = 0f;
			Vector3 startScale = blade.localScale;
			Vector3 endScale = startScale + d;
			while (index < 1f)
			{
				blade.localScale = Vector3.Lerp(startScale, endScale, index);
				index += rate * Time.deltaTime;
				yield return index;
			}
			blade.localScale = endScale;
			if (blade.localScale == new Vector3(blade.localScale.x, 0f, blade.localScale.z))
			{
				if (this.item.isPenetrating)
				{
					this.pierce.GetComponent<Damager>().UnPenetrateAll();
					if (this.pierce2 != null)
					{
						this.pierce2.GetComponent<Damager>().UnPenetrateAll();
					}
				}
				blade.gameObject.SetActive(false);
			}
			else
			{
				blade.GetComponent<Collider>().enabled = true;
				if (this.saberTrailsActive)
				{
					foreach (GameObject gameObject2 in this.saberTrails)
					{
						gameObject2.SetActive(true);
					}
				}
			}
			this.item.ResetColliderCollision();
			yield break;
		}

		// Token: 0x04000031 RID: 49
		private Item item;

		// Token: 0x04000032 RID: 50
		private AudioSource ignitionOnSound;

		// Token: 0x04000033 RID: 51
		private AudioSource ignitionOffSound;

		// Token: 0x04000034 RID: 52
		private AudioSource idleSound;

		// Token: 0x04000035 RID: 53
		private bool isSaberOn;

		// Token: 0x04000036 RID: 54
		private Transform saberBladesHolder;

		// Token: 0x04000037 RID: 55
		private List<GameObject> saberBlades = new List<GameObject>();

		// Token: 0x04000038 RID: 56
		private List<Vector3> bladeOffScales = new List<Vector3>();

		// Token: 0x04000039 RID: 57
		private List<Vector3> bladeOnScales = new List<Vector3>();

		// Token: 0x0400003A RID: 58
		private GameObject whoosh;

		// Token: 0x0400003B RID: 59
		private GameObject pierce;

		// Token: 0x0400003C RID: 60
		private Transform pierce2t;

		// Token: 0x0400003D RID: 61
		private GameObject pierce2;

		// Token: 0x0400003E RID: 62
		private float ignitionOnVol;

		// Token: 0x0400003F RID: 63
		private float ignitionOffVol;

		// Token: 0x04000040 RID: 64
		private float idleVol;

		// Token: 0x04000041 RID: 65
		private bool saberCycling;

		// Token: 0x04000042 RID: 66
		private bool saberDropped;

		// Token: 0x04000043 RID: 67
		private Coroutine co;

		// Token: 0x04000044 RID: 68
		private string bladeOrigColor;

		// Token: 0x04000045 RID: 69
		private List<GameObject> saberGlows = new List<GameObject>();

		// Token: 0x04000046 RID: 70
		private List<GameObject> saberTrails = new List<GameObject>();

		// Token: 0x04000047 RID: 71
		private int clickCounter;

		// Token: 0x04000048 RID: 72
		private bool colorCycling;

		// Token: 0x04000049 RID: 73
		private bool isDarksaber;

		// Token: 0x0400004A RID: 74
		private Material darksaberCutout;

		// Token: 0x0400004B RID: 75
		private bool recallAllowed;

		// Token: 0x0400004C RID: 76
		private bool recallTurnSaberOff;

		// Token: 0x0400004D RID: 77
		private float recallMaxDistance;

		// Token: 0x0400004E RID: 78
		private float recallStrength;

		// Token: 0x0400004F RID: 79
		private bool isRecalling;

		// Token: 0x04000050 RID: 80
		private float ignitionSpeed;

		// Token: 0x04000051 RID: 81
		private float ignitionDelay;

		// Token: 0x04000052 RID: 82
		private bool saberTrailsActive;

		// Token: 0x04000053 RID: 83
		private float saberTrailsSpeedDiff;
	}
}
