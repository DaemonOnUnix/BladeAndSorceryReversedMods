using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001EE RID: 494
	internal sealed class EmbeddedSourceDebugInformation : CustomDebugInformation
	{
		// Token: 0x17000340 RID: 832
		// (get) Token: 0x06000F60 RID: 3936 RVA: 0x0003497F File Offset: 0x00032B7F
		// (set) Token: 0x06000F61 RID: 3937 RVA: 0x00034987 File Offset: 0x00032B87
		public byte[] Content
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

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x06000F62 RID: 3938 RVA: 0x00034990 File Offset: 0x00032B90
		// (set) Token: 0x06000F63 RID: 3939 RVA: 0x00034998 File Offset: 0x00032B98
		public bool Compress
		{
			get
			{
				return this.compress;
			}
			set
			{
				this.compress = value;
			}
		}

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06000F64 RID: 3940 RVA: 0x000349A1 File Offset: 0x00032BA1
		public override CustomDebugInformationKind Kind
		{
			get
			{
				return CustomDebugInformationKind.EmbeddedSource;
			}
		}

		// Token: 0x06000F65 RID: 3941 RVA: 0x000349A4 File Offset: 0x00032BA4
		public EmbeddedSourceDebugInformation(byte[] content, bool compress)
			: base(EmbeddedSourceDebugInformation.KindIdentifier)
		{
			this.content = content;
			this.compress = compress;
		}

		// Token: 0x0400094D RID: 2381
		internal byte[] content;

		// Token: 0x0400094E RID: 2382
		internal bool compress;

		// Token: 0x0400094F RID: 2383
		public static Guid KindIdentifier = new Guid("{0E8A571B-6926-466E-B4AD-8AB04611F5FE}");
	}
}
