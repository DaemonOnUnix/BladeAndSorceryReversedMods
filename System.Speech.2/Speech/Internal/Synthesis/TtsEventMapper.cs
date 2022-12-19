using System;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000B4 RID: 180
	internal abstract class TtsEventMapper : ITtsEventSink
	{
		// Token: 0x06000618 RID: 1560 RVA: 0x00018265 File Offset: 0x00016465
		internal TtsEventMapper(ITtsEventSink sink)
		{
			this._sink = sink;
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x00018274 File Offset: 0x00016474
		protected virtual void SendToOutput(TTSEvent evt)
		{
			if (this._sink != null)
			{
				this._sink.AddEvent(evt);
			}
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x0001828A File Offset: 0x0001648A
		public virtual void AddEvent(TTSEvent evt)
		{
			this.SendToOutput(evt);
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x00018293 File Offset: 0x00016493
		public virtual void FlushEvent()
		{
			if (this._sink != null)
			{
				this._sink.FlushEvent();
			}
		}

		// Token: 0x040004A4 RID: 1188
		private ITtsEventSink _sink;
	}
}
