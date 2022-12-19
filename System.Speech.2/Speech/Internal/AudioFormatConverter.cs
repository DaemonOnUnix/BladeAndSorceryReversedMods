using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Speech.AudioFormat;

namespace System.Speech.Internal
{
	// Token: 0x0200008B RID: 139
	internal static class AudioFormatConverter
	{
		// Token: 0x06000496 RID: 1174 RVA: 0x00012404 File Offset: 0x00010604
		internal static SpeechAudioFormatInfo ToSpeechAudioFormatInfo(IntPtr waveFormatPtr)
		{
			AudioFormatConverter.WaveFormatEx waveFormatEx = (AudioFormatConverter.WaveFormatEx)Marshal.PtrToStructure(waveFormatPtr, typeof(AudioFormatConverter.WaveFormatEx));
			byte[] array = new byte[(int)waveFormatEx.cbSize];
			IntPtr intPtr = new IntPtr(waveFormatPtr.ToInt64() + (long)Marshal.SizeOf(waveFormatEx));
			for (int i = 0; i < (int)waveFormatEx.cbSize; i++)
			{
				array[i] = Marshal.ReadByte(intPtr, i);
			}
			return new SpeechAudioFormatInfo((EncodingFormat)waveFormatEx.wFormatTag, (int)waveFormatEx.nSamplesPerSec, (int)((short)waveFormatEx.wBitsPerSample), (int)((short)waveFormatEx.nChannels), (int)waveFormatEx.nAvgBytesPerSec, (int)((short)waveFormatEx.nBlockAlign), array);
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x00012494 File Offset: 0x00010694
		internal static SpeechAudioFormatInfo ToSpeechAudioFormatInfo(string formatString)
		{
			short num;
			if (short.TryParse(formatString, NumberStyles.None, CultureInfo.InvariantCulture, out num))
			{
				return AudioFormatConverter.ConvertFormat((AudioFormatConverter.StreamFormat)num);
			}
			return null;
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x000124BC File Offset: 0x000106BC
		private static SpeechAudioFormatInfo ConvertFormat(AudioFormatConverter.StreamFormat eFormat)
		{
			AudioFormatConverter.WaveFormatEx waveFormatEx = new AudioFormatConverter.WaveFormatEx();
			byte[] array = null;
			if (eFormat >= AudioFormatConverter.StreamFormat.PCM_8kHz8BitMono && eFormat <= AudioFormatConverter.StreamFormat.PCM_48kHz16BitStereo)
			{
				uint num = (uint)(eFormat - AudioFormatConverter.StreamFormat.PCM_8kHz8BitMono);
				bool flag = (num & 1U) > 0U;
				bool flag2 = (num & 2U) > 0U;
				uint num2 = (num & 60U) >> 2;
				uint[] array2 = new uint[] { 8000U, 11025U, 12000U, 16000U, 22050U, 24000U, 32000U, 44100U, 48000U };
				waveFormatEx.wFormatTag = 1;
				waveFormatEx.nChannels = (waveFormatEx.nBlockAlign = (flag ? 2 : 1));
				waveFormatEx.nSamplesPerSec = array2[(int)num2];
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
				bool flag3 = (num3 & 1U) > 0U;
				waveFormatEx.wFormatTag = 6;
				waveFormatEx.nChannels = (waveFormatEx.nBlockAlign = (flag3 ? 2 : 1));
				waveFormatEx.nSamplesPerSec = array3[(int)num4];
				waveFormatEx.wBitsPerSample = 8;
				waveFormatEx.nAvgBytesPerSec = waveFormatEx.nSamplesPerSec * (uint)waveFormatEx.nBlockAlign;
			}
			else if (eFormat >= AudioFormatConverter.StreamFormat.CCITT_uLaw_8kHzMono && eFormat <= AudioFormatConverter.StreamFormat.CCITT_uLaw_44kHzStereo)
			{
				uint num5 = (uint)(eFormat - AudioFormatConverter.StreamFormat.CCITT_uLaw_8kHzMono);
				uint num6 = num5 / 2U;
				uint[] array4 = new uint[] { 8000U, 11025U, 22050U, 44100U };
				bool flag4 = (num5 & 1U) > 0U;
				waveFormatEx.wFormatTag = 7;
				waveFormatEx.nChannels = (waveFormatEx.nBlockAlign = (flag4 ? 2 : 1));
				waveFormatEx.nSamplesPerSec = array4[(int)num6];
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
				bool flag5 = (num7 & 1U) > 0U;
				waveFormatEx.wFormatTag = 2;
				waveFormatEx.nChannels = (flag5 ? 2 : 1);
				waveFormatEx.nSamplesPerSec = array5[(int)num8];
				waveFormatEx.nAvgBytesPerSec = array6[(int)num7];
				waveFormatEx.nBlockAlign = (ushort)(array7[(int)num8] * (uint)waveFormatEx.nChannels);
				waveFormatEx.wBitsPerSample = 4;
				waveFormatEx.cbSize = 32;
				array = (byte[])array11[(int)num8].Clone();
			}
			else if (eFormat >= AudioFormatConverter.StreamFormat.GSM610_8kHzMono && eFormat <= AudioFormatConverter.StreamFormat.GSM610_44kHzMono)
			{
				uint[] array12 = new uint[] { 8000U, 11025U, 22050U, 44100U };
				uint[] array13 = new uint[] { 1625U, 2239U, 4478U, 8957U };
				uint num9 = (uint)(eFormat - AudioFormatConverter.StreamFormat.GSM610_8kHzMono);
				waveFormatEx.wFormatTag = 49;
				waveFormatEx.nChannels = 1;
				waveFormatEx.nSamplesPerSec = array12[(int)num9];
				waveFormatEx.nAvgBytesPerSec = array13[(int)num9];
				waveFormatEx.nBlockAlign = 65;
				waveFormatEx.wBitsPerSample = 0;
				waveFormatEx.cbSize = 2;
				array = new byte[] { 64, 1 };
			}
			else
			{
				waveFormatEx = null;
				if (eFormat != AudioFormatConverter.StreamFormat.NoAssignedFormat && eFormat != AudioFormatConverter.StreamFormat.Text)
				{
					throw new FormatException();
				}
			}
			if (waveFormatEx == null)
			{
				return null;
			}
			return new SpeechAudioFormatInfo((EncodingFormat)waveFormatEx.wFormatTag, (int)waveFormatEx.nSamplesPerSec, (int)waveFormatEx.wBitsPerSample, (int)waveFormatEx.nChannels, (int)waveFormatEx.nAvgBytesPerSec, (int)waveFormatEx.nBlockAlign, array);
		}

		// Token: 0x02000183 RID: 387
		private enum StreamFormat
		{
			// Token: 0x040008BE RID: 2238
			Default = -1,
			// Token: 0x040008BF RID: 2239
			NoAssignedFormat,
			// Token: 0x040008C0 RID: 2240
			Text,
			// Token: 0x040008C1 RID: 2241
			NonStandardFormat,
			// Token: 0x040008C2 RID: 2242
			ExtendedAudioFormat,
			// Token: 0x040008C3 RID: 2243
			PCM_8kHz8BitMono,
			// Token: 0x040008C4 RID: 2244
			PCM_8kHz8BitStereo,
			// Token: 0x040008C5 RID: 2245
			PCM_8kHz16BitMono,
			// Token: 0x040008C6 RID: 2246
			PCM_8kHz16BitStereo,
			// Token: 0x040008C7 RID: 2247
			PCM_11kHz8BitMono,
			// Token: 0x040008C8 RID: 2248
			PCM_11kHz8BitStereo,
			// Token: 0x040008C9 RID: 2249
			PCM_11kHz16BitMono,
			// Token: 0x040008CA RID: 2250
			PCM_11kHz16BitStereo,
			// Token: 0x040008CB RID: 2251
			PCM_12kHz8BitMono,
			// Token: 0x040008CC RID: 2252
			PCM_12kHz8BitStereo,
			// Token: 0x040008CD RID: 2253
			PCM_12kHz16BitMono,
			// Token: 0x040008CE RID: 2254
			PCM_12kHz16BitStereo,
			// Token: 0x040008CF RID: 2255
			PCM_16kHz8BitMono,
			// Token: 0x040008D0 RID: 2256
			PCM_16kHz8BitStereo,
			// Token: 0x040008D1 RID: 2257
			PCM_16kHz16BitMono,
			// Token: 0x040008D2 RID: 2258
			PCM_16kHz16BitStereo,
			// Token: 0x040008D3 RID: 2259
			PCM_22kHz8BitMono,
			// Token: 0x040008D4 RID: 2260
			PCM_22kHz8BitStereo,
			// Token: 0x040008D5 RID: 2261
			PCM_22kHz16BitMono,
			// Token: 0x040008D6 RID: 2262
			PCM_22kHz16BitStereo,
			// Token: 0x040008D7 RID: 2263
			PCM_24kHz8BitMono,
			// Token: 0x040008D8 RID: 2264
			PCM_24kHz8BitStereo,
			// Token: 0x040008D9 RID: 2265
			PCM_24kHz16BitMono,
			// Token: 0x040008DA RID: 2266
			PCM_24kHz16BitStereo,
			// Token: 0x040008DB RID: 2267
			PCM_32kHz8BitMono,
			// Token: 0x040008DC RID: 2268
			PCM_32kHz8BitStereo,
			// Token: 0x040008DD RID: 2269
			PCM_32kHz16BitMono,
			// Token: 0x040008DE RID: 2270
			PCM_32kHz16BitStereo,
			// Token: 0x040008DF RID: 2271
			PCM_44kHz8BitMono,
			// Token: 0x040008E0 RID: 2272
			PCM_44kHz8BitStereo,
			// Token: 0x040008E1 RID: 2273
			PCM_44kHz16BitMono,
			// Token: 0x040008E2 RID: 2274
			PCM_44kHz16BitStereo,
			// Token: 0x040008E3 RID: 2275
			PCM_48kHz8BitMono,
			// Token: 0x040008E4 RID: 2276
			PCM_48kHz8BitStereo,
			// Token: 0x040008E5 RID: 2277
			PCM_48kHz16BitMono,
			// Token: 0x040008E6 RID: 2278
			PCM_48kHz16BitStereo,
			// Token: 0x040008E7 RID: 2279
			TrueSpeech_8kHz1BitMono,
			// Token: 0x040008E8 RID: 2280
			CCITT_ALaw_8kHzMono,
			// Token: 0x040008E9 RID: 2281
			CCITT_ALaw_8kHzStereo,
			// Token: 0x040008EA RID: 2282
			CCITT_ALaw_11kHzMono,
			// Token: 0x040008EB RID: 2283
			CCITT_ALaw_11kHzStereo,
			// Token: 0x040008EC RID: 2284
			CCITT_ALaw_22kHzMono,
			// Token: 0x040008ED RID: 2285
			CCITT_ALaw_22kHzStereo,
			// Token: 0x040008EE RID: 2286
			CCITT_ALaw_44kHzMono,
			// Token: 0x040008EF RID: 2287
			CCITT_ALaw_44kHzStereo,
			// Token: 0x040008F0 RID: 2288
			CCITT_uLaw_8kHzMono,
			// Token: 0x040008F1 RID: 2289
			CCITT_uLaw_8kHzStereo,
			// Token: 0x040008F2 RID: 2290
			CCITT_uLaw_11kHzMono,
			// Token: 0x040008F3 RID: 2291
			CCITT_uLaw_11kHzStereo,
			// Token: 0x040008F4 RID: 2292
			CCITT_uLaw_22kHzMono,
			// Token: 0x040008F5 RID: 2293
			CCITT_uLaw_22kHzStereo,
			// Token: 0x040008F6 RID: 2294
			CCITT_uLaw_44kHzMono,
			// Token: 0x040008F7 RID: 2295
			CCITT_uLaw_44kHzStereo,
			// Token: 0x040008F8 RID: 2296
			ADPCM_8kHzMono,
			// Token: 0x040008F9 RID: 2297
			ADPCM_8kHzStereo,
			// Token: 0x040008FA RID: 2298
			ADPCM_11kHzMono,
			// Token: 0x040008FB RID: 2299
			ADPCM_11kHzStereo,
			// Token: 0x040008FC RID: 2300
			ADPCM_22kHzMono,
			// Token: 0x040008FD RID: 2301
			ADPCM_22kHzStereo,
			// Token: 0x040008FE RID: 2302
			ADPCM_44kHzMono,
			// Token: 0x040008FF RID: 2303
			ADPCM_44kHzStereo,
			// Token: 0x04000900 RID: 2304
			GSM610_8kHzMono,
			// Token: 0x04000901 RID: 2305
			GSM610_11kHzMono,
			// Token: 0x04000902 RID: 2306
			GSM610_22kHzMono,
			// Token: 0x04000903 RID: 2307
			GSM610_44kHzMono,
			// Token: 0x04000904 RID: 2308
			NUM_FORMATS
		}

		// Token: 0x02000184 RID: 388
		private enum WaveFormatId
		{
			// Token: 0x04000906 RID: 2310
			Pcm = 1,
			// Token: 0x04000907 RID: 2311
			AdPcm,
			// Token: 0x04000908 RID: 2312
			TrueSpeech = 34,
			// Token: 0x04000909 RID: 2313
			Alaw = 6,
			// Token: 0x0400090A RID: 2314
			Mulaw,
			// Token: 0x0400090B RID: 2315
			Gsm610 = 49
		}

		// Token: 0x02000185 RID: 389
		[StructLayout(LayoutKind.Sequential)]
		private class WaveFormatEx
		{
			// Token: 0x0400090C RID: 2316
			public ushort wFormatTag;

			// Token: 0x0400090D RID: 2317
			public ushort nChannels;

			// Token: 0x0400090E RID: 2318
			public uint nSamplesPerSec;

			// Token: 0x0400090F RID: 2319
			public uint nAvgBytesPerSec;

			// Token: 0x04000910 RID: 2320
			public ushort nBlockAlign;

			// Token: 0x04000911 RID: 2321
			public ushort wBitsPerSample;

			// Token: 0x04000912 RID: 2322
			public ushort cbSize;
		}
	}
}
