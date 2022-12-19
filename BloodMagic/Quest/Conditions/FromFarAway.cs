using System;
using ThunderRoad;
using UnityEngine;

namespace BloodMagic.Quest.Conditions
{
	// Token: 0x02000009 RID: 9
	public class FromFarAway : Condition
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600003B RID: 59 RVA: 0x000026FD File Offset: 0x000008FD
		public override int conditionCost
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002810 File Offset: 0x00000A10
		public override Condition SetupCondition(int p_seed, int p_level)
		{
			base.SetupCondition(p_seed, p_level);
			this.distance = (float)this.random.Next(2, 3 * ((p_level + 2) / 2));
			this.conditionText = string.Format("from {0} meters away", this.distance);
			return this;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002861 File Offset: 0x00000A61
		public override bool CanBeUsedWithType(Type mainType)
		{
			return !(mainType != typeof(Kill));
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002878 File Offset: 0x00000A78
		public override bool IsConditionMet(Type mainType, object[] parseProperties = null)
		{
			return mainType == typeof(Kill) && (double)Vector3.Distance((parseProperties[0] as Creature).transform.position, Player.currentCreature.transform.position) > (double)this.distance;
		}

		// Token: 0x0400001E RID: 30
		private float distance;
	}
}
