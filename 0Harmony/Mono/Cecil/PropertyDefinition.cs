using System;
using System.Text;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000257 RID: 599
	public sealed class PropertyDefinition : PropertyReference, IMemberDefinition, ICustomAttributeProvider, IMetadataTokenProvider, IConstantProvider
	{
		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x06000E81 RID: 3713 RVA: 0x0002D670 File Offset: 0x0002B870
		// (set) Token: 0x06000E82 RID: 3714 RVA: 0x0002D678 File Offset: 0x0002B878
		public PropertyAttributes Attributes
		{
			get
			{
				return (PropertyAttributes)this.attributes;
			}
			set
			{
				this.attributes = (ushort)value;
			}
		}

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x06000E83 RID: 3715 RVA: 0x0002D684 File Offset: 0x0002B884
		// (set) Token: 0x06000E84 RID: 3716 RVA: 0x0002D6D3 File Offset: 0x0002B8D3
		public bool HasThis
		{
			get
			{
				if (this.has_this != null)
				{
					return this.has_this.Value;
				}
				if (this.GetMethod != null)
				{
					return this.get_method.HasThis;
				}
				return this.SetMethod != null && this.set_method.HasThis;
			}
			set
			{
				this.has_this = new bool?(value);
			}
		}

		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x06000E85 RID: 3717 RVA: 0x0002D6E1 File Offset: 0x0002B8E1
		public bool HasCustomAttributes
		{
			get
			{
				if (this.custom_attributes != null)
				{
					return this.custom_attributes.Count > 0;
				}
				return this.GetHasCustomAttributes(this.Module);
			}
		}

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x06000E86 RID: 3718 RVA: 0x0002D706 File Offset: 0x0002B906
		public Collection<CustomAttribute> CustomAttributes
		{
			get
			{
				return this.custom_attributes ?? this.GetCustomAttributes(ref this.custom_attributes, this.Module);
			}
		}

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x06000E87 RID: 3719 RVA: 0x0002D724 File Offset: 0x0002B924
		// (set) Token: 0x06000E88 RID: 3720 RVA: 0x0002D741 File Offset: 0x0002B941
		public MethodDefinition GetMethod
		{
			get
			{
				if (this.get_method != null)
				{
					return this.get_method;
				}
				this.InitializeMethods();
				return this.get_method;
			}
			set
			{
				this.get_method = value;
			}
		}

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x06000E89 RID: 3721 RVA: 0x0002D74A File Offset: 0x0002B94A
		// (set) Token: 0x06000E8A RID: 3722 RVA: 0x0002D767 File Offset: 0x0002B967
		public MethodDefinition SetMethod
		{
			get
			{
				if (this.set_method != null)
				{
					return this.set_method;
				}
				this.InitializeMethods();
				return this.set_method;
			}
			set
			{
				this.set_method = value;
			}
		}

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x06000E8B RID: 3723 RVA: 0x0002D770 File Offset: 0x0002B970
		public bool HasOtherMethods
		{
			get
			{
				if (this.other_methods != null)
				{
					return this.other_methods.Count > 0;
				}
				this.InitializeMethods();
				return !this.other_methods.IsNullOrEmpty<MethodDefinition>();
			}
		}

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x06000E8C RID: 3724 RVA: 0x0002D79D File Offset: 0x0002B99D
		public Collection<MethodDefinition> OtherMethods
		{
			get
			{
				if (this.other_methods != null)
				{
					return this.other_methods;
				}
				this.InitializeMethods();
				if (this.other_methods != null)
				{
					return this.other_methods;
				}
				Interlocked.CompareExchange<Collection<MethodDefinition>>(ref this.other_methods, new Collection<MethodDefinition>(), null);
				return this.other_methods;
			}
		}

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x06000E8D RID: 3725 RVA: 0x0002D7DC File Offset: 0x0002B9DC
		public bool HasParameters
		{
			get
			{
				this.InitializeMethods();
				if (this.get_method != null)
				{
					return this.get_method.HasParameters;
				}
				return this.set_method != null && this.set_method.HasParameters && this.set_method.Parameters.Count > 1;
			}
		}

		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x06000E8E RID: 3726 RVA: 0x0002D82F File Offset: 0x0002BA2F
		public override Collection<ParameterDefinition> Parameters
		{
			get
			{
				this.InitializeMethods();
				if (this.get_method != null)
				{
					return PropertyDefinition.MirrorParameters(this.get_method, 0);
				}
				if (this.set_method != null)
				{
					return PropertyDefinition.MirrorParameters(this.set_method, 1);
				}
				return new Collection<ParameterDefinition>();
			}
		}

		// Token: 0x06000E8F RID: 3727 RVA: 0x0002D868 File Offset: 0x0002BA68
		private static Collection<ParameterDefinition> MirrorParameters(MethodDefinition method, int bound)
		{
			Collection<ParameterDefinition> collection = new Collection<ParameterDefinition>();
			if (!method.HasParameters)
			{
				return collection;
			}
			Collection<ParameterDefinition> parameters = method.Parameters;
			int num = parameters.Count - bound;
			for (int i = 0; i < num; i++)
			{
				collection.Add(parameters[i]);
			}
			return collection;
		}

		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x06000E90 RID: 3728 RVA: 0x0002D8AF File Offset: 0x0002BAAF
		// (set) Token: 0x06000E91 RID: 3729 RVA: 0x0002D8D3 File Offset: 0x0002BAD3
		public bool HasConstant
		{
			get
			{
				this.ResolveConstant(ref this.constant, this.Module);
				return this.constant != Mixin.NoValue;
			}
			set
			{
				if (!value)
				{
					this.constant = Mixin.NoValue;
				}
			}
		}

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x06000E92 RID: 3730 RVA: 0x0002D8E3 File Offset: 0x0002BAE3
		// (set) Token: 0x06000E93 RID: 3731 RVA: 0x0002D8F5 File Offset: 0x0002BAF5
		public object Constant
		{
			get
			{
				if (!this.HasConstant)
				{
					return null;
				}
				return this.constant;
			}
			set
			{
				this.constant = value;
			}
		}

		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x06000E94 RID: 3732 RVA: 0x0002D8FE File Offset: 0x0002BAFE
		// (set) Token: 0x06000E95 RID: 3733 RVA: 0x0002D910 File Offset: 0x0002BB10
		public bool IsSpecialName
		{
			get
			{
				return this.attributes.GetAttributes(512);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(512, value);
			}
		}

		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x06000E96 RID: 3734 RVA: 0x0002D929 File Offset: 0x0002BB29
		// (set) Token: 0x06000E97 RID: 3735 RVA: 0x0002D93B File Offset: 0x0002BB3B
		public bool IsRuntimeSpecialName
		{
			get
			{
				return this.attributes.GetAttributes(1024);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(1024, value);
			}
		}

		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x06000E98 RID: 3736 RVA: 0x0002D954 File Offset: 0x0002BB54
		// (set) Token: 0x06000E99 RID: 3737 RVA: 0x0002D966 File Offset: 0x0002BB66
		public bool HasDefault
		{
			get
			{
				return this.attributes.GetAttributes(4096);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(4096, value);
			}
		}

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x06000E9A RID: 3738 RVA: 0x0002667C File Offset: 0x0002487C
		// (set) Token: 0x06000E9B RID: 3739 RVA: 0x00026689 File Offset: 0x00024889
		public new TypeDefinition DeclaringType
		{
			get
			{
				return (TypeDefinition)base.DeclaringType;
			}
			set
			{
				base.DeclaringType = value;
			}
		}

		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x06000E9C RID: 3740 RVA: 0x000183ED File Offset: 0x000165ED
		public override bool IsDefinition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x06000E9D RID: 3741 RVA: 0x0002D980 File Offset: 0x0002BB80
		public override string FullName
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(base.PropertyType.ToString());
				stringBuilder.Append(' ');
				stringBuilder.Append(base.MemberFullName());
				stringBuilder.Append('(');
				if (this.HasParameters)
				{
					Collection<ParameterDefinition> parameters = this.Parameters;
					for (int i = 0; i < parameters.Count; i++)
					{
						if (i > 0)
						{
							stringBuilder.Append(',');
						}
						stringBuilder.Append(parameters[i].ParameterType.FullName);
					}
				}
				stringBuilder.Append(')');
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06000E9E RID: 3742 RVA: 0x0002DA18 File Offset: 0x0002BC18
		public PropertyDefinition(string name, PropertyAttributes attributes, TypeReference propertyType)
			: base(name, propertyType)
		{
			this.attributes = (ushort)attributes;
			this.token = new MetadataToken(TokenType.Property);
		}

		// Token: 0x06000E9F RID: 3743 RVA: 0x0002DA44 File Offset: 0x0002BC44
		private void InitializeMethods()
		{
			ModuleDefinition module = this.Module;
			if (module == null)
			{
				return;
			}
			object syncRoot = module.SyncRoot;
			lock (syncRoot)
			{
				if (this.get_method == null && this.set_method == null)
				{
					if (module.HasImage())
					{
						module.Read<PropertyDefinition>(this, delegate(PropertyDefinition property, MetadataReader reader)
						{
							reader.ReadMethods(property);
						});
					}
				}
			}
		}

		// Token: 0x06000EA0 RID: 3744 RVA: 0x00017E2C File Offset: 0x0001602C
		public override PropertyDefinition Resolve()
		{
			return this;
		}

		// Token: 0x040004AA RID: 1194
		private bool? has_this;

		// Token: 0x040004AB RID: 1195
		private ushort attributes;

		// Token: 0x040004AC RID: 1196
		private Collection<CustomAttribute> custom_attributes;

		// Token: 0x040004AD RID: 1197
		internal MethodDefinition get_method;

		// Token: 0x040004AE RID: 1198
		internal MethodDefinition set_method;

		// Token: 0x040004AF RID: 1199
		internal Collection<MethodDefinition> other_methods;

		// Token: 0x040004B0 RID: 1200
		private object constant = Mixin.NotResolved;
	}
}
