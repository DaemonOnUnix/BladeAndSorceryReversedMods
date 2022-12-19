using System;
using ThunderRoad;
using UnityEngine;

namespace UltimateFantasyPack
{
	// Token: 0x02000009 RID: 9
	internal class ParticleCollisionHandler : MonoBehaviour
	{
		// Token: 0x0600001B RID: 27 RVA: 0x0000280C File Offset: 0x00000A0C
		private void OnParticleCollision(GameObject other)
		{
			RagdollPart part;
			other.gameObject.TryGetComponent<RagdollPart>(ref part);
			part.ragdoll.creature.TryElectrocute(50f, 5f, true, true, Catalog.GetData<EffectData>("ImbueFireRagdoll", true));
		}
	}
}
