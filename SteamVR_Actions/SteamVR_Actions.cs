using System;

namespace Valve.VR
{
	// Token: 0x02000003 RID: 3
	public class SteamVR_Actions
	{
		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000023 RID: 35 RVA: 0x00002140 File Offset: 0x00000340
		public static SteamVR_Input_ActionSet_Default Default
		{
			get
			{
				return SteamVR_Actions.p_Default.GetCopy<SteamVR_Input_ActionSet_Default>();
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x0000214C File Offset: 0x0000034C
		private static void StartPreInitActionSets()
		{
			SteamVR_Actions.p_Default = SteamVR_ActionSet.Create<SteamVR_Input_ActionSet_Default>("/actions/Default");
			SteamVR_Input.actionSets = new SteamVR_ActionSet[] { SteamVR_Actions.Default };
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000025 RID: 37 RVA: 0x00002171 File Offset: 0x00000371
		public static SteamVR_Action_Boolean default_GrabPinch
		{
			get
			{
				return SteamVR_Actions.p_default_GrabPinch.GetCopy<SteamVR_Action_Boolean>();
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000026 RID: 38 RVA: 0x0000217D File Offset: 0x0000037D
		public static SteamVR_Action_Boolean default_GrabGrip
		{
			get
			{
				return SteamVR_Actions.p_default_GrabGrip.GetCopy<SteamVR_Action_Boolean>();
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000027 RID: 39 RVA: 0x00002189 File Offset: 0x00000389
		public static SteamVR_Action_Single default_GripAxis
		{
			get
			{
				return SteamVR_Actions.p_default_GripAxis.GetCopy<SteamVR_Action_Single>();
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000028 RID: 40 RVA: 0x00002195 File Offset: 0x00000395
		public static SteamVR_Action_Pose default_Pose
		{
			get
			{
				return SteamVR_Actions.p_default_Pose.GetCopy<SteamVR_Action_Pose>();
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000029 RID: 41 RVA: 0x000021A1 File Offset: 0x000003A1
		public static SteamVR_Action_Skeleton default_SkeletonLeftHand
		{
			get
			{
				return SteamVR_Actions.p_default_SkeletonLeftHand.GetCopy<SteamVR_Action_Skeleton>();
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600002A RID: 42 RVA: 0x000021AD File Offset: 0x000003AD
		public static SteamVR_Action_Skeleton default_SkeletonRightHand
		{
			get
			{
				return SteamVR_Actions.p_default_SkeletonRightHand.GetCopy<SteamVR_Action_Skeleton>();
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600002B RID: 43 RVA: 0x000021B9 File Offset: 0x000003B9
		public static SteamVR_Action_Boolean default_HeadsetOnHead
		{
			get
			{
				return SteamVR_Actions.p_default_HeadsetOnHead.GetCopy<SteamVR_Action_Boolean>();
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600002C RID: 44 RVA: 0x000021C5 File Offset: 0x000003C5
		public static SteamVR_Action_Vector2 default_Move
		{
			get
			{
				return SteamVR_Actions.p_default_Move.GetCopy<SteamVR_Action_Vector2>();
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600002D RID: 45 RVA: 0x000021D1 File Offset: 0x000003D1
		public static SteamVR_Action_Vector2 default_Turn
		{
			get
			{
				return SteamVR_Actions.p_default_Turn.GetCopy<SteamVR_Action_Vector2>();
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600002E RID: 46 RVA: 0x000021DD File Offset: 0x000003DD
		public static SteamVR_Action_Boolean default_Menu
		{
			get
			{
				return SteamVR_Actions.p_default_Menu.GetCopy<SteamVR_Action_Boolean>();
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600002F RID: 47 RVA: 0x000021E9 File Offset: 0x000003E9
		public static SteamVR_Action_Boolean default_SpellSelector
		{
			get
			{
				return SteamVR_Actions.p_default_SpellSelector.GetCopy<SteamVR_Action_Boolean>();
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000030 RID: 48 RVA: 0x000021F5 File Offset: 0x000003F5
		public static SteamVR_Action_Boolean default_SlowTime
		{
			get
			{
				return SteamVR_Actions.p_default_SlowTime.GetCopy<SteamVR_Action_Boolean>();
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000031 RID: 49 RVA: 0x00002201 File Offset: 0x00000401
		public static SteamVR_Action_Boolean default_Kick
		{
			get
			{
				return SteamVR_Actions.p_default_Kick.GetCopy<SteamVR_Action_Boolean>();
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000032 RID: 50 RVA: 0x0000220D File Offset: 0x0000040D
		public static SteamVR_Action_Boolean default_Jump
		{
			get
			{
				return SteamVR_Actions.p_default_Jump.GetCopy<SteamVR_Action_Boolean>();
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000033 RID: 51 RVA: 0x00002219 File Offset: 0x00000419
		public static SteamVR_Action_Boolean default_ClickUI
		{
			get
			{
				return SteamVR_Actions.p_default_ClickUI.GetCopy<SteamVR_Action_Boolean>();
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000034 RID: 52 RVA: 0x00002225 File Offset: 0x00000425
		public static SteamVR_Action_Vector2 default_Height
		{
			get
			{
				return SteamVR_Actions.p_default_Height.GetCopy<SteamVR_Action_Vector2>();
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000035 RID: 53 RVA: 0x00002231 File Offset: 0x00000431
		public static SteamVR_Action_Boolean default_ExternalView
		{
			get
			{
				return SteamVR_Actions.p_default_ExternalView.GetCopy<SteamVR_Action_Boolean>();
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000036 RID: 54 RVA: 0x0000223D File Offset: 0x0000043D
		public static SteamVR_Action_Boolean default_Screenshot
		{
			get
			{
				return SteamVR_Actions.p_default_Screenshot.GetCopy<SteamVR_Action_Boolean>();
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000037 RID: 55 RVA: 0x00002249 File Offset: 0x00000449
		public static SteamVR_Action_Boolean default_ExternalViewLock
		{
			get
			{
				return SteamVR_Actions.p_default_ExternalViewLock.GetCopy<SteamVR_Action_Boolean>();
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000038 RID: 56 RVA: 0x00002255 File Offset: 0x00000455
		public static SteamVR_Action_Vector2 default_ScrollUI
		{
			get
			{
				return SteamVR_Actions.p_default_ScrollUI.GetCopy<SteamVR_Action_Vector2>();
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000039 RID: 57 RVA: 0x00002261 File Offset: 0x00000461
		public static SteamVR_Action_Single default_Cast
		{
			get
			{
				return SteamVR_Actions.p_default_Cast.GetCopy<SteamVR_Action_Single>();
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x0600003A RID: 58 RVA: 0x0000226D File Offset: 0x0000046D
		public static SteamVR_Action_Single default_Use
		{
			get
			{
				return SteamVR_Actions.p_default_Use.GetCopy<SteamVR_Action_Single>();
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x0600003B RID: 59 RVA: 0x00002279 File Offset: 0x00000479
		public static SteamVR_Action_Single default_AlternateUse
		{
			get
			{
				return SteamVR_Actions.p_default_AlternateUse.GetCopy<SteamVR_Action_Single>();
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00002285 File Offset: 0x00000485
		public static SteamVR_Action_Boolean default_Telekinesis
		{
			get
			{
				return SteamVR_Actions.p_default_Telekinesis.GetCopy<SteamVR_Action_Boolean>();
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00002291 File Offset: 0x00000491
		public static SteamVR_Action_Vector2 default_TelekinesisAxis
		{
			get
			{
				return SteamVR_Actions.p_default_TelekinesisAxis.GetCopy<SteamVR_Action_Vector2>();
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600003E RID: 62 RVA: 0x0000229D File Offset: 0x0000049D
		public static SteamVR_Action_Single default_TelekinesisPull
		{
			get
			{
				return SteamVR_Actions.p_default_TelekinesisPull.GetCopy<SteamVR_Action_Single>();
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600003F RID: 63 RVA: 0x000022A9 File Offset: 0x000004A9
		public static SteamVR_Action_Single default_TelekinesisRepel
		{
			get
			{
				return SteamVR_Actions.p_default_TelekinesisRepel.GetCopy<SteamVR_Action_Single>();
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000040 RID: 64 RVA: 0x000022B5 File Offset: 0x000004B5
		public static SteamVR_Action_Boolean default_TelekinesisSpin
		{
			get
			{
				return SteamVR_Actions.p_default_TelekinesisSpin.GetCopy<SteamVR_Action_Boolean>();
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000041 RID: 65 RVA: 0x000022C1 File Offset: 0x000004C1
		public static SteamVR_Action_Boolean default_TelekinesisRepel2
		{
			get
			{
				return SteamVR_Actions.p_default_TelekinesisRepel2.GetCopy<SteamVR_Action_Boolean>();
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000042 RID: 66 RVA: 0x000022CD File Offset: 0x000004CD
		public static SteamVR_Action_Boolean default_AlternateUse2
		{
			get
			{
				return SteamVR_Actions.p_default_AlternateUse2.GetCopy<SteamVR_Action_Boolean>();
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000043 RID: 67 RVA: 0x000022D9 File Offset: 0x000004D9
		public static SteamVR_Action_Boolean default_MovePress
		{
			get
			{
				return SteamVR_Actions.p_default_MovePress.GetCopy<SteamVR_Action_Boolean>();
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000044 RID: 68 RVA: 0x000022E5 File Offset: 0x000004E5
		public static SteamVR_Action_Boolean default_TurnPress
		{
			get
			{
				return SteamVR_Actions.p_default_TurnPress.GetCopy<SteamVR_Action_Boolean>();
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000045 RID: 69 RVA: 0x000022F1 File Offset: 0x000004F1
		public static SteamVR_Action_Vibration default_Haptic
		{
			get
			{
				return SteamVR_Actions.p_default_Haptic.GetCopy<SteamVR_Action_Vibration>();
			}
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002300 File Offset: 0x00000500
		private static void InitializeActionArrays()
		{
			SteamVR_Input.actions = new SteamVR_Action[]
			{
				SteamVR_Actions.default_GrabPinch,
				SteamVR_Actions.default_GrabGrip,
				SteamVR_Actions.default_GripAxis,
				SteamVR_Actions.default_Pose,
				SteamVR_Actions.default_SkeletonLeftHand,
				SteamVR_Actions.default_SkeletonRightHand,
				SteamVR_Actions.default_HeadsetOnHead,
				SteamVR_Actions.default_Move,
				SteamVR_Actions.default_Turn,
				SteamVR_Actions.default_Menu,
				SteamVR_Actions.default_SpellSelector,
				SteamVR_Actions.default_SlowTime,
				SteamVR_Actions.default_Kick,
				SteamVR_Actions.default_Jump,
				SteamVR_Actions.default_ClickUI,
				SteamVR_Actions.default_Height,
				SteamVR_Actions.default_ExternalView,
				SteamVR_Actions.default_Screenshot,
				SteamVR_Actions.default_ExternalViewLock,
				SteamVR_Actions.default_ScrollUI,
				SteamVR_Actions.default_Cast,
				SteamVR_Actions.default_Use,
				SteamVR_Actions.default_AlternateUse,
				SteamVR_Actions.default_Telekinesis,
				SteamVR_Actions.default_TelekinesisAxis,
				SteamVR_Actions.default_TelekinesisPull,
				SteamVR_Actions.default_TelekinesisRepel,
				SteamVR_Actions.default_TelekinesisSpin,
				SteamVR_Actions.default_TelekinesisRepel2,
				SteamVR_Actions.default_AlternateUse2,
				SteamVR_Actions.default_MovePress,
				SteamVR_Actions.default_TurnPress,
				SteamVR_Actions.default_Haptic
			};
			SteamVR_Input.actionsIn = new ISteamVR_Action_In[]
			{
				SteamVR_Actions.default_GrabPinch,
				SteamVR_Actions.default_GrabGrip,
				SteamVR_Actions.default_GripAxis,
				SteamVR_Actions.default_Pose,
				SteamVR_Actions.default_SkeletonLeftHand,
				SteamVR_Actions.default_SkeletonRightHand,
				SteamVR_Actions.default_HeadsetOnHead,
				SteamVR_Actions.default_Move,
				SteamVR_Actions.default_Turn,
				SteamVR_Actions.default_Menu,
				SteamVR_Actions.default_SpellSelector,
				SteamVR_Actions.default_SlowTime,
				SteamVR_Actions.default_Kick,
				SteamVR_Actions.default_Jump,
				SteamVR_Actions.default_ClickUI,
				SteamVR_Actions.default_Height,
				SteamVR_Actions.default_ExternalView,
				SteamVR_Actions.default_Screenshot,
				SteamVR_Actions.default_ExternalViewLock,
				SteamVR_Actions.default_ScrollUI,
				SteamVR_Actions.default_Cast,
				SteamVR_Actions.default_Use,
				SteamVR_Actions.default_AlternateUse,
				SteamVR_Actions.default_Telekinesis,
				SteamVR_Actions.default_TelekinesisAxis,
				SteamVR_Actions.default_TelekinesisPull,
				SteamVR_Actions.default_TelekinesisRepel,
				SteamVR_Actions.default_TelekinesisSpin,
				SteamVR_Actions.default_TelekinesisRepel2,
				SteamVR_Actions.default_AlternateUse2,
				SteamVR_Actions.default_MovePress,
				SteamVR_Actions.default_TurnPress
			};
			SteamVR_Input.actionsOut = new ISteamVR_Action_Out[] { SteamVR_Actions.default_Haptic };
			SteamVR_Input.actionsVibration = new SteamVR_Action_Vibration[] { SteamVR_Actions.default_Haptic };
			SteamVR_Input.actionsPose = new SteamVR_Action_Pose[] { SteamVR_Actions.default_Pose };
			SteamVR_Input.actionsBoolean = new SteamVR_Action_Boolean[]
			{
				SteamVR_Actions.default_GrabPinch,
				SteamVR_Actions.default_GrabGrip,
				SteamVR_Actions.default_HeadsetOnHead,
				SteamVR_Actions.default_Menu,
				SteamVR_Actions.default_SpellSelector,
				SteamVR_Actions.default_SlowTime,
				SteamVR_Actions.default_Kick,
				SteamVR_Actions.default_Jump,
				SteamVR_Actions.default_ClickUI,
				SteamVR_Actions.default_ExternalView,
				SteamVR_Actions.default_Screenshot,
				SteamVR_Actions.default_ExternalViewLock,
				SteamVR_Actions.default_Telekinesis,
				SteamVR_Actions.default_TelekinesisSpin,
				SteamVR_Actions.default_TelekinesisRepel2,
				SteamVR_Actions.default_AlternateUse2,
				SteamVR_Actions.default_MovePress,
				SteamVR_Actions.default_TurnPress
			};
			SteamVR_Input.actionsSingle = new SteamVR_Action_Single[]
			{
				SteamVR_Actions.default_GripAxis,
				SteamVR_Actions.default_Cast,
				SteamVR_Actions.default_Use,
				SteamVR_Actions.default_AlternateUse,
				SteamVR_Actions.default_TelekinesisPull,
				SteamVR_Actions.default_TelekinesisRepel
			};
			SteamVR_Input.actionsVector2 = new SteamVR_Action_Vector2[]
			{
				SteamVR_Actions.default_Move,
				SteamVR_Actions.default_Turn,
				SteamVR_Actions.default_Height,
				SteamVR_Actions.default_ScrollUI,
				SteamVR_Actions.default_TelekinesisAxis
			};
			SteamVR_Input.actionsVector3 = new SteamVR_Action_Vector3[0];
			SteamVR_Input.actionsSkeleton = new SteamVR_Action_Skeleton[]
			{
				SteamVR_Actions.default_SkeletonLeftHand,
				SteamVR_Actions.default_SkeletonRightHand
			};
			SteamVR_Input.actionsNonPoseNonSkeletonIn = new ISteamVR_Action_In[]
			{
				SteamVR_Actions.default_GrabPinch,
				SteamVR_Actions.default_GrabGrip,
				SteamVR_Actions.default_GripAxis,
				SteamVR_Actions.default_HeadsetOnHead,
				SteamVR_Actions.default_Move,
				SteamVR_Actions.default_Turn,
				SteamVR_Actions.default_Menu,
				SteamVR_Actions.default_SpellSelector,
				SteamVR_Actions.default_SlowTime,
				SteamVR_Actions.default_Kick,
				SteamVR_Actions.default_Jump,
				SteamVR_Actions.default_ClickUI,
				SteamVR_Actions.default_Height,
				SteamVR_Actions.default_ExternalView,
				SteamVR_Actions.default_Screenshot,
				SteamVR_Actions.default_ExternalViewLock,
				SteamVR_Actions.default_ScrollUI,
				SteamVR_Actions.default_Cast,
				SteamVR_Actions.default_Use,
				SteamVR_Actions.default_AlternateUse,
				SteamVR_Actions.default_Telekinesis,
				SteamVR_Actions.default_TelekinesisAxis,
				SteamVR_Actions.default_TelekinesisPull,
				SteamVR_Actions.default_TelekinesisRepel,
				SteamVR_Actions.default_TelekinesisSpin,
				SteamVR_Actions.default_TelekinesisRepel2,
				SteamVR_Actions.default_AlternateUse2,
				SteamVR_Actions.default_MovePress,
				SteamVR_Actions.default_TurnPress
			};
		}

		// Token: 0x06000047 RID: 71 RVA: 0x000027D8 File Offset: 0x000009D8
		private static void PreInitActions()
		{
			SteamVR_Actions.p_default_GrabPinch = SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/Default/in/GrabPinch");
			SteamVR_Actions.p_default_GrabGrip = SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/Default/in/GrabGrip");
			SteamVR_Actions.p_default_GripAxis = SteamVR_Action.Create<SteamVR_Action_Single>("/actions/Default/in/GripAxis");
			SteamVR_Actions.p_default_Pose = SteamVR_Action.Create<SteamVR_Action_Pose>("/actions/Default/in/Pose");
			SteamVR_Actions.p_default_SkeletonLeftHand = SteamVR_Action.Create<SteamVR_Action_Skeleton>("/actions/Default/in/SkeletonLeftHand");
			SteamVR_Actions.p_default_SkeletonRightHand = SteamVR_Action.Create<SteamVR_Action_Skeleton>("/actions/Default/in/SkeletonRightHand");
			SteamVR_Actions.p_default_HeadsetOnHead = SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/Default/in/HeadsetOnHead");
			SteamVR_Actions.p_default_Move = SteamVR_Action.Create<SteamVR_Action_Vector2>("/actions/Default/in/Move");
			SteamVR_Actions.p_default_Turn = SteamVR_Action.Create<SteamVR_Action_Vector2>("/actions/Default/in/Turn");
			SteamVR_Actions.p_default_Menu = SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/Default/in/Menu");
			SteamVR_Actions.p_default_SpellSelector = SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/Default/in/SpellSelector");
			SteamVR_Actions.p_default_SlowTime = SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/Default/in/SlowTime");
			SteamVR_Actions.p_default_Kick = SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/Default/in/Kick");
			SteamVR_Actions.p_default_Jump = SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/Default/in/Jump");
			SteamVR_Actions.p_default_ClickUI = SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/Default/in/ClickUI");
			SteamVR_Actions.p_default_Height = SteamVR_Action.Create<SteamVR_Action_Vector2>("/actions/Default/in/Height");
			SteamVR_Actions.p_default_ExternalView = SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/Default/in/ExternalView");
			SteamVR_Actions.p_default_Screenshot = SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/Default/in/Screenshot");
			SteamVR_Actions.p_default_ExternalViewLock = SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/Default/in/ExternalViewLock");
			SteamVR_Actions.p_default_ScrollUI = SteamVR_Action.Create<SteamVR_Action_Vector2>("/actions/Default/in/ScrollUI");
			SteamVR_Actions.p_default_Cast = SteamVR_Action.Create<SteamVR_Action_Single>("/actions/Default/in/Cast");
			SteamVR_Actions.p_default_Use = SteamVR_Action.Create<SteamVR_Action_Single>("/actions/Default/in/Use");
			SteamVR_Actions.p_default_AlternateUse = SteamVR_Action.Create<SteamVR_Action_Single>("/actions/Default/in/AlternateUse");
			SteamVR_Actions.p_default_Telekinesis = SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/Default/in/Telekinesis");
			SteamVR_Actions.p_default_TelekinesisAxis = SteamVR_Action.Create<SteamVR_Action_Vector2>("/actions/Default/in/TelekinesisAxis");
			SteamVR_Actions.p_default_TelekinesisPull = SteamVR_Action.Create<SteamVR_Action_Single>("/actions/Default/in/TelekinesisPull");
			SteamVR_Actions.p_default_TelekinesisRepel = SteamVR_Action.Create<SteamVR_Action_Single>("/actions/Default/in/TelekinesisRepel");
			SteamVR_Actions.p_default_TelekinesisSpin = SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/Default/in/TelekinesisSpin");
			SteamVR_Actions.p_default_TelekinesisRepel2 = SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/Default/in/TelekinesisRepel2");
			SteamVR_Actions.p_default_AlternateUse2 = SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/Default/in/AlternateUse2");
			SteamVR_Actions.p_default_MovePress = SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/Default/in/MovePress");
			SteamVR_Actions.p_default_TurnPress = SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/Default/in/TurnPress");
			SteamVR_Actions.p_default_Haptic = SteamVR_Action.Create<SteamVR_Action_Vibration>("/actions/Default/out/Haptic");
		}

		// Token: 0x06000048 RID: 72 RVA: 0x000029D5 File Offset: 0x00000BD5
		public static void PreInitialize()
		{
			SteamVR_Actions.StartPreInitActionSets();
			SteamVR_Input.PreinitializeActionSetDictionaries();
			SteamVR_Actions.PreInitActions();
			SteamVR_Actions.InitializeActionArrays();
			SteamVR_Input.PreinitializeActionDictionaries();
			SteamVR_Input.PreinitializeFinishActionSets();
		}

		// Token: 0x04000001 RID: 1
		private static SteamVR_Input_ActionSet_Default p_Default;

		// Token: 0x04000002 RID: 2
		private static SteamVR_Action_Boolean p_default_GrabPinch;

		// Token: 0x04000003 RID: 3
		private static SteamVR_Action_Boolean p_default_GrabGrip;

		// Token: 0x04000004 RID: 4
		private static SteamVR_Action_Single p_default_GripAxis;

		// Token: 0x04000005 RID: 5
		private static SteamVR_Action_Pose p_default_Pose;

		// Token: 0x04000006 RID: 6
		private static SteamVR_Action_Skeleton p_default_SkeletonLeftHand;

		// Token: 0x04000007 RID: 7
		private static SteamVR_Action_Skeleton p_default_SkeletonRightHand;

		// Token: 0x04000008 RID: 8
		private static SteamVR_Action_Boolean p_default_HeadsetOnHead;

		// Token: 0x04000009 RID: 9
		private static SteamVR_Action_Vector2 p_default_Move;

		// Token: 0x0400000A RID: 10
		private static SteamVR_Action_Vector2 p_default_Turn;

		// Token: 0x0400000B RID: 11
		private static SteamVR_Action_Boolean p_default_Menu;

		// Token: 0x0400000C RID: 12
		private static SteamVR_Action_Boolean p_default_SpellSelector;

		// Token: 0x0400000D RID: 13
		private static SteamVR_Action_Boolean p_default_SlowTime;

		// Token: 0x0400000E RID: 14
		private static SteamVR_Action_Boolean p_default_Kick;

		// Token: 0x0400000F RID: 15
		private static SteamVR_Action_Boolean p_default_Jump;

		// Token: 0x04000010 RID: 16
		private static SteamVR_Action_Boolean p_default_ClickUI;

		// Token: 0x04000011 RID: 17
		private static SteamVR_Action_Vector2 p_default_Height;

		// Token: 0x04000012 RID: 18
		private static SteamVR_Action_Boolean p_default_ExternalView;

		// Token: 0x04000013 RID: 19
		private static SteamVR_Action_Boolean p_default_Screenshot;

		// Token: 0x04000014 RID: 20
		private static SteamVR_Action_Boolean p_default_ExternalViewLock;

		// Token: 0x04000015 RID: 21
		private static SteamVR_Action_Vector2 p_default_ScrollUI;

		// Token: 0x04000016 RID: 22
		private static SteamVR_Action_Single p_default_Cast;

		// Token: 0x04000017 RID: 23
		private static SteamVR_Action_Single p_default_Use;

		// Token: 0x04000018 RID: 24
		private static SteamVR_Action_Single p_default_AlternateUse;

		// Token: 0x04000019 RID: 25
		private static SteamVR_Action_Boolean p_default_Telekinesis;

		// Token: 0x0400001A RID: 26
		private static SteamVR_Action_Vector2 p_default_TelekinesisAxis;

		// Token: 0x0400001B RID: 27
		private static SteamVR_Action_Single p_default_TelekinesisPull;

		// Token: 0x0400001C RID: 28
		private static SteamVR_Action_Single p_default_TelekinesisRepel;

		// Token: 0x0400001D RID: 29
		private static SteamVR_Action_Boolean p_default_TelekinesisSpin;

		// Token: 0x0400001E RID: 30
		private static SteamVR_Action_Boolean p_default_TelekinesisRepel2;

		// Token: 0x0400001F RID: 31
		private static SteamVR_Action_Boolean p_default_AlternateUse2;

		// Token: 0x04000020 RID: 32
		private static SteamVR_Action_Boolean p_default_MovePress;

		// Token: 0x04000021 RID: 33
		private static SteamVR_Action_Boolean p_default_TurnPress;

		// Token: 0x04000022 RID: 34
		private static SteamVR_Action_Vibration p_default_Haptic;
	}
}
