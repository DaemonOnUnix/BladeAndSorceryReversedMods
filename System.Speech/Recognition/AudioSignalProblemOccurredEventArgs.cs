using System;

namespace System.Speech.Recognition
{
	// Token: 0x02000188 RID: 392
	public class AudioSignalProblemOccurredEventArgs : EventArgs
	{
		// Token: 0x060009BF RID: 2495 RVA: 0x0002ACED File Offset: 0x00029CED
		internal AudioSignalProblemOccurredEventArgs(AudioSignalProblem audioSignalProblem, int audioLevel, TimeSpan audioPosition, TimeSpan recognizerPosition)
		{
			this._audioSignalProblem = audioSignalProblem;
			this._audioLevel = audioLevel;
			this._audioPosition = audioPosition;
			this._recognizerPosition = recognizerPosition;
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x060009C0 RID: 2496 RVA: 0x0002AD12 File Offset: 0x00029D12
		public AudioSignalProblem AudioSignalProblem
		{
			get
			{
				return this._audioSignalProblem;
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x060009C1 RID: 2497 RVA: 0x0002AD1A File Offset: 0x00029D1A
		public int AudioLevel
		{
			get
			{
				return this._audioLevel;
			}
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x060009C2 RID: 2498 RVA: 0x0002AD22 File Offset: 0x00029D22
		public TimeSpan AudioPosition
		{
			get
			{
				return this._audioPosition;
			}
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x060009C3 RID: 2499 RVA: 0x0002AD2A File Offset: 0x00029D2A
		public TimeSpan RecognizerAudioPosition
		{
			get
			{
				return this._recognizerPosition;
			}
		}

		// Token: 0x040008F6 RID: 2294
		private AudioSignalProblem _audioSignalProblem;

		// Token: 0x040008F7 RID: 2295
		private TimeSpan _recognizerPosition;

		// Token: 0x040008F8 RID: 2296
		private TimeSpan _audioPosition;

		// Token: 0x040008F9 RID: 2297
		private int _audioLevel;
	}
}
