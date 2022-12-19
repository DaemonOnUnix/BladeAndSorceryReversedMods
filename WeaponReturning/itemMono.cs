using System;
using ThunderRoad;
using UnityEngine;

namespace WeaponReturning
{
	// Token: 0x02000003 RID: 3
	internal class itemMono : MonoBehaviour
	{
		// Token: 0x06000003 RID: 3 RVA: 0x00002075 File Offset: 0x00000275
		private void Awake()
		{
			this.item = base.GetComponent<Item>();
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002084 File Offset: 0x00000284
		private void checkHandSide(RagdollHand handBinded, Item value)
		{
			bool flag = handBinded.side == 0;
			if (flag)
			{
				itemMono.rightHandItem = value;
			}
			else
			{
				itemMono.leftHandItem = value;
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020B1 File Offset: 0x000002B1
		private void startTimer()
		{
			this.timer = new float?(this.settings.holdDuration);
			this.hasStarted = true;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000020D4 File Offset: 0x000002D4
		private void bringItem()
		{
			this.item.SetColliderAndMeshLayer(GameManager.GetLayer(1));
			float sqrMagnitude = (this.handInUse.transform.position - this.item.transform.position).sqrMagnitude;
			bool flag = sqrMagnitude <= 2f;
			if (flag)
			{
				this.handInUse.Grab(this.item.handles[0]);
				this.item.RefreshCollision(false);
			}
			Vector3 normalized = (this.handInUse.transform.position - this.item.transform.position).normalized;
			this.item.rb.AddForce(normalized * this.settings.returnForce, 2);
			bool flag2 = !this.item.isPenetrating;
			if (flag2)
			{
				Quaternion quaternion = Quaternion.Euler(new Vector3(this.settings.rotationPerSecond, 0f, 0f) * Time.fixedDeltaTime);
				this.item.rb.MoveRotation(this.item.rb.rotation * quaternion);
			}
			else
			{
				this.item.GetComponentInChildren<Damager>().UnPenetrateAll();
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x0000222C File Offset: 0x0000042C
		private void FixedUpdate()
		{
			bool flag = this.item.handlers.Count > 0;
			if (flag)
			{
				this.handInUse = this.item.handlers[0];
				this.checkHandSide(this.handInUse, this.item);
			}
			bool flag2 = this.handInUse != null;
			if (flag2)
			{
				bool alternateUsePressed = this.handInUse.playerHand.controlHand.alternateUsePressed;
				if (alternateUsePressed)
				{
					bool flag3 = !this.hasStarted;
					if (flag3)
					{
						this.startTimer();
					}
					bool flag4 = this.timer != null;
					if (flag4)
					{
						this.timer -= Time.deltaTime;
						float? num = this.timer;
						float num2 = 0f;
						bool flag5 = (num.GetValueOrDefault() <= num2) & (num != null);
						if (flag5)
						{
							this.timer = null;
							this.checkHandSide(this.handInUse, null);
							this.hasStarted = false;
						}
					}
				}
				bool flag6 = this.item.holder == null;
				if (flag6)
				{
					bool flag7 = this.handInUse.playerHand.controlHand.gripPressed && this.handInUse.grabbedHandle == null && this.handInUse.caster.telekinesis.catchedHandle == null;
					if (flag7)
					{
						bool flag8 = itemMono.rightHandItem == this.item || itemMono.leftHandItem == this.item;
						if (flag8)
						{
							this.bringItem();
						}
						else
						{
							this.handInUse = null;
						}
					}
				}
			}
		}

		// Token: 0x04000004 RID: 4
		private Item item;

		// Token: 0x04000005 RID: 5
		public itemClass settings;

		// Token: 0x04000006 RID: 6
		private RagdollHand handInUse;

		// Token: 0x04000007 RID: 7
		private bool hasStarted = false;

		// Token: 0x04000008 RID: 8
		private float? timer;

		// Token: 0x04000009 RID: 9
		public static Item leftHandItem;

		// Token: 0x0400000A RID: 10
		public static Item rightHandItem;
	}
}
