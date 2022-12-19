using System;

namespace BloodMagic.Quest.Conditions
{
	// Token: 0x0200000A RID: 10
	public class Condition
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000040 RID: 64 RVA: 0x000025A0 File Offset: 0x000007A0
		public virtual int conditionCost
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06000041 RID: 65 RVA: 0x000028CC File Offset: 0x00000ACC
		public virtual Condition SetupCondition(int p_seed, int p_level)
		{
			this.random = new Random(p_seed);
			return null;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000025A0 File Offset: 0x000007A0
		public virtual bool CanBeUsedWithType(Type mainType)
		{
			return true;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x000028EB File Offset: 0x00000AEB
		public virtual bool IsConditionMet(Type mainType, object[] parseProperties = null)
		{
			return false;
		}

		// Token: 0x0400001F RID: 31
		public string conditionText = "";

		// Token: 0x04000020 RID: 32
		protected Random random;
	}
}
