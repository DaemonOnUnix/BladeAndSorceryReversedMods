using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Speech.AudioFormat;
using System.Speech.Internal.Synthesis;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000167 RID: 359
	internal class SpAudioStreamWrapper : SpStreamWrapper, ISpStreamFormat, IStream
	{
		// Token: 0x06000AD0 RID: 2768 RVA: 0x0002BC40 File Offset: 0x00029E40
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

		// Token: 0x06000AD1 RID: 2769 RVA: 0x0002BD64 File Offset: 0x00029F64
		void ISpStreamFormat.GetFormat(out Guid guid, out IntPtr format)
		{
			guid = this._formatType;
			format = Marshal.AllocCoTaskMem(this._wfx.Length);
			Marshal.Copy(this._wfx, 0, format, this._wfx.Length);
		}

		// Token: 0x06000AD2 RID: 2770 RVA: 0x0002BD98 File Offset: 0x00029F98
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
			SpAudioStreamWrapper.BLOCKHDR blockhdr = new SpAudioStreamWrapper.BLOCKHDR
			{
				_id = binaryReader.ReadUInt32(),
				_len = binaryReader.ReadInt32()
			};
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
					return;
				}
				datahdr._id = binaryReader.ReadUInt32();
				datahdr._len = binaryReader.ReadInt32();
				if (datahdr._id == 1635017060U)
				{
					break;
				}
				stream.Seek((long)datahdr._len, SeekOrigin.Current);
			}
			this._endOfStreamPosition = stream.Position + (long)datahdr._len;
		}

		// Token: 0x04000821 RID: 2081
		private const uint RIFF_MARKER = 1179011410U;

		// Token: 0x04000822 RID: 2082
		private const uint WAVE_MARKER = 1163280727U;

		// Token: 0x04000823 RID: 2083
		private const uint FMT_MARKER = 544501094U;

		// Token: 0x04000824 RID: 2084
		private const uint DATA_MARKER = 1635017060U;

		// Token: 0x04000825 RID: 2085
		private byte[] _wfx;

		// Token: 0x04000826 RID: 2086
		private Guid _formatType;

		// Token: 0x020001CF RID: 463
		private struct RIFFHDR
		{
			// Token: 0x04000A01 RID: 2561
			internal uint _id;

			// Token: 0x04000A02 RID: 2562
			internal int _len;

			// Token: 0x04000A03 RID: 2563
			internal uint _type;
		}

		// Token: 0x020001D0 RID: 464
		private struct BLOCKHDR
		{
			// Token: 0x04000A04 RID: 2564
			internal uint _id;

			// Token: 0x04000A05 RID: 2565
			internal int _len;
		}

		// Token: 0x020001D1 RID: 465
		private struct DATAHDR
		{
			// Token: 0x04000A06 RID: 2566
			internal uint _id;

			// Token: 0x04000A07 RID: 2567
			internal int _len;
		}
	}
}
