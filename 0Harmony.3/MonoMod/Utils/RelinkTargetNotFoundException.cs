using System;
using Mono.Cecil;

namespace MonoMod.Utils
{
	// Token: 0x0200035A RID: 858
	internal class RelinkTargetNotFoundException : RelinkFailedException
	{
		// Token: 0x0600140E RID: 5134 RVA: 0x0004A049 File Offset: 0x00048249
		public RelinkTargetNotFoundException(IMetadataTokenProvider mtp, IMetadataTokenProvider context = null)
			: base(RelinkFailedException._Format("MonoMod relinker failed finding", mtp, context), mtp, context)
		{
		}

		// Token: 0x0600140F RID: 5135 RVA: 0x0004A05F File Offset: 0x0004825F
		public RelinkTargetNotFoundException(string message, IMetadataTokenProvider mtp, IMetadataTokenProvider context = null)
			: base(message ?? "MonoMod relinker failed finding", mtp, context)
		{
		}

		// Token: 0x06001410 RID: 5136 RVA: 0x0004A073 File Offset: 0x00048273
		public RelinkTargetNotFoundException(string message, Exception innerException, IMetadataTokenProvider mtp, IMetadataTokenProvider context = null)
			: base(message ?? "MonoMod relinker failed finding", innerException, mtp, context)
		{
		}

		// Token: 0x04000FFF RID: 4095
		public new const string DefaultMessage = "MonoMod relinker failed finding";
	}
}
