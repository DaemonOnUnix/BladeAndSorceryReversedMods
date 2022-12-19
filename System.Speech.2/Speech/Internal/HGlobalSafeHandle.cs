using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal
{
	// Token: 0x02000090 RID: 144
	internal sealed class HGlobalSafeHandle : SafeHandle
	{
		// Token: 0x060004B7 RID: 1207 RVA: 0x00012F4D File Offset: 0x0001114D
		internal HGlobalSafeHandle()
			: base(IntPtr.Zero, true)
		{
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x00012F5C File Offset: 0x0001115C
		~HGlobalSafeHandle()
		{
			this.Dispose(false);
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x00012F8C File Offset: 0x0001118C
		protected override void Dispose(bool disposing)
		{
			this.ReleaseHandle();
			base.Dispose(disposing);
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x00012F9C File Offset: 0x0001119C
		internal IntPtr Buffer(int size)
		{
			if (size > this._bufferSize)
			{
				if (this._bufferSize == 0)
				{
					base.SetHandle(Marshal.AllocHGlobal(size));
				}
				else
				{
					base.SetHandle(Marshal.ReAllocHGlobal(this.handle, (IntPtr)size));
				}
				GC.AddMemoryPressure((long)(size - this._bufferSize));
				this._bufferSize = size;
			}
			return this.handle;
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060004BB RID: 1211 RVA: 0x00012FFA File Offset: 0x000111FA
		public override bool IsInvalid
		{
			get
			{
				return this.handle == IntPtr.Zero;
			}
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x0001300C File Offset: 0x0001120C
		protected override bool ReleaseHandle()
		{
			if (this.handle != IntPtr.Zero)
			{
				if (this._bufferSize > 0)
				{
					GC.RemoveMemoryPressure((long)this._bufferSize);
					this._bufferSize = 0;
				}
				Marshal.FreeHGlobal(this.handle);
				this.handle = IntPtr.Zero;
				return true;
			}
			return false;
		}

		// Token: 0x0400042D RID: 1069
		private int _bufferSize;
	}
}
