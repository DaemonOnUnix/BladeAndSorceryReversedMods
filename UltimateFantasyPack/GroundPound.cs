using System;
using ThunderRoad;
using UnityEngine;

namespace UltimateFantasyPack
{
	// Token: 0x02000020 RID: 32
	public class GroundPound : MonoBehaviour
	{
		// Token: 0x06000078 RID: 120 RVA: 0x00004392 File Offset: 0x00002592
		public void Awake()
		{
			this.item = base.GetComponent<Item>();
		}

		// Token: 0x06000079 RID: 121 RVA: 0x000043A1 File Offset: 0x000025A1
		public void Setup(float importPower)
		{
			this.explodePower1 = importPower;
		}

		// Token: 0x0600007A RID: 122 RVA: 0x000043AB File Offset: 0x000025AB
		public void Update()
		{
			this.timer -= Time.deltaTime;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x000043C0 File Offset: 0x000025C0
		public void ResetTimer()
		{
			Debug.Log("ResetTimer");
			this.timer = 10f;
		}

		// Token: 0x0600007C RID: 124 RVA: 0x000043DC File Offset: 0x000025DC
		public void OnCollisionEnter(Collision collision)
		{
			bool alternateUsePressed = Player.local.handRight.controlHand.alternateUsePressed;
			if (alternateUsePressed)
			{
				bool flag = this.timer <= 0f;
				if (flag)
				{
					this.timer = 10f;
					foreach (Collider colldier in Physics.OverlapSphere(this.item.transform.position, 10f))
					{
						Item componentInParent = colldier.GetComponentInParent<Item>();
						Object @object;
						if (componentInParent == null)
						{
							@object = null;
						}
						else
						{
							RagdollHand mainHandler = componentInParent.mainHandler;
							@object = ((mainHandler != null) ? mainHandler.GetComponentInParent<Player>() : null);
						}
						bool flag2 = @object == null;
						if (flag2)
						{
							bool flag3 = colldier.GetComponentInParent<Creature>() == null;
							if (flag3)
							{
								Vector3 direction = (colldier.transform.position - this.item.gameObject.transform.position).normalized;
								Rigidbody attachedRigidbody = colldier.attachedRigidbody;
								if (attachedRigidbody != null)
								{
									attachedRigidbody.AddForce(direction * this.explodePower1 * colldier.attachedRigidbody.mass, 1);
								}
							}
						}
						bool flag4 = colldier.GetComponentInParent<Creature>() == null;
						if (flag4)
						{
							Vector3 direction2 = (colldier.transform.position - this.item.gameObject.transform.position).normalized;
							Rigidbody attachedRigidbody2 = colldier.attachedRigidbody;
							if (attachedRigidbody2 != null)
							{
								attachedRigidbody2.AddForce(direction2 * this.explodePower1 * colldier.attachedRigidbody.mass, 1);
							}
							colldier.GetComponentInParent<Creature>().ragdoll.SetState(0);
						}
					}
					Catalog.GetData<ItemData>("GroundPoundVFX", true).SpawnAsync(delegate(Item item1)
					{
						ContactPoint contact = collision.contacts[0];
						Quaternion rot = Quaternion.FromToRotation(Vector3.left, contact.normal);
						item1.transform.rotation = rot;
						item1.transform.position = this.item.flyDirRef.transform.position;
						item1.rb.isKinematic = true;
						this.ResetTimer();
					}, null, null, null, true, null);
				}
			}
			bool alternateUsePressed2 = Player.local.handLeft.controlHand.alternateUsePressed;
			if (alternateUsePressed2)
			{
				bool flag5 = this.timer <= 0f;
				if (flag5)
				{
					this.timer = 10f;
					foreach (Collider colldier2 in Physics.OverlapSphere(this.item.transform.position, 10f))
					{
						Item componentInParent2 = colldier2.GetComponentInParent<Item>();
						Object object2;
						if (componentInParent2 == null)
						{
							object2 = null;
						}
						else
						{
							RagdollHand mainHandler2 = componentInParent2.mainHandler;
							object2 = ((mainHandler2 != null) ? mainHandler2.GetComponentInParent<Player>() : null);
						}
						bool flag6 = object2 == null;
						if (flag6)
						{
							Vector3 direction3 = (colldier2.transform.position - this.item.gameObject.transform.position).normalized;
							Rigidbody attachedRigidbody3 = colldier2.attachedRigidbody;
							if (attachedRigidbody3 != null)
							{
								attachedRigidbody3.AddForce(direction3 * this.explodePower1 * colldier2.attachedRigidbody.mass, 1);
							}
						}
						bool flag7 = colldier2.GetComponentInParent<Creature>() == null;
						if (flag7)
						{
							Vector3 direction4 = (colldier2.transform.position - this.item.gameObject.transform.position).normalized;
							Rigidbody attachedRigidbody4 = colldier2.attachedRigidbody;
							if (attachedRigidbody4 != null)
							{
								attachedRigidbody4.AddForce(direction4 * this.explodePower1 * colldier2.attachedRigidbody.mass, 1);
							}
							colldier2.GetComponentInParent<Creature>().ragdoll.SetState(0);
						}
					}
					Catalog.GetData<ItemData>("GroundPoundVFX", true).SpawnAsync(delegate(Item item1)
					{
						ContactPoint contact = collision.contacts[0];
						Quaternion rot = Quaternion.FromToRotation(Vector3.left, contact.normal);
						item1.transform.rotation = rot;
						item1.transform.position = this.item.flyDirRef.transform.position;
						item1.rb.isKinematic = true;
						this.ResetTimer();
					}, null, null, null, true, null);
				}
			}
		}

		// Token: 0x0400005A RID: 90
		private Item item;

		// Token: 0x0400005B RID: 91
		public float timer = 0f;

		// Token: 0x0400005C RID: 92
		public float explodePower1;
	}
}
