using System;
using ThunderRoad;
using UnityEngine;

namespace SaberMod
{
	// Token: 0x02000004 RID: 4
	public class ItemModuleSaber : ItemModule
	{
		// Token: 0x0600001A RID: 26 RVA: 0x000022A0 File Offset: 0x000004A0
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			GameObject gameObject = item.gameObject;
			Component component;
			if (!gameObject.TryGetComponent(typeof(ItemSaber), ref component))
			{
				gameObject.AddComponent(typeof(ItemSaber));
			}
		}
	}
}
