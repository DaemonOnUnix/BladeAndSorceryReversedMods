using System;
using ThunderRoad;
using UnityEngine;

namespace BloodMagic.Quest.Conditions
{
	// Token: 0x02000007 RID: 7
	public class Headshot : Condition
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000031 RID: 49 RVA: 0x000026FD File Offset: 0x000008FD
		public override int conditionCost
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002700 File Offset: 0x00000900
		public override Condition SetupCondition(int p_seed, int p_level)
		{
			base.SetupCondition(p_seed, p_level);
			this.conditionText = "with a headshot";
			return this;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x0000266F File Offset: 0x0000086F
		public override bool CanBeUsedWithType(Type mainType)
		{
			return mainType == typeof(Kill);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002728 File Offset: 0x00000928
		public override bool IsConditionMet(Type mainType, object[] parseProperties = null)
		{
			Debug.Log(base.GetType().FullName + " :: Headshot check condition called");
			bool flag = (parseProperties[2] as CollisionInstance).damageStruct.hitRagdollPart.type == 1 || (parseProperties[2] as CollisionInstance).damageStruct.hitRagdollPart.type == 2;
			bool flag2;
			if (flag)
			{
				flag2 = true;
			}
			else
			{
				Debug.Log(base.GetType().FullName + " :: Returned false");
				flag2 = false;
			}
			return flag2;
		}
	}
}
