using System;
using Mono.Cecil;

namespace MonoMod.Utils
{
	// Token: 0x02000458 RID: 1112
	internal class RelinkTargetNotFoundException : RelinkFailedException
	{
		// Token: 0x060017B4 RID: 6068 RVA: 0x00052F8D File Offset: 0x0005118D
		public RelinkTargetNotFoundException(IMetadataTokenProvider mtp, IMetadataTokenProvider context = null)
			: base(RelinkFailedException._Format("MonoMod relinker failed finding", mtp, context), mtp, context)
		{
		}

		// Token: 0x060017B5 RID: 6069 RVA: 0x00052FA3 File Offset: 0x000511A3
		public RelinkTargetNotFoundException(string message, IMetadataTokenProvider mtp, IMetadataTokenProvider context = null)
			: base(message ?? "MonoMod relinker failed finding", mtp, context)
		{
		}

		// Token: 0x060017B6 RID: 6070 RVA: 0x00052FB7 File Offset: 0x000511B7
		public RelinkTargetNotFoundException(string message, Exception innerException, IMetadataTokenProvider mtp, IMetadataTokenProvider context = null)
			: base(message ?? "MonoMod relinker failed finding", innerException, mtp, context)
		{
		}

		// Token: 0x04001061 RID: 4193
		public new const string DefaultMessage = "MonoMod relinker failed finding";
	}
}
