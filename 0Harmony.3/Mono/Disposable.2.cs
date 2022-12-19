using System;

namespace Mono
{
	// Token: 0x020000B0 RID: 176
	internal struct Disposable<T> : IDisposable where T : class, IDisposable
	{
		// Token: 0x060003A6 RID: 934 RVA: 0x000117F0 File Offset: 0x0000F9F0
		public Disposable(T value, bool owned)
		{
			this.value = value;
			this.owned = owned;
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x00011800 File Offset: 0x0000FA00
		public void Dispose()
		{
			if (this.value != null && this.owned)
			{
				this.value.Dispose();
			}
		}

		// Token: 0x040001FA RID: 506
		internal readonly T value;

		// Token: 0x040001FB RID: 507
		private readonly bool owned;
	}
}
