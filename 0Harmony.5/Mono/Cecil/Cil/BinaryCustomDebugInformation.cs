using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020002E0 RID: 736
	internal sealed class BinaryCustomDebugInformation : CustomDebugInformation
	{
		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x060012B5 RID: 4789 RVA: 0x0003C64E File Offset: 0x0003A84E
		// (set) Token: 0x060012B6 RID: 4790 RVA: 0x0003C656 File Offset: 0x0003A856
		public byte[] Data
		{
			get
			{
				return this.data;
			}
			set
			{
				this.data = value;
			}
		}

		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x060012B7 RID: 4791 RVA: 0x00017DC4 File Offset: 0x00015FC4
		public override CustomDebugInformationKind Kind
		{
			get
			{
				return CustomDebugInformationKind.Binary;
			}
		}

		// Token: 0x060012B8 RID: 4792 RVA: 0x0003C65F File Offset: 0x0003A85F
		public BinaryCustomDebugInformation(Guid identifier, byte[] data)
			: base(identifier)
		{
			this.data = data;
		}

		// Token: 0x0400097F RID: 2431
		private byte[] data;
	}
}
