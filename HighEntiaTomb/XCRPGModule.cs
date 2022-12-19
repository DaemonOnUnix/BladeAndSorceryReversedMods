using System;
using System.Collections;
using ThunderRoad;
using UnityEngine;
using XenobladeRPG;

namespace HighEntiaTomb
{
	// Token: 0x02000004 RID: 4
	internal class XCRPGModule : LevelModule
	{
		// Token: 0x06000007 RID: 7 RVA: 0x000023EC File Offset: 0x000005EC
		public override IEnumerator OnLoadCoroutine()
		{
			this.battleMusic = this.level.customReferences.Find((Level.CustomReference match) => match.name == "Music").transforms.Find((Transform match) => match.name == "BattleMusic").GetComponent<AudioSource>();
			this.uniqueBattleMusic = this.level.customReferences.Find((Level.CustomReference match) => match.name == "Music").transforms.Find((Transform match) => match.name == "UniqueBattleMusic").GetComponent<AudioSource>();
			return base.OnLoadCoroutine();
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000024CC File Offset: 0x000006CC
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

		// Token: 0x04000005 RID: 5
		private AudioSource battleMusic;

		// Token: 0x04000006 RID: 6
		private AudioSource uniqueBattleMusic;

		// Token: 0x04000007 RID: 7
		private bool isFightingUnique;
	}
}
