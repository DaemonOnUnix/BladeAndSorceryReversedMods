using System;

namespace Mono.Cecil
{
	// Token: 0x02000139 RID: 313
	public abstract class MemberReference : IMetadataTokenProvider
	{
		// Token: 0x17000130 RID: 304
		// (get) Token: 0x060008CE RID: 2254 RVA: 0x00023190 File Offset: 0x00021390
		// (set) Token: 0x060008CF RID: 2255 RVA: 0x00023198 File Offset: 0x00021398
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

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x060008D0 RID: 2256
		public abstract string FullName { get; }

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x060008D1 RID: 2257 RVA: 0x000231BD File Offset: 0x000213BD
		// (set) Token: 0x060008D2 RID: 2258 RVA: 0x000231C5 File Offset: 0x000213C5
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

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x060008D3 RID: 2259 RVA: 0x000231CE File Offset: 0x000213CE
		// (set) Token: 0x060008D4 RID: 2260 RVA: 0x000231D6 File Offset: 0x000213D6
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

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060008D5 RID: 2261 RVA: 0x000231DF File Offset: 0x000213DF
		public bool IsWindowsRuntimeProjection
		{
			get
			{
				return this.projection != null;
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060008D6 RID: 2262 RVA: 0x000231EA File Offset: 0x000213EA
		// (set) Token: 0x060008D7 RID: 2263 RVA: 0x00020F72 File Offset: 0x0001F172
		internal MemberReferenceProjection WindowsRuntimeProjection
		{
			get
			{
				return (MemberReferenceProjection)this.projection;
			}
			set
			{
				this.projection = value;
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060008D8 RID: 2264 RVA: 0x000231F8 File Offset: 0x000213F8
		internal bool HasImage
		{
			get
			{
				ModuleDefinition module = this.Module;
				return module != null && module.HasImage;
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x060008D9 RID: 2265 RVA: 0x00023217 File Offset: 0x00021417
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

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x060008DA RID: 2266 RVA: 0x00011F38 File Offset: 0x00010138
		public virtual bool IsDefinition
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x060008DB RID: 2267 RVA: 0x0002322E File Offset: 0x0002142E
		public virtual bool ContainsGenericParameter
		{
			get
			{
				return this.declaring_type != null && this.declaring_type.ContainsGenericParameter;
			}
		}

		// Token: 0x060008DC RID: 2268 RVA: 0x00002AED File Offset: 0x00000CED
		internal MemberReference()
		{
		}

		// Token: 0x060008DD RID: 2269 RVA: 0x00023245 File Offset: 0x00021445
		internal MemberReference(string name)
		{
			this.name = name ?? string.Empty;
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x0002325D File Offset: 0x0002145D
		internal string MemberFullName()
		{
			if (this.declaring_type == null)
			{
				return this.name;
			}
			return this.declaring_type.FullName + "::" + this.name;
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x00023289 File Offset: 0x00021489
		public IMemberDefinition Resolve()
		{
			return this.ResolveDefinition();
		}

		// Token: 0x060008E0 RID: 2272
		protected abstract IMemberDefinition ResolveDefinition();

		// Token: 0x060008E1 RID: 2273 RVA: 0x00023291 File Offset: 0x00021491
		public override string ToString()
		{
			return this.FullName;
		}

		// Token: 0x0400032B RID: 811
		private string name;

		// Token: 0x0400032C RID: 812
		private TypeReference declaring_type;

		// Token: 0x0400032D RID: 813
		internal MetadataToken token;

		// Token: 0x0400032E RID: 814
		internal object projection;
	}
}
