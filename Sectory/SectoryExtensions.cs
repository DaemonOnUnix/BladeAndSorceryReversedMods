using System;
using ThunderRoad;

namespace Sectory
{
	// Token: 0x02000012 RID: 18
	public static class SectoryExtensions
	{
		// Token: 0x06000021 RID: 33 RVA: 0x0000280E File Offset: 0x00000A0E
		public static Systems Systems(this Creature creature)
		{
			return creature.GetComponent<Systems>();
		}
	}
}
