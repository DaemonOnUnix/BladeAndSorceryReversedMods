using System;
using System.Collections.Generic;

namespace MonoMod.Utils
{
	// Token: 0x02000459 RID: 1113
	internal sealed class WeakReferenceComparer : EqualityComparer<WeakReference>
	{
		// Token: 0x060017B7 RID: 6071 RVA: 0x00052FCD File Offset: 0x000511CD
		public override bool Equals(WeakReference x, WeakReference y)
		{
			return x.SafeGetTarget() == y.SafeGetTarget() && x.SafeGetIsAlive() == y.SafeGetIsAlive();
		}

		// Token: 0x060017B8 RID: 6072 RVA: 0x00052FED File Offset: 0x000511ED
		public override int GetHashCode(WeakReference obj)
		{
			object obj2 = obj.SafeGetTarget();
			if (obj2 == null)
			{
				return 0;
			}
			return obj2.GetHashCode();
		}
	}
}
