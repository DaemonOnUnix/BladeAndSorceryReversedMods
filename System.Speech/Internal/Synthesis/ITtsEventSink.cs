using System;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000E1 RID: 225
	internal interface ITtsEventSink
	{
		// Token: 0x0600051E RID: 1310
		void AddEvent(TTSEvent evt);

		// Token: 0x0600051F RID: 1311
		void FlushEvent();
	}
}
