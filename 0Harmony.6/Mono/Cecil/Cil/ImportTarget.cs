using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001E5 RID: 485
	public sealed class ImportTarget
	{
		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06000F33 RID: 3891 RVA: 0x000346DC File Offset: 0x000328DC
		// (set) Token: 0x06000F34 RID: 3892 RVA: 0x000346E4 File Offset: 0x000328E4
		public string Namespace
		{
			get
			{
				return this.@namespace;
			}
			set
			{
				this.@namespace = value;
			}
		}

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06000F35 RID: 3893 RVA: 0x000346ED File Offset: 0x000328ED
		// (set) Token: 0x06000F36 RID: 3894 RVA: 0x000346F5 File Offset: 0x000328F5
		public TypeReference Type
		{
			get
			{
				return this.type;
			}
			set
			{
				this.type = value;
			}
		}

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x06000F37 RID: 3895 RVA: 0x000346FE File Offset: 0x000328FE
		// (set) Token: 0x06000F38 RID: 3896 RVA: 0x00034706 File Offset: 0x00032906
		public AssemblyNameReference AssemblyReference
		{
			get
			{
				return this.reference;
			}
			set
			{
				this.reference = value;
			}
		}

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06000F39 RID: 3897 RVA: 0x0003470F File Offset: 0x0003290F
		// (set) Token: 0x06000F3A RID: 3898 RVA: 0x00034717 File Offset: 0x00032917
		public string Alias
		{
			get
			{
				return this.alias;
			}
			set
			{
				this.alias = value;
			}
		}

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x06000F3B RID: 3899 RVA: 0x00034720 File Offset: 0x00032920
		// (set) Token: 0x06000F3C RID: 3900 RVA: 0x00034728 File Offset: 0x00032928
		public ImportTargetKind Kind
		{
			get
			{
				return this.kind;
			}
			set
			{
				this.kind = value;
			}
		}

		// Token: 0x06000F3D RID: 3901 RVA: 0x00034731 File Offset: 0x00032931
		public ImportTarget(ImportTargetKind kind)
		{
			this.kind = kind;
		}

		// Token: 0x04000933 RID: 2355
		internal ImportTargetKind kind;

		// Token: 0x04000934 RID: 2356
		internal string @namespace;

		// Token: 0x04000935 RID: 2357
		internal TypeReference type;

		// Token: 0x04000936 RID: 2358
		internal AssemblyNameReference reference;

		// Token: 0x04000937 RID: 2359
		internal string alias;
	}
}
