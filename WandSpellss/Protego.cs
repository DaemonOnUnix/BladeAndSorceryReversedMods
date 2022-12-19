using System;
using System.Linq;
using ThunderRoad;
using UnityEngine;

namespace WandSpellss
{
	// Token: 0x02000013 RID: 19
	public class Protego : MonoBehaviour
	{
		// Token: 0x0600003B RID: 59 RVA: 0x00003C8B File Offset: 0x00001E8B
		public void Start()
		{
			this.item = base.GetComponent<Item>();
			this.source = base.GetComponent<AudioSource>();
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00003CA8 File Offset: 0x00001EA8
		public void OnParticleCollision(Collision c)
		{
			foreach (Rigidbody rigidbody in c.gameObject.GetComponentInParent<Creature>().ragdoll.parts.Select((RagdollPart part) => part.rb))
			{
				c.gameObject.GetComponentInParent<Creature>().ragdoll.SetState(1);
				rigidbody.AddForce(this.item.flyDirRef.transform.forward * 40f, 1);
			}
		}

		// Token: 0x04000055 RID: 85
		private Item item;

		// Token: 0x04000056 RID: 86
		private Item npcItem;

		// Token: 0x04000057 RID: 87
		internal AudioSource source;
	}
}
