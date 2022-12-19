using System;
using System.Collections;
using ThunderRoad;
using UnityEngine;

namespace Yamato
{
	// Token: 0x0200000A RID: 10
	public class MirageComponent : MonoBehaviour
	{
		// Token: 0x06000027 RID: 39 RVA: 0x00002F98 File Offset: 0x00001198
		public void Start()
		{
			this.item = base.GetComponent<Item>();
			this.item.OnUnSnapEvent += new Item.HolderDelegate(this.Item_OnUnSnapEvent);
			this.item.mainCollisionHandler.OnCollisionStartEvent += new CollisionHandler.CollisionEvent(this.MainCollisionHandler_OnCollisionStartEvent);
			this.item.OnHeldActionEvent += new Item.HeldActionDelegate(this.Item_OnHeldActionEvent);
			this.item.OnGrabEvent += new Item.GrabDelegate(this.Item_OnGrabEvent);
			this.item.OnUngrabEvent += new Item.ReleaseDelegate(this.Item_OnUngrabEvent);
			this.item.OnTelekinesisReleaseEvent += new Item.TelekinesisDelegate(this.Item_OnTelekinesisReleaseEvent);
			EffectInstance effectInstance = Catalog.GetData<EffectData>("MirageFire", true).Spawn(this.item.transform, false, null, false, Array.Empty<Type>());
			effectInstance.SetRenderer(this.item.colliderGroups[0].imbueEffectRenderer, false);
			effectInstance.SetIntensity(1f);
			effectInstance.Play(0, false);
			this.item.data.category = "Utilities";
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000030B3 File Offset: 0x000012B3
		private void Item_OnTelekinesisReleaseEvent(Handle handle, SpellTelekinesis teleGrabber)
		{
			this.lastHandler = null;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000030BD File Offset: 0x000012BD
		private void Item_OnUngrabEvent(Handle handle, RagdollHand ragdollHand, bool throwing)
		{
			this.lastHandler = ragdollHand;
			this.beam = false;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000030CE File Offset: 0x000012CE
		private void Item_OnGrabEvent(Handle handle, RagdollHand ragdollHand)
		{
			this.item.rb.useGravity = true;
			this.isThrown = false;
			this.startUpdate = false;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000030F4 File Offset: 0x000012F4
		public void Setup(float speed, string direction, bool gravity, bool collision, float time, float SwordSpeed, float BeamCooldown, float rotationSpeed, float returnSpeed, bool stop, bool start, bool thumbstick, bool swap, bool toggle)
		{
			this.DashSpeed = speed;
			this.DashDirection = direction;
			this.DisableGravity = gravity;
			this.DisableCollision = collision;
			this.DashTime = time;
			bool flag = direction.ToLower().Contains("player") || direction.ToLower().Contains("head") || direction.ToLower().Contains("sight");
			if (flag)
			{
				this.DashDirection = "Player";
			}
			else
			{
				bool flag2 = direction.ToLower().Contains("item") || direction.ToLower().Contains("sheath") || direction.ToLower().Contains("flyref") || direction.ToLower().Contains("weapon");
				if (flag2)
				{
					this.DashDirection = "Item";
				}
			}
			this.swordSpeed = SwordSpeed;
			this.cooldown = BeamCooldown;
			this.RotationSpeed = rotationSpeed;
			this.ReturnSpeed = returnSpeed;
			this.StopOnEnd = stop;
			this.StopOnStart = start;
			this.ThumbstickDash = thumbstick;
			this.SwapButtons = swap;
			this.ToggleSwordBeams = toggle;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00003214 File Offset: 0x00001414
		private void Item_OnHeldActionEvent(RagdollHand ragdollHand, Handle handle, Interactable.Action action)
		{
			bool flag = (!this.SwapButtons && action == 2) || (this.SwapButtons && action == 0);
			if (flag)
			{
				base.StopCoroutine(this.Dash());
				base.StartCoroutine(this.Dash());
			}
			bool flag2 = !this.ToggleSwordBeams;
			if (flag2)
			{
				bool flag3 = (!this.SwapButtons && action == null) || (this.SwapButtons && action == 2);
				if (flag3)
				{
					this.beam = true;
				}
				else
				{
					bool flag4 = (!this.SwapButtons && action == 1) || (this.SwapButtons && action == 3);
					if (flag4)
					{
						this.beam = false;
					}
				}
			}
			else
			{
				bool flag5 = (!this.SwapButtons && action == null) || (this.SwapButtons && action == 2);
				if (flag5)
				{
					this.beam = !this.beam;
				}
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000032FE File Offset: 0x000014FE
		public IEnumerator Dash()
		{
			bool stopOnStart = this.StopOnStart;
			if (stopOnStart)
			{
				Player.local.locomotion.rb.velocity = Vector3.zero;
			}
			bool flag = Player.local.locomotion.moveDirection.magnitude <= 0f || !this.ThumbstickDash;
			if (flag)
			{
				bool flag2 = this.DashDirection == "Item";
				if (flag2)
				{
					Player.local.locomotion.rb.AddForce(this.item.mainHandler.grip.up * this.DashSpeed, 1);
				}
				else
				{
					Player.local.locomotion.rb.AddForce(Player.local.head.transform.forward * this.DashSpeed, 1);
				}
			}
			else
			{
				Player.local.locomotion.rb.AddForce(Player.local.locomotion.moveDirection.normalized * this.DashSpeed, 1);
			}
			bool disableGravity = this.DisableGravity;
			if (disableGravity)
			{
				Player.local.locomotion.rb.useGravity = false;
			}
			bool disableCollision = this.DisableCollision;
			if (disableCollision)
			{
				Player.local.locomotion.rb.detectCollisions = false;
				this.item.rb.detectCollisions = false;
				this.item.mainHandler.rb.detectCollisions = false;
				this.item.mainHandler.otherHand.rb.detectCollisions = false;
			}
			yield return new WaitForSeconds(this.DashTime);
			bool disableGravity2 = this.DisableGravity;
			if (disableGravity2)
			{
				Player.local.locomotion.rb.useGravity = true;
			}
			bool disableCollision2 = this.DisableCollision;
			if (disableCollision2)
			{
				Player.local.locomotion.rb.detectCollisions = true;
				this.item.rb.detectCollisions = true;
				this.item.mainHandler.rb.detectCollisions = true;
				this.item.mainHandler.otherHand.rb.detectCollisions = true;
			}
			bool stopOnEnd = this.StopOnEnd;
			if (stopOnEnd)
			{
				Player.local.locomotion.rb.velocity = Vector3.zero;
			}
			yield break;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00003310 File Offset: 0x00001510
		private void MainCollisionHandler_OnCollisionStartEvent(CollisionInstance collisionInstance)
		{
			bool flag = this.isThrown && collisionInstance.damageStruct.damager != null && collisionInstance.damageStruct.damageType != 3;
			if (flag)
			{
				collisionInstance.damageStruct.damager.UnPenetrateAll();
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00003364 File Offset: 0x00001564
		private void Item_OnUnSnapEvent(Holder holder)
		{
			this.lastHolder = holder;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00003370 File Offset: 0x00001570
		public void FixedUpdate()
		{
			bool isTelekinesisGrabbed = this.item.isTelekinesisGrabbed;
			if (isTelekinesisGrabbed)
			{
				this.lastHandler = null;
			}
			bool flag = this.item.isFlying && this.lastHandler != null;
			if (flag)
			{
				this.item.flyDirRef.Rotate(new Vector3(0f, this.RotationSpeed, 0f) * Time.fixedDeltaTime);
				this.item.rb.useGravity = false;
				this.item.rb.AddForce(-(this.item.transform.position - this.lastHandler.transform.position).normalized * this.ReturnSpeed, 0);
				this.item.IgnoreRagdollCollision(Player.local.creature.ragdoll);
				this.isThrown = true;
				this.startUpdate = true;
			}
			else
			{
				bool flag2 = this.isThrown && !this.item.IsHanded(null) && this.item.holder == null && !this.item.isTelekinesisGrabbed && this.lastHandler != null;
				if (flag2)
				{
					this.item.Throw(1f, 2);
				}
				else
				{
					this.item.flyDirRef.localRotation = Quaternion.identity;
					this.item.rb.useGravity = true;
					this.isThrown = false;
					this.startUpdate = false;
				}
			}
			bool flag3 = this.lastHandler != null && Vector3.Dot(this.item.rb.velocity.normalized, (this.item.transform.position - this.lastHandler.transform.position).normalized) < 0f && Vector3.Distance(this.item.GetMainHandle(this.lastHandler.side).transform.position, this.lastHandler.transform.position) <= 1f && !this.item.IsHanded(null) && this.isThrown && !this.item.isTelekinesisGrabbed && this.startUpdate;
			if (flag3)
			{
				bool flag4 = this.lastHandler.grabbedHandle == null;
				if (flag4)
				{
					this.lastHandler.Grab(this.item.GetMainHandle(this.lastHandler.side), true);
				}
				else
				{
					bool flag5 = this.lastHandler.grabbedHandle != null && this.lastHolder != null && this.lastHolder.HasSlotFree();
					if (flag5)
					{
						Common.MoveAlign(this.item.transform, this.item.holderPoint, this.lastHolder.slots[0], null);
						this.lastHolder.Snap(this.item, false, true);
					}
					else
					{
						bool flag6 = this.lastHandler.grabbedHandle != null && (this.lastHolder == null || !this.lastHolder.HasSlotFree()) && Player.local.creature.equipment.GetFirstFreeHolder(null, null) != null;
						if (flag6)
						{
							Holder firstFreeHolder = Player.local.creature.equipment.GetFirstFreeHolder(null, null);
							Common.MoveAlign(this.item.transform, this.item.holderPoint, firstFreeHolder.slots[0], null);
							firstFreeHolder.Snap(this.item, false, true);
						}
						else
						{
							bool flag7 = this.lastHandler.grabbedHandle != null && (this.lastHolder == null || !this.lastHolder.HasSlotFree()) && Player.local.creature.equipment.GetFirstFreeHolder(null, null) == null;
							if (flag7)
							{
								BackpackHolder.instance.StoreItem(this.item);
							}
						}
					}
				}
				this.item.rb.useGravity = true;
				this.isThrown = false;
				this.startUpdate = false;
			}
			bool flag8 = Time.time - this.cdH <= this.cooldown || !this.beam || this.item.rb.velocity.magnitude - Player.local.locomotion.rb.velocity.magnitude < this.swordSpeed;
			if (!flag8)
			{
				this.cdH = Time.time;
				Catalog.GetData<ItemData>("YamatoBeam", true).SpawnAsync(null, new Vector3?(this.item.flyDirRef.position), new Quaternion?(Quaternion.LookRotation(this.item.flyDirRef.forward, this.item.rb.velocity)), null, true, null);
			}
		}

		// Token: 0x04000032 RID: 50
		private Item item;

		// Token: 0x04000033 RID: 51
		private bool isThrown;

		// Token: 0x04000034 RID: 52
		private Holder lastHolder;

		// Token: 0x04000035 RID: 53
		private RagdollHand lastHandler;

		// Token: 0x04000036 RID: 54
		private bool startUpdate;

		// Token: 0x04000037 RID: 55
		public float DashSpeed;

		// Token: 0x04000038 RID: 56
		public string DashDirection;

		// Token: 0x04000039 RID: 57
		public bool DisableGravity;

		// Token: 0x0400003A RID: 58
		public bool DisableCollision;

		// Token: 0x0400003B RID: 59
		public float DashTime;

		// Token: 0x0400003C RID: 60
		private float cdH;

		// Token: 0x0400003D RID: 61
		private float cooldown;

		// Token: 0x0400003E RID: 62
		private float swordSpeed;

		// Token: 0x0400003F RID: 63
		private bool beam;

		// Token: 0x04000040 RID: 64
		public float RotationSpeed;

		// Token: 0x04000041 RID: 65
		public float ReturnSpeed;

		// Token: 0x04000042 RID: 66
		public bool StopOnEnd;

		// Token: 0x04000043 RID: 67
		public bool StopOnStart;

		// Token: 0x04000044 RID: 68
		private bool ThumbstickDash;

		// Token: 0x04000045 RID: 69
		private bool SwapButtons;

		// Token: 0x04000046 RID: 70
		private bool ToggleSwordBeams;
	}
}
