using System;
using ThunderRoad;
using UnityEngine;

namespace UltimateFantasyPack
{
	// Token: 0x0200001E RID: 30
	public class DrakenFlame : MonoBehaviour
	{
		// Token: 0x06000071 RID: 113 RVA: 0x00003EF8 File Offset: 0x000020F8
		public void Awake()
		{
			this.item = base.GetComponent<Item>();
			this.item.OnHeldActionEvent += new Item.HeldActionDelegate(this.OnHeld);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00003F20 File Offset: 0x00002120
		public void Update()
		{
			this.timer -= Time.deltaTime;
			this.timer1 -= Time.deltaTime;
			bool flag = this.timer <= 0f;
			if (flag)
			{
				this.timer = 0f;
			}
			bool flag2 = this.timer1 <= 0f;
			if (flag2)
			{
				this.timer1 = 0f;
				this.Imbued = false;
			}
			bool imbued = this.Imbued;
			if (imbued)
			{
				bool flag3 = this.timer1 > 0f;
				if (flag3)
				{
					foreach (Imbue imbue in this.item.imbues)
					{
						imbue.Transfer(Catalog.GetData<SpellCastCharge>("Fire", true).Clone(), imbue.maxEnergy);
					}
				}
			}
			else
			{
				bool flag4 = !this.Imbued;
				if (flag4)
				{
					this.item.GetCustomReference("FireMesh", true).gameObject.SetActive(false);
					this.item.GetCustomReference("NormalMesh", true).gameObject.SetActive(true);
					foreach (Imbue imbue2 in this.item.imbues)
					{
						imbue2.energy = 0f;
					}
				}
			}
		}

		// Token: 0x06000073 RID: 115 RVA: 0x000040C8 File Offset: 0x000022C8
		private void OnCollisionEnter(Collision collision)
		{
			bool imbued = this.Imbued;
			if (imbued)
			{
				bool flag = collision.collider.gameObject.GetComponentInParent<Creature>() != null;
				if (flag)
				{
					bool flag2 = collision.collider.gameObject.GetComponentInParent<Player>() == null;
					if (flag2)
					{
						collision.collider.GetComponentInParent<Creature>().ragdoll.SetState(0);
						collision.collider.GetComponentInParent<Creature>().TryElectrocute(50f, 5f, true, true, Catalog.GetData<EffectData>("ImbueFireRagdoll", true));
						Vector3 direction = (collision.collider.transform.position - this.item.gameObject.transform.position).normalized;
						Rigidbody attachedRigidbody = collision.collider.attachedRigidbody;
						if (attachedRigidbody != null)
						{
							attachedRigidbody.AddForce(direction * this.force * collision.collider.attachedRigidbody.mass, 1);
						}
					}
				}
				bool flag3 = collision.collider.gameObject.GetComponentInParent<Item>() != null;
				if (flag3)
				{
					Vector3 direction2 = (collision.collider.transform.position - this.item.gameObject.transform.position).normalized;
					Rigidbody attachedRigidbody2 = collision.collider.attachedRigidbody;
					if (attachedRigidbody2 != null)
					{
						attachedRigidbody2.AddForce(direction2 * this.force * collision.collider.attachedRigidbody.mass, 1);
					}
				}
			}
		}

		// Token: 0x06000074 RID: 116 RVA: 0x0000425C File Offset: 0x0000245C
		private void OnHeld(RagdollHand ragdollHand, Handle handle, Interactable.Action action)
		{
			bool flag = action == 2;
			if (flag)
			{
				bool flag2 = this.timer <= 0f;
				if (flag2)
				{
					bool flag3 = !this.Imbued;
					if (flag3)
					{
						this.Imbued = true;
						this.timer1 = 10f;
						this.timer = 20f;
						this.item.GetCustomReference("FireMesh", true).gameObject.SetActive(true);
						this.item.GetCustomReference("NormalMesh", true).gameObject.SetActive(false);
						EffectInstance effect = Catalog.GetData<EffectData>("DrakenChargeEffect", true).Spawn(this.item.transform, true, null, false, Array.Empty<Type>());
						effect.Play(0, false);
						effect.onEffectFinished += delegate(EffectInstance effect2)
						{
							effect2.Despawn();
						};
					}
				}
			}
		}

		// Token: 0x04000055 RID: 85
		private Item item;

		// Token: 0x04000056 RID: 86
		public bool Imbued = false;

		// Token: 0x04000057 RID: 87
		public float timer = 0f;

		// Token: 0x04000058 RID: 88
		public float timer1 = 0f;

		// Token: 0x04000059 RID: 89
		public float force;
	}
}
