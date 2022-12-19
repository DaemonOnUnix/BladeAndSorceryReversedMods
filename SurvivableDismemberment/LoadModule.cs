using System;
using System.Collections;
using ThunderRoad;

namespace SurvivableDismemberment
{
	// Token: 0x02000002 RID: 2
	public class LoadModule : LevelModule
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public override IEnumerator OnLoadCoroutine()
		{
			EventManager.onCreatureSpawn += new EventManager.CreatureSpawnedEvent(this.EventManager_onCreatureSpawn);
			return base.OnLoadCoroutine();
		}

		// Token: 0x06000002 RID: 2 RVA: 0x0000207C File Offset: 0x0000027C
		private void EventManager_onCreatureSpawn(Creature creature)
		{
			bool flag = !creature.isPlayer;
			if (flag)
			{
				bool flag2 = !creature.gameObject.GetComponent<UndyingRagdoll>();
				if (flag2)
				{
					UndyingRagdoll undyingRagdoll = creature.gameObject.AddComponent<UndyingRagdoll>();
				}
			}
		}

		// Token: 0x04000001 RID: 1
		public static bool dieOnHeadChop = true;

		// Token: 0x04000002 RID: 2
		public static bool destabilizeOneLeg = true;
	}
}
