using System;
using ThunderRoad;
using UnityEngine;

namespace WandSpellss
{
	// Token: 0x0200000D RID: 13
	public class Imperio : MonoBehaviour
	{
		// Token: 0x06000023 RID: 35 RVA: 0x00003359 File Offset: 0x00001559
		public void Start()
		{
			this.item = base.GetComponent<Item>();
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00003368 File Offset: 0x00001568
		internal void CastRay()
		{
			RaycastHit hit;
			bool flag = Physics.Raycast(this.item.flyDirRef.position, this.item.flyDirRef.forward, ref hit);
			if (flag)
			{
				Debug.Log("Did hit.");
				Debug.Log(hit.collider.gameObject.transform.parent.name);
				Transform parent = hit.collider.gameObject.transform.parent;
				this.parentLocal = parent.gameObject;
				bool flag2 = this.parentLocal.GetComponent<Creature>() != null;
				if (flag2)
				{
					this.playerCreature = Player.local.creature;
					this.ogCreature = this.parentLocal.GetComponent<Creature>();
					Player.local.SetCreature(this.parentLocal.GetComponent<Creature>(), false);
					Player.selfCollision = true;
					Player.local.creature.currentHealth = 30f;
					Player.local.creature.ragdoll.allowSelfDamage = true;
					string text = "playerCreature1st: ";
					Creature creature = this.playerCreature;
					Debug.Log(text + ((creature != null) ? creature.ToString() : null));
					string text2 = "ogCreature: ";
					Creature creature2 = this.ogCreature;
					Debug.Log(text2 + ((creature2 != null) ? creature2.ToString() : null));
					string text3 = "playerCreature2nd: ";
					Creature creature3 = Player.local.creature;
					Debug.Log(text3 + ((creature3 != null) ? creature3.ToString() : null));
				}
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000034E0 File Offset: 0x000016E0
		private void Update()
		{
			bool flag = this.playerCreature != null;
			if (flag)
			{
				bool isKilled = this.playerCreature.isKilled;
				if (isKilled)
				{
					Player.currentCreature.Kill();
				}
				else
				{
					bool flag2 = Player.local.creature.currentHealth <= 1f;
					if (flag2)
					{
						Player.local.creature.OnKillEvent += new Creature.KillEvent(this.Creature_OnKillEvent);
					}
				}
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x0000355C File Offset: 0x0000175C
		private void Creature_OnKillEvent(CollisionInstance collisionInstance, EventTime eventTime)
		{
			Player.local.SetCreature(this.playerCreature, false);
			Player.local.creature.currentHealth = 50f;
			Player.selfCollision = false;
			Player.local.creature.ragdoll.allowSelfDamage = false;
		}

		// Token: 0x04000036 RID: 54
		private Item item;

		// Token: 0x04000037 RID: 55
		private Creature playerCreature;

		// Token: 0x04000038 RID: 56
		private Creature ogCreature;

		// Token: 0x04000039 RID: 57
		internal GameObject parentLocal;
	}
}
