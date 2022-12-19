using System;
using System.Speech.Internal;

namespace System.Speech.Recognition
{
	// Token: 0x0200010E RID: 270
	public class SpeechUI
	{
		// Token: 0x060006D8 RID: 1752 RVA: 0x0001FB1C File Offset: 0x0001EB1C
		internal SpeechUI()
		{
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x0001FB24 File Offset: 0x0001EB24
		public static bool SendTextFeedback(RecognitionResult result, string feedback, bool isSuccessfulAction)
		{
			Helpers.ThrowIfNull(result, "result");
			Helpers.ThrowIfEmptyOrNull(feedback, "feedback");
			return result.SetTextFeedback(feedback, isSuccessfulAction);
		}
	}
}
