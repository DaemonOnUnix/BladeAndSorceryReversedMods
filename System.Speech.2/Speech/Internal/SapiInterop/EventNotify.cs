using System;
using System.Collections.Generic;
using System.Speech.AudioFormat;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000105 RID: 261
	internal class EventNotify
	{
		// Token: 0x06000927 RID: 2343 RVA: 0x0002AB23 File Offset: 0x00028D23
		internal EventNotify(ISpEventSource sapiEventSource, IAsyncDispatch dispatcher, bool additionalSapiFeatures)
		{
			this._sapiEventSourceReference = new WeakReference(sapiEventSource);
			this._dispatcher = dispatcher;
			this._additionalSapiFeatures = additionalSapiFeatures;
			this._notifySink = new SpNotifySink(this);
			sapiEventSource.SetNotifySink(this._notifySink);
		}

		// Token: 0x06000928 RID: 2344 RVA: 0x0002AB60 File Offset: 0x00028D60
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

		// Token: 0x06000929 RID: 2345 RVA: 0x0002ABC8 File Offset: 0x00028DC8
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

		// Token: 0x170001E3 RID: 483
		// (set) Token: 0x0600092A RID: 2346 RVA: 0x0002AC54 File Offset: 0x00028E54
		internal SpeechAudioFormatInfo AudioFormat
		{
			set
			{
				this._audioFormat = value;
			}
		}

		// Token: 0x04000655 RID: 1621
		private IAsyncDispatch _dispatcher;

		// Token: 0x04000656 RID: 1622
		private WeakReference _sapiEventSourceReference;

		// Token: 0x04000657 RID: 1623
		private bool _additionalSapiFeatures;

		// Token: 0x04000658 RID: 1624
		private SpeechAudioFormatInfo _audioFormat;

		// Token: 0x04000659 RID: 1625
		private ISpNotifySink _notifySink;
	}
}
