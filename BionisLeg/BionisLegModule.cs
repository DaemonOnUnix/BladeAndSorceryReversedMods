using System;
using System.Collections;
using ThunderRoad;
using UnityEngine;

namespace BionisLeg
{
	// Token: 0x02000002 RID: 2
	public class BionisLegModule : LevelModule
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public override IEnumerator OnLoadCoroutine()
		{
			foreach (Transform transform in this.level.customReferences.Find((Level.CustomReference match) => match.name == "Climbable").transforms)
			{
				transform.gameObject.AddComponent<ClimbableComponent>();
			}
			return base.OnLoadCoroutine();
		}
	}
}
