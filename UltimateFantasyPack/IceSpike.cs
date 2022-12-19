using System;
using System.Collections;
using ThunderRoad;
using UnityEngine;

namespace UltimateFantasyPack
{
	// Token: 0x02000012 RID: 18
	public class IceSpike : ItemModule
	{
		// Token: 0x0600003E RID: 62 RVA: 0x00002F04 File Offset: 0x00001104
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			Debug.Log(this.spikeAddressablesNames.Length);
			this.spikeGhosts = new GameObject[this.spikeAddressablesNames.Length];
			for (int i = 0; i < this.spikeGhosts.Length; i++)
			{
				this.Register(i);
			}
			item.OnHeldActionEvent += new Item.HeldActionDelegate(this.Item_OnHeldActionEvent);
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002F74 File Offset: 0x00001174
		private void Item_OnHeldActionEvent(RagdollHand ragdollHand, Handle handle, Interactable.Action action)
		{
			bool flag = action == 2 && !this.onCooldown;
			if (flag)
			{
				GameObject go = Object.Instantiate<GameObject>(this.spikeGhosts[Random.Range(0, this.spikeGhosts.Length)], ragdollHand.creature.ragdoll.transform.position + ragdollHand.creature.ragdoll.transform.forward * this.iceForwardStart - new Vector3(0f, this.downOffset, 0f), ragdollHand.creature.ragdoll.transform.rotation);
				go.GetComponentInChildren<ParticleSystem>().Play();
				Object.Destroy(go, 10f);
				GameManager.local.StartCoroutine(this.Cooldown());
				foreach (Collider collider in Physics.OverlapSphere(this.item.transform.position, 10f))
				{
					bool flag2 = collider.attachedRigidbody != Player.local.locomotion.rb && collider.attachedRigidbody != this.item.rb;
					if (flag2)
					{
						Rigidbody attachedRigidbody = collider.attachedRigidbody;
						if (attachedRigidbody != null)
						{
							attachedRigidbody.AddForce((collider.transform.position - this.item.transform.position).normalized * this.blastForce, 1);
						}
					}
					Creature creature = collider.gameObject.GetComponentInParent<Creature>();
					bool flag3 = creature != null && !creature.isPlayer;
					if (flag3)
					{
						bool flag4 = !creature.isKilled;
						if (flag4)
						{
							creature.ragdoll.SetState(1);
						}
						bool flag5 = this.killIfStruck;
						if (flag5)
						{
							creature.Kill();
						}
					}
				}
			}
		}

		// Token: 0x06000040 RID: 64 RVA: 0x0000315E File Offset: 0x0000135E
		private IEnumerator Cooldown()
		{
			this.onCooldown = true;
			yield return new WaitForSeconds(this.cooldown);
			this.onCooldown = false;
			yield break;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00003170 File Offset: 0x00001370
		private void Register(int i)
		{
			Catalog.LoadAssetAsync<GameObject>(this.spikeAddressablesNames[i], delegate(GameObject go)
			{
				this.spikeGhosts[i] = go;
			}, "Ultimate Fantasy Pack");
		}

		// Token: 0x04000022 RID: 34
		public float cooldown;

		// Token: 0x04000023 RID: 35
		public float iceForwardStart;

		// Token: 0x04000024 RID: 36
		public float downOffset;

		// Token: 0x04000025 RID: 37
		public float blastDistance;

		// Token: 0x04000026 RID: 38
		public float blastForce;

		// Token: 0x04000027 RID: 39
		public bool killIfStruck;

		// Token: 0x04000028 RID: 40
		public string[] spikeAddressablesNames;

		// Token: 0x04000029 RID: 41
		private bool onCooldown;

		// Token: 0x0400002A RID: 42
		private GameObject[] spikeGhosts;
	}
}
