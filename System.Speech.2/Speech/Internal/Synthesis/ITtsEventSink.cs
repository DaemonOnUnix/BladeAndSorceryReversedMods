using System;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000B3 RID: 179
	internal interface ITtsEventSink
	{
		// Token: 0x06000616 RID: 1558
		void AddEvent(TTSEvent evt);

		// Token: 0x06000617 RID: 1559
		void FlushEvent();
	}
}
