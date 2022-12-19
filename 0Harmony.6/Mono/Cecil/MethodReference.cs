using System;
using System.Text;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000145 RID: 325
	public class MethodReference : MemberReference, IMethodSignature, IMetadataTokenProvider, IGenericParameterProvider, IGenericContext
	{
		// Token: 0x1700017B RID: 379
		// (get) Token: 0x060009AC RID: 2476 RVA: 0x00024D60 File Offset: 0x00022F60
		// (set) Token: 0x060009AD RID: 2477 RVA: 0x00024D68 File Offset: 0x00022F68
		public virtual bool HasThis
		{
			get
			{
				return this.has_this;
			}
			set
			{
				this.has_this = value;
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x060009AE RID: 2478 RVA: 0x00024D71 File Offset: 0x00022F71
		// (set) Token: 0x060009AF RID: 2479 RVA: 0x00024D79 File Offset: 0x00022F79
		public virtual bool ExplicitThis
		{
			get
			{
				return this.explicit_this;
			}
			set
			{
				this.explicit_this = value;
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x060009B0 RID: 2480 RVA: 0x00024D82 File Offset: 0x00022F82
		// (set) Token: 0x060009B1 RID: 2481 RVA: 0x00024D8A File Offset: 0x00022F8A
		public virtual MethodCallingConvention CallingConvention
		{
			get
			{
				return this.calling_convention;
			}
			set
			{
				this.calling_convention = value;
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x060009B2 RID: 2482 RVA: 0x00024D93 File Offset: 0x00022F93
		public virtual bool HasParameters
		{
			get
			{
				return !this.parameters.IsNullOrEmpty<ParameterDefinition>();
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x060009B3 RID: 2483 RVA: 0x00024DA3 File Offset: 0x00022FA3
		public virtual Collection<ParameterDefinition> Parameters
		{
			get
			{
				if (this.parameters == null)
				{
					Interlocked.CompareExchange<ParameterDefinitionCollection>(ref this.parameters, new ParameterDefinitionCollection(this), null);
				}
				return this.parameters;
			}
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x060009B4 RID: 2484 RVA: 0x00024DC8 File Offset: 0x00022FC8
		IGenericParameterProvider IGenericContext.Type
		{
			get
			{
				TypeReference declaringType = this.DeclaringType;
				GenericInstanceType genericInstanceType = declaringType as GenericInstanceType;
				if (genericInstanceType != null)
				{
					return genericInstanceType.ElementType;
				}
				return declaringType;
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x060009B5 RID: 2485 RVA: 0x00011FA0 File Offset: 0x000101A0
		IGenericParameterProvider IGenericContext.Method
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x060009B6 RID: 2486 RVA: 0x00012561 File Offset: 0x00010761
		GenericParameterType IGenericParameterProvider.GenericParameterType
		{
			get
			{
				return GenericParameterType.Method;
			}
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x060009B7 RID: 2487 RVA: 0x00024DEE File Offset: 0x00022FEE
		public virtual bool HasGenericParameters
		{
			get
			{
				return !this.generic_parameters.IsNullOrEmpty<GenericParameter>();
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x060009B8 RID: 2488 RVA: 0x00024DFE File Offset: 0x00022FFE
		public virtual Collection<GenericParameter> GenericParameters
		{
			get
			{
				if (this.generic_parameters == null)
				{
					Interlocked.CompareExchange<Collection<GenericParameter>>(ref this.generic_parameters, new GenericParameterCollection(this), null);
				}
				return this.generic_parameters;
			}
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x060009B9 RID: 2489 RVA: 0x00024E24 File Offset: 0x00023024
		// (set) Token: 0x060009BA RID: 2490 RVA: 0x00024E44 File Offset: 0x00023044
		public TypeReference ReturnType
		{
			get
			{
				MethodReturnType methodReturnType = this.MethodReturnType;
				if (methodReturnType == null)
				{
					return null;
				}
				return methodReturnType.ReturnType;
			}
			set
			{
				MethodReturnType methodReturnType = this.MethodReturnType;
				if (methodReturnType != null)
				{
					methodReturnType.ReturnType = value;
				}
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x060009BB RID: 2491 RVA: 0x00024E62 File Offset: 0x00023062
		// (set) Token: 0x060009BC RID: 2492 RVA: 0x00024E6A File Offset: 0x0002306A
		public virtual MethodReturnType MethodReturnType
		{
			get
			{
				return this.return_type;
			}
			set
			{
				this.return_type = value;
			}
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x060009BD RID: 2493 RVA: 0x00024E74 File Offset: 0x00023074
		public override string FullName
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(this.ReturnType.FullName).Append(" ").Append(base.MemberFullName());
				this.MethodSignatureFullName(stringBuilder);
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x060009BE RID: 2494 RVA: 0x00011F38 File Offset: 0x00010138
		public virtual bool IsGenericInstance
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x060009BF RID: 2495 RVA: 0x00024EBC File Offset: 0x000230BC
		public override bool ContainsGenericParameter
		{
			get
			{
				if (this.ReturnType.ContainsGenericParameter || base.ContainsGenericParameter)
				{
					return true;
				}
				if (!this.HasParameters)
				{
					return false;
				}
				Collection<ParameterDefinition> collection = this.Parameters;
				for (int i = 0; i < collection.Count; i++)
				{
					if (collection[i].ParameterType.ContainsGenericParameter)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x060009C0 RID: 2496 RVA: 0x00024F18 File Offset: 0x00023118
		internal MethodReference()
		{
			this.return_type = new MethodReturnType(this);
			this.token = new MetadataToken(TokenType.MemberRef);
		}

		// Token: 0x060009C1 RID: 2497 RVA: 0x00024F3C File Offset: 0x0002313C
		public MethodReference(string name, TypeReference returnType)
			: base(name)
		{
			Mixin.CheckType(returnType, Mixin.Argument.returnType);
			this.return_type = new MethodReturnType(this);
			this.return_type.ReturnType = returnType;
			this.token = new MetadataToken(TokenType.MemberRef);
		}

		// Token: 0x060009C2 RID: 2498 RVA: 0x00024F75 File Offset: 0x00023175
		public MethodReference(string name, TypeReference returnType, TypeReference declaringType)
			: this(name, returnType)
		{
			Mixin.CheckType(declaringType, Mixin.Argument.declaringType);
			this.DeclaringType = declaringType;
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x00011FA0 File Offset: 0x000101A0
		public virtual MethodReference GetElementMethod()
		{
			return this;
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x00024F8E File Offset: 0x0002318E
		protected override IMemberDefinition ResolveDefinition()
		{
			return this.Resolve();
		}

		// Token: 0x060009C5 RID: 2501 RVA: 0x00024F96 File Offset: 0x00023196
		public new virtual MethodDefinition Resolve()
		{
			ModuleDefinition module = this.Module;
			if (module == null)
			{
				throw new NotSupportedException();
			}
			return module.Resolve(this);
		}

		// Token: 0x04000393 RID: 915
		internal ParameterDefinitionCollection parameters;

		// Token: 0x04000394 RID: 916
		private MethodReturnType return_type;

		// Token: 0x04000395 RID: 917
		private bool has_this;

		// Token: 0x04000396 RID: 918
		private bool explicit_this;

		// Token: 0x04000397 RID: 919
		private MethodCallingConvention calling_convention;

		// Token: 0x04000398 RID: 920
		internal Collection<GenericParameter> generic_parameters;
	}
}
