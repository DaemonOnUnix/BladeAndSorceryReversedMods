using System;

namespace Mono.Cecil
{
	// Token: 0x02000102 RID: 258
	public struct CustomAttributeArgument
	{
		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060006F4 RID: 1780 RVA: 0x000200D9 File Offset: 0x0001E2D9
		public TypeReference Type
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060006F5 RID: 1781 RVA: 0x000200E1 File Offset: 0x0001E2E1
		public object Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x000200E9 File Offset: 0x0001E2E9
		public CustomAttributeArgument(TypeReference type, object value)
		{
			Mixin.CheckType(type);
			this.type = type;
			this.value = value;
		}

		// Token: 0x040002A5 RID: 677
		private readonly TypeReference type;

		// Token: 0x040002A6 RID: 678
		private readonly object value;
	}
}
