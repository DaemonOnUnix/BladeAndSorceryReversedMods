using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001E9 RID: 489
	public abstract class CustomDebugInformation : DebugInformation
	{
		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06000F45 RID: 3909 RVA: 0x0003479B File Offset: 0x0003299B
		public Guid Identifier
		{
			get
			{
				return this.identifier;
			}
		}

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06000F46 RID: 3910
		public abstract CustomDebugInformationKind Kind { get; }

		// Token: 0x06000F47 RID: 3911 RVA: 0x000347A3 File Offset: 0x000329A3
		internal CustomDebugInformation(Guid identifier)
		{
			this.identifier = identifier;
			this.token = new MetadataToken(TokenType.CustomDebugInformation);
		}

		// Token: 0x04000942 RID: 2370
		private Guid identifier;
	}
}
