using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200011A RID: 282
	internal class GenericParameterConstraintCollection : Collection<GenericParameterConstraint>
	{
		// Token: 0x0600082F RID: 2095 RVA: 0x00021D59 File Offset: 0x0001FF59
		internal GenericParameterConstraintCollection(GenericParameter genericParameter)
		{
			this.generic_parameter = genericParameter;
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x00021D68 File Offset: 0x0001FF68
		internal GenericParameterConstraintCollection(GenericParameter genericParameter, int length)
			: base(length)
		{
			this.generic_parameter = genericParameter;
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x00021D78 File Offset: 0x0001FF78
		protected override void OnAdd(GenericParameterConstraint item, int index)
		{
			item.generic_parameter = this.generic_parameter;
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x00021D78 File Offset: 0x0001FF78
		protected override void OnInsert(GenericParameterConstraint item, int index)
		{
			item.generic_parameter = this.generic_parameter;
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x00021D78 File Offset: 0x0001FF78
		protected override void OnSet(GenericParameterConstraint item, int index)
		{
			item.generic_parameter = this.generic_parameter;
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x00021D86 File Offset: 0x0001FF86
		protected override void OnRemove(GenericParameterConstraint item, int index)
		{
			item.generic_parameter = null;
		}

		// Token: 0x040002FF RID: 767
		private readonly GenericParameter generic_parameter;
	}
}
