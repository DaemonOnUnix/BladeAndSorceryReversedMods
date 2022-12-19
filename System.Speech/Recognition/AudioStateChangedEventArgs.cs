using System;

namespace System.Speech.Recognition
{
	// Token: 0x0200018A RID: 394
	public class AudioStateChangedEventArgs : EventArgs
	{
		// Token: 0x060009C4 RID: 2500 RVA: 0x0002AD32 File Offset: 0x00029D32
		internal AudioStateChangedEventArgs(AudioState audioState)
		{
			this._audioState = audioState;
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x060009C5 RID: 2501 RVA: 0x0002AD41 File Offset: 0x00029D41
		public AudioState AudioState
		{
			get
			{
				return this._audioState;
			}
		}

		// Token: 0x040008FE RID: 2302
		private AudioState _audioState;
	}
}
