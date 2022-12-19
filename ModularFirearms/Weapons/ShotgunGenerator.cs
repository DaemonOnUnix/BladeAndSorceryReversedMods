using System;
using ModularFirearms.Items;
using ModularFirearms.Projectiles;
using ModularFirearms.Shared;
using ThunderRoad;
using UnityEngine;

namespace ModularFirearms.Weapons
{
	// Token: 0x0200000F RID: 15
	internal class ShotgunGenerator : MonoBehaviour
	{
		// Token: 0x06000073 RID: 115 RVA: 0x00005490 File Offset: 0x00003690
		protected void Awake()
		{
			this.item = base.GetComponent<Item>();
			this.module = this.item.data.GetModule<FirearmModule>();
			this.useRaycast = this.module.useHitscan || FrameworkSettings.local.useHitscan;
			this.rayCastMaxDist = (this.module.useHitscan ? this.module.hitscanMaxDistance : FrameworkSettings.local.hitscanMaxDistance);
			if (this.rayCastMaxDist <= 0f)
			{
				this.rayCastMaxDist = float.PositiveInfinity;
			}
			this.raycastForce = this.module.bulletForce * this.module.hitscanForceMult * this.module.shotgunForceMult;
			if (!string.IsNullOrEmpty(this.module.muzzlePositionRef))
			{
				this.muzzlePoint = this.item.GetCustomReference(this.module.muzzlePositionRef, true);
			}
			if (!string.IsNullOrEmpty(this.module.rayCastPointRef))
			{
				this.rayCastPoint = this.item.GetCustomReference(this.module.muzzlePositionRef, true);
			}
			if (!string.IsNullOrEmpty(this.module.shellEjectionRef))
			{
				this.shellEjectionPoint = this.item.GetCustomReference(this.module.shellEjectionRef, true);
			}
			if (!string.IsNullOrEmpty(this.module.animationRef))
			{
				this.Animations = this.item.GetCustomReference(this.module.animationRef, true).GetComponent<Animator>();
			}
			if (!string.IsNullOrEmpty(this.module.fireSoundRef))
			{
				this.fireSound = this.item.GetCustomReference(this.module.fireSoundRef, true).GetComponent<AudioSource>();
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
			if (!string.IsNullOrEmpty(this.module.shellInsertSoundRef))
			{
				this.shellInsertSound = this.item.GetCustomReference(this.module.shellInsertSoundRef, true).GetComponent<AudioSource>();
			}
			if (!string.IsNullOrEmpty(this.module.flashRef))
			{
				this.muzzleFlash = this.item.GetCustomReference(this.module.flashRef, true).GetComponent<ParticleSystem>();
			}
			if (!string.IsNullOrEmpty(this.module.smokeRef))
			{
				this.muzzleSmoke = this.item.GetCustomReference(this.module.smokeRef, true).GetComponent<ParticleSystem>();
			}
			if (!string.IsNullOrEmpty(this.module.mainHandleRef))
			{
				this.gunGrip = this.item.GetCustomReference(this.module.mainHandleRef, true).GetComponent<Handle>();
			}
			else
			{
				Debug.LogError("[ModularFirearmsFramework][ERROR] No Reference to Main Handle (\"mainHandleRef\") in JSON! Weapon will not work as intended !!!");
			}
			if (!string.IsNullOrEmpty(this.module.slideHandleRef))
			{
				this.slideObject = this.item.GetCustomReference(this.module.slideHandleRef, true).gameObject;
			}
			else
			{
				Debug.LogError("[ModularFirearmsFramework][ERROR] No Reference to Slide Handle (\"slideHandleRef\") in JSON! Weapon will not work as intended !!!");
			}
			if (!string.IsNullOrEmpty(this.module.slideCenterRef))
			{
				this.slideCenterPosition = this.item.GetCustomReference(this.module.slideCenterRef, true).gameObject;
			}
			else
			{
				Debug.LogError("[ModularFirearmsFramework][ERROR] No Reference to Slide Center Position(\"slideCenterRef\") in JSON! Weapon will not work as intended...");
			}
			if (this.slideObject != null)
			{
				this.slideHandle = this.slideObject.GetComponent<Handle>();
			}
			if (!string.IsNullOrEmpty(this.module.flashlightRef))
			{
				this.attachedLight = this.item.GetCustomReference(this.module.flashlightRef, true).GetComponent<Light>();
			}
			this.RACK_THRESHOLD = -0.1f * this.module.slideTravelDistance;
			this.PULL_THRESHOLD = -0.5f * this.module.slideTravelDistance;
			this.currentReceiverAmmo = this.module.maxReceiverAmmo;
			this.item.OnHeldActionEvent += new Item.HeldActionDelegate(this.OnHeldAction);
			this.item.OnGrabEvent += new Item.GrabDelegate(this.OnAnyHandleGrabbed);
			this.item.OnUngrabEvent += new Item.ReleaseDelegate(this.OnAnyHandleUngrabbed);
			this.shellReceiver = this.item.GetCustomReference(this.module.shellReceiverDef, true).GetComponentInChildren<Holder>();
			this.shellReceiver.Snapped += new Holder.HolderDelegate(this.OnShellInserted);
			this.shellReceiver.UnSnapped += new Holder.HolderDelegate(this.OnShellRemoved);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00005960 File Offset: 0x00003B60
		protected void Start()
		{
			this.num1 = this.slideHandle.data.positionDamperMultiplier;
			this.num2 = this.slideHandle.data.positionSpringMultiplier;
			this.num3 = this.slideHandle.data.rotationDamperMultiplier;
			this.num4 = this.slideHandle.data.rotationSpringMultiplier;
			this.InitializeConfigurableJoint(this.module.slideStabilizerRadius);
			this.slideController = new ChildRigidbodyController(this.item, this.module);
			this.slideController.InitializeSlide(this.slideObject);
			if (this.slideController == null)
			{
				Debug.LogError("[ModularFirearmsFramework] ERROR! CHILD SLIDE CONTROLLER WAS NULL");
			}
			else
			{
				this.slideController.SetupSlide();
			}
			this.shellReceiver.data.disableTouch = true;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00005A30 File Offset: 0x00003C30
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

		// Token: 0x06000076 RID: 118 RVA: 0x00005C74 File Offset: 0x00003E74
		public void OnHeldAction(RagdollHand interactor, Handle handle, Interactable.Action action)
		{
			if (action == 2 && this.attachedLight != null)
			{
				this.attachedLight.enabled = !this.attachedLight.enabled;
				if (this.emptySound != null)
				{
					this.emptySound.Play();
				}
			}
			if (handle.name.Equals(this.slideHandle.name))
			{
				if ((action == null || action == 2) && !this.holdingSlideTrigger)
				{
					this.holdingSlideTrigger = true;
					this.slideController.LockSlide(false);
					if (this.emptySound != null)
					{
						this.emptySound.Play();
					}
				}
				if ((action == 1 || action == 3) && this.holdingSlideTrigger)
				{
					this.holdingSlideTrigger = false;
					this.slideController.UnlockSlide(false);
				}
				if (action == 5)
				{
					if (this.holdingSlideTrigger)
					{
						this.holdingSlideTrigger = false;
						this.slideController.UnlockSlide(true);
					}
					if (interactor.playerHand == Player.local.handRight)
					{
						this.slideGripHeldRight = false;
					}
					if (interactor.playerHand == Player.local.handLeft)
					{
						this.slideGripHeldLeft = false;
					}
					this.slideController.SetHeld(false);
				}
				if (action == 4)
				{
					if (interactor.playerHand == Player.local.handRight)
					{
						this.slideGripHeldRight = true;
					}
					if (interactor.playerHand == Player.local.handLeft)
					{
						this.slideGripHeldLeft = true;
					}
					this.slideController.SetHeld(true);
				}
			}
			if (handle.Equals(this.gunGrip))
			{
				if (action == 4 && (this.gunGripHeldRight || this.gunGripHeldLeft) && this.slideController != null)
				{
					this.slideController.UnlockSlide(true);
				}
				if (action == 5)
				{
					if (this.slideController != null)
					{
						this.slideController.LockSlide(true);
					}
					try
					{
						this.slideHandle.Release();
					}
					catch
					{
					}
				}
				if (action == null)
				{
					this.triggerPressed = true;
					if (!this.holdingSlideTrigger)
					{
						this.slideController.LockSlide(false);
					}
					if (!this.TrackedFire() && this.emptySound != null)
					{
						this.emptySound.Play();
					}
				}
				if (action == 1)
				{
					this.triggerPressed = false;
					if (!this.holdingSlideTrigger)
					{
						this.slideController.UnlockSlide(false);
					}
				}
			}
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00005EC4 File Offset: 0x000040C4
		public void OnAnyHandleGrabbed(Handle handle, RagdollHand interactor)
		{
			if (handle.Equals(this.gunGrip))
			{
				if (interactor.playerHand == Player.local.handRight)
				{
					this.gunGripHeldRight = true;
				}
				if (interactor.playerHand == Player.local.handLeft)
				{
					this.gunGripHeldLeft = true;
				}
				if ((this.gunGripHeldRight || this.gunGripHeldLeft) && this.slideController != null)
				{
					this.slideHandle.data.positionDamperMultiplier = this.num1;
					this.slideHandle.data.positionSpringMultiplier = this.num2;
					this.slideHandle.data.rotationDamperMultiplier = this.num3;
					this.slideHandle.data.rotationSpringMultiplier = this.num4;
					this.slideController.UnlockSlide(true);
				}
			}
			if (handle.name.Equals(this.slideHandle.name))
			{
				if (interactor.playerHand == Player.local.handRight)
				{
					this.slideGripHeldRight = true;
				}
				if (interactor.playerHand == Player.local.handLeft)
				{
					this.slideGripHeldLeft = true;
				}
				this.slideController.SetHeld(true);
			}
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00005FFC File Offset: 0x000041FC
		public void OnAnyHandleUngrabbed(Handle handle, RagdollHand interactor, bool throwing)
		{
			if (handle.Equals(this.gunGrip))
			{
				if (interactor.playerHand == Player.local.handRight)
				{
					this.gunGripHeldRight = false;
				}
				if (interactor.playerHand == Player.local.handLeft)
				{
					this.gunGripHeldLeft = false;
				}
				if (!this.gunGripHeldRight && !this.gunGripHeldLeft && this.slideController != null)
				{
					this.slideHandle.data.positionDamperMultiplier = 1f;
					this.slideHandle.data.positionSpringMultiplier = 1f;
					this.slideHandle.data.rotationDamperMultiplier = 1f;
					this.slideHandle.data.rotationSpringMultiplier = 1f;
					this.slideController.LockSlide(true);
				}
			}
			if (handle.name.Equals(this.slideHandle.name))
			{
				if (interactor.playerHand == Player.local.handRight)
				{
					this.slideGripHeldRight = false;
				}
				if (interactor.playerHand == Player.local.handLeft)
				{
					this.slideGripHeldLeft = false;
				}
				this.slideController.SetHeld(false);
			}
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00006130 File Offset: 0x00004330
		protected void OnShellInserted(Item interactiveObject)
		{
			try
			{
				InteractiveAmmo component = interactiveObject.GetComponent<InteractiveAmmo>();
				this.shellReceiver.UnSnap(interactiveObject, false, true);
				if (component != null && component.GetAmmoType().Equals(FrameworkCore.AmmoType.ShotgunShell))
				{
					if (this.shellInsertSound != null)
					{
						this.shellInsertSound.Play();
					}
					this.chamberRoundOnNext = true;
					this.currentReceiverAmmo++;
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("[ModularFirearmsFramework][ERROR] Exception in Adding magazine: " + ex.ToString());
			}
		}

		// Token: 0x0600007A RID: 122 RVA: 0x000061D0 File Offset: 0x000043D0
		protected void OnShellRemoved(Item interactiveObject)
		{
			try
			{
				InteractiveAmmo component = interactiveObject.GetComponent<InteractiveAmmo>();
				if (component != null && component.GetAmmoType().Equals(FrameworkCore.AmmoType.ShotgunShell))
				{
					interactiveObject.Despawn();
				}
			}
			catch (Exception ex)
			{
				Debug.Log("[ModularFirearmsFramework][ERROR] Exception in removing shell from receiver." + ex.ToString());
			}
		}

		// Token: 0x0600007B RID: 123 RVA: 0x0000623C File Offset: 0x0000443C
		public void PreFireEffects()
		{
			if (this.muzzleFlash != null)
			{
				this.muzzleFlash.Play();
			}
			if (this.fireSound != null)
			{
				this.fireSound.Play();
			}
			if (this.muzzleSmoke != null)
			{
				this.muzzleSmoke.Play();
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00006294 File Offset: 0x00004494
		public bool Fire()
		{
			this.PreFireEffects();
			if (this.useRaycast)
			{
				FrameworkCore.ShootRaycastDamage(this.muzzlePoint, this.raycastForce, this.rayCastMaxDist, 1f);
			}
			Catalog.GetData<ItemData>(this.module.projectileID, true);
			string imbueSpell = FrameworkCore.GetItemSpellChargeID(this.item);
			ItemData data = Catalog.GetData<ItemData>(this.module.projectileID, true);
			if (this.muzzlePoint == null || string.IsNullOrEmpty(this.module.projectileID))
			{
				return false;
			}
			if (data == null)
			{
				Debug.LogError("[ModularFirearmsFramework][ERROR] No projectile named " + this.module.projectileID.ToString());
				return false;
			}
			Vector3[] buckshotOffsetPosiitions = FrameworkCore.buckshotOffsetPosiitions;
			for (int j = 0; j < buckshotOffsetPosiitions.Length; j++)
			{
				Vector3 offsetVec = buckshotOffsetPosiitions[j];
				data.SpawnAsync(delegate(Item i)
				{
					try
					{
						i.Throw(1f, 2);
						this.item.IgnoreObjectCollision(i);
						i.IgnoreObjectCollision(this.item);
						i.IgnoreRagdollCollision(Player.local.creature.ragdoll);
						FrameworkCore.IgnoreProjectile(this.item, i, true);
						i.transform.position = this.muzzlePoint.position + offsetVec;
						i.transform.rotation = Quaternion.Euler(this.muzzlePoint.rotation.eulerAngles);
						i.rb.velocity = this.item.rb.velocity;
						i.rb.AddForce(i.rb.transform.forward * 1000f * this.module.bulletForce);
						if (this.slideCapsuleStabilizer != null)
						{
							try
							{
								i.IgnoreColliderCollision(this.slideCapsuleStabilizer);
								foreach (ColliderGroup colliderGroup in this.item.colliderGroups)
								{
									foreach (Collider collider in colliderGroup.colliders)
									{
										Physics.IgnoreCollision(i.colliderGroups[0].colliders[0], collider);
									}
								}
							}
							catch
							{
							}
						}
						BasicProjectile component = i.gameObject.GetComponent<BasicProjectile>();
						if (component != null)
						{
							component.SetShooterItem(this.item);
						}
						if (!string.IsNullOrEmpty(imbueSpell) && component != null)
						{
							component.AddChargeToQueue(imbueSpell);
						}
					}
					catch (Exception ex)
					{
						Debug.Log("[ModularFirearmsFramework] EXCEPTION IN SPAWNING " + ex.Message + " \n " + ex.StackTrace);
					}
				}, new Vector3?(Vector3.zero), new Quaternion?(Quaternion.Euler(Vector3.zero)), null, false, null);
			}
			FrameworkCore.ApplyRecoil(this.item.rb, this.module.recoilForces, 1f, this.gunGripHeldLeft || this.slideGripHeldLeft, this.gunGripHeldRight || this.slideGripHeldRight, this.module.hapticForce, null);
			return true;
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00006403 File Offset: 0x00004603
		protected bool TrackedFire()
		{
			if (this.slideController != null && this.slideController.IsLocked())
			{
				return false;
			}
			if (!this.roundChambered || this.roundSpent)
			{
				return false;
			}
			this.Fire();
			this.roundSpent = true;
			return true;
		}

		// Token: 0x0600007E RID: 126 RVA: 0x0000643D File Offset: 0x0000463D
		public void SetFiringFlag(bool status)
		{
			this.isFiring = status;
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00006446 File Offset: 0x00004646
		public bool TriggerIsPressed()
		{
			return this.triggerPressed;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x0000644E File Offset: 0x0000464E
		public int CountAmmoFromReceiver()
		{
			return this.currentReceiverAmmo;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00006456 File Offset: 0x00004656
		public bool ConsumeOneFromReceiver()
		{
			if (this.currentReceiverAmmo > 0)
			{
				this.currentReceiverAmmo--;
				return true;
			}
			return false;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00006474 File Offset: 0x00004674
		protected void LateUpdate()
		{
			if (this.slideObject.transform.localPosition.z <= this.PULL_THRESHOLD && !this.isPulledBack && this.slideController != null && this.slideController.IsHeld())
			{
				if (this.pullbackSound != null)
				{
					this.pullbackSound.Play();
				}
				this.isPulledBack = true;
				this.isRacked = false;
				this.playSoundOnNext = true;
				this.slideController.LockedBackState();
				FrameworkCore.Animate(this.Animations, this.module.openAnimationRef);
				this.shellReceiver.data.disableTouch = false;
				if (this.roundChambered)
				{
					if (this.roundSpent)
					{
						FrameworkCore.ShootProjectile(this.item, this.module.shellID, this.shellEjectionPoint, null, this.module.shellEjectionForce, 1f, false, this.slideCapsuleStabilizer, null);
					}
					else
					{
						FrameworkCore.ShootProjectile(this.item, this.module.ammoID, this.shellEjectionPoint, null, this.module.shellEjectionForce, 1f, false, this.slideCapsuleStabilizer, null);
					}
					this.roundChambered = false;
					this.slideController.ChamberRoundVisible(false);
				}
				if (this.CountAmmoFromReceiver() > 0)
				{
					this.chamberRoundOnNext = true;
				}
			}
			if (this.slideObject.transform.localPosition.z > this.PULL_THRESHOLD - this.RACK_THRESHOLD && this.isPulledBack && this.CountAmmoFromReceiver() > 0)
			{
				this.slideController.ChamberRoundVisible(true);
			}
			if (this.slideObject.transform.localPosition.z >= this.RACK_THRESHOLD && !this.isRacked)
			{
				this.isRacked = true;
				this.isPulledBack = false;
				this.shellReceiver.data.disableTouch = true;
				this.slideController.ForwardState();
				FrameworkCore.Animate(this.Animations, this.module.closeAnimationRef);
				if (this.playSoundOnNext)
				{
					if (this.rackforwardSound != null)
					{
						this.rackforwardSound.Play();
					}
					this.playSoundOnNext = false;
				}
				if (this.chamberRoundOnNext)
				{
					if (!this.ConsumeOneFromReceiver())
					{
						return;
					}
					this.slideController.ChamberRoundVisible(true);
					this.chamberRoundOnNext = false;
					this.roundChambered = true;
					this.roundSpent = false;
				}
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
				if (this.gunGripHeldRight || this.gunGripHeldLeft)
				{
					this.slideController.UnlockSlide(true);
					this.slideController.initialCheck = true;
				}
			}
			catch
			{
				Debug.Log("[ModularFirearmsFramework] Slide EXCEPTION");
			}
		}

		// Token: 0x04000086 RID: 134
		protected Item item;

		// Token: 0x04000087 RID: 135
		protected FirearmModule module;

		// Token: 0x04000088 RID: 136
		protected Holder shellReceiver;

		// Token: 0x04000089 RID: 137
		private Light attachedLight;

		// Token: 0x0400008A RID: 138
		private float PULL_THRESHOLD;

		// Token: 0x0400008B RID: 139
		private float RACK_THRESHOLD;

		// Token: 0x0400008C RID: 140
		private SphereCollider slideCapsuleStabilizer;

		// Token: 0x0400008D RID: 141
		protected Handle slideHandle;

		// Token: 0x0400008E RID: 142
		private ChildRigidbodyController slideController;

		// Token: 0x0400008F RID: 143
		private GameObject slideObject;

		// Token: 0x04000090 RID: 144
		private GameObject slideCenterPosition;

		// Token: 0x04000091 RID: 145
		private ConstantForce slideForce;

		// Token: 0x04000092 RID: 146
		private Rigidbody slideRB;

		// Token: 0x04000093 RID: 147
		private bool holdingSlideTrigger;

		// Token: 0x04000094 RID: 148
		public ConfigurableJoint connectedJoint;

		// Token: 0x04000095 RID: 149
		protected Handle gunGrip;

		// Token: 0x04000096 RID: 150
		protected Transform muzzlePoint;

		// Token: 0x04000097 RID: 151
		protected Transform rayCastPoint;

		// Token: 0x04000098 RID: 152
		protected Transform shellEjectionPoint;

		// Token: 0x04000099 RID: 153
		protected ParticleSystem muzzleFlash;

		// Token: 0x0400009A RID: 154
		protected ParticleSystem muzzleSmoke;

		// Token: 0x0400009B RID: 155
		protected AudioSource fireSound;

		// Token: 0x0400009C RID: 156
		protected AudioSource emptySound;

		// Token: 0x0400009D RID: 157
		protected AudioSource pullbackSound;

		// Token: 0x0400009E RID: 158
		protected AudioSource rackforwardSound;

		// Token: 0x0400009F RID: 159
		private AudioSource shellInsertSound;

		// Token: 0x040000A0 RID: 160
		protected Animator Animations;

		// Token: 0x040000A1 RID: 161
		public bool gunGripHeldLeft;

		// Token: 0x040000A2 RID: 162
		public bool gunGripHeldRight;

		// Token: 0x040000A3 RID: 163
		public bool slideGripHeldRight;

		// Token: 0x040000A4 RID: 164
		public bool slideGripHeldLeft;

		// Token: 0x040000A5 RID: 165
		public bool isFiring;

		// Token: 0x040000A6 RID: 166
		private bool triggerPressed;

		// Token: 0x040000A7 RID: 167
		private bool isRacked = true;

		// Token: 0x040000A8 RID: 168
		private bool isPulledBack;

		// Token: 0x040000A9 RID: 169
		private bool chamberRoundOnNext;

		// Token: 0x040000AA RID: 170
		private bool roundChambered;

		// Token: 0x040000AB RID: 171
		private bool roundSpent;

		// Token: 0x040000AC RID: 172
		private bool playSoundOnNext;

		// Token: 0x040000AD RID: 173
		private bool useRaycast;

		// Token: 0x040000AE RID: 174
		private float rayCastMaxDist;

		// Token: 0x040000AF RID: 175
		private float raycastForce;

		// Token: 0x040000B0 RID: 176
		private int currentReceiverAmmo;

		// Token: 0x040000B1 RID: 177
		private float num1;

		// Token: 0x040000B2 RID: 178
		private float num2;

		// Token: 0x040000B3 RID: 179
		private float num3;

		// Token: 0x040000B4 RID: 180
		private float num4;
	}
}
