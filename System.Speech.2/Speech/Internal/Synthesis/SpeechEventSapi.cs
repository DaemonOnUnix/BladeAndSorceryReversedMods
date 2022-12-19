using System;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000B8 RID: 184
	internal struct SpeechEventSapi
	{
		// Token: 0x06000635 RID: 1589 RVA: 0x000187AC File Offset: 0x000169AC
		public static bool operator ==(SpeechEventSapi event1, SpeechEventSapi event2)
		{
			return event1.EventId == event2.EventId && event1.ParameterType == event2.ParameterType && event1.StreamNumber == event2.StreamNumber && event1.AudioStreamOffset == event2.AudioStreamOffset && event1.Param1 == event2.Param1 && event1.Param2 == event2.Param2;
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x00018817 File Offset: 0x00016A17
		public static bool operator !=(SpeechEventSapi event1, SpeechEventSapi event2)
		{
			return !(event1 == event2);
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x00018823 File Offset: 0x00016A23
		public override bool Equals(object obj)
		{
			return obj is SpeechEventSapi && this == (SpeechEventSapi)obj;
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x00018840 File Offset: 0x00016A40
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x040004AC RID: 1196
		public short EventId;

		// Token: 0x040004AD RID: 1197
		public short ParameterType;

		// Token: 0x040004AE RID: 1198
		public int StreamNumber;

		// Token: 0x040004AF RID: 1199
		public long AudioStreamOffset;

		// Token: 0x040004B0 RID: 1200
		public IntPtr Param1;

		// Token: 0x040004B1 RID: 1201
		public IntPtr Param2;
	}
}
