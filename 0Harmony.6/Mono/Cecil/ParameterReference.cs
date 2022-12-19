using System;

namespace Mono.Cecil
{
	// Token: 0x0200015D RID: 349
	public abstract class ParameterReference : IMetadataTokenProvider
	{
		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000AFE RID: 2814 RVA: 0x00026C56 File Offset: 0x00024E56
		// (set) Token: 0x06000AFF RID: 2815 RVA: 0x00026C5E File Offset: 0x00024E5E
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

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000B00 RID: 2816 RVA: 0x00026C67 File Offset: 0x00024E67
		public int Index
		{
			get
			{
				return this.index;
			}
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000B01 RID: 2817 RVA: 0x00026C6F File Offset: 0x00024E6F
		// (set) Token: 0x06000B02 RID: 2818 RVA: 0x00026C77 File Offset: 0x00024E77
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

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000B03 RID: 2819 RVA: 0x00026C80 File Offset: 0x00024E80
		// (set) Token: 0x06000B04 RID: 2820 RVA: 0x00026C88 File Offset: 0x00024E88
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

		// Token: 0x06000B05 RID: 2821 RVA: 0x00026C91 File Offset: 0x00024E91
		internal ParameterReference(string name, TypeReference parameterType)
		{
			if (parameterType == null)
			{
				throw new ArgumentNullException("parameterType");
			}
			this.name = name ?? string.Empty;
			this.parameter_type = parameterType;
		}

		// Token: 0x06000B06 RID: 2822 RVA: 0x00026C56 File Offset: 0x00024E56
		public override string ToString()
		{
			return this.name;
		}

		// Token: 0x06000B07 RID: 2823
		public abstract ParameterDefinition Resolve();

		// Token: 0x04000454 RID: 1108
		private string name;

		// Token: 0x04000455 RID: 1109
		internal int index = -1;

		// Token: 0x04000456 RID: 1110
		protected TypeReference parameter_type;

		// Token: 0x04000457 RID: 1111
		internal MetadataToken token;
	}
}
