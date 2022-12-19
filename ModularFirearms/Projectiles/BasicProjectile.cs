using System;
using ModularFirearms.Shared;
using ThunderRoad;
using UnityEngine;

namespace ModularFirearms.Projectiles
{
	// Token: 0x02000016 RID: 22
	public class BasicProjectile : MonoBehaviour
	{
		// Token: 0x060000A4 RID: 164 RVA: 0x00007C1A File Offset: 0x00005E1A
		protected void Awake()
		{
			this.item = base.GetComponent<Item>();
			this.module = this.item.data.GetModule<ProjectileModule>();
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00007C3E File Offset: 0x00005E3E
		protected void Start()
		{
			if (this.module.allowFlyTime)
			{
				this.item.rb.useGravity = false;
				this.isFlying = true;
			}
			this.item.Despawn(this.module.lifetime);
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00007C7B File Offset: 0x00005E7B
		public void SetShooterItem(Item ShooterItemIn)
		{
			this.shooterItemString = ShooterItemIn.name;
			this.shooterItem = ShooterItemIn;
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00007C90 File Offset: 0x00005E90
		public void AddChargeToQueue(string SpellID)
		{
			this.queuedSpell = SpellID;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00007C9C File Offset: 0x00005E9C
		private void LateUpdate()
		{
			if (this.isFlying)
			{
				this.item.rb.velocity = this.item.rb.velocity * this.module.flyingAcceleration;
			}
			this.TransferImbueCharge(this.item, this.queuedSpell);
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00007CF4 File Offset: 0x00005EF4
		private void OnCollisionEnter(Collision hit)
		{
			if (hit.gameObject.name.Contains(this.shooterItemString))
			{
				return;
			}
			if (this.item.rb.useGravity)
			{
				return;
			}
			this.item.rb.useGravity = true;
			this.isFlying = false;
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00007D48 File Offset: 0x00005F48
		private void TransferImbueCharge(Item imbueTarget, string spellID)
		{
			if (string.IsNullOrEmpty(spellID))
			{
				return;
			}
			SpellCastCharge spellCastCharge = Catalog.GetData<SpellCastCharge>(spellID, true).Clone();
			foreach (Imbue imbue in imbueTarget.imbues)
			{
				try
				{
					base.StartCoroutine(FrameworkCore.TransferDeltaEnergy(imbue, spellCastCharge, 20f, 5));
					this.queuedSpell = null;
					break;
				}
				catch
				{
				}
			}
		}

		// Token: 0x0400017F RID: 383
		protected Item item;

		// Token: 0x04000180 RID: 384
		protected ProjectileModule module;

		// Token: 0x04000181 RID: 385
		protected string queuedSpell;

		// Token: 0x04000182 RID: 386
		protected bool isFlying;

		// Token: 0x04000183 RID: 387
		public string shooterItemString = "";

		// Token: 0x04000184 RID: 388
		public Item shooterItem;
	}
}
