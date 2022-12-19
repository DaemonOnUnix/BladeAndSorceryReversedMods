using System;
using System.Runtime.Serialization;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001F5 RID: 501
	[Serializable]
	public sealed class SymbolsNotMatchingException : InvalidOperationException
	{
		// Token: 0x06000F89 RID: 3977 RVA: 0x00034EA1 File Offset: 0x000330A1
		public SymbolsNotMatchingException(string message)
			: base(message)
		{
		}

		// Token: 0x06000F8A RID: 3978 RVA: 0x00034EAA File Offset: 0x000330AA
		private SymbolsNotMatchingException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
