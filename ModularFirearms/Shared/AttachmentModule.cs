using System;
using ModularFirearms.Attachments;
using ThunderRoad;

namespace ModularFirearms.Shared
{
	// Token: 0x02000012 RID: 18
	public class AttachmentModule : ItemModule
	{
		// Token: 0x06000099 RID: 153 RVA: 0x00007688 File Offset: 0x00005888
		public FrameworkCore.AttachmentType GetSelectedType(string attachmentType)
		{
			return (FrameworkCore.AttachmentType)Enum.Parse(typeof(FrameworkCore.AttachmentType), attachmentType);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x000076A0 File Offset: 0x000058A0
		public override void OnItemLoaded(Item item)
		{
			base.OnItemLoaded(item);
			foreach (string text in this.attachmentTypes)
			{
				this.selectedType = this.GetSelectedType(text);
				if (this.selectedType.Equals(FrameworkCore.AttachmentType.Flashlight))
				{
					item.gameObject.AddComponent<FlashlightController>();
				}
				else if (this.selectedType.Equals(FrameworkCore.AttachmentType.Laser))
				{
					item.gameObject.AddComponent<LaserController>();
				}
				else if (this.selectedType.Equals(FrameworkCore.AttachmentType.Compass))
				{
					item.gameObject.AddComponent<CompassController>();
				}
				else if (this.selectedType.Equals(FrameworkCore.AttachmentType.AmmoCounter))
				{
					item.gameObject.AddComponent<AmmoCounterController>();
				}
				else if (this.selectedType.Equals(FrameworkCore.AttachmentType.FireModeSwitch))
				{
					item.gameObject.AddComponent<FireModeSwitchController>();
				}
				else if (this.selectedType.Equals(FrameworkCore.AttachmentType.GrenadeLauncher))
				{
					item.gameObject.AddComponent<GrenadeLauncherController>();
				}
				else if (this.selectedType.Equals(FrameworkCore.AttachmentType.SecondaryFire))
				{
					item.gameObject.AddComponent<SecondaryFire>();
				}
			}
		}

		// Token: 0x040000DE RID: 222
		private FrameworkCore.AttachmentType selectedType;

		// Token: 0x040000DF RID: 223
		public string[] attachmentTypes;

		// Token: 0x040000E0 RID: 224
		public string attachmentRef;

		// Token: 0x040000E1 RID: 225
		public string attachmentHandleRef;

		// Token: 0x040000E2 RID: 226
		public string activationSoundRef;

		// Token: 0x040000E3 RID: 227
		public string ignoredMeshRef;

		// Token: 0x040000E4 RID: 228
		public string rayCastPointRef;

		// Token: 0x040000E5 RID: 229
		public bool longPressToActivate = true;

		// Token: 0x040000E6 RID: 230
		public float longPressTime = 0.25f;

		// Token: 0x040000E7 RID: 231
		public string swtichRef;

		// Token: 0x040000E8 RID: 232
		public string switchActivationSoundRef;

		// Token: 0x040000E9 RID: 233
		public string swtichHandleRef;

		// Token: 0x040000EA RID: 234
		public string[] allowedFireModes = new string[0];

		// Token: 0x040000EB RID: 235
		public string[] switchPositionRefs = new string[0];

		// Token: 0x040000EC RID: 236
		public string laserRef;

		// Token: 0x040000ED RID: 237
		public string laserStartRef;

		// Token: 0x040000EE RID: 238
		public string laserEndRef;

		// Token: 0x040000EF RID: 239
		public float maxLaserDistance = 10f;

		// Token: 0x040000F0 RID: 240
		public string laserHandleRef;

		// Token: 0x040000F1 RID: 241
		public string laserRayCastPointRef;

		// Token: 0x040000F2 RID: 242
		public string laserActivationSoundRef;

		// Token: 0x040000F3 RID: 243
		public bool laserStartActivated = true;

		// Token: 0x040000F4 RID: 244
		public string ammoCounterRef;

		// Token: 0x040000F5 RID: 245
		public string compassRef;

		// Token: 0x040000F6 RID: 246
		public string flashlightRef;

		// Token: 0x040000F7 RID: 247
		public string flashlightHandleRef;

		// Token: 0x040000F8 RID: 248
		public string flashlightActivationSoundRef;

		// Token: 0x040000F9 RID: 249
		public string flashlightMeshRef;

		// Token: 0x040000FA RID: 250
		public float fireDelay = 1f;

		// Token: 0x040000FB RID: 251
		public float forceMult = 100f;

		// Token: 0x040000FC RID: 252
		public float throwMult = 1f;

		// Token: 0x040000FD RID: 253
		public string projectileID;

		// Token: 0x040000FE RID: 254
		public string muzzlePositionRef;

		// Token: 0x040000FF RID: 255
		public string fireSoundRef;

		// Token: 0x04000100 RID: 256
		public string muzzleFlashRef;

		// Token: 0x04000101 RID: 257
		public string fireAnim;

		// Token: 0x04000102 RID: 258
		public string mainGripID;
	}
}
