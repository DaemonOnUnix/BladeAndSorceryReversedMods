using System;
using System.Linq;
using ThunderRoad;
using UnityEngine;

namespace SoulFireSpell
{
	// Token: 0x02000006 RID: 6
	public class SFFireMerge : SpellMergeData
	{
		// Token: 0x0600000D RID: 13 RVA: 0x000026F8 File Offset: 0x000008F8
		public override void Merge(bool active)
		{
			base.Merge(active);
			Vector3 from = Player.local.transform.rotation * PlayerControl.GetHand(1).GetHandVelocity();
			Vector3 from2 = Player.local.transform.rotation * PlayerControl.GetHand(0).GetHandVelocity();
			bool flag = (double)from.magnitude <= (double)SpellCaster.throwMinHandVelocity || (double)from2.magnitude <= (double)SpellCaster.throwMinHandVelocity || ((double)Vector3.Angle(from, this.mana.casterLeft.magicSource.position - this.mana.mergePoint.position) >= 45.0 && (double)Vector3.Angle(from2, this.mana.casterRight.magicSource.position - this.mana.mergePoint.position) >= 45.0);
			if (!flag)
			{
				foreach (Creature creature in Creature.allActive)
				{
					bool flag2 = Creature.allActive == null;
					if (flag2)
					{
						break;
					}
					bool isKilled = creature.isKilled;
					if (isKilled)
					{
						Vector3 pos = creature.ragdoll.rootPart.transform.position;
						GameObject go = new GameObject();
						go.transform.position = pos;
						EffectInstance explosionEffect = Catalog.GetData<EffectData>("SFFireExplosion", true).Spawn(go.transform, true, null, false, Array.Empty<Type>());
						creature.Despawn();
						explosionEffect.Play(0, false);
						GOMono.DestroyGo(go);
						foreach (Rigidbody rigidbody in from i in Physics.OverlapSphere(pos, 2.5f)
							select i.attachedRigidbody)
						{
							bool flag3 = rigidbody && rigidbody != Player.local.locomotion.rb;
							if (flag3)
							{
								rigidbody.AddForce((rigidbody.transform.position - creature.transform.position).normalized * 2.25f * rigidbody.mass, 1);
								Creature componentInParent = rigidbody.gameObject.GetComponentInParent<Creature>();
								bool flag4 = componentInParent && !componentInParent.isPlayer;
								if (flag4)
								{
									componentInParent.Kill();
									foreach (RagdollPart part in componentInParent.ragdoll.parts)
									{
										int random = Random.Range(1, 5);
										bool flag5 = random == 1;
										if (flag5)
										{
											part.ragdoll.physicToggle = true;
											bool sliceAllowed = part.sliceAllowed;
											if (sliceAllowed)
											{
												part.ragdoll.TrySlice(part);
												part.rb.AddForce((rigidbody.transform.position - creature.transform.position).normalized * 2.25f * rigidbody.mass, 1);
											}
											part.ragdoll.physicToggle = false;
										}
									}
								}
							}
						}
					}
				}
			}
		}
	}
}
