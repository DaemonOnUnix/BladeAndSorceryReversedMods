using System;
using System.Collections;
using ThunderRoad;
using UnityEngine;

namespace UltimateFantasyPack
{
	// Token: 0x02000013 RID: 19
	public class Vampire : ItemModule
	{
		// Token: 0x06000043 RID: 67 RVA: 0x000031C0 File Offset: 0x000013C0
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			item.OnGrabEvent += new Item.GrabDelegate(this.Grab);
			foreach (CollisionHandler handler in item.collisionHandlers)
			{
				handler.OnCollisionStartEvent += new CollisionHandler.CollisionEvent(this.Collision);
			}
			Debug.Log("Vampire ability loaded!");
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00003248 File Offset: 0x00001448
		private void Grab(Handle handle, RagdollHand ragdollHand)
		{
			this.bindedHand = ragdollHand;
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00003254 File Offset: 0x00001454
		private void Collision(CollisionInstance collisionInstance)
		{
			RagdollPart hitRagdollPart = collisionInstance.damageStruct.hitRagdollPart;
			Creature creature;
			if (hitRagdollPart == null)
			{
				creature = null;
			}
			else
			{
				Ragdoll ragdoll = hitRagdollPart.ragdoll;
				creature = ((ragdoll != null) ? ragdoll.creature : null);
			}
			this.mostRecentHitCreature = creature;
			bool alternateUsePressed = PlayerControl.GetHand(this.bindedHand.side).alternateUsePressed;
			if (alternateUsePressed)
			{
				GameManager.local.StartCoroutine(this.DrainBlood());
			}
		}

		// Token: 0x06000046 RID: 70 RVA: 0x000032B5 File Offset: 0x000014B5
		private IEnumerator DrainBlood()
		{
			yield return new WaitForSeconds(0.1f);
			LineRenderer line = new GameObject().AddComponent<LineRenderer>();
			line.material = new Material(Shader.Find("Universal Render Pipeline/Lit"))
			{
				color = Color.red
			};
			bool running = true;
			while (this.item.isPenetrating && this.mostRecentHitCreature && this.bindedHand && running)
			{
				line.SetPositions(new Vector3[]
				{
					this.item.transform.position,
					this.bindedHand.transform.position
				});
				bool flag = !this.mostRecentHitCreature.isKilled;
				if (flag)
				{
					this.mostRecentHitCreature.currentHealth -= Time.fixedDeltaTime * this.healthDrain;
					this.bindedHand.creature.currentHealth += Time.fixedDeltaTime * this.healthDrain;
				}
				else
				{
					this.mostRecentHitCreature.currentHealth -= Time.fixedDeltaTime * this.deadHealthDrain;
					this.bindedHand.creature.currentHealth += Time.fixedDeltaTime * this.deadHealthDrain;
				}
				bool flag2 = this.mostRecentHitCreature.currentHealth < 0f;
				if (flag2)
				{
					this.mostRecentHitCreature.Kill();
				}
				yield return null;
			}
			Object.Destroy(line.gameObject);
			this.mostRecentHitCreature = null;
			yield break;
		}

		// Token: 0x0400002B RID: 43
		public float healthDrain;

		// Token: 0x0400002C RID: 44
		public float deadHealthDrain;

		// Token: 0x0400002D RID: 45
		private Creature mostRecentHitCreature;

		// Token: 0x0400002E RID: 46
		private RagdollHand bindedHand;
	}
}
