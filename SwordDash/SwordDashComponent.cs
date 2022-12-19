using System;
using System.Collections;
using ThunderRoad;
using UnityEngine;

namespace SwordDash
{
	// Token: 0x02000003 RID: 3
	public class SwordDashComponent : MonoBehaviour
	{
		// Token: 0x06000003 RID: 3 RVA: 0x00002099 File Offset: 0x00000299
		public void Start()
		{
			this.item = base.GetComponent<Item>();
			this.item.OnHeldActionEvent += new Item.HeldActionDelegate(this.Item_OnHeldActionEvent);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020C0 File Offset: 0x000002C0
		private void Item_OnHeldActionEvent(RagdollHand ragdollHand, Handle handle, Interactable.Action action)
		{
			string activationButton = this.ActivationButton;
			string text = activationButton;
			if (!(text == "Trigger"))
			{
				if (!(text == "Alt Use"))
				{
					bool flag = action == 0;
					if (flag)
					{
						base.StopCoroutine(this.Dash());
						base.StartCoroutine(this.Dash());
					}
				}
				else
				{
					bool flag2 = action == 2;
					if (flag2)
					{
						base.StopCoroutine(this.Dash());
						base.StartCoroutine(this.Dash());
					}
				}
			}
			else
			{
				bool flag3 = action == 0;
				if (flag3)
				{
					base.StopCoroutine(this.Dash());
					base.StartCoroutine(this.Dash());
				}
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002167 File Offset: 0x00000367
		public IEnumerator Dash()
		{
			bool flag = this.DashDirection == "Item";
			if (flag)
			{
				Player.local.locomotion.rb.AddForce(this.item.mainHandler.grip.up * this.DashSpeed, 1);
			}
			else
			{
				Player.local.locomotion.rb.AddForce(Player.local.head.transform.forward * this.DashSpeed, 1);
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
			yield break;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002178 File Offset: 0x00000378
		public void Setup(float speed, string direction, bool gravity, bool collision, float time, string button)
		{
			this.DashSpeed = speed;
			this.DashDirection = direction;
			this.DisableGravity = gravity;
			this.DisableCollision = collision;
			this.DashTime = time;
			bool flag = button.ToLower().Contains("trigger") || button.ToLower() == "use";
			if (flag)
			{
				this.ActivationButton = "Trigger";
			}
			else
			{
				bool flag2 = button.ToLower().Contains("alt") || button.ToLower().Contains("spell");
				if (flag2)
				{
					this.ActivationButton = "Alt Use";
				}
			}
			bool flag3 = direction.ToLower().Contains("player") || direction.ToLower().Contains("head") || direction.ToLower().Contains("sight");
			if (flag3)
			{
				this.DashDirection = "Player";
			}
			else
			{
				bool flag4 = direction.ToLower().Contains("item") || direction.ToLower().Contains("pierce") || direction.ToLower().Contains("flyref") || direction.ToLower().Contains("weapon");
				if (flag4)
				{
					this.DashDirection = "Item";
				}
			}
		}

		// Token: 0x04000007 RID: 7
		public float DashSpeed;

		// Token: 0x04000008 RID: 8
		public string DashDirection;

		// Token: 0x04000009 RID: 9
		public bool DisableGravity;

		// Token: 0x0400000A RID: 10
		public bool DisableCollision;

		// Token: 0x0400000B RID: 11
		public float DashTime;

		// Token: 0x0400000C RID: 12
		public string ActivationButton;

		// Token: 0x0400000D RID: 13
		private Item item;
	}
}
