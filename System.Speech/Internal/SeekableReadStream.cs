using System;
using System.Collections.Generic;
using System.IO;

namespace System.Speech.Internal
{
	// Token: 0x0200001D RID: 29
	internal class SeekableReadStream : Stream
	{
		// Token: 0x0600008B RID: 139 RVA: 0x0000616A File Offset: 0x0000516A
		internal SeekableReadStream(Stream baseStream)
		{
			this._canSeek = baseStream.CanSeek;
			this._baseStream = baseStream;
		}

		// Token: 0x17000017 RID: 23
		// (set) Token: 0x0600008C RID: 140 RVA: 0x00006197 File Offset: 0x00005197
		internal bool CacheDataForSeeking
		{
			set
			{
				this._cacheDataForSeeking = value;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600008D RID: 141 RVA: 0x000061A0 File Offset: 0x000051A0
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600008E RID: 142 RVA: 0x000061A3 File Offset: 0x000051A3
		public override bool CanSeek
		{
			get
			{
				return this._canSeek || this._cacheDataForSeeking;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600008F RID: 143 RVA: 0x000061B5 File Offset: 0x000051B5
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000090 RID: 144 RVA: 0x000061B8 File Offset: 0x000051B8
		public override long Length
		{
			get
			{
				return this._baseStream.Length;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000091 RID: 145 RVA: 0x000061C5 File Offset: 0x000051C5
		// (set) Token: 0x06000092 RID: 146 RVA: 0x000061E4 File Offset: 0x000051E4
		public override long Position
		{
			get
			{
				if (this._canSeek)
				{
					return this._baseStream.Position;
				}
				return this._virtualPosition;
			}
			set
			{
				if (this._canSeek)
				{
					this._baseStream.Position = value;
					return;
				}
				if (value == this._virtualPosition)
				{
					return;
				}
				if (value < 0L)
				{
					throw new ArgumentOutOfRangeException("value", SR.Get(SRID.MustBeGreaterThanZero, new object[0]));
				}
				if (!this._cacheDataForSeeking)
				{
					throw new NotSupportedException(SR.Get(SRID.SeekNotSupported, new object[0]));
				}
				if (value < (long)this._buffer.Count)
				{
					this._virtualPosition = value;
					return;
				}
				long num = value - (long)this._buffer.Count;
				if (num > 2147483647L)
				{
					throw new NotSupportedException(SR.Get(SRID.SeekNotSupported, new object[0]));
				}
				byte[] array = new byte[num];
				Helpers.BlockingRead(this._baseStream, array, 0, (int)num);
				this._buffer.AddRange(array);
				this._virtualPosition = value;
			}
		}

		// Token: 0x06000093 RID: 147 RVA: 0x000062C0 File Offset: 0x000052C0
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._canSeek)
			{
				return this._baseStream.Read(buffer, offset, count);
			}
			int num = 0;
			if (this._virtualPosition < (long)this._buffer.Count)
			{
				int num2 = (int)((long)this._buffer.Count - this._virtualPosition);
				if (num2 > count)
				{
					num2 = count;
				}
				this._buffer.CopyTo((int)this._virtualPosition, buffer, offset, num2);
				count -= num2;
				this._virtualPosition += (long)num2;
				offset += num2;
				num += num2;
				if (!this._cacheDataForSeeking && this._virtualPosition >= (long)this._buffer.Count)
				{
					this._buffer.Clear();
				}
			}
			if (count > 0)
			{
				int num3 = this._baseStream.Read(buffer, offset, count);
				num += num3;
				this._virtualPosition += (long)num3;
				if (this._cacheDataForSeeking)
				{
					this._buffer.Capacity += num3;
					for (int i = 0; i < num3; i++)
					{
						this._buffer.Add(buffer[offset + i]);
					}
				}
			}
			return num;
		}

		// Token: 0x06000094 RID: 148 RVA: 0x000063CC File Offset: 0x000053CC
		public override long Seek(long offset, SeekOrigin origin)
		{
			checked
			{
				long num;
				switch (origin)
				{
				case 0:
					num = offset;
					break;
				case 1:
					num = this.Position + offset;
					break;
				case 2:
					num = this.Length + offset;
					break;
				default:
					throw new ArgumentException(SR.Get(SRID.EnumInvalid, new object[] { "SeekOrigin" }), "origin");
				}
				this.Position = num;
				return num;
			}
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00006432 File Offset: 0x00005432
		public override void SetLength(long value)
		{
			throw new NotSupportedException(SR.Get(SRID.SeekNotSupported, new object[0]));
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00006449 File Offset: 0x00005449
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException(SR.Get(SRID.StreamMustBeWriteable, new object[0]));
		}

		// Token: 0x06000097 RID: 151 RVA: 0x0000645D File Offset: 0x0000545D
		public override void Flush()
		{
			this._baseStream.Flush();
		}

		// Token: 0x0400009F RID: 159
		private long _virtualPosition;

		// Token: 0x040000A0 RID: 160
		private List<byte> _buffer = new List<byte>();

		// Token: 0x040000A1 RID: 161
		private Stream _baseStream;

		// Token: 0x040000A2 RID: 162
		private bool _cacheDataForSeeking = true;

		// Token: 0x040000A3 RID: 163
		private bool _canSeek;
	}
}
