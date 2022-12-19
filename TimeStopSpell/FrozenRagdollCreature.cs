using System;
using ThunderRoad;
using UnityEngine;

namespace TimeStopSpell
{
	// Token: 0x02000009 RID: 9
	public class FrozenRagdollCreature : MonoBehaviour
	{
		// Token: 0x0600002A RID: 42 RVA: 0x00003368 File Offset: 0x00001568
		private void Start()
		{
			this._creature = base.gameObject.GetComponentInParent<Creature>();
			foreach (RagdollPart ragdollPart in this._creature.ragdoll.parts)
			{
				if (ragdollPart.rb != null && ragdollPart.gameObject.GetComponent<FrozenRagdollPart>() == null)
				{
					ragdollPart.gameObject.AddComponent<FrozenRagdollPart>();
				}
			}
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000033FC File Offset: 0x000015FC
		private void Update()
		{
			if (this._creature.ragdoll.isGrabbed || this._creature.ragdoll.isTkGrabbed)
			{
				Object.Destroy(this);
			}
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00003428 File Offset: 0x00001628
		private void OnDestroy()
		{
			foreach (RagdollPart ragdollPart in this._creature.ragdoll.parts)
			{
				if (ragdollPart.gameObject.GetComponent<FrozenRagdollPart>() != null)
				{
					Object.Destroy(ragdollPart.gameObject.GetComponent<FrozenRagdollPart>());
				}
			}
		}

		// Token: 0x0400001D RID: 29
		private Creature _creature;
	}
}
