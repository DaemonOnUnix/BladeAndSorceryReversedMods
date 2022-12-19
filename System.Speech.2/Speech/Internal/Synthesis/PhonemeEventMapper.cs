using System;
using System.Collections;
using System.Diagnostics;
using System.Speech.Synthesis.TtsEngine;
using System.Text;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000B5 RID: 181
	internal class PhonemeEventMapper : TtsEventMapper
	{
		// Token: 0x0600061C RID: 1564 RVA: 0x000182A8 File Offset: 0x000164A8
		internal PhonemeEventMapper(ITtsEventSink sink, PhonemeEventMapper.PhonemeConversion conversion, AlphabetConverter alphabetConverter)
			: base(sink)
		{
			this._queue = new Queue();
			this._phonemeQueue = new Queue();
			this._conversion = conversion;
			this._alphabetConverter = alphabetConverter;
			this.Reset();
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x000182DC File Offset: 0x000164DC
		public override void AddEvent(TTSEvent evt)
		{
			if (this._conversion == PhonemeEventMapper.PhonemeConversion.NoConversion)
			{
				this.SendToOutput(evt);
				return;
			}
			if (evt.Id == TtsEventId.Phoneme)
			{
				this._phonemeQueue.Enqueue(evt);
				int num = this._phonemes.Length + 1;
				this._phonemes.Append(evt.Phoneme);
				for (;;)
				{
					string text = this._phonemes.ToString(0, num);
					if (this._alphabetConverter.IsPrefix(text, this._conversion == PhonemeEventMapper.PhonemeConversion.SapiToIpa))
					{
						if (this._alphabetConverter.IsConvertibleUnit(text, this._conversion == PhonemeEventMapper.PhonemeConversion.SapiToIpa))
						{
							this._lastComplete = num;
						}
						num++;
					}
					else
					{
						if (this._lastComplete == 0)
						{
							break;
						}
						this.ConvertCompleteUnit();
						this._lastComplete = 0;
						num = 1;
					}
					if (num > this._phonemes.Length)
					{
						return;
					}
				}
				Trace.TraceError("Cannot convert the phonemes correctly. Attempt to start over...");
				this.Reset();
				return;
			}
			this.SendToQueue(evt);
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x000183BC File Offset: 0x000165BC
		public override void FlushEvent()
		{
			this.ConvertCompleteUnit();
			while (this._queue.Count > 0)
			{
				this.SendToOutput((TTSEvent)this._queue.Dequeue());
			}
			this._phonemeQueue.Clear();
			this._lastComplete = 0;
			base.FlushEvent();
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x00018410 File Offset: 0x00016610
		private void ConvertCompleteUnit()
		{
			if (this._lastComplete == 0)
			{
				return;
			}
			if (this._phonemeQueue.Count == 0)
			{
				Trace.TraceError("Failed to convert phonemes. Phoneme queue is empty.");
				return;
			}
			char[] array = new char[this._lastComplete];
			this._phonemes.CopyTo(0, array, 0, this._lastComplete);
			this._phonemes.Remove(0, this._lastComplete);
			char[] array2;
			if (this._conversion == PhonemeEventMapper.PhonemeConversion.IpaToSapi)
			{
				array2 = this._alphabetConverter.IpaToSapi(array);
			}
			else
			{
				array2 = this._alphabetConverter.SapiToIpa(array);
			}
			long num = 0L;
			TTSEvent ttsevent = (TTSEvent)this._phonemeQueue.Peek();
			TTSEvent ttsevent2;
			for (int i = 0; i < this._lastComplete; i += ttsevent2.Phoneme.Length)
			{
				ttsevent2 = (TTSEvent)this._phonemeQueue.Dequeue();
				num += (long)ttsevent2.PhonemeDuration.Milliseconds;
			}
			TTSEvent ttsevent3 = TTSEvent.CreatePhonemeEvent(new string(array2), "", TimeSpan.FromMilliseconds((double)num), ttsevent.PhonemeEmphasis, ttsevent.Prompt, ttsevent.AudioPosition);
			this.SendToQueue(ttsevent3);
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x00018528 File Offset: 0x00016728
		private void Reset()
		{
			this._phonemeQueue.Clear();
			this._phonemes = new StringBuilder();
			this._lastComplete = 0;
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x00018548 File Offset: 0x00016748
		private void SendToQueue(TTSEvent evt)
		{
			if (evt.Id == TtsEventId.Phoneme)
			{
				if (this._queue.Count > 0)
				{
					TTSEvent ttsevent = this._queue.Dequeue() as TTSEvent;
					if (ttsevent.Id == TtsEventId.Phoneme)
					{
						ttsevent.NextPhoneme = evt.Phoneme;
					}
					else
					{
						Trace.TraceError("First event in the queue of the phone mapper is not a PHONEME event");
					}
					this.SendToOutput(ttsevent);
					while (this._queue.Count > 0)
					{
						this.SendToOutput(this._queue.Dequeue() as TTSEvent);
					}
				}
				this._queue.Enqueue(evt);
				return;
			}
			if (this._queue.Count > 0)
			{
				this._queue.Enqueue(evt);
				return;
			}
			this.SendToOutput(evt);
		}

		// Token: 0x040004A5 RID: 1189
		private PhonemeEventMapper.PhonemeConversion _conversion;

		// Token: 0x040004A6 RID: 1190
		private StringBuilder _phonemes;

		// Token: 0x040004A7 RID: 1191
		private Queue _queue;

		// Token: 0x040004A8 RID: 1192
		private Queue _phonemeQueue;

		// Token: 0x040004A9 RID: 1193
		private AlphabetConverter _alphabetConverter;

		// Token: 0x040004AA RID: 1194
		private int _lastComplete;

		// Token: 0x02000194 RID: 404
		public enum PhonemeConversion
		{
			// Token: 0x04000937 RID: 2359
			IpaToSapi,
			// Token: 0x04000938 RID: 2360
			SapiToIpa,
			// Token: 0x04000939 RID: 2361
			NoConversion
		}
	}
}
