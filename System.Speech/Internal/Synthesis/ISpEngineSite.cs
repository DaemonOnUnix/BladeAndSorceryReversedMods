using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000E6 RID: 230
	[Guid("9880499B-CCE9-11D2-B503-00C04F797396")]
	[InterfaceType(1)]
	[ComImport]
	internal interface ISpEngineSite
	{
		// Token: 0x06000542 RID: 1346
		void AddEvents([MarshalAs(42, SizeParamIndex = 1)] SpeechEventSapi[] events, int count);

		// Token: 0x06000543 RID: 1347
		void GetEventInterest(out long eventInterest);

		// Token: 0x06000544 RID: 1348
		[PreserveSig]
		int GetActions();

		// Token: 0x06000545 RID: 1349
		void Write(IntPtr data, int count, IntPtr bytesWritten);

		// Token: 0x06000546 RID: 1350
		void GetRate(out int rate);

		// Token: 0x06000547 RID: 1351
		void GetVolume(out short volume);

		// Token: 0x06000548 RID: 1352
		void GetSkipInfo(out int type, out int count);

		// Token: 0x06000549 RID: 1353
		void CompleteSkip(int skipped);

		// Token: 0x0600054A RID: 1354
		void LoadResource([MarshalAs(21)] string resource, ref string mediaType, out IStream stream);
	}
}
