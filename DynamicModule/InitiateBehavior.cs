using System;
using DynamicModule.Modules;
using ThunderRoad;
using UnityEngine;

namespace DynamicModule
{
	// Token: 0x02000003 RID: 3
	public class InitiateBehavior : MonoBehaviour
	{
		// Token: 0x06000003 RID: 3 RVA: 0x00002078 File Offset: 0x00000278
		private void Start()
		{
			this.Item = base.GetComponent<Item>();
			bool enableFlightModule = this.Data.EnableFlightModule;
			if (enableFlightModule)
			{
				this.Item.gameObject.AddComponent<FlightModule>().Weapon = this;
			}
			bool enableImbueModule = this.Data.EnableImbueModule;
			if (enableImbueModule)
			{
				this.Item.gameObject.AddComponent<ImbueModule>().Weapon = this;
			}
			bool enableReturnModule = this.Data.EnableReturnModule;
			if (enableReturnModule)
			{
				this.Item.gameObject.AddComponent<ReturnModule>().Weapon = this;
			}
			bool enableGroundPoundModule = this.Data.EnableGroundPoundModule;
			if (enableGroundPoundModule)
			{
				this.Item.gameObject.AddComponent<GroundPoundModule>().Weapon = this;
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002128 File Offset: 0x00000328
		private void Update()
		{
			bool flag = this.Item.handlers.Count > 0;
			if (flag)
			{
				this.currentHand = this.Item.handlers[0].playerHand;
			}
			else
			{
				this.currentHand = null;
			}
		}

		// Token: 0x04000014 RID: 20
		public Initiate Data;

		// Token: 0x04000015 RID: 21
		public PlayerHand currentHand;

		// Token: 0x04000016 RID: 22
		public Item Item;
	}
}
