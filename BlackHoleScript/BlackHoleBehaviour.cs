using System;
using System.Collections;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

namespace BlackHole
{
	// Token: 0x02000003 RID: 3
	public class BlackHoleBehaviour : MonoBehaviour
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000003 RID: 3 RVA: 0x00002076 File Offset: 0x00000276
		// (set) Token: 0x06000004 RID: 4 RVA: 0x0000207E File Offset: 0x0000027E
		public BlackHoleModule itemModule { get; internal set; }

		// Token: 0x06000005 RID: 5 RVA: 0x00002088 File Offset: 0x00000288
		public void Start()
		{
			this.gravitationalPull *= this.itemModule.gravitationalPullMultiplier;
			this.distanceMultiplier *= this.itemModule.gravityDistanceMultiplier;
			this.power *= this.itemModule.explosionPowerMultiplier;
			this.masseffect *= this.itemModule.masseffectMultiplier;
			this.distancefalloff *= this.distancefalloff * this.itemModule.explosionDistancefalloffMultiplier;
			this.blackHoleDuration = this.itemModule.blackHoleDuration;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002126 File Offset: 0x00000326
		public void Update()
		{
			this.timepassed += Time.deltaTime;
			this.timeAtCollision += Time.deltaTime;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002150 File Offset: 0x00000350
		public void OnCollisionEnter(Collision collision)
		{
			bool flag = this.count == 0 && this.timepassed > 0.02f;
			if (flag)
			{
				this.count++;
				base.GetComponent<Rigidbody>().isKinematic = true;
				base.StartCoroutine(this.BlackHoleSequence());
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000021A4 File Offset: 0x000003A4
		private IEnumerator BlackHoleSequence()
		{
			base.StartCoroutine(this.Gravity());
			this.timeAtCollision = 0f;
			yield return new WaitForSeconds(this.blackHoleDuration + 0.2f);
			foreach (Creature creature in Creature.all)
			{
				this.PushAwayCreatures(creature);
				creature = null;
			}
			List<Creature>.Enumerator enumerator = default(List<Creature>.Enumerator);
			foreach (Item item in Item.allActive)
			{
				this.PushAwayItems(item);
				item = null;
			}
			List<Item>.Enumerator enumerator2 = default(List<Item>.Enumerator);
			Object.Destroy(base.gameObject);
			yield break;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000021B3 File Offset: 0x000003B3
		private IEnumerator Gravity()
		{
			while (this.timeAtCollision < this.blackHoleDuration)
			{
				bool flag = this.timeAtCollision > 1f && this.timeAtCollision < this.blackHoleDuration - 1f;
				if (flag)
				{
					this.gravitationalPull *= 1f + Time.deltaTime / this.timepassed;
					base.gameObject.transform.localScale *= 1f + Time.deltaTime / this.timepassed;
				}
				bool flag2 = this.timeAtCollision > this.blackHoleDuration - 1f;
				if (flag2)
				{
					this.gravitationalPull *= 0.98f - Time.deltaTime / this.timepassed;
					base.gameObject.transform.localScale *= 0.97f - Time.deltaTime / this.timepassed;
				}
				foreach (Creature creature in Creature.all)
				{
					this.PullCreatures(creature);
					creature = null;
				}
				List<Creature>.Enumerator enumerator = default(List<Creature>.Enumerator);
				foreach (Item item in Item.allActive)
				{
					this.PullItems(item);
					item = null;
				}
				List<Item>.Enumerator enumerator2 = default(List<Item>.Enumerator);
				yield return null;
			}
			yield break;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000021C4 File Offset: 0x000003C4
		public void PullItems(Item item)
		{
			bool flag = item.rb == null;
			if (!flag)
			{
				Vector3 vector = base.transform.position - item.transform.position;
				float num = vector.sqrMagnitude * this.distanceMultiplier + 1f;
				bool flag2 = num < 0.05f || num > 50f;
				if (!flag2)
				{
					item.rb.AddForce(vector.normalized * (this.gravitationalPull / num) * item.rb.mass * Time.fixedDeltaTime);
					item.Throw(1f, 1);
				}
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002278 File Offset: 0x00000478
		public void PullCreatures(Creature creature)
		{
			Vector3 vector = base.transform.position - creature.transform.position;
			float num = vector.sqrMagnitude * this.distanceMultiplier + 1f;
			bool flag = num < 0.05f || num > 50f;
			if (!flag)
			{
				bool flag2 = creature.state != 1 && creature.state != null && creature != Player.currentCreature;
				if (flag2)
				{
					creature.ragdoll.SetState(1);
				}
				foreach (RagdollPart ragdollPart in creature.ragdoll.parts)
				{
					ragdollPart.rb.AddForce(vector.normalized * (this.gravitationalPull / 5f / num) * ragdollPart.rb.mass * Time.fixedDeltaTime);
				}
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002390 File Offset: 0x00000590
		public void PushAwayItems(Item item)
		{
			bool flag = item.rb == null;
			if (!flag)
			{
				float num = (base.transform.position - base.gameObject.transform.position).sqrMagnitude * 1f + 1f;
				item.rb.AddExplosionForce(this.power * item.rb.mass * this.masseffect / num / this.distancefalloff, base.transform.position, this.radius, this.upwardsForce);
				item.Throw(1f, 1);
				item.Throw(1f, 1);
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002448 File Offset: 0x00000648
		public void PushAwayCreatures(Creature creature)
		{
			bool flag = creature == Player.currentCreature;
			if (!flag)
			{
				float num = (base.transform.position - base.gameObject.transform.position).sqrMagnitude * 1f + 1f;
				foreach (RagdollPart ragdollPart in creature.ragdoll.parts)
				{
					ragdollPart.rb.AddExplosionForce(this.power * ragdollPart.rb.mass * this.masseffect / num / this.distancefalloff, base.transform.position, this.radius, this.upwardsForce);
				}
			}
		}

		// Token: 0x04000009 RID: 9
		private int count = 0;

		// Token: 0x0400000A RID: 10
		private float timepassed = 0f;

		// Token: 0x0400000B RID: 11
		private float timeAtCollision = 0f;

		// Token: 0x0400000C RID: 12
		private float gravitationalPull = 100000f;

		// Token: 0x0400000D RID: 13
		private float distanceMultiplier = 1f;

		// Token: 0x0400000E RID: 14
		private float power = 40000f;

		// Token: 0x0400000F RID: 15
		private float masseffect = 1f;

		// Token: 0x04000010 RID: 16
		private float distancefalloff = 1f;

		// Token: 0x04000011 RID: 17
		private float radius = 100f;

		// Token: 0x04000012 RID: 18
		private float upwardsForce = 0.1f;

		// Token: 0x04000013 RID: 19
		private float blackHoleDuration;
	}
}
