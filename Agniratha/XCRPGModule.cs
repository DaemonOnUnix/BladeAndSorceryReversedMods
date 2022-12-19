using System;
using System.Collections;
using ThunderRoad;
using UnityEngine;
using XenobladeRPG;

namespace Agniratha
{
	// Token: 0x02000003 RID: 3
	internal class XCRPGModule : LevelModule
	{
		// Token: 0x06000004 RID: 4 RVA: 0x00002388 File Offset: 0x00000588
		public override IEnumerator OnLoadCoroutine()
		{
			this.battleMusic = this.level.customReferences[0].transforms.Find((Transform match) => match.name == "BattleMusic").GetComponent<AudioSource>();
			this.uniqueBattleMusic = this.level.customReferences[0].transforms.Find((Transform match) => match.name == "UniqueBattleMusic").GetComponent<AudioSource>();
			return base.OnLoadCoroutine();
		}

		// Token: 0x06000005 RID: 5 RVA: 0x0000242C File Offset: 0x0000062C
		public override void Update()
		{
			base.Update();
			WaveSpawner waveSpawner;
			bool flag = !WaveSpawner.TryGetRunningInstance(ref waveSpawner);
			if (flag)
			{
				this.isFightingUnique = false;
				foreach (Creature creature in Creature.allActive)
				{
					Object @object;
					if (creature == null)
					{
						@object = null;
					}
					else
					{
						Brain brain = creature.brain;
						@object = ((brain != null) ? brain.currentTarget : null);
					}
					bool flag2 = @object != null && !creature.isPlayer && !creature.isKilled && (creature.brain.currentTarget == Player.local.creature || creature.brain.currentTarget.faction == Player.local.creature.faction);
					if (flag2)
					{
						bool flag3 = creature.GetComponent<XenobladeStats>() != null && creature.GetComponent<XenobladeStats>().isUnique;
						if (flag3)
						{
							this.isFightingUnique = true;
							bool isPlaying = this.uniqueBattleMusic.isPlaying;
							if (isPlaying)
							{
								return;
							}
							this.uniqueBattleMusic.Play();
							this.battleMusic.mute = true;
						}
					}
				}
				bool flag4 = this.isFightingUnique || !this.uniqueBattleMusic.isPlaying;
				if (!flag4)
				{
					this.uniqueBattleMusic.Stop();
					this.battleMusic.mute = false;
				}
			}
		}

		// Token: 0x04000006 RID: 6
		private AudioSource battleMusic;

		// Token: 0x04000007 RID: 7
		private AudioSource uniqueBattleMusic;

		// Token: 0x04000008 RID: 8
		private bool isFightingUnique;
	}
}
