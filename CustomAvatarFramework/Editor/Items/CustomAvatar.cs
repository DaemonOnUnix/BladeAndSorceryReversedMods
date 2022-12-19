using System;
using System.Collections.Generic;
using UnityEngine;

namespace CustomAvatarFramework.Editor.Items
{
	// Token: 0x0200001C RID: 28
	public class CustomAvatar : MonoBehaviour
	{
		// Token: 0x0400006F RID: 111
		public Animator animator;

		// Token: 0x04000070 RID: 112
		public Transform Rig_Mesh;

		// Token: 0x04000071 RID: 113
		public Transform Head_Mesh;

		// Token: 0x04000072 RID: 114
		public Transform Neck_Mesh;

		// Token: 0x04000073 RID: 115
		public Transform Jaw_Mesh;

		// Token: 0x04000074 RID: 116
		public Transform Spine1_Mesh;

		// Token: 0x04000075 RID: 117
		public Transform Spine_Mesh;

		// Token: 0x04000076 RID: 118
		public Transform Hips_Mesh;

		// Token: 0x04000077 RID: 119
		public Transform LeftUpLeg_Mesh;

		// Token: 0x04000078 RID: 120
		public Transform LeftLeg_Mesh;

		// Token: 0x04000079 RID: 121
		public Transform LeftFoot_Mesh;

		// Token: 0x0400007A RID: 122
		public Transform LeftToeBase_Mesh;

		// Token: 0x0400007B RID: 123
		public Transform RightUpLeg_Mesh;

		// Token: 0x0400007C RID: 124
		public Transform RightLeg_Mesh;

		// Token: 0x0400007D RID: 125
		public Transform RightFoot_Mesh;

		// Token: 0x0400007E RID: 126
		public Transform RightToeBase_Mesh;

		// Token: 0x0400007F RID: 127
		public Transform LeftShoulder_Mesh;

		// Token: 0x04000080 RID: 128
		public Transform LeftArm_Mesh;

		// Token: 0x04000081 RID: 129
		public Transform LeftForeArm_Mesh;

		// Token: 0x04000082 RID: 130
		public Transform LeftHand_Mesh;

		// Token: 0x04000083 RID: 131
		public Transform RightShoulder_Mesh;

		// Token: 0x04000084 RID: 132
		public Transform RightArm_Mesh;

		// Token: 0x04000085 RID: 133
		public Transform RightForeArm_Mesh;

		// Token: 0x04000086 RID: 134
		public Transform RightHand_Mesh;

		// Token: 0x04000087 RID: 135
		public Transform LeftFingerIndexProximal_Mesh;

		// Token: 0x04000088 RID: 136
		public Transform LeftFingerIndexIntermediate_Mesh;

		// Token: 0x04000089 RID: 137
		public Transform LeftFingerIndexDistal_Mesh;

		// Token: 0x0400008A RID: 138
		public Transform LeftFingerLittleProximal_Mesh;

		// Token: 0x0400008B RID: 139
		public Transform LeftFingerLittleIntermediate_Mesh;

		// Token: 0x0400008C RID: 140
		public Transform LeftFingerLittleDistal_Mesh;

		// Token: 0x0400008D RID: 141
		public Transform LeftFingerMiddleProximal_Mesh;

		// Token: 0x0400008E RID: 142
		public Transform LeftFingerMIddleIntermediate_Mesh;

		// Token: 0x0400008F RID: 143
		public Transform LeftFingerMiddleDistal_Mesh;

		// Token: 0x04000090 RID: 144
		public Transform LeftFingerRingProximal_Mesh;

		// Token: 0x04000091 RID: 145
		public Transform LeftFingerRingIntermediate_Mesh;

		// Token: 0x04000092 RID: 146
		public Transform LeftFingerRingDistal_Mesh;

		// Token: 0x04000093 RID: 147
		public Transform LeftFingerThumbProximal_Mesh;

		// Token: 0x04000094 RID: 148
		public Transform LeftFingerThumbIntermediate_Mesh;

		// Token: 0x04000095 RID: 149
		public Transform LeftFingerThumbDistal_Mesh;

		// Token: 0x04000096 RID: 150
		public Transform RightFingerIndexProximal_Mesh;

		// Token: 0x04000097 RID: 151
		public Transform RightFingerIndexIntermediate_Mesh;

		// Token: 0x04000098 RID: 152
		public Transform RightFingerIndexDistal_Mesh;

		// Token: 0x04000099 RID: 153
		public Transform RightFingerLittleProximal_Mesh;

		// Token: 0x0400009A RID: 154
		public Transform RightFingerLittleIntermediate_Mesh;

		// Token: 0x0400009B RID: 155
		public Transform RightFingerLittleDistal_Mesh;

		// Token: 0x0400009C RID: 156
		public Transform RightFingerMiddleProximal_Mesh;

		// Token: 0x0400009D RID: 157
		public Transform RightFingerMIddleIntermediate_Mesh;

		// Token: 0x0400009E RID: 158
		public Transform RightFingerMiddleDistal_Mesh;

		// Token: 0x0400009F RID: 159
		public Transform RightFingerRingProximal_Mesh;

		// Token: 0x040000A0 RID: 160
		public Transform RightFingerRingIntermediate_Mesh;

		// Token: 0x040000A1 RID: 161
		public Transform RightFingerRingDistal_Mesh;

		// Token: 0x040000A2 RID: 162
		public Transform RightFingerThumbProximal_Mesh;

		// Token: 0x040000A3 RID: 163
		public Transform RightFingerThumbIntermediate_Mesh;

		// Token: 0x040000A4 RID: 164
		public Transform RightFingerThumbDistal_Mesh;

		// Token: 0x040000A5 RID: 165
		public static Dictionary<string, HumanBodyBones?> boneMappers = new Dictionary<string, HumanBodyBones?>
		{
			{ "Rig_Mesh", null },
			{
				"Head_Mesh",
				new HumanBodyBones?(10)
			},
			{
				"Neck_Mesh",
				new HumanBodyBones?(9)
			},
			{
				"Jaw_Mesh",
				new HumanBodyBones?(23)
			},
			{
				"Spine1_Mesh",
				new HumanBodyBones?(8)
			},
			{
				"Spine_Mesh",
				new HumanBodyBones?(7)
			},
			{
				"Hips_Mesh",
				new HumanBodyBones?(0)
			},
			{
				"LeftUpLeg_Mesh",
				new HumanBodyBones?(1)
			},
			{
				"LeftLeg_Mesh",
				new HumanBodyBones?(3)
			},
			{
				"LeftFoot_Mesh",
				new HumanBodyBones?(5)
			},
			{
				"LeftToeBase_Mesh",
				new HumanBodyBones?(19)
			},
			{
				"RightUpLeg_Mesh",
				new HumanBodyBones?(2)
			},
			{
				"RightLeg_Mesh",
				new HumanBodyBones?(4)
			},
			{
				"RightFoot_Mesh",
				new HumanBodyBones?(6)
			},
			{
				"RightToeBase_Mesh",
				new HumanBodyBones?(20)
			},
			{
				"LeftShoulder_Mesh",
				new HumanBodyBones?(11)
			},
			{
				"LeftArm_Mesh",
				new HumanBodyBones?(13)
			},
			{
				"LeftForeArm_Mesh",
				new HumanBodyBones?(15)
			},
			{
				"LeftHand_Mesh",
				new HumanBodyBones?(17)
			},
			{
				"RightShoulder_Mesh",
				new HumanBodyBones?(12)
			},
			{
				"RightArm_Mesh",
				new HumanBodyBones?(14)
			},
			{
				"RightForeArm_Mesh",
				new HumanBodyBones?(16)
			},
			{
				"RightHand_Mesh",
				new HumanBodyBones?(18)
			},
			{
				"LeftFingerIndexProximal_Mesh",
				new HumanBodyBones?(27)
			},
			{
				"LeftFingerIndexIntermediate_Mesh",
				new HumanBodyBones?(28)
			},
			{
				"LeftFingerIndexDistal_Mesh",
				new HumanBodyBones?(29)
			},
			{
				"LeftFingerMiddleProximal_Mesh",
				new HumanBodyBones?(30)
			},
			{
				"LeftFingerMIddleIntermediate_Mesh",
				new HumanBodyBones?(31)
			},
			{
				"LeftFingerMiddleDistal_Mesh",
				new HumanBodyBones?(32)
			},
			{
				"LeftFingerRingProximal_Mesh",
				new HumanBodyBones?(33)
			},
			{
				"LeftFingerRingIntermediate_Mesh",
				new HumanBodyBones?(34)
			},
			{
				"LeftFingerRingDistal_Mesh",
				new HumanBodyBones?(35)
			},
			{
				"LeftFingerLittleProximal_Mesh",
				new HumanBodyBones?(36)
			},
			{
				"LeftFingerLittleIntermediate_Mesh",
				new HumanBodyBones?(37)
			},
			{
				"LeftFingerLittleDistal_Mesh",
				new HumanBodyBones?(38)
			},
			{
				"LeftFingerThumbProximal_Mesh",
				new HumanBodyBones?(24)
			},
			{
				"LeftFingerThumbIntermediate_Mesh",
				new HumanBodyBones?(25)
			},
			{
				"LeftFingerThumbDistal_Mesh",
				new HumanBodyBones?(26)
			},
			{
				"RightFingerIndexProximal_Mesh",
				new HumanBodyBones?(42)
			},
			{
				"RightFingerIndexIntermediate_Mesh",
				new HumanBodyBones?(43)
			},
			{
				"RightFingerIndexDistal_Mesh",
				new HumanBodyBones?(44)
			},
			{
				"RightFingerMiddleProximal_Mesh",
				new HumanBodyBones?(45)
			},
			{
				"RightFingerMIddleIntermediate_Mesh",
				new HumanBodyBones?(46)
			},
			{
				"RightFingerMiddleDistal_Mesh",
				new HumanBodyBones?(47)
			},
			{
				"RightFingerRingProximal_Mesh",
				new HumanBodyBones?(48)
			},
			{
				"RightFingerRingIntermediate_Mesh",
				new HumanBodyBones?(49)
			},
			{
				"RightFingerRingDistal_Mesh",
				new HumanBodyBones?(50)
			},
			{
				"RightFingerLittleProximal_Mesh",
				new HumanBodyBones?(51)
			},
			{
				"RightFingerLittleIntermediate_Mesh",
				new HumanBodyBones?(52)
			},
			{
				"RightFingerLittleDistal_Mesh",
				new HumanBodyBones?(53)
			},
			{
				"RightFingerThumbProximal_Mesh",
				new HumanBodyBones?(39)
			},
			{
				"RightFingerThumbIntermediate_Mesh",
				new HumanBodyBones?(40)
			},
			{
				"RightFingerThumbDistal_Mesh",
				new HumanBodyBones?(41)
			}
		};
	}
}
