using System;
using ThunderRoad;
using UnityEngine;

namespace SoulFireSpell
{
	// Token: 0x02000003 RID: 3
	public class SoulFirePossess : SpellCastProjectile
	{
		// Token: 0x06000002 RID: 2 RVA: 0x0000205C File Offset: 0x0000025C
		public static void SoulResurrect(Creature creature)
		{
			creature.SetColor(new Color(0f, 0.8f, 1f, 1f), 0, true);
			creature.SetColor(new Color(1f, 1f, 1f), 3, true);
			creature.SetColor(new Color(1f, 1f, 1f), 4, true);
			creature.SetFaction(2);
			creature.Resurrect(creature.maxHealth, Player.currentCreature);
			creature.brain.Load(creature.brain.instance.id);
			creature.brain.canDamage = true;
			bool flag = !creature.gameObject.GetComponent<ResurrectedCreature>();
			if (flag)
			{
				creature.gameObject.AddComponent<ResurrectedCreature>();
			}
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002130 File Offset: 0x00000330
		protected override void OnProjectileCollision(ItemMagicProjectile projectile, CollisionInstance collisionInstance)
		{
			base.OnProjectileCollision(projectile, collisionInstance);
			bool flag = collisionInstance.damageStruct.hitRagdollPart;
			if (flag)
			{
				Creature creature = collisionInstance.targetCollider.attachedRigidbody.GetComponentInParent<Creature>();
				bool flag2 = creature;
				if (flag2)
				{
					SoulFirePossess.SoulResurrect(creature);
				}
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002184 File Offset: 0x00000384
		public override bool OnImbueCollisionStart(CollisionInstance collisionInstance)
		{
			base.OnImbueCollisionStart(collisionInstance);
			RagdollPart hitRagdollPart = collisionInstance.damageStruct.hitRagdollPart;
			Creature creature = ((hitRagdollPart != null) ? hitRagdollPart.ragdoll.creature : null);
			bool flag = creature && collisionInstance.damageStruct.damageType == 1;
			if (flag)
			{
				bool flag2 = creature != Player.currentCreature;
				if (flag2)
				{
					SoulFirePossess.SoulResurrect(creature);
				}
				else
				{
					creature.SetColor(new Color(0f, 0.8f, 1f, 1f), 0, this.updateProperties = true);
					creature.SetColor(new Color(1f, 1f, 1f, 1f), 3, this.updateProperties = true);
					creature.SetColor(new Color(1f, 1f, 1f, 1f), 4, this.updateProperties = true);
					creature.maxHealth = 300f;
					creature.Heal(300f, Player.currentCreature);
					creature.mana.maxMana = 300f;
					creature.mana.maxFocus = 300f;
				}
			}
			return false;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000022BC File Offset: 0x000004BC
		public override bool OnCrystalSlam(CollisionInstance collisionInstance)
		{
			this.Xpos = 0.7f;
			for (int i = 0; i <= 1; i++)
			{
				Random random = new Random();
				string creatureId = ((random.Next(1, 101) <= GameManager.options.maleRatio) ? "HumanMale" : "HumanFemale");
				CreatureData creatureData = Catalog.GetData<CreatureData>(creatureId, true);
				float rotation = Player.currentCreature.transform.rotation.eulerAngles.y;
				Vector3 position = Player.local.transform.position + new Vector3(this.Xpos, 0f, 0f);
				GameManager.local.StartCoroutine(creatureData.SpawnCoroutine(position, rotation, null, delegate(Creature spawnedCreature)
				{
					spawnedCreature.SetFaction(2);
					spawnedCreature.SetColor(new Color(0f, 0.8f, 1f, 1f), 0, this.updateProperties = true);
					spawnedCreature.SetColor(new Color(1f, 1f, 1f), 3, this.updateProperties = true);
					spawnedCreature.SetColor(new Color(1f, 1f, 1f), 4, this.updateProperties = true);
					spawnedCreature.gameObject.AddComponent<ResurrectedCreature>();
					spawnedCreature.brain.canDamage = true;
				}, true, null));
				this.Xpos -= 1.4f;
			}
			this.Xpos = 0.7f;
			this.imbue.ConsumeInstant(1000f);
			return true;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000023C8 File Offset: 0x000005C8
		public override bool OnCrystalUse(RagdollHand hand, bool active)
		{
			EffectInstance staffUseEffect = Catalog.GetData<EffectData>("SFStaffUse", true).Spawn(this.imbue.colliderGroup.imbueShoot, true, null, false, Array.Empty<Type>());
			staffUseEffect.Play(0, false);
			foreach (Creature creature in Creature.allActive)
			{
				bool flag = creature.gameObject.GetComponent<ResurrectedCreature>();
				if (flag)
				{
					creature.Kill();
				}
			}
			this.imbue.ConsumeInstant(1000f);
			return true;
		}

		// Token: 0x04000001 RID: 1
		public string itemID;

		// Token: 0x04000002 RID: 2
		public bool updateProperties;

		// Token: 0x04000003 RID: 3
		public float Xpos;

		// Token: 0x04000004 RID: 4
		public bool isNeckGrabbed = true;

		// Token: 0x04000005 RID: 5
		public Creature creature;
	}
}
