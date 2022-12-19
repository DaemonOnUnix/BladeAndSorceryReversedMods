using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThunderRoad;
using UnityEngine;

namespace SoulFireSpell
{
	// Token: 0x02000009 RID: 9
	public class SFLightningMerge : SpellMergeData
	{
		// Token: 0x06000013 RID: 19 RVA: 0x00002B4C File Offset: 0x00000D4C
		public override void Merge(bool active)
		{
			base.Merge(active);
			Vector3 from = Player.local.transform.rotation * PlayerControl.GetHand(1).GetHandVelocity();
			Vector3 from2 = Player.local.transform.rotation * PlayerControl.GetHand(0).GetHandVelocity();
			bool flag = (double)from.magnitude <= (double)SpellCaster.throwMinHandVelocity || (double)from2.magnitude <= (double)SpellCaster.throwMinHandVelocity || ((double)Vector3.Angle(from, this.mana.casterLeft.magicSource.position - this.mana.mergePoint.position) >= 45.0 && (double)Vector3.Angle(from2, this.mana.casterRight.magicSource.position - this.mana.mergePoint.position) >= 45.0);
			if (!flag)
			{
				this.Zap(Player.currentCreature);
			}
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002C5C File Offset: 0x00000E5C
		public async void Zap(Creature startPoint)
		{
			List<Creature> nearbyCreatures = new List<Creature>();
			foreach (Rigidbody rigidbody in from i in Physics.OverlapSphere(startPoint.transform.position, 4f)
				select i.attachedRigidbody)
			{
				bool flag = rigidbody && rigidbody != Player.local.locomotion.rb;
				if (flag)
				{
					bool flag2 = rigidbody.gameObject.GetComponentInParent<Creature>() && rigidbody.gameObject.GetComponentInParent<Creature>() != Player.currentCreature && rigidbody.gameObject.GetComponentInParent<Creature>() != startPoint && !rigidbody.gameObject.GetComponentInParent<Creature>().gameObject.GetComponent<ZapMono>();
					if (flag2)
					{
						nearbyCreatures.Add(rigidbody.gameObject.GetComponentInParent<Creature>());
					}
				}
				rigidbody = null;
			}
			IEnumerator<Rigidbody> enumerator = null;
			Creature creature = (from creature2 in nearbyCreatures
				where !creature2.isPlayer && creature2
				orderby (creature2.transform.position - startPoint.transform.position).sqrMagnitude
				select creature2).First<Creature>();
			bool flag3 = creature;
			if (flag3)
			{
				EffectInstance BoltEffect = Catalog.GetData<EffectData>("SpellLightningBolt", true).Spawn(startPoint.transform, true, null, false, Array.Empty<Type>());
				BoltEffect.SetSource(startPoint.ragdoll.rootPart.transform);
				BoltEffect.SetTarget(creature.ragdoll.rootPart.transform);
				creature.gameObject.AddComponent<ZapMono>();
				BoltEffect.Play(0, false);
				creature.TryElectrocute(10f, 4f, true, false, Catalog.GetData<EffectData>("ImbueLightningRagdoll", true));
				DamageStruct zapDamage = new DamageStruct(4, 20f);
				zapDamage.hitRagdollPart = creature.ragdoll.rootPart;
				creature.Damage(new CollisionInstance(zapDamage, null, null));
				bool isKilled = creature.isKilled;
				if (isKilled)
				{
					SoulFirePossess.SoulResurrect(creature);
				}
				await Task.Delay(500);
				this.Zap(creature);
				await Task.Delay(5000);
				CompMono.DestroyComp(creature);
				BoltEffect = null;
				zapDamage = default(DamageStruct);
			}
		}
	}
}
