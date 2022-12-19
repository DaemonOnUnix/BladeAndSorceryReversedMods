using System;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000E9 RID: 233
	internal struct SpeechEventSapi
	{
		// Token: 0x06000555 RID: 1365 RVA: 0x00017468 File Offset: 0x00016468
		public static bool operator ==(SpeechEventSapi event1, SpeechEventSapi event2)
		{
			return event1.EventId == event2.EventId && event1.ParameterType == event2.ParameterType && event1.StreamNumber == event2.StreamNumber && event1.AudioStreamOffset == event2.AudioStreamOffset && event1.Param1 == event2.Param1 && event1.Param2 == event2.Param2;
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x000174DF File Offset: 0x000164DF
		public static bool operator !=(SpeechEventSapi event1, SpeechEventSapi event2)
		{
			return !(event1 == event2);
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x000174EB File Offset: 0x000164EB
		public override bool Equals(object obj)
		{
			return obj is SpeechEventSapi && this == (SpeechEventSapi)obj;
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x00017508 File Offset: 0x00016508
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x04000421 RID: 1057
		public short EventId;

		// Token: 0x04000422 RID: 1058
		public short ParameterType;

		// Token: 0x04000423 RID: 1059
		public int StreamNumber;

		// Token: 0x04000424 RID: 1060
		public long AudioStreamOffset;

		// Token: 0x04000425 RID: 1061
		public IntPtr Param1;

		// Token: 0x04000426 RID: 1062
		public IntPtr Param2;
	}
}
