using System;
using ThunderRoad;
using UnityEngine;

namespace UltimateFantasyPack
{
	// Token: 0x02000022 RID: 34
	public class ImbueCycle : MonoBehaviour
	{
		// Token: 0x06000080 RID: 128 RVA: 0x000047F1 File Offset: 0x000029F1
		private void Awake()
		{
			this.item = base.GetComponent<Item>();
			this.item.OnHeldActionEvent += new Item.HeldActionDelegate(this.OnHeld);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00004818 File Offset: 0x00002A18
		private void OnHeld(RagdollHand ragdollHand, Handle handle, Interactable.Action action)
		{
			bool flag = action == 2;
			if (flag)
			{
				bool none = this.None;
				if (none)
				{
					this.None = false;
					this.Lightning = true;
					foreach (Imbue imbue in this.item.imbues)
					{
						imbue.Transfer(Catalog.GetData<SpellCastCharge>("Lightning", true).Clone(), imbue.maxEnergy);
					}
				}
				else
				{
					bool lightning = this.Lightning;
					if (lightning)
					{
						this.Lightning = false;
						this.Fire = true;
						foreach (Imbue imbue2 in this.item.imbues)
						{
							imbue2.Transfer(Catalog.GetData<SpellCastCharge>("Fire", true).Clone(), imbue2.maxEnergy);
						}
					}
					else
					{
						bool fire = this.Fire;
						if (fire)
						{
							this.Fire = false;
							this.Gravity = true;
							foreach (Imbue imbue3 in this.item.imbues)
							{
								imbue3.Transfer(Catalog.GetData<SpellCastCharge>("Gravity", true).Clone(), imbue3.maxEnergy);
							}
						}
						else
						{
							bool gravity = this.Gravity;
							if (gravity)
							{
								this.Gravity = false;
								this.None = true;
								foreach (Imbue imbue4 in this.item.imbues)
								{
									imbue4.Transfer(Catalog.GetData<SpellCastCharge>("Fire", true).Clone(), 0f);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0400005E RID: 94
		public Item item;

		// Token: 0x0400005F RID: 95
		public bool Lightning = false;

		// Token: 0x04000060 RID: 96
		public bool Fire = false;

		// Token: 0x04000061 RID: 97
		public bool Gravity = false;

		// Token: 0x04000062 RID: 98
		public bool None = true;
	}
}
