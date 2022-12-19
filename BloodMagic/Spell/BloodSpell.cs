using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BloodMagic.Spell.Abilities;
using BloodMagic.UI;
using ThunderRoad;
using UnityEngine;

namespace BloodMagic.Spell
{
	// Token: 0x02000017 RID: 23
	public class BloodSpell : SpellCastCharge
	{
		// Token: 0x0600007D RID: 125 RVA: 0x00003578 File Offset: 0x00001778
		public override void Init()
		{
			EventManager.onPossess += new EventManager.PossessEvent(this.EventManager_onPossess);
			base.Init();
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00003594 File Offset: 0x00001794
		private void EventManager_onPossess(Creature creature, EventTime eventTime)
		{
			bool flag = creature.container.contents.Where((ContainerData.Content c) => c.itemData.id == "SpellBloodItem").Count<ContainerData.Content>() <= 0;
			if (flag)
			{
				creature.container.AddContent(Catalog.GetData<ItemData>("SpellBloodItem", true), null);
			}
			creature.OnDamageEvent += new Creature.DamageEvent(this.Creature_OnDamageEvent);
			BloodDrain.DrainHealth(0f, this, null);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00003618 File Offset: 0x00001818
		private void Creature_OnDamageEvent(CollisionInstance collisionInstance)
		{
			BloodSpell.lastDamageTime = Time.time;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00003624 File Offset: 0x00001824
		public override void Throw(Vector3 velocity)
		{
			base.Throw(velocity);
			this.leftVel = Player.local.transform.rotation * PlayerControl.GetHand(1).GetHandVelocity();
			this.rightVel = Player.local.transform.rotation * PlayerControl.GetHand(0).GetHandVelocity();
			bool flag = BloodBulletAbility.TryToActivate(this, velocity, BookUIHandler.saveData);
			if (!flag)
			{
				BloodDaggerAbility.TryToActivate(this, velocity, BookUIHandler.saveData);
			}
		}

		// Token: 0x06000081 RID: 129 RVA: 0x000036A4 File Offset: 0x000018A4
		public override void Fire(bool active)
		{
			base.Fire(active);
			if (!active)
			{
				this.currentCharge = 0f;
			}
		}

		// Token: 0x06000082 RID: 130 RVA: 0x000036CC File Offset: 0x000018CC
		public override void UpdateCaster()
		{
			base.UpdateCaster();
			bool flag = this.spellCaster.ragdollHand.side == 1;
			if (flag)
			{
				BloodDrain.drainEffectLeft.SetVector3("AttractiveTargetPosition", this.spellCaster.magic.position);
			}
			else
			{
				BloodDrain.drainEffectRight.SetVector3("AttractiveTargetPosition", this.spellCaster.magic.position);
			}
			bool flag2 = SkillHandler.IsSkillUnlocked("Healing") && (double)Time.time - (double)BloodSpell.lastDamageTime > 5.0;
			if (flag2)
			{
				Creature creature = this.spellCaster.ragdollHand.creature;
				bool flag3 = (double)creature.currentHealth < (double)creature.maxHealth;
				if (flag3)
				{
					creature.Heal(BookUIHandler.saveData.drainPower * 0.2f * Time.deltaTime, creature);
				}
			}
			bool flag4 = SkillHandler.IsSkillUnlocked("Choke Drain");
			if (flag4)
			{
				Handle grabbedHandle = this.spellCaster.ragdollHand.grabbedHandle;
				bool flag5 = grabbedHandle is HandleRagdoll && (double)PlayerControl.GetHand(this.spellCaster.ragdollHand.side).useAxis > 0.0;
				if (flag5)
				{
					Creature creature2 = (grabbedHandle as HandleRagdoll).ragdollPart.ragdoll.creature;
					bool flag6 = !creature2.isKilled;
					if (flag6)
					{
						float num = (float)((double)BookUIHandler.saveData.drainPower * (double)Time.deltaTime * 2.0);
						CollisionInstance collisionInstance = new CollisionInstance(new DamageStruct(4, num), null, null);
						creature2.Damage(collisionInstance);
						BloodSpell.lastDamageTime = Time.time;
						BloodDrain.DrainHealth(num, this, creature2);
					}
				}
			}
			bool flag7 = (double)this.currentCharge > 0.800000011920929;
			if (flag7)
			{
				bool flag8 = BloodDrain.TryToActivate(this, Vector3.zero, BookUIHandler.saveData);
				if (flag8)
				{
					bool flag9 = this.spellCaster.ragdollHand.side == 1;
					if (flag9)
					{
						BloodDrain.drainEffectLeft.Play();
					}
					else
					{
						BloodDrain.drainEffectRight.Play();
					}
				}
				else
				{
					bool flag10 = this.spellCaster.ragdollHand.side == 1;
					if (flag10)
					{
						BloodDrain.drainEffectLeft.Stop();
					}
					else
					{
						BloodDrain.drainEffectRight.Stop();
					}
				}
				bool flag11 = !this.swordSpawned && SkillHandler.IsSkillUnlocked("Blood Sword") && BloodSword.TryToActivate(this, Vector3.zero, BookUIHandler.saveData);
				if (flag11)
				{
					this.spellCaster.StartCoroutine(this.SpawnSword());
					BloodSpell.lastDamageTime = Time.time;
				}
				bool flag12 = !this.bowSpawned && SkillHandler.IsSkillUnlocked("Blood Bow") && BloodBow.TryToActivate(this, Vector3.zero, BookUIHandler.saveData);
				if (flag12)
				{
					this.spellCaster.StartCoroutine(this.SpawnBow());
					BloodSpell.lastDamageTime = Time.time;
				}
				bool flag13 = BloodWave.waveCreated || !SkillHandler.IsSkillUnlocked("Blood Wave") || !BloodWave.TryToActivate(this, Vector3.zero, BookUIHandler.saveData);
				if (!flag13)
				{
					this.spellCaster.StartCoroutine(this.SpawnBloodWave());
					BloodSpell.lastDamageTime = Time.time;
				}
			}
			else
			{
				bool flag14 = this.spellCaster.ragdollHand.side == 1;
				if (flag14)
				{
					BloodDrain.drainEffectLeft.Stop();
				}
				else
				{
					BloodDrain.drainEffectRight.Stop();
				}
			}
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00003A33 File Offset: 0x00001C33
		private IEnumerator SpawnBloodWave()
		{
			EffectInstance effectInstance = Catalog.GetData<EffectData>("BloodWave", true).Spawn(Player.currentCreature.locomotion.transform.position + Player.currentCreature.transform.forward * 2f, Quaternion.LookRotation(Player.currentCreature.transform.forward), null, null, true, Array.Empty<Type>());
			float startTime = Time.time;
			GameObject wave = effectInstance.effects[0].gameObject;
			while ((double)Time.time - (double)startTime < 2.0)
			{
				wave.transform.position += wave.transform.forward * 2f * Time.deltaTime;
				foreach (Creature creature in Creature.list)
				{
					Creature creature2 = creature;
					bool flag = creature2 != Player.currentCreature && (double)Vector3.Distance(wave.transform.position, creature2.transform.position) < 3.0;
					if (flag)
					{
						creature2.ragdoll.SetState(1);
						creature2.ragdoll.GetPart(new RagdollPart.Type[] { 3 }).rb.AddForce((wave.transform.forward + Vector3.up) * BookUIHandler.saveData.wavePushStrenght, 1);
					}
					creature2 = null;
					creature2 = null;
					creature = null;
				}
				List<Creature>.Enumerator enumerator = default(List<Creature>.Enumerator);
				yield return new WaitForEndOfFrame();
			}
			BloodWave.waveCreated = false;
			effectInstance.Despawn();
			yield break;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00003A42 File Offset: 0x00001C42
		private IEnumerator SpawnBow()
		{
			this.bowSpawned = true;
			yield return new WaitForSeconds(0.2f);
			Catalog.GetData<ItemData>("BloodBow", true).SpawnAsync(delegate(Item bow)
			{
				Handle handle = bow.handles.First((Handle h) => h.interactableId == "ObjectHandleBow");
				this.spellCaster.ragdollHand.Grab(handle, true);
				Catalog.GetData<ItemData>("BloodArrow", true).SpawnAsync(delegate(Item arrow)
				{
					BowString component = bow.GetCustomReference("StringHandle").gameObject.GetComponent<BowString>();
					Handle handle2 = arrow.handles.First((Handle h) => h.interactableId == "ObjectHandleArrowBack");
					this.spellCaster.other.ragdollHand.Grab(handle2, true);
					this.spellCaster.other.spellInstance.Fire(false);
					(this.spellCaster.other.spellInstance as BloodSpell).currentCharge = 0f;
					this.spellCaster.other.isFiring = false;
					component.NockArrow(handle2);
					bow.handles.First((Handle h) => h.interactableId == "ObjectHandleBowString").Grabbed += new Handle.GrabEvent(this.BowHandle_Grabbed);
					this.tryarrow = true;
				}, null, null, null, false, null);
				this.Fire(false);
				this.currentCharge = 0f;
				this.spellCaster.isFiring = false;
				handle.UnGrabbed += new Handle.GrabEvent(this.L_MainHandle_UnGrabbed);
			}, new Vector3?(this.spellCaster.magic.position), new Quaternion?(Quaternion.LookRotation(this.spellCaster.magic.up, this.spellCaster.magic.right)), null, false, null);
			yield break;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00003A51 File Offset: 0x00001C51
		private void BowHandle_Grabbed(RagdollHand ragdollHand, Handle handle, EventTime eventTime)
		{
			this.spellCaster.StartCoroutine(this.SpawnArrowInBow(ragdollHand, handle));
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00003A67 File Offset: 0x00001C67
		private IEnumerator SpawnArrowInBow(RagdollHand ragdollHand, Handle handle)
		{
			yield return new WaitForEndOfFrame();
			Item bow = handle.item;
			BowString bowString = bow.GetCustomReference("StringHandle").gameObject.GetComponent<BowString>();
			bool flag = bowString.nockedArrow == null && bowString.restedArrow == null && this.tryarrow;
			if (flag)
			{
				this.tryarrow = false;
				ragdollHand.UnGrab(false);
				bool flag2 = SpellAbilityManager.HasEnoughHealth(2f);
				if (flag2)
				{
					Catalog.GetData<ItemData>("BloodArrow", true).SpawnAsync(delegate(Item arrow)
					{
						Handle handle2 = arrow.handles.First((Handle h) => h.interactableId == "ObjectHandleArrowBack");
						ragdollHand.Grab(handle2, true);
						bowString.NockArrow(handle2);
						SpellAbilityManager.SpendHealth(2f);
						this.tryarrow = true;
					}, null, null, null, false, null);
				}
			}
			yield break;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00003A84 File Offset: 0x00001C84
		private void L_MainHandle_UnGrabbed(RagdollHand ragdollHand, Handle handle, EventTime eventTime)
		{
			bool flag = handle.handlers.Count > 0;
			if (!flag)
			{
				this.bowSpawned = false;
				handle.UnGrabbed -= new Handle.GrabEvent(this.L_MainHandle_UnGrabbed);
				handle.gameObject.SetActive(false);
				handle.item.Despawn(0.2f);
			}
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00003ADE File Offset: 0x00001CDE
		private IEnumerator SpawnSword()
		{
			this.swordSpawned = true;
			yield return new WaitForSeconds(0.2f);
			Catalog.GetData<ItemData>("BloodSword", true).SpawnAsync(delegate(Item sword)
			{
				Handle mainHandle = sword.GetMainHandle(this.spellCaster.ragdollHand.side);
				this.spellCaster.ragdollHand.Grab(mainHandle, true);
				this.Fire(false);
				this.spellCaster.isFiring = false;
				this.currentCharge = 0f;
				mainHandle.UnGrabbed += new Handle.GrabEvent(this.L_handle_UnGrabbed);
			}, new Vector3?(this.spellCaster.magic.position), new Quaternion?(Quaternion.LookRotation(this.spellCaster.magic.up, -this.spellCaster.magic.right)), null, false, null);
			yield break;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00003AF0 File Offset: 0x00001CF0
		private void L_handle_UnGrabbed(RagdollHand ragdollHand, Handle handle, EventTime eventTime)
		{
			bool flag = handle.handlers.Count > 0;
			if (!flag)
			{
				this.swordSpawned = false;
				handle.UnGrabbed -= new Handle.GrabEvent(this.L_handle_UnGrabbed);
				handle.gameObject.SetActive(false);
				handle.item.Despawn(0.2f);
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00003B4C File Offset: 0x00001D4C
		public static BloodSpell.AimStruct AimAssist(Vector3 ownPosition, Vector3 ownDirection, float aimPrecision, float randomness)
		{
			Creature creature = null;
			float num = -1f;
			Vector3 vector = Vector3.zero;
			foreach (Creature creature2 in Creature.list)
			{
				bool flag = creature2 != Player.currentCreature && !creature2.isKilled;
				if (flag)
				{
					Vector3 normalized = (creature2.ragdoll.GetPart(new RagdollPart.Type[] { 1 }).transform.position - ownPosition).normalized;
					bool flag2 = (double)Vector3.Dot(ownDirection, normalized) > (double)aimPrecision && (double)Vector3.Dot(ownDirection, normalized) > (double)num;
					if (flag2)
					{
						num = Vector3.Dot(ownDirection, normalized);
						creature = creature2;
						vector = normalized;
					}
				}
			}
			bool flag3 = !(creature != null);
			BloodSpell.AimStruct aimStruct;
			if (flag3)
			{
				aimStruct = new BloodSpell.AimStruct(ownDirection, null);
			}
			else
			{
				Vector3 vector2 = Random.insideUnitSphere * randomness;
				aimStruct = new BloodSpell.AimStruct((vector + vector2).normalized, creature);
			}
			return aimStruct;
		}

		// Token: 0x04000034 RID: 52
		private Vector3 leftVel;

		// Token: 0x04000035 RID: 53
		private Vector3 rightVel;

		// Token: 0x04000036 RID: 54
		public static float lastDamageTime;

		// Token: 0x04000037 RID: 55
		private bool swordSpawned;

		// Token: 0x04000038 RID: 56
		private bool bowSpawned;

		// Token: 0x04000039 RID: 57
		private bool tryarrow;

		// Token: 0x0200002D RID: 45
		public struct AimStruct
		{
			// Token: 0x060000DE RID: 222 RVA: 0x000061A0 File Offset: 0x000043A0
			public AimStruct(Vector3 p_aimDir, Creature p_creature)
			{
				this.aimDir = p_aimDir;
				this.toHit = p_creature;
			}

			// Token: 0x04000077 RID: 119
			public Vector3 aimDir;

			// Token: 0x04000078 RID: 120
			public Creature toHit;
		}
	}
}
