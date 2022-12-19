using System;
using System.IO;
using System.Runtime.Serialization;

namespace Mono.Cecil.Cil
{
	// Token: 0x020002EA RID: 746
	[Serializable]
	public sealed class SymbolsNotFoundException : FileNotFoundException
	{
		// Token: 0x060012F7 RID: 4855 RVA: 0x0003CDE4 File Offset: 0x0003AFE4
		public SymbolsNotFoundException(string message)
			: base(message)
		{
		}

		// Token: 0x060012F8 RID: 4856 RVA: 0x000255CA File Offset: 0x000237CA
		private SymbolsNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
