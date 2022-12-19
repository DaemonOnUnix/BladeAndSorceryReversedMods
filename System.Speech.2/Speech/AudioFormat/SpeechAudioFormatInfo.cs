using System;
using System.ComponentModel;
using System.Speech.Internal.Synthesis;

namespace System.Speech.AudioFormat
{
	// Token: 0x02000088 RID: 136
	[Serializable]
	public class SpeechAudioFormatInfo
	{
		// Token: 0x06000489 RID: 1161 RVA: 0x00012090 File Offset: 0x00010290
		private SpeechAudioFormatInfo(EncodingFormat encodingFormat, int samplesPerSecond, short bitsPerSample, short channelCount, byte[] formatSpecificData)
		{
			if (encodingFormat == (EncodingFormat)0)
			{
				throw new ArgumentException(SR.Get(SRID.CannotUseCustomFormat, new object[0]), "encodingFormat");
			}
			if (samplesPerSecond <= 0)
			{
				throw new ArgumentOutOfRangeException("samplesPerSecond", SR.Get(SRID.MustBeGreaterThanZero, new object[0]));
			}
			if (bitsPerSample <= 0)
			{
				throw new ArgumentOutOfRangeException("bitsPerSample", SR.Get(SRID.MustBeGreaterThanZero, new object[0]));
			}
			if (channelCount <= 0)
			{
				throw new ArgumentOutOfRangeException("channelCount", SR.Get(SRID.MustBeGreaterThanZero, new object[0]));
			}
			this._encodingFormat = encodingFormat;
			this._samplesPerSecond = samplesPerSecond;
			this._bitsPerSample = bitsPerSample;
			this._channelCount = channelCount;
			if (formatSpecificData == null)
			{
				this._formatSpecificData = new byte[0];
			}
			else
			{
				this._formatSpecificData = (byte[])formatSpecificData.Clone();
			}
			if (encodingFormat - EncodingFormat.ALaw <= 1)
			{
				if (bitsPerSample != 8)
				{
					throw new ArgumentOutOfRangeException("bitsPerSample");
				}
				if (formatSpecificData != null && formatSpecificData.Length != 0)
				{
					throw new ArgumentOutOfRangeException("formatSpecificData");
				}
			}
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x00012180 File Offset: 0x00010380
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public SpeechAudioFormatInfo(EncodingFormat encodingFormat, int samplesPerSecond, int bitsPerSample, int channelCount, int averageBytesPerSecond, int blockAlign, byte[] formatSpecificData)
			: this(encodingFormat, samplesPerSecond, (short)bitsPerSample, (short)channelCount, formatSpecificData)
		{
			if (averageBytesPerSecond <= 0)
			{
				throw new ArgumentOutOfRangeException("averageBytesPerSecond", SR.Get(SRID.MustBeGreaterThanZero, new object[0]));
			}
			if (blockAlign <= 0)
			{
				throw new ArgumentOutOfRangeException("blockAlign", SR.Get(SRID.MustBeGreaterThanZero, new object[0]));
			}
			this._averageBytesPerSecond = averageBytesPerSecond;
			this._blockAlign = (short)blockAlign;
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x000121E7 File Offset: 0x000103E7
		public SpeechAudioFormatInfo(int samplesPerSecond, AudioBitsPerSample bitsPerSample, AudioChannel channel)
			: this(EncodingFormat.Pcm, samplesPerSecond, (short)bitsPerSample, (short)channel, null)
		{
			this._blockAlign = this._channelCount * (this._bitsPerSample / 8);
			this._averageBytesPerSecond = this._samplesPerSecond * (int)this._blockAlign;
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x0600048C RID: 1164 RVA: 0x0001221F File Offset: 0x0001041F
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public int AverageBytesPerSecond
		{
			get
			{
				return this._averageBytesPerSecond;
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x0600048D RID: 1165 RVA: 0x00012227 File Offset: 0x00010427
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public int BitsPerSample
		{
			get
			{
				return (int)this._bitsPerSample;
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x0600048E RID: 1166 RVA: 0x0001222F File Offset: 0x0001042F
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public int BlockAlign
		{
			get
			{
				return (int)this._blockAlign;
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x0600048F RID: 1167 RVA: 0x00012237 File Offset: 0x00010437
		public EncodingFormat EncodingFormat
		{
			get
			{
				return this._encodingFormat;
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000490 RID: 1168 RVA: 0x0001223F File Offset: 0x0001043F
		public int ChannelCount
		{
			get
			{
				return (int)this._channelCount;
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000491 RID: 1169 RVA: 0x00012247 File Offset: 0x00010447
		public int SamplesPerSecond
		{
			get
			{
				return this._samplesPerSecond;
			}
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x0001224F File Offset: 0x0001044F
		public byte[] FormatSpecificData()
		{
			return (byte[])this._formatSpecificData.Clone();
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x00012264 File Offset: 0x00010464
		public override bool Equals(object obj)
		{
			SpeechAudioFormatInfo speechAudioFormatInfo = obj as SpeechAudioFormatInfo;
			if (speechAudioFormatInfo == null)
			{
				return false;
			}
			if (!this._averageBytesPerSecond.Equals(speechAudioFormatInfo._averageBytesPerSecond) || !this._bitsPerSample.Equals(speechAudioFormatInfo._bitsPerSample) || !this._blockAlign.Equals(speechAudioFormatInfo._blockAlign) || !this._encodingFormat.Equals(speechAudioFormatInfo._encodingFormat) || !this._channelCount.Equals(speechAudioFormatInfo._channelCount) || !this._samplesPerSecond.Equals(speechAudioFormatInfo._samplesPerSecond))
			{
				return false;
			}
			if (this._formatSpecificData.Length != speechAudioFormatInfo._formatSpecificData.Length)
			{
				return false;
			}
			for (int i = 0; i < this._formatSpecificData.Length; i++)
			{
				if (this._formatSpecificData[i] != speechAudioFormatInfo._formatSpecificData[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x00012338 File Offset: 0x00010538
		public override int GetHashCode()
		{
			return this._averageBytesPerSecond.GetHashCode();
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000495 RID: 1173 RVA: 0x00012348 File Offset: 0x00010548
		internal byte[] WaveFormat
		{
			get
			{
				WAVEFORMATEX waveformatex = default(WAVEFORMATEX);
				waveformatex.wFormatTag = (short)this.EncodingFormat;
				waveformatex.nChannels = (short)this.ChannelCount;
				waveformatex.nSamplesPerSec = this.SamplesPerSecond;
				waveformatex.nAvgBytesPerSec = this.AverageBytesPerSecond;
				waveformatex.nBlockAlign = (short)this.BlockAlign;
				waveformatex.wBitsPerSample = (short)this.BitsPerSample;
				waveformatex.cbSize = (short)this.FormatSpecificData().Length;
				byte[] array = waveformatex.ToBytes();
				if (waveformatex.cbSize > 0)
				{
					byte[] array2 = new byte[array.Length + (int)waveformatex.cbSize];
					Array.Copy(array, array2, array.Length);
					Array.Copy(this.FormatSpecificData(), 0, array2, array.Length, (int)waveformatex.cbSize);
					array = array2;
				}
				return array;
			}
		}

		// Token: 0x04000413 RID: 1043
		private int _averageBytesPerSecond;

		// Token: 0x04000414 RID: 1044
		private short _bitsPerSample;

		// Token: 0x04000415 RID: 1045
		private short _blockAlign;

		// Token: 0x04000416 RID: 1046
		private EncodingFormat _encodingFormat;

		// Token: 0x04000417 RID: 1047
		private short _channelCount;

		// Token: 0x04000418 RID: 1048
		private int _samplesPerSecond;

		// Token: 0x04000419 RID: 1049
		private byte[] _formatSpecificData;
	}
}
