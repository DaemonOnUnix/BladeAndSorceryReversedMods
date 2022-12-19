using System;
using System.ComponentModel;

namespace System.Speech.Synthesis.TtsEngine
{
	// Token: 0x0200002D RID: 45
	[ImmutableObject(true)]
	public struct SpeechEventInfo : IEquatable<SpeechEventInfo>
	{
		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x000049D1 File Offset: 0x00002BD1
		// (set) Token: 0x060000C7 RID: 199 RVA: 0x000049D9 File Offset: 0x00002BD9
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

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x000049E2 File Offset: 0x00002BE2
		// (set) Token: 0x060000C9 RID: 201 RVA: 0x000049EA File Offset: 0x00002BEA
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

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000CA RID: 202 RVA: 0x000049F3 File Offset: 0x00002BF3
		// (set) Token: 0x060000CB RID: 203 RVA: 0x000049FB File Offset: 0x00002BFB
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

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000CC RID: 204 RVA: 0x00004A04 File Offset: 0x00002C04
		// (set) Token: 0x060000CD RID: 205 RVA: 0x00004A0C File Offset: 0x00002C0C
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

		// Token: 0x060000CE RID: 206 RVA: 0x00004A15 File Offset: 0x00002C15
		public SpeechEventInfo(short eventId, short parameterType, int param1, IntPtr param2)
		{
			this._eventId = eventId;
			this._parameterType = parameterType;
			this._param1 = param1;
			this._param2 = param2;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00004A34 File Offset: 0x00002C34
		public static bool operator ==(SpeechEventInfo event1, SpeechEventInfo event2)
		{
			return event1.EventId == event2.EventId && event1.ParameterType == event2.ParameterType && event1.Param1 == event2.Param1 && event1.Param2 == event2.Param2;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00004A86 File Offset: 0x00002C86
		public static bool operator !=(SpeechEventInfo event1, SpeechEventInfo event2)
		{
			return !(event1 == event2);
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00004A92 File Offset: 0x00002C92
		public bool Equals(SpeechEventInfo other)
		{
			return this == other;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00004AA0 File Offset: 0x00002CA0
		public override bool Equals(object obj)
		{
			return obj is SpeechEventInfo && this.Equals((SpeechEventInfo)obj);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00004AB8 File Offset: 0x00002CB8
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x0400024C RID: 588
		private short _eventId;

		// Token: 0x0400024D RID: 589
		private short _parameterType;

		// Token: 0x0400024E RID: 590
		private int _param1;

		// Token: 0x0400024F RID: 591
		private IntPtr _param2;
	}
}
