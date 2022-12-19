using System;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

namespace CustomAvatarFramework
{
	// Token: 0x02000027 RID: 39
	public class ItemModuleAvatar : ItemModule
	{
		// Token: 0x040000C3 RID: 195
		public string avatarItemId;

		// Token: 0x040000C4 RID: 196
		public float avatarHeight;

		// Token: 0x040000C5 RID: 197
		public float playerScaleRatio;

		// Token: 0x040000C6 RID: 198
		public float npcScaleRatio;

		// Token: 0x040000C7 RID: 199
		public List<string> positionMatchingExemptionBoneNames;

		// Token: 0x040000C8 RID: 200
		public Dictionary<string, Vector3> bones = new Dictionary<string, Vector3>();

		// Token: 0x040000C9 RID: 201
		public Dictionary<string, Vector3> extraPositionBones = new Dictionary<string, Vector3>();

		// Token: 0x040000CA RID: 202
		public int version = 1;

		// Token: 0x040000CB RID: 203
		public Vector3 extraHipPosition;

		// Token: 0x040000CC RID: 204
		public bool enforceHandPosition;

		// Token: 0x040000CD RID: 205
		public bool disableDynamicBones;

		// Token: 0x040000CE RID: 206
		public Vector3 extraDimension = Vector3.one;

		// Token: 0x040000CF RID: 207
		public bool hasWardrobe;

		// Token: 0x040000D0 RID: 208
		public List<WardrobeApparel> wardrobeApparels;

		// Token: 0x040000D1 RID: 209
		public bool isGodMode;
	}
}
