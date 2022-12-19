using System;
using UnityEngine;

namespace UltimateFantasyPack
{
	// Token: 0x02000015 RID: 21
	public class Example : MonoBehaviour
	{
		// Token: 0x0600004E RID: 78 RVA: 0x000033FF File Offset: 0x000015FF
		public void Setup(int typesToUse)
		{
			this.typesToUse = typesToUse;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00003408 File Offset: 0x00001608
		public void SetType(Type type1, Type type2, Type type3)
		{
			this.type1 = type1;
			this.type2 = type2;
			this.type3 = type3;
		}

		// Token: 0x04000036 RID: 54
		private Type type1;

		// Token: 0x04000037 RID: 55
		private Type type2;

		// Token: 0x04000038 RID: 56
		private Type type3;

		// Token: 0x04000039 RID: 57
		private int typesToUse;
	}
}
