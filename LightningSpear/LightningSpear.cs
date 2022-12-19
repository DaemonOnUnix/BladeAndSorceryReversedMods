using System;
using System.Linq;
using ThunderRoad;
using UnityEngine;

namespace LightningSpear
{
	// Token: 0x02000002 RID: 2
	public class LightningSpear : SpellCastCharge
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public override void Load(SpellCaster spellCaster, SpellData.Level level)
		{
			Catalog.LoadAssetAsync<GameObject>("LightningExplosionSFX", new Action<GameObject>(this.Setup), "HandlerName");
			base.Load(spellCaster, level);
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002078 File Offset: 0x00000278
		private void Setup(GameObject gameObject)
		{
			this.lightningSFX = gameObject;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002084 File Offset: 0x00000284
		public override void Fire(bool active)
		{
			base.Fire(active);
			bool flag = !active && this.currentCharge > 0.95f;
			if (flag)
			{
				Catalog.GetData<ItemData>("LightningSpear", true).SpawnAsync(new Action<Item>(this.Setup), null, null, null, true, null);
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020E8 File Offset: 0x000002E8
		private void Setup(Item item)
		{
			item.transform.position = this.spellCaster.ragdollHand.transform.position;
			this.spellCaster.ragdollHand.Grab(item.GetMainHandle(this.spellCaster.ragdollHand.side));
			item.mainCollisionHandler.OnCollisionStartEvent += new CollisionHandler.CollisionEvent(this.MainCollisionHandler_OnCollisionStartEvent);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002158 File Offset: 0x00000358
		private void MainCollisionHandler_OnCollisionStartEvent(CollisionInstance collisionInstance)
		{
			bool flag = !collisionInstance.targetCollider.GetComponentInParent<Player>();
			if (flag)
			{
				Catalog.GetData<EffectData>("LightningExplosionVFX", true).Spawn(collisionInstance.contactPoint, Quaternion.identity, null, null, true, null, false, Array.Empty<Type>()).Play(0, false);
				GameObject ExplosionSFX = Object.Instantiate<GameObject>(this.lightningSFX);
				ExplosionSFX.GetComponentInChildren<AudioSource>().Play();
				collisionInstance.sourceCollider.GetComponentInParent<Item>().Despawn();
				this.Explode(collisionInstance.contactPoint);
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000021E4 File Offset: 0x000003E4
		private void Explode(Vector3 position)
		{
			foreach (Rigidbody rb in from i in Physics.OverlapSphere(position, 3f)
				select i.attachedRigidbody)
			{
				bool flag = rb && rb != Player.local.locomotion.rb;
				if (flag)
				{
					rb.AddForce((rb.transform.position - position).normalized * 5f * rb.mass, 1);
					Creature creature = rb.gameObject.GetComponentInParent<Creature>();
					bool flag2 = creature && !creature.isPlayer;
					if (flag2)
					{
						creature.ragdoll.SetState(1, false);
						this.ElectrocuteCreature(creature);
					}
				}
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000022FC File Offset: 0x000004FC
		private void ElectrocuteCreature(Creature creature)
		{
			creature.brain.instance.GetModule<BrainModuleElectrocute>(true).TryElectrocute(10f, 5f, true, false, Catalog.GetData<EffectData>("ImbueLightningRagdoll", true));
		}

		// Token: 0x04000001 RID: 1
		private GameObject lightningSFX;
	}
}
