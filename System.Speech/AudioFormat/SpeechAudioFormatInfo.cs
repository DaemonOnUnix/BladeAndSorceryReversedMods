using System;
using System.ComponentModel;
using System.Speech.Internal.Synthesis;

namespace System.Speech.AudioFormat
{
	// Token: 0x02000007 RID: 7
	[Serializable]
	public class SpeechAudioFormatInfo
	{
		// Token: 0x06000005 RID: 5 RVA: 0x000026B4 File Offset: 0x000016B4
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
			switch (encodingFormat)
			{
			case EncodingFormat.ALaw:
			case EncodingFormat.ULaw:
				if (bitsPerSample != 8)
				{
					throw new ArgumentOutOfRangeException("bitsPerSample");
				}
				if (formatSpecificData != null && formatSpecificData.Length != 0)
				{
					throw new ArgumentOutOfRangeException("formatSpecificData");
				}
				return;
			default:
				return;
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000027B4 File Offset: 0x000017B4
		[EditorBrowsable(2)]
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

		// Token: 0x06000007 RID: 7 RVA: 0x0000281B File Offset: 0x0000181B
		public SpeechAudioFormatInfo(int samplesPerSecond, AudioBitsPerSample bitsPerSample, AudioChannel channel)
			: this(EncodingFormat.Pcm, samplesPerSecond, (short)bitsPerSample, (short)channel, null)
		{
			this._blockAlign = this._channelCount * (this._bitsPerSample / 8);
			this._averageBytesPerSecond = this._samplesPerSecond * (int)this._blockAlign;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000008 RID: 8 RVA: 0x00002853 File Offset: 0x00001853
		[EditorBrowsable(2)]
		public int AverageBytesPerSecond
		{
			get
			{
				return this._averageBytesPerSecond;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000009 RID: 9 RVA: 0x0000285B File Offset: 0x0000185B
		[EditorBrowsable(2)]
		public int BitsPerSample
		{
			get
			{
				return (int)this._bitsPerSample;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000A RID: 10 RVA: 0x00002863 File Offset: 0x00001863
		[EditorBrowsable(2)]
		public int BlockAlign
		{
			get
			{
				return (int)this._blockAlign;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000B RID: 11 RVA: 0x0000286B File Offset: 0x0000186B
		public EncodingFormat EncodingFormat
		{
			get
			{
				return this._encodingFormat;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000C RID: 12 RVA: 0x00002873 File Offset: 0x00001873
		public int ChannelCount
		{
			get
			{
				return (int)this._channelCount;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000D RID: 13 RVA: 0x0000287B File Offset: 0x0000187B
		public int SamplesPerSecond
		{
			get
			{
				return this._samplesPerSecond;
			}
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002883 File Offset: 0x00001883
		public byte[] FormatSpecificData()
		{
			return (byte[])this._formatSpecificData.Clone();
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002898 File Offset: 0x00001898
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

		// Token: 0x06000010 RID: 16 RVA: 0x0000296B File Offset: 0x0000196B
		public override int GetHashCode()
		{
			return this._averageBytesPerSecond.GetHashCode();
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000011 RID: 17 RVA: 0x00002978 File Offset: 0x00001978
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

		// Token: 0x0400005B RID: 91
		private int _averageBytesPerSecond;

		// Token: 0x0400005C RID: 92
		private short _bitsPerSample;

		// Token: 0x0400005D RID: 93
		private short _blockAlign;

		// Token: 0x0400005E RID: 94
		private EncodingFormat _encodingFormat;

		// Token: 0x0400005F RID: 95
		private short _channelCount;

		// Token: 0x04000060 RID: 96
		private int _samplesPerSecond;

		// Token: 0x04000061 RID: 97
		private byte[] _formatSpecificData;
	}
}
