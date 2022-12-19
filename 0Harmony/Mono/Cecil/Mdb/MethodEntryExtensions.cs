using System;
using Mono.CompilerServices.SymbolWriter;

namespace Mono.Cecil.Mdb
{
	// Token: 0x0200031A RID: 794
	internal static class MethodEntryExtensions
	{
		// Token: 0x06001405 RID: 5125 RVA: 0x00040FEB File Offset: 0x0003F1EB
		public static bool HasColumnInfo(this MethodEntry entry)
		{
			return (entry.MethodFlags & MethodEntry.Flags.ColumnsInfoIncluded) > (MethodEntry.Flags)0;
		}

		// Token: 0x06001406 RID: 5126 RVA: 0x00040FF8 File Offset: 0x0003F1F8
		public static bool HasEndInfo(this MethodEntry entry)
		{
			return (entry.MethodFlags & MethodEntry.Flags.EndInfoIncluded) > (MethodEntry.Flags)0;
		}
	}
}
