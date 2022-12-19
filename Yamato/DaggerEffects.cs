using System;
using ThunderRoad;
using UnityEngine;

namespace Yamato
{
	// Token: 0x02000005 RID: 5
	public class DaggerEffects : MonoBehaviour
	{
		// Token: 0x06000009 RID: 9 RVA: 0x0000221C File Offset: 0x0000041C
		public void Start()
		{
			this.item = base.GetComponent<Item>();
			EffectInstance effectInstance = Catalog.GetData<EffectData>("MirageFire", true).Spawn(this.item.transform, false, null, false, Array.Empty<Type>());
			effectInstance.SetRenderer(this.item.colliderGroups[0].imbueEffectRenderer, false);
			effectInstance.SetIntensity(1f);
			effectInstance.Play(0, false);
		}

		// Token: 0x04000005 RID: 5
		private Item item;
	}
}
