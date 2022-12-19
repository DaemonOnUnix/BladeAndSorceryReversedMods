using System;
using System.Collections;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

namespace UltimateFantasyPack
{
	// Token: 0x02000018 RID: 24
	public class LightningStrikeNew : MonoBehaviour
	{
		// Token: 0x06000058 RID: 88 RVA: 0x00003680 File Offset: 0x00001880
		public void Setup(float explosionForce, float explosionRange, float airDuration)
		{
			this.item = base.GetComponent<Item>();
			this.lightningSpell = Catalog.GetData<SpellCastCharge>("Lightning", true);
			this.audioSource = this.item.GetCustomReference("audioSource", true).gameObject.GetComponent<AudioSource>();
			this.item.OnGrabEvent += new Item.GrabDelegate(this.OnGrab);
			this.item.OnUngrabEvent += new Item.ReleaseDelegate(this.OnUnGrab);
			this.item.mainCollisionHandler.OnCollisionStartEvent += new CollisionHandler.CollisionEvent(this.MainCollisionHandler_OnCollisionStartEvent);
			this.explosionForce = explosionForce;
			this.explosionRange = explosionRange;
			this.airDuration = airDuration;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00003730 File Offset: 0x00001930
		private void MainCollisionHandler_OnCollisionStartEvent(CollisionInstance collisionInstance)
		{
			bool flag = (this.imbued && this.canPound && PlayerControl.GetHand(1).alternateUsePressed) || (this.imbued && this.canPound && PlayerControl.GetHand(0).alternateUsePressed);
			if (flag)
			{
				this.imbued = false;
				this.canPound = false;
				Catalog.GetData<EffectData>("SpellLightningStaffSlam", true).Spawn(collisionInstance.contactPoint, Quaternion.LookRotation(collisionInstance.contactNormal), null, null, true, null, false, Array.Empty<Type>()).Play(0, false);
				foreach (Imbue imbue in this.item.imbues)
				{
					imbue.energy = 0f;
				}
				foreach (Collider collider in Physics.OverlapSphere(this.item.transform.position, this.explosionRange))
				{
					Item item = collider.GetComponentInParent<Item>();
					bool flag2 = item != null && !item.mainHandler;
					if (flag2)
					{
						Vector3 direction = (collider.transform.position - item.gameObject.transform.position).normalized;
						Rigidbody attachedRigidbody = collider.attachedRigidbody;
						if (attachedRigidbody != null)
						{
							attachedRigidbody.AddForce(direction * this.explosionForce, 1);
						}
					}
					Creature creature = collider.GetComponentInParent<Creature>();
					bool flag3 = creature != null && !creature.isPlayer;
					if (flag3)
					{
						Vector3 direction2 = (collider.transform.position - this.item.gameObject.transform.position).normalized;
						Rigidbody attachedRigidbody2 = collider.attachedRigidbody;
						if (attachedRigidbody2 != null)
						{
							attachedRigidbody2.AddForce(direction2 * this.explosionForce, 1);
						}
						bool flag4 = !creature.isKilled;
						if (flag4)
						{
							creature.ragdoll.SetState(1);
						}
						creature.TryElectrocute(50f, 5f, true, true, Catalog.GetData<EffectData>("ImbueLightningRagdoll", true));
					}
				}
			}
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00003980 File Offset: 0x00001B80
		public void Update()
		{
			bool flag;
			if (this.held && this.item.transform.position.y > Player.local.creature.ragdoll.headPart.transform.position.y + 0.1f && !this.imbued)
			{
				RagdollHand mainHandler = this.item.mainHandler;
				if (((mainHandler != null) ? mainHandler.creature : null) == Player.currentCreature)
				{
					flag = this.airCheck == null;
					goto IL_80;
				}
			}
			flag = false;
			IL_80:
			bool flag2 = flag;
			if (flag2)
			{
				this.airCheck = GameManager.local.StartCoroutine(this.AirCheck());
			}
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00003A27 File Offset: 0x00001C27
		public IEnumerator AirCheck()
		{
			float time = this.airDuration;
			while (time > 0f)
			{
				time -= Time.deltaTime;
				bool flag = this.item.transform.position.y < Player.local.creature.ragdoll.headPart.transform.position.y + 0.1f || !this.item.mainHandler;
				if (flag)
				{
					this.airCheck = null;
					yield break;
				}
				yield return null;
			}
			this.imbued = true;
			this.canPound = true;
			foreach (Imbue imbue in this.item.imbues)
			{
				imbue.Transfer(this.lightningSpell, imbue.maxEnergy);
				imbue = null;
			}
			List<Imbue>.Enumerator enumerator = default(List<Imbue>.Enumerator);
			EffectInstance BoltEffect = Catalog.GetData<EffectData>("SpellLightningBolt", true).Spawn(this.item.transform, true, null, false, Array.Empty<Type>());
			GameObject strike = new GameObject();
			strike.transform.position = Player.local.creature.ragdoll.headPart.transform.position + new Vector3(0f, 50f, 0f);
			int num;
			for (int bolts = 0; bolts < 35; bolts = num + 1)
			{
				BoltEffect.SetSource(strike.transform);
				BoltEffect.SetTarget(this.audioSource.transform);
				BoltEffect.Play(0, false);
				num = bolts;
			}
			this.audioSource.Play();
			this.airCheck = null;
			yield break;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00003A36 File Offset: 0x00001C36
		private void OnUnGrab(Handle handle, RagdollHand ragdollHand, bool throwing)
		{
			this.held = false;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00003A3F File Offset: 0x00001C3F
		private void OnGrab(Handle handle, RagdollHand ragdollHand)
		{
			this.held = true;
		}

		// Token: 0x0400003E RID: 62
		public float explosionForce = 7f;

		// Token: 0x0400003F RID: 63
		public float explosionRange = 10f;

		// Token: 0x04000040 RID: 64
		public float airDuration = 2f;

		// Token: 0x04000041 RID: 65
		private Item item;

		// Token: 0x04000042 RID: 66
		private SpellCastCharge lightningSpell;

		// Token: 0x04000043 RID: 67
		private bool canPound;

		// Token: 0x04000044 RID: 68
		private bool imbued;

		// Token: 0x04000045 RID: 69
		private bool held;

		// Token: 0x04000046 RID: 70
		private AudioSource audioSource;

		// Token: 0x04000047 RID: 71
		private Coroutine airCheck;
	}
}
