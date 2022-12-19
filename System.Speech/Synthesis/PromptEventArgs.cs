using System;
using System.ComponentModel;

namespace System.Speech.Synthesis
{
	// Token: 0x02000138 RID: 312
	public abstract class PromptEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x06000856 RID: 2134 RVA: 0x00025974 File Offset: 0x00024974
		internal PromptEventArgs(Prompt prompt)
			: base(prompt._exception, prompt._exception != null, prompt)
		{
			this._prompt = prompt;
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000857 RID: 2135 RVA: 0x00025996 File Offset: 0x00024996
		public Prompt Prompt
		{
			get
			{
				return this._prompt;
			}
		}

		// Token: 0x040005E8 RID: 1512
		private Prompt _prompt;
	}
}
