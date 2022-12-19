using System;
using System.IO;
using System.Speech.AudioFormat;
using System.Speech.Internal;

namespace System.Speech.Recognition
{
	// Token: 0x02000051 RID: 81
	[Serializable]
	public class RecognizedAudio
	{
		// Token: 0x060001A0 RID: 416 RVA: 0x000073F2 File Offset: 0x000055F2
		internal RecognizedAudio(byte[] rawAudioData, SpeechAudioFormatInfo audioFormat, DateTime startTime, TimeSpan audioPosition, TimeSpan audioDuration)
		{
			this._audioFormat = audioFormat;
			this._startTime = startTime;
			this._audioPosition = audioPosition;
			this._audioDuration = audioDuration;
			this._rawAudioData = rawAudioData;
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060001A1 RID: 417 RVA: 0x0000741F File Offset: 0x0000561F
		public SpeechAudioFormatInfo Format
		{
			get
			{
				return this._audioFormat;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x00007427 File Offset: 0x00005627
		public DateTime StartTime
		{
			get
			{
				return this._startTime;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060001A3 RID: 419 RVA: 0x0000742F File Offset: 0x0000562F
		public TimeSpan AudioPosition
		{
			get
			{
				return this._audioPosition;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060001A4 RID: 420 RVA: 0x00007437 File Offset: 0x00005637
		public TimeSpan Duration
		{
			get
			{
				return this._audioDuration;
			}
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00007440 File Offset: 0x00005640
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

		// Token: 0x060001A6 RID: 422 RVA: 0x000074A0 File Offset: 0x000056A0
		public void WriteToAudioStream(Stream outputStream)
		{
			Helpers.ThrowIfNull(outputStream, "outputStream");
			outputStream.Write(this._rawAudioData, 0, this._rawAudioData.Length);
			outputStream.Flush();
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x000074C8 File Offset: 0x000056C8
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

		// Token: 0x060001A8 RID: 424 RVA: 0x000075F4 File Offset: 0x000057F4
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
			if (array2.Length != 0)
			{
				sm.WriteStream(array2);
			}
			char[] array5 = new char[] { 'd', 'a', 't', 'a' };
			sm.WriteArray<char>(array5, array5.Length);
			sm.WriteStream(this._rawAudioData.Length);
		}

		// Token: 0x04000305 RID: 773
		private DateTime _startTime;

		// Token: 0x04000306 RID: 774
		private TimeSpan _audioPosition;

		// Token: 0x04000307 RID: 775
		private TimeSpan _audioDuration;

		// Token: 0x04000308 RID: 776
		private SpeechAudioFormatInfo _audioFormat;

		// Token: 0x04000309 RID: 777
		private byte[] _rawAudioData;
	}
}
