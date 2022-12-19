using System;

namespace System.Speech.Recognition
{
	// Token: 0x02000056 RID: 86
	public class AudioLevelUpdatedEventArgs : EventArgs
	{
		// Token: 0x060001ED RID: 493 RVA: 0x000093A7 File Offset: 0x000075A7
		internal AudioLevelUpdatedEventArgs(int audioLevel)
		{
			this._audioLevel = audioLevel;
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060001EE RID: 494 RVA: 0x000093B6 File Offset: 0x000075B6
		public int AudioLevel
		{
			get
			{
				return this._audioLevel;
			}
		}

		// Token: 0x0400032E RID: 814
		private int _audioLevel;
	}
}
