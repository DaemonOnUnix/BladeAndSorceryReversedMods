using System;

namespace Mono
{
	// Token: 0x020000AF RID: 175
	internal static class Disposable
	{
		// Token: 0x060003A4 RID: 932 RVA: 0x000117DE File Offset: 0x0000F9DE
		public static Disposable<T> Owned<T>(T value) where T : class, IDisposable
		{
			return new Disposable<T>(value, true);
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x000117E7 File Offset: 0x0000F9E7
		public static Disposable<T> NotOwned<T>(T value) where T : class, IDisposable
		{
			return new Disposable<T>(value, false);
		}
	}
}
