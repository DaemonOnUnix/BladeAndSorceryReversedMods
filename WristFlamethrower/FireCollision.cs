using System;
using System.Collections.Generic;
using System.Linq;
using RainyReignGames.RevealMask;
using ThunderRoad;
using UnityEngine;

namespace WristFlamethrower
{
	// Token: 0x02000004 RID: 4
	public class FireCollision : MonoBehaviour
	{
		// Token: 0x06000010 RID: 16 RVA: 0x000024E2 File Offset: 0x000006E2
		public void Start()
		{
			this.particleSystem = base.GetComponent<ParticleSystem>();
			this.collisionEvents = new List<ParticleCollisionEvent>();
			this.fireEffect = Catalog.GetData<EffectData>("ImbueFireRagdoll", true);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002510 File Offset: 0x00000710
		private void OnParticleCollision(GameObject other)
		{
			Debug.Log("Collision Added");
			int numCollisionEvents = ParticlePhysicsExtensions.GetCollisionEvents(this.particleSystem, other, this.collisionEvents);
			for (int i = 0; i < numCollisionEvents; i++)
			{
				Creature creature = other.GetComponentInParent<Creature>();
				bool flag = creature != null;
				if (flag)
				{
					bool flag2 = !creature.isPlayer;
					if (flag2)
					{
						creature.TryElectrocute(20f, 7f, true, true, this.fireEffect);
						creature.ragdoll.SetState(1);
						creature.locomotion.rb.AddForce(-creature.transform.forward * 200f, 1);
						bool flag3 = other.GetComponentInParent<RagdollPart>() || other.GetComponent<RagdollPart>();
						if (flag3)
						{
							RagdollPart rp = other.GetComponentInParent<RagdollPart>();
							DamageStruct damageStruct;
							damageStruct..ctor(4, 10f * Time.deltaTime);
							damageStruct.hitRagdollPart = rp;
							creature.Damage(new CollisionInstance(damageStruct, null, null)
							{
								contactPoint = this.collisionEvents[i].intersection,
								contactNormal = this.collisionEvents[i].intersection.normalized
							});
							FireCollision.DrawDecal(rp, this.collisionEvents[i].intersection, null, creature);
						}
					}
				}
				Item fireItem = other.GetComponentInParent<Item>();
				bool flag4 = fireItem != null;
				if (flag4)
				{
					foreach (Imbue imbue in fireItem.imbues)
					{
						SpellCastCharge fire = Catalog.GetData<SpellCastCharge>("Fire", true);
						imbue.Transfer(fire, 100f);
					}
				}
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002704 File Offset: 0x00000904
		private static void DrawDecal(RagdollPart rp, Vector3 hit, string customDecal, Creature creature)
		{
			EffectModuleReveal rem = (EffectModuleReveal)Catalog.GetData<EffectData>("HitBladeDecalFlesh", true).modules[5];
			List<RevealMaterialController> rmcs = new List<RevealMaterialController>();
			IEnumerable<Creature.RendererData> renderers = rp.renderers;
			Func<Creature.RendererData, bool> <>9__0;
			Func<Creature.RendererData, bool> func;
			if ((func = <>9__0) == null)
			{
				func = (<>9__0 = (Creature.RendererData renderer) => rem != null && renderer.revealDecal && ((renderer.revealDecal.type == null && rem.typeFilter.HasFlag(1)) || (renderer.revealDecal.type == 1 && rem.typeFilter.HasFlag(2)) || (renderer.revealDecal.type == 2 && rem.typeFilter.HasFlag(4))));
			}
			foreach (Creature.RendererData r in renderers.Where(func))
			{
				rmcs.Add(r.revealDecal.revealMaterialController);
			}
			Transform rev = new GameObject().transform;
			rev.position = hit;
			rev.rotation = Quaternion.LookRotation(hit);
			GameManager.local.StartCoroutine(RevealMaskProjection.ProjectAsync(rev.position + rev.forward * rem.offsetDistance, -rev.forward, rev.up, rem.depth, rem.maxSize, rem.maskTexture, rem.maxChannelMultiplier, rmcs, rem.revealData, null));
		}

		// Token: 0x04000005 RID: 5
		private ParticleSystem particleSystem;

		// Token: 0x04000006 RID: 6
		public List<ParticleCollisionEvent> collisionEvents;

		// Token: 0x04000007 RID: 7
		public float force = 10f;

		// Token: 0x04000008 RID: 8
		private EffectData fireEffect;
	}
}
