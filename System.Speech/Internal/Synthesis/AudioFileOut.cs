using System;
using System.IO;
using System.Speech.AudioFormat;
using System.Threading;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000DB RID: 219
	internal class AudioFileOut : AudioBase, IDisposable
	{
		// Token: 0x060004DA RID: 1242 RVA: 0x00015E00 File Offset: 0x00014E00
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

		// Token: 0x060004DB RID: 1243 RVA: 0x00015F14 File Offset: 0x00014F14
		public void Dispose()
		{
			this._evt.Close();
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x00015F24 File Offset: 0x00014F24
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

		// Token: 0x060004DD RID: 1245 RVA: 0x00015FA8 File Offset: 0x00014FA8
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
				this._stream.Seek(position, 0);
			}
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x00016014 File Offset: 0x00015014
		internal override void Play(byte[] buffer)
		{
			if (!this._deviceOpen)
			{
				return;
			}
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

		// Token: 0x060004DF RID: 1247 RVA: 0x0001609C File Offset: 0x0001509C
		internal override void Pause()
		{
			if (!this._aborted && !this._paused)
			{
				lock (this._noWriteOutLock)
				{
					this._paused = true;
				}
			}
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x000160E8 File Offset: 0x000150E8
		internal override void Resume()
		{
			if (!this._aborted && this._paused)
			{
				lock (this._noWriteOutLock)
				{
					this._paused = false;
					this._evt.Set();
				}
			}
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x00016140 File Offset: 0x00015140
		internal override void Abort()
		{
			lock (this._noWriteOutLock)
			{
				this._aborted = true;
				this._paused = false;
				this._evt.Set();
			}
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x00016190 File Offset: 0x00015190
		internal override void InjectEvent(TTSEvent ttsEvent)
		{
			if (!this._aborted && this._asyncDispatch != null)
			{
				this._asyncDispatch.Post(ttsEvent);
			}
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x000161B0 File Offset: 0x000151B0
		internal override void WaitUntilDone()
		{
			lock (this._noWriteOutLock)
			{
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060004E4 RID: 1252 RVA: 0x000161E4 File Offset: 0x000151E4
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

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060004E5 RID: 1253 RVA: 0x0001621B File Offset: 0x0001521B
		internal override long Position
		{
			get
			{
				return this._stream.Position;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060004E6 RID: 1254 RVA: 0x00016228 File Offset: 0x00015228
		internal override byte[] WaveFormat
		{
			get
			{
				return this._wfxOut.ToBytes();
			}
		}

		// Token: 0x040003F1 RID: 1009
		protected ManualResetEvent _evt = new ManualResetEvent(false);

		// Token: 0x040003F2 RID: 1010
		protected bool _deviceOpen;

		// Token: 0x040003F3 RID: 1011
		protected Stream _stream;

		// Token: 0x040003F4 RID: 1012
		protected PcmConverter _pcmConverter = new PcmConverter();

		// Token: 0x040003F5 RID: 1013
		protected bool _doConversion;

		// Token: 0x040003F6 RID: 1014
		protected bool _paused;

		// Token: 0x040003F7 RID: 1015
		protected int _totalByteWrittens;

		// Token: 0x040003F8 RID: 1016
		protected int _bytesWritten;

		// Token: 0x040003F9 RID: 1017
		private IAsyncDispatch _asyncDispatch;

		// Token: 0x040003FA RID: 1018
		private object _noWriteOutLock = new object();

		// Token: 0x040003FB RID: 1019
		private WAVEFORMATEX _wfxIn;

		// Token: 0x040003FC RID: 1020
		private WAVEFORMATEX _wfxOut;

		// Token: 0x040003FD RID: 1021
		private bool _hasHeader;

		// Token: 0x040003FE RID: 1022
		private long _startStreamPosition;
	}
}
