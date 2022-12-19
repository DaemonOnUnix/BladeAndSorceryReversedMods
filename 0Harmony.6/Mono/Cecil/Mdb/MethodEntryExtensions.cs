using System;
using Mono.CompilerServices.SymbolWriter;

namespace Mono.Cecil.Mdb
{
	// Token: 0x02000224 RID: 548
	internal static class MethodEntryExtensions
	{
		// Token: 0x06001095 RID: 4245 RVA: 0x0003909F File Offset: 0x0003729F
		public static bool HasColumnInfo(this MethodEntry entry)
		{
			return (entry.MethodFlags & MethodEntry.Flags.ColumnsInfoIncluded) > (MethodEntry.Flags)0;
		}

		// Token: 0x06001096 RID: 4246 RVA: 0x000390AC File Offset: 0x000372AC
		public static bool HasEndInfo(this MethodEntry entry)
		{
			return (entry.MethodFlags & MethodEntry.Flags.EndInfoIncluded) > (MethodEntry.Flags)0;
		}
	}
}
