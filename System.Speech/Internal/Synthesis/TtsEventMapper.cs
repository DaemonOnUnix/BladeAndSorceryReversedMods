using System;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000E3 RID: 227
	internal abstract class TtsEventMapper : ITtsEventSink
	{
		// Token: 0x06000538 RID: 1336 RVA: 0x00016F28 File Offset: 0x00015F28
		internal TtsEventMapper(ITtsEventSink sink)
		{
			this._sink = sink;
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x00016F37 File Offset: 0x00015F37
		protected virtual void SendToOutput(TTSEvent evt)
		{
			if (this._sink != null)
			{
				this._sink.AddEvent(evt);
			}
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x00016F4D File Offset: 0x00015F4D
		public virtual void AddEvent(TTSEvent evt)
		{
			this.SendToOutput(evt);
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x00016F56 File Offset: 0x00015F56
		public virtual void FlushEvent()
		{
			if (this._sink != null)
			{
				this._sink.FlushEvent();
			}
		}

		// Token: 0x04000411 RID: 1041
		private ITtsEventSink _sink;
	}
}
