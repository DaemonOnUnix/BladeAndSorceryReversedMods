using System;
using ModularFirearms.Weapons;
using ThunderRoad;

namespace ModularFirearms.Legacy
{
	// Token: 0x0200000C RID: 12
	public class ItemModuleFirearmSimple : ItemModule
	{
		// Token: 0x06000050 RID: 80 RVA: 0x00003C0C File Offset: 0x00001E0C
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			item.gameObject.AddComponent<SimpleFirearm>();
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00003C24 File Offset: 0x00001E24
		public ItemModuleFirearmSimple()
		{
			float[] array = new float[6];
			array[0] = 500f;
			array[1] = 700f;
			this.recoilTorques = array;
			this.recoilForces = new float[] { 0f, 0f, 600f, 800f, -3000f, -2000f };
			base..ctor();
		}

		// Token: 0x04000030 RID: 48
		public string projectileID;

		// Token: 0x04000031 RID: 49
		public string muzzlePositionRef;

		// Token: 0x04000032 RID: 50
		public string npcRaycastPositionRef;

		// Token: 0x04000033 RID: 51
		public float npcDistanceToFire = 10f;

		// Token: 0x04000034 RID: 52
		public bool npcMeleeEnableFlag = true;

		// Token: 0x04000035 RID: 53
		public string fireSoundRef;

		// Token: 0x04000036 RID: 54
		public string emptySoundRef;

		// Token: 0x04000037 RID: 55
		public string swtichSoundRef;

		// Token: 0x04000038 RID: 56
		public string reloadSoundRef;

		// Token: 0x04000039 RID: 57
		public string muzzleFlashRef;

		// Token: 0x0400003A RID: 58
		public string animatorRef;

		// Token: 0x0400003B RID: 59
		public string fireAnim;

		// Token: 0x0400003C RID: 60
		public string emptyAnim;

		// Token: 0x0400003D RID: 61
		public string reloadAnim;

		// Token: 0x0400003E RID: 62
		public string mainGripID;

		// Token: 0x0400003F RID: 63
		public int ammoCapacity;

		// Token: 0x04000040 RID: 64
		public bool allowCycleFireMode;

		// Token: 0x04000041 RID: 65
		public int fireMode = 1;

		// Token: 0x04000042 RID: 66
		public int burstNumber = 3;

		// Token: 0x04000043 RID: 67
		public int fireRate = 400;

		// Token: 0x04000044 RID: 68
		public float bulletForce = 7f;

		// Token: 0x04000045 RID: 69
		public float recoilMult = 1f;

		// Token: 0x04000046 RID: 70
		public float soundVolume;

		// Token: 0x04000047 RID: 71
		public float hapticForce = 4f;

		// Token: 0x04000048 RID: 72
		public float throwMult = 2f;

		// Token: 0x04000049 RID: 73
		public float[] recoilTorques;

		// Token: 0x0400004A RID: 74
		public float[] recoilForces;
	}
}
