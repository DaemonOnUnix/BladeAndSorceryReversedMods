using System;
using System.Threading;
using Mono.Cecil.Metadata;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200017F RID: 383
	public class TypeReference : MemberReference, IGenericParameterProvider, IMetadataTokenProvider, IGenericContext
	{
		// Token: 0x17000290 RID: 656
		// (get) Token: 0x06000C31 RID: 3121 RVA: 0x00024209 File Offset: 0x00022409
		// (set) Token: 0x06000C32 RID: 3122 RVA: 0x0002917C File Offset: 0x0002737C
		public override string Name
		{
			get
			{
				return base.Name;
			}
			set
			{
				if (base.IsWindowsRuntimeProjection && value != base.Name)
				{
					throw new InvalidOperationException("Projected type reference name can't be changed.");
				}
				base.Name = value;
				this.ClearFullName();
			}
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x06000C33 RID: 3123 RVA: 0x000291AC File Offset: 0x000273AC
		// (set) Token: 0x06000C34 RID: 3124 RVA: 0x000291B4 File Offset: 0x000273B4
		public virtual string Namespace
		{
			get
			{
				return this.@namespace;
			}
			set
			{
				if (base.IsWindowsRuntimeProjection && value != this.@namespace)
				{
					throw new InvalidOperationException("Projected type reference namespace can't be changed.");
				}
				this.@namespace = value;
				this.ClearFullName();
			}
		}

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06000C35 RID: 3125 RVA: 0x000291E4 File Offset: 0x000273E4
		// (set) Token: 0x06000C36 RID: 3126 RVA: 0x000291EC File Offset: 0x000273EC
		public virtual bool IsValueType
		{
			get
			{
				return this.value_type;
			}
			set
			{
				this.value_type = value;
			}
		}

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x06000C37 RID: 3127 RVA: 0x000291F8 File Offset: 0x000273F8
		public override ModuleDefinition Module
		{
			get
			{
				if (this.module != null)
				{
					return this.module;
				}
				TypeReference declaringType = this.DeclaringType;
				if (declaringType != null)
				{
					return declaringType.Module;
				}
				return null;
			}
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x06000C38 RID: 3128 RVA: 0x00029226 File Offset: 0x00027426
		// (set) Token: 0x06000C39 RID: 3129 RVA: 0x00020F72 File Offset: 0x0001F172
		internal new TypeReferenceProjection WindowsRuntimeProjection
		{
			get
			{
				return (TypeReferenceProjection)this.projection;
			}
			set
			{
				this.projection = value;
			}
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x06000C3A RID: 3130 RVA: 0x00011FA0 File Offset: 0x000101A0
		IGenericParameterProvider IGenericContext.Type
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x06000C3B RID: 3131 RVA: 0x00021621 File Offset: 0x0001F821
		IGenericParameterProvider IGenericContext.Method
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x06000C3C RID: 3132 RVA: 0x00011F38 File Offset: 0x00010138
		GenericParameterType IGenericParameterProvider.GenericParameterType
		{
			get
			{
				return GenericParameterType.Type;
			}
		}

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x06000C3D RID: 3133 RVA: 0x00029233 File Offset: 0x00027433
		public virtual bool HasGenericParameters
		{
			get
			{
				return !this.generic_parameters.IsNullOrEmpty<GenericParameter>();
			}
		}

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x06000C3E RID: 3134 RVA: 0x00029243 File Offset: 0x00027443
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

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x06000C3F RID: 3135 RVA: 0x00029268 File Offset: 0x00027468
		// (set) Token: 0x06000C40 RID: 3136 RVA: 0x0002928C File Offset: 0x0002748C
		public virtual IMetadataScope Scope
		{
			get
			{
				TypeReference declaringType = this.DeclaringType;
				if (declaringType != null)
				{
					return declaringType.Scope;
				}
				return this.scope;
			}
			set
			{
				TypeReference declaringType = this.DeclaringType;
				if (declaringType != null)
				{
					if (base.IsWindowsRuntimeProjection && value != declaringType.Scope)
					{
						throw new InvalidOperationException("Projected type scope can't be changed.");
					}
					declaringType.Scope = value;
					return;
				}
				else
				{
					if (base.IsWindowsRuntimeProjection && value != this.scope)
					{
						throw new InvalidOperationException("Projected type scope can't be changed.");
					}
					this.scope = value;
					return;
				}
			}
		}

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x06000C41 RID: 3137 RVA: 0x000292EA File Offset: 0x000274EA
		public bool IsNested
		{
			get
			{
				return this.DeclaringType != null;
			}
		}

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x06000C42 RID: 3138 RVA: 0x000292F5 File Offset: 0x000274F5
		// (set) Token: 0x06000C43 RID: 3139 RVA: 0x000292FD File Offset: 0x000274FD
		public override TypeReference DeclaringType
		{
			get
			{
				return base.DeclaringType;
			}
			set
			{
				if (base.IsWindowsRuntimeProjection && value != base.DeclaringType)
				{
					throw new InvalidOperationException("Projected type declaring type can't be changed.");
				}
				base.DeclaringType = value;
				this.ClearFullName();
			}
		}

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x06000C44 RID: 3140 RVA: 0x00029328 File Offset: 0x00027528
		public override string FullName
		{
			get
			{
				if (this.fullname != null)
				{
					return this.fullname;
				}
				string text = this.TypeFullName();
				if (this.IsNested)
				{
					text = this.DeclaringType.FullName + "/" + text;
				}
				Interlocked.CompareExchange<string>(ref this.fullname, text, null);
				return this.fullname;
			}
		}

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x06000C45 RID: 3141 RVA: 0x00011F38 File Offset: 0x00010138
		public virtual bool IsByReference
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x06000C46 RID: 3142 RVA: 0x00011F38 File Offset: 0x00010138
		public virtual bool IsPointer
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x06000C47 RID: 3143 RVA: 0x00011F38 File Offset: 0x00010138
		public virtual bool IsSentinel
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x06000C48 RID: 3144 RVA: 0x00011F38 File Offset: 0x00010138
		public virtual bool IsArray
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x06000C49 RID: 3145 RVA: 0x00011F38 File Offset: 0x00010138
		public virtual bool IsGenericParameter
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x06000C4A RID: 3146 RVA: 0x00011F38 File Offset: 0x00010138
		public virtual bool IsGenericInstance
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x06000C4B RID: 3147 RVA: 0x00011F38 File Offset: 0x00010138
		public virtual bool IsRequiredModifier
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x06000C4C RID: 3148 RVA: 0x00011F38 File Offset: 0x00010138
		public virtual bool IsOptionalModifier
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06000C4D RID: 3149 RVA: 0x00011F38 File Offset: 0x00010138
		public virtual bool IsPinned
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06000C4E RID: 3150 RVA: 0x00011F38 File Offset: 0x00010138
		public virtual bool IsFunctionPointer
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06000C4F RID: 3151 RVA: 0x0002937E File Offset: 0x0002757E
		public virtual bool IsPrimitive
		{
			get
			{
				return this.etype.IsPrimitive();
			}
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06000C50 RID: 3152 RVA: 0x0002938B File Offset: 0x0002758B
		public virtual MetadataType MetadataType
		{
			get
			{
				if (this.etype != ElementType.None)
				{
					return (MetadataType)this.etype;
				}
				if (!this.IsValueType)
				{
					return MetadataType.Class;
				}
				return MetadataType.ValueType;
			}
		}

		// Token: 0x06000C51 RID: 3153 RVA: 0x000293A9 File Offset: 0x000275A9
		protected TypeReference(string @namespace, string name)
			: base(name)
		{
			this.@namespace = @namespace ?? string.Empty;
			this.token = new MetadataToken(TokenType.TypeRef, 0);
		}

		// Token: 0x06000C52 RID: 3154 RVA: 0x000293D3 File Offset: 0x000275D3
		public TypeReference(string @namespace, string name, ModuleDefinition module, IMetadataScope scope)
			: this(@namespace, name)
		{
			this.module = module;
			this.scope = scope;
		}

		// Token: 0x06000C53 RID: 3155 RVA: 0x000293EC File Offset: 0x000275EC
		public TypeReference(string @namespace, string name, ModuleDefinition module, IMetadataScope scope, bool valueType)
			: this(@namespace, name, module, scope)
		{
			this.value_type = valueType;
		}

		// Token: 0x06000C54 RID: 3156 RVA: 0x00029401 File Offset: 0x00027601
		protected virtual void ClearFullName()
		{
			this.fullname = null;
		}

		// Token: 0x06000C55 RID: 3157 RVA: 0x00011FA0 File Offset: 0x000101A0
		public virtual TypeReference GetElementType()
		{
			return this;
		}

		// Token: 0x06000C56 RID: 3158 RVA: 0x0002940A File Offset: 0x0002760A
		protected override IMemberDefinition ResolveDefinition()
		{
			return this.Resolve();
		}

		// Token: 0x06000C57 RID: 3159 RVA: 0x00029412 File Offset: 0x00027612
		public new virtual TypeDefinition Resolve()
		{
			ModuleDefinition moduleDefinition = this.Module;
			if (moduleDefinition == null)
			{
				throw new NotSupportedException();
			}
			return moduleDefinition.Resolve(this);
		}

		// Token: 0x04000536 RID: 1334
		private string @namespace;

		// Token: 0x04000537 RID: 1335
		private bool value_type;

		// Token: 0x04000538 RID: 1336
		internal IMetadataScope scope;

		// Token: 0x04000539 RID: 1337
		internal ModuleDefinition module;

		// Token: 0x0400053A RID: 1338
		internal ElementType etype;

		// Token: 0x0400053B RID: 1339
		private string fullname;

		// Token: 0x0400053C RID: 1340
		protected Collection<GenericParameter> generic_parameters;
	}
}
