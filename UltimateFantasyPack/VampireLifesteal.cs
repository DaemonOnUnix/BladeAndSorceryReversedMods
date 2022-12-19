using System;
using System.Linq;
using ThunderRoad;
using UnityEngine;

namespace UltimateFantasyPack
{
	// Token: 0x02000028 RID: 40
	public class VampireLifesteal : MonoBehaviour
	{
		// Token: 0x0600009D RID: 157 RVA: 0x00004E78 File Offset: 0x00003078
		private void Start()
		{
			this._item = base.GetComponent<Item>();
			this._activationAudio = this._item.GetCustomReference("activationAudio", true).gameObject.GetComponent<AudioSource>();
			this._readyAudio = this._item.GetCustomReference("readyAudio", true).gameObject.GetComponent<AudioSource>();
			this._cooldown = base.gameObject.AddComponent<Cooldown>();
			this._duration = base.gameObject.AddComponent<Timer>();
			this._cooldown.cooldownLength = 20f;
			this._duration.TimerDuration = 10f;
			this._cooldown.onCooldownEnd = delegate()
			{
				this._readyAudio.Play(0UL);
			};
			this._duration.AlwaysActive = false;
			this._duration.DestroyOnFinish = false;
			this._duration.OnTimerEnd = new Action(this.UnImbue);
			this._imbued = false;
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00004F64 File Offset: 0x00003164
		private void Update()
		{
			bool flag = !this._imbued;
			if (!flag)
			{
				foreach (Imbue imbue in this._item.imbues)
				{
					imbue.Transfer(Catalog.GetData<SpellCastCharge>("Vampiric", true).Clone(), imbue.maxEnergy);
				}
			}
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00004FE4 File Offset: 0x000031E4
		private void Imbue()
		{
			this._imbued = true;
			this._duration.AlwaysActive = true;
			EventManager.onCreatureSpawn += new EventManager.CreatureSpawnedEvent(this.OnCreatureSpawn);
			Creature.allActive.Where((Creature creature) => creature.GetComponent<Player>() == null).ToList<Creature>().ForEach(delegate(Creature creature)
			{
				creature.OnDamageEvent += new Creature.DamageEvent(this.OnDamage);
			});
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00005057 File Offset: 0x00003257
		private void OnCreatureSpawn(Creature creature)
		{
			creature.OnDamageEvent += new Creature.DamageEvent(this.OnDamage);
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x0000506C File Offset: 0x0000326C
		private void UnImbue()
		{
			this._cooldown.StartCooldown();
			this._imbued = false;
			this._duration.AlwaysActive = false;
			this._duration.ResetTimer();
			foreach (Imbue imbue in this._item.imbues)
			{
				imbue.energy = 0f;
			}
			EventManager.onCreatureSpawn += new EventManager.CreatureSpawnedEvent(this.OnCreatureSpawn);
			Creature.allActive.Where((Creature creature) => creature.GetComponent<Player>() == null).ToList<Creature>().ForEach(delegate(Creature creature)
			{
				creature.OnDamageEvent -= new Creature.DamageEvent(this.OnDamage);
			});
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00005148 File Offset: 0x00003348
		private void OnDamage(CollisionInstance collisioninstance)
		{
			bool flag = !collisioninstance.targetCollider.GetComponent<Creature>() || collisioninstance.targetCollider.GetComponent<Player>() || !collisioninstance.sourceCollider.GetComponent<VampireLifesteal>() || !this._imbued;
			if (!flag)
			{
				Debug.Log("Healing player for " + (collisioninstance.damageStruct.damage * 0.3f).ToString());
				Player.currentCreature.Heal(collisioninstance.damageStruct.damage * 0.3f, null);
			}
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x000051E4 File Offset: 0x000033E4
		private void OnHeld(RagdollHand ragdollHand, Handle handle, Interactable.Action action)
		{
			bool flag = action != 2 || this._cooldown.IsOnCooldown || this._imbued;
			if (!flag)
			{
				this.Imbue();
			}
		}

		// Token: 0x04000073 RID: 115
		private bool _imbued;

		// Token: 0x04000074 RID: 116
		private Item _item;

		// Token: 0x04000075 RID: 117
		private Cooldown _cooldown;

		// Token: 0x04000076 RID: 118
		private Timer _duration;

		// Token: 0x04000077 RID: 119
		private AudioSource _activationAudio;

		// Token: 0x04000078 RID: 120
		private AudioSource _readyAudio;
	}
}
