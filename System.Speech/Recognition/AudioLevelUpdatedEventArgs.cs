using System;

namespace System.Speech.Recognition
{
	// Token: 0x02000186 RID: 390
	public class AudioLevelUpdatedEventArgs : EventArgs
	{
		// Token: 0x060009BD RID: 2493 RVA: 0x0002ACD6 File Offset: 0x00029CD6
		internal AudioLevelUpdatedEventArgs(int audioLevel)
		{
			this._audioLevel = audioLevel;
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x060009BE RID: 2494 RVA: 0x0002ACE5 File Offset: 0x00029CE5
		public int AudioLevel
		{
			get
			{
				return this._audioLevel;
			}
		}

		// Token: 0x040008ED RID: 2285
		private int _audioLevel;
	}
}
