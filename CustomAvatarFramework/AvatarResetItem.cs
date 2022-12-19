using System;
using System.Collections;
using ThunderRoad;
using UnityEngine;

namespace CustomAvatarFramework
{
	// Token: 0x0200002B RID: 43
	public class AvatarResetItem : MonoBehaviour
	{
		// Token: 0x060000CA RID: 202 RVA: 0x00007AC2 File Offset: 0x00005CC2
		private void Awake()
		{
			this._item = base.GetComponent<Item>();
			this._item.OnSnapEvent += delegate(Holder holder)
			{
				base.StartCoroutine(this.UnEquipAvatar(holder.creature, this._item));
			};
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00007C1C File Offset: 0x00005E1C
		private IEnumerator UnEquipAvatar(Creature creature, Item item)
		{
			item.holder.UnSnap(item, true, true);
			yield return new WaitForSeconds(0.5f);
			item.Despawn();
			try
			{
				CustomAvatarCreatureV2 component = creature.gameObject.GetComponent<CustomAvatarCreatureV2>();
				ItemData data = component.avatar.data;
				Object.Destroy(component);
				CustomAvatarEventManager.InvokeOnCustomAvatarUnEquipped(creature);
			}
			catch (Exception)
			{
			}
			try
			{
				CustomAvatarCreature component2 = creature.gameObject.GetComponent<CustomAvatarCreature>();
				Object.Destroy(component2);
			}
			catch (Exception)
			{
			}
			yield return null;
			yield break;
		}

		// Token: 0x040000D6 RID: 214
		private Item _item;
	}
}
