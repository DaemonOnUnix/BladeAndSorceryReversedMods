using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000AC RID: 172
	[Serializable]
	internal class AudioException : Exception
	{
		// Token: 0x060005C0 RID: 1472 RVA: 0x00017130 File Offset: 0x00015330
		internal AudioException()
		{
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x00017138 File Offset: 0x00015338
		internal AudioException(MMSYSERR errorCode)
			: base(string.Format(CultureInfo.InvariantCulture, "{0} - Error Code: 0x{1:x}", new object[]
			{
				SR.Get(SRID.AudioDeviceError, new object[0]),
				(int)errorCode
			}))
		{
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x00017171 File Offset: 0x00015371
		protected AudioException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
