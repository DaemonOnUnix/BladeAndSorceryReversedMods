using System;

namespace MonoMod.RuntimeDetour
{
	// Token: 0x02000366 RID: 870
	internal interface IDetourNativePlatform
	{
		// Token: 0x0600149C RID: 5276
		NativeDetourData Create(IntPtr from, IntPtr to, byte? type = null);

		// Token: 0x0600149D RID: 5277
		void Free(NativeDetourData detour);

		// Token: 0x0600149E RID: 5278
		void Apply(NativeDetourData detour);

		// Token: 0x0600149F RID: 5279
		void Copy(IntPtr src, IntPtr dst, byte type);

		// Token: 0x060014A0 RID: 5280
		void MakeWritable(IntPtr src, uint size);

		// Token: 0x060014A1 RID: 5281
		void MakeExecutable(IntPtr src, uint size);

		// Token: 0x060014A2 RID: 5282
		void FlushICache(IntPtr src, uint size);

		// Token: 0x060014A3 RID: 5283
		IntPtr MemAlloc(uint size);

		// Token: 0x060014A4 RID: 5284
		void MemFree(IntPtr ptr);
	}
}
