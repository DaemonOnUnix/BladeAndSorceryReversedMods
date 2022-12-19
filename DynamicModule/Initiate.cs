using System;
using System.Collections.Generic;
using ThunderRoad;

namespace DynamicModule
{
	// Token: 0x02000002 RID: 2
	public class Initiate : ItemModule
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public override void OnItemLoaded(Item item)
		{
			item.gameObject.AddComponent<InitiateBehavior>().Data = this;
			base.OnItemLoaded(item);
		}

		// Token: 0x04000001 RID: 1
		public bool EnableFlightModule;

		// Token: 0x04000002 RID: 2
		public float FlightPower;

		// Token: 0x04000003 RID: 3
		public bool EnableImbueModule;

		// Token: 0x04000004 RID: 4
		public string ImbueModuleBind;

		// Token: 0x04000005 RID: 5
		public List<string> SpellIds;

		// Token: 0x04000006 RID: 6
		public bool EnableReturnModule;

		// Token: 0x04000007 RID: 7
		public string ReturnModuleReturnBind;

		// Token: 0x04000008 RID: 8
		public string ReturnModuleResetBind;

		// Token: 0x04000009 RID: 9
		public float HoldDurationReset;

		// Token: 0x0400000A RID: 10
		public float GrabDistance;

		// Token: 0x0400000B RID: 11
		public float ReturnForce;

		// Token: 0x0400000C RID: 12
		public float RoationSpeed;

		// Token: 0x0400000D RID: 13
		public bool EnableGroundPoundModule;

		// Token: 0x0400000E RID: 14
		public string GroundPoundVFXRef;

		// Token: 0x0400000F RID: 15
		public List<string> ColliderTriggerNames;

		// Token: 0x04000010 RID: 16
		public float GroundPoundActivationForce;

		// Token: 0x04000011 RID: 17
		public float GroundPoundCooldown;

		// Token: 0x04000012 RID: 18
		public int GroundPoundPushLevel;

		// Token: 0x04000013 RID: 19
		public float GroundPoundRadius;
	}
}
