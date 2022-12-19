using System;

namespace System.Speech.Synthesis
{
	// Token: 0x02000156 RID: 342
	public class VoiceChangeEventArgs : PromptEventArgs
	{
		// Token: 0x06000905 RID: 2309 RVA: 0x0002813D File Offset: 0x0002713D
		internal VoiceChangeEventArgs(Prompt prompt, VoiceInfo voice)
			: base(prompt)
		{
			this._voice = voice;
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000906 RID: 2310 RVA: 0x0002814D File Offset: 0x0002714D
		public VoiceInfo Voice
		{
			get
			{
				return this._voice;
			}
		}

		// Token: 0x04000687 RID: 1671
		private VoiceInfo _voice;
	}
}
