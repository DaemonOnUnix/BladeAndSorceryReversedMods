using System;
using System.Collections.Generic;
using System.Linq;
using ThunderRoad;
using UnityEngine;

namespace SpellCastIce
{
	// Token: 0x02000008 RID: 8
	public class SpellCastIce : SpellCastCharge
	{
		// Token: 0x06000028 RID: 40 RVA: 0x00002EA7 File Offset: 0x000010A7
		public override void Init()
		{
			Debug.LogError("Initted");
			EventManager.onPossess += new EventManager.PossessEvent(this.EventManager_onPossess);
			IceManager.LoadFromJSON();
			base.Init();
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002ED4 File Offset: 0x000010D4
		private void EventManager_onPossess(Creature creature, EventTime eventTime)
		{
			bool flag = creature.container.contents.Where((ContainerData.Content c) => c.itemData.id == "SpellIceItem").Count<ContainerData.Content>() <= 0;
			if (flag)
			{
				creature.container.AddContent(Catalog.GetData<ItemData>("SpellIceItem", true), null, null);
			}
			IceManager.LoadFromSave(creature);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002F40 File Offset: 0x00001140
		public static void IceStats()
		{
			Debug.Log("Level: " + IceManager.level.ToString());
			Debug.Log("XP: " + IceManager.xp.ToString() + " / " + IceManager.XpForNextLevel(IceManager.level).ToString());
			Debug.Log("Points to spend: " + IceManager.levelPoints.ToString());
			foreach (KeyValuePair<IceManager.AbilitiesEnum, IceManager.Ability> keyValuePair in IceManager.abilityDict)
			{
				Debug.Log(keyValuePair.Key.ToString() + "Unlocked: " + keyValuePair.Value.unlocked.ToString());
			}
			Debug.Log("ChargeSpeed: " + IceManager.chargeSpeed.ToString());
			Debug.Log("SpikeSpeed: " + IceManager.spikeSpeed.ToString());
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00003060 File Offset: 0x00001260
		public override void Throw(Vector3 velocity)
		{
			base.Throw(velocity);
			bool flag = this.spellCaster.ragdollHand.playerHand;
			if (flag)
			{
				PlayerControl.GetHand(this.spellCaster.ragdollHand.side).HapticPlayClip(Catalog.gameData.haptics.telekinesisThrow, 1f);
			}
			Catalog.GetData<ItemData>("IceSpike", true).SpawnAsync(delegate(Item iceSpike)
			{
				iceSpike.IgnoreRagdollCollision(this.spellCaster.mana.creature.ragdoll);
				bool flag2 = !IceManager.IsAbilityUnlocked(IceManager.AbilitiesEnum.iceSpikeAim);
				if (flag2)
				{
					iceSpike.rb.AddForce(velocity * IceManager.spikeSpeed, 1);
					iceSpike.transform.rotation = Quaternion.LookRotation(velocity.normalized);
				}
				else
				{
					Vector3 vector = this.AimAssist(iceSpike.transform.position, velocity.normalized, 0.7f, 0.01f);
					iceSpike.rb.AddForce(vector * velocity.magnitude * IceManager.spikeSpeed, 1);
					iceSpike.transform.rotation = Quaternion.LookRotation(vector);
				}
				iceSpike.Throw(1f, 2);
			}, new Vector3?(this.spellCaster.magic.position - this.spellCaster.magic.forward * 0.5f), null, null, false, null);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00003138 File Offset: 0x00001338
		private Vector3 AimAssist(Vector3 ownPosition, Vector3 ownDirection, float aimPrecision, float randomness)
		{
			Creature creature = null;
			float num = -1f;
			Vector3 vector = Vector3.zero;
			foreach (Creature creature2 in Creature.all)
			{
				bool flag = creature2 != Player.currentCreature && !creature2.isKilled;
				if (flag)
				{
					Vector3 normalized = (creature2.ragdoll.GetPart(1).transform.position - ownPosition).normalized;
					bool flag2 = Vector3.Dot(ownDirection, normalized) > aimPrecision;
					if (flag2)
					{
						bool flag3 = Vector3.Dot(ownDirection, normalized) > num;
						if (flag3)
						{
							num = Vector3.Dot(ownDirection, normalized);
							creature = creature2;
							vector = normalized;
						}
					}
				}
			}
			bool flag4 = creature != null;
			Vector3 vector3;
			if (flag4)
			{
				Vector3 vector2 = Random.insideUnitSphere * randomness;
				vector3 = (vector + vector2).normalized;
			}
			else
			{
				vector3 = ownDirection;
			}
			return vector3;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00003258 File Offset: 0x00001458
		public override bool OnImbueCollisionStart(CollisionInstance collisionInstance)
		{
			base.OnImbueCollisionStart(collisionInstance);
			bool flag = collisionInstance.damageStruct.hitRagdollPart;
			if (flag)
			{
				bool flag2 = collisionInstance.damageStruct.damage > 1f;
				if (flag2)
				{
					Creature componentInParent = collisionInstance.targetCollider.GetComponentInParent<Creature>();
					bool flag3 = componentInParent != Player.currentCreature && !componentInParent.isKilled;
					if (flag3)
					{
						bool flag4 = componentInParent.animator.speed == 1f;
						if (flag4)
						{
							bool flag5 = !componentInParent.GetComponent<IceSpellMWE>();
							if (flag5)
							{
								componentInParent.gameObject.AddComponent<IceSpellMWE>();
							}
							IceSpellMWE component = componentInParent.GetComponent<IceSpellMWE>();
							component.SlowStartCoroutine(componentInParent, collisionInstance.sourceColliderGroup.imbue.energy, 50f, 80f, 5f);
						}
					}
				}
			}
			return true;
		}

		// Token: 0x0400001C RID: 28
		public bool useLevelSystem = false;
	}
}
