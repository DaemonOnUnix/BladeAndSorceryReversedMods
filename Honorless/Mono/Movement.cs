using System;
using ThunderRoad;
using UnityEngine;

namespace Wully.Mono
{
	// Token: 0x0200000B RID: 11
	public class Movement : MonoBehaviour
	{
		// Token: 0x0600004D RID: 77 RVA: 0x00004705 File Offset: 0x00002905
		private void Awake()
		{
			PlayerControl.local.OnJumpButtonEvent += new PlayerControl.JumpEvent(this.OnJumpButtonEvent);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00004720 File Offset: 0x00002920
		public void OnJumpButtonEvent(bool active, EventTime eventTime)
		{
			if (eventTime == null)
			{
				return;
			}
			if (!active)
			{
				return;
			}
			if (Player.local.locomotion != null)
			{
				Locomotion lm = Player.local.locomotion;
				if (lm.isGrounded)
				{
					this.isDoubleJumping = false;
				}
				if (!this.isDoubleJumping && !lm.isJumping && !lm.isGrounded)
				{
					Debug.Log("double jumping");
					Vector3 velocity = lm.rb.velocity;
					velocity.y = 0f;
					lm.rb.velocity = velocity;
					this.isDoubleJumping = true;
					lm.isGrounded = true;
					lm.Jump(true);
					lm.isGrounded = false;
				}
			}
		}

		// Token: 0x0400006E RID: 110
		private bool isDoubleJumping;
	}
}
