using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001EA RID: 490
	internal sealed class BinaryCustomDebugInformation : CustomDebugInformation
	{
		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06000F48 RID: 3912 RVA: 0x000347C2 File Offset: 0x000329C2
		// (set) Token: 0x06000F49 RID: 3913 RVA: 0x000347CA File Offset: 0x000329CA
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

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06000F4A RID: 3914 RVA: 0x00011F38 File Offset: 0x00010138
		public override CustomDebugInformationKind Kind
		{
			get
			{
				return CustomDebugInformationKind.Binary;
			}
		}

		// Token: 0x06000F4B RID: 3915 RVA: 0x000347D3 File Offset: 0x000329D3
		public BinaryCustomDebugInformation(Guid identifier, byte[] data)
			: base(identifier)
		{
			this.data = data;
		}

		// Token: 0x04000943 RID: 2371
		private byte[] data;
	}
}
