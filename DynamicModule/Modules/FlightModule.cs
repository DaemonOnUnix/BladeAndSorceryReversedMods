using System;
using System.Collections;
using ThunderRoad;
using UnityEngine;

namespace DynamicModule.Modules
{
	// Token: 0x02000005 RID: 5
	public class FlightModule : MonoBehaviour
	{
		// Token: 0x06000007 RID: 7 RVA: 0x000021E8 File Offset: 0x000003E8
		private void Update()
		{
			bool flag = this.Weapon.currentHand != null && this.Weapon.currentHand.controlHand.secondaryUsePressed;
			if (flag)
			{
				Player.local.locomotion.rb.AddForce(this.Weapon.Item.holderPoint.forward * this.Weapon.Data.FlightPower, 1);
				bool flag2 = !this.isFlying;
				if (flag2)
				{
					base.StartCoroutine(this.InitFlight());
				}
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002281 File Offset: 0x00000481
		private IEnumerator InitFlight()
		{
			this.isFlying = true;
			bool temp = Player.fallDamage;
			while (!Player.local.locomotion.isGrounded)
			{
				Player.fallDamage = false;
				yield return null;
			}
			Player.fallDamage = temp;
			this.isFlying = false;
			yield break;
		}

		// Token: 0x04000017 RID: 23
		public InitiateBehavior Weapon;

		// Token: 0x04000018 RID: 24
		private bool isFlying = false;
	}
}
