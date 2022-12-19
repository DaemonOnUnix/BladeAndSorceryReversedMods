using System;
using UnityEngine;

namespace CustomAvatarFramework.Editor.Items
{
	// Token: 0x02000023 RID: 35
	public class CustomAvatarOptionalPart : MonoBehaviour
	{
		// Token: 0x040000BF RID: 191
		[Tooltip("the visibility of this part is based on the parent part")]
		public Transform parentPart;

		// Token: 0x040000C0 RID: 192
		[Range(0f, 100f)]
		public float percentage;
	}
}
