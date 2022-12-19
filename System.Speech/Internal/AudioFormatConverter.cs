using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Speech.AudioFormat;

namespace System.Speech.Internal
{
	// Token: 0x02000002 RID: 2
	internal static class AudioFormatConverter
	{
		// Token: 0x06000001 RID: 1 RVA: 0x000020D0 File Offset: 0x000010D0
		internal static SpeechAudioFormatInfo ToSpeechAudioFormatInfo(IntPtr waveFormatPtr)
		{
			AudioFormatConverter.WaveFormatEx waveFormatEx = (AudioFormatConverter.WaveFormatEx)Marshal.PtrToStructure(waveFormatPtr, typeof(AudioFormatConverter.WaveFormatEx));
			byte[] array = new byte[(int)waveFormatEx.cbSize];
			IntPtr intPtr;
			intPtr..ctor(waveFormatPtr.ToInt64() + (long)Marshal.SizeOf(waveFormatEx));
			for (int i = 0; i < (int)waveFormatEx.cbSize; i++)
			{
				array[i] = Marshal.ReadByte(intPtr, i);
			}
			return new SpeechAudioFormatInfo((EncodingFormat)waveFormatEx.wFormatTag, (int)waveFormatEx.nSamplesPerSec, (int)((short)waveFormatEx.wBitsPerSample), (int)((short)waveFormatEx.nChannels), (int)waveFormatEx.nAvgBytesPerSec, (int)((short)waveFormatEx.nBlockAlign), array);
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002160 File Offset: 0x00001160
		internal static SpeechAudioFormatInfo ToSpeechAudioFormatInfo(string formatString)
		{
			short num;
			if (short.TryParse(formatString, 0, CultureInfo.InvariantCulture, ref num))
			{
				return AudioFormatConverter.ConvertFormat((AudioFormatConverter.StreamFormat)num);
			}
			return null;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002290 File Offset: 0x00001290
		private static SpeechAudioFormatInfo ConvertFormat(AudioFormatConverter.StreamFormat eFormat)
		{
			AudioFormatConverter.WaveFormatEx waveFormatEx = new AudioFormatConverter.WaveFormatEx();
			byte[] array = null;
			if (eFormat >= AudioFormatConverter.StreamFormat.PCM_8kHz8BitMono && eFormat <= AudioFormatConverter.StreamFormat.PCM_48kHz16BitStereo)
			{
				uint num = (uint)(eFormat - AudioFormatConverter.StreamFormat.PCM_8kHz8BitMono);
				bool flag = (num & 1U) != 0U;
				bool flag2 = (num & 2U) != 0U;
				uint num2 = (num & 60U) >> 2;
				uint[] array2 = new uint[] { 8000U, 11025U, 12000U, 16000U, 22050U, 24000U, 32000U, 44100U, 48000U };
				waveFormatEx.wFormatTag = 1;
				waveFormatEx.nChannels = (waveFormatEx.nBlockAlign = (flag ? 2 : 1));
				waveFormatEx.nSamplesPerSec = array2[(int)((UIntPtr)num2)];
				waveFormatEx.wBitsPerSample = 8;
				if (flag2)
				{
					AudioFormatConverter.WaveFormatEx waveFormatEx2 = waveFormatEx;
					waveFormatEx2.wBitsPerSample *= 2;
					AudioFormatConverter.WaveFormatEx waveFormatEx3 = waveFormatEx;
					waveFormatEx3.nBlockAlign *= 2;
				}
				waveFormatEx.nAvgBytesPerSec = waveFormatEx.nSamplesPerSec * (uint)waveFormatEx.nBlockAlign;
			}
			else if (eFormat == AudioFormatConverter.StreamFormat.TrueSpeech_8kHz1BitMono)
			{
				waveFormatEx.wFormatTag = 34;
				waveFormatEx.nChannels = 1;
				waveFormatEx.nSamplesPerSec = 8000U;
				waveFormatEx.nAvgBytesPerSec = 1067U;
				waveFormatEx.nBlockAlign = 32;
				waveFormatEx.wBitsPerSample = 1;
				waveFormatEx.cbSize = 32;
				array = new byte[32];
				array[0] = 1;
				array[2] = 240;
			}
			else if (eFormat >= AudioFormatConverter.StreamFormat.CCITT_ALaw_8kHzMono && eFormat <= AudioFormatConverter.StreamFormat.CCITT_ALaw_44kHzStereo)
			{
				uint num3 = (uint)(eFormat - AudioFormatConverter.StreamFormat.CCITT_ALaw_8kHzMono);
				uint num4 = num3 / 2U;
				uint[] array3 = new uint[] { 8000U, 11025U, 22050U, 44100U };
				bool flag3 = (num3 & 1U) != 0U;
				waveFormatEx.wFormatTag = 6;
				waveFormatEx.nChannels = (waveFormatEx.nBlockAlign = (flag3 ? 2 : 1));
				waveFormatEx.nSamplesPerSec = array3[(int)((UIntPtr)num4)];
				waveFormatEx.wBitsPerSample = 8;
				waveFormatEx.nAvgBytesPerSec = waveFormatEx.nSamplesPerSec * (uint)waveFormatEx.nBlockAlign;
			}
			else if (eFormat >= AudioFormatConverter.StreamFormat.CCITT_uLaw_8kHzMono && eFormat <= AudioFormatConverter.StreamFormat.CCITT_uLaw_44kHzStereo)
			{
				uint num5 = (uint)(eFormat - AudioFormatConverter.StreamFormat.CCITT_uLaw_8kHzMono);
				uint num6 = num5 / 2U;
				uint[] array4 = new uint[] { 8000U, 11025U, 22050U, 44100U };
				bool flag4 = (num5 & 1U) != 0U;
				waveFormatEx.wFormatTag = 7;
				waveFormatEx.nChannels = (waveFormatEx.nBlockAlign = (flag4 ? 2 : 1));
				waveFormatEx.nSamplesPerSec = array4[(int)((UIntPtr)num6)];
				waveFormatEx.wBitsPerSample = 8;
				waveFormatEx.nAvgBytesPerSec = waveFormatEx.nSamplesPerSec * (uint)waveFormatEx.nBlockAlign;
			}
			else if (eFormat >= AudioFormatConverter.StreamFormat.ADPCM_8kHzMono && eFormat <= AudioFormatConverter.StreamFormat.ADPCM_44kHzStereo)
			{
				uint[] array5 = new uint[] { 8000U, 11025U, 22050U, 44100U };
				uint[] array6 = new uint[] { 4096U, 8192U, 5644U, 11289U, 11155U, 22311U, 22179U, 44359U };
				uint[] array7 = new uint[] { 256U, 256U, 512U, 1024U };
				byte[] array8 = new byte[]
				{
					244, 1, 7, 0, 0, 1, 0, 0, 0, 2,
					0, byte.MaxValue, 0, 0, 0, 0, 192, 0, 64, 0,
					240, 0, 0, 0, 204, 1, 48, byte.MaxValue, 136, 1,
					24, byte.MaxValue
				};
				byte[] array9 = new byte[]
				{
					244, 3, 7, 0, 0, 1, 0, 0, 0, 2,
					0, byte.MaxValue, 0, 0, 0, 0, 192, 0, 64, 0,
					240, 0, 0, 0, 204, 1, 48, byte.MaxValue, 136, 1,
					24, byte.MaxValue
				};
				byte[] array10 = new byte[]
				{
					244, 7, 7, 0, 0, 1, 0, 0, 0, 2,
					0, byte.MaxValue, 0, 0, 0, 0, 192, 0, 64, 0,
					240, 0, 0, 0, 204, 1, 48, byte.MaxValue, 136, 1,
					24, byte.MaxValue
				};
				byte[][] array11 = new byte[][] { array8, array8, array9, array10 };
				uint num7 = (uint)(eFormat - AudioFormatConverter.StreamFormat.ADPCM_8kHzMono);
				uint num8 = num7 / 2U;
				bool flag5 = (num7 & 1U) != 0U;
				waveFormatEx.wFormatTag = 2;
				waveFormatEx.nChannels = (flag5 ? 2 : 1);
				waveFormatEx.nSamplesPerSec = array5[(int)((UIntPtr)num8)];
				waveFormatEx.nAvgBytesPerSec = array6[(int)((UIntPtr)num7)];
				waveFormatEx.nBlockAlign = (ushort)(array7[(int)((UIntPtr)num8)] * (uint)waveFormatEx.nChannels);
				waveFormatEx.wBitsPerSample = 4;
				waveFormatEx.cbSize = 32;
				array = (byte[])array11[(int)((UIntPtr)num8)].Clone();
			}
			else if (eFormat >= AudioFormatConverter.StreamFormat.GSM610_8kHzMono && eFormat <= AudioFormatConverter.StreamFormat.GSM610_44kHzMono)
			{
				uint[] array12 = new uint[] { 8000U, 11025U, 22050U, 44100U };
				uint[] array13 = new uint[] { 1625U, 2239U, 4478U, 8957U };
				uint num9 = (uint)(eFormat - AudioFormatConverter.StreamFormat.GSM610_8kHzMono);
				waveFormatEx.wFormatTag = 49;
				waveFormatEx.nChannels = 1;
				waveFormatEx.nSamplesPerSec = array12[(int)((UIntPtr)num9)];
				waveFormatEx.nAvgBytesPerSec = array13[(int)((UIntPtr)num9)];
				waveFormatEx.nBlockAlign = 65;
				waveFormatEx.wBitsPerSample = 0;
				waveFormatEx.cbSize = 2;
				array = new byte[] { 64, 1 };
			}
			else
			{
				waveFormatEx = null;
				switch (eFormat)
				{
				case AudioFormatConverter.StreamFormat.NoAssignedFormat:
				case AudioFormatConverter.StreamFormat.Text:
					break;
				default:
					throw new FormatException();
				}
			}
			if (waveFormatEx == null)
			{
				return null;
			}
			return new SpeechAudioFormatInfo((EncodingFormat)waveFormatEx.wFormatTag, (int)waveFormatEx.nSamplesPerSec, (int)waveFormatEx.wBitsPerSample, (int)waveFormatEx.nChannels, (int)waveFormatEx.nAvgBytesPerSec, (int)waveFormatEx.nBlockAlign, array);
		}

		// Token: 0x02000003 RID: 3
		private enum StreamFormat
		{
			// Token: 0x04000002 RID: 2
			Default = -1,
			// Token: 0x04000003 RID: 3
			NoAssignedFormat,
			// Token: 0x04000004 RID: 4
			Text,
			// Token: 0x04000005 RID: 5
			NonStandardFormat,
			// Token: 0x04000006 RID: 6
			ExtendedAudioFormat,
			// Token: 0x04000007 RID: 7
			PCM_8kHz8BitMono,
			// Token: 0x04000008 RID: 8
			PCM_8kHz8BitStereo,
			// Token: 0x04000009 RID: 9
			PCM_8kHz16BitMono,
			// Token: 0x0400000A RID: 10
			PCM_8kHz16BitStereo,
			// Token: 0x0400000B RID: 11
			PCM_11kHz8BitMono,
			// Token: 0x0400000C RID: 12
			PCM_11kHz8BitStereo,
			// Token: 0x0400000D RID: 13
			PCM_11kHz16BitMono,
			// Token: 0x0400000E RID: 14
			PCM_11kHz16BitStereo,
			// Token: 0x0400000F RID: 15
			PCM_12kHz8BitMono,
			// Token: 0x04000010 RID: 16
			PCM_12kHz8BitStereo,
			// Token: 0x04000011 RID: 17
			PCM_12kHz16BitMono,
			// Token: 0x04000012 RID: 18
			PCM_12kHz16BitStereo,
			// Token: 0x04000013 RID: 19
			PCM_16kHz8BitMono,
			// Token: 0x04000014 RID: 20
			PCM_16kHz8BitStereo,
			// Token: 0x04000015 RID: 21
			PCM_16kHz16BitMono,
			// Token: 0x04000016 RID: 22
			PCM_16kHz16BitStereo,
			// Token: 0x04000017 RID: 23
			PCM_22kHz8BitMono,
			// Token: 0x04000018 RID: 24
			PCM_22kHz8BitStereo,
			// Token: 0x04000019 RID: 25
			PCM_22kHz16BitMono,
			// Token: 0x0400001A RID: 26
			PCM_22kHz16BitStereo,
			// Token: 0x0400001B RID: 27
			PCM_24kHz8BitMono,
			// Token: 0x0400001C RID: 28
			PCM_24kHz8BitStereo,
			// Token: 0x0400001D RID: 29
			PCM_24kHz16BitMono,
			// Token: 0x0400001E RID: 30
			PCM_24kHz16BitStereo,
			// Token: 0x0400001F RID: 31
			PCM_32kHz8BitMono,
			// Token: 0x04000020 RID: 32
			PCM_32kHz8BitStereo,
			// Token: 0x04000021 RID: 33
			PCM_32kHz16BitMono,
			// Token: 0x04000022 RID: 34
			PCM_32kHz16BitStereo,
			// Token: 0x04000023 RID: 35
			PCM_44kHz8BitMono,
			// Token: 0x04000024 RID: 36
			PCM_44kHz8BitStereo,
			// Token: 0x04000025 RID: 37
			PCM_44kHz16BitMono,
			// Token: 0x04000026 RID: 38
			PCM_44kHz16BitStereo,
			// Token: 0x04000027 RID: 39
			PCM_48kHz8BitMono,
			// Token: 0x04000028 RID: 40
			PCM_48kHz8BitStereo,
			// Token: 0x04000029 RID: 41
			PCM_48kHz16BitMono,
			// Token: 0x0400002A RID: 42
			PCM_48kHz16BitStereo,
			// Token: 0x0400002B RID: 43
			TrueSpeech_8kHz1BitMono,
			// Token: 0x0400002C RID: 44
			CCITT_ALaw_8kHzMono,
			// Token: 0x0400002D RID: 45
			CCITT_ALaw_8kHzStereo,
			// Token: 0x0400002E RID: 46
			CCITT_ALaw_11kHzMono,
			// Token: 0x0400002F RID: 47
			CCITT_ALaw_11kHzStereo,
			// Token: 0x04000030 RID: 48
			CCITT_ALaw_22kHzMono,
			// Token: 0x04000031 RID: 49
			CCITT_ALaw_22kHzStereo,
			// Token: 0x04000032 RID: 50
			CCITT_ALaw_44kHzMono,
			// Token: 0x04000033 RID: 51
			CCITT_ALaw_44kHzStereo,
			// Token: 0x04000034 RID: 52
			CCITT_uLaw_8kHzMono,
			// Token: 0x04000035 RID: 53
			CCITT_uLaw_8kHzStereo,
			// Token: 0x04000036 RID: 54
			CCITT_uLaw_11kHzMono,
			// Token: 0x04000037 RID: 55
			CCITT_uLaw_11kHzStereo,
			// Token: 0x04000038 RID: 56
			CCITT_uLaw_22kHzMono,
			// Token: 0x04000039 RID: 57
			CCITT_uLaw_22kHzStereo,
			// Token: 0x0400003A RID: 58
			CCITT_uLaw_44kHzMono,
			// Token: 0x0400003B RID: 59
			CCITT_uLaw_44kHzStereo,
			// Token: 0x0400003C RID: 60
			ADPCM_8kHzMono,
			// Token: 0x0400003D RID: 61
			ADPCM_8kHzStereo,
			// Token: 0x0400003E RID: 62
			ADPCM_11kHzMono,
			// Token: 0x0400003F RID: 63
			ADPCM_11kHzStereo,
			// Token: 0x04000040 RID: 64
			ADPCM_22kHzMono,
			// Token: 0x04000041 RID: 65
			ADPCM_22kHzStereo,
			// Token: 0x04000042 RID: 66
			ADPCM_44kHzMono,
			// Token: 0x04000043 RID: 67
			ADPCM_44kHzStereo,
			// Token: 0x04000044 RID: 68
			GSM610_8kHzMono,
			// Token: 0x04000045 RID: 69
			GSM610_11kHzMono,
			// Token: 0x04000046 RID: 70
			GSM610_22kHzMono,
			// Token: 0x04000047 RID: 71
			GSM610_44kHzMono,
			// Token: 0x04000048 RID: 72
			NUM_FORMATS
		}

		// Token: 0x02000004 RID: 4
		private enum WaveFormatId
		{
			// Token: 0x0400004A RID: 74
			Pcm = 1,
			// Token: 0x0400004B RID: 75
			AdPcm,
			// Token: 0x0400004C RID: 76
			TrueSpeech = 34,
			// Token: 0x0400004D RID: 77
			Alaw = 6,
			// Token: 0x0400004E RID: 78
			Mulaw,
			// Token: 0x0400004F RID: 79
			Gsm610 = 49
		}

		// Token: 0x02000005 RID: 5
		[StructLayout(0)]
		private class WaveFormatEx
		{
			// Token: 0x04000050 RID: 80
			public ushort wFormatTag;

			// Token: 0x04000051 RID: 81
			public ushort nChannels;

			// Token: 0x04000052 RID: 82
			public uint nSamplesPerSec;

			// Token: 0x04000053 RID: 83
			public uint nAvgBytesPerSec;

			// Token: 0x04000054 RID: 84
			public ushort nBlockAlign;

			// Token: 0x04000055 RID: 85
			public ushort wBitsPerSample;

			// Token: 0x04000056 RID: 86
			public ushort cbSize;
		}
	}
}
