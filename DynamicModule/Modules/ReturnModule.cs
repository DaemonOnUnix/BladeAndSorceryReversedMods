using System;
using ThunderRoad;
using UnityEngine;

namespace DynamicModule.Modules
{
	// Token: 0x02000008 RID: 8
	public class ReturnModule : MonoBehaviour
	{
		// Token: 0x06000013 RID: 19 RVA: 0x00002784 File Offset: 0x00000984
		private void Update()
		{
			bool flag = this.Weapon.currentHand != null;
			if (flag)
			{
				this.bindPlayerHand = this.Weapon.currentHand;
			}
			bool flag2 = this.bindPlayerHand != null;
			if (flag2)
			{
				bool flag3 = KeyBind.CheckBind(this.bindPlayerHand, this.Weapon.Data.ReturnModuleReturnBind) && !this.Weapon.Item.isPooled && this.bindPlayerHand.ragdollHand.grabbedHandle == null && this.bindPlayerHand.ragdollHand.caster.telekinesis.catchedHandle == null;
				if (flag3)
				{
					bool flag4 = Vector3.Distance(this.bindPlayerHand.ragdollHand.transform.position, base.transform.position) < this.Weapon.Data.GrabDistance;
					if (flag4)
					{
						this.Weapon.Item.rb.velocity = Vector3.zero;
						this.bindPlayerHand.ragdollHand.Grab(this.Weapon.Item.handles[0]);
					}
					Vector3 normalized = (this.bindPlayerHand.ragdollHand.transform.position - base.transform.position).normalized;
					this.Weapon.Item.rb.AddForce(normalized * this.Weapon.Data.ReturnForce, 1);
					this.Weapon.Item.transform.Rotate(0f, this.Weapon.Data.RoationSpeed * Time.deltaTime, 0f);
				}
				bool flag5 = KeyBind.CheckBind(this.bindPlayerHand, this.Weapon.Data.ReturnModuleResetBind);
				if (flag5)
				{
					bool flag6 = !this.isHeld;
					if (flag6)
					{
						this.isHeld = true;
						this.timer = this.Weapon.Data.HoldDurationReset;
					}
					this.timer -= Time.deltaTime;
					bool flag7 = this.timer <= 0f;
					if (flag7)
					{
						this.bindPlayerHand = null;
						this.isHeld = false;
					}
				}
			}
		}

		// Token: 0x04000022 RID: 34
		public InitiateBehavior Weapon;

		// Token: 0x04000023 RID: 35
		private PlayerHand bindPlayerHand;

		// Token: 0x04000024 RID: 36
		private bool isHeld = false;

		// Token: 0x04000025 RID: 37
		private float timer;
	}
}
