using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020002E5 RID: 741
	internal sealed class SourceLinkDebugInformation : CustomDebugInformation
	{
		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x060012D7 RID: 4823 RVA: 0x0003C91E File Offset: 0x0003AB1E
		// (set) Token: 0x060012D8 RID: 4824 RVA: 0x0003C926 File Offset: 0x0003AB26
		public string Content
		{
			get
			{
				return this.content;
			}
			set
			{
				this.content = value;
			}
		}

		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x060012D9 RID: 4825 RVA: 0x0003C92F File Offset: 0x0003AB2F
		public override CustomDebugInformationKind Kind
		{
			get
			{
				return CustomDebugInformationKind.SourceLink;
			}
		}

		// Token: 0x060012DA RID: 4826 RVA: 0x0003C932 File Offset: 0x0003AB32
		public SourceLinkDebugInformation(string content)
			: base(SourceLinkDebugInformation.KindIdentifier)
		{
			this.content = content;
		}

		// Token: 0x0400098F RID: 2447
		internal string content;

		// Token: 0x04000990 RID: 2448
		public static Guid KindIdentifier = new Guid("{CC110556-A091-4D38-9FEC-25AB9A351A6A}");
	}
}
