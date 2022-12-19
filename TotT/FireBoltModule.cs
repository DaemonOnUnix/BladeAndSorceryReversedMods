using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExtensionMethods;
using ThunderRoad;
using UnityEngine;

namespace TotT
{
	// Token: 0x02000025 RID: 37
	public class FireBoltModule : MonoBehaviour
	{
		// Token: 0x0600010C RID: 268 RVA: 0x00008A9F File Offset: 0x00006C9F
		public void Start()
		{
			this.item = base.GetComponent<Item>();
			this.item.mainCollisionHandler.OnCollisionStartEvent += new CollisionHandler.CollisionEvent(this.MainCollisionHandler_OnCollisionStartEvent);
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00008ACC File Offset: 0x00006CCC
		private void MainCollisionHandler_OnCollisionStartEvent(CollisionInstance collisionInstance)
		{
			bool isThrowed = this.item.isThrowed;
			if (isThrowed)
			{
				Catalog.GetData<EffectData>("GrooveSlinger.EffectDishonoredExplosion", true).Spawn(this.item.transform, true, null, false, Array.Empty<Type>()).Play(0, false);
				IEnumerable<Creature> result = ExtensionMethods.SphereCastCreature(this.item.transform.position, 4f, new Vector3(0f, 0f, 0f), 0f, true, false);
				List<Creature> explode = result.ToList<Creature>();
				foreach (Creature i in explode)
				{
					i.ragdoll.state = 0;
					i.Damage(new CollisionInstance(new DamageStruct(4, 40f), null, null));
					Rigidbody rb = i.gameObject.GetComponentInChildren<Rigidbody>();
					rb.AddExplosionForce(20f, this.item.transform.position, 2f, 1f, 1);
				}
				base.StartCoroutine(this.DelayDespawn(1f));
			}
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00008C04 File Offset: 0x00006E04
		public IEnumerator DelayDespawn(float delay)
		{
			yield return new WaitForSeconds(delay);
			this.item.Despawn();
			yield break;
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00008C1A File Offset: 0x00006E1A
		public void Update()
		{
		}

		// Token: 0x040000D3 RID: 211
		private Item item;
	}
}
