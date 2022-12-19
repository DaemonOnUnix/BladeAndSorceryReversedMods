using System;
using ThunderRoad;
using UnityEngine;

namespace SoulFireSpell
{
	// Token: 0x02000008 RID: 8
	public class CompMono : MonoBehaviour
	{
		// Token: 0x06000011 RID: 17 RVA: 0x00002B2C File Offset: 0x00000D2C
		public static void DestroyComp(Creature creature)
		{
			Object.Destroy(creature.gameObject.GetComponent<ZapMono>());
		}
	}
}
