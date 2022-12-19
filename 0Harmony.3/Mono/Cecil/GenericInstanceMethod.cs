using System;
using System.Text;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000114 RID: 276
	internal sealed class GenericInstanceMethod : MethodSpecification, IGenericInstance, IMetadataTokenProvider, IGenericContext
	{
		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060007E2 RID: 2018 RVA: 0x00021624 File Offset: 0x0001F824
		public bool HasGenericArguments
		{
			get
			{
				return !this.arguments.IsNullOrEmpty<TypeReference>();
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060007E3 RID: 2019 RVA: 0x00021634 File Offset: 0x0001F834
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

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060007E4 RID: 2020 RVA: 0x00012561 File Offset: 0x00010761
		public override bool IsGenericInstance
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060007E5 RID: 2021 RVA: 0x00021656 File Offset: 0x0001F856
		IGenericParameterProvider IGenericContext.Method
		{
			get
			{
				return base.ElementMethod;
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060007E6 RID: 2022 RVA: 0x0002165E File Offset: 0x0001F85E
		IGenericParameterProvider IGenericContext.Type
		{
			get
			{
				return base.ElementMethod.DeclaringType;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060007E7 RID: 2023 RVA: 0x0002166B File Offset: 0x0001F86B
		public override bool ContainsGenericParameter
		{
			get
			{
				return this.ContainsGenericParameter() || base.ContainsGenericParameter;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060007E8 RID: 2024 RVA: 0x00021680 File Offset: 0x0001F880
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

		// Token: 0x060007E9 RID: 2025 RVA: 0x000216EF File Offset: 0x0001F8EF
		public GenericInstanceMethod(MethodReference method)
			: base(method)
		{
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x000216F8 File Offset: 0x0001F8F8
		internal GenericInstanceMethod(MethodReference method, int arity)
			: this(method)
		{
			this.arguments = new Collection<TypeReference>(arity);
		}

		// Token: 0x040002EF RID: 751
		private Collection<TypeReference> arguments;
	}
}
