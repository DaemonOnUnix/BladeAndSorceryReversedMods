using System;

namespace MonoMod.Utils
{
	// Token: 0x02000445 RID: 1093
	internal static class GCListener
	{
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06001749 RID: 5961 RVA: 0x00050314 File Offset: 0x0004E514
		// (remove) Token: 0x0600174A RID: 5962 RVA: 0x00050348 File Offset: 0x0004E548
		public static event Action OnCollect;

		// Token: 0x0600174B RID: 5963 RVA: 0x0005037B File Offset: 0x0004E57B
		static GCListener()
		{
			new GCListener.CollectionDummy();
		}

		// Token: 0x0400100B RID: 4107
		private static bool Unloading;

		// Token: 0x02000446 RID: 1094
		private sealed class CollectionDummy
		{
			// Token: 0x0600174C RID: 5964 RVA: 0x00050384 File Offset: 0x0004E584
			protected override void Finalize()
			{
				try
				{
					GCListener.Unloading |= AppDomain.CurrentDomain.IsFinalizingForUnload() || Environment.HasShutdownStarted;
					if (!GCListener.Unloading)
					{
						GC.ReRegisterForFinalize(this);
					}
					Action onCollect = GCListener.OnCollect;
					if (onCollect != null)
					{
						onCollect();
					}
				}
				finally
				{
					base.Finalize();
				}
			}
		}
	}
}
