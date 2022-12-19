using System;
using System.Collections;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

namespace UltimateFantasyPack
{
	// Token: 0x02000014 RID: 20
	public class Ancient : ItemModule
	{
		// Token: 0x06000048 RID: 72 RVA: 0x000032D0 File Offset: 0x000014D0
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			item.collisionHandlers.ForEach(delegate(CollisionHandler i)
			{
				i.OnCollisionStartEvent += new CollisionHandler.CollisionEvent(this.Collision);
			});
			item.OnHeldActionEvent += new Item.HeldActionDelegate(this.HeldAction);
			bool flag = !string.IsNullOrEmpty(this.forceAncientImbueSpellID);
			if (flag)
			{
				this.spellData = Catalog.GetData<SpellCastCharge>(this.forceAncientImbueSpellID, true);
			}
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00003338 File Offset: 0x00001538
		private void HeldAction(RagdollHand ragdollHand, Handle handle, Interactable.Action action)
		{
			bool flag = action == 2 && !this.cooldown && !this.active;
			if (flag)
			{
				GameManager.local.StartCoroutine(this.CooldownCoroutine());
			}
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00003374 File Offset: 0x00001574
		private void Collision(CollisionInstance collisionInstance)
		{
			Creature creature;
			bool flag;
			if (this.active)
			{
				RagdollPart hitRagdollPart = collisionInstance.damageStruct.hitRagdollPart;
				Creature creature2;
				if (hitRagdollPart == null)
				{
					creature2 = null;
				}
				else
				{
					Ragdoll ragdoll = hitRagdollPart.ragdoll;
					creature2 = ((ragdoll != null) ? ragdoll.creature : null);
				}
				creature = creature2;
				if (creature != null)
				{
					flag = collisionInstance.damageStruct.damage > this.minDamageForKill;
					goto IL_46;
				}
			}
			flag = false;
			IL_46:
			bool flag2 = flag;
			if (flag2)
			{
				creature.Kill();
			}
		}

		// Token: 0x0600004B RID: 75 RVA: 0x000033D2 File Offset: 0x000015D2
		private IEnumerator CooldownCoroutine()
		{
			this.active = true;
			bool flag = this.spellData == null;
			if (flag)
			{
				yield return new WaitForSeconds(this.abilityDuration);
			}
			else
			{
				float abilityDuration = this.abilityDuration;
				while (abilityDuration > 0f)
				{
					abilityDuration -= Time.fixedDeltaTime;
					foreach (Imbue imbue in this.item.imbues)
					{
						imbue.Transfer(this.spellData, imbue.maxEnergy);
						imbue = null;
					}
					List<Imbue>.Enumerator enumerator = default(List<Imbue>.Enumerator);
					yield return null;
				}
			}
			this.active = false;
			this.cooldown = true;
			yield return new WaitForSeconds(this.cooldownDuration);
			this.cooldown = false;
			yield break;
		}

		// Token: 0x0400002F RID: 47
		public float cooldownDuration;

		// Token: 0x04000030 RID: 48
		public float abilityDuration;

		// Token: 0x04000031 RID: 49
		public float minDamageForKill;

		// Token: 0x04000032 RID: 50
		public string forceAncientImbueSpellID;

		// Token: 0x04000033 RID: 51
		private SpellCastCharge spellData;

		// Token: 0x04000034 RID: 52
		private bool active;

		// Token: 0x04000035 RID: 53
		private bool cooldown;
	}
}
