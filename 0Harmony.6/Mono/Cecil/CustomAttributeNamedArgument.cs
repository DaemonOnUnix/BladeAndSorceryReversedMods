using System;

namespace Mono.Cecil
{
	// Token: 0x02000103 RID: 259
	public struct CustomAttributeNamedArgument
	{
		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060006F7 RID: 1783 RVA: 0x000200FF File Offset: 0x0001E2FF
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060006F8 RID: 1784 RVA: 0x00020107 File Offset: 0x0001E307
		public CustomAttributeArgument Argument
		{
			get
			{
				return this.argument;
			}
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x0002010F File Offset: 0x0001E30F
		public CustomAttributeNamedArgument(string name, CustomAttributeArgument argument)
		{
			Mixin.CheckName(name);
			this.name = name;
			this.argument = argument;
		}

		// Token: 0x040002A7 RID: 679
		private readonly string name;

		// Token: 0x040002A8 RID: 680
		private readonly CustomAttributeArgument argument;
	}
}
