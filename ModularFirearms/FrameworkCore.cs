using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ModularFirearms.Projectiles;
using ModularFirearms.Shared;
using ThunderRoad;
using UnityEngine;

namespace ModularFirearms
{
	// Token: 0x0200000A RID: 10
	public class FrameworkCore
	{
		// Token: 0x06000032 RID: 50 RVA: 0x00002763 File Offset: 0x00000963
		public static void DisableCulling(Item item, bool cullingEnabled = false)
		{
			item.GetType().GetField("cullingDetectionEnabled", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(item, cullingEnabled);
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002784 File Offset: 0x00000984
		public static void InitializeConfigurableJoint(ref Item thisItem, ref GameObject slideObject, ref GameObject slideCenterPosition, ref ConstantForce slideForce, ref ConfigurableJoint connectedJoint, ref Rigidbody slideRB, ref SphereCollider slideCapsuleStabilizer, float stabilizerRadius, float slideTravelDistance, float slideMassOffset)
		{
			slideRB = slideObject.GetComponent<Rigidbody>();
			if (slideRB == null)
			{
				slideRB = slideObject.AddComponent<Rigidbody>();
			}
			slideRB.mass = 1f;
			slideRB.drag = 0f;
			slideRB.angularDrag = 0.05f;
			slideRB.useGravity = true;
			slideRB.isKinematic = false;
			slideRB.interpolation = 0;
			slideRB.collisionDetectionMode = 0;
			slideCapsuleStabilizer = slideCenterPosition.AddComponent<SphereCollider>();
			slideCapsuleStabilizer.radius = stabilizerRadius;
			slideCapsuleStabilizer.gameObject.layer = 21;
			Physics.IgnoreLayerCollision(21, 12);
			Physics.IgnoreLayerCollision(21, 15);
			Physics.IgnoreLayerCollision(21, 22);
			Physics.IgnoreLayerCollision(21, 23);
			slideForce = slideObject.AddComponent<ConstantForce>();
			connectedJoint = thisItem.gameObject.AddComponent<ConfigurableJoint>();
			connectedJoint.connectedBody = slideRB;
			connectedJoint.anchor = new Vector3(0f, 0f, -0.5f * slideTravelDistance);
			connectedJoint.axis = Vector3.right;
			connectedJoint.autoConfigureConnectedAnchor = false;
			connectedJoint.connectedAnchor = Vector3.zero;
			connectedJoint.secondaryAxis = Vector3.up;
			connectedJoint.xMotion = 0;
			connectedJoint.yMotion = 0;
			connectedJoint.zMotion = 1;
			connectedJoint.angularXMotion = 0;
			connectedJoint.angularYMotion = 0;
			connectedJoint.angularZMotion = 0;
			ConfigurableJoint configurableJoint = connectedJoint;
			SoftJointLimit softJointLimit = default(SoftJointLimit);
			softJointLimit.limit = 0.5f * slideTravelDistance;
			softJointLimit.bounciness = 0f;
			softJointLimit.contactDistance = 0f;
			configurableJoint.linearLimit = softJointLimit;
			connectedJoint.massScale = 1f;
			connectedJoint.connectedMassScale = slideMassOffset;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x0000293C File Offset: 0x00000B3C
		public static FrameworkCore.FireMode CycleFireMode(FrameworkCore.FireMode currentSelection, List<int> allowedFireModes = null)
		{
			int num = (int)(currentSelection + 1);
			if (allowedFireModes != null)
			{
				foreach (int num2 in Enumerable.Range(0, FrameworkCore.fireModeEnums.Length))
				{
					if (allowedFireModes.Contains(num))
					{
						return (FrameworkCore.FireMode)FrameworkCore.fireModeEnums.GetValue(num);
					}
					num++;
					if (num >= FrameworkCore.fireModeEnums.Length)
					{
						num = 0;
					}
				}
				return currentSelection;
			}
			if (num < FrameworkCore.fireModeEnums.Length)
			{
				return (FrameworkCore.FireMode)FrameworkCore.fireModeEnums.GetValue(num);
			}
			return (FrameworkCore.FireMode)FrameworkCore.fireModeEnums.GetValue(0);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000029F8 File Offset: 0x00000BF8
		public static bool Animate(Animator animator, string animationName)
		{
			if (animator == null || string.IsNullOrEmpty(animationName))
			{
				return false;
			}
			animator.Play(animationName);
			return true;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002A18 File Offset: 0x00000C18
		public static void ApplyRecoil(Rigidbody itemRB, float[] recoilForces, float recoilMult = 1f, bool leftHandHaptic = false, bool rightHandHaptic = false, float hapticForce = 1f, float[] recoilTorque = null)
		{
			if (rightHandHaptic)
			{
				PlayerControl.handRight.HapticShort(hapticForce);
			}
			if (leftHandHaptic)
			{
				PlayerControl.handLeft.HapticShort(hapticForce);
			}
			if (recoilForces == null)
			{
				return;
			}
			itemRB.AddRelativeForce(new Vector3(Random.Range(recoilForces[0], recoilForces[1]) * recoilMult, Random.Range(recoilForces[2], recoilForces[3]) * recoilMult, Random.Range(recoilForces[4], recoilForces[5]) * recoilMult));
			if (recoilTorque == null)
			{
				return;
			}
			itemRB.AddRelativeTorque(new Vector3(Random.Range(recoilTorque[0], recoilTorque[1]) * recoilMult, Random.Range(recoilTorque[2], recoilTorque[3]) * recoilMult, Random.Range(recoilTorque[4], recoilTorque[5]) * recoilMult));
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002AB8 File Offset: 0x00000CB8
		public static void FullSlice(Creature creature)
		{
			creature.ragdoll.headPart.TrySlice();
			creature.ragdoll.GetPart(128).TrySlice();
			creature.ragdoll.GetPart(256).TrySlice();
			creature.ragdoll.GetPart(16).TrySlice();
			creature.ragdoll.GetPart(8).TrySlice();
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002B28 File Offset: 0x00000D28
		public static void HitscanExplosion(Vector3 origin, float force, float blastRadius, float liftMult, ForceMode forceMode = 1, bool ignorePlayer = true)
		{
			try
			{
				foreach (Item item in Item.allActive)
				{
					if (Math.Abs(Vector3.Distance(item.transform.position, origin)) <= blastRadius)
					{
						item.rb.AddExplosionForce(force * item.rb.mass, origin, blastRadius, liftMult, forceMode);
						item.rb.AddForce(Vector3.up * liftMult * item.rb.mass, forceMode);
					}
				}
				foreach (Creature creature in Creature.allActive)
				{
					if (creature.isActiveAndEnabled && creature.ragdoll.isActiveAndEnabled && (!ignorePlayer || !(creature == Player.currentCreature)) && Math.Abs(Vector3.Distance(creature.transform.position, origin)) <= blastRadius)
					{
						if (creature.state == 2)
						{
							creature.TestKill();
						}
						FrameworkCore.FullSlice(creature);
						creature.locomotion.rb.AddExplosionForce(force * creature.locomotion.rb.mass, origin, blastRadius, liftMult, forceMode);
						creature.locomotion.rb.AddForce(Vector3.up * liftMult * creature.locomotion.rb.mass, forceMode);
						foreach (RagdollPart ragdollPart in creature.ragdoll.parts)
						{
							ragdollPart.rb.AddExplosionForce(force * ragdollPart.rb.mass, origin, blastRadius, liftMult, forceMode);
							ragdollPart.rb.AddForce(Vector3.up * liftMult * ragdollPart.rb.mass, forceMode);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("[Modular-Firearms][HitscanExplosion][EXCEPTION] " + ex.Message + " \n " + ex.StackTrace);
			}
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002DBC File Offset: 0x00000FBC
		public static void SetFireSelectionAnimator(Animator animator, FrameworkCore.FireMode selection, string paramFloat1 = "x", string paramFloat2 = "y")
		{
			if (animator == null)
			{
				return;
			}
			try
			{
				animator.SetFloat(paramFloat1, FrameworkCore.blendTreePositions[(int)selection, 0]);
				animator.SetFloat(paramFloat2, FrameworkCore.blendTreePositions[(int)selection, 1]);
			}
			catch
			{
				Debug.LogError("[FL42-FirearmFunctions][SetSwitchAnimation] Exception in setting Animator floats 'x' and 'y'");
			}
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002E1C File Offset: 0x0000101C
		public static bool IsAnimationPlaying(Animator animator, string animationName)
		{
			if (animator == null || string.IsNullOrEmpty(animationName))
			{
				return false;
			}
			bool flag;
			try
			{
				if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Contains(animationName))
				{
					flag = true;
				}
				else
				{
					flag = false;
				}
			}
			catch (Exception ex)
			{
				Debug.Log("[Fisher-Firearms] Could not check animation: " + ex.StackTrace);
				flag = false;
			}
			return flag;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002E90 File Offset: 0x00001090
		public static void IgnoreProjectile(Item shooter, Item i, bool ignore = true)
		{
			foreach (ColliderGroup colliderGroup in shooter.colliderGroups)
			{
				foreach (Collider collider in colliderGroup.colliders)
				{
					foreach (ColliderGroup colliderGroup2 in i.colliderGroups)
					{
						foreach (Collider collider2 in colliderGroup2.colliders)
						{
							Physics.IgnoreCollision(collider, collider2, ignore);
						}
					}
				}
			}
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002F9C File Offset: 0x0000119C
		public static bool ShootRaycastDamage(Transform spawnPoint, float bulletForce, float maxDistance, float damageMultiplier = 1f)
		{
			RaycastHit raycastHit;
			if (!Physics.Raycast(spawnPoint.position, spawnPoint.forward, ref raycastHit, maxDistance, FrameworkCore.raycastMask))
			{
				return false;
			}
			RagdollPart componentInChildren = raycastHit.transform.GetComponentInChildren<RagdollPart>();
			if (componentInChildren == null)
			{
				return false;
			}
			Creature componentInChildren2 = raycastHit.transform.root.GetComponentInChildren<Creature>();
			if (componentInChildren2 == null)
			{
				return false;
			}
			float num = FrameworkCore.ragdollDamageMap[componentInChildren.type] * damageMultiplier;
			ColliderGroup colliderGroup = componentInChildren.colliderGroup;
			DamageStruct damageStruct;
			damageStruct..ctor(1, num);
			damageStruct.active = true;
			damageStruct.damageType = 1;
			damageStruct.baseDamage = num;
			damageStruct.damage = num;
			damageStruct.hitRagdollPart = componentInChildren;
			damageStruct.penetration = 2;
			DamageStruct damageStruct2 = damageStruct;
			CollisionInstance collisionInstance = new CollisionInstance
			{
				active = true,
				incomplete = true,
				contactPoint = raycastHit.point,
				contactNormal = raycastHit.normal,
				sourceMaterial = FrameworkCore.sourceMaterial,
				targetMaterial = FrameworkCore.targetMaterial,
				damageStruct = damageStruct2,
				intensity = 2f,
				hasEffect = true,
				targetColliderGroup = colliderGroup,
				sourceColliderGroup = colliderGroup
			};
			Catalog.GetData<EffectData>(FrameworkCore.effectID1, true).Spawn(componentInChildren.transform, true, null, false, Array.Empty<Type>()).Play(0, false);
			Catalog.GetData<EffectData>(FrameworkCore.effectID2, true).Spawn(componentInChildren.transform, true, null, false, Array.Empty<Type>()).Play(0, false);
			Catalog.GetData<EffectData>(FrameworkCore.effectID3, true).Spawn(raycastHit.point, Quaternion.LookRotation(raycastHit.normal, Vector3.up), componentInChildren.transform, collisionInstance, true, colliderGroup, true, Array.Empty<Type>()).Play(0, false);
			Catalog.GetData<EffectData>(FrameworkCore.customEffectID, true).Spawn(raycastHit.point, Quaternion.LookRotation(raycastHit.normal, Vector3.up), componentInChildren.transform, collisionInstance, true, colliderGroup, true, Array.Empty<Type>()).Play(0, false);
			componentInChildren.rb.AddRelativeForce(spawnPoint.forward * bulletForce, 1);
			componentInChildren2.Damage(collisionInstance);
			return true;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x000031B8 File Offset: 0x000013B8
		public static void ShootProjectile(Item shooterItem, string projectileID, Transform spawnPoint, string imbueSpell = null, float forceMult = 1f, float throwMult = 1f, bool pooled = false, Collider IgnoreArg1 = null, SetSpawningStatusDelegate SetSpawnStatus = null)
		{
			FrameworkCore.<>c__DisplayClass33_0 CS$<>8__locals1 = new FrameworkCore.<>c__DisplayClass33_0();
			CS$<>8__locals1.shooterItem = shooterItem;
			CS$<>8__locals1.IgnoreArg1 = IgnoreArg1;
			CS$<>8__locals1.forceMult = forceMult;
			CS$<>8__locals1.imbueSpell = imbueSpell;
			CS$<>8__locals1.SetSpawnStatus = SetSpawnStatus;
			if (spawnPoint == null || string.IsNullOrEmpty(projectileID))
			{
				return;
			}
			ItemData data = Catalog.GetData<ItemData>(projectileID, true);
			if (data == null)
			{
				Debug.LogError("[Modular-Firearms][ERROR] No projectile named " + projectileID.ToString());
				return;
			}
			Vector3 shootLocation = new Vector3(spawnPoint.position.x, spawnPoint.position.y, spawnPoint.position.z);
			Quaternion shooterAngles = Quaternion.Euler(spawnPoint.rotation.eulerAngles);
			Vector3 shootVelocity = new Vector3(CS$<>8__locals1.shooterItem.rb.velocity.x, CS$<>8__locals1.shooterItem.rb.velocity.y, CS$<>8__locals1.shooterItem.rb.velocity.z);
			SetSpawningStatusDelegate setSpawnStatus = CS$<>8__locals1.SetSpawnStatus;
			if (setSpawnStatus != null)
			{
				setSpawnStatus(true);
			}
			data.SpawnAsync(delegate(Item i)
			{
				try
				{
					i.Throw(1f, 2);
					CS$<>8__locals1.shooterItem.IgnoreObjectCollision(i);
					i.IgnoreObjectCollision(CS$<>8__locals1.shooterItem);
					i.IgnoreRagdollCollision(Player.local.creature.ragdoll);
					if (CS$<>8__locals1.IgnoreArg1 != null)
					{
						try
						{
							i.IgnoreColliderCollision(CS$<>8__locals1.IgnoreArg1);
							foreach (ColliderGroup colliderGroup in CS$<>8__locals1.shooterItem.colliderGroups)
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
					FrameworkCore.IgnoreProjectile(CS$<>8__locals1.shooterItem, i, true);
					BasicProjectile component = i.gameObject.GetComponent<BasicProjectile>();
					if (component != null)
					{
						component.SetShooterItem(CS$<>8__locals1.shooterItem);
					}
					i.transform.position = shootLocation;
					i.transform.rotation = shooterAngles;
					i.rb.velocity = shootVelocity;
					i.rb.AddForce(i.rb.transform.forward * 1000f * CS$<>8__locals1.forceMult);
					if (!string.IsNullOrEmpty(CS$<>8__locals1.imbueSpell) && component != null)
					{
						component.AddChargeToQueue(CS$<>8__locals1.imbueSpell);
					}
					SetSpawningStatusDelegate setSpawnStatus2 = CS$<>8__locals1.SetSpawnStatus;
					if (setSpawnStatus2 != null)
					{
						setSpawnStatus2(false);
					}
				}
				catch
				{
					Debug.Log("[ModularFirearmsFramework] EXCEPTION IN SPAWNING ");
				}
			}, new Vector3?(shootLocation), new Quaternion?(Quaternion.Euler(Vector3.zero)), null, false, null);
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00003318 File Offset: 0x00001518
		public static void ShotgunBlast(Item shooterItem, string projectileID, Transform spawnPoint, float distance, float force, float forceMult, string imbueSpell = null, float throwMult = 1f, bool pooled = false, Collider IgnoreArg1 = null)
		{
			RaycastHit raycastHit;
			if (Physics.Raycast(spawnPoint.position, spawnPoint.forward, ref raycastHit, distance))
			{
				Creature componentInParent = raycastHit.collider.transform.root.GetComponentInParent<Creature>();
				if (componentInParent != null)
				{
					if (componentInParent == Player.currentCreature)
					{
						return;
					}
					componentInParent.locomotion.rb.AddExplosionForce(force, raycastHit.point, 1f, 1f, 2);
					using (List<RagdollPart>.Enumerator enumerator = componentInParent.ragdoll.parts.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							RagdollPart ragdollPart = enumerator.Current;
							ragdollPart.rb.AddExplosionForce(force, raycastHit.point, 1f, 1f, 2);
							ragdollPart.rb.AddForce(spawnPoint.forward * force, 1);
						}
						goto IL_163;
					}
				}
				try
				{
					raycastHit.collider.attachedRigidbody.AddExplosionForce(force, raycastHit.point, 0.5f, 1f, 2);
					raycastHit.collider.attachedRigidbody.AddForce(spawnPoint.forward * force, 1);
				}
				catch
				{
				}
			}
			IL_163:
			ItemData data = Catalog.GetData<ItemData>(projectileID, true);
			if (data == null)
			{
				Debug.LogError("[Modular-Firearms][ERROR] No projectile named " + projectileID.ToString());
				return;
			}
			Vector3[] array = FrameworkCore.buckshotOffsetPosiitions;
			for (int j = 0; j < array.Length; j++)
			{
				Vector3 offsetVec = array[j];
				data.SpawnAsync(delegate(Item i)
				{
					try
					{
						i.transform.position = spawnPoint.position + offsetVec;
						i.transform.rotation = Quaternion.Euler(spawnPoint.rotation.eulerAngles);
						i.rb.velocity = shooterItem.rb.velocity;
						i.rb.AddForce(i.rb.transform.forward * 1000f * forceMult);
						shooterItem.IgnoreObjectCollision(i);
						i.IgnoreObjectCollision(shooterItem);
						i.IgnoreRagdollCollision(Player.local.creature.ragdoll);
						if (IgnoreArg1 != null)
						{
							try
							{
								i.IgnoreColliderCollision(IgnoreArg1);
								foreach (ColliderGroup colliderGroup in shooterItem.colliderGroups)
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
							component.SetShooterItem(shooterItem);
						}
						if (!string.IsNullOrEmpty(imbueSpell) && component != null)
						{
							component.AddChargeToQueue(imbueSpell);
						}
					}
					catch (Exception ex)
					{
						Debug.Log("[Modular-Firearms] EXCEPTION IN SPAWNING " + ex.Message + " \n " + ex.StackTrace);
					}
				}, new Vector3?(spawnPoint.position), new Quaternion?(Quaternion.Euler(spawnPoint.rotation.eulerAngles)), null, false, null);
			}
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00003550 File Offset: 0x00001750
		public static string GetItemSpellChargeID(Item interactiveObject)
		{
			string text;
			try
			{
				foreach (Imbue imbue in interactiveObject.imbues)
				{
					if (imbue.spellCastBase != null)
					{
						return imbue.spellCastBase.id;
					}
				}
				text = null;
			}
			catch
			{
				text = null;
			}
			return text;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x000035C8 File Offset: 0x000017C8
		public static Imbue GetFirstImbue(Item imbueTarget)
		{
			try
			{
				if (imbueTarget.imbues.Count > 0)
				{
					return imbueTarget.imbues[0];
				}
			}
			catch
			{
				return null;
			}
			return null;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x0000360C File Offset: 0x0000180C
		public static Vector3 NpcAimingAngle(BrainModuleBow NPCBrain, Vector3 initial, float npcDistanceToFire = 10f)
		{
			if (NPCBrain == null)
			{
				return initial;
			}
			float num = Random.Range(NPCBrain.minMaxTimeToAttackFromAim.x, NPCBrain.minMaxTimeToAttackFromAim.y);
			float num2 = 0.2f * (num / npcDistanceToFire);
			return new Vector3(initial.x + Random.Range(-num2, num2), initial.y + Random.Range(-num2, num2), initial.z);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x0000366D File Offset: 0x0000186D
		public static IEnumerator TransferDeltaEnergy(Imbue itemImbue, SpellCastCharge activeSpell, float energyDelta = 20f, int counts = 5)
		{
			if (activeSpell != null)
			{
				int num;
				for (int i = 0; i < counts; i = num + 1)
				{
					try
					{
						itemImbue.Transfer(activeSpell, energyDelta);
					}
					catch
					{
					}
					yield return new WaitForSeconds(0.01f);
					num = i;
				}
			}
			yield return null;
			yield break;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00003694 File Offset: 0x00001894
		public static IEnumerator GeneralFire(TrackFiredDelegate TrackedFire, TriggerPressedDelegate TriggerPressed, FrameworkCore.FireMode fireSelector = FrameworkCore.FireMode.Single, int fireRate = 60, int burstNumber = 3, AudioSource emptySoundDriver = null, IsFiringDelegate WeaponIsFiring = null, IsSpawningDelegate ProjectileIsSpawning = null)
		{
			if (WeaponIsFiring != null)
			{
				WeaponIsFiring(true);
			}
			float fireDelay = 60f / (float)fireRate;
			if (fireSelector == FrameworkCore.FireMode.Safe)
			{
				if (emptySoundDriver != null)
				{
					emptySoundDriver.Play();
				}
				yield return null;
			}
			else if (fireSelector == FrameworkCore.FireMode.Single)
			{
				if (ProjectileIsSpawning != null)
				{
					do
					{
						yield return null;
					}
					while (ProjectileIsSpawning());
				}
				if (!TrackedFire())
				{
					if (emptySoundDriver != null)
					{
						emptySoundDriver.Play();
					}
					yield return null;
				}
				yield return new WaitForSeconds(fireDelay);
			}
			else if (fireSelector == FrameworkCore.FireMode.Burst)
			{
				int num;
				for (int i = 0; i < burstNumber; i = num + 1)
				{
					if (ProjectileIsSpawning != null)
					{
						do
						{
							yield return null;
						}
						while (ProjectileIsSpawning());
					}
					if (!TrackedFire())
					{
						if (emptySoundDriver != null)
						{
							emptySoundDriver.Play();
						}
						yield return null;
						break;
					}
					yield return new WaitForSeconds(fireDelay);
					num = i;
				}
				yield return null;
			}
			else if (fireSelector == FrameworkCore.FireMode.Auto)
			{
				while (TriggerPressed())
				{
					if (ProjectileIsSpawning != null)
					{
						do
						{
							yield return null;
						}
						while (ProjectileIsSpawning());
					}
					if (!TrackedFire())
					{
						if (emptySoundDriver != null)
						{
							emptySoundDriver.Play();
						}
						yield return null;
						break;
					}
					yield return new WaitForSeconds(fireDelay);
				}
			}
			if (WeaponIsFiring != null)
			{
				WeaponIsFiring(false);
			}
			yield return null;
			yield break;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x000036E4 File Offset: 0x000018E4
		public static void DamageCreatureCustom(Creature triggerCreature, float damageApplied, Vector3 hitPoint)
		{
			try
			{
				if (triggerCreature.currentHealth > 0f)
				{
					MaterialData data = Catalog.GetData<MaterialData>("Metal", true);
					MaterialData data2 = Catalog.GetData<MaterialData>("Flesh", true);
					CollisionInstance collisionInstance = new CollisionInstance(new DamageStruct(1, damageApplied), data, data2)
					{
						contactPoint = hitPoint
					};
					triggerCreature.Damage(collisionInstance);
					EffectInstance effectInstance;
					if (collisionInstance.SpawnEffect(data, data2, false, ref effectInstance))
					{
						effectInstance.Play(0, false);
					}
				}
			}
			catch
			{
				Debug.Log("[F-L42-RayCast][ERROR] Unable to damage enemy!");
			}
		}

		// Token: 0x06000045 RID: 69 RVA: 0x0000272B File Offset: 0x0000092B
		public static void DumpRigidbodyToLog(Rigidbody rb)
		{
		}

		// Token: 0x0400001A RID: 26
		public static float[,] blendTreePositions = new float[,]
		{
			{ 0f, 0f },
			{ 0f, 1f },
			{ 1f, 0f },
			{ 1f, 1f }
		};

		// Token: 0x0400001B RID: 27
		public static Vector3[] buckshotOffsetPosiitions = new Vector3[]
		{
			Vector3.zero,
			new Vector3(0.05f, 0.05f, 0f),
			new Vector3(-0.05f, -0.05f, 0f),
			new Vector3(0.05f, -0.05f, 0f),
			new Vector3(0.07f, 0.07f, 0f)
		};

		// Token: 0x0400001C RID: 28
		public static string projectileColliderReference = "BodyCollider";

		// Token: 0x0400001D RID: 29
		public static Array weaponTypeEnums = Enum.GetValues(typeof(FrameworkCore.WeaponType));

		// Token: 0x0400001E RID: 30
		public static Array ammoTypeEnums = Enum.GetValues(typeof(FrameworkCore.AmmoType));

		// Token: 0x0400001F RID: 31
		public static Array projectileTypeEnums = Enum.GetValues(typeof(FrameworkCore.ProjectileType));

		// Token: 0x04000020 RID: 32
		public static Array attachmentTypeEnums = Enum.GetValues(typeof(FrameworkCore.AttachmentType));

		// Token: 0x04000021 RID: 33
		private static readonly string effectID1 = "HitBladeOnFlesh";

		// Token: 0x04000022 RID: 34
		private static readonly string effectID2 = "PenetrationDeepFlesh";

		// Token: 0x04000023 RID: 35
		private static readonly string effectID3 = "HitBladeDecalFlesh";

		// Token: 0x04000024 RID: 36
		private static readonly string customEffectID = FrameworkSettings.local.customEffectID;

		// Token: 0x04000025 RID: 37
		private static readonly LayerMask raycastMask = 134225920;

		// Token: 0x04000026 RID: 38
		private static readonly MaterialData sourceMaterial = Catalog.GetData<MaterialData>("Metal", true);

		// Token: 0x04000027 RID: 39
		private static readonly MaterialData targetMaterial = Catalog.GetData<MaterialData>("Flesh", true);

		// Token: 0x04000028 RID: 40
		private static readonly Dictionary<RagdollPart.Type, float> ragdollDamageMap = new Dictionary<RagdollPart.Type, float>
		{
			{ 1, 300f },
			{ 2, 50f },
			{ 4, 25f },
			{ 2048, 15f },
			{ 4096, 15f },
			{ 8192, 15f },
			{ 8, 10f },
			{ 16, 10f },
			{ 128, 10f },
			{ 256, 10f },
			{ 512, 5f },
			{ 1024, 5f },
			{ 32, 5f },
			{ 64, 5f }
		};

		// Token: 0x04000029 RID: 41
		public static Array fireModeEnums = Enum.GetValues(typeof(FrameworkCore.FireMode));

		// Token: 0x0400002A RID: 42
		public static Array forceModeEnums = Enum.GetValues(typeof(ForceMode));

		// Token: 0x02000024 RID: 36
		public enum WeaponType
		{
			// Token: 0x040001DE RID: 478
			AutoMag,
			// Token: 0x040001DF RID: 479
			SemiAuto,
			// Token: 0x040001E0 RID: 480
			Shotgun,
			// Token: 0x040001E1 RID: 481
			BoltAction,
			// Token: 0x040001E2 RID: 482
			Revolver,
			// Token: 0x040001E3 RID: 483
			Sniper,
			// Token: 0x040001E4 RID: 484
			HighYield,
			// Token: 0x040001E5 RID: 485
			Energy,
			// Token: 0x040001E6 RID: 486
			TestWeapon,
			// Token: 0x040001E7 RID: 487
			SemiAutoLegacy,
			// Token: 0x040001E8 RID: 488
			SimpleFirearm
		}

		// Token: 0x02000025 RID: 37
		public enum AmmoType
		{
			// Token: 0x040001EA RID: 490
			Pouch,
			// Token: 0x040001EB RID: 491
			Magazine,
			// Token: 0x040001EC RID: 492
			AmmoLoader,
			// Token: 0x040001ED RID: 493
			SemiAuto,
			// Token: 0x040001EE RID: 494
			ShotgunShell,
			// Token: 0x040001EF RID: 495
			Revolver,
			// Token: 0x040001F0 RID: 496
			Battery,
			// Token: 0x040001F1 RID: 497
			Sniper,
			// Token: 0x040001F2 RID: 498
			Explosive,
			// Token: 0x040001F3 RID: 499
			Generic
		}

		// Token: 0x02000026 RID: 38
		public enum ProjectileType
		{
			// Token: 0x040001F5 RID: 501
			notype,
			// Token: 0x040001F6 RID: 502
			Pierce,
			// Token: 0x040001F7 RID: 503
			Explosive,
			// Token: 0x040001F8 RID: 504
			Energy,
			// Token: 0x040001F9 RID: 505
			Blunt,
			// Token: 0x040001FA RID: 506
			HitScan,
			// Token: 0x040001FB RID: 507
			Sniper
		}

		// Token: 0x02000027 RID: 39
		public enum AttachmentType
		{
			// Token: 0x040001FD RID: 509
			SecondaryFire,
			// Token: 0x040001FE RID: 510
			Flashlight,
			// Token: 0x040001FF RID: 511
			Laser,
			// Token: 0x04000200 RID: 512
			GrenadeLauncher,
			// Token: 0x04000201 RID: 513
			AmmoCounter,
			// Token: 0x04000202 RID: 514
			Compass,
			// Token: 0x04000203 RID: 515
			FireModeSwitch
		}

		// Token: 0x02000028 RID: 40
		public enum FireMode
		{
			// Token: 0x04000205 RID: 517
			Safe,
			// Token: 0x04000206 RID: 518
			Single,
			// Token: 0x04000207 RID: 519
			Burst,
			// Token: 0x04000208 RID: 520
			Auto
		}
	}
}
