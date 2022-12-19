using System;

namespace System.Speech.Synthesis
{
	// Token: 0x0200001A RID: 26
	public class SpeakCompletedEventArgs : PromptEventArgs
	{
		// Token: 0x0600008E RID: 142 RVA: 0x00003BEC File Offset: 0x00001DEC
		internal SpeakCompletedEventArgs(Prompt prompt)
			: base(prompt)
		{
		}
	}
}
