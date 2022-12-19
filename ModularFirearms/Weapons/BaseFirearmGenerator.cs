using System;
using System.Collections.Generic;
using ModularFirearms.Items;
using ModularFirearms.Shared;
using ThunderRoad;
using UnityEngine;

namespace ModularFirearms.Weapons
{
	// Token: 0x0200000D RID: 13
	public class BaseFirearmGenerator : MonoBehaviour
	{
		// Token: 0x06000052 RID: 82 RVA: 0x00003CC1 File Offset: 0x00001EC1
		public bool ProjectileIsSpawning()
		{
			return this.projectileIsSpawning;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00003CC9 File Offset: 0x00001EC9
		public void SetProjectileSpawningState(bool newState)
		{
			this.projectileIsSpawning = newState;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00003CD2 File Offset: 0x00001ED2
		public void SetFiringFlag(bool status)
		{
			this.isFiring = status;
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00003CDB File Offset: 0x00001EDB
		public bool TriggerIsPressed()
		{
			return this.triggerPressed;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00003CE3 File Offset: 0x00001EE3
		public void SetAmmoCounter(int value)
		{
			this.ammoCount = value;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00003CEC File Offset: 0x00001EEC
		public int GetAmmoCounter()
		{
			return this.ammoCount;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003CF4 File Offset: 0x00001EF4
		public void SetNextFireMode(FrameworkCore.FireMode NewFireMode)
		{
			this.fireModeSelection = NewFireMode;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00003CFD File Offset: 0x00001EFD
		public FrameworkCore.FireMode GetCurrentFireMode()
		{
			return this.fireModeSelection;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00003D05 File Offset: 0x00001F05
		public void IncSoundCounter()
		{
			this.soundCounter++;
			if (this.soundCounter > this.maxSoundCounter)
			{
				this.soundCounter = 1;
			}
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00003D2A File Offset: 0x00001F2A
		public bool ConsumeOneFromMagazine()
		{
			if (!(this.insertedMagazine != null))
			{
				return false;
			}
			if (this.insertedMagazine.GetAmmoCount() > 0)
			{
				this.insertedMagazine.ConsumeOne();
				return true;
			}
			return false;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00003D58 File Offset: 0x00001F58
		public int CountAmmoFromMagazine()
		{
			if (this.insertedMagazine != null)
			{
				return this.insertedMagazine.GetAmmoCount();
			}
			return 0;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00003D75 File Offset: 0x00001F75
		public void UpdateAmmoCounter()
		{
			if (!this.roundChambered)
			{
				this.SetAmmoCounter(this.CountAmmoFromMagazine());
				return;
			}
			this.SetAmmoCounter(this.CountAmmoFromMagazine() + 1);
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00003D9A File Offset: 0x00001F9A
		protected void StartLongPress()
		{
			this.checkForLongPress = true;
			this.lastSpellMenuPress = Time.time;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00003DAE File Offset: 0x00001FAE
		public void CancelLongPress()
		{
			this.checkForLongPress = false;
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00003DB8 File Offset: 0x00001FB8
		public void MagazineRelease()
		{
			try
			{
				if (this.magazineHolder.items.Count > 0)
				{
					this.magazineHolder.UnSnap(this.magazineHolder.items[0], false, true);
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00003E0C File Offset: 0x0000200C
		public void ForceDrop()
		{
			try
			{
				this.slideHandle.Release();
			}
			catch
			{
			}
			if (this.slideController != null)
			{
				this.slideController.LockSlide(true);
			}
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00003E50 File Offset: 0x00002050
		public void PlayFireSound()
		{
			if (this.soundCounter == 0)
			{
				return;
			}
			if (this.soundCounter == 1)
			{
				this.fireSound1.Play();
			}
			else if (this.soundCounter == 2)
			{
				this.fireSound2.Play();
			}
			else if (this.soundCounter == 3)
			{
				this.fireSound3.Play();
			}
			this.IncSoundCounter();
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00003EAC File Offset: 0x000020AC
		public void PreFireEffects()
		{
			FrameworkCore.Animate(this.animations, this.module.fireAnimationRef);
			if (this.muzzleFlash != null)
			{
				this.muzzleFlash.Play();
			}
			this.PlayFireSound();
			if (this.muzzleSmoke != null)
			{
				this.muzzleSmoke.Play();
			}
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00003F08 File Offset: 0x00002108
		protected bool TrackedFire()
		{
			if (this.slideController != null && this.slideController.IsLocked())
			{
				return false;
			}
			if (!this.roundChambered)
			{
				return false;
			}
			this.roundChambered = false;
			this.slideController.ChamberRoundVisible(this.roundChambered);
			this.Fire(false, true);
			if (this.ConsumeOneFromMagazine())
			{
				this.roundChambered = true;
				this.slideController.ChamberRoundVisible(this.roundChambered);
				this.slideController.BlowBack(false);
			}
			else
			{
				this.isRacked = false;
				this.isPulledBack = true;
				this.chamberRoundOnNext = true;
				this.slideController.LastShot();
			}
			this.UpdateAmmoCounter();
			return true;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00003FAC File Offset: 0x000021AC
		private void InitializeConfigurableJoint(float stabilizerRadius)
		{
			this.slideRB = this.slideObject.GetComponent<Rigidbody>();
			if (this.slideRB == null)
			{
				this.slideRB = this.slideObject.AddComponent<Rigidbody>();
			}
			this.slideRB.mass = 1f;
			this.slideRB.drag = 0f;
			this.slideRB.angularDrag = 0.05f;
			this.slideRB.useGravity = true;
			this.slideRB.isKinematic = false;
			this.slideRB.interpolation = 0;
			this.slideRB.collisionDetectionMode = 0;
			this.slideCapsuleStabilizer = this.slideCenterPosition.AddComponent<SphereCollider>();
			this.slideCapsuleStabilizer.radius = stabilizerRadius;
			this.slideCapsuleStabilizer.gameObject.layer = 21;
			Physics.IgnoreLayerCollision(21, 12);
			Physics.IgnoreLayerCollision(21, 15);
			Physics.IgnoreLayerCollision(21, 22);
			Physics.IgnoreLayerCollision(21, 23);
			this.slideForce = this.slideObject.AddComponent<ConstantForce>();
			this.connectedJoint = this.item.gameObject.AddComponent<ConfigurableJoint>();
			this.connectedJoint.connectedBody = this.slideRB;
			this.connectedJoint.anchor = new Vector3(0f, 0f, -0.5f * this.module.slideTravelDistance);
			this.connectedJoint.axis = Vector3.right;
			this.connectedJoint.autoConfigureConnectedAnchor = false;
			this.connectedJoint.connectedAnchor = Vector3.zero;
			this.connectedJoint.secondaryAxis = Vector3.up;
			this.connectedJoint.xMotion = 0;
			this.connectedJoint.yMotion = 0;
			this.connectedJoint.zMotion = 1;
			this.connectedJoint.angularXMotion = 0;
			this.connectedJoint.angularYMotion = 0;
			this.connectedJoint.angularZMotion = 0;
			ConfigurableJoint configurableJoint = this.connectedJoint;
			SoftJointLimit softJointLimit = default(SoftJointLimit);
			softJointLimit.limit = 0.5f * this.module.slideTravelDistance;
			softJointLimit.bounciness = 0f;
			softJointLimit.contactDistance = 0f;
			configurableJoint.linearLimit = softJointLimit;
			this.connectedJoint.massScale = 1f;
			this.connectedJoint.connectedMassScale = this.module.slideMassOffset;
		}

		// Token: 0x06000066 RID: 102 RVA: 0x000041F0 File Offset: 0x000023F0
		public bool SlideToggleLock()
		{
			if (this.insertedMagazine != null && this.insertedMagazine.GetAmmoCount() <= 0)
			{
				return false;
			}
			if (this.slideController == null)
			{
				return false;
			}
			if (this.slideController.IsLocked())
			{
				if (this.ConsumeOneFromMagazine())
				{
					this.roundChambered = true;
				}
				this.chamberRoundOnNext = false;
				this.playSoundOnNext = false;
				this.isRacked = true;
				this.isPulledBack = false;
				this.slideController.ForwardState();
				if (this.rackforwardSound != null)
				{
					this.rackforwardSound.Play();
				}
				this.UpdateAmmoCounter();
				return true;
			}
			if (this.slideController.IsHeld() && this.isPulledBack)
			{
				this.slideController.LockedBackState();
				if (this.emptySound != null)
				{
					this.emptySound.Play();
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x000042C8 File Offset: 0x000024C8
		private void Fire(bool firedByNPC = false, bool playEffects = true)
		{
			if (playEffects)
			{
				this.PreFireEffects();
			}
			if (firedByNPC)
			{
				return;
			}
			if (!this.useRaycast || !FrameworkCore.ShootRaycastDamage(this.muzzlePoint, this.raycastForce, this.rayCastMaxDist, 1f))
			{
				FrameworkCore.ShootProjectile(this.item, this.module.projectileID, this.muzzlePoint, FrameworkCore.GetItemSpellChargeID(this.item), this.module.bulletForce, this.module.throwMult, false, this.slideCapsuleStabilizer, new SetSpawningStatusDelegate(this.SetProjectileSpawningState));
			}
			FrameworkCore.ApplyRecoil(this.item.rb, this.module.recoilForces, this.module.throwMult, this.mainHandleHeldLeft, this.mainHandleHeldRight, this.module.hapticForce, this.module.recoilTorques);
			if (this.shellParticle != null)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.shellParticle.gameObject, null);
				gameObject.transform.position = this.shellParticle.transform.position;
				gameObject.transform.rotation = this.shellParticle.transform.rotation;
				gameObject.transform.parent = null;
				ParticleSystem component = gameObject.GetComponent<ParticleSystem>();
				if (component != null)
				{
					component.main.stopAction = 2;
					component.Play();
				}
			}
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00004428 File Offset: 0x00002628
		private void Awake()
		{
			this.soundCounter = 0;
			this.maxSoundCounter = 0;
			this.item = base.GetComponent<Item>();
			this.module = this.item.data.GetModule<FirearmModule>();
			this.useRaycast = this.module.useHitscan || FrameworkSettings.local.useHitscan;
			this.rayCastMaxDist = (this.module.useHitscan ? this.module.hitscanMaxDistance : FrameworkSettings.local.hitscanMaxDistance);
			if (this.rayCastMaxDist <= 0f)
			{
				this.rayCastMaxDist = float.PositiveInfinity;
			}
			this.raycastForce = this.module.bulletForce * this.module.hitscanForceMult;
			FrameworkCore.DisableCulling(this.item, false);
			if (!string.IsNullOrEmpty(this.module.rayCastPointRef))
			{
				this.rayCastPoint = this.item.GetCustomReference(this.module.rayCastPointRef, true);
			}
			if (!string.IsNullOrEmpty(this.module.muzzlePositionRef))
			{
				this.muzzlePoint = this.item.GetCustomReference(this.module.muzzlePositionRef, true);
			}
			if (!string.IsNullOrEmpty(this.module.shellEjectionRef))
			{
				this.shellEjectionPoint = this.item.GetCustomReference(this.module.shellEjectionRef, true);
			}
			if (!string.IsNullOrEmpty(this.module.animationRef))
			{
				this.animations = this.item.GetCustomReference(this.module.animationRef, true).GetComponent<Animator>();
			}
			if (!string.IsNullOrEmpty(this.module.fireSoundRef))
			{
				this.fireSound = this.item.GetCustomReference(this.module.fireSoundRef, true).GetComponent<AudioSource>();
			}
			if (!string.IsNullOrEmpty(this.module.fireSound1Ref))
			{
				this.fireSound1 = this.item.GetCustomReference(this.module.fireSound1Ref, true).GetComponent<AudioSource>();
				this.maxSoundCounter++;
				this.soundCounter = 1;
			}
			if (!string.IsNullOrEmpty(this.module.fireSound2Ref))
			{
				this.fireSound2 = this.item.GetCustomReference(this.module.fireSound2Ref, true).GetComponent<AudioSource>();
				this.maxSoundCounter++;
			}
			if (!string.IsNullOrEmpty(this.module.fireSound3Ref))
			{
				this.fireSound3 = this.item.GetCustomReference(this.module.fireSound3Ref, true).GetComponent<AudioSource>();
				this.maxSoundCounter++;
			}
			if (!string.IsNullOrEmpty(this.module.emptySoundRef))
			{
				this.emptySound = this.item.GetCustomReference(this.module.emptySoundRef, true).GetComponent<AudioSource>();
			}
			if (!string.IsNullOrEmpty(this.module.pullSoundRef))
			{
				this.pullbackSound = this.item.GetCustomReference(this.module.pullSoundRef, true).GetComponent<AudioSource>();
			}
			if (!string.IsNullOrEmpty(this.module.rackSoundRef))
			{
				this.rackforwardSound = this.item.GetCustomReference(this.module.rackSoundRef, true).GetComponent<AudioSource>();
			}
			if (!string.IsNullOrEmpty(this.module.flashRef))
			{
				this.muzzleFlash = this.item.GetCustomReference(this.module.flashRef, true).GetComponent<ParticleSystem>();
			}
			if (!string.IsNullOrEmpty(this.module.smokeRef))
			{
				this.muzzleSmoke = this.item.GetCustomReference(this.module.smokeRef, true).GetComponent<ParticleSystem>();
			}
			if (!string.IsNullOrEmpty(this.module.shellParticleRef))
			{
				this.shellParticle = this.item.GetCustomReference(this.module.shellParticleRef, true).GetComponent<ParticleSystem>();
			}
			if (!string.IsNullOrEmpty(this.module.mainHandleRef))
			{
				this.mainHandle = this.item.GetCustomReference(this.module.mainHandleRef, true).GetComponent<Handle>();
			}
			else
			{
				Debug.LogError("[Fisher-ModularFirearms][ERROR] No Reference to Main Handle (\"mainHandleRef\") in JSON! Weapon will not work as intended !!!");
			}
			if (!string.IsNullOrEmpty(this.module.slideHandleRef))
			{
				this.slideObject = this.item.GetCustomReference(this.module.slideHandleRef, true).gameObject;
			}
			else
			{
				Debug.LogError("[Fisher-ModularFirearms][ERROR] No Reference to Slide Handle (\"slideHandleRef\") in JSON! Weapon will not work as intended !!!");
			}
			if (!string.IsNullOrEmpty(this.module.slideCenterRef))
			{
				this.slideCenterPosition = this.item.GetCustomReference(this.module.slideCenterRef, true).gameObject;
			}
			else
			{
				Debug.LogError("[Fisher-ModularFirearms][ERROR] No Reference to Slide Center Position(\"slideCenterRef\") in JSON! Weapon will not work as intended...");
			}
			if (this.slideObject != null)
			{
				this.slideHandle = this.slideObject.GetComponent<Handle>();
			}
			this.lastSpellMenuPress = 0f;
			this.RACK_THRESHOLD = -0.1f * this.module.slideTravelDistance;
			this.PULL_THRESHOLD = -0.5f * this.module.slideTravelDistance;
			this.fireModeSelection = (FrameworkCore.FireMode)Enum.Parse(typeof(FrameworkCore.FireMode), this.module.fireMode);
			this.validMagazineIDs = new List<string>(this.module.acceptedMagazineIDs);
			this.item.OnHeldActionEvent += new Item.HeldActionDelegate(this.OnHeldAction);
			this.item.OnGrabEvent += new Item.GrabDelegate(this.OnAnyHandleGrabbed);
			this.item.OnUngrabEvent += new Item.ReleaseDelegate(this.OnAnyHandleUngrabbed);
			this.magazineHolder = this.item.GetComponentInChildren<Holder>();
			this.magazineHolder.Snapped += new Holder.HolderDelegate(this.OnMagazineInserted);
			this.magazineHolder.UnSnapped += new Holder.HolderDelegate(this.OnMagazineRemoved);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x000049C4 File Offset: 0x00002BC4
		private void Start()
		{
			if (this.fireSound1 != null)
			{
				this.fireSound1.volume = this.module.soundVolume;
			}
			if (this.fireSound2 != null)
			{
				this.fireSound2.volume = this.module.soundVolume;
			}
			if (this.fireSound3 != null)
			{
				this.fireSound3.volume = this.module.soundVolume;
			}
			this.InitializeConfigurableJoint(this.module.slideStabilizerRadius);
			this.slideController = new ChildRigidbodyController(this.item, this.module);
			this.slideController.InitializeSlide(this.slideObject);
			if (this.slideController == null)
			{
				Debug.LogError("[Fisher-ModularFirearms] ERROR! CHILD SLIDE CONTROLLER WAS NULL");
			}
			else
			{
				this.slideController.SetupSlide();
			}
			ItemData data = Catalog.GetData<ItemData>(this.module.acceptedMagazineIDs[0], true);
			if (data == null)
			{
				Debug.LogError("[Fisher-ModularFirearms][ERROR] No Magazine named " + this.module.acceptedMagazineIDs[0].ToString());
				return;
			}
			data.SpawnAsync(delegate(Item i)
			{
				try
				{
					this.magazineHolder.Snap(i, false, true);
					this.magazineHolder.data.disableTouch = !this.module.allowGrabMagazineFromGun;
				}
				catch
				{
					Debug.Log("[Fisher-ModularFirearms] EXCEPTION IN SNAPPING MAGAZINE ");
				}
			}, new Vector3?(this.item.transform.position), new Quaternion?(Quaternion.Euler(this.item.transform.rotation.eulerAngles)), null, false, null);
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00004B1C File Offset: 0x00002D1C
		protected void LateUpdate()
		{
			if (this.checkForLongPress)
			{
				if (this.spellMenuPressed)
				{
					if (Time.time - this.lastSpellMenuPress > this.module.longPressTime)
					{
						if (this.module.longPressToEject)
						{
							this.MagazineRelease();
						}
						this.CancelLongPress();
					}
				}
				else
				{
					this.CancelLongPress();
					if (!this.module.longPressToEject)
					{
						this.MagazineRelease();
					}
				}
			}
			if (!this.mainHandleHeldLeft && !this.mainHandleHeldRight)
			{
				this.triggerPressed = false;
				if (this.slideController != null)
				{
					this.slideController.LockSlide(true);
				}
			}
			if (this.slideObject.transform.localPosition.z <= this.PULL_THRESHOLD && !this.isPulledBack && this.slideController != null && this.slideController.IsHeld())
			{
				if (this.pullbackSound != null)
				{
					this.pullbackSound.Play();
				}
				this.isPulledBack = true;
				this.isRacked = false;
				this.playSoundOnNext = true;
				if (!this.roundChambered)
				{
					this.chamberRoundOnNext = true;
					this.UpdateAmmoCounter();
				}
				else
				{
					FrameworkCore.ShootProjectile(this.item, this.module.ammoID, this.shellEjectionPoint, null, this.module.shellEjectionForce, 1f, false, this.slideCapsuleStabilizer, null);
					this.roundChambered = false;
					this.chamberRoundOnNext = true;
				}
				this.slideController.ChamberRoundVisible(false);
			}
			if (this.slideObject.transform.localPosition.z > this.PULL_THRESHOLD - this.RACK_THRESHOLD && this.isPulledBack && this.CountAmmoFromMagazine() > 0)
			{
				this.slideController.ChamberRoundVisible(true);
			}
			if (this.slideObject.transform.localPosition.z >= this.RACK_THRESHOLD && !this.isRacked)
			{
				this.isRacked = true;
				this.isPulledBack = false;
				if (this.chamberRoundOnNext && this.ConsumeOneFromMagazine())
				{
					this.slideController.ChamberRoundVisible(true);
					this.chamberRoundOnNext = false;
					this.roundChambered = true;
				}
				if (this.playSoundOnNext)
				{
					if (this.rackforwardSound != null)
					{
						this.rackforwardSound.Play();
					}
					this.playSoundOnNext = false;
				}
				this.UpdateAmmoCounter();
			}
			if (this.slideController == null)
			{
				return;
			}
			this.slideController.FixCustomComponents();
			if (this.slideController.initialCheck)
			{
				return;
			}
			try
			{
				if (this.mainHandleHeldRight || this.mainHandleHeldLeft)
				{
					this.slideController.UnlockSlide(true);
					this.slideController.initialCheck = true;
				}
			}
			catch
			{
				Debug.Log("[Fisher-ModularFirearms] Slide EXCEPTION");
			}
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00004DC4 File Offset: 0x00002FC4
		public void OnHeldAction(RagdollHand interactor, Handle handle, Interactable.Action action)
		{
			if (handle.Equals(this.mainHandle))
			{
				if (action == null)
				{
					this.triggerPressed = true;
					if (!this.isFiring)
					{
						base.StartCoroutine(FrameworkCore.GeneralFire(new TrackFiredDelegate(this.TrackedFire), new TriggerPressedDelegate(this.TriggerIsPressed), this.fireModeSelection, this.module.fireRate, this.module.burstNumber, this.emptySound, new IsFiringDelegate(this.SetFiringFlag), new IsSpawningDelegate(this.ProjectileIsSpawning)));
					}
				}
				if (action == 1)
				{
					this.triggerPressed = false;
				}
				if (action == 2)
				{
					this.spellMenuPressed = true;
					if (this.SlideToggleLock())
					{
						return;
					}
					this.StartLongPress();
				}
				if (action == 3)
				{
					this.spellMenuPressed = false;
				}
			}
			if (action == 4)
			{
				if (handle.Equals(this.mainHandle))
				{
					if (interactor.playerHand == Player.local.handRight)
					{
						this.mainHandleHeldRight = true;
					}
					if (interactor.playerHand == Player.local.handLeft)
					{
						this.mainHandleHeldLeft = true;
					}
					if ((this.mainHandleHeldRight || this.mainHandleHeldLeft) && this.slideController != null)
					{
						this.slideController.UnlockSlide(true);
					}
				}
				if (handle.Equals(this.slideHandle))
				{
					if (interactor.playerHand == Player.local.handRight)
					{
						this.slideHandleHeldRight = true;
					}
					if (interactor.playerHand == Player.local.handLeft)
					{
						this.slideHandleHeldLeft = true;
					}
					if (this.slideController != null)
					{
						this.slideController.SetHeld(true);
					}
					this.slideController.ForwardState();
				}
			}
			if (action == 5)
			{
				if (handle.Equals(this.mainHandle))
				{
					if (interactor.playerHand == Player.local.handRight)
					{
						this.mainHandleHeldRight = false;
					}
					if (interactor.playerHand == Player.local.handLeft)
					{
						this.mainHandleHeldLeft = false;
					}
					if (!this.mainHandleHeldRight && !this.mainHandleHeldLeft)
					{
						if (interactor.playerHand == Player.local.handRight)
						{
							this.slideHandleHeldRight = false;
						}
						if (interactor.playerHand == Player.local.handLeft)
						{
							this.slideHandleHeldLeft = false;
						}
						if (this.slideController != null)
						{
							this.slideController.LockSlide(true);
						}
						this.ForceDrop();
					}
				}
				if (handle.Equals(this.slideHandle) && this.slideController != null)
				{
					this.slideController.SetHeld(false);
				}
			}
		}

		// Token: 0x0600006C RID: 108 RVA: 0x0000503C File Offset: 0x0000323C
		protected void OnMagazineInserted(Item interactiveObject)
		{
			try
			{
				interactiveObject.IgnoreColliderCollision(this.slideCapsuleStabilizer);
				this.insertedMagazine = interactiveObject.GetComponent<InteractiveMagazine>();
				if (this.insertedMagazine != null)
				{
					this.insertedMagazine.Insert();
					this.magazineHolder.data.disableTouch = !this.module.allowGrabMagazineFromGun;
					Handle componentInChildren = interactiveObject.GetComponentInChildren<Handle>();
					if (componentInChildren != null)
					{
						componentInChildren.data.disableTouch = !this.module.allowGrabMagazineFromGun;
					}
					string magazineID = this.insertedMagazine.GetMagazineID();
					bool flag = false;
					using (List<string>.Enumerator enumerator = this.validMagazineIDs.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.Equals(magazineID))
							{
								flag = true;
								break;
							}
						}
					}
					if (!flag)
					{
						Debug.LogWarning(string.Concat(new string[]
						{
							"[Fisher-ModularFirearms] ",
							this.item.name,
							" REJECTED MAGAZINE ",
							magazineID.ToString(),
							". Allowed Magazines are:  ",
							string.Join(",", this.validMagazineIDs.ToArray())
						}));
						this.MagazineRelease();
					}
				}
				else
				{
					Debug.LogWarning("[Fisher-ModularFirearms] Rejected MAGAZINE Due to NULL InteractiveMagazine Object");
					this.magazineHolder.UnSnap(interactiveObject, false, true);
					this.insertedMagazine = null;
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("[Fisher-ModularFirearms][ERROR] Exception in Adding magazine: " + ex.ToString());
			}
			if (this.roundChambered)
			{
				this.UpdateAmmoCounter();
			}
		}

		// Token: 0x0600006D RID: 109 RVA: 0x000051EC File Offset: 0x000033EC
		protected void OnMagazineRemoved(Item interactiveObject)
		{
			try
			{
				if (this.insertedMagazine != null)
				{
					Handle componentInChildren = interactiveObject.GetComponentInChildren<Handle>();
					if (componentInChildren != null)
					{
						componentInChildren.data.disableTouch = false;
					}
					this.insertedMagazine.Eject(this.item);
					this.insertedMagazine = null;
				}
			}
			catch
			{
				Debug.LogWarning("[Fisher-ModularFirearms] Unable to Eject the Magazine!");
			}
			this.magazineHolder.data.disableTouch = false;
			this.UpdateAmmoCounter();
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00005274 File Offset: 0x00003474
		public void OnAnyHandleGrabbed(Handle handle, RagdollHand interactor)
		{
			if (handle.Equals(this.mainHandle))
			{
				if (interactor.playerHand == Player.local.handRight)
				{
					this.mainHandleHeldRight = true;
				}
				if (interactor.playerHand == Player.local.handLeft)
				{
					this.mainHandleHeldLeft = true;
				}
				if ((this.mainHandleHeldRight || this.mainHandleHeldLeft) && this.slideController != null)
				{
					this.slideController.UnlockSlide(true);
				}
			}
			if (handle.Equals(this.slideHandle))
			{
				if (interactor.playerHand == Player.local.handRight)
				{
					this.slideHandleHeldRight = true;
				}
				if (interactor.playerHand == Player.local.handLeft)
				{
					this.slideHandleHeldLeft = true;
				}
				this.slideController.SetHeld(true);
				this.slideController.ForwardState();
			}
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00005350 File Offset: 0x00003550
		public void OnAnyHandleUngrabbed(Handle handle, RagdollHand interactor, bool throwing)
		{
			if (handle.Equals(this.mainHandle))
			{
				if (interactor.playerHand == Player.local.handRight)
				{
					this.mainHandleHeldRight = false;
				}
				if (interactor.playerHand == Player.local.handLeft)
				{
					this.mainHandleHeldLeft = false;
				}
				if (!this.mainHandleHeldRight && !this.mainHandleHeldLeft)
				{
					if (this.slideController != null)
					{
						this.slideController.LockSlide(true);
					}
					this.ForceDrop();
				}
			}
			if (handle.Equals(this.slideHandle))
			{
				if (interactor.playerHand == Player.local.handRight)
				{
					this.slideHandleHeldRight = false;
				}
				if (interactor.playerHand == Player.local.handLeft)
				{
					this.slideHandleHeldLeft = false;
				}
				this.slideController.SetHeld(false);
			}
		}

		// Token: 0x0400004B RID: 75
		protected Item item;

		// Token: 0x0400004C RID: 76
		protected FirearmModule module;

		// Token: 0x0400004D RID: 77
		public float lastSpellMenuPress;

		// Token: 0x0400004E RID: 78
		public bool isLongPress;

		// Token: 0x0400004F RID: 79
		public bool checkForLongPress;

		// Token: 0x04000050 RID: 80
		private bool useRaycast;

		// Token: 0x04000051 RID: 81
		private float rayCastMaxDist;

		// Token: 0x04000052 RID: 82
		private float raycastForce;

		// Token: 0x04000053 RID: 83
		protected Holder magazineHolder;

		// Token: 0x04000054 RID: 84
		protected InteractiveMagazine insertedMagazine;

		// Token: 0x04000055 RID: 85
		protected List<string> validMagazineIDs;

		// Token: 0x04000056 RID: 86
		private float PULL_THRESHOLD;

		// Token: 0x04000057 RID: 87
		private float RACK_THRESHOLD;

		// Token: 0x04000058 RID: 88
		private SphereCollider slideCapsuleStabilizer;

		// Token: 0x04000059 RID: 89
		protected Handle slideHandle;

		// Token: 0x0400005A RID: 90
		private ChildRigidbodyController slideController;

		// Token: 0x0400005B RID: 91
		private GameObject slideObject;

		// Token: 0x0400005C RID: 92
		private GameObject slideCenterPosition;

		// Token: 0x0400005D RID: 93
		private ConstantForce slideForce;

		// Token: 0x0400005E RID: 94
		private Rigidbody slideRB;

		// Token: 0x0400005F RID: 95
		public ConfigurableJoint connectedJoint;

		// Token: 0x04000060 RID: 96
		protected Handle mainHandle;

		// Token: 0x04000061 RID: 97
		protected Transform rayCastPoint;

		// Token: 0x04000062 RID: 98
		protected Transform muzzlePoint;

		// Token: 0x04000063 RID: 99
		protected Transform shellEjectionPoint;

		// Token: 0x04000064 RID: 100
		protected ParticleSystem muzzleFlash;

		// Token: 0x04000065 RID: 101
		protected ParticleSystem muzzleSmoke;

		// Token: 0x04000066 RID: 102
		protected ParticleSystem shellParticle;

		// Token: 0x04000067 RID: 103
		protected AudioSource fireSound;

		// Token: 0x04000068 RID: 104
		protected AudioSource fireSound1;

		// Token: 0x04000069 RID: 105
		protected AudioSource fireSound2;

		// Token: 0x0400006A RID: 106
		protected AudioSource fireSound3;

		// Token: 0x0400006B RID: 107
		protected AudioSource emptySound;

		// Token: 0x0400006C RID: 108
		protected AudioSource reloadSound;

		// Token: 0x0400006D RID: 109
		protected AudioSource pullbackSound;

		// Token: 0x0400006E RID: 110
		protected AudioSource rackforwardSound;

		// Token: 0x0400006F RID: 111
		protected Animator animations;

		// Token: 0x04000070 RID: 112
		public FrameworkCore.FireMode fireModeSelection;

		// Token: 0x04000071 RID: 113
		public List<int> allowedFireModes;

		// Token: 0x04000072 RID: 114
		public int ammoCount;

		// Token: 0x04000073 RID: 115
		public bool mainHandleHeldLeft;

		// Token: 0x04000074 RID: 116
		public bool mainHandleHeldRight;

		// Token: 0x04000075 RID: 117
		public bool slideHandleHeldLeft;

		// Token: 0x04000076 RID: 118
		public bool slideHandleHeldRight;

		// Token: 0x04000077 RID: 119
		public bool projectileIsSpawning;

		// Token: 0x04000078 RID: 120
		public bool isFiring;

		// Token: 0x04000079 RID: 121
		public bool triggerPressed;

		// Token: 0x0400007A RID: 122
		public bool spellMenuPressed;

		// Token: 0x0400007B RID: 123
		public bool isRacked = true;

		// Token: 0x0400007C RID: 124
		public bool isPulledBack;

		// Token: 0x0400007D RID: 125
		public bool roundChambered;

		// Token: 0x0400007E RID: 126
		private bool chamberRoundOnNext;

		// Token: 0x0400007F RID: 127
		private bool playSoundOnNext;

		// Token: 0x04000080 RID: 128
		private int soundCounter;

		// Token: 0x04000081 RID: 129
		private int maxSoundCounter;
	}
}
