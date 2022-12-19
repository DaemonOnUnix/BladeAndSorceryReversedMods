using System;
using System.IO;
using System.Runtime.Serialization;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001F4 RID: 500
	[Serializable]
	public sealed class SymbolsNotFoundException : FileNotFoundException
	{
		// Token: 0x06000F87 RID: 3975 RVA: 0x00034E98 File Offset: 0x00033098
		public SymbolsNotFoundException(string message)
			: base(message)
		{
		}

		// Token: 0x06000F88 RID: 3976 RVA: 0x0001F72A File Offset: 0x0001D92A
		private SymbolsNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
