using System;
using System.Collections;
using System.Speech.Synthesis.TtsEngine;
using System.Text;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000E4 RID: 228
	internal class PhonemeEventMapper : TtsEventMapper
	{
		// Token: 0x0600053C RID: 1340 RVA: 0x00016F6B File Offset: 0x00015F6B
		internal PhonemeEventMapper(ITtsEventSink sink, PhonemeEventMapper.PhonemeConversion conversion, AlphabetConverter alphabetConverter)
			: base(sink)
		{
			this._queue = new Queue();
			this._phonemeQueue = new Queue();
			this._conversion = conversion;
			this._alphabetConverter = alphabetConverter;
			this.Reset();
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x00016FA0 File Offset: 0x00015FA0
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
				this.Reset();
				return;
			}
			this.SendToQueue(evt);
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x00017074 File Offset: 0x00016074
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

		// Token: 0x0600053F RID: 1343 RVA: 0x000170C8 File Offset: 0x000160C8
		private void ConvertCompleteUnit()
		{
			if (this._lastComplete == 0)
			{
				return;
			}
			if (this._phonemeQueue.Count == 0)
			{
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

		// Token: 0x06000540 RID: 1344 RVA: 0x000171D6 File Offset: 0x000161D6
		private void Reset()
		{
			this._phonemeQueue.Clear();
			this._phonemes = new StringBuilder();
			this._lastComplete = 0;
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x000171F8 File Offset: 0x000161F8
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

		// Token: 0x04000412 RID: 1042
		private PhonemeEventMapper.PhonemeConversion _conversion;

		// Token: 0x04000413 RID: 1043
		private StringBuilder _phonemes;

		// Token: 0x04000414 RID: 1044
		private Queue _queue;

		// Token: 0x04000415 RID: 1045
		private Queue _phonemeQueue;

		// Token: 0x04000416 RID: 1046
		private AlphabetConverter _alphabetConverter;

		// Token: 0x04000417 RID: 1047
		private int _lastComplete;

		// Token: 0x020000E5 RID: 229
		public enum PhonemeConversion
		{
			// Token: 0x04000419 RID: 1049
			IpaToSapi,
			// Token: 0x0400041A RID: 1050
			SapiToIpa,
			// Token: 0x0400041B RID: 1051
			NoConversion
		}
	}
}
