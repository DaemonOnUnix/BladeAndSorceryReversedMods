using System;
using System.Runtime.CompilerServices;

namespace HarmonyLib
{
	// Token: 0x0200003F RID: 63
	internal class Sandbox
	{
		// Token: 0x06000141 RID: 321 RVA: 0x00009560 File Offset: 0x00007760
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal Sandbox.SomeStruct_Net GetStruct_Net(IntPtr x, IntPtr y)
		{
			throw new Exception("This method should've been detoured!");
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00009560 File Offset: 0x00007760
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal Sandbox.SomeStruct_Mono GetStruct_Mono(IntPtr x, IntPtr y)
		{
			throw new Exception("This method should've been detoured!");
		}

		// Token: 0x06000143 RID: 323 RVA: 0x0000956C File Offset: 0x0000776C
		internal static void GetStructReplacement_Net(Sandbox self, IntPtr ptr, IntPtr a, IntPtr b)
		{
			Sandbox.hasStructReturnBuffer_Net = a == Sandbox.magicValue && b == Sandbox.magicValue;
		}

		// Token: 0x06000144 RID: 324 RVA: 0x0000958E File Offset: 0x0000778E
		internal static void GetStructReplacement_Mono(Sandbox self, IntPtr ptr, IntPtr a, IntPtr b)
		{
			Sandbox.hasStructReturnBuffer_Mono = a == Sandbox.magicValue && b == Sandbox.magicValue;
		}

		// Token: 0x040000D3 RID: 211
		internal static bool hasStructReturnBuffer_Net;

		// Token: 0x040000D4 RID: 212
		internal static bool hasStructReturnBuffer_Mono;

		// Token: 0x040000D5 RID: 213
		internal static readonly IntPtr magicValue = (IntPtr)305419896;

		// Token: 0x02000040 RID: 64
		internal struct SomeStruct_Net
		{
			// Token: 0x040000D6 RID: 214
			private readonly byte b1;

			// Token: 0x040000D7 RID: 215
			private readonly byte b2;

			// Token: 0x040000D8 RID: 216
			private readonly byte b3;
		}

		// Token: 0x02000041 RID: 65
		internal struct SomeStruct_Mono
		{
			// Token: 0x040000D9 RID: 217
			private readonly byte b1;

			// Token: 0x040000DA RID: 218
			private readonly byte b2;

			// Token: 0x040000DB RID: 219
			private readonly byte b3;

			// Token: 0x040000DC RID: 220
			private readonly byte b4;
		}
	}
}
