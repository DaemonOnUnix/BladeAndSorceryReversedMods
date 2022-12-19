using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020002DF RID: 735
	public abstract class CustomDebugInformation : DebugInformation
	{
		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x060012B2 RID: 4786 RVA: 0x0003C627 File Offset: 0x0003A827
		public Guid Identifier
		{
			get
			{
				return this.identifier;
			}
		}

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x060012B3 RID: 4787
		public abstract CustomDebugInformationKind Kind { get; }

		// Token: 0x060012B4 RID: 4788 RVA: 0x0003C62F File Offset: 0x0003A82F
		internal CustomDebugInformation(Guid identifier)
		{
			this.identifier = identifier;
			this.token = new MetadataToken(TokenType.CustomDebugInformation);
		}

		// Token: 0x0400097E RID: 2430
		private Guid identifier;
	}
}
