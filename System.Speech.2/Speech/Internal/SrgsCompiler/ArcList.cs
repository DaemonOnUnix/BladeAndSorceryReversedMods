using System;
using System.Collections.Generic;

namespace System.Speech.Internal.SrgsCompiler
{
	// Token: 0x020000E7 RID: 231
	internal class ArcList : RedBackList
	{
		// Token: 0x060007EB RID: 2027 RVA: 0x00022D68 File Offset: 0x00020F68
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

		// Token: 0x060007EC RID: 2028 RVA: 0x00022DC4 File Offset: 0x00020FC4
		protected override int CompareTo(object arc1, object arc2)
		{
			return Arc.CompareContentForKey((Arc)arc1, (Arc)arc2);
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x060007ED RID: 2029 RVA: 0x00022DD7 File Offset: 0x00020FD7
		internal new Arc First
		{
			get
			{
				return (Arc)base.First;
			}
		}
	}
}
