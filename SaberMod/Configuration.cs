using System;
using System.Collections;
using ThunderRoad;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace SaberMod
{
	// Token: 0x02000002 RID: 2
	public class Configuration : LevelModule
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		// (set) Token: 0x06000002 RID: 2 RVA: 0x00002057 File Offset: 0x00000257
		public static bool SaberTrailsActive { get; private set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000003 RID: 3 RVA: 0x0000205F File Offset: 0x0000025F
		// (set) Token: 0x06000004 RID: 4 RVA: 0x00002066 File Offset: 0x00000266
		public static float SaberTrailsSpeedDiff { get; private set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000005 RID: 5 RVA: 0x0000206E File Offset: 0x0000026E
		// (set) Token: 0x06000006 RID: 6 RVA: 0x00002075 File Offset: 0x00000275
		public static bool RecallAllowed { get; private set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000007 RID: 7 RVA: 0x0000207D File Offset: 0x0000027D
		// (set) Token: 0x06000008 RID: 8 RVA: 0x00002084 File Offset: 0x00000284
		public static bool RecallTurnSaberOff { get; private set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000009 RID: 9 RVA: 0x0000208C File Offset: 0x0000028C
		// (set) Token: 0x0600000A RID: 10 RVA: 0x00002093 File Offset: 0x00000293
		public static float RecallMaxDistance { get; private set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000B RID: 11 RVA: 0x0000209B File Offset: 0x0000029B
		// (set) Token: 0x0600000C RID: 12 RVA: 0x000020A2 File Offset: 0x000002A2
		public static float RecallStrength { get; private set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600000D RID: 13 RVA: 0x000020AA File Offset: 0x000002AA
		// (set) Token: 0x0600000E RID: 14 RVA: 0x000020B1 File Offset: 0x000002B1
		public static float IgnitionSpeed { get; private set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600000F RID: 15 RVA: 0x000020B9 File Offset: 0x000002B9
		// (set) Token: 0x06000010 RID: 16 RVA: 0x000020C0 File Offset: 0x000002C0
		public static float IgnitionDelay { get; private set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000011 RID: 17 RVA: 0x000020C8 File Offset: 0x000002C8
		// (set) Token: 0x06000012 RID: 18 RVA: 0x000020CF File Offset: 0x000002CF
		public static float HumVolume { get; private set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000013 RID: 19 RVA: 0x000020D7 File Offset: 0x000002D7
		// (set) Token: 0x06000014 RID: 20 RVA: 0x000020DE File Offset: 0x000002DE
		public static float IgnitionVolume { get; private set; }

		// Token: 0x06000015 RID: 21 RVA: 0x000020E8 File Offset: 0x000002E8
		public override IEnumerator OnLoadCoroutine()
		{
			Configuration.RecallAllowed = this.recallAllowed;
			Configuration.RecallTurnSaberOff = this.recallTurnSaberOff;
			Configuration.RecallMaxDistance = this.recallMaxDistance;
			Configuration.RecallStrength = this.recallStrength;
			Configuration.IgnitionSpeed = this.ignitionSpeed;
			Configuration.IgnitionDelay = this.ignitionDelay;
			Configuration.HumVolume = this.humVolume;
			Configuration.IgnitionVolume = this.ignitionVolume;
			Configuration.SaberTrailsActive = this.saberTrailsActive;
			Configuration.SaberTrailsSpeedDiff = this.saberTrailsSpeedDiff;
			SceneManager.sceneLoaded += new UnityAction<Scene, LoadSceneMode>(this.OnNewSceneLoaded);
			return base.OnLoadCoroutine();
		}

		// Token: 0x06000016 RID: 22 RVA: 0x0000217C File Offset: 0x0000037C
		private void OnNewSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			Configuration.RecallAllowed = this.recallAllowed;
			Configuration.RecallTurnSaberOff = this.recallTurnSaberOff;
			Configuration.RecallMaxDistance = this.recallMaxDistance;
			Configuration.RecallStrength = this.recallStrength;
			Configuration.IgnitionSpeed = this.ignitionSpeed;
			Configuration.IgnitionDelay = this.ignitionDelay;
			Configuration.HumVolume = this.humVolume;
			Configuration.IgnitionVolume = this.ignitionVolume;
			Configuration.SaberTrailsActive = this.saberTrailsActive;
			Configuration.SaberTrailsSpeedDiff = this.saberTrailsSpeedDiff;
		}

		// Token: 0x0400000B RID: 11
		public bool recallAllowed = true;

		// Token: 0x0400000C RID: 12
		public bool saberTrailsActive = true;

		// Token: 0x0400000D RID: 13
		public float saberTrailsSpeedDiff = 2f;

		// Token: 0x0400000E RID: 14
		public float recallMaxDistance = 4f;

		// Token: 0x0400000F RID: 15
		public bool recallTurnSaberOff = true;

		// Token: 0x04000010 RID: 16
		public float recallStrength = 15f;

		// Token: 0x04000011 RID: 17
		public float ignitionSpeed = 0.2f;

		// Token: 0x04000012 RID: 18
		public float ignitionDelay = 1f;

		// Token: 0x04000013 RID: 19
		public float humVolume = 0.8f;

		// Token: 0x04000014 RID: 20
		public float ignitionVolume = 1f;
	}
}
