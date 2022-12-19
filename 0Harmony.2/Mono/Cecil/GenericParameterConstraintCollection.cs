using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200020C RID: 524
	internal class GenericParameterConstraintCollection : Collection<GenericParameterConstraint>
	{
		// Token: 0x06000B69 RID: 2921 RVA: 0x00027C41 File Offset: 0x00025E41
		internal GenericParameterConstraintCollection(GenericParameter genericParameter)
		{
			this.generic_parameter = genericParameter;
		}

		// Token: 0x06000B6A RID: 2922 RVA: 0x00027C50 File Offset: 0x00025E50
		internal GenericParameterConstraintCollection(GenericParameter genericParameter, int length)
			: base(length)
		{
			this.generic_parameter = genericParameter;
		}

		// Token: 0x06000B6B RID: 2923 RVA: 0x00027C60 File Offset: 0x00025E60
		protected override void OnAdd(GenericParameterConstraint item, int index)
		{
			item.generic_parameter = this.generic_parameter;
		}

		// Token: 0x06000B6C RID: 2924 RVA: 0x00027C60 File Offset: 0x00025E60
		protected override void OnInsert(GenericParameterConstraint item, int index)
		{
			item.generic_parameter = this.generic_parameter;
		}

		// Token: 0x06000B6D RID: 2925 RVA: 0x00027C60 File Offset: 0x00025E60
		protected override void OnSet(GenericParameterConstraint item, int index)
		{
			item.generic_parameter = this.generic_parameter;
		}

		// Token: 0x06000B6E RID: 2926 RVA: 0x00027C6E File Offset: 0x00025E6E
		protected override void OnRemove(GenericParameterConstraint item, int index)
		{
			item.generic_parameter = null;
		}

		// Token: 0x04000331 RID: 817
		private readonly GenericParameter generic_parameter;
	}
}
