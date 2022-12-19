using System;
using System.ComponentModel;

namespace System.Speech.Synthesis
{
	// Token: 0x0200000D RID: 13
	public abstract class PromptEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x06000049 RID: 73 RVA: 0x00003BC5 File Offset: 0x00001DC5
		internal PromptEventArgs(Prompt prompt)
			: base(prompt._exception, prompt._exception != null, prompt)
		{
			this._prompt = prompt;
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600004A RID: 74 RVA: 0x00003BE4 File Offset: 0x00001DE4
		public Prompt Prompt
		{
			get
			{
				return this._prompt;
			}
		}

		// Token: 0x040001A5 RID: 421
		private Prompt _prompt;
	}
}
