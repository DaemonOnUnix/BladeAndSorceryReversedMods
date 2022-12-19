using System;
using ThunderRoad;

namespace CustomAvatarFramework
{
	// Token: 0x02000004 RID: 4
	public class CustomAvatarEventManager
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600000B RID: 11 RVA: 0x00002638 File Offset: 0x00000838
		// (remove) Token: 0x0600000C RID: 12 RVA: 0x0000266C File Offset: 0x0000086C
		public static event CustomAvatarEventManager.OnCustomAvatarEquipped onCustomAvatarEquipped;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600000D RID: 13 RVA: 0x000026A0 File Offset: 0x000008A0
		// (remove) Token: 0x0600000E RID: 14 RVA: 0x000026D4 File Offset: 0x000008D4
		public static event CustomAvatarEventManager.OnCustomAvatarUnEquipped onCustomAvatarUnEquipped;

		// Token: 0x0600000F RID: 15 RVA: 0x00002707 File Offset: 0x00000907
		public static void InvokeOnCustomAvatarEquipped(Creature creature, ItemData itemData)
		{
			if (CustomAvatarEventManager.onCustomAvatarEquipped != null)
			{
				CustomAvatarEventManager.onCustomAvatarEquipped(creature, itemData);
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x0000271C File Offset: 0x0000091C
		public static void InvokeOnCustomAvatarUnEquipped(Creature creature)
		{
			if (CustomAvatarEventManager.onCustomAvatarUnEquipped != null)
			{
				CustomAvatarEventManager.onCustomAvatarUnEquipped(creature);
			}
		}

		// Token: 0x02000005 RID: 5
		// (Invoke) Token: 0x06000013 RID: 19
		public delegate void OnCustomAvatarEquipped(Creature creature, ItemData itemData);

		// Token: 0x02000006 RID: 6
		// (Invoke) Token: 0x06000017 RID: 23
		public delegate void OnCustomAvatarUnEquipped(Creature creature);
	}
}
