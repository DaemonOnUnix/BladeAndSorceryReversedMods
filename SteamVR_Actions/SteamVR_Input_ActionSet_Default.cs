using System;

namespace Valve.VR
{
	// Token: 0x02000002 RID: 2
	public class SteamVR_Input_ActionSet_Default : SteamVR_ActionSet
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public virtual SteamVR_Action_Boolean GrabPinch
		{
			get
			{
				return SteamVR_Actions.default_GrabPinch;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000002 RID: 2 RVA: 0x00002057 File Offset: 0x00000257
		public virtual SteamVR_Action_Boolean GrabGrip
		{
			get
			{
				return SteamVR_Actions.default_GrabGrip;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000003 RID: 3 RVA: 0x0000205E File Offset: 0x0000025E
		public virtual SteamVR_Action_Single GripAxis
		{
			get
			{
				return SteamVR_Actions.default_GripAxis;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000004 RID: 4 RVA: 0x00002065 File Offset: 0x00000265
		public virtual SteamVR_Action_Pose Pose
		{
			get
			{
				return SteamVR_Actions.default_Pose;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000005 RID: 5 RVA: 0x0000206C File Offset: 0x0000026C
		public virtual SteamVR_Action_Skeleton SkeletonLeftHand
		{
			get
			{
				return SteamVR_Actions.default_SkeletonLeftHand;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000006 RID: 6 RVA: 0x00002073 File Offset: 0x00000273
		public virtual SteamVR_Action_Skeleton SkeletonRightHand
		{
			get
			{
				return SteamVR_Actions.default_SkeletonRightHand;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000007 RID: 7 RVA: 0x0000207A File Offset: 0x0000027A
		public virtual SteamVR_Action_Boolean HeadsetOnHead
		{
			get
			{
				return SteamVR_Actions.default_HeadsetOnHead;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000008 RID: 8 RVA: 0x00002081 File Offset: 0x00000281
		public virtual SteamVR_Action_Vector2 Move
		{
			get
			{
				return SteamVR_Actions.default_Move;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000009 RID: 9 RVA: 0x00002088 File Offset: 0x00000288
		public virtual SteamVR_Action_Vector2 Turn
		{
			get
			{
				return SteamVR_Actions.default_Turn;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600000A RID: 10 RVA: 0x0000208F File Offset: 0x0000028F
		public virtual SteamVR_Action_Boolean Menu
		{
			get
			{
				return SteamVR_Actions.default_Menu;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600000B RID: 11 RVA: 0x00002096 File Offset: 0x00000296
		public virtual SteamVR_Action_Boolean SpellSelector
		{
			get
			{
				return SteamVR_Actions.default_SpellSelector;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600000C RID: 12 RVA: 0x0000209D File Offset: 0x0000029D
		public virtual SteamVR_Action_Boolean SlowTime
		{
			get
			{
				return SteamVR_Actions.default_SlowTime;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600000D RID: 13 RVA: 0x000020A4 File Offset: 0x000002A4
		public virtual SteamVR_Action_Boolean Kick
		{
			get
			{
				return SteamVR_Actions.default_Kick;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600000E RID: 14 RVA: 0x000020AB File Offset: 0x000002AB
		public virtual SteamVR_Action_Boolean Jump
		{
			get
			{
				return SteamVR_Actions.default_Jump;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600000F RID: 15 RVA: 0x000020B2 File Offset: 0x000002B2
		public virtual SteamVR_Action_Boolean ClickUI
		{
			get
			{
				return SteamVR_Actions.default_ClickUI;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000010 RID: 16 RVA: 0x000020B9 File Offset: 0x000002B9
		public virtual SteamVR_Action_Vector2 Height
		{
			get
			{
				return SteamVR_Actions.default_Height;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000011 RID: 17 RVA: 0x000020C0 File Offset: 0x000002C0
		public virtual SteamVR_Action_Boolean ExternalView
		{
			get
			{
				return SteamVR_Actions.default_ExternalView;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000012 RID: 18 RVA: 0x000020C7 File Offset: 0x000002C7
		public virtual SteamVR_Action_Boolean Screenshot
		{
			get
			{
				return SteamVR_Actions.default_Screenshot;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000013 RID: 19 RVA: 0x000020CE File Offset: 0x000002CE
		public virtual SteamVR_Action_Boolean ExternalViewLock
		{
			get
			{
				return SteamVR_Actions.default_ExternalViewLock;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000014 RID: 20 RVA: 0x000020D5 File Offset: 0x000002D5
		public virtual SteamVR_Action_Vector2 ScrollUI
		{
			get
			{
				return SteamVR_Actions.default_ScrollUI;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000015 RID: 21 RVA: 0x000020DC File Offset: 0x000002DC
		public virtual SteamVR_Action_Single Cast
		{
			get
			{
				return SteamVR_Actions.default_Cast;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000016 RID: 22 RVA: 0x000020E3 File Offset: 0x000002E3
		public virtual SteamVR_Action_Single Use
		{
			get
			{
				return SteamVR_Actions.default_Use;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000017 RID: 23 RVA: 0x000020EA File Offset: 0x000002EA
		public virtual SteamVR_Action_Single AlternateUse
		{
			get
			{
				return SteamVR_Actions.default_AlternateUse;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000018 RID: 24 RVA: 0x000020F1 File Offset: 0x000002F1
		public virtual SteamVR_Action_Boolean Telekinesis
		{
			get
			{
				return SteamVR_Actions.default_Telekinesis;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000019 RID: 25 RVA: 0x000020F8 File Offset: 0x000002F8
		public virtual SteamVR_Action_Vector2 TelekinesisAxis
		{
			get
			{
				return SteamVR_Actions.default_TelekinesisAxis;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600001A RID: 26 RVA: 0x000020FF File Offset: 0x000002FF
		public virtual SteamVR_Action_Single TelekinesisPull
		{
			get
			{
				return SteamVR_Actions.default_TelekinesisPull;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600001B RID: 27 RVA: 0x00002106 File Offset: 0x00000306
		public virtual SteamVR_Action_Single TelekinesisRepel
		{
			get
			{
				return SteamVR_Actions.default_TelekinesisRepel;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600001C RID: 28 RVA: 0x0000210D File Offset: 0x0000030D
		public virtual SteamVR_Action_Boolean TelekinesisSpin
		{
			get
			{
				return SteamVR_Actions.default_TelekinesisSpin;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600001D RID: 29 RVA: 0x00002114 File Offset: 0x00000314
		public virtual SteamVR_Action_Boolean TelekinesisRepel2
		{
			get
			{
				return SteamVR_Actions.default_TelekinesisRepel2;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600001E RID: 30 RVA: 0x0000211B File Offset: 0x0000031B
		public virtual SteamVR_Action_Boolean AlternateUse2
		{
			get
			{
				return SteamVR_Actions.default_AlternateUse2;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600001F RID: 31 RVA: 0x00002122 File Offset: 0x00000322
		public virtual SteamVR_Action_Boolean MovePress
		{
			get
			{
				return SteamVR_Actions.default_MovePress;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000020 RID: 32 RVA: 0x00002129 File Offset: 0x00000329
		public virtual SteamVR_Action_Boolean TurnPress
		{
			get
			{
				return SteamVR_Actions.default_TurnPress;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000021 RID: 33 RVA: 0x00002130 File Offset: 0x00000330
		public virtual SteamVR_Action_Vibration Haptic
		{
			get
			{
				return SteamVR_Actions.default_Haptic;
			}
		}
	}
}
