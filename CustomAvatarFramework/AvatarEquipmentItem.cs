using System;
using System.Collections;
using ThunderRoad;
using UnityEngine;

namespace CustomAvatarFramework
{
	// Token: 0x02000029 RID: 41
	public class AvatarEquipmentItem : MonoBehaviour
	{
		// Token: 0x060000C4 RID: 196 RVA: 0x00007908 File Offset: 0x00005B08
		private void Awake()
		{
			this._item = base.GetComponent<Item>();
			this._module = this._item.data.GetModule<ItemModuleAvatarEquipment>();
			this._item.OnSnapEvent += delegate(Holder holder)
			{
				base.StartCoroutine(this.EquipAvatar(holder.creature, this._item));
			};
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00007A58 File Offset: 0x00005C58
		private IEnumerator EquipAvatar(Creature creature, Item item)
		{
			item.holder.UnSnap(item, true, true);
			yield return new WaitForSeconds(0.5f);
			item.Despawn();
			ItemData apparelData = Catalog.GetData<ItemData>(this._module.avatarApparelId, true);
			if (apparelData != null)
			{
				if (apparelData.type != 7)
				{
					Debug.Log("Item is not an apparel");
				}
				else
				{
					creature.equipment.EquipWardrobe(new ContainerData.Content(apparelData, null, null, 1), true);
					yield return null;
				}
			}
			yield break;
		}

		// Token: 0x040000D4 RID: 212
		private Item _item;

		// Token: 0x040000D5 RID: 213
		private ItemModuleAvatarEquipment _module;
	}
}
