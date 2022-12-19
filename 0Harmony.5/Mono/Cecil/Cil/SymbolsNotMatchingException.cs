using System;
using System.Runtime.Serialization;

namespace Mono.Cecil.Cil
{
	// Token: 0x020002EB RID: 747
	[Serializable]
	public sealed class SymbolsNotMatchingException : InvalidOperationException
	{
		// Token: 0x060012F9 RID: 4857 RVA: 0x0003CDED File Offset: 0x0003AFED
		public SymbolsNotMatchingException(string message)
			: base(message)
		{
		}

		// Token: 0x060012FA RID: 4858 RVA: 0x0003CDF6 File Offset: 0x0003AFF6
		private SymbolsNotMatchingException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
