using System;
using ModularFirearms.Weapons;
using ThunderRoad;

namespace ModularFirearms.Shared
{
	// Token: 0x02000013 RID: 19
	public class FirearmModule : ItemModule
	{
		// Token: 0x0600009C RID: 156 RVA: 0x00007868 File Offset: 0x00005A68
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			this.selectedType = (FrameworkCore.WeaponType)Enum.Parse(typeof(FrameworkCore.WeaponType), this.firearmType);
			if (this.selectedType.Equals(FrameworkCore.WeaponType.SemiAuto))
			{
				item.gameObject.AddComponent<BaseFirearmGenerator>();
				return;
			}
			if (this.selectedType.Equals(FrameworkCore.WeaponType.Shotgun))
			{
				item.gameObject.AddComponent<ShotgunGenerator>();
				return;
			}
			if (this.selectedType.Equals(FrameworkCore.WeaponType.TestWeapon))
			{
				item.gameObject.AddComponent<BaseFirearmGenerator>();
				return;
			}
			if (this.selectedType.Equals(FrameworkCore.WeaponType.SimpleFirearm))
			{
				item.gameObject.AddComponent<SimpleFirearm>();
				return;
			}
			item.gameObject.AddComponent<BaseFirearmGenerator>();
		}

		// Token: 0x04000103 RID: 259
		private FrameworkCore.WeaponType selectedType;

		// Token: 0x04000104 RID: 260
		public int firearmCategory;

		// Token: 0x04000105 RID: 261
		public string firearmType = "SemiAuto";

		// Token: 0x04000106 RID: 262
		public bool useHitscan;

		// Token: 0x04000107 RID: 263
		public float hitscanMaxDistance = 1f;

		// Token: 0x04000108 RID: 264
		public string mainHandleRef;

		// Token: 0x04000109 RID: 265
		public string slideHandleRef;

		// Token: 0x0400010A RID: 266
		public string slideCenterRef;

		// Token: 0x0400010B RID: 267
		public string muzzlePositionRef;

		// Token: 0x0400010C RID: 268
		public string shellEjectionRef;

		// Token: 0x0400010D RID: 269
		public string chamberBulletRef;

		// Token: 0x0400010E RID: 270
		public string flashRef;

		// Token: 0x0400010F RID: 271
		public string shellParticleRef;

		// Token: 0x04000110 RID: 272
		public string smokeRef;

		// Token: 0x04000111 RID: 273
		public float soundVolume = 1f;

		// Token: 0x04000112 RID: 274
		public string fireSoundRef;

		// Token: 0x04000113 RID: 275
		public string fireSound1Ref;

		// Token: 0x04000114 RID: 276
		public string fireSound2Ref;

		// Token: 0x04000115 RID: 277
		public string fireSound3Ref;

		// Token: 0x04000116 RID: 278
		public int maxFireSounds = 3;

		// Token: 0x04000117 RID: 279
		public string compassRef;

		// Token: 0x04000118 RID: 280
		public string laserRef;

		// Token: 0x04000119 RID: 281
		public string laserStartRef;

		// Token: 0x0400011A RID: 282
		public string laserEndRef;

		// Token: 0x0400011B RID: 283
		public float maxLaserDistance = 10f;

		// Token: 0x0400011C RID: 284
		public bool laserTogglePriority;

		// Token: 0x0400011D RID: 285
		public float laserToggleHoldTime = 0.25f;

		// Token: 0x0400011E RID: 286
		public bool longPressToEject;

		// Token: 0x0400011F RID: 287
		public float longPressTime = 0.25f;

		// Token: 0x04000120 RID: 288
		public string emptySoundRef;

		// Token: 0x04000121 RID: 289
		public string pullSoundRef;

		// Token: 0x04000122 RID: 290
		public string rackSoundRef;

		// Token: 0x04000123 RID: 291
		public string shellInsertSoundRef;

		// Token: 0x04000124 RID: 292
		public string animationRef;

		// Token: 0x04000125 RID: 293
		public string openAnimationRef;

		// Token: 0x04000126 RID: 294
		public string closeAnimationRef;

		// Token: 0x04000127 RID: 295
		public string fireAnimationRef;

		// Token: 0x04000128 RID: 296
		public bool useBuckshot;

		// Token: 0x04000129 RID: 297
		public string rayCastPointRef;

		// Token: 0x0400012A RID: 298
		public string ammoCounterRef;

		// Token: 0x0400012B RID: 299
		public string flashlightRef;

		// Token: 0x0400012C RID: 300
		public string flashlightMeshRef;

		// Token: 0x0400012D RID: 301
		public string foregripHandleRef;

		// Token: 0x0400012E RID: 302
		public float blastRadius = 1f;

		// Token: 0x0400012F RID: 303
		public float blastRange = 5f;

		// Token: 0x04000130 RID: 304
		public float blastForce = 500f;

		// Token: 0x04000131 RID: 305
		public float slideTravelDistance = 0.05f;

		// Token: 0x04000132 RID: 306
		public float slideStabilizerRadius = 0.02f;

		// Token: 0x04000133 RID: 307
		public float slideRackThreshold = -0.01f;

		// Token: 0x04000134 RID: 308
		public float slideNeutralLockOffset;

		// Token: 0x04000135 RID: 309
		public float slideMassOffset = 1f;

		// Token: 0x04000136 RID: 310
		public float slideForwardForce = 50f;

		// Token: 0x04000137 RID: 311
		public float slideBlowbackForce = 30f;

		// Token: 0x04000138 RID: 312
		public string projectileID;

		// Token: 0x04000139 RID: 313
		public string shellID;

		// Token: 0x0400013A RID: 314
		public string ammoID;

		// Token: 0x0400013B RID: 315
		public string[] acceptedMagazineIDs;

		// Token: 0x0400013C RID: 316
		public bool allowGrabMagazineFromGun;

		// Token: 0x0400013D RID: 317
		public string fireMode = "Single";

		// Token: 0x0400013E RID: 318
		public int[] allowedFireModes;

		// Token: 0x0400013F RID: 319
		public int fireRate = 600;

		// Token: 0x04000140 RID: 320
		public int burstNumber = 3;

		// Token: 0x04000141 RID: 321
		public bool allowCycleFireMode;

		// Token: 0x04000142 RID: 322
		public int maxReceiverAmmo = 12;

		// Token: 0x04000143 RID: 323
		public string shellReceiverDef;

		// Token: 0x04000144 RID: 324
		public float hitscanForceMult = 4f;

		// Token: 0x04000145 RID: 325
		public float shotgunForceMult = 2f;

		// Token: 0x04000146 RID: 326
		public float bulletForce = 10f;

		// Token: 0x04000147 RID: 327
		public float shellEjectionForce = 0.5f;

		// Token: 0x04000148 RID: 328
		public float hapticForce = 5f;

		// Token: 0x04000149 RID: 329
		public float throwMult = 2f;

		// Token: 0x0400014A RID: 330
		public float[] recoilForces;

		// Token: 0x0400014B RID: 331
		public float[] recoilTorques;

		// Token: 0x0400014C RID: 332
		public string idleAnimName = "idle";

		// Token: 0x0400014D RID: 333
		public string overheatAnimName = "overheat";

		// Token: 0x0400014E RID: 334
		public float projectileForce = 1000f;

		// Token: 0x0400014F RID: 335
		public float energyCapacity = 100f;

		// Token: 0x04000150 RID: 336
		public string overheatSoundRef;

		// Token: 0x04000151 RID: 337
		public string chargeLightRef;

		// Token: 0x04000152 RID: 338
		public string chargeEffectRef;

		// Token: 0x04000153 RID: 339
		public string PlasmaChargeUp;

		// Token: 0x04000154 RID: 340
		public string PlasmaChargeLoop;

		// Token: 0x04000155 RID: 341
		public string PlasmaHeatLatch;

		// Token: 0x04000156 RID: 342
		public string PlasmaChargeRelease;

		// Token: 0x04000157 RID: 343
		public string muzzleFlashRef;

		// Token: 0x04000158 RID: 344
		public string swtichSoundRef;

		// Token: 0x04000159 RID: 345
		public string reloadSoundRef;

		// Token: 0x0400015A RID: 346
		public string animatorRef;

		// Token: 0x0400015B RID: 347
		public string fireAnim;

		// Token: 0x0400015C RID: 348
		public string emptyAnim;

		// Token: 0x0400015D RID: 349
		public string reloadAnim;

		// Token: 0x0400015E RID: 350
		public string mainGripID;

		// Token: 0x0400015F RID: 351
		public string npcRaycastPositionRef;

		// Token: 0x04000160 RID: 352
		public float npcDistanceToFire = 10f;

		// Token: 0x04000161 RID: 353
		public bool npcMeleeEnableOverride = true;

		// Token: 0x04000162 RID: 354
		public float npcDamageToPlayer = 1f;

		// Token: 0x04000163 RID: 355
		public float npcDetectionRadius = 100f;

		// Token: 0x04000164 RID: 356
		public float npcMeleeEnableDistance = 0.5f;

		// Token: 0x04000165 RID: 357
		public bool loopedFireSound;

		// Token: 0x04000166 RID: 358
		public int ammoCapacity;

		// Token: 0x04000167 RID: 359
		public bool isFlintlock;

		// Token: 0x04000168 RID: 360
		public bool waitForReloadAnim;

		// Token: 0x04000169 RID: 361
		public bool waitForFireAnim;

		// Token: 0x0400016A RID: 362
		public string earlyFireSoundRef;

		// Token: 0x0400016B RID: 363
		public string earlyMuzzleFlashRef;

		// Token: 0x0400016C RID: 364
		public float flintlockDelay = 1f;
	}
}
