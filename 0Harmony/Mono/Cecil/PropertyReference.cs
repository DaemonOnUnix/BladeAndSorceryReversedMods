using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000259 RID: 601
	public abstract class PropertyReference : MemberReference
	{
		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x06000EA4 RID: 3748 RVA: 0x0002DAE1 File Offset: 0x0002BCE1
		// (set) Token: 0x06000EA5 RID: 3749 RVA: 0x0002DAE9 File Offset: 0x0002BCE9
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

		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x06000EA6 RID: 3750
		public abstract Collection<ParameterDefinition> Parameters { get; }

		// Token: 0x06000EA7 RID: 3751 RVA: 0x0002DAF2 File Offset: 0x0002BCF2
		internal PropertyReference(string name, TypeReference propertyType)
			: base(name)
		{
			Mixin.CheckType(propertyType, Mixin.Argument.propertyType);
			this.property_type = propertyType;
		}

		// Token: 0x06000EA8 RID: 3752 RVA: 0x0002DB0A File Offset: 0x0002BD0A
		protected override IMemberDefinition ResolveDefinition()
		{
			return this.Resolve();
		}

		// Token: 0x06000EA9 RID: 3753
		public new abstract PropertyDefinition Resolve();

		// Token: 0x040004B3 RID: 1203
		private TypeReference property_type;
	}
}
