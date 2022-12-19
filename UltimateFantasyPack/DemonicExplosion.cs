using System;
using System.Collections;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

namespace UltimateFantasyPack
{
	// Token: 0x0200001C RID: 28
	public class DemonicExplosion : MonoBehaviour
	{
		// Token: 0x06000067 RID: 103 RVA: 0x00003B6C File Offset: 0x00001D6C
		public void Awake()
		{
			this.item = base.GetComponent<Item>();
			this.fireSpell = Catalog.GetData<SpellCastCharge>("Fire", true);
			this.Imbue();
			base.Invoke("Imbue", 0.25f);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00003BA4 File Offset: 0x00001DA4
		public void Setup(float explodePower, float cooldownDuration, float fireLerpDuration)
		{
			this.explodePower = explodePower;
			this.cooldownDuration = cooldownDuration;
			this.fireLerpDuration = fireLerpDuration;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00003BBC File Offset: 0x00001DBC
		private void Imbue()
		{
			foreach (Imbue imbue in this.item.imbues)
			{
				imbue.Transfer(this.fireSpell, imbue.maxEnergy);
				imbue.energy = imbue.maxEnergy;
			}
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00003C34 File Offset: 0x00001E34
		public void Explosion()
		{
			this.imbue = false;
			GameManager.local.StartCoroutine(this.ItemImbue());
			Catalog.GetData<ItemData>("DemonicExplosionVFX", true).SpawnAsync(delegate(Item item1)
			{
				item1.transform.position = this.item.flyDirRef.transform.position;
				item1.rb.isKinematic = true;
			}, null, null, null, true, null);
			foreach (Collider collider in Physics.OverlapSphere(this.item.transform.position, 10f))
			{
				Item item = collider.GetComponentInParent<Item>();
				bool flag = item != null && !item.mainHandler;
				if (flag)
				{
					Vector3 direction = (collider.transform.position - item.gameObject.transform.position).normalized;
					Rigidbody attachedRigidbody = collider.attachedRigidbody;
					if (attachedRigidbody != null)
					{
						attachedRigidbody.AddForce(direction * this.explodePower, 1);
					}
				}
				Creature creature = collider.GetComponentInParent<Creature>();
				bool flag2 = creature != null && !creature.isPlayer;
				if (flag2)
				{
					Vector3 direction2 = (collider.transform.position - this.item.gameObject.transform.position).normalized;
					Rigidbody attachedRigidbody2 = collider.attachedRigidbody;
					if (attachedRigidbody2 != null)
					{
						attachedRigidbody2.AddForce(direction2 * this.explodePower, 1);
					}
					creature.TryElectrocute(100f, 10f, true, true, Catalog.GetData<EffectData>("ImbueFire", true));
					creature.Kill();
					bool flag3 = !creature.isKilled;
					if (flag3)
					{
						creature.ragdoll.SetState(1);
					}
				}
			}
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00003DF4 File Offset: 0x00001FF4
		public void OnCollisionEnter(Collision collision)
		{
			bool flag = this.imbue;
			if (flag)
			{
				this.Imbue();
			}
			RagdollHand mainHandler = this.item.mainHandler;
			object obj;
			if (mainHandler == null)
			{
				obj = null;
			}
			else
			{
				PlayerHand playerHand = mainHandler.playerHand;
				obj = ((playerHand != null) ? playerHand.controlHand : null);
			}
			bool flag2 = obj == null;
			if (!flag2)
			{
				bool flag3 = this.item.mainHandler.playerHand.controlHand.alternateUsePressed && this.imbue;
				if (flag3)
				{
					this.Explosion();
				}
			}
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00003E71 File Offset: 0x00002071
		private IEnumerator ItemImbue()
		{
			foreach (Imbue imbue in this.item.imbues)
			{
				imbue.energy = 0f;
				imbue = null;
			}
			List<Imbue>.Enumerator enumerator = default(List<Imbue>.Enumerator);
			yield return new WaitForSeconds(this.cooldownDuration);
			float time = this.fireLerpDuration;
			while (time > 0f)
			{
				time -= Time.deltaTime;
				foreach (Imbue imbue2 in this.item.imbues)
				{
					imbue2.Transfer(this.fireSpell, 1f);
					imbue2.energy = Mathf.Lerp(imbue2.maxEnergy, 0f, time / this.fireLerpDuration);
					imbue2 = null;
				}
				List<Imbue>.Enumerator enumerator2 = default(List<Imbue>.Enumerator);
				yield return null;
			}
			this.imbue = true;
			EffectInstance effect = Catalog.GetData<EffectData>("DemonicChargeEffect", true).Spawn(this.item.transform, true, null, false, Array.Empty<Type>());
			effect.Play(0, false);
			effect.onEffectFinished += delegate(EffectInstance effect2)
			{
				effect2.Despawn();
			};
			yield break;
		}

		// Token: 0x0400004C RID: 76
		private Item item;

		// Token: 0x0400004D RID: 77
		private float explodePower;

		// Token: 0x0400004E RID: 78
		private float cooldownDuration;

		// Token: 0x0400004F RID: 79
		private float fireLerpDuration;

		// Token: 0x04000050 RID: 80
		private bool imbue = true;

		// Token: 0x04000051 RID: 81
		private SpellCastCharge fireSpell;
	}
}
