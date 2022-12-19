using System;
using System.IO;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000CE RID: 206
	internal abstract class AudioBase
	{
		// Token: 0x0600048F RID: 1167 RVA: 0x00013BD8 File Offset: 0x00012BD8
		internal AudioBase()
		{
		}

		// Token: 0x06000490 RID: 1168
		internal abstract void Begin(byte[] wfx);

		// Token: 0x06000491 RID: 1169
		internal abstract void End();

		// Token: 0x06000492 RID: 1170 RVA: 0x00013BE0 File Offset: 0x00012BE0
		internal virtual void Play(IntPtr pBuff, int cb)
		{
			byte[] array = new byte[cb];
			Marshal.Copy(pBuff, array, 0, cb);
			this.Play(array);
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x00013C04 File Offset: 0x00012C04
		internal virtual void Play(byte[] buffer)
		{
			GCHandle gchandle = GCHandle.Alloc(buffer);
			this.Play(gchandle.AddrOfPinnedObject(), buffer.Length);
			gchandle.Free();
		}

		// Token: 0x06000494 RID: 1172
		internal abstract void Pause();

		// Token: 0x06000495 RID: 1173
		internal abstract void Resume();

		// Token: 0x06000496 RID: 1174
		internal abstract void InjectEvent(TTSEvent ttsEvent);

		// Token: 0x06000497 RID: 1175
		internal abstract void WaitUntilDone();

		// Token: 0x06000498 RID: 1176
		internal abstract void Abort();

		// Token: 0x06000499 RID: 1177 RVA: 0x00013C30 File Offset: 0x00012C30
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
					string mimeType;
					if ((mimeType = audio._mimeType) != null)
					{
						if (!(mimeType == "audio/basic"))
						{
							if (!(mimeType == "audio/x-alaw-basic"))
							{
								goto IL_8D;
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
							goto IL_1C7;
						}
						finally
						{
							this.WaitUntilDone();
							this.End();
						}
						goto IL_E9;
					}
					IL_8D:
					throw new FormatException(SR.Get(SRID.UnknownMimeFormat, new object[0]));
				}
				IL_E9:
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
								audio._stream.Seek((long)datahdr._len, 1);
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
					binaryReader.Dispose();
				}
				IL_1C7:;
			}
			finally
			{
				audio.Dispose();
			}
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x00013E74 File Offset: 0x00012E74
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
			AudioBase.BLOCKHDR blockhdr = default(AudioBase.BLOCKHDR);
			blockhdr._id = br.ReadUInt32();
			blockhdr._len = br.ReadInt32();
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

		// Token: 0x0600049B RID: 1179 RVA: 0x00013F30 File Offset: 0x00012F30
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
					stream.Seek(position, 0);
					stream.Write(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
				}
				finally
				{
					binaryWriter.Dispose();
				}
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600049C RID: 1180
		internal abstract TimeSpan Duration { get; }

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x0600049D RID: 1181 RVA: 0x00014084 File Offset: 0x00013084
		internal virtual long Position
		{
			get
			{
				return 0L;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x0600049F RID: 1183 RVA: 0x00014091 File Offset: 0x00013091
		// (set) Token: 0x0600049E RID: 1182 RVA: 0x00014088 File Offset: 0x00013088
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

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060004A0 RID: 1184 RVA: 0x00014099 File Offset: 0x00013099
		internal virtual byte[] WaveFormat
		{
			get
			{
				return null;
			}
		}

		// Token: 0x040003BF RID: 959
		private const uint RIFF_MARKER = 1179011410U;

		// Token: 0x040003C0 RID: 960
		private const uint WAVE_MARKER = 1163280727U;

		// Token: 0x040003C1 RID: 961
		private const uint FMT_MARKER = 544501094U;

		// Token: 0x040003C2 RID: 962
		private const uint DATA_MARKER = 1635017060U;

		// Token: 0x040003C3 RID: 963
		protected bool _aborted;

		// Token: 0x020000CF RID: 207
		private struct RIFFHDR
		{
			// Token: 0x060004A1 RID: 1185 RVA: 0x0001409C File Offset: 0x0001309C
			internal RIFFHDR(int length)
			{
				this._id = 1179011410U;
				this._type = 1163280727U;
				this._len = length;
			}

			// Token: 0x040003C4 RID: 964
			internal uint _id;

			// Token: 0x040003C5 RID: 965
			internal int _len;

			// Token: 0x040003C6 RID: 966
			internal uint _type;
		}

		// Token: 0x020000D0 RID: 208
		private struct BLOCKHDR
		{
			// Token: 0x060004A2 RID: 1186 RVA: 0x000140BB File Offset: 0x000130BB
			internal BLOCKHDR(int length)
			{
				this._id = 544501094U;
				this._len = length;
			}

			// Token: 0x040003C7 RID: 967
			internal uint _id;

			// Token: 0x040003C8 RID: 968
			internal int _len;
		}

		// Token: 0x020000D1 RID: 209
		private struct DATAHDR
		{
			// Token: 0x060004A3 RID: 1187 RVA: 0x000140CF File Offset: 0x000130CF
			internal DATAHDR(int length)
			{
				this._id = 1635017060U;
				this._len = length;
			}

			// Token: 0x040003C9 RID: 969
			internal uint _id;

			// Token: 0x040003CA RID: 970
			internal int _len;
		}
	}
}
