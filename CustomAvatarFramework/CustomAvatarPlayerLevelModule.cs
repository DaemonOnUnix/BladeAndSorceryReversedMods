using System;
using System.Collections;
using CustomAvatarFramework.Extensions;
using ThunderRoad;

namespace CustomAvatarFramework
{
	// Token: 0x02000011 RID: 17
	public class CustomAvatarPlayerLevelModule : LevelModule
	{
		// Token: 0x0600006B RID: 107 RVA: 0x00004E80 File Offset: 0x00003080
		public override IEnumerator OnLoadCoroutine()
		{
			this._customPlayerAvatarManager = GameManager.local.GetComponent<CustomPlayerAvatarManager>();
			if (this._customPlayerAvatarManager == null)
			{
				this._customPlayerAvatarManager = GameManager.local.gameObject.AddComponent<CustomPlayerAvatarManager>();
			}
			EventManager.onCreatureSpawn += new EventManager.CreatureSpawnedEvent(this.EventManagerOnonCreatureSpawn);
			CustomAvatarEventManager.onCustomAvatarEquipped += this.CustomAvatarEventManagerOnonCustomAvatarEquipped;
			CustomAvatarEventManager.onCustomAvatarUnEquipped += this.CustomAvatarEventManagerOnonCustomAvatarUnEquipped;
			return base.OnLoadCoroutine();
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00004EF9 File Offset: 0x000030F9
		private void CustomAvatarEventManagerOnonCustomAvatarUnEquipped(Creature creature)
		{
			if (!creature.isPlayer)
			{
				return;
			}
			this._customPlayerAvatarManager.currentPlayerAvatar = null;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00004F10 File Offset: 0x00003110
		private void CustomAvatarEventManagerOnonCustomAvatarEquipped(Creature creature, ItemData itemData)
		{
			if (!creature.isPlayer)
			{
				return;
			}
			this._customPlayerAvatarManager.currentPlayerAvatar = itemData;
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00004F27 File Offset: 0x00003127
		private void EventManagerOnonCreatureSpawn(Creature creature)
		{
			if (!creature.isPlayer)
			{
				return;
			}
			if (this._customPlayerAvatarManager.currentPlayerAvatar == null)
			{
				return;
			}
			GameManager.local.StartCoroutine(creature.EquipApparelAvatarV2(this._customPlayerAvatarManager.currentPlayerAvatar));
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00004F5C File Offset: 0x0000315C
		public override void OnUnload()
		{
			CustomAvatarEventManager.onCustomAvatarEquipped -= this.CustomAvatarEventManagerOnonCustomAvatarEquipped;
			CustomAvatarEventManager.onCustomAvatarUnEquipped -= this.CustomAvatarEventManagerOnonCustomAvatarUnEquipped;
		}

		// Token: 0x04000026 RID: 38
		private CustomPlayerAvatarManager _customPlayerAvatarManager;
	}
}
