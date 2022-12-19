using System;

namespace Mono.Cecil
{
	// Token: 0x02000251 RID: 593
	public abstract class ParameterReference : IMetadataTokenProvider
	{
		// Token: 0x170003DC RID: 988
		// (get) Token: 0x06000E48 RID: 3656 RVA: 0x0002D2BE File Offset: 0x0002B4BE
		// (set) Token: 0x06000E49 RID: 3657 RVA: 0x0002D2C6 File Offset: 0x0002B4C6
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

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x06000E4A RID: 3658 RVA: 0x0002D2CF File Offset: 0x0002B4CF
		public int Index
		{
			get
			{
				return this.index;
			}
		}

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x06000E4B RID: 3659 RVA: 0x0002D2D7 File Offset: 0x0002B4D7
		// (set) Token: 0x06000E4C RID: 3660 RVA: 0x0002D2DF File Offset: 0x0002B4DF
		public TypeReference ParameterType
		{
			get
			{
				return this.parameter_type;
			}
			set
			{
				this.parameter_type = value;
			}
		}

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x06000E4D RID: 3661 RVA: 0x0002D2E8 File Offset: 0x0002B4E8
		// (set) Token: 0x06000E4E RID: 3662 RVA: 0x0002D2F0 File Offset: 0x0002B4F0
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

		// Token: 0x06000E4F RID: 3663 RVA: 0x0002D2F9 File Offset: 0x0002B4F9
		internal ParameterReference(string name, TypeReference parameterType)
		{
			if (parameterType == null)
			{
				throw new ArgumentNullException("parameterType");
			}
			this.name = name ?? string.Empty;
			this.parameter_type = parameterType;
		}

		// Token: 0x06000E50 RID: 3664 RVA: 0x0002D2BE File Offset: 0x0002B4BE
		public override string ToString()
		{
			return this.name;
		}

		// Token: 0x06000E51 RID: 3665
		public abstract ParameterDefinition Resolve();

		// Token: 0x04000489 RID: 1161
		private string name;

		// Token: 0x0400048A RID: 1162
		internal int index = -1;

		// Token: 0x0400048B RID: 1163
		protected TypeReference parameter_type;

		// Token: 0x0400048C RID: 1164
		internal MetadataToken token;
	}
}
