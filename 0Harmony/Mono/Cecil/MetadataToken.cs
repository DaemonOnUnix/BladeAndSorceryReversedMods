using System;

namespace Mono.Cecil
{
	// Token: 0x02000283 RID: 643
	public struct MetadataToken : IEquatable<MetadataToken>
	{
		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x0600101A RID: 4122 RVA: 0x00032CB8 File Offset: 0x00030EB8
		public uint RID
		{
			get
			{
				return this.token & 16777215U;
			}
		}

		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x0600101B RID: 4123 RVA: 0x00032CC6 File Offset: 0x00030EC6
		public TokenType TokenType
		{
			get
			{
				return (TokenType)(this.token & 4278190080U);
			}
		}

		// Token: 0x0600101C RID: 4124 RVA: 0x00032CD4 File Offset: 0x00030ED4
		public MetadataToken(uint token)
		{
			this.token = token;
		}

		// Token: 0x0600101D RID: 4125 RVA: 0x00032CDD File Offset: 0x00030EDD
		public MetadataToken(TokenType type)
		{
			this = new MetadataToken(type, 0);
		}

		// Token: 0x0600101E RID: 4126 RVA: 0x00032CE7 File Offset: 0x00030EE7
		public MetadataToken(TokenType type, uint rid)
		{
			this.token = (uint)(type | (TokenType)rid);
		}

		// Token: 0x0600101F RID: 4127 RVA: 0x00032CE7 File Offset: 0x00030EE7
		public MetadataToken(TokenType type, int rid)
		{
			this.token = (uint)(type | (TokenType)rid);
		}

		// Token: 0x06001020 RID: 4128 RVA: 0x00032CF2 File Offset: 0x00030EF2
		public int ToInt32()
		{
			return (int)this.token;
		}

		// Token: 0x06001021 RID: 4129 RVA: 0x00032CF2 File Offset: 0x00030EF2
		public uint ToUInt32()
		{
			return this.token;
		}

		// Token: 0x06001022 RID: 4130 RVA: 0x00032CF2 File Offset: 0x00030EF2
		public override int GetHashCode()
		{
			return (int)this.token;
		}

		// Token: 0x06001023 RID: 4131 RVA: 0x00032CFA File Offset: 0x00030EFA
		public bool Equals(MetadataToken other)
		{
			return other.token == this.token;
		}

		// Token: 0x06001024 RID: 4132 RVA: 0x00032D0A File Offset: 0x00030F0A
		public override bool Equals(object obj)
		{
			return obj is MetadataToken && ((MetadataToken)obj).token == this.token;
		}

		// Token: 0x06001025 RID: 4133 RVA: 0x00032D29 File Offset: 0x00030F29
		public static bool operator ==(MetadataToken one, MetadataToken other)
		{
			return one.token == other.token;
		}

		// Token: 0x06001026 RID: 4134 RVA: 0x00032D39 File Offset: 0x00030F39
		public static bool operator !=(MetadataToken one, MetadataToken other)
		{
			return one.token != other.token;
		}

		// Token: 0x06001027 RID: 4135 RVA: 0x00032D4C File Offset: 0x00030F4C
		public override string ToString()
		{
			return string.Format("[{0}:0x{1}]", this.TokenType, this.RID.ToString("x4"));
		}

		// Token: 0x040005C0 RID: 1472
		private readonly uint token;

		// Token: 0x040005C1 RID: 1473
		public static readonly MetadataToken Zero = new MetadataToken(0U);
	}
}
