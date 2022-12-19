using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal
{
	// Token: 0x0200000E RID: 14
	internal sealed class HGlobalSafeHandle : SafeHandle
	{
		// Token: 0x06000031 RID: 49 RVA: 0x00003260 File Offset: 0x00002260
		internal HGlobalSafeHandle()
			: base(IntPtr.Zero, true)
		{
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00003270 File Offset: 0x00002270
		~HGlobalSafeHandle()
		{
			this.Dispose(false);
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000032A0 File Offset: 0x000022A0
		protected override void Dispose(bool disposing)
		{
			this.ReleaseHandle();
			base.Dispose(disposing);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x000032B8 File Offset: 0x000022B8
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

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000035 RID: 53 RVA: 0x00003316 File Offset: 0x00002316
		public override bool IsInvalid
		{
			get
			{
				return this.handle == IntPtr.Zero;
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00003328 File Offset: 0x00002328
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

		// Token: 0x04000075 RID: 117
		private int _bufferSize;
	}
}
