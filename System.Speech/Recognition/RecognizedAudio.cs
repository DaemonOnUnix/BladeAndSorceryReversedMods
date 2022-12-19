using System;
using System.IO;
using System.Speech.AudioFormat;
using System.Speech.Internal;

namespace System.Speech.Recognition
{
	// Token: 0x02000133 RID: 307
	[Serializable]
	public class RecognizedAudio
	{
		// Token: 0x06000826 RID: 2086 RVA: 0x000251F2 File Offset: 0x000241F2
		internal RecognizedAudio(byte[] rawAudioData, SpeechAudioFormatInfo audioFormat, DateTime startTime, TimeSpan audioPosition, TimeSpan audioDuration)
		{
			this._audioFormat = audioFormat;
			this._startTime = startTime;
			this._audioPosition = audioPosition;
			this._audioDuration = audioDuration;
			this._rawAudioData = rawAudioData;
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000827 RID: 2087 RVA: 0x0002521F File Offset: 0x0002421F
		public SpeechAudioFormatInfo Format
		{
			get
			{
				return this._audioFormat;
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000828 RID: 2088 RVA: 0x00025227 File Offset: 0x00024227
		public DateTime StartTime
		{
			get
			{
				return this._startTime;
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000829 RID: 2089 RVA: 0x0002522F File Offset: 0x0002422F
		public TimeSpan AudioPosition
		{
			get
			{
				return this._audioPosition;
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x0600082A RID: 2090 RVA: 0x00025237 File Offset: 0x00024237
		public TimeSpan Duration
		{
			get
			{
				return this._audioDuration;
			}
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x00025240 File Offset: 0x00024240
		public void WriteToWaveStream(Stream outputStream)
		{
			Helpers.ThrowIfNull(outputStream, "outputStream");
			using (StreamMarshaler streamMarshaler = new StreamMarshaler(outputStream))
			{
				this.WriteWaveHeader(streamMarshaler);
			}
			outputStream.Write(this._rawAudioData, 0, this._rawAudioData.Length);
			outputStream.Flush();
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x000252A0 File Offset: 0x000242A0
		public void WriteToAudioStream(Stream outputStream)
		{
			Helpers.ThrowIfNull(outputStream, "outputStream");
			outputStream.Write(this._rawAudioData, 0, this._rawAudioData.Length);
			outputStream.Flush();
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x000252C8 File Offset: 0x000242C8
		public RecognizedAudio GetRange(TimeSpan audioPosition, TimeSpan duration)
		{
			if (audioPosition.Ticks < 0L)
			{
				throw new ArgumentOutOfRangeException("audioPosition", SR.Get(SRID.NegativeTimesNotSupported, new object[0]));
			}
			if (duration.Ticks < 0L)
			{
				throw new ArgumentOutOfRangeException("duration", SR.Get(SRID.NegativeTimesNotSupported, new object[0]));
			}
			if (audioPosition > this._audioDuration)
			{
				throw new ArgumentOutOfRangeException("audioPosition");
			}
			if (duration > audioPosition + this._audioDuration)
			{
				throw new ArgumentOutOfRangeException("duration");
			}
			int num = (int)((long)(this._audioFormat.BitsPerSample * this._audioFormat.SamplesPerSecond) * audioPosition.Ticks / 80000000L);
			int num2 = (int)((long)(this._audioFormat.BitsPerSample * this._audioFormat.SamplesPerSecond) * duration.Ticks / 80000000L);
			if (num + num2 > this._rawAudioData.Length)
			{
				num2 = this._rawAudioData.Length - num;
			}
			byte[] array = new byte[num2];
			Array.Copy(this._rawAudioData, num, array, 0, num2);
			return new RecognizedAudio(array, this._audioFormat, this._startTime + audioPosition, audioPosition, duration);
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x00025418 File Offset: 0x00024418
		private void WriteWaveHeader(StreamMarshaler sm)
		{
			char[] array = new char[] { 'R', 'I', 'F', 'F' };
			byte[] array2 = this._audioFormat.FormatSpecificData();
			sm.WriteArray<char>(array, array.Length);
			sm.WriteStream((uint)(this._rawAudioData.Length + 38 + array2.Length));
			char[] array3 = new char[] { 'W', 'A', 'V', 'E' };
			sm.WriteArray<char>(array3, array3.Length);
			char[] array4 = new char[] { 'f', 'm', 't', ' ' };
			sm.WriteArray<char>(array4, array4.Length);
			sm.WriteStream(18 + array2.Length);
			sm.WriteStream((ushort)this._audioFormat.EncodingFormat);
			sm.WriteStream((ushort)this._audioFormat.ChannelCount);
			sm.WriteStream(this._audioFormat.SamplesPerSecond);
			sm.WriteStream(this._audioFormat.AverageBytesPerSecond);
			sm.WriteStream((ushort)this._audioFormat.BlockAlign);
			sm.WriteStream((ushort)this._audioFormat.BitsPerSample);
			sm.WriteStream((ushort)array2.Length);
			if (array2.Length > 0)
			{
				sm.WriteStream(array2);
			}
			char[] array5 = new char[] { 'd', 'a', 't', 'a' };
			sm.WriteArray<char>(array5, array5.Length);
			sm.WriteStream(this._rawAudioData.Length);
		}

		// Token: 0x040005CD RID: 1485
		private DateTime _startTime;

		// Token: 0x040005CE RID: 1486
		private TimeSpan _audioPosition;

		// Token: 0x040005CF RID: 1487
		private TimeSpan _audioDuration;

		// Token: 0x040005D0 RID: 1488
		private SpeechAudioFormatInfo _audioFormat;

		// Token: 0x040005D1 RID: 1489
		private byte[] _rawAudioData;
	}
}
