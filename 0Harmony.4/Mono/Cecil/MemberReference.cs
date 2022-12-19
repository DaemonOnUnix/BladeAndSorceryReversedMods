using System;

namespace Mono.Cecil
{
	// Token: 0x0200022C RID: 556
	public abstract class MemberReference : IMetadataTokenProvider
	{
		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06000C13 RID: 3091 RVA: 0x00029468 File Offset: 0x00027668
		// (set) Token: 0x06000C14 RID: 3092 RVA: 0x00029470 File Offset: 0x00027670
		public virtual string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				if (this.IsWindowsRuntimeProjection && value != this.name)
				{
					throw new InvalidOperationException();
				}
				this.name = value;
			}
		}

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06000C15 RID: 3093
		public abstract string FullName { get; }

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06000C16 RID: 3094 RVA: 0x00029495 File Offset: 0x00027695
		// (set) Token: 0x06000C17 RID: 3095 RVA: 0x0002949D File Offset: 0x0002769D
		public virtual TypeReference DeclaringType
		{
			get
			{
				return this.declaring_type;
			}
			set
			{
				this.declaring_type = value;
			}
		}

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06000C18 RID: 3096 RVA: 0x000294A6 File Offset: 0x000276A6
		// (set) Token: 0x06000C19 RID: 3097 RVA: 0x000294AE File Offset: 0x000276AE
		public MetadataToken MetadataToken
		{
			get
			{
				return this.token;
			}
			set
			{
				this.token = value;
			}
		}

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06000C1A RID: 3098 RVA: 0x000294B7 File Offset: 0x000276B7
		public bool IsWindowsRuntimeProjection
		{
			get
			{
				return this.projection != null;
			}
		}

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06000C1B RID: 3099 RVA: 0x000294C4 File Offset: 0x000276C4
		internal bool HasImage
		{
			get
			{
				ModuleDefinition module = this.Module;
				return module != null && module.HasImage;
			}
		}

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06000C1C RID: 3100 RVA: 0x000294E3 File Offset: 0x000276E3
		public virtual ModuleDefinition Module
		{
			get
			{
				if (this.declaring_type == null)
				{
					return null;
				}
				return this.declaring_type.Module;
			}
		}

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06000C1D RID: 3101 RVA: 0x00017DC4 File Offset: 0x00015FC4
		public virtual bool IsDefinition
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06000C1E RID: 3102 RVA: 0x000294FA File Offset: 0x000276FA
		public virtual bool ContainsGenericParameter
		{
			get
			{
				return this.declaring_type != null && this.declaring_type.ContainsGenericParameter;
			}
		}

		// Token: 0x06000C1F RID: 3103 RVA: 0x00002AED File Offset: 0x00000CED
		internal MemberReference()
		{
		}

		// Token: 0x06000C20 RID: 3104 RVA: 0x00029511 File Offset: 0x00027711
		internal MemberReference(string name)
		{
			this.name = name ?? string.Empty;
		}

		// Token: 0x06000C21 RID: 3105 RVA: 0x00029529 File Offset: 0x00027729
		internal string MemberFullName()
		{
			if (this.declaring_type == null)
			{
				return this.name;
			}
			return this.declaring_type.FullName + "::" + this.name;
		}

		// Token: 0x06000C22 RID: 3106 RVA: 0x00029555 File Offset: 0x00027755
		public IMemberDefinition Resolve()
		{
			return this.ResolveDefinition();
		}

		// Token: 0x06000C23 RID: 3107
		protected abstract IMemberDefinition ResolveDefinition();

		// Token: 0x06000C24 RID: 3108 RVA: 0x0002955D File Offset: 0x0002775D
		public override string ToString()
		{
			return this.FullName;
		}

		// Token: 0x0400035D RID: 861
		private string name;

		// Token: 0x0400035E RID: 862
		private TypeReference declaring_type;

		// Token: 0x0400035F RID: 863
		internal MetadataToken token;

		// Token: 0x04000360 RID: 864
		internal object projection;
	}
}
