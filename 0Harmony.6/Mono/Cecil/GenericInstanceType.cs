using System;
using System.Text;
using System.Threading;
using Mono.Cecil.Metadata;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000115 RID: 277
	internal sealed class GenericInstanceType : TypeSpecification, IGenericInstance, IMetadataTokenProvider, IGenericContext
	{
		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060007EB RID: 2027 RVA: 0x0002170D File Offset: 0x0001F90D
		public bool HasGenericArguments
		{
			get
			{
				return !this.arguments.IsNullOrEmpty<TypeReference>();
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060007EC RID: 2028 RVA: 0x0002171D File Offset: 0x0001F91D
		public Collection<TypeReference> GenericArguments
		{
			get
			{
				if (this.arguments == null)
				{
					Interlocked.CompareExchange<Collection<TypeReference>>(ref this.arguments, new Collection<TypeReference>(), null);
				}
				return this.arguments;
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060007ED RID: 2029 RVA: 0x0002173F File Offset: 0x0001F93F
		// (set) Token: 0x060007EE RID: 2030 RVA: 0x000039F6 File Offset: 0x00001BF6
		public override TypeReference DeclaringType
		{
			get
			{
				return base.ElementType.DeclaringType;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060007EF RID: 2031 RVA: 0x0002174C File Offset: 0x0001F94C
		public override string FullName
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(base.FullName);
				this.GenericInstanceFullName(stringBuilder);
				return stringBuilder.ToString();
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060007F0 RID: 2032 RVA: 0x00012561 File Offset: 0x00010761
		public override bool IsGenericInstance
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060007F1 RID: 2033 RVA: 0x00021779 File Offset: 0x0001F979
		public override bool ContainsGenericParameter
		{
			get
			{
				return this.ContainsGenericParameter() || base.ContainsGenericParameter;
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060007F2 RID: 2034 RVA: 0x0002178B File Offset: 0x0001F98B
		IGenericParameterProvider IGenericContext.Type
		{
			get
			{
				return base.ElementType;
			}
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x00021793 File Offset: 0x0001F993
		public GenericInstanceType(TypeReference type)
			: base(type)
		{
			base.IsValueType = type.IsValueType;
			this.etype = Mono.Cecil.Metadata.ElementType.GenericInst;
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x000217B0 File Offset: 0x0001F9B0
		internal GenericInstanceType(TypeReference type, int arity)
			: this(type)
		{
			this.arguments = new Collection<TypeReference>(arity);
		}

		// Token: 0x040002F0 RID: 752
		private Collection<TypeReference> arguments;
	}
}
