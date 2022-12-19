using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000165 RID: 357
	public abstract class PropertyReference : MemberReference
	{
		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000B5A RID: 2906 RVA: 0x00027479 File Offset: 0x00025679
		// (set) Token: 0x06000B5B RID: 2907 RVA: 0x00027481 File Offset: 0x00025681
		public TypeReference PropertyType
		{
			get
			{
				return this.property_type;
			}
			set
			{
				this.property_type = value;
			}
		}

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000B5C RID: 2908
		public abstract Collection<ParameterDefinition> Parameters { get; }

		// Token: 0x06000B5D RID: 2909 RVA: 0x0002748A File Offset: 0x0002568A
		internal PropertyReference(string name, TypeReference propertyType)
			: base(name)
		{
			Mixin.CheckType(propertyType, Mixin.Argument.propertyType);
			this.property_type = propertyType;
		}

		// Token: 0x06000B5E RID: 2910 RVA: 0x000274A2 File Offset: 0x000256A2
		protected override IMemberDefinition ResolveDefinition()
		{
			return this.Resolve();
		}

		// Token: 0x06000B5F RID: 2911
		public new abstract PropertyDefinition Resolve();

		// Token: 0x0400047E RID: 1150
		private TypeReference property_type;
	}
}
