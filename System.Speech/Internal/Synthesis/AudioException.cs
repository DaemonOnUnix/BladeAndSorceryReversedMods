using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000DA RID: 218
	[Serializable]
	internal class AudioException : Exception
	{
		// Token: 0x060004D7 RID: 1239 RVA: 0x00015DA8 File Offset: 0x00014DA8
		internal AudioException()
		{
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x00015DB0 File Offset: 0x00014DB0
		internal AudioException(MMSYSERR errorCode)
			: base(string.Format(CultureInfo.InvariantCulture, "{0} - Error Code: 0x{1:x}", new object[]
			{
				SR.Get(SRID.AudioDeviceError, new object[0]),
				(int)errorCode
			}))
		{
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x00015DF6 File Offset: 0x00014DF6
		protected AudioException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
