using System;
using System.Collections.Generic;
using System.Speech.AudioFormat;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200002B RID: 43
	internal class EventNotify
	{
		// Token: 0x0600012D RID: 301 RVA: 0x00007E3B File Offset: 0x00006E3B
		internal EventNotify(ISpEventSource sapiEventSource, IAsyncDispatch dispatcher, bool additionalSapiFeatures)
		{
			this._sapiEventSourceReference = new WeakReference(sapiEventSource);
			this._dispatcher = dispatcher;
			this._additionalSapiFeatures = additionalSapiFeatures;
			this._notifySink = new SpNotifySink(this);
			sapiEventSource.SetNotifySink(this._notifySink);
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00007E78 File Offset: 0x00006E78
		internal void Dispose()
		{
			lock (this)
			{
				if (this._sapiEventSourceReference != null)
				{
					ISpEventSource spEventSource = (ISpEventSource)this._sapiEventSourceReference.Target;
					if (spEventSource != null)
					{
						spEventSource.SetNotifySink(null);
						this._notifySink = null;
					}
				}
				this._sapiEventSourceReference = null;
			}
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00007ED8 File Offset: 0x00006ED8
		internal void SendNotification(object ignored)
		{
			lock (this)
			{
				if (this._sapiEventSourceReference != null)
				{
					ISpEventSource spEventSource = (ISpEventSource)this._sapiEventSourceReference.Target;
					if (spEventSource != null)
					{
						List<SpeechEvent> list = new List<SpeechEvent>();
						SpeechEvent speechEvent;
						while ((speechEvent = SpeechEvent.TryCreateSpeechEvent(spEventSource, this._additionalSapiFeatures, this._audioFormat)) != null)
						{
							list.Add(speechEvent);
						}
						this._dispatcher.Post(list.ToArray());
					}
				}
			}
		}

		// Token: 0x1700002A RID: 42
		// (set) Token: 0x06000130 RID: 304 RVA: 0x00007F58 File Offset: 0x00006F58
		internal SpeechAudioFormatInfo AudioFormat
		{
			set
			{
				this._audioFormat = value;
			}
		}

		// Token: 0x040000D9 RID: 217
		private IAsyncDispatch _dispatcher;

		// Token: 0x040000DA RID: 218
		private WeakReference _sapiEventSourceReference;

		// Token: 0x040000DB RID: 219
		private bool _additionalSapiFeatures;

		// Token: 0x040000DC RID: 220
		private SpeechAudioFormatInfo _audioFormat;

		// Token: 0x040000DD RID: 221
		private ISpNotifySink _notifySink;
	}
}
