using System;
using System.Speech.Internal;

namespace System.Speech.Recognition
{
	// Token: 0x02000049 RID: 73
	public class SpeechUI
	{
		// Token: 0x06000183 RID: 387 RVA: 0x00003BF5 File Offset: 0x00001DF5
		internal SpeechUI()
		{
		}

		// Token: 0x06000184 RID: 388 RVA: 0x000067C4 File Offset: 0x000049C4
		public static bool SendTextFeedback(RecognitionResult result, string feedback, bool isSuccessfulAction)
		{
			Helpers.ThrowIfNull(result, "result");
			Helpers.ThrowIfEmptyOrNull(feedback, "feedback");
			return result.SetTextFeedback(feedback, isSuccessfulAction);
		}
	}
}
