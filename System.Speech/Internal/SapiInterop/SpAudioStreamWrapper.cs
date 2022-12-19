using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Speech.AudioFormat;
using System.Speech.Internal.Synthesis;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000091 RID: 145
	internal class SpAudioStreamWrapper : SpStreamWrapper, ISpStreamFormat, IStream
	{
		// Token: 0x060002DB RID: 731 RVA: 0x00009A2C File Offset: 0x00008A2C
		internal SpAudioStreamWrapper(Stream stream, SpeechAudioFormatInfo audioFormat)
			: base(stream)
		{
			this._formatType = SAPIGuids.SPDFID_WaveFormatEx;
			if (audioFormat != null)
			{
				WAVEFORMATEX waveformatex = default(WAVEFORMATEX);
				waveformatex.wFormatTag = (short)audioFormat.EncodingFormat;
				waveformatex.nChannels = (short)audioFormat.ChannelCount;
				waveformatex.nSamplesPerSec = audioFormat.SamplesPerSecond;
				waveformatex.nAvgBytesPerSec = audioFormat.AverageBytesPerSecond;
				waveformatex.nBlockAlign = (short)audioFormat.BlockAlign;
				waveformatex.wBitsPerSample = (short)audioFormat.BitsPerSample;
				waveformatex.cbSize = (short)audioFormat.FormatSpecificData().Length;
				this._wfx = waveformatex.ToBytes();
				if (waveformatex.cbSize == 0)
				{
					byte[] array = new byte[this._wfx.Length + (int)waveformatex.cbSize];
					Array.Copy(this._wfx, array, this._wfx.Length);
					Array.Copy(audioFormat.FormatSpecificData(), 0, array, this._wfx.Length, (int)waveformatex.cbSize);
					this._wfx = array;
					return;
				}
			}
			else
			{
				try
				{
					this.GetStreamOffsets(stream);
				}
				catch (IOException)
				{
					throw new FormatException(SR.Get(SRID.SynthesizerInvalidWaveFile, new object[0]));
				}
			}
		}

		// Token: 0x060002DC RID: 732 RVA: 0x00009B50 File Offset: 0x00008B50
		void ISpStreamFormat.GetFormat(out Guid guid, out IntPtr format)
		{
			guid = this._formatType;
			format = Marshal.AllocCoTaskMem(this._wfx.Length);
			Marshal.Copy(this._wfx, 0, format, this._wfx.Length);
		}

		// Token: 0x060002DD RID: 733 RVA: 0x00009B8C File Offset: 0x00008B8C
		internal void GetStreamOffsets(Stream stream)
		{
			BinaryReader binaryReader = new BinaryReader(stream);
			SpAudioStreamWrapper.RIFFHDR riffhdr = default(SpAudioStreamWrapper.RIFFHDR);
			riffhdr._id = binaryReader.ReadUInt32();
			riffhdr._len = binaryReader.ReadInt32();
			riffhdr._type = binaryReader.ReadUInt32();
			if (riffhdr._id != 1179011410U && riffhdr._type != 1163280727U)
			{
				throw new FormatException();
			}
			SpAudioStreamWrapper.BLOCKHDR blockhdr = default(SpAudioStreamWrapper.BLOCKHDR);
			blockhdr._id = binaryReader.ReadUInt32();
			blockhdr._len = binaryReader.ReadInt32();
			if (blockhdr._id != 544501094U)
			{
				throw new FormatException();
			}
			this._wfx = binaryReader.ReadBytes(blockhdr._len);
			if (blockhdr._len == 16)
			{
				byte[] array = new byte[18];
				Array.Copy(this._wfx, array, 16);
				this._wfx = array;
			}
			SpAudioStreamWrapper.DATAHDR datahdr;
			for (;;)
			{
				datahdr = default(SpAudioStreamWrapper.DATAHDR);
				if (stream.Position + 8L >= stream.Length)
				{
					break;
				}
				datahdr._id = binaryReader.ReadUInt32();
				datahdr._len = binaryReader.ReadInt32();
				if (datahdr._id == 1635017060U)
				{
					goto Block_6;
				}
				stream.Seek((long)datahdr._len, 1);
			}
			return;
			Block_6:
			this._endOfStreamPosition = stream.Position + (long)datahdr._len;
		}

		// Token: 0x040002B1 RID: 689
		private const uint RIFF_MARKER = 1179011410U;

		// Token: 0x040002B2 RID: 690
		private const uint WAVE_MARKER = 1163280727U;

		// Token: 0x040002B3 RID: 691
		private const uint FMT_MARKER = 544501094U;

		// Token: 0x040002B4 RID: 692
		private const uint DATA_MARKER = 1635017060U;

		// Token: 0x040002B5 RID: 693
		private byte[] _wfx;

		// Token: 0x040002B6 RID: 694
		private Guid _formatType;

		// Token: 0x02000092 RID: 146
		private struct RIFFHDR
		{
			// Token: 0x040002B7 RID: 695
			internal uint _id;

			// Token: 0x040002B8 RID: 696
			internal int _len;

			// Token: 0x040002B9 RID: 697
			internal uint _type;
		}

		// Token: 0x02000093 RID: 147
		private struct BLOCKHDR
		{
			// Token: 0x040002BA RID: 698
			internal uint _id;

			// Token: 0x040002BB RID: 699
			internal int _len;
		}

		// Token: 0x02000094 RID: 148
		private struct DATAHDR
		{
			// Token: 0x040002BC RID: 700
			internal uint _id;

			// Token: 0x040002BD RID: 701
			internal int _len;
		}
	}
}
