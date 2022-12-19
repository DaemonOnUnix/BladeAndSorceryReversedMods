using System;
using System.ComponentModel;

namespace System.Speech.Synthesis.TtsEngine
{
	// Token: 0x02000163 RID: 355
	[ImmutableObject(true)]
	public struct SpeechEventInfo : IEquatable<SpeechEventInfo>
	{
		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000910 RID: 2320 RVA: 0x00028176 File Offset: 0x00027176
		// (set) Token: 0x06000911 RID: 2321 RVA: 0x0002817E File Offset: 0x0002717E
		public short EventId
		{
			get
			{
				return this._eventId;
			}
			internal set
			{
				this._eventId = value;
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000912 RID: 2322 RVA: 0x00028187 File Offset: 0x00027187
		// (set) Token: 0x06000913 RID: 2323 RVA: 0x0002818F File Offset: 0x0002718F
		public short ParameterType
		{
			get
			{
				return this._parameterType;
			}
			internal set
			{
				this._parameterType = value;
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000914 RID: 2324 RVA: 0x00028198 File Offset: 0x00027198
		// (set) Token: 0x06000915 RID: 2325 RVA: 0x000281A0 File Offset: 0x000271A0
		public int Param1
		{
			get
			{
				return this._param1;
			}
			internal set
			{
				this._param1 = value;
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06000916 RID: 2326 RVA: 0x000281A9 File Offset: 0x000271A9
		// (set) Token: 0x06000917 RID: 2327 RVA: 0x000281B1 File Offset: 0x000271B1
		public IntPtr Param2
		{
			get
			{
				return this._param2;
			}
			internal set
			{
				this._param2 = value;
			}
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x000281BA File Offset: 0x000271BA
		public SpeechEventInfo(short eventId, short parameterType, int param1, IntPtr param2)
		{
			this._eventId = eventId;
			this._parameterType = parameterType;
			this._param1 = param1;
			this._param2 = param2;
		}

		// Token: 0x06000919 RID: 2329 RVA: 0x000281DC File Offset: 0x000271DC
		public static bool operator ==(SpeechEventInfo event1, SpeechEventInfo event2)
		{
			return event1.EventId == event2.EventId && event1.ParameterType == event2.ParameterType && event1.Param1 == event2.Param1 && event1.Param2 == event2.Param2;
		}

		// Token: 0x0600091A RID: 2330 RVA: 0x0002822E File Offset: 0x0002722E
		public static bool operator !=(SpeechEventInfo event1, SpeechEventInfo event2)
		{
			return !(event1 == event2);
		}

		// Token: 0x0600091B RID: 2331 RVA: 0x0002823A File Offset: 0x0002723A
		public bool Equals(SpeechEventInfo other)
		{
			return this == other;
		}

		// Token: 0x0600091C RID: 2332 RVA: 0x00028248 File Offset: 0x00027248
		public override bool Equals(object obj)
		{
			return obj is SpeechEventInfo && this.Equals((SpeechEventInfo)obj);
		}

		// Token: 0x0600091D RID: 2333 RVA: 0x00028260 File Offset: 0x00027260
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x040006C6 RID: 1734
		private short _eventId;

		// Token: 0x040006C7 RID: 1735
		private short _parameterType;

		// Token: 0x040006C8 RID: 1736
		private int _param1;

		// Token: 0x040006C9 RID: 1737
		private IntPtr _param2;
	}
}
