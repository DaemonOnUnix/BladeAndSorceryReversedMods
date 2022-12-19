using System;

namespace System.Speech.Synthesis
{
	// Token: 0x0200001E RID: 30
	public class VoiceChangeEventArgs : PromptEventArgs
	{
		// Token: 0x060000AF RID: 175 RVA: 0x000048FA File Offset: 0x00002AFA
		internal VoiceChangeEventArgs(Prompt prompt, VoiceInfo voice)
			: base(prompt)
		{
			this._voice = voice;
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x0000490A File Offset: 0x00002B0A
		public VoiceInfo Voice
		{
			get
			{
				return this._voice;
			}
		}

		// Token: 0x04000203 RID: 515
		private VoiceInfo _voice;
	}
}
