using System;
using System.Collections;
using ThunderRoad;
using UnityEngine;

namespace Agniratha
{
	// Token: 0x02000002 RID: 2
	public class AgnirathaModule : LevelModule
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public override IEnumerator OnLoadCoroutine()
		{
			this.battleMusic = this.level.customReferences.Find((Level.CustomReference match) => match.name == "Music").transforms.Find((Transform match) => match.name == "BattleMusic").GetComponent<AudioSource>();
			this.uniqueBattleMusic = this.level.customReferences.Find((Level.CustomReference match) => match.name == "Music").transforms.Find((Transform match) => match.name == "UniqueBattleMusic").GetComponent<AudioSource>();
			this.dayMusic = this.level.customReferences.Find((Level.CustomReference match) => match.name == "Music").transforms.Find((Transform match) => match.name == "DayMusic").GetComponent<AudioSource>();
			this.nightMusic = this.level.customReferences.Find((Level.CustomReference match) => match.name == "Music").transforms.Find((Transform match) => match.name == "NightMusic").GetComponent<AudioSource>();
			return base.OnLoadCoroutine();
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000021F4 File Offset: 0x000003F4
		public override void Update()
		{
			base.Update();
			WaveSpawner waveSpawner;
			bool flag = !WaveSpawner.TryGetRunningInstance(ref waveSpawner);
			if (flag)
			{
				this.isInCombat = false;
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
					bool flag2 = @object != null && !creature.isPlayer && !creature.isKilled && (creature.brain.currentTarget.isPlayer || creature.brain.currentTarget.faction == Player.local.creature.faction);
					if (flag2)
					{
						this.isInCombat = true;
						bool isPlaying = this.battleMusic.isPlaying;
						if (isPlaying)
						{
							return;
						}
						this.battleMusic.Play();
						this.dayMusic.Pause();
						this.nightMusic.Pause();
					}
				}
				bool flag3 = this.isInCombat || !this.battleMusic.isPlaying;
				if (!flag3)
				{
					this.uniqueBattleMusic.Stop();
					this.battleMusic.Stop();
					this.battleMusic.mute = false;
					this.dayMusic.UnPause();
					this.nightMusic.UnPause();
				}
			}
		}

		// Token: 0x04000001 RID: 1
		private AudioSource battleMusic;

		// Token: 0x04000002 RID: 2
		private AudioSource uniqueBattleMusic;

		// Token: 0x04000003 RID: 3
		private AudioSource dayMusic;

		// Token: 0x04000004 RID: 4
		private AudioSource nightMusic;

		// Token: 0x04000005 RID: 5
		private bool isInCombat;
	}
}
