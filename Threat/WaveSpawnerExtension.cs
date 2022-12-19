using System;
using ThunderRoad;

namespace Threat
{
	// Token: 0x02000005 RID: 5
	public static class WaveSpawnerExtension
	{
		// Token: 0x0600001D RID: 29 RVA: 0x00002EE4 File Offset: 0x000010E4
		public static void RemoveFromWave(this WaveSpawner waveSpawner, Creature waveCreature)
		{
			bool flag = waveSpawner.spawnedCreatures.Contains(waveCreature);
			if (flag)
			{
				waveSpawner.spawnedCreatures.Remove(waveCreature);
			}
			waveCreature.spawnGroup = null;
		}
	}
}
