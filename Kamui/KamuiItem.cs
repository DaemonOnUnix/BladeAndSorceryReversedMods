using System;
using ThunderRoad;
using UnityEngine;

namespace Kamui
{
	// Token: 0x02000005 RID: 5
	public class KamuiItem : ItemModule
	{
		// Token: 0x0600000F RID: 15 RVA: 0x000028A8 File Offset: 0x00000AA8
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			item.gameObject.AddComponent<Attractor>();
			item.gameObject.GetComponent<Attractor>().mainAttractor = true;
			item.gameObject.GetComponent<Attractor>().rb = item.gameObject.GetComponent<Rigidbody>();
			item.gameObject.AddComponent<Kamui>();
		}
	}
}
