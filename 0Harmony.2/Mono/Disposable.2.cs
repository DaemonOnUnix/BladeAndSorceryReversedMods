using System;

namespace Mono
{
	// Token: 0x020001A2 RID: 418
	internal struct Disposable<T> : IDisposable where T : class, IDisposable
	{
		// Token: 0x060006DC RID: 1756 RVA: 0x0001767C File Offset: 0x0001587C
		public Disposable(T value, bool owned)
		{
			this.value = value;
			this.owned = owned;
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x0001768C File Offset: 0x0001588C
		public void Dispose()
		{
			if (this.value != null && this.owned)
			{
				this.value.Dispose();
			}
		}

		// Token: 0x04000228 RID: 552
		internal readonly T value;

		// Token: 0x04000229 RID: 553
		private readonly bool owned;
	}
}
