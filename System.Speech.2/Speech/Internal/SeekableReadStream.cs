using System;
using System.Collections.Generic;
using System.IO;

namespace System.Speech.Internal
{
	// Token: 0x02000097 RID: 151
	internal class SeekableReadStream : Stream
	{
		// Token: 0x060004F7 RID: 1271 RVA: 0x00014295 File Offset: 0x00012495
		internal SeekableReadStream(Stream baseStream)
		{
			this._canSeek = baseStream.CanSeek;
			this._baseStream = baseStream;
		}

		// Token: 0x17000127 RID: 295
		// (set) Token: 0x060004F8 RID: 1272 RVA: 0x000142C2 File Offset: 0x000124C2
		internal bool CacheDataForSeeking
		{
			set
			{
				this._cacheDataForSeeking = value;
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x060004F9 RID: 1273 RVA: 0x0000936B File Offset: 0x0000756B
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x060004FA RID: 1274 RVA: 0x000142CB File Offset: 0x000124CB
		public override bool CanSeek
		{
			get
			{
				return this._canSeek || this._cacheDataForSeeking;
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x060004FB RID: 1275 RVA: 0x000142DD File Offset: 0x000124DD
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x060004FC RID: 1276 RVA: 0x000142E0 File Offset: 0x000124E0
		public override long Length
		{
			get
			{
				return this._baseStream.Length;
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x060004FD RID: 1277 RVA: 0x000142ED File Offset: 0x000124ED
		// (set) Token: 0x060004FE RID: 1278 RVA: 0x0001430C File Offset: 0x0001250C
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

		// Token: 0x060004FF RID: 1279 RVA: 0x000143E8 File Offset: 0x000125E8
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

		// Token: 0x06000500 RID: 1280 RVA: 0x000144F4 File Offset: 0x000126F4
		public override long Seek(long offset, SeekOrigin origin)
		{
			checked
			{
				long num;
				switch (origin)
				{
				case SeekOrigin.Begin:
					num = offset;
					break;
				case SeekOrigin.Current:
					num = this.Position + offset;
					break;
				case SeekOrigin.End:
					num = this.Length + offset;
					break;
				default:
					throw new ArgumentException(SR.Get(SRID.EnumInvalid, new object[] { "SeekOrigin" }), "origin");
				}
				this.Position = num;
				return num;
			}
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x00014556 File Offset: 0x00012756
		public override void SetLength(long value)
		{
			throw new NotSupportedException(SR.Get(SRID.SeekNotSupported, new object[0]));
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x0001456D File Offset: 0x0001276D
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException(SR.Get(SRID.StreamMustBeWriteable, new object[0]));
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x00014581 File Offset: 0x00012781
		public override void Flush()
		{
			this._baseStream.Flush();
		}

		// Token: 0x0400043E RID: 1086
		private long _virtualPosition;

		// Token: 0x0400043F RID: 1087
		private List<byte> _buffer = new List<byte>();

		// Token: 0x04000440 RID: 1088
		private Stream _baseStream;

		// Token: 0x04000441 RID: 1089
		private bool _cacheDataForSeeking = true;

		// Token: 0x04000442 RID: 1090
		private bool _canSeek;
	}
}
