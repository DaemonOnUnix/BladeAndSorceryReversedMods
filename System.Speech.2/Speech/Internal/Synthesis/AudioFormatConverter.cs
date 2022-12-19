using System;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000A9 RID: 169
	internal static class AudioFormatConverter
	{
		// Token: 0x0600059F RID: 1439 RVA: 0x0001640C File Offset: 0x0001460C
		internal static short[] Convert(byte[] data, AudioCodec from, AudioCodec to)
		{
			AudioFormatConverter.ConvertByteShort convertByteShort = null;
			if (from <= AudioCodec.G711A)
			{
				if (from != AudioCodec.G711U)
				{
					if (from == AudioCodec.G711A)
					{
						if (to == AudioCodec.PCM16)
						{
							convertByteShort = new AudioFormatConverter.ConvertByteShort(AudioFormatConverter.ConvertALaw2Linear);
							goto IL_80;
						}
						goto IL_80;
					}
				}
				else
				{
					if (to == AudioCodec.PCM16)
					{
						convertByteShort = new AudioFormatConverter.ConvertByteShort(AudioFormatConverter.ConvertULaw2Linear);
						goto IL_80;
					}
					goto IL_80;
				}
			}
			else if (from != AudioCodec.PCM8)
			{
				if (from == AudioCodec.PCM16)
				{
					if (to == AudioCodec.PCM16)
					{
						convertByteShort = new AudioFormatConverter.ConvertByteShort(AudioFormatConverter.ConvertLinear2LinearByteShort);
						goto IL_80;
					}
					goto IL_80;
				}
			}
			else
			{
				if (to == AudioCodec.PCM16)
				{
					convertByteShort = new AudioFormatConverter.ConvertByteShort(AudioFormatConverter.ConvertLinear8LinearByteShort);
					goto IL_80;
				}
				goto IL_80;
			}
			throw new FormatException();
			IL_80:
			if (convertByteShort == null)
			{
				throw new FormatException();
			}
			return convertByteShort(data, data.Length);
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x000164AC File Offset: 0x000146AC
		internal static byte[] Convert(short[] data, AudioCodec from, AudioCodec to)
		{
			AudioFormatConverter.ConvertShortByte convertShortByte = null;
			if (from == AudioCodec.PCM16)
			{
				if (to <= AudioCodec.G711A)
				{
					if (to != AudioCodec.G711U)
					{
						if (to == AudioCodec.G711A)
						{
							convertShortByte = new AudioFormatConverter.ConvertShortByte(AudioFormatConverter.ConvertLinear2ALaw);
						}
					}
					else
					{
						convertShortByte = new AudioFormatConverter.ConvertShortByte(AudioFormatConverter.ConvertLinear2ULaw);
					}
				}
				else if (to != AudioCodec.PCM8)
				{
					if (to == AudioCodec.PCM16)
					{
						convertShortByte = new AudioFormatConverter.ConvertShortByte(AudioFormatConverter.ConvertLinear2LinearShortByte);
					}
				}
				else
				{
					convertShortByte = new AudioFormatConverter.ConvertShortByte(AudioFormatConverter.ConvertLinear8LinearShortByte);
				}
				return convertShortByte(data, data.Length);
			}
			throw new FormatException();
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x0001652C File Offset: 0x0001472C
		internal static AudioCodec TypeOf(WAVEFORMATEX format)
		{
			AudioCodec audioCodec = AudioCodec.Undefined;
			AudioFormatConverter.WaveFormatTag wFormatTag = (AudioFormatConverter.WaveFormatTag)format.wFormatTag;
			if (wFormatTag != AudioFormatConverter.WaveFormatTag.WAVE_FORMAT_PCM)
			{
				if (wFormatTag != AudioFormatConverter.WaveFormatTag.WAVE_FORMAT_ALAW)
				{
					if (wFormatTag == AudioFormatConverter.WaveFormatTag.WAVE_FORMAT_MULAW)
					{
						audioCodec = AudioCodec.G711U;
					}
				}
				else
				{
					audioCodec = AudioCodec.G711A;
				}
			}
			else
			{
				int num = (int)(format.nBlockAlign / format.nChannels);
				if (num != 1)
				{
					if (num == 2)
					{
						audioCodec = AudioCodec.PCM16;
					}
				}
				else
				{
					audioCodec = AudioCodec.PCM8;
				}
			}
			return audioCodec;
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x0001657C File Offset: 0x0001477C
		internal static byte[] ConvertLinear2ULaw(short[] data, int size)
		{
			byte[] array = new byte[size];
			AudioFormatConverter._uLawCompTableCached = ((AudioFormatConverter._uLawCompTableCached == null) ? AudioFormatConverter.CalcLinear2ULawTable() : AudioFormatConverter._uLawCompTableCached);
			for (int i = 0; i < size; i++)
			{
				array[i] = AudioFormatConverter._uLawCompTableCached[(ushort)data[i] >> 2];
			}
			return array;
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x000165C4 File Offset: 0x000147C4
		internal static short[] ConvertULaw2Linear(byte[] data, int size)
		{
			short[] array = new short[size];
			for (int i = 0; i < size; i++)
			{
				int num = AudioFormatConverter.ULaw_exp_table[(int)data[i]];
				array[i] = (short)num;
			}
			return array;
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x000165F4 File Offset: 0x000147F4
		private static byte[] CalcLinear2ULawTable()
		{
			bool flag = false;
			byte[] array = new byte[16384];
			for (int i = 0; i < 65535; i += 4)
			{
				short num = (short)i;
				int num2 = num >> 2 << 2;
				int num3 = (num2 >> 8) & 128;
				if (num3 != 0)
				{
					num2 = -num2;
				}
				if (num2 > 32635)
				{
					num2 = 32635;
				}
				num2 += 132;
				int num4 = AudioFormatConverter.exp_lut_linear2ulaw[(num2 >> 7) & 255];
				int num5 = (num2 >> num4 + 3) & 15;
				byte b = (byte)(~(byte)(num3 | (num4 << 4) | num5));
				if (flag && b == 0)
				{
					b = 2;
				}
				array[i >> 2] = b;
			}
			return array;
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x0001669C File Offset: 0x0001489C
		internal static byte[] ConvertLinear2ALaw(short[] data, int size)
		{
			byte[] array = new byte[size];
			AudioFormatConverter._aLawCompTableCached = ((AudioFormatConverter._aLawCompTableCached == null) ? AudioFormatConverter.CalcLinear2ALawTable() : AudioFormatConverter._aLawCompTableCached);
			for (int i = 0; i < size; i++)
			{
				array[i] = AudioFormatConverter._aLawCompTableCached[(ushort)data[i] >> 2];
			}
			return array;
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x000166E4 File Offset: 0x000148E4
		internal static short[] ConvertALaw2Linear(byte[] data, int size)
		{
			short[] array = new short[size];
			for (int i = 0; i < size; i++)
			{
				int num = AudioFormatConverter.ALaw_exp_table[(int)data[i]];
				array[i] = (short)num;
			}
			return array;
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x00016714 File Offset: 0x00014914
		private static byte[] CalcLinear2ALawTable()
		{
			byte[] array = new byte[16384];
			for (int i = 0; i < 65535; i += 4)
			{
				short num = (short)i;
				int num2 = num >> 2 << 2;
				int num3 = (~num2 >> 8) & 128;
				if (num3 == 0)
				{
					num2 = -num2;
				}
				if (num2 > 31744)
				{
					num2 = 31744;
				}
				byte b;
				if (num2 >= 256)
				{
					int num4 = AudioFormatConverter.exp_lut_linear2alaw[(num2 >> 8) & 127];
					int num5 = (num2 >> num4 + 3) & 15;
					b = (byte)((num4 << 4) | num5);
				}
				else
				{
					b = (byte)(num2 >> 4);
				}
				b ^= (byte)(num3 ^ 85);
				array[i >> 2] = b;
			}
			return array;
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x000167B0 File Offset: 0x000149B0
		private static short[] ConvertLinear2LinearByteShort(byte[] data, int size)
		{
			short[] array = new short[size / 2];
			for (int i = 0; i < size; i += 2)
			{
				array[i / 2] = (short)data[i] + (short)(data[i + 1] << 8);
			}
			return array;
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x000167E8 File Offset: 0x000149E8
		private static short[] ConvertLinear8LinearByteShort(byte[] data, int size)
		{
			short[] array = new short[size];
			for (int i = 0; i < size; i++)
			{
				array[i] = (short)(data[i] - 128 << 8);
			}
			return array;
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x00016818 File Offset: 0x00014A18
		private static byte[] ConvertLinear2LinearShortByte(short[] data, int size)
		{
			byte[] array = new byte[size * 2];
			for (int i = 0; i < size; i++)
			{
				short num = data[i];
				array[2 * i] = (byte)num;
				array[2 * i + 1] = (byte)(num >> 8);
			}
			return array;
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x00016854 File Offset: 0x00014A54
		private static byte[] ConvertLinear8LinearShortByte(short[] data, int size)
		{
			byte[] array = new byte[size];
			for (int i = 0; i < size; i++)
			{
				array[i] = (byte)((ushort)(data[i] + 127 >> 8) + 128);
			}
			return array;
		}

		// Token: 0x0400046B RID: 1131
		private static byte[] _uLawCompTableCached;

		// Token: 0x0400046C RID: 1132
		private static byte[] _aLawCompTableCached;

		// Token: 0x0400046D RID: 1133
		private static readonly int[] exp_lut_linear2alaw = new int[]
		{
			1, 1, 2, 2, 3, 3, 3, 3, 4, 4,
			4, 4, 4, 4, 4, 4, 5, 5, 5, 5,
			5, 5, 5, 5, 5, 5, 5, 5, 5, 5,
			5, 5, 6, 6, 6, 6, 6, 6, 6, 6,
			6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
			6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
			6, 6, 6, 6, 7, 7, 7, 7, 7, 7,
			7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
			7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
			7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
			7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
			7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
			7, 7, 7, 7, 7, 7, 7, 7
		};

		// Token: 0x0400046E RID: 1134
		private static int[] exp_lut_linear2ulaw = new int[]
		{
			0, 0, 1, 1, 2, 2, 2, 2, 3, 3,
			3, 3, 3, 3, 3, 3, 4, 4, 4, 4,
			4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
			4, 4, 5, 5, 5, 5, 5, 5, 5, 5,
			5, 5, 5, 5, 5, 5, 5, 5, 5, 5,
			5, 5, 5, 5, 5, 5, 5, 5, 5, 5,
			5, 5, 5, 5, 6, 6, 6, 6, 6, 6,
			6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
			6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
			6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
			6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
			6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
			6, 6, 6, 6, 6, 6, 6, 6, 7, 7,
			7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
			7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
			7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
			7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
			7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
			7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
			7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
			7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
			7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
			7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
			7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
			7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
			7, 7, 7, 7, 7, 7
		};

		// Token: 0x0400046F RID: 1135
		private static int[] ULaw_exp_table = new int[]
		{
			-32124, -31100, -30076, -29052, -28028, -27004, -25980, -24956, -23932, -22908,
			-21884, -20860, -19836, -18812, -17788, -16764, -15996, -15484, -14972, -14460,
			-13948, -13436, -12924, -12412, -11900, -11388, -10876, -10364, -9852, -9340,
			-8828, -8316, -7932, -7676, -7420, -7164, -6908, -6652, -6396, -6140,
			-5884, -5628, -5372, -5116, -4860, -4604, -4348, -4092, -3900, -3772,
			-3644, -3516, -3388, -3260, -3132, -3004, -2876, -2748, -2620, -2492,
			-2364, -2236, -2108, -1980, -1884, -1820, -1756, -1692, -1628, -1564,
			-1500, -1436, -1372, -1308, -1244, -1180, -1116, -1052, -988, -924,
			-876, -844, -812, -780, -748, -716, -684, -652, -620, -588,
			-556, -524, -492, -460, -428, -396, -372, -356, -340, -324,
			-308, -292, -276, -260, -244, -228, -212, -196, -180, -164,
			-148, -132, -120, -112, -104, -96, -88, -80, -72, -64,
			-56, -48, -40, -32, -24, -16, -8, 0, 32124, 31100,
			30076, 29052, 28028, 27004, 25980, 24956, 23932, 22908, 21884, 20860,
			19836, 18812, 17788, 16764, 15996, 15484, 14972, 14460, 13948, 13436,
			12924, 12412, 11900, 11388, 10876, 10364, 9852, 9340, 8828, 8316,
			7932, 7676, 7420, 7164, 6908, 6652, 6396, 6140, 5884, 5628,
			5372, 5116, 4860, 4604, 4348, 4092, 3900, 3772, 3644, 3516,
			3388, 3260, 3132, 3004, 2876, 2748, 2620, 2492, 2364, 2236,
			2108, 1980, 1884, 1820, 1756, 1692, 1628, 1564, 1500, 1436,
			1372, 1308, 1244, 1180, 1116, 1052, 988, 924, 876, 844,
			812, 780, 748, 716, 684, 652, 620, 588, 556, 524,
			492, 460, 428, 396, 372, 356, 340, 324, 308, 292,
			276, 260, 244, 228, 212, 196, 180, 164, 148, 132,
			120, 112, 104, 96, 88, 80, 72, 64, 56, 48,
			40, 32, 24, 16, 8, 0
		};

		// Token: 0x04000470 RID: 1136
		private static int[] ALaw_exp_table = new int[]
		{
			-5504, -5248, -6016, -5760, -4480, -4224, -4992, -4736, -7552, -7296,
			-8064, -7808, -6528, -6272, -7040, -6784, -2752, -2624, -3008, -2880,
			-2240, -2112, -2496, -2368, -3776, -3648, -4032, -3904, -3264, -3136,
			-3520, -3392, -22016, -20992, -24064, -23040, -17920, -16896, -19968, -18944,
			-30208, -29184, -32256, -31232, -26112, -25088, -28160, -27136, -11008, -10496,
			-12032, -11520, -8960, -8448, -9984, -9472, -15104, -14592, -16128, -15616,
			-13056, -12544, -14080, -13568, -344, -328, -376, -360, -280, -264,
			-312, -296, -472, -456, -504, -488, -408, -392, -440, -424,
			-88, -72, -120, -104, -24, -8, -56, -40, -216, -200,
			-248, -232, -152, -136, -184, -168, -1376, -1312, -1504, -1440,
			-1120, -1056, -1248, -1184, -1888, -1824, -2016, -1952, -1632, -1568,
			-1760, -1696, -688, -656, -752, -720, -560, -528, -624, -592,
			-944, -912, -1008, -976, -816, -784, -880, -848, 5504, 5248,
			6016, 5760, 4480, 4224, 4992, 4736, 7552, 7296, 8064, 7808,
			6528, 6272, 7040, 6784, 2752, 2624, 3008, 2880, 2240, 2112,
			2496, 2368, 3776, 3648, 4032, 3904, 3264, 3136, 3520, 3392,
			22016, 20992, 24064, 23040, 17920, 16896, 19968, 18944, 30208, 29184,
			32256, 31232, 26112, 25088, 28160, 27136, 11008, 10496, 12032, 11520,
			8960, 8448, 9984, 9472, 15104, 14592, 16128, 15616, 13056, 12544,
			14080, 13568, 344, 328, 376, 360, 280, 264, 312, 296,
			472, 456, 504, 488, 408, 392, 440, 424, 88, 72,
			120, 104, 24, 8, 56, 40, 216, 200, 248, 232,
			152, 136, 184, 168, 1376, 1312, 1504, 1440, 1120, 1056,
			1248, 1184, 1888, 1824, 2016, 1952, 1632, 1568, 1760, 1696,
			688, 656, 752, 720, 560, 528, 624, 592, 944, 912,
			1008, 976, 816, 784, 880, 848
		};

		// Token: 0x02000190 RID: 400
		internal enum WaveFormatTag
		{
			// Token: 0x04000931 RID: 2353
			WAVE_FORMAT_PCM = 1,
			// Token: 0x04000932 RID: 2354
			WAVE_FORMAT_ALAW = 6,
			// Token: 0x04000933 RID: 2355
			WAVE_FORMAT_MULAW
		}

		// Token: 0x02000191 RID: 401
		// (Invoke) Token: 0x06000B88 RID: 2952
		private delegate short[] ConvertByteShort(byte[] data, int size);

		// Token: 0x02000192 RID: 402
		// (Invoke) Token: 0x06000B8C RID: 2956
		private delegate byte[] ConvertShortByte(short[] data, int size);
	}
}
