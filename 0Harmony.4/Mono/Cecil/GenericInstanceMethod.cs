using System;
using System.Text;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000206 RID: 518
	internal sealed class GenericInstanceMethod : MethodSpecification, IGenericInstance, IMetadataTokenProvider, IGenericContext
	{
		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06000B1C RID: 2844 RVA: 0x0002750C File Offset: 0x0002570C
		public bool HasGenericArguments
		{
			get
			{
				return !this.arguments.IsNullOrEmpty<TypeReference>();
			}
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06000B1D RID: 2845 RVA: 0x0002751C File Offset: 0x0002571C
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

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06000B1E RID: 2846 RVA: 0x000183ED File Offset: 0x000165ED
		public override bool IsGenericInstance
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000B1F RID: 2847 RVA: 0x0002753E File Offset: 0x0002573E
		IGenericParameterProvider IGenericContext.Method
		{
			get
			{
				return base.ElementMethod;
			}
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06000B20 RID: 2848 RVA: 0x00027546 File Offset: 0x00025746
		IGenericParameterProvider IGenericContext.Type
		{
			get
			{
				return base.ElementMethod.DeclaringType;
			}
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06000B21 RID: 2849 RVA: 0x00027553 File Offset: 0x00025753
		public override bool ContainsGenericParameter
		{
			get
			{
				return this.ContainsGenericParameter() || base.ContainsGenericParameter;
			}
		}

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06000B22 RID: 2850 RVA: 0x00027568 File Offset: 0x00025768
		public override string FullName
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				MethodReference elementMethod = base.ElementMethod;
				stringBuilder.Append(elementMethod.ReturnType.FullName).Append(" ").Append(elementMethod.DeclaringType.FullName)
					.Append("::")
					.Append(elementMethod.Name);
				this.GenericInstanceFullName(stringBuilder);
				this.MethodSignatureFullName(stringBuilder);
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06000B23 RID: 2851 RVA: 0x000275D7 File Offset: 0x000257D7
		public GenericInstanceMethod(MethodReference method)
			: base(method)
		{
		}

		// Token: 0x06000B24 RID: 2852 RVA: 0x000275E0 File Offset: 0x000257E0
		internal GenericInstanceMethod(MethodReference method, int arity)
			: this(method)
		{
			this.arguments = new Collection<TypeReference>(arity);
		}

		// Token: 0x04000321 RID: 801
		private Collection<TypeReference> arguments;
	}
}
