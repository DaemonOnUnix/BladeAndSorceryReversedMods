using System;

namespace Mono.Cecil
{
	// Token: 0x020001F5 RID: 501
	public struct CustomAttributeNamedArgument
	{
		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000A2F RID: 2607 RVA: 0x00025FA7 File Offset: 0x000241A7
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06000A30 RID: 2608 RVA: 0x00025FAF File Offset: 0x000241AF
		public CustomAttributeArgument Argument
		{
			get
			{
				return this.argument;
			}
		}

		// Token: 0x06000A31 RID: 2609 RVA: 0x00025FB7 File Offset: 0x000241B7
		public CustomAttributeNamedArgument(string name, CustomAttributeArgument argument)
		{
			Mixin.CheckName(name);
			this.name = name;
			this.argument = argument;
		}

		// Token: 0x040002D9 RID: 729
		private readonly string name;

		// Token: 0x040002DA RID: 730
		private readonly CustomAttributeArgument argument;
	}
}
