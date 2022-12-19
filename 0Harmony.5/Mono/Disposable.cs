using System;

namespace Mono
{
	// Token: 0x020001A1 RID: 417
	internal static class Disposable
	{
		// Token: 0x060006DA RID: 1754 RVA: 0x0001766A File Offset: 0x0001586A
		public static Disposable<T> Owned<T>(T value) where T : class, IDisposable
		{
			return new Disposable<T>(value, true);
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x00017673 File Offset: 0x00015873
		public static Disposable<T> NotOwned<T>(T value) where T : class, IDisposable
		{
			return new Disposable<T>(value, false);
		}
	}
}
