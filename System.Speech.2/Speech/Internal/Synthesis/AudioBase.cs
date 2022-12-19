using System;
using System.IO;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000A7 RID: 167
	internal abstract class AudioBase
	{
		// Token: 0x06000587 RID: 1415 RVA: 0x00003BF5 File Offset: 0x00001DF5
		internal AudioBase()
		{
		}

		// Token: 0x06000588 RID: 1416
		internal abstract void Begin(byte[] wfx);

		// Token: 0x06000589 RID: 1417
		internal abstract void End();

		// Token: 0x0600058A RID: 1418 RVA: 0x00015DD0 File Offset: 0x00013FD0
		internal virtual void Play(IntPtr pBuff, int cb)
		{
			byte[] array = new byte[cb];
			Marshal.Copy(pBuff, array, 0, cb);
			this.Play(array);
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x00015DF4 File Offset: 0x00013FF4
		internal virtual void Play(byte[] buffer)
		{
			GCHandle gchandle = GCHandle.Alloc(buffer);
			this.Play(gchandle.AddrOfPinnedObject(), buffer.Length);
			gchandle.Free();
		}

		// Token: 0x0600058C RID: 1420
		internal abstract void Pause();

		// Token: 0x0600058D RID: 1421
		internal abstract void Resume();

		// Token: 0x0600058E RID: 1422
		internal abstract void InjectEvent(TTSEvent ttsEvent);

		// Token: 0x0600058F RID: 1423
		internal abstract void WaitUntilDone();

		// Token: 0x06000590 RID: 1424
		internal abstract void Abort();

		// Token: 0x06000591 RID: 1425 RVA: 0x00015E20 File Offset: 0x00014020
		internal void PlayWaveFile(AudioData audio)
		{
			try
			{
				if (!string.IsNullOrEmpty(audio._mimeType))
				{
					WAVEFORMATEX waveformatex = default(WAVEFORMATEX);
					waveformatex.nChannels = 1;
					waveformatex.nSamplesPerSec = 8000;
					waveformatex.nAvgBytesPerSec = 8000;
					waveformatex.nBlockAlign = 1;
					waveformatex.wBitsPerSample = 8;
					waveformatex.cbSize = 0;
					string mimeType = audio._mimeType;
					if (!(mimeType == "audio/basic"))
					{
						if (!(mimeType == "audio/x-alaw-basic"))
						{
							throw new FormatException(SR.Get(SRID.UnknownMimeFormat, new object[0]));
						}
						waveformatex.wFormatTag = 6;
					}
					else
					{
						waveformatex.wFormatTag = 7;
					}
					this.Begin(waveformatex.ToBytes());
					try
					{
						byte[] array = new byte[(int)audio._stream.Length];
						audio._stream.Read(array, 0, array.Length);
						this.Play(array);
						return;
					}
					finally
					{
						this.WaitUntilDone();
						this.End();
					}
				}
				BinaryReader binaryReader = new BinaryReader(audio._stream);
				try
				{
					byte[] waveFormat = AudioBase.GetWaveFormat(binaryReader);
					if (waveFormat == null)
					{
						throw new FormatException(SR.Get(SRID.NotValidAudioFile, new object[] { audio._uri.ToString() }));
					}
					this.Begin(waveFormat);
					try
					{
						for (;;)
						{
							AudioBase.DATAHDR datahdr = default(AudioBase.DATAHDR);
							if (audio._stream.Position + 8L >= audio._stream.Length)
							{
								break;
							}
							datahdr._id = binaryReader.ReadUInt32();
							datahdr._len = binaryReader.ReadInt32();
							if (datahdr._id == 1635017060U)
							{
								byte[] array2 = Helpers.ReadStreamToByteArray(audio._stream, datahdr._len);
								this.Play(array2);
							}
							else
							{
								audio._stream.Seek((long)datahdr._len, SeekOrigin.Current);
							}
						}
					}
					finally
					{
						this.WaitUntilDone();
						this.End();
					}
				}
				finally
				{
					((IDisposable)binaryReader).Dispose();
				}
			}
			finally
			{
				audio.Dispose();
			}
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x00016058 File Offset: 0x00014258
		internal static byte[] GetWaveFormat(BinaryReader br)
		{
			AudioBase.RIFFHDR riffhdr = default(AudioBase.RIFFHDR);
			riffhdr._id = br.ReadUInt32();
			riffhdr._len = br.ReadInt32();
			riffhdr._type = br.ReadUInt32();
			if (riffhdr._id != 1179011410U && riffhdr._type != 1163280727U)
			{
				return null;
			}
			AudioBase.BLOCKHDR blockhdr = new AudioBase.BLOCKHDR
			{
				_id = br.ReadUInt32(),
				_len = br.ReadInt32()
			};
			if (blockhdr._id != 544501094U)
			{
				return null;
			}
			byte[] array = br.ReadBytes(blockhdr._len);
			if (blockhdr._len == 16)
			{
				byte[] array2 = new byte[18];
				Array.Copy(array, array2, 16);
				array = array2;
			}
			return array;
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x0001610C File Offset: 0x0001430C
		internal static void WriteWaveHeader(Stream stream, WAVEFORMATEX waveEx, long position, int cData)
		{
			AudioBase.RIFFHDR riffhdr = new AudioBase.RIFFHDR(0);
			AudioBase.BLOCKHDR blockhdr = new AudioBase.BLOCKHDR(0);
			AudioBase.DATAHDR datahdr = new AudioBase.DATAHDR(0);
			int num = Marshal.SizeOf(riffhdr);
			int num2 = Marshal.SizeOf(blockhdr);
			int length = waveEx.Length;
			int num3 = Marshal.SizeOf(datahdr);
			int num4 = num + num2 + length + num3;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
				try
				{
					riffhdr._len = num4 + cData - 8;
					binaryWriter.Write(riffhdr._id);
					binaryWriter.Write(riffhdr._len);
					binaryWriter.Write(riffhdr._type);
					blockhdr._len = length;
					binaryWriter.Write(blockhdr._id);
					binaryWriter.Write(blockhdr._len);
					binaryWriter.Write(waveEx.ToBytes());
					datahdr._len = cData;
					binaryWriter.Write(datahdr._id);
					binaryWriter.Write(datahdr._len);
					stream.Seek(position, SeekOrigin.Begin);
					stream.Write(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
				}
				finally
				{
					((IDisposable)binaryWriter).Dispose();
				}
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000594 RID: 1428
		internal abstract TimeSpan Duration { get; }

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000595 RID: 1429 RVA: 0x00016254 File Offset: 0x00014454
		internal virtual long Position
		{
			get
			{
				return 0L;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000597 RID: 1431 RVA: 0x00016261 File Offset: 0x00014461
		// (set) Token: 0x06000596 RID: 1430 RVA: 0x00016258 File Offset: 0x00014458
		internal virtual bool IsAborted
		{
			get
			{
				return this._aborted;
			}
			set
			{
				this._aborted = false;
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000598 RID: 1432 RVA: 0x00016269 File Offset: 0x00014469
		internal virtual byte[] WaveFormat
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0400045F RID: 1119
		protected bool _aborted;

		// Token: 0x04000460 RID: 1120
		private const uint RIFF_MARKER = 1179011410U;

		// Token: 0x04000461 RID: 1121
		private const uint WAVE_MARKER = 1163280727U;

		// Token: 0x04000462 RID: 1122
		private const uint FMT_MARKER = 544501094U;

		// Token: 0x04000463 RID: 1123
		private const uint DATA_MARKER = 1635017060U;

		// Token: 0x0200018D RID: 397
		private struct RIFFHDR
		{
			// Token: 0x06000B84 RID: 2948 RVA: 0x0002DBC0 File Offset: 0x0002BDC0
			internal RIFFHDR(int length)
			{
				this._id = 1179011410U;
				this._type = 1163280727U;
				this._len = length;
			}

			// Token: 0x04000929 RID: 2345
			internal uint _id;

			// Token: 0x0400092A RID: 2346
			internal int _len;

			// Token: 0x0400092B RID: 2347
			internal uint _type;
		}

		// Token: 0x0200018E RID: 398
		private struct BLOCKHDR
		{
			// Token: 0x06000B85 RID: 2949 RVA: 0x0002DBDF File Offset: 0x0002BDDF
			internal BLOCKHDR(int length)
			{
				this._id = 544501094U;
				this._len = length;
			}

			// Token: 0x0400092C RID: 2348
			internal uint _id;

			// Token: 0x0400092D RID: 2349
			internal int _len;
		}

		// Token: 0x0200018F RID: 399
		private struct DATAHDR
		{
			// Token: 0x06000B86 RID: 2950 RVA: 0x0002DBF3 File Offset: 0x0002BDF3
			internal DATAHDR(int length)
			{
				this._id = 1635017060U;
				this._len = length;
			}

			// Token: 0x0400092E RID: 2350
			internal uint _id;

			// Token: 0x0400092F RID: 2351
			internal int _len;
		}
	}
}
