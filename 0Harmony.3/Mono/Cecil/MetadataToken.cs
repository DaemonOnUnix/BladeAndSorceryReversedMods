using System;

namespace Mono.Cecil
{
	// Token: 0x0200018E RID: 398
	public struct MetadataToken : IEquatable<MetadataToken>
	{
		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06000CB7 RID: 3255 RVA: 0x0002B2D8 File Offset: 0x000294D8
		public uint RID
		{
			get
			{
				return this.token & 16777215U;
			}
		}

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06000CB8 RID: 3256 RVA: 0x0002B2E6 File Offset: 0x000294E6
		public TokenType TokenType
		{
			get
			{
				return (TokenType)(this.token & 4278190080U);
			}
		}

		// Token: 0x06000CB9 RID: 3257 RVA: 0x0002B2F4 File Offset: 0x000294F4
		public MetadataToken(uint token)
		{
			this.token = token;
		}

		// Token: 0x06000CBA RID: 3258 RVA: 0x0002B2FD File Offset: 0x000294FD
		public MetadataToken(TokenType type)
		{
			this = new MetadataToken(type, 0);
		}

		// Token: 0x06000CBB RID: 3259 RVA: 0x0002B307 File Offset: 0x00029507
		public MetadataToken(TokenType type, uint rid)
		{
			this.token = (uint)(type | (TokenType)rid);
		}

		// Token: 0x06000CBC RID: 3260 RVA: 0x0002B307 File Offset: 0x00029507
		public MetadataToken(TokenType type, int rid)
		{
			this.token = (uint)(type | (TokenType)rid);
		}

		// Token: 0x06000CBD RID: 3261 RVA: 0x0002B312 File Offset: 0x00029512
		public int ToInt32()
		{
			return (int)this.token;
		}

		// Token: 0x06000CBE RID: 3262 RVA: 0x0002B312 File Offset: 0x00029512
		public uint ToUInt32()
		{
			return this.token;
		}

		// Token: 0x06000CBF RID: 3263 RVA: 0x0002B312 File Offset: 0x00029512
		public override int GetHashCode()
		{
			return (int)this.token;
		}

		// Token: 0x06000CC0 RID: 3264 RVA: 0x0002B31A File Offset: 0x0002951A
		public bool Equals(MetadataToken other)
		{
			return other.token == this.token;
		}

		// Token: 0x06000CC1 RID: 3265 RVA: 0x0002B32A File Offset: 0x0002952A
		public override bool Equals(object obj)
		{
			return obj is MetadataToken && ((MetadataToken)obj).token == this.token;
		}

		// Token: 0x06000CC2 RID: 3266 RVA: 0x0002B349 File Offset: 0x00029549
		public static bool operator ==(MetadataToken one, MetadataToken other)
		{
			return one.token == other.token;
		}

		// Token: 0x06000CC3 RID: 3267 RVA: 0x0002B359 File Offset: 0x00029559
		public static bool operator !=(MetadataToken one, MetadataToken other)
		{
			return one.token != other.token;
		}

		// Token: 0x06000CC4 RID: 3268 RVA: 0x0002B36C File Offset: 0x0002956C
		public override string ToString()
		{
			return string.Format("[{0}:0x{1}]", this.TokenType, this.RID.ToString("x4"));
		}

		// Token: 0x04000589 RID: 1417
		private readonly uint token;

		// Token: 0x0400058A RID: 1418
		public static readonly MetadataToken Zero = new MetadataToken(0U);
	}
}
