using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001EF RID: 495
	internal sealed class SourceLinkDebugInformation : CustomDebugInformation
	{
		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06000F67 RID: 3943 RVA: 0x000349D0 File Offset: 0x00032BD0
		// (set) Token: 0x06000F68 RID: 3944 RVA: 0x000349D8 File Offset: 0x00032BD8
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

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x06000F69 RID: 3945 RVA: 0x000349E1 File Offset: 0x00032BE1
		public override CustomDebugInformationKind Kind
		{
			get
			{
				return CustomDebugInformationKind.SourceLink;
			}
		}

		// Token: 0x06000F6A RID: 3946 RVA: 0x000349E4 File Offset: 0x00032BE4
		public SourceLinkDebugInformation(string content)
			: base(SourceLinkDebugInformation.KindIdentifier)
		{
			this.content = content;
		}

		// Token: 0x04000950 RID: 2384
		internal string content;

		// Token: 0x04000951 RID: 2385
		public static Guid KindIdentifier = new Guid("{CC110556-A091-4D38-9FEC-25AB9A351A6A}");
	}
}
