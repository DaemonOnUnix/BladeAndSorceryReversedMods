using System;
using System.Text;
using System.Threading;
using Mono.Cecil.Metadata;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000207 RID: 519
	internal sealed class GenericInstanceType : TypeSpecification, IGenericInstance, IMetadataTokenProvider, IGenericContext
	{
		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06000B25 RID: 2853 RVA: 0x000275F5 File Offset: 0x000257F5
		public bool HasGenericArguments
		{
			get
			{
				return !this.arguments.IsNullOrEmpty<TypeReference>();
			}
		}

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06000B26 RID: 2854 RVA: 0x00027605 File Offset: 0x00025805
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

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06000B27 RID: 2855 RVA: 0x00027627 File Offset: 0x00025827
		// (set) Token: 0x06000B28 RID: 2856 RVA: 0x00003A32 File Offset: 0x00001C32
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

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06000B29 RID: 2857 RVA: 0x00027634 File Offset: 0x00025834
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

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06000B2A RID: 2858 RVA: 0x000183ED File Offset: 0x000165ED
		public override bool IsGenericInstance
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06000B2B RID: 2859 RVA: 0x00027661 File Offset: 0x00025861
		public override bool ContainsGenericParameter
		{
			get
			{
				return this.ContainsGenericParameter() || base.ContainsGenericParameter;
			}
		}

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x06000B2C RID: 2860 RVA: 0x00027673 File Offset: 0x00025873
		IGenericParameterProvider IGenericContext.Type
		{
			get
			{
				return base.ElementType;
			}
		}

		// Token: 0x06000B2D RID: 2861 RVA: 0x0002767B File Offset: 0x0002587B
		public GenericInstanceType(TypeReference type)
			: base(type)
		{
			base.IsValueType = type.IsValueType;
			this.etype = Mono.Cecil.Metadata.ElementType.GenericInst;
		}

		// Token: 0x06000B2E RID: 2862 RVA: 0x00027698 File Offset: 0x00025898
		internal GenericInstanceType(TypeReference type, int arity)
			: this(type)
		{
			this.arguments = new Collection<TypeReference>(arity);
		}

		// Token: 0x04000322 RID: 802
		private Collection<TypeReference> arguments;
	}
}
