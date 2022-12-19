using System;
using System.Collections;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

namespace UltimateFantasyPack
{
	// Token: 0x02000026 RID: 38
	public class Vampiric : MonoBehaviour
	{
		// Token: 0x06000093 RID: 147 RVA: 0x00004C08 File Offset: 0x00002E08
		public void Start()
		{
			this.item = base.GetComponent<Item>();
			this.vampSpell = Catalog.GetData<SpellCastCharge>("Vamp", true);
			this.item.OnDespawnEvent += new Item.SpawnEvent(this.Item_OnDespawnEvent);
			this.item.OnHeldActionEvent += new Item.HeldActionDelegate(this.Item_OnHeldActionEvent);
			this.activationAudio = this.item.GetCustomReference("activationAudio", true).gameObject.GetComponent<AudioSource>();
			this.bloodVFX = this.item.GetCustomReference("BloodVFX", true).gameObject;
			this.readyAudio = this.item.GetCustomReference("readyAudio", true).gameObject.GetComponent<AudioSource>();
			this.bloodVFX.SetActive(false);
			EventManager.onCreatureHit += new EventManager.CreatureHitEvent(this.EventManager_onCreatureHit);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00004CE0 File Offset: 0x00002EE0
		private void Item_OnHeldActionEvent(RagdollHand ragdollHand, Handle handle, Interactable.Action action)
		{
			bool flag = action == 2;
			if (flag)
			{
				GameManager.local.StartCoroutine(this.Imbue());
			}
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00004D0C File Offset: 0x00002F0C
		private void Item_OnDespawnEvent(EventTime eventTime)
		{
			bool flag = eventTime == 0;
			if (flag)
			{
				this.item.OnHeldActionEvent -= new Item.HeldActionDelegate(this.Item_OnHeldActionEvent);
				this.item.OnDespawnEvent -= new Item.SpawnEvent(this.Item_OnDespawnEvent);
				EventManager.onCreatureHit -= new EventManager.CreatureHitEvent(this.EventManager_onCreatureHit);
			}
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00004D68 File Offset: 0x00002F68
		private void EventManager_onCreatureHit(Creature creature, CollisionInstance collisionInstance)
		{
			bool flag = !this.imbued;
			if (!flag)
			{
				Player.currentCreature.Heal(collisionInstance.damageStruct.damage * 0.3f, Player.currentCreature);
			}
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00004DA8 File Offset: 0x00002FA8
		public void Update()
		{
			bool flag = !this.imbued;
			if (!flag)
			{
				foreach (Imbue imbue in this.item.imbues)
				{
					imbue.Transfer(this.vampSpell, imbue.maxEnergy);
				}
			}
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00004E20 File Offset: 0x00003020
		public IEnumerator Imbue()
		{
			bool flag = !this.onCooldown;
			if (flag)
			{
				this.onCooldown = true;
				this.activationAudio.Play();
				this.bloodVFX.SetActive(true);
				this.imbued = true;
				yield return new WaitForSeconds(10f);
				GameManager.local.StartCoroutine(this.Cooldown());
				this.bloodVFX.SetActive(false);
				this.imbued = false;
			}
			yield break;
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00004E2F File Offset: 0x0000302F
		public IEnumerator Cooldown()
		{
			foreach (Imbue imbue in this.item.imbues)
			{
				imbue.energy = 0f;
				imbue = null;
			}
			List<Imbue>.Enumerator enumerator = default(List<Imbue>.Enumerator);
			yield return new WaitForSeconds(20f);
			this.onCooldown = false;
			this.readyAudio.Play();
			yield break;
		}

		// Token: 0x0400006C RID: 108
		private bool imbued = false;

		// Token: 0x0400006D RID: 109
		private bool onCooldown = false;

		// Token: 0x0400006E RID: 110
		private Item item;

		// Token: 0x0400006F RID: 111
		private AudioSource activationAudio;

		// Token: 0x04000070 RID: 112
		private AudioSource readyAudio;

		// Token: 0x04000071 RID: 113
		private SpellCastCharge vampSpell;

		// Token: 0x04000072 RID: 114
		public GameObject bloodVFX;
	}
}
