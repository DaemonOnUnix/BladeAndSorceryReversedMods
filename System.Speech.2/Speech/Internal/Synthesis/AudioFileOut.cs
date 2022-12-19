using System;
using System.IO;
using System.Speech.AudioFormat;
using System.Threading;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000AD RID: 173
	internal class AudioFileOut : AudioBase, IDisposable
	{
		// Token: 0x060005C3 RID: 1475 RVA: 0x0001717C File Offset: 0x0001537C
		internal AudioFileOut(Stream stream, SpeechAudioFormatInfo formatInfo, bool headerInfo, IAsyncDispatch asyncDispatch)
		{
			this._asyncDispatch = asyncDispatch;
			this._stream = stream;
			this._startStreamPosition = this._stream.Position;
			this._hasHeader = headerInfo;
			this._wfxOut = default(WAVEFORMATEX);
			if (formatInfo != null)
			{
				this._wfxOut.wFormatTag = (short)formatInfo.EncodingFormat;
				this._wfxOut.wBitsPerSample = (short)formatInfo.BitsPerSample;
				this._wfxOut.nSamplesPerSec = formatInfo.SamplesPerSecond;
				this._wfxOut.nChannels = (short)formatInfo.ChannelCount;
			}
			else
			{
				this._wfxOut = WAVEFORMATEX.Default;
			}
			this._wfxOut.nBlockAlign = this._wfxOut.nChannels * this._wfxOut.wBitsPerSample / 8;
			this._wfxOut.nAvgBytesPerSec = (int)this._wfxOut.wBitsPerSample * this._wfxOut.nSamplesPerSec * (int)this._wfxOut.nChannels / 8;
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x00017290 File Offset: 0x00015490
		public void Dispose()
		{
			this._evt.Close();
			GC.SuppressFinalize(this);
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x000172A4 File Offset: 0x000154A4
		internal override void Begin(byte[] wfx)
		{
			if (this._deviceOpen)
			{
				throw new InvalidOperationException();
			}
			this._wfxIn = WAVEFORMATEX.ToWaveHeader(wfx);
			this._doConversion = this._pcmConverter.PrepareConverter(ref this._wfxIn, ref this._wfxOut);
			if (this._totalByteWrittens == 0 && this._hasHeader)
			{
				AudioBase.WriteWaveHeader(this._stream, this._wfxOut, this._startStreamPosition, 0);
			}
			this._bytesWritten = 0;
			this._aborted = false;
			this._deviceOpen = true;
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x00017328 File Offset: 0x00015528
		internal override void End()
		{
			if (!this._deviceOpen)
			{
				throw new InvalidOperationException();
			}
			this._deviceOpen = false;
			if (!this._aborted && this._hasHeader)
			{
				long position = this._stream.Position;
				AudioBase.WriteWaveHeader(this._stream, this._wfxOut, this._startStreamPosition, this._totalByteWrittens);
				this._stream.Seek(position, SeekOrigin.Begin);
			}
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x00017394 File Offset: 0x00015594
		internal override void Play(byte[] buffer)
		{
			if (this._deviceOpen)
			{
				byte[] array = (this._doConversion ? this._pcmConverter.ConvertSamples(buffer) : buffer);
				if (this._paused)
				{
					this._evt.WaitOne();
					this._evt.Reset();
				}
				if (!this._aborted)
				{
					this._stream.Write(array, 0, array.Length);
					this._totalByteWrittens += array.Length;
					this._bytesWritten += array.Length;
				}
			}
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x0001741C File Offset: 0x0001561C
		internal override void Pause()
		{
			if (!this._aborted && !this._paused)
			{
				object noWriteOutLock = this._noWriteOutLock;
				lock (noWriteOutLock)
				{
					this._paused = true;
				}
			}
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x00017470 File Offset: 0x00015670
		internal override void Resume()
		{
			if (!this._aborted && this._paused)
			{
				object noWriteOutLock = this._noWriteOutLock;
				lock (noWriteOutLock)
				{
					this._paused = false;
					this._evt.Set();
				}
			}
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x000174D0 File Offset: 0x000156D0
		internal override void Abort()
		{
			object noWriteOutLock = this._noWriteOutLock;
			lock (noWriteOutLock)
			{
				this._aborted = true;
				this._paused = false;
				this._evt.Set();
			}
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x00017524 File Offset: 0x00015724
		internal override void InjectEvent(TTSEvent ttsEvent)
		{
			if (!this._aborted && this._asyncDispatch != null)
			{
				this._asyncDispatch.Post(ttsEvent);
			}
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x00017544 File Offset: 0x00015744
		internal override void WaitUntilDone()
		{
			object noWriteOutLock = this._noWriteOutLock;
			lock (noWriteOutLock)
			{
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x060005CD RID: 1485 RVA: 0x00017580 File Offset: 0x00015780
		internal override TimeSpan Duration
		{
			get
			{
				if (this._wfxIn.nAvgBytesPerSec == 0)
				{
					return new TimeSpan(0L);
				}
				return new TimeSpan((long)this._bytesWritten * 10000000L / (long)this._wfxIn.nAvgBytesPerSec);
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x060005CE RID: 1486 RVA: 0x000175B7 File Offset: 0x000157B7
		internal override long Position
		{
			get
			{
				return this._stream.Position;
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x060005CF RID: 1487 RVA: 0x000175C4 File Offset: 0x000157C4
		internal override byte[] WaveFormat
		{
			get
			{
				return this._wfxOut.ToBytes();
			}
		}

		// Token: 0x04000484 RID: 1156
		protected ManualResetEvent _evt = new ManualResetEvent(false);

		// Token: 0x04000485 RID: 1157
		protected bool _deviceOpen;

		// Token: 0x04000486 RID: 1158
		protected Stream _stream;

		// Token: 0x04000487 RID: 1159
		protected PcmConverter _pcmConverter = new PcmConverter();

		// Token: 0x04000488 RID: 1160
		protected bool _doConversion;

		// Token: 0x04000489 RID: 1161
		protected bool _paused;

		// Token: 0x0400048A RID: 1162
		protected int _totalByteWrittens;

		// Token: 0x0400048B RID: 1163
		protected int _bytesWritten;

		// Token: 0x0400048C RID: 1164
		private IAsyncDispatch _asyncDispatch;

		// Token: 0x0400048D RID: 1165
		private object _noWriteOutLock = new object();

		// Token: 0x0400048E RID: 1166
		private WAVEFORMATEX _wfxIn;

		// Token: 0x0400048F RID: 1167
		private WAVEFORMATEX _wfxOut;

		// Token: 0x04000490 RID: 1168
		private bool _hasHeader;

		// Token: 0x04000491 RID: 1169
		private long _startStreamPosition;
	}
}
