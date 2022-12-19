using System;

namespace System.Speech.Recognition
{
	// Token: 0x02000058 RID: 88
	public class AudioSignalProblemOccurredEventArgs : EventArgs
	{
		// Token: 0x060001EF RID: 495 RVA: 0x000093BE File Offset: 0x000075BE
		internal AudioSignalProblemOccurredEventArgs(AudioSignalProblem audioSignalProblem, int audioLevel, TimeSpan audioPosition, TimeSpan recognizerPosition)
		{
			this._audioSignalProblem = audioSignalProblem;
			this._audioLevel = audioLevel;
			this._audioPosition = audioPosition;
			this._recognizerPosition = recognizerPosition;
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060001F0 RID: 496 RVA: 0x000093E3 File Offset: 0x000075E3
		public AudioSignalProblem AudioSignalProblem
		{
			get
			{
				return this._audioSignalProblem;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060001F1 RID: 497 RVA: 0x000093EB File Offset: 0x000075EB
		public int AudioLevel
		{
			get
			{
				return this._audioLevel;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x000093F3 File Offset: 0x000075F3
		public TimeSpan AudioPosition
		{
			get
			{
				return this._audioPosition;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060001F3 RID: 499 RVA: 0x000093FB File Offset: 0x000075FB
		public TimeSpan RecognizerAudioPosition
		{
			get
			{
				return this._recognizerPosition;
			}
		}

		// Token: 0x04000337 RID: 823
		private AudioSignalProblem _audioSignalProblem;

		// Token: 0x04000338 RID: 824
		private TimeSpan _recognizerPosition;

		// Token: 0x04000339 RID: 825
		private TimeSpan _audioPosition;

		// Token: 0x0400033A RID: 826
		private int _audioLevel;
	}
}
