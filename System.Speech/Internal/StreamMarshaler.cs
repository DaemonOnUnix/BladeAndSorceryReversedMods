using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Speech.Internal
{
	// Token: 0x0200001E RID: 30
	internal sealed class StreamMarshaler : IDisposable
	{
		// Token: 0x06000098 RID: 152 RVA: 0x0000646A File Offset: 0x0000546A
		internal StreamMarshaler()
		{
		}

		// Token: 0x06000099 RID: 153 RVA: 0x0000647D File Offset: 0x0000547D
		internal StreamMarshaler(Stream stream)
		{
			this._stream = stream;
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00006497 File Offset: 0x00005497
		public void Dispose()
		{
			this._safeHMem.Dispose();
		}

		// Token: 0x0600009B RID: 155 RVA: 0x000064A4 File Offset: 0x000054A4
		internal void ReadArray<T>(T[] ao, int c)
		{
			Type typeFromHandle = typeof(T);
			int num = Marshal.SizeOf(typeFromHandle);
			int num2 = num * c;
			byte[] array = Helpers.ReadStreamToByteArray(this._stream, num2);
			IntPtr intPtr = this._safeHMem.Buffer(num2);
			Marshal.Copy(array, 0, intPtr, num2);
			for (int i = 0; i < c; i++)
			{
				ao[i] = (T)((object)Marshal.PtrToStructure((IntPtr)((long)intPtr + (long)(i * num)), typeFromHandle));
			}
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00006524 File Offset: 0x00005524
		internal void WriteArray<T>(T[] ao, int c)
		{
			Type typeFromHandle = typeof(T);
			int num = Marshal.SizeOf(typeFromHandle);
			int num2 = num * c;
			byte[] array = new byte[num2];
			IntPtr intPtr = this._safeHMem.Buffer(num2);
			for (int i = 0; i < c; i++)
			{
				Marshal.StructureToPtr(ao[i], (IntPtr)((long)intPtr + (long)(i * num)), false);
			}
			Marshal.Copy(intPtr, array, 0, num2);
			this._stream.Write(array, 0, num2);
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000065AC File Offset: 0x000055AC
		internal void ReadArrayChar(char[] ach, int c)
		{
			int num = c * 2;
			if (num > 0)
			{
				byte[] array = Helpers.ReadStreamToByteArray(this._stream, num);
				IntPtr intPtr = this._safeHMem.Buffer(num);
				Marshal.Copy(array, 0, intPtr, num);
				Marshal.Copy(intPtr, ach, 0, c);
			}
		}

		// Token: 0x0600009E RID: 158 RVA: 0x000065F0 File Offset: 0x000055F0
		internal string ReadNullTerminatedString()
		{
			BinaryReader binaryReader = new BinaryReader(this._stream, Encoding.Unicode);
			StringBuilder stringBuilder = new StringBuilder();
			for (;;)
			{
				char c = binaryReader.ReadChar();
				if (c == '\0')
				{
					break;
				}
				stringBuilder.Append(c);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00006630 File Offset: 0x00005630
		internal void WriteArrayChar(char[] ach, int c)
		{
			int num = c * 2;
			if (num > 0)
			{
				byte[] array = new byte[num];
				IntPtr intPtr = this._safeHMem.Buffer(num);
				Marshal.Copy(ach, 0, intPtr, c);
				Marshal.Copy(intPtr, array, 0, num);
				this._stream.Write(array, 0, num);
			}
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x0000667C File Offset: 0x0000567C
		internal void ReadStream(object o)
		{
			int num = Marshal.SizeOf(o.GetType());
			byte[] array = Helpers.ReadStreamToByteArray(this._stream, num);
			IntPtr intPtr = this._safeHMem.Buffer(num);
			Marshal.Copy(array, 0, intPtr, num);
			Marshal.PtrToStructure(intPtr, o);
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x000066C0 File Offset: 0x000056C0
		internal void WriteStream(object o)
		{
			int num = Marshal.SizeOf(o.GetType());
			byte[] array = new byte[num];
			IntPtr intPtr = this._safeHMem.Buffer(num);
			Marshal.StructureToPtr(o, intPtr, false);
			Marshal.Copy(intPtr, array, 0, num);
			this._stream.Write(array, 0, num);
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x0000670C File Offset: 0x0000570C
		internal Stream Stream
		{
			get
			{
				return this._stream;
			}
		}

		// Token: 0x1700001E RID: 30
		// (set) Token: 0x060000A3 RID: 163 RVA: 0x00006714 File Offset: 0x00005714
		internal uint Position
		{
			set
			{
				this._stream.Position = (long)((ulong)value);
			}
		}

		// Token: 0x040000A4 RID: 164
		private HGlobalSafeHandle _safeHMem = new HGlobalSafeHandle();

		// Token: 0x040000A5 RID: 165
		private Stream _stream;
	}
}
