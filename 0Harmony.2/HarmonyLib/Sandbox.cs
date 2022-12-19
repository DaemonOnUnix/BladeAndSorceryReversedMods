using System;
using System.Runtime.CompilerServices;

namespace HarmonyLib
{
	// Token: 0x02000040 RID: 64
	internal class Sandbox
	{
		// Token: 0x06000150 RID: 336 RVA: 0x0000A3A8 File Offset: 0x000085A8
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal Sandbox.SomeStruct_Net GetStruct_Net(IntPtr x, IntPtr y)
		{
			throw new Exception("This method should've been detoured!");
		}

		// Token: 0x06000151 RID: 337 RVA: 0x0000A3A8 File Offset: 0x000085A8
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal Sandbox.SomeStruct_Mono GetStruct_Mono(IntPtr x, IntPtr y)
		{
			throw new Exception("This method should've been detoured!");
		}

		// Token: 0x06000152 RID: 338 RVA: 0x0000A3B4 File Offset: 0x000085B4
		internal static void GetStructReplacement_Net(Sandbox self, IntPtr ptr, IntPtr a, IntPtr b)
		{
			Sandbox.hasStructReturnBuffer_Net = a == Sandbox.magicValue && b == Sandbox.magicValue;
		}

		// Token: 0x06000153 RID: 339 RVA: 0x0000A3D6 File Offset: 0x000085D6
		internal static void GetStructReplacement_Mono(Sandbox self, IntPtr ptr, IntPtr a, IntPtr b)
		{
			Sandbox.hasStructReturnBuffer_Mono = a == Sandbox.magicValue && b == Sandbox.magicValue;
		}

		// Token: 0x040000DE RID: 222
		internal static bool hasStructReturnBuffer_Net;

		// Token: 0x040000DF RID: 223
		internal static bool hasStructReturnBuffer_Mono;

		// Token: 0x040000E0 RID: 224
		internal static readonly IntPtr magicValue = (IntPtr)305419896;

		// Token: 0x02000041 RID: 65
		internal struct SomeStruct_Net
		{
			// Token: 0x040000E1 RID: 225
			private readonly byte b1;

			// Token: 0x040000E2 RID: 226
			private readonly byte b2;

			// Token: 0x040000E3 RID: 227
			private readonly byte b3;
		}

		// Token: 0x02000042 RID: 66
		internal struct SomeStruct_Mono
		{
			// Token: 0x040000E4 RID: 228
			private readonly byte b1;

			// Token: 0x040000E5 RID: 229
			private readonly byte b2;

			// Token: 0x040000E6 RID: 230
			private readonly byte b3;

			// Token: 0x040000E7 RID: 231
			private readonly byte b4;
		}
	}
}
