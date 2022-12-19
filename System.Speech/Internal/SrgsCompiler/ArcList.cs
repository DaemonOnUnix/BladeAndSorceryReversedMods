using System;
using System.Collections.Generic;

namespace System.Speech.Internal.SrgsCompiler
{
	// Token: 0x02000099 RID: 153
	internal class ArcList : RedBackList
	{
		// Token: 0x06000312 RID: 786 RVA: 0x0000AB88 File Offset: 0x00009B88
		internal List<Arc> ToList()
		{
			List<Arc> list = new List<Arc>();
			foreach (object obj in this)
			{
				Arc arc = (Arc)obj;
				list.Add(arc);
			}
			return list;
		}

		// Token: 0x06000313 RID: 787 RVA: 0x0000ABE4 File Offset: 0x00009BE4
		protected override int CompareTo(object arc1, object arc2)
		{
			return Arc.CompareContentForKey((Arc)arc1, (Arc)arc2);
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000314 RID: 788 RVA: 0x0000ABF7 File Offset: 0x00009BF7
		internal new Arc First
		{
			get
			{
				return (Arc)base.First;
			}
		}
	}
}
