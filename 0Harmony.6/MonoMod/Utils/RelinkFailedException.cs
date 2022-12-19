using System;
using System.Text;
using Mono.Cecil;

namespace MonoMod.Utils
{
	// Token: 0x02000359 RID: 857
	internal class RelinkFailedException : Exception
	{
		// Token: 0x0600140A RID: 5130 RVA: 0x00049F7E File Offset: 0x0004817E
		public RelinkFailedException(IMetadataTokenProvider mtp, IMetadataTokenProvider context = null)
			: this(RelinkFailedException._Format("MonoMod failed relinking", mtp, context), mtp, context)
		{
		}

		// Token: 0x0600140B RID: 5131 RVA: 0x00049F94 File Offset: 0x00048194
		public RelinkFailedException(string message, IMetadataTokenProvider mtp, IMetadataTokenProvider context = null)
			: base(message)
		{
			this.MTP = mtp;
			this.Context = context;
		}

		// Token: 0x0600140C RID: 5132 RVA: 0x00049FAB File Offset: 0x000481AB
		public RelinkFailedException(string message, Exception innerException, IMetadataTokenProvider mtp, IMetadataTokenProvider context = null)
			: base(message ?? RelinkFailedException._Format("MonoMod failed relinking", mtp, context), innerException)
		{
			this.MTP = mtp;
			this.Context = context;
		}

		// Token: 0x0600140D RID: 5133 RVA: 0x00049FD8 File Offset: 0x000481D8
		protected static string _Format(string message, IMetadataTokenProvider mtp, IMetadataTokenProvider context)
		{
			if (mtp == null && context == null)
			{
				return message;
			}
			StringBuilder stringBuilder = new StringBuilder(message);
			stringBuilder.Append(" ");
			if (mtp != null)
			{
				stringBuilder.Append(mtp.ToString());
			}
			if (context != null)
			{
				stringBuilder.Append(" ");
			}
			if (context != null)
			{
				stringBuilder.Append("(context: ").Append(context.ToString()).Append(")");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04000FFC RID: 4092
		public const string DefaultMessage = "MonoMod failed relinking";

		// Token: 0x04000FFD RID: 4093
		public IMetadataTokenProvider MTP;

		// Token: 0x04000FFE RID: 4094
		public IMetadataTokenProvider Context;
	}
}
