using System;

namespace Mono.Cecil
{
	// Token: 0x020001F4 RID: 500
	public struct CustomAttributeArgument
	{
		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000A2C RID: 2604 RVA: 0x00025F81 File Offset: 0x00024181
		public TypeReference Type
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000A2D RID: 2605 RVA: 0x00025F89 File Offset: 0x00024189
		public object Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x06000A2E RID: 2606 RVA: 0x00025F91 File Offset: 0x00024191
		public CustomAttributeArgument(TypeReference type, object value)
		{
			Mixin.CheckType(type);
			this.type = type;
			this.value = value;
		}

		// Token: 0x040002D7 RID: 727
		private readonly TypeReference type;

		// Token: 0x040002D8 RID: 728
		private readonly object value;
	}
}
