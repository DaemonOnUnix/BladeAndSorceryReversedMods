using System;
using System.Collections;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

namespace Yamato
{
	// Token: 0x02000003 RID: 3
	public class JudgementCutHit : MonoBehaviour
	{
		// Token: 0x06000004 RID: 4 RVA: 0x00002114 File Offset: 0x00000314
		public void Start()
		{
			this.item = base.GetComponent<Item>();
			this.item.rb.isKinematic = true;
			Item.allThrowed.Add(this.item);
			base.StartCoroutine(this.AnimeSlice());
			GameObject gameObject = new GameObject();
			gameObject.transform.position = this.item.transform.position;
			gameObject.transform.rotation = Quaternion.identity;
			EffectInstance effectInstance = Catalog.GetData<EffectData>("JudgementCutHit", true).Spawn(gameObject.transform, false, null, false, Array.Empty<Type>());
			effectInstance.SetIntensity(2f);
			effectInstance.Play(0, false);
			Object.Destroy(gameObject, 2f);
			this.item.Despawn(5f);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000021E2 File Offset: 0x000003E2
		public IEnumerator AnimeSlice()
		{
			List<Creature> creaturesPushed = new List<Creature>();
			List<Rigidbody> rigidbodiesPushed = new List<Rigidbody>();
			yield return null;
			Collider[] array = Physics.OverlapSphere(this.item.transform.position, 1.5f);
			int i = 0;
			while (i < array.Length)
			{
				Collider collider = array[i];
				if (!(collider.GetComponentInParent<RagdollPart>() != null) || !(collider.GetComponentInParent<RagdollPart>().ragdoll.creature != Player.local.creature))
				{
					goto IL_167;
				}
				RagdollPart componentInParent = collider.GetComponentInParent<RagdollPart>();
				bool flag;
				if (componentInParent == null)
				{
					flag = false;
				}
				else
				{
					Ragdoll ragdoll = componentInParent.ragdoll;
					bool? flag2;
					if (ragdoll == null)
					{
						flag2 = null;
					}
					else
					{
						Creature creature = ragdoll.creature;
						if (creature == null)
						{
							flag2 = null;
						}
						else
						{
							GameObject gameObject = creature.gameObject;
							flag2 = ((gameObject != null) ? new bool?(gameObject.activeSelf) : null);
						}
					}
					bool? flag3 = flag2;
					bool flag4 = true;
					flag = (flag3.GetValueOrDefault() == flag4) & (flag3 != null);
				}
				if (!flag)
				{
					goto IL_167;
				}
				bool flag5 = !collider.GetComponentInParent<RagdollPart>().isSliced;
				IL_168:
				bool flag6 = flag5;
				if (flag6)
				{
					RagdollPart part = collider.GetComponentInParent<RagdollPart>();
					bool sliceAllowed = part.sliceAllowed;
					if (sliceAllowed)
					{
						CollisionInstance instance = new CollisionInstance(new DamageStruct(2, 20f), null, null);
						instance.damageStruct.hitRagdollPart = part;
						part.ragdoll.creature.Damage(instance);
						part.ragdoll.TrySlice(part);
						bool sliceForceKill = part.data.sliceForceKill;
						if (sliceForceKill)
						{
							part.ragdoll.creature.Kill();
						}
						yield return null;
						instance = null;
					}
					else
					{
						bool flag7 = !part.ragdoll.creature.isKilled;
						if (flag7)
						{
							CollisionInstance instance2 = new CollisionInstance(new DamageStruct(2, 20f), null, null);
							instance2.damageStruct.hitRagdollPart = part;
							part.ragdoll.creature.Damage(instance2);
							instance2 = null;
						}
					}
					part = null;
				}
				bool flag8 = collider.attachedRigidbody && !collider.attachedRigidbody.isKinematic;
				if (flag8)
				{
					bool flag9 = collider.attachedRigidbody.gameObject.layer == GameManager.GetLayer(10) || collider.attachedRigidbody.gameObject.layer == GameManager.GetLayer(13);
					if (flag9)
					{
						RagdollPart component = collider.attachedRigidbody.gameObject.GetComponent<RagdollPart>();
						bool flag10 = component && !creaturesPushed.Contains(component.ragdoll.creature);
						if (flag10)
						{
							component.ragdoll.creature.TryPush(0, (component.ragdoll.rootPart.transform.position - this.item.transform.position).normalized, 1, 0);
							creaturesPushed.Add(component.ragdoll.creature);
						}
						component = null;
					}
					bool flag11 = collider.attachedRigidbody.gameObject.layer != GameManager.GetLayer(10) && !rigidbodiesPushed.Contains(collider.attachedRigidbody);
					if (flag11)
					{
						collider.attachedRigidbody.AddExplosionForce(2f, this.item.transform.position, 1.5f, 0f, 2);
						rigidbodiesPushed.Add(collider.attachedRigidbody);
					}
				}
				collider = null;
				i++;
				continue;
				IL_167:
				flag5 = false;
				goto IL_168;
			}
			array = null;
			Item.allThrowed.Remove(this.item);
			yield break;
		}

		// Token: 0x04000004 RID: 4
		private Item item;
	}
}
