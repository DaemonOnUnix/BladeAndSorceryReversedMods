using System;
using System.Text;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000163 RID: 355
	public sealed class PropertyDefinition : PropertyReference, IMemberDefinition, ICustomAttributeProvider, IMetadataTokenProvider, IConstantProvider
	{
		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000B37 RID: 2871 RVA: 0x00027008 File Offset: 0x00025208
		// (set) Token: 0x06000B38 RID: 2872 RVA: 0x00027010 File Offset: 0x00025210
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

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06000B39 RID: 2873 RVA: 0x0002701C File Offset: 0x0002521C
		// (set) Token: 0x06000B3A RID: 2874 RVA: 0x0002706B File Offset: 0x0002526B
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

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06000B3B RID: 2875 RVA: 0x00027079 File Offset: 0x00025279
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

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000B3C RID: 2876 RVA: 0x0002709E File Offset: 0x0002529E
		public Collection<CustomAttribute> CustomAttributes
		{
			get
			{
				return this.custom_attributes ?? this.GetCustomAttributes(ref this.custom_attributes, this.Module);
			}
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000B3D RID: 2877 RVA: 0x000270BC File Offset: 0x000252BC
		// (set) Token: 0x06000B3E RID: 2878 RVA: 0x000270D9 File Offset: 0x000252D9
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

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000B3F RID: 2879 RVA: 0x000270E2 File Offset: 0x000252E2
		// (set) Token: 0x06000B40 RID: 2880 RVA: 0x000270FF File Offset: 0x000252FF
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

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000B41 RID: 2881 RVA: 0x00027108 File Offset: 0x00025308
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

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000B42 RID: 2882 RVA: 0x00027135 File Offset: 0x00025335
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

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000B43 RID: 2883 RVA: 0x00027174 File Offset: 0x00025374
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

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000B44 RID: 2884 RVA: 0x000271C7 File Offset: 0x000253C7
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

		// Token: 0x06000B45 RID: 2885 RVA: 0x00027200 File Offset: 0x00025400
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

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000B46 RID: 2886 RVA: 0x00027247 File Offset: 0x00025447
		// (set) Token: 0x06000B47 RID: 2887 RVA: 0x0002726B File Offset: 0x0002546B
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

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000B48 RID: 2888 RVA: 0x0002727B File Offset: 0x0002547B
		// (set) Token: 0x06000B49 RID: 2889 RVA: 0x0002728D File Offset: 0x0002548D
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

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000B4A RID: 2890 RVA: 0x00027296 File Offset: 0x00025496
		// (set) Token: 0x06000B4B RID: 2891 RVA: 0x000272A8 File Offset: 0x000254A8
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

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06000B4C RID: 2892 RVA: 0x000272C1 File Offset: 0x000254C1
		// (set) Token: 0x06000B4D RID: 2893 RVA: 0x000272D3 File Offset: 0x000254D3
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

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000B4E RID: 2894 RVA: 0x000272EC File Offset: 0x000254EC
		// (set) Token: 0x06000B4F RID: 2895 RVA: 0x000272FE File Offset: 0x000254FE
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

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000B50 RID: 2896 RVA: 0x000207D4 File Offset: 0x0001E9D4
		// (set) Token: 0x06000B51 RID: 2897 RVA: 0x000207E1 File Offset: 0x0001E9E1
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

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000B52 RID: 2898 RVA: 0x00012561 File Offset: 0x00010761
		public override bool IsDefinition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06000B53 RID: 2899 RVA: 0x00027318 File Offset: 0x00025518
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

		// Token: 0x06000B54 RID: 2900 RVA: 0x000273B0 File Offset: 0x000255B0
		public PropertyDefinition(string name, PropertyAttributes attributes, TypeReference propertyType)
			: base(name, propertyType)
		{
			this.attributes = (ushort)attributes;
			this.token = new MetadataToken(TokenType.Property);
		}

		// Token: 0x06000B55 RID: 2901 RVA: 0x000273DC File Offset: 0x000255DC
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

		// Token: 0x06000B56 RID: 2902 RVA: 0x00011FA0 File Offset: 0x000101A0
		public override PropertyDefinition Resolve()
		{
			return this;
		}

		// Token: 0x04000475 RID: 1141
		private bool? has_this;

		// Token: 0x04000476 RID: 1142
		private ushort attributes;

		// Token: 0x04000477 RID: 1143
		private Collection<CustomAttribute> custom_attributes;

		// Token: 0x04000478 RID: 1144
		internal MethodDefinition get_method;

		// Token: 0x04000479 RID: 1145
		internal MethodDefinition set_method;

		// Token: 0x0400047A RID: 1146
		internal Collection<MethodDefinition> other_methods;

		// Token: 0x0400047B RID: 1147
		private object constant = Mixin.NotResolved;
	}
}
