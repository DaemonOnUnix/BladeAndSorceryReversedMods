using System;
using ThunderRoad;
using UnityEngine;

namespace DynamicModule.Modules
{
	// Token: 0x02000007 RID: 7
	public class ImbueModule : MonoBehaviour
	{
		// Token: 0x0600000E RID: 14 RVA: 0x00002542 File Offset: 0x00000742
		private void Start()
		{
			this.Weapon.Item.OnHeldActionEvent += new Item.HeldActionDelegate(this.ItemOnOnHeldActionEvent);
			this.bind = this.GetEnum(this.Weapon.Data.ImbueModuleBind);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002580 File Offset: 0x00000780
		private void ItemOnOnHeldActionEvent(RagdollHand ragdollhand, Handle handle, Interactable.Action action)
		{
			bool flag;
			if (this.bind != null)
			{
				Interactable.Action? action2 = this.bind;
				flag = (action == action2.GetValueOrDefault()) & (action2 != null);
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			if (flag2)
			{
				bool flag3 = this.index < this.Weapon.Data.SpellIds.Count;
				if (flag3)
				{
					this.selectedSpell = this.Weapon.Data.SpellIds[this.index];
					this.index++;
				}
				else
				{
					this.selectedSpell = null;
					this.index = 0;
				}
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002624 File Offset: 0x00000824
		private void Update()
		{
			bool flag = this.selectedSpell != null;
			if (flag)
			{
				SpellCastCharge spellCastCharge = Catalog.GetData<SpellCastCharge>(this.selectedSpell, true).Clone();
				foreach (Imbue imbue in this.Weapon.Item.imbues)
				{
					imbue.Transfer(spellCastCharge, 300f);
				}
			}
			else
			{
				foreach (Imbue imbue2 in this.Weapon.Item.imbues)
				{
					bool flag2 = imbue2 != null;
					if (flag2)
					{
						imbue2.energy = 0f;
					}
				}
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002710 File Offset: 0x00000910
		private Interactable.Action? GetEnum(string bind)
		{
			Interactable.Action? action;
			if (!(bind == "trigger"))
			{
				if (!(bind == "spellWheel"))
				{
					if (!(bind == "grab"))
					{
						action = null;
					}
					else
					{
						action = new Interactable.Action?(6);
					}
				}
				else
				{
					action = new Interactable.Action?(2);
				}
			}
			else
			{
				action = new Interactable.Action?(0);
			}
			return action;
		}

		// Token: 0x0400001E RID: 30
		public InitiateBehavior Weapon;

		// Token: 0x0400001F RID: 31
		private string selectedSpell;

		// Token: 0x04000020 RID: 32
		private Interactable.Action? bind;

		// Token: 0x04000021 RID: 33
		private int index = 0;
	}
}
