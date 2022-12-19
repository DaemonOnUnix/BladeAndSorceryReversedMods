using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Speech.Internal
{
	// Token: 0x02000098 RID: 152
	internal sealed class StreamMarshaler : IDisposable
	{
		// Token: 0x06000504 RID: 1284 RVA: 0x0001458E File Offset: 0x0001278E
		internal StreamMarshaler()
		{
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x000145A1 File Offset: 0x000127A1
		internal StreamMarshaler(Stream stream)
		{
			this._stream = stream;
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x000145BB File Offset: 0x000127BB
		public void Dispose()
		{
			this._safeHMem.Dispose();
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x000145C8 File Offset: 0x000127C8
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

		// Token: 0x06000508 RID: 1288 RVA: 0x00014648 File Offset: 0x00012848
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

		// Token: 0x06000509 RID: 1289 RVA: 0x000146D0 File Offset: 0x000128D0
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

		// Token: 0x0600050A RID: 1290 RVA: 0x00014714 File Offset: 0x00012914
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

		// Token: 0x0600050B RID: 1291 RVA: 0x00014754 File Offset: 0x00012954
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

		// Token: 0x0600050C RID: 1292 RVA: 0x000147A0 File Offset: 0x000129A0
		internal void ReadStream(object o)
		{
			int num = Marshal.SizeOf(o.GetType());
			byte[] array = Helpers.ReadStreamToByteArray(this._stream, num);
			IntPtr intPtr = this._safeHMem.Buffer(num);
			Marshal.Copy(array, 0, intPtr, num);
			Marshal.PtrToStructure(intPtr, o);
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x000147E4 File Offset: 0x000129E4
		internal void WriteStream(object o)
		{
			int num = Marshal.SizeOf(o.GetType());
			byte[] array = new byte[num];
			IntPtr intPtr = this._safeHMem.Buffer(num);
			Marshal.StructureToPtr(o, intPtr, false);
			Marshal.Copy(intPtr, array, 0, num);
			this._stream.Write(array, 0, num);
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x0600050E RID: 1294 RVA: 0x00014830 File Offset: 0x00012A30
		internal Stream Stream
		{
			get
			{
				return this._stream;
			}
		}

		// Token: 0x1700012E RID: 302
		// (set) Token: 0x0600050F RID: 1295 RVA: 0x00014838 File Offset: 0x00012A38
		internal uint Position
		{
			set
			{
				this._stream.Position = (long)((ulong)value);
			}
		}

		// Token: 0x04000443 RID: 1091
		private HGlobalSafeHandle _safeHMem = new HGlobalSafeHandle();

		// Token: 0x04000444 RID: 1092
		private Stream _stream;
	}
}
