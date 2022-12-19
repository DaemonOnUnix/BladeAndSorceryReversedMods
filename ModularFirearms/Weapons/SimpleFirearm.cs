using System;
using System.Collections;
using System.Collections.Generic;
using ModularFirearms.Shared;
using ThunderRoad;
using UnityEngine;

namespace ModularFirearms.Weapons
{
	// Token: 0x02000010 RID: 16
	public class SimpleFirearm : MonoBehaviour
	{
		// Token: 0x06000084 RID: 132 RVA: 0x00006750 File Offset: 0x00004950
		public void Awake()
		{
			this.item = base.GetComponent<Item>();
			this.module = this.item.data.GetModule<FirearmModule>();
			this.useRaycast = this.module.useHitscan || FrameworkSettings.local.useHitscan;
			this.rayCastMaxDist = (this.module.useHitscan ? this.module.hitscanMaxDistance : FrameworkSettings.local.hitscanMaxDistance);
			if (this.rayCastMaxDist <= 0f)
			{
				this.rayCastMaxDist = float.PositiveInfinity;
			}
			try
			{
				if (!string.IsNullOrEmpty(this.module.muzzlePositionRef))
				{
					this.muzzlePoint = this.item.GetCustomReference(this.module.muzzlePositionRef, true);
				}
				else
				{
					this.muzzlePoint = this.item.transform;
				}
			}
			catch
			{
				Debug.LogError(string.Format("[SimpleFirearmsFramework] Exception: '\"muzzlePositionRef\": \"{0}\"' was set in JSON, but \"{0}\" is not present on the Unity Prefab.", this.module.muzzlePositionRef));
				this.muzzlePoint = this.item.transform;
			}
			if (!string.IsNullOrEmpty(this.module.fireSoundRef))
			{
				this.fireSound = this.item.GetCustomReference(this.module.fireSoundRef, true).GetComponent<AudioSource>();
			}
			else
			{
				Debug.LogError(string.Format("[SimpleFirearmsFramework] Exception: '\"fireSoundRef\": \"{0}\"' was set in JSON, but \"{0}\" is not present on the Unity Prefab.", this.module.fireSoundRef));
			}
			if (!string.IsNullOrEmpty(this.module.emptySoundRef))
			{
				this.emptySound = this.item.GetCustomReference(this.module.emptySoundRef, true).GetComponent<AudioSource>();
			}
			else
			{
				Debug.LogError(string.Format("[SimpleFirearmsFramework] Exception: '\"emptySoundRef\": \"{0}\"' was set in JSON, but \"{0}\" is not present on the Unity Prefab.", this.module.emptySoundRef));
			}
			if (!string.IsNullOrEmpty(this.module.reloadSoundRef))
			{
				this.reloadSound = this.item.GetCustomReference(this.module.reloadSoundRef, true).GetComponent<AudioSource>();
			}
			else
			{
				Debug.LogError(string.Format("[SimpleFirearmsFramework] Exception: '\"reloadSoundRef\": \"{0}\"' was set in JSON, but \"{0}\" is not present on the Unity Prefab.", this.module.reloadSoundRef));
			}
			if (!string.IsNullOrEmpty(this.module.swtichSoundRef))
			{
				this.switchSound = this.item.GetCustomReference(this.module.swtichSoundRef, true).GetComponent<AudioSource>();
			}
			else
			{
				Debug.LogError(string.Format("[SimpleFirearmsFramework] Exception: '\"swtichSoundRef\": \"{0}\"' was set in JSON, but \"{0}\" is not present on the Unity Prefab.", this.module.swtichSoundRef));
			}
			if (!string.IsNullOrEmpty(this.module.npcRaycastPositionRef))
			{
				this.npcRayCastPoint = this.item.GetCustomReference(this.module.npcRaycastPositionRef, true);
			}
			else
			{
				Debug.LogError(string.Format("[SimpleFirearmsFramework] Exception: '\"npcRaycastPositionRef\": \"{0}\"' was set in JSON, but \"{0}\" is not present on the Unity Prefab.", this.module.npcRaycastPositionRef));
			}
			if (!string.IsNullOrEmpty(this.module.muzzleFlashRef))
			{
				this.MuzzleFlash = this.item.GetCustomReference(this.module.muzzleFlashRef, true).GetComponent<ParticleSystem>();
			}
			else
			{
				Debug.LogError(string.Format("[SimpleFirearmsFramework] Exception: '\"muzzleFlashRef\": \"{0}\"' was set in JSON, but \"{0}\" is not present on the Unity Prefab.", this.module.muzzleFlashRef));
			}
			if (!string.IsNullOrEmpty(this.module.animatorRef))
			{
				this.Animations = this.item.GetCustomReference(this.module.animatorRef, true).GetComponent<Animator>();
			}
			else
			{
				Debug.LogError(string.Format("[SimpleFirearmsFramework] Exception: '\"animatorRef\": \"{0}\"' was set in JSON, but \"{0}\" is not present on the Unity Prefab.", this.module.animatorRef));
			}
			if (!string.IsNullOrEmpty(this.module.earlyFireSoundRef))
			{
				this.earlyFireSound = this.item.GetCustomReference(this.module.earlyFireSoundRef, true).GetComponent<AudioSource>();
			}
			else
			{
				Debug.LogError(string.Format("[SimpleFirearmsFramework] Exception: '\"secondaryFireSound\": \"{0}\"' was set in JSON, but \"{0}\" is not present on the Unity Prefab.", this.module.earlyFireSoundRef));
			}
			if (!string.IsNullOrEmpty(this.module.earlyMuzzleFlashRef))
			{
				this.earlyMuzzleFlash = this.item.GetCustomReference(this.module.earlyMuzzleFlashRef, true).GetComponent<ParticleSystem>();
			}
			else
			{
				Debug.LogError(string.Format("[SimpleFirearmsFramework] Exception: '\"secondaryMuzzleFlashRef\": \"{0}\"' was set in JSON, but \"{0}\" is not present on the Unity Prefab.", this.module.earlyMuzzleFlashRef));
			}
			if (this.npcRayCastPoint == null)
			{
				this.npcRayCastPoint = this.muzzlePoint;
			}
			if (this.module.ammoCapacity > 0)
			{
				this.remaingingAmmo = this.module.ammoCapacity;
			}
			else
			{
				this.infAmmo = true;
			}
			if (this.module.soundVolume > 0f && this.module.soundVolume <= 1f && this.fireSound != null)
			{
				this.fireSound.volume = this.module.soundVolume;
			}
			if (this.module.loopedFireSound)
			{
				this.fireSound.loop = true;
			}
			this.fireModeSelection = (FrameworkCore.FireMode)Enum.Parse(typeof(FrameworkCore.FireMode), this.module.fireMode);
			if (this.module.allowedFireModes != null)
			{
				this.allowedFireModes = new List<int>(this.module.allowedFireModes);
			}
			this.item.OnHeldActionEvent += new Item.HeldActionDelegate(this.OnHeldAction);
			if (!string.IsNullOrEmpty(this.module.mainGripID))
			{
				this.gunGrip = this.item.GetCustomReference(this.module.mainGripID, true).GetComponent<Handle>();
			}
			if (this.gunGrip == null)
			{
				this.gunGrip = this.item.transform.Find("Handle").GetComponent<Handle>();
				if (this.gunGrip == null)
				{
					this.gunGrip = this.item.GetComponentInChildren<Handle>();
				}
			}
			if (this.gunGrip != null)
			{
				this.gunGrip.Grabbed += new Handle.GrabEvent(this.OnMainGripGrabbed);
				this.gunGrip.UnGrabbed += new Handle.GrabEvent(this.OnMainGripUnGrabbed);
			}
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00006CF0 File Offset: 0x00004EF0
		public void OnHeldAction(RagdollHand interactor, Handle handle, Interactable.Action action)
		{
			if (handle.Equals(this.gunGrip))
			{
				if (action == null)
				{
					if (this.module.waitForReloadAnim && FrameworkCore.IsAnimationPlaying(this.Animations, this.module.reloadAnim))
					{
						return;
					}
					this.triggerPressed = true;
					if (this.module.isFlintlock)
					{
						base.StartCoroutine(this.FlintlockLinkedFire());
					}
					else if (!this.isFiring)
					{
						base.StartCoroutine(FrameworkCore.GeneralFire(new TrackFiredDelegate(this.TrackedFire), new TriggerPressedDelegate(this.TriggerIsPressed), this.fireModeSelection, this.module.fireRate, this.module.burstNumber, this.emptySound, new IsFiringDelegate(this.SetFiringFlag), new IsSpawningDelegate(this.ProjectileIsSpawning)));
					}
				}
				if (action == 1 || action == 5)
				{
					this.triggerPressed = false;
				}
				if (action == 2)
				{
					if (this.module.waitForReloadAnim && FrameworkCore.IsAnimationPlaying(this.Animations, this.module.reloadAnim))
					{
						return;
					}
					if (this.module.allowCycleFireMode && !this.isEmpty)
					{
						if (this.emptySound != null)
						{
							this.emptySound.Play();
						}
						this.fireModeSelection = FrameworkCore.CycleFireMode(this.fireModeSelection, this.allowedFireModes);
						return;
					}
					if (this.isEmpty)
					{
						this.ReloadWeapon();
					}
				}
			}
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00006E50 File Offset: 0x00005050
		public void OnMainGripGrabbed(RagdollHand interactor, Handle handle, EventTime eventTime)
		{
			if (interactor.playerHand == Player.local.handRight)
			{
				this.gunGripHeldRight = true;
			}
			if (interactor.playerHand == Player.local.handLeft)
			{
				this.gunGripHeldLeft = true;
			}
			if (!this.gunGripHeldLeft && !this.gunGripHeldRight)
			{
				if (this.isEmpty)
				{
					this.ReloadWeapon();
				}
				this.thisNPC = interactor.ragdoll.creature;
				this.thisNPCBrain = this.thisNPC.brain.instance;
				this.BrainBow = this.thisNPCBrain.GetModule<BrainModuleBow>(true);
				this.BrainMelee = this.thisNPCBrain.GetModule<BrainModuleMelee>(true);
				this.BrainParry = this.thisNPCBrain.GetModule<BrainModuleDefense>(true);
				this.thisNPC.brain.currentTarget = Player.local.creature;
				this.thisNPC.brain.isDefending = true;
				this.BrainMelee.meleeEnabled = this.module.npcMeleeEnableOverride;
			}
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00006F5C File Offset: 0x0000515C
		public void OnMainGripUnGrabbed(RagdollHand interactor, Handle handle, EventTime eventTime)
		{
			if (interactor.playerHand == Player.local.handRight)
			{
				this.gunGripHeldRight = false;
			}
			if (interactor.playerHand == Player.local.handLeft)
			{
				this.gunGripHeldLeft = false;
			}
			if (this.thisNPC != null)
			{
				this.thisNPC = null;
				this.thisNPCBrain = null;
				this.BrainBow = null;
				this.BrainMelee = null;
				this.BrainParry = null;
			}
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00006FD8 File Offset: 0x000051D8
		public void LateUpdate()
		{
			if (this.module.loopedFireSound)
			{
				bool flag = this.TriggerIsPressed();
				if (flag && !this.fireSound.isPlaying)
				{
					this.fireSound.Play();
				}
				else if ((!flag && this.fireSound.isPlaying) || this.isEmpty)
				{
					this.fireSound.Stop();
				}
			}
			if (this.BrainParry != null && this.thisNPC.brain.currentTarget != null)
			{
				this.BrainParry.StartDefense();
				if (!this.module.npcMeleeEnableOverride)
				{
					this.BrainMelee.meleeEnabled = Vector3.Distance(this.item.rb.position, this.thisNPC.brain.currentTarget.transform.position) <= this.module.npcMeleeEnableDistance;
				}
			}
			if (this.npcShootDelay > 0f)
			{
				this.npcShootDelay -= Time.deltaTime;
			}
			if (this.npcShootDelay <= 0f)
			{
				this.NPCFire();
			}
		}

		// Token: 0x06000089 RID: 137 RVA: 0x000070F0 File Offset: 0x000052F0
		private void ReloadWeapon()
		{
			if (this.module.waitForReloadAnim && FrameworkCore.IsAnimationPlaying(this.Animations, this.module.reloadAnim))
			{
				return;
			}
			if (this.reloadSound != null)
			{
				this.reloadSound.Play();
			}
			FrameworkCore.Animate(this.Animations, this.module.reloadAnim);
			this.remaingingAmmo = this.module.ammoCapacity;
			this.isEmpty = false;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x0000716B File Offset: 0x0000536B
		public void SetFiringFlag(bool status)
		{
			this.isFiring = status;
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00007174 File Offset: 0x00005374
		private void NPCFire()
		{
			if (this.thisNPC != null && this.thisNPCBrain != null && this.thisNPC.brain.currentTarget != null)
			{
				Vector3 vector = FrameworkCore.NpcAimingAngle(this.BrainBow, this.npcRayCastPoint.TransformDirection(Vector3.forward), this.module.npcDistanceToFire);
				RaycastHit raycastHit;
				if (Physics.Raycast(this.npcRayCastPoint.position, vector, ref raycastHit, this.module.npcDetectionRadius))
				{
					Creature creature = null;
					if (raycastHit.collider.transform.root.name.Contains("Player") || raycastHit.collider.transform.root.name.Contains("Pool_Human"))
					{
						creature = raycastHit.collider.transform.root.GetComponentInChildren<Creature>();
					}
					if (creature != null && this.thisNPC != creature && this.thisNPC.faction.attackBehaviour != 1 && this.thisNPC.faction.attackBehaviour != 2 && creature.faction.attackBehaviour != 1 && (this.thisNPC.faction.attackBehaviour == 3 || this.thisNPC.factionId != creature.factionId))
					{
						this.Fire(true, true);
						FrameworkCore.DamageCreatureCustom(creature, this.module.npcDamageToPlayer, raycastHit.point);
						this.npcShootDelay = Random.Range(this.BrainBow.minMaxTimeBetweenAttack.x, this.BrainBow.minMaxTimeBetweenAttack.y);
					}
				}
			}
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00007324 File Offset: 0x00005524
		public bool TriggerIsPressed()
		{
			return this.triggerPressed;
		}

		// Token: 0x0600008D RID: 141 RVA: 0x0000732C File Offset: 0x0000552C
		public bool ProjectileIsSpawning()
		{
			return this.currentlySpawningProjectile;
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00007334 File Offset: 0x00005534
		public void PreFireEffects()
		{
			if (this.MuzzleFlash != null)
			{
				this.MuzzleFlash.Play();
			}
			if (this.remaingingAmmo == 1)
			{
				FrameworkCore.Animate(this.Animations, this.module.emptyAnim);
				this.isEmpty = true;
			}
			else
			{
				FrameworkCore.Animate(this.Animations, this.module.fireAnim);
			}
			if (!this.module.loopedFireSound && this.fireSound != null)
			{
				this.fireSound.Play();
			}
		}

		// Token: 0x0600008F RID: 143 RVA: 0x000073C0 File Offset: 0x000055C0
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
			if (!this.useRaycast || !FrameworkCore.ShootRaycastDamage(this.muzzlePoint, this.module.bulletForce, this.rayCastMaxDist, 1f))
			{
				FrameworkCore.ShootProjectile(this.item, this.module.projectileID, this.muzzlePoint, FrameworkCore.GetItemSpellChargeID(this.item), this.module.bulletForce, this.module.throwMult, false, null, null);
			}
			FrameworkCore.ApplyRecoil(this.item.rb, this.module.recoilForces, this.module.throwMult, this.gunGripHeldLeft, this.gunGripHeldRight, this.module.hapticForce, null);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00007484 File Offset: 0x00005684
		public bool TrackedFlintlockFire()
		{
			if (this.isEmpty)
			{
				return false;
			}
			if (this.infAmmo || this.remaingingAmmo > 0)
			{
				if (this.MuzzleFlash != null)
				{
					this.MuzzleFlash.Play();
				}
				if (this.fireSound != null)
				{
					this.fireSound.Play();
				}
				if (this.remaingingAmmo == 1)
				{
					this.isEmpty = true;
				}
				this.Fire(false, false);
				this.remaingingAmmo--;
				return true;
			}
			return false;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00007507 File Offset: 0x00005707
		public IEnumerator FlintlockLinkedFire()
		{
			if (!this.isEmpty && (this.infAmmo || this.remaingingAmmo > 0))
			{
				FrameworkCore.Animate(this.Animations, this.module.fireAnim);
				if (this.module.waitForFireAnim)
				{
					do
					{
						yield return null;
					}
					while (FrameworkCore.IsAnimationPlaying(this.Animations, this.module.fireAnim));
				}
				if (this.remaingingAmmo == 1)
				{
					this.isEmpty = true;
				}
				if (this.earlyMuzzleFlash != null)
				{
					this.earlyMuzzleFlash.Play();
				}
				if (this.earlyFireSound != null)
				{
					this.earlyFireSound.Play();
				}
				yield return new WaitForSeconds(this.module.flintlockDelay);
				this.Fire(false, true);
				this.remaingingAmmo--;
			}
			else
			{
				if (this.emptySound != null)
				{
					this.emptySound.Play();
				}
				yield return null;
			}
			yield return null;
			yield break;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00007516 File Offset: 0x00005716
		private IEnumerator FlintlockFireDelay(bool waitForFireAnim, float secondaryDelay)
		{
			yield return new WaitForSeconds(secondaryDelay);
			yield break;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00007525 File Offset: 0x00005725
		public bool TrackedFire()
		{
			if (this.isEmpty)
			{
				return false;
			}
			if (this.infAmmo || this.remaingingAmmo > 0)
			{
				this.Fire(false, true);
				this.remaingingAmmo--;
				return true;
			}
			return false;
		}

		// Token: 0x040000B5 RID: 181
		protected Item item;

		// Token: 0x040000B6 RID: 182
		protected FirearmModule module;

		// Token: 0x040000B7 RID: 183
		private Handle gunGrip;

		// Token: 0x040000B8 RID: 184
		private Animator Animations;

		// Token: 0x040000B9 RID: 185
		private Transform muzzlePoint;

		// Token: 0x040000BA RID: 186
		private Transform npcRayCastPoint;

		// Token: 0x040000BB RID: 187
		private ParticleSystem MuzzleFlash;

		// Token: 0x040000BC RID: 188
		private ParticleSystem earlyMuzzleFlash;

		// Token: 0x040000BD RID: 189
		private AudioSource fireSound;

		// Token: 0x040000BE RID: 190
		private AudioSource emptySound;

		// Token: 0x040000BF RID: 191
		private AudioSource switchSound;

		// Token: 0x040000C0 RID: 192
		private AudioSource reloadSound;

		// Token: 0x040000C1 RID: 193
		private AudioSource earlyFireSound;

		// Token: 0x040000C2 RID: 194
		private FrameworkCore.FireMode fireModeSelection;

		// Token: 0x040000C3 RID: 195
		private List<int> allowedFireModes;

		// Token: 0x040000C4 RID: 196
		private int remaingingAmmo;

		// Token: 0x040000C5 RID: 197
		private bool infAmmo;

		// Token: 0x040000C6 RID: 198
		private bool isEmpty;

		// Token: 0x040000C7 RID: 199
		private bool triggerPressed;

		// Token: 0x040000C8 RID: 200
		private bool gunGripHeldLeft;

		// Token: 0x040000C9 RID: 201
		private bool gunGripHeldRight;

		// Token: 0x040000CA RID: 202
		public bool isFiring;

		// Token: 0x040000CB RID: 203
		public bool currentlySpawningProjectile;

		// Token: 0x040000CC RID: 204
		private bool useRaycast;

		// Token: 0x040000CD RID: 205
		private float rayCastMaxDist;

		// Token: 0x040000CE RID: 206
		private Creature thisNPC;

		// Token: 0x040000CF RID: 207
		private BrainData thisNPCBrain;

		// Token: 0x040000D0 RID: 208
		private BrainModuleBow BrainBow;

		// Token: 0x040000D1 RID: 209
		private BrainModuleMelee BrainMelee;

		// Token: 0x040000D2 RID: 210
		private BrainModuleDefense BrainParry;

		// Token: 0x040000D3 RID: 211
		private float npcShootDelay;
	}
}
