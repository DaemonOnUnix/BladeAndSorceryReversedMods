using System;
using System.Text;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000238 RID: 568
	public class MethodReference : MemberReference, IMethodSignature, IMetadataTokenProvider, IGenericParameterProvider, IGenericContext
	{
		// Token: 0x1700034B RID: 843
		// (get) Token: 0x06000CEF RID: 3311 RVA: 0x0002B038 File Offset: 0x00029238
		// (set) Token: 0x06000CF0 RID: 3312 RVA: 0x0002B040 File Offset: 0x00029240
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

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x06000CF1 RID: 3313 RVA: 0x0002B049 File Offset: 0x00029249
		// (set) Token: 0x06000CF2 RID: 3314 RVA: 0x0002B051 File Offset: 0x00029251
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

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x06000CF3 RID: 3315 RVA: 0x0002B05A File Offset: 0x0002925A
		// (set) Token: 0x06000CF4 RID: 3316 RVA: 0x0002B062 File Offset: 0x00029262
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

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06000CF5 RID: 3317 RVA: 0x0002B06B File Offset: 0x0002926B
		public virtual bool HasParameters
		{
			get
			{
				return !this.parameters.IsNullOrEmpty<ParameterDefinition>();
			}
		}

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x06000CF6 RID: 3318 RVA: 0x0002B07B File Offset: 0x0002927B
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

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x06000CF7 RID: 3319 RVA: 0x0002B0A0 File Offset: 0x000292A0
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

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x06000CF8 RID: 3320 RVA: 0x00017E2C File Offset: 0x0001602C
		IGenericParameterProvider IGenericContext.Method
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x06000CF9 RID: 3321 RVA: 0x000183ED File Offset: 0x000165ED
		GenericParameterType IGenericParameterProvider.GenericParameterType
		{
			get
			{
				return GenericParameterType.Method;
			}
		}

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06000CFA RID: 3322 RVA: 0x0002B0C6 File Offset: 0x000292C6
		public virtual bool HasGenericParameters
		{
			get
			{
				return !this.generic_parameters.IsNullOrEmpty<GenericParameter>();
			}
		}

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06000CFB RID: 3323 RVA: 0x0002B0D6 File Offset: 0x000292D6
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

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x06000CFC RID: 3324 RVA: 0x0002B0FC File Offset: 0x000292FC
		// (set) Token: 0x06000CFD RID: 3325 RVA: 0x0002B11C File Offset: 0x0002931C
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

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x06000CFE RID: 3326 RVA: 0x0002B13A File Offset: 0x0002933A
		// (set) Token: 0x06000CFF RID: 3327 RVA: 0x0002B142 File Offset: 0x00029342
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

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06000D00 RID: 3328 RVA: 0x0002B14C File Offset: 0x0002934C
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

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06000D01 RID: 3329 RVA: 0x00017DC4 File Offset: 0x00015FC4
		public virtual bool IsGenericInstance
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x06000D02 RID: 3330 RVA: 0x0002B194 File Offset: 0x00029394
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

		// Token: 0x06000D03 RID: 3331 RVA: 0x0002B1F0 File Offset: 0x000293F0
		internal MethodReference()
		{
			this.return_type = new MethodReturnType(this);
			this.token = new MetadataToken(TokenType.MemberRef);
		}

		// Token: 0x06000D04 RID: 3332 RVA: 0x0002B214 File Offset: 0x00029414
		public MethodReference(string name, TypeReference returnType)
			: base(name)
		{
			Mixin.CheckType(returnType, Mixin.Argument.returnType);
			this.return_type = new MethodReturnType(this);
			this.return_type.ReturnType = returnType;
			this.token = new MetadataToken(TokenType.MemberRef);
		}

		// Token: 0x06000D05 RID: 3333 RVA: 0x0002B24D File Offset: 0x0002944D
		public MethodReference(string name, TypeReference returnType, TypeReference declaringType)
			: this(name, returnType)
		{
			Mixin.CheckType(declaringType, Mixin.Argument.declaringType);
			this.DeclaringType = declaringType;
		}

		// Token: 0x06000D06 RID: 3334 RVA: 0x00017E2C File Offset: 0x0001602C
		public virtual MethodReference GetElementMethod()
		{
			return this;
		}

		// Token: 0x06000D07 RID: 3335 RVA: 0x0002B266 File Offset: 0x00029466
		protected override IMemberDefinition ResolveDefinition()
		{
			return this.Resolve();
		}

		// Token: 0x06000D08 RID: 3336 RVA: 0x0002B26E File Offset: 0x0002946E
		public new virtual MethodDefinition Resolve()
		{
			ModuleDefinition module = this.Module;
			if (module == null)
			{
				throw new NotSupportedException();
			}
			return module.Resolve(this);
		}

		// Token: 0x040003C5 RID: 965
		internal ParameterDefinitionCollection parameters;

		// Token: 0x040003C6 RID: 966
		private MethodReturnType return_type;

		// Token: 0x040003C7 RID: 967
		private bool has_this;

		// Token: 0x040003C8 RID: 968
		private bool explicit_this;

		// Token: 0x040003C9 RID: 969
		private MethodCallingConvention calling_convention;

		// Token: 0x040003CA RID: 970
		internal Collection<GenericParameter> generic_parameters;
	}
}
