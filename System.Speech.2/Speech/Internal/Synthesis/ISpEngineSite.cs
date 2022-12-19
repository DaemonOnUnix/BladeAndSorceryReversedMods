using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000B7 RID: 183
	[Guid("9880499B-CCE9-11D2-B503-00C04F797396")]
	[InterfaceType(1)]
	[ComImport]
	internal interface ISpEngineSite
	{
		// Token: 0x0600062C RID: 1580
		void AddEvents([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] SpeechEventSapi[] events, int count);

		// Token: 0x0600062D RID: 1581
		void GetEventInterest(out long eventInterest);

		// Token: 0x0600062E RID: 1582
		[PreserveSig]
		int GetActions();

		// Token: 0x0600062F RID: 1583
		void Write(IntPtr data, int count, IntPtr bytesWritten);

		// Token: 0x06000630 RID: 1584
		void GetRate(out int rate);

		// Token: 0x06000631 RID: 1585
		void GetVolume(out short volume);

		// Token: 0x06000632 RID: 1586
		void GetSkipInfo(out int type, out int count);

		// Token: 0x06000633 RID: 1587
		void CompleteSkip(int skipped);

		// Token: 0x06000634 RID: 1588
		void LoadResource([MarshalAs(UnmanagedType.LPWStr)] string resource, ref string mediaType, out IStream stream);
	}
}
