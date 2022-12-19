using System;
using ThunderRoad;
using UnityEngine;
using Wully.Utils;

namespace Wully.Mono
{
	// Token: 0x02000009 RID: 9
	public class Blink : MonoBehaviour
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00002F17 File Offset: 0x00001117
		public bool IsTargeting
		{
			get
			{
				return this.targeting;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000030 RID: 48 RVA: 0x00002F1F File Offset: 0x0000111F
		public bool IsTeleporting
		{
			get
			{
				return this.teleporting;
			}
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002F27 File Offset: 0x00001127
		public void Load()
		{
			this.SpellLoaded = true;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002F30 File Offset: 0x00001130
		public void Unload()
		{
			this.isBlinkStartPlaying = false;
			this.isBlinkTravelPlaying = false;
			this.targeting = false;
			this.heightControl = false;
			this.validTarget = false;
			this.invalidTarget = false;
			this.teleporting = false;
			this.canTeleport = false;
			EffectInstance effectInstance = this.blinkTravel;
			if (effectInstance != null)
			{
				effectInstance.Stop(0);
			}
			EffectInstance effectInstance2 = this.blinkStart;
			if (effectInstance2 != null)
			{
				effectInstance2.Stop(0);
			}
			EffectInstance effectInstance3 = this.blinkLoop;
			if (effectInstance3 != null)
			{
				effectInstance3.Stop(0);
			}
			EffectInstance effectInstance4 = this.teleportEffect;
			if (effectInstance4 != null)
			{
				effectInstance4.Stop(0);
			}
			this.StopTimeFreeze();
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002FC4 File Offset: 0x000011C4
		private void Start()
		{
			PlayerCollisionEvents.OnCollideWithWorldEvent += this.PlayerCollisionEvents_OnCollideWithWorldEvent;
			EffectInstance effectInstance = this.groundMarker;
			if (effectInstance != null)
			{
				effectInstance.Stop(0);
			}
			EffectInstance effectInstance2 = this.targetMarker;
			if (effectInstance2 != null)
			{
				effectInstance2.Stop(0);
			}
			EffectInstance effectInstance3 = this.climbMarker;
			if (effectInstance3 != null)
			{
				effectInstance3.Stop(0);
			}
			this.sphereCastDiameter = this.sphereCastRadius * 2f;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x0000302C File Offset: 0x0000122C
		private void MakeStealthy(bool canHear)
		{
			for (int i = 0; i < Creature.allActive.Count; i++)
			{
				Creature.allActive[i].brain.instance.GetModule<BrainModuleDetection>(true).canHear = canHear;
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00003070 File Offset: 0x00001270
		private void PlayerCollisionEvents_OnCollideWithWorldEvent()
		{
			if (!this.validTarget && this.teleporting)
			{
				this.FullStop();
				this.teleporting = false;
				if (this.isBlinkTravelPlaying)
				{
					this.isBlinkTravelPlaying = false;
				}
				EffectInstance effectInstance = this.blinkStart;
				if (effectInstance != null)
				{
					effectInstance.Stop(0);
				}
				this.isBlinkStartPlaying = false;
				GameManager.SetPlayerFallDamage(this.previousFallDamageSetting);
				this.MakeStealthy(false);
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000030D4 File Offset: 0x000012D4
		private void Update()
		{
			if (this.SpellLoaded && this.climbMarker != null && this.targetMarker != null && this.groundMarker != null && Player.local != null)
			{
				this.CheckCanTeleport();
				this.InputSystem();
				this.Targeting();
				return;
			}
			this.targeting = false;
			this.validTarget = false;
			this.invalidTarget = false;
			this.teleporting = false;
			this.canTeleport = false;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00003144 File Offset: 0x00001344
		private void FixedUpdate()
		{
			if (this.SpellLoaded)
			{
				if (this.canTeleport && this.validTarget)
				{
					this.teleporting = true;
					if (!this.isBlinkTravelPlaying && this.blinkTravelData != null)
					{
						this.blinkTravel = this.blinkTravelData.Spawn(this.spellCaster.magic, true, null, false, Array.Empty<Type>());
						this.blinkTravel.Play(0, false);
						if (Player.local)
						{
							RagdollPart part = Extensions.GetRagdollPartByName("Spine1");
							if (part)
							{
								if (this.teleportData != null)
								{
									this.teleportEffect = this.teleportData.Spawn(part.transform.position, part.transform.rotation, Level.current.transform, null, true, null, false, Array.Empty<Type>());
								}
								this.teleportEffect.Play(0, false);
							}
						}
						this.isBlinkTravelPlaying = true;
					}
					this.previousFallDamageSetting = Player.fallDamage;
					GameManager.SetPlayerFallDamage(false);
					this.MakeStealthy(true);
				}
				if (this.emilyStyle)
				{
					this.EmilyTeleport();
					return;
				}
				this.CorvoTeleport();
			}
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00003262 File Offset: 0x00001462
		private bool IsOtherSideTargeting()
		{
			if (this.side == null)
			{
				if (Blink.localLeft != null)
				{
					return Blink.localLeft.IsTargeting;
				}
			}
			else if (Blink.localRight != null)
			{
				return Blink.localRight.IsTargeting;
			}
			return false;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x0000329D File Offset: 0x0000149D
		private bool IsOtherSideTeleporting()
		{
			if (this.side == null && Blink.localLeft != null)
			{
				return Blink.localLeft.IsTeleporting;
			}
			return Blink.localRight != null && Blink.localRight.IsTeleporting;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000032D8 File Offset: 0x000014D8
		private void CheckCanTeleport()
		{
			if (!Player.local)
			{
				return;
			}
			if (this.IsOtherSideTeleporting())
			{
				this.canTeleport = false;
			}
			if (Player.local.locomotion.isGrounded && !this.teleporting)
			{
				this.canTeleport = true;
			}
			if (!Player.local.locomotion.isGrounded && !this.teleporting)
			{
				this.canTeleport = true;
			}
			if (!Player.local.locomotion.isGrounded && this.teleporting)
			{
				this.canTeleport = false;
			}
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00003364 File Offset: 0x00001564
		private void CorvoTeleport()
		{
			if ((!this.teleporting && !this.validTarget) || !Player.local || !Player.local.creature)
			{
				return;
			}
			Player player = Player.local;
			Vector3 playerView;
			playerView..ctor(player.head.anchor.position.x, player.transform.position.y, player.head.anchor.position.z);
			Vector3 playSpace = player.transform.position;
			Vector3 offset = playSpace - playerView;
			Vector3 teleportTarget = this.teleportPos + offset;
			if (!this.validTarget && this.teleporting)
			{
				if (Player.local.locomotion.isGrounded)
				{
					this.teleporting = false;
					if (this.isBlinkTravelPlaying)
					{
						this.isBlinkTravelPlaying = false;
					}
					EffectInstance effectInstance = this.blinkStart;
					if (effectInstance != null)
					{
						effectInstance.Stop(0);
					}
					this.isBlinkStartPlaying = false;
					GameManager.SetPlayerFallDamage(this.previousFallDamageSetting);
					this.MakeStealthy(false);
				}
				return;
			}
			if ((this.teleportPos - playerView).magnitude < this.sphereCastRadius)
			{
				if (this.validTarget)
				{
					this.validTarget = false;
					return;
				}
			}
			else
			{
				Vector3 target = Vector3.MoveTowards(playSpace, teleportTarget, this.teleportSpeed * Time.deltaTime);
				this.TeleportPlayer(player, target);
			}
		}

		// Token: 0x0600003C RID: 60 RVA: 0x000034B8 File Offset: 0x000016B8
		private void TeleportPlayer(Player player, Vector3 target)
		{
			Creature creature = player.creature;
			Vector3 position = Vector3.zero;
			Quaternion localRotation = Quaternion.identity;
			if (creature.handLeft.grabbedHandle && creature.handLeft.grabbedHandle.item)
			{
				position = creature.transform.InverseTransformPoint(creature.handLeft.grabbedHandle.item.transform.position);
				localRotation = Common.InverseTransformRotation(creature.transform, creature.handLeft.grabbedHandle.item.transform.rotation);
			}
			Vector3 position2 = Vector3.zero;
			Quaternion localRotation2 = Quaternion.identity;
			if (creature.handRight.grabbedHandle && creature.handRight.grabbedHandle.item)
			{
				position2 = creature.transform.InverseTransformPoint(creature.handRight.grabbedHandle.item.transform.position);
				localRotation2 = Common.InverseTransformRotation(creature.transform, creature.handRight.grabbedHandle.item.transform.rotation);
			}
			creature.transform.position = target;
			creature.locomotion.prevPosition = target;
			if (creature.handLeft.grabbedHandle && creature.handLeft.grabbedHandle.item)
			{
				creature.handLeft.grabbedHandle.item.transform.position = creature.transform.TransformPoint(position);
				creature.handLeft.grabbedHandle.item.transform.rotation = Common.TransformRotation(creature.transform, localRotation);
			}
			if (creature.handRight.grabbedHandle && creature.handRight.grabbedHandle.item)
			{
				creature.handRight.grabbedHandle.item.transform.position = creature.transform.TransformPoint(position2);
				creature.handRight.grabbedHandle.item.transform.rotation = Common.TransformRotation(creature.transform, localRotation2);
			}
			player.locomotion.rb.MovePosition(target);
			player.locomotion.prevPosition = target;
			player.transform.position = target;
			Physics.SyncTransforms();
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00003704 File Offset: 0x00001904
		private void EmilyTeleport()
		{
			if (!this.teleporting)
			{
				return;
			}
			Vector3 dir = this.teleportPos - Player.local.locomotion.rb.position;
			if (dir.x < 0.5f && dir.y < 0.5f && dir.z < 0.5f)
			{
				this.teleporting = false;
				this.canTeleport = false;
				Player.local.locomotion.rb.velocity = this.oldVelocity;
			}
			dir /= Time.fixedDeltaTime;
			dir = Vector3.ClampMagnitude(dir, this.teleportSpeed);
			Player.local.locomotion.rb.velocity = dir;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x000037B8 File Offset: 0x000019B8
		private void OnDrawGizmos()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawSphere(this.targetHitPosition, 0.25f);
			Gizmos.DrawSphere(this.targetRayHitPos, 0.5f);
			Gizmos.DrawLine(this.targetRayHitPos + this.sphereCastDiameter * Vector3.up, this.targetRayHitPos + this.sphereCastDiameter * Vector3.up + Vector3.down * this.range);
			Gizmos.color = Color.blue;
			Gizmos.DrawSphere(this.groundHitPosition, 0.25f);
			Gizmos.DrawLine(base.transform.position, this.targetRayHitPos);
			Gizmos.DrawSphere(this.groundRayHitPos, 0.5f);
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(this.ledgeRayHitPos, 0.5f);
			Gizmos.color = Color.green;
			Gizmos.DrawSphere(this.teleportPos, 0.5f);
		}

		// Token: 0x0600003F RID: 63 RVA: 0x000038B3 File Offset: 0x00001AB3
		private void StartTimeFreeze()
		{
			Player.local.locomotion.rb.isKinematic = true;
			this.FullStop();
		}

		// Token: 0x06000040 RID: 64 RVA: 0x000038D0 File Offset: 0x00001AD0
		private void StopTimeFreeze()
		{
			if (Player.local.locomotion.rb.isKinematic)
			{
				Player.local.locomotion.rb.isKinematic = false;
			}
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00003900 File Offset: 0x00001B00
		private void Targeting()
		{
			if (!Player.local)
			{
				return;
			}
			if (this.targeting)
			{
				if (!Player.local.locomotion.isGrounded && !Player.local.locomotion.isJumping)
				{
					this.StartTimeFreeze();
				}
				if (!this.isBlinkStartPlaying)
				{
					if (this.blinkStartData != null)
					{
						this.blinkStart = this.blinkStartData.Spawn(this.spellCaster.magic, true, null, false, Array.Empty<Type>());
						this.blinkStart.Play(0, false);
					}
					if (this.blinkLoopData != null)
					{
						this.blinkLoop = this.blinkLoopData.Spawn(this.spellCaster.magic, true, null, false, Array.Empty<Type>());
						this.blinkLoop.Play(0, false);
					}
					this.isBlinkStartPlaying = true;
				}
				this.targetMarker.Enable();
				this.groundMarker.Enable();
				this.climbMarker.Disable();
				Vector3 origin = this.spellCaster.ragdollHand.transform.position;
				Vector3 direction = -this.spellCaster.ragdollHand.transform.right;
				bool hitLedge = false;
				float heightOffset = 0f;
				if (!this.heightControl)
				{
					RaycastHit targetHit;
					if (this.GetSphereHit(origin, this.sphereCastRadius, direction, out targetHit))
					{
						this.targetRayHitPos = targetHit.point;
						this.targetHitPosition = targetHit.point + -direction * this.sphereCastDiameter;
					}
					else
					{
						this.targetHitPosition = origin + direction * this.range;
						this.targetRayHitPos = this.targetHitPosition;
					}
					RaycastHit groundHit;
					if (this.GetSphereHit(this.targetRayHitPos + this.sphereCastDiameter * Vector3.up, this.sphereCastRadius * 0.5f, Vector3.down, out groundHit))
					{
						this.groundHitPosition = groundHit.point;
						this.groundRayHitPos = groundHit.point;
						this.climbMarker.SetRotation(Quaternion.LookRotation((Player.local.locomotion.rb.position - groundHit.point).normalized, Vector3.up));
					}
					else
					{
						this.groundHitPosition = this.targetHitPosition + Vector3.down * this.range;
						this.groundRayHitPos = this.groundHitPosition;
					}
					if (this.groundHitPosition.y > this.targetHitPosition.y)
					{
						hitLedge = true;
						this.targetMarker.Disable();
						this.climbMarker.Enable();
						this.teleportPos = this.groundRayHitPos + (this.groundRayHitPos - new Vector3(Player.local.locomotion.rb.position.x, this.groundHitPosition.y, Player.local.locomotion.rb.position.z)).normalized * 0.25f;
					}
					else
					{
						if (Mathf.Abs(this.targetRayHitPos.y - this.groundRayHitPos.y) < 0.01f)
						{
							this.teleportPos = this.groundRayHitPos;
							this.targetHitPosition.x = this.targetRayHitPos.x;
							this.targetHitPosition.z = this.targetRayHitPos.z;
						}
						else
						{
							this.teleportPos = this.targetHitPosition;
						}
						this.groundHitPosition.x = this.targetHitPosition.x;
						this.groundHitPosition.z = this.targetHitPosition.z;
					}
					this.tmpTelepos = this.teleportPos;
				}
				else
				{
					Ray pointRay;
					pointRay..ctor(origin, direction);
					float enter;
					if (new Plane((origin - this.groundHitPosition).normalized, this.groundHitPosition).Raycast(pointRay, ref enter))
					{
						heightOffset = pointRay.GetPoint(enter).y;
					}
					RaycastHit hit;
					float maxHeight;
					if (Physics.SphereCast(this.groundHitPosition, this.sphereCastRadius * 0.5f, Vector3.up, ref hit, this.range, 32769))
					{
						maxHeight = hit.point.y;
					}
					else
					{
						maxHeight = this.range;
					}
					heightOffset = Mathf.Clamp(heightOffset - this.groundHitPosition.y, 0f, maxHeight);
				}
				Vector3 target = this.targetHitPosition;
				Vector3 ground = this.groundHitPosition;
				Vector3 teleportTarget = this.tmpTelepos;
				if (hitLedge)
				{
					teleportTarget += this.ledgeOffset;
				}
				target.y += heightOffset;
				teleportTarget.y += heightOffset;
				this.teleportPos = teleportTarget;
				float playerHeight = Player.local.head.transform.position.y - Player.local.footLeft.transform.position.y;
				if (Physics.Raycast(this.teleportPos, Vector3.up, playerHeight, 32769, 1))
				{
					this.invalidTarget = true;
					this.targetMarker.Disable();
					this.groundMarker.Disable();
					this.climbMarker.Disable();
				}
				else
				{
					this.invalidTarget = false;
				}
				this.targetMarker.SetPosition(target + this.targetOffset);
				this.targetMarker.SetRotation(Quaternion.identity);
				this.groundMarker.SetPosition(ground + this.markerOffset);
				this.groundMarker.SetRotation(Quaternion.identity);
				this.climbMarker.SetRotation(Quaternion.identity);
				this.climbMarker.SetPosition(ground);
				return;
			}
			this.targetMarker.Disable();
			this.groundMarker.Disable();
			this.climbMarker.Disable();
			this.StopTimeFreeze();
			EffectInstance effectInstance = this.blinkLoop;
			if (effectInstance == null)
			{
				return;
			}
			effectInstance.Stop(0);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00003EBC File Offset: 0x000020BC
		private bool GetSphereHit(Vector3 origin, float radius, Vector3 direction, out RaycastHit hit)
		{
			int layerMask = 135169;
			return Physics.SphereCast(origin, radius, direction, ref hit, this.range, layerMask);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00003EE0 File Offset: 0x000020E0
		private void InputSystem()
		{
			if (this.SpellLoaded && this.FireActive && !this.targeting && !this.teleporting && this.canTeleport)
			{
				this.targeting = true;
			}
			if (this.IsOtherSideTargeting())
			{
				this.targeting = false;
			}
			if (this.SpellLoaded && PlayerControl.GetHand(this.spellCaster.ragdollHand.side).gripPressed && !this.spellCaster.ragdollHand.grabbedHandle && this.targeting && !this.teleporting && this.canTeleport)
			{
				this.heightControl = true;
			}
			else
			{
				this.heightControl = false;
			}
			if (this.SpellLoaded && !this.FireActive && this.targeting && !this.teleporting)
			{
				this.targeting = false;
				this.heightControl = false;
				this.validTarget = !this.invalidTarget;
			}
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00003FCC File Offset: 0x000021CC
		private void FullStop()
		{
			Player.local.locomotion.rb.velocity = Vector3.zero;
			Player.local.locomotion.MoveStop();
			Player.local.locomotion.velocity = Vector3.zero;
			foreach (RagdollPart ragdollPart in Player.local.creature.ragdoll.parts)
			{
				ragdollPart.rb.velocity = Vector3.zero;
			}
		}

		// Token: 0x0400002D RID: 45
		public SpellCaster spellCaster;

		// Token: 0x0400002E RID: 46
		public EffectInstance groundMarker;

		// Token: 0x0400002F RID: 47
		public EffectInstance targetMarker;

		// Token: 0x04000030 RID: 48
		public EffectInstance climbMarker;

		// Token: 0x04000031 RID: 49
		public EffectData teleportData;

		// Token: 0x04000032 RID: 50
		public EffectData blinkStartData;

		// Token: 0x04000033 RID: 51
		public EffectData blinkTravelData;

		// Token: 0x04000034 RID: 52
		public EffectData blinkLoopData;

		// Token: 0x04000035 RID: 53
		public EffectInstance blinkStart;

		// Token: 0x04000036 RID: 54
		public EffectInstance blinkTravel;

		// Token: 0x04000037 RID: 55
		public EffectInstance blinkLoop;

		// Token: 0x04000038 RID: 56
		public EffectInstance teleportEffect;

		// Token: 0x04000039 RID: 57
		public float range = 10f;

		// Token: 0x0400003A RID: 58
		public float sphereCastRadius = 0.25f;

		// Token: 0x0400003B RID: 59
		public float sphereCastDiameter;

		// Token: 0x0400003C RID: 60
		public float teleportSpeed = 50f;

		// Token: 0x0400003D RID: 61
		public bool emilyStyle;

		// Token: 0x0400003E RID: 62
		private Vector3 oldVelocity;

		// Token: 0x0400003F RID: 63
		private Vector3 ledgeOffset = new Vector3(0f, 1f, 0f);

		// Token: 0x04000040 RID: 64
		private Vector3 targetOffset = new Vector3(0f, 0.5f, 0f);

		// Token: 0x04000041 RID: 65
		private Vector3 markerOffset = new Vector3(0f, 0.1f, 0f);

		// Token: 0x04000042 RID: 66
		[SerializeField]
		private bool targeting;

		// Token: 0x04000043 RID: 67
		private bool heightControl;

		// Token: 0x04000044 RID: 68
		[SerializeField]
		private bool teleporting;

		// Token: 0x04000045 RID: 69
		[SerializeField]
		private bool canTeleport;

		// Token: 0x04000046 RID: 70
		[SerializeField]
		private bool validTarget;

		// Token: 0x04000047 RID: 71
		private bool invalidTarget;

		// Token: 0x04000048 RID: 72
		[SerializeField]
		private Vector3 targetHitPosition = Vector3.zero;

		// Token: 0x04000049 RID: 73
		[SerializeField]
		private Vector3 groundHitPosition = Vector3.zero;

		// Token: 0x0400004A RID: 74
		[SerializeField]
		private Vector3 targetRayHitPos;

		// Token: 0x0400004B RID: 75
		[SerializeField]
		private Vector3 groundRayHitPos;

		// Token: 0x0400004C RID: 76
		[SerializeField]
		private Vector3 ledgeRayHitPos;

		// Token: 0x0400004D RID: 77
		private Vector3 teleportPos;

		// Token: 0x0400004E RID: 78
		public static Blink localLeft;

		// Token: 0x0400004F RID: 79
		public static Blink localRight;

		// Token: 0x04000050 RID: 80
		public Side side;

		// Token: 0x04000051 RID: 81
		private LineRenderer lr;

		// Token: 0x04000052 RID: 82
		private bool isBlinkTravelPlaying;

		// Token: 0x04000053 RID: 83
		private bool previousFallDamageSetting;

		// Token: 0x04000054 RID: 84
		public bool isBlinkStartPlaying;

		// Token: 0x04000055 RID: 85
		public bool FireActive;

		// Token: 0x04000056 RID: 86
		public bool SpellLoaded;

		// Token: 0x04000057 RID: 87
		private Vector3 tmpTelepos;
	}
}
