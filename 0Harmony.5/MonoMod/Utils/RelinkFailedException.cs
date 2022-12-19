using System;
using System.Text;
using Mono.Cecil;

namespace MonoMod.Utils
{
	// Token: 0x02000457 RID: 1111
	internal class RelinkFailedException : Exception
	{
		// Token: 0x060017B0 RID: 6064 RVA: 0x00052EC4 File Offset: 0x000510C4
		public RelinkFailedException(IMetadataTokenProvider mtp, IMetadataTokenProvider context = null)
			: this(RelinkFailedException._Format("MonoMod failed relinking", mtp, context), mtp, context)
		{
		}

		// Token: 0x060017B1 RID: 6065 RVA: 0x00052EDA File Offset: 0x000510DA
		public RelinkFailedException(string message, IMetadataTokenProvider mtp, IMetadataTokenProvider context = null)
			: base(message)
		{
			this.MTP = mtp;
			this.Context = context;
		}

		// Token: 0x060017B2 RID: 6066 RVA: 0x00052EF1 File Offset: 0x000510F1
		public RelinkFailedException(string message, Exception innerException, IMetadataTokenProvider mtp, IMetadataTokenProvider context = null)
			: base(message ?? RelinkFailedException._Format("MonoMod failed relinking", mtp, context), innerException)
		{
			this.MTP = mtp;
			this.Context = context;
		}

		// Token: 0x060017B3 RID: 6067 RVA: 0x00052F1C File Offset: 0x0005111C
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

		// Token: 0x0400105E RID: 4190
		public const string DefaultMessage = "MonoMod failed relinking";

		// Token: 0x0400105F RID: 4191
		public IMetadataTokenProvider MTP;

		// Token: 0x04001060 RID: 4192
		public IMetadataTokenProvider Context;
	}
}
