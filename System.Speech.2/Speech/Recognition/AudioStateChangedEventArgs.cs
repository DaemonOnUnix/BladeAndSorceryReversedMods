using System;

namespace System.Speech.Recognition
{
	// Token: 0x0200005A RID: 90
	public class AudioStateChangedEventArgs : EventArgs
	{
		// Token: 0x060001F4 RID: 500 RVA: 0x00009403 File Offset: 0x00007603
		internal AudioStateChangedEventArgs(AudioState audioState)
		{
			this._audioState = audioState;
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060001F5 RID: 501 RVA: 0x00009412 File Offset: 0x00007612
		public AudioState AudioState
		{
			get
			{
				return this._audioState;
			}
		}

		// Token: 0x0400033F RID: 831
		private AudioState _audioState;
	}
}
