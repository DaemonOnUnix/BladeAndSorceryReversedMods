using System;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000B9 RID: 185
	internal class PcmConverter
	{
		// Token: 0x06000639 RID: 1593 RVA: 0x00018854 File Offset: 0x00016A54
		internal bool PrepareConverter(ref WAVEFORMATEX inWavFormat, ref WAVEFORMATEX outWavFormat)
		{
			bool flag = true;
			if (inWavFormat.nSamplesPerSec <= 0 || inWavFormat.nChannels > 2 || inWavFormat.nChannels <= 0 || outWavFormat.nChannels <= 0 || outWavFormat.nSamplesPerSec <= 0 || outWavFormat.nChannels > 2)
			{
				throw new FormatException();
			}
			this._iInFormatType = AudioFormatConverter.TypeOf(inWavFormat);
			this._iOutFormatType = AudioFormatConverter.TypeOf(outWavFormat);
			if (this._iInFormatType < AudioCodec.G711U || this._iOutFormatType < AudioCodec.G711U)
			{
				throw new FormatException();
			}
			if (outWavFormat.nSamplesPerSec == inWavFormat.nSamplesPerSec && this._iOutFormatType == this._iInFormatType && outWavFormat.nChannels == inWavFormat.nChannels)
			{
				flag = false;
			}
			else
			{
				if (inWavFormat.nSamplesPerSec != outWavFormat.nSamplesPerSec)
				{
					this.CreateResamplingFilter(inWavFormat.nSamplesPerSec, outWavFormat.nSamplesPerSec);
				}
				this._inWavFormat = inWavFormat;
				this._outWavFormat = outWavFormat;
			}
			return flag;
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x00018940 File Offset: 0x00016B40
		internal byte[] ConvertSamples(byte[] pvInSamples)
		{
			short[] array = null;
			short[] array2 = AudioFormatConverter.Convert(pvInSamples, this._iInFormatType, AudioCodec.PCM16);
			if (this._inWavFormat.nChannels == 2 && this._outWavFormat.nChannels == 1)
			{
				array = this.Resample(this._inWavFormat, this._outWavFormat, PcmConverter.Stereo2Mono(array2), this._leftMemory);
			}
			else if (this._inWavFormat.nChannels == 1 && this._outWavFormat.nChannels == 2)
			{
				array = PcmConverter.Mono2Stereo(this.Resample(this._inWavFormat, this._outWavFormat, array2, this._leftMemory));
			}
			if (this._inWavFormat.nChannels == 2 && this._outWavFormat.nChannels == 2)
			{
				if (this._inWavFormat.nSamplesPerSec != this._outWavFormat.nSamplesPerSec)
				{
					short[] array3;
					short[] array4;
					PcmConverter.SplitStereo(array2, out array3, out array4);
					array = PcmConverter.MergeStereo(this.Resample(this._inWavFormat, this._outWavFormat, array3, this._leftMemory), this.Resample(this._inWavFormat, this._outWavFormat, array4, this._rightMemory));
				}
				else
				{
					array = array2;
				}
			}
			if (this._inWavFormat.nChannels == 1 && this._outWavFormat.nChannels == 1)
			{
				array = this.Resample(this._inWavFormat, this._outWavFormat, array2, this._leftMemory);
			}
			this._eChunkStatus = PcmConverter.Block.Middle;
			return AudioFormatConverter.Convert(array, AudioCodec.PCM16, this._iOutFormatType);
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x00018AA4 File Offset: 0x00016CA4
		private short[] Resample(WAVEFORMATEX inWavFormat, WAVEFORMATEX outWavFormat, short[] pnBuff, float[] memory)
		{
			if (inWavFormat.nSamplesPerSec != outWavFormat.nSamplesPerSec)
			{
				float[] array = PcmConverter.Short2Float(pnBuff);
				array = this.Resampling(array, memory);
				pnBuff = PcmConverter.Float2Short(array);
			}
			return pnBuff;
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x00018ADC File Offset: 0x00016CDC
		private static float[] Short2Float(short[] inSamples)
		{
			float[] array = new float[inSamples.Length];
			for (int i = 0; i < inSamples.Length; i++)
			{
				array[i] = (float)inSamples[i];
			}
			return array;
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x00018B08 File Offset: 0x00016D08
		private static short[] Float2Short(float[] inSamples)
		{
			short[] array = new short[inSamples.Length];
			for (int i = 0; i < inSamples.Length; i++)
			{
				float num;
				if (inSamples[i] >= 0f)
				{
					num = inSamples[i] + 0.5f;
					if (num > 32767f)
					{
						num = 32767f;
					}
				}
				else
				{
					num = inSamples[i] - 0.5f;
					if (num < -32768f)
					{
						num = -32768f;
					}
				}
				array[i] = (short)num;
			}
			return array;
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x00018B70 File Offset: 0x00016D70
		private static short[] Mono2Stereo(short[] inSamples)
		{
			short[] array = new short[inSamples.Length * 2];
			int i = 0;
			int num = 0;
			while (i < inSamples.Length)
			{
				array[num] = inSamples[i];
				array[num + 1] = inSamples[i];
				i++;
				num += 2;
			}
			return array;
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x00018BAC File Offset: 0x00016DAC
		private static short[] Stereo2Mono(short[] inSamples)
		{
			short[] array = new short[inSamples.Length / 2];
			int i = 0;
			int num = 0;
			while (i < inSamples.Length)
			{
				array[num] = (inSamples[i] + inSamples[i + 1]) / 2;
				i += 2;
				num++;
			}
			return array;
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x00018BE8 File Offset: 0x00016DE8
		private static short[] MergeStereo(short[] leftSamples, short[] rightSamples)
		{
			short[] array = new short[leftSamples.Length * 2];
			int i = 0;
			int num = 0;
			while (i < leftSamples.Length)
			{
				array[num] = leftSamples[i];
				array[num + 1] = rightSamples[i];
				i++;
				num += 2;
			}
			return array;
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x00018C24 File Offset: 0x00016E24
		private static void SplitStereo(short[] inSamples, out short[] leftSamples, out short[] rightSamples)
		{
			int num = inSamples.Length / 2;
			leftSamples = new short[num];
			rightSamples = new short[num];
			int i = 0;
			int num2 = 0;
			while (i < inSamples.Length)
			{
				leftSamples[num2] = inSamples[i];
				rightSamples[num2] = inSamples[i + 1];
				i += 2;
			}
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x00018C68 File Offset: 0x00016E68
		private void CreateResamplingFilter(int inHz, int outHz)
		{
			if (inHz <= 0)
			{
				throw new ArgumentOutOfRangeException("inHz");
			}
			if (outHz <= 0)
			{
				throw new ArgumentOutOfRangeException("outHz");
			}
			this.FindResampleFactors(inHz, outHz);
			int num = ((this._iUpFactor > this._iDownFactor) ? this._iUpFactor : this._iDownFactor);
			this._iFilterHalf = (int)((float)(inHz * num) * 0.0005f);
			this._iFilterLen = 2 * this._iFilterHalf + 1;
			this._filterCoeff = this.WindowedLowPass(0.5f / (float)num, (float)this._iUpFactor);
			this._iBuffLen = (int)((float)this._iFilterLen / (float)this._iUpFactor);
			this._leftMemory = new float[this._iBuffLen];
			this._rightMemory = new float[this._iBuffLen];
			this._eChunkStatus = PcmConverter.Block.First;
		}

		// Token: 0x06000643 RID: 1603 RVA: 0x00018D34 File Offset: 0x00016F34
		private float[] WindowedLowPass(float dCutOff, float dGain)
		{
			float[] array = PcmConverter.Blackman(this._iFilterLen, true);
			float[] array2 = new float[this._iFilterLen];
			double num = 6.283185307179586 * (double)dCutOff;
			array2[this._iFilterHalf] = (float)((double)dGain * 2.0 * (double)dCutOff);
			for (long num2 = 1L; num2 <= (long)this._iFilterHalf; num2 += 1L)
			{
				double num3 = (double)dGain * Math.Sin(num * (double)num2) / (3.141592653589793 * (double)num2) * (double)array[(int)(checked((IntPtr)(unchecked((long)this._iFilterHalf - num2))))];
				checked
				{
					array2[(int)((IntPtr)(unchecked((long)this._iFilterHalf + num2)))] = (float)num3;
					array2[(int)((IntPtr)(unchecked((long)this._iFilterHalf - num2)))] = (float)num3;
				}
			}
			return array2;
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x00018DE8 File Offset: 0x00016FE8
		private void FindResampleFactors(int inHz, int outHz)
		{
			int num = 1;
			while (num != 0)
			{
				num = 0;
				for (int i = 0; i < PcmConverter.piPrimes.Length; i++)
				{
					if (inHz % PcmConverter.piPrimes[i] == 0 && outHz % PcmConverter.piPrimes[i] == 0)
					{
						inHz /= PcmConverter.piPrimes[i];
						outHz /= PcmConverter.piPrimes[i];
						num = 1;
						break;
					}
				}
			}
			this._iUpFactor = outHz;
			this._iDownFactor = inHz;
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x00018E50 File Offset: 0x00017050
		private float[] Resampling(float[] inSamples, float[] pdMemory)
		{
			int num = inSamples.Length;
			int num2;
			int num3;
			if (this._eChunkStatus == PcmConverter.Block.First)
			{
				num2 = (num * this._iUpFactor - this._iFilterHalf) / this._iDownFactor;
				num3 = 1;
			}
			else if (this._eChunkStatus == PcmConverter.Block.Middle)
			{
				num2 = num * this._iUpFactor / this._iDownFactor;
				num3 = 2;
			}
			else
			{
				num2 = this._iFilterHalf * this._iUpFactor / this._iDownFactor;
				num3 = 2;
			}
			if (num2 < 0)
			{
				num2 = 0;
			}
			float[] array = new float[num2];
			for (int i = 0; i < num2; i++)
			{
				double num4 = 0.0;
				int num5 = (i * this._iDownFactor - num3 * this._iFilterHalf) / this._iUpFactor;
				int num6 = i * this._iDownFactor - (num5 * this._iUpFactor + num3 * this._iFilterHalf);
				for (int j = 0; j < this._iFilterLen / this._iUpFactor; j++)
				{
					if (this._iUpFactor * j > num6)
					{
						if (num5 + j >= 0 && num5 + j < num)
						{
							num4 += (double)(inSamples[num5 + j] * this._filterCoeff[this._iUpFactor * j - num6]);
						}
						else if (num5 + j < 0)
						{
							num4 += (double)(pdMemory[this._iBuffLen + num5 + j] * this._filterCoeff[this._iUpFactor * j - num6]);
						}
					}
				}
				array[i] = (float)num4;
			}
			if (this._eChunkStatus != PcmConverter.Block.Last)
			{
				int num5 = num - (this._iBuffLen + 1);
				for (int k = 0; k < this._iBuffLen; k++)
				{
					if (num5 >= 0)
					{
						pdMemory[k] = inSamples[num5++];
					}
					else
					{
						num5++;
						pdMemory[k] = 0f;
					}
				}
			}
			return array;
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x00018FF8 File Offset: 0x000171F8
		private static float[] Blackman(int iLength, bool bSymmetric)
		{
			float[] array = new float[iLength];
			double num = 6.283185307179586;
			if (bSymmetric)
			{
				num /= (double)((float)(iLength - 1));
			}
			else
			{
				num /= (double)((float)iLength);
			}
			double num2 = 2.0 * num;
			for (int i = 0; i < iLength; i++)
			{
				array[i] = (float)(0.42 - 0.5 * Math.Cos(num * (double)i) + 0.08 * Math.Cos(num2 * (double)i));
			}
			return array;
		}

		// Token: 0x040004B2 RID: 1202
		private WAVEFORMATEX _inWavFormat;

		// Token: 0x040004B3 RID: 1203
		private WAVEFORMATEX _outWavFormat;

		// Token: 0x040004B4 RID: 1204
		private AudioCodec _iInFormatType;

		// Token: 0x040004B5 RID: 1205
		private AudioCodec _iOutFormatType;

		// Token: 0x040004B6 RID: 1206
		private PcmConverter.Block _eChunkStatus;

		// Token: 0x040004B7 RID: 1207
		private int _iUpFactor;

		// Token: 0x040004B8 RID: 1208
		private int _iFilterHalf;

		// Token: 0x040004B9 RID: 1209
		private int _iDownFactor;

		// Token: 0x040004BA RID: 1210
		private int _iFilterLen;

		// Token: 0x040004BB RID: 1211
		private int _iBuffLen;

		// Token: 0x040004BC RID: 1212
		private float[] _filterCoeff;

		// Token: 0x040004BD RID: 1213
		private float[] _leftMemory;

		// Token: 0x040004BE RID: 1214
		private float[] _rightMemory;

		// Token: 0x040004BF RID: 1215
		private const float _dHalfFilterLen = 0.0005f;

		// Token: 0x040004C0 RID: 1216
		private static readonly int[] piPrimes = new int[]
		{
			2, 3, 5, 7, 11, 13, 17, 19, 23, 29,
			31, 37
		};

		// Token: 0x02000196 RID: 406
		private enum Block
		{
			// Token: 0x0400093F RID: 2367
			First,
			// Token: 0x04000940 RID: 2368
			Middle,
			// Token: 0x04000941 RID: 2369
			Last
		}
	}
}
