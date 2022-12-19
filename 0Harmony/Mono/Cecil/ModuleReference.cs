using System;

namespace Mono.Cecil
{
	// Token: 0x0200024C RID: 588
	public class ModuleReference : IMetadataScope, IMetadataTokenProvider
	{
		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x06000E1B RID: 3611 RVA: 0x0002CF3C File Offset: 0x0002B13C
		// (set) Token: 0x06000E1C RID: 3612 RVA: 0x0002CF44 File Offset: 0x0002B144
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x06000E1D RID: 3613 RVA: 0x000183ED File Offset: 0x000165ED
		public virtual MetadataScopeType MetadataScopeType
		{
			get
			{
				return MetadataScopeType.ModuleReference;
			}
		}

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x06000E1E RID: 3614 RVA: 0x0002CF4D File Offset: 0x0002B14D
		// (set) Token: 0x06000E1F RID: 3615 RVA: 0x0002CF55 File Offset: 0x0002B155
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

		// Token: 0x06000E20 RID: 3616 RVA: 0x0002CF5E File Offset: 0x0002B15E
		internal ModuleReference()
		{
			this.token = new MetadataToken(TokenType.ModuleRef);
		}

		// Token: 0x06000E21 RID: 3617 RVA: 0x0002CF76 File Offset: 0x0002B176
		public ModuleReference(string name)
			: this()
		{
			this.name = name;
		}

		// Token: 0x06000E22 RID: 3618 RVA: 0x0002CF3C File Offset: 0x0002B13C
		public override string ToString()
		{
			return this.name;
		}

		// Token: 0x04000451 RID: 1105
		private string name;

		// Token: 0x04000452 RID: 1106
		internal MetadataToken token;
	}
}
