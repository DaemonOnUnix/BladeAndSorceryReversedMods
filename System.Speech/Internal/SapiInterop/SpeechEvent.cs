using System;
using System.Runtime.InteropServices;
using System.Speech.AudioFormat;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000095 RID: 149
	internal class SpeechEvent : IDisposable
	{
		// Token: 0x060002DE RID: 734 RVA: 0x00009CCC File Offset: 0x00008CCC
		private SpeechEvent(SPEVENTENUM eEventId, SPEVENTLPARAMTYPE elParamType, ulong ullAudioStreamOffset, IntPtr wParam, IntPtr lParam)
		{
			this._eventId = eEventId;
			this._paramType = elParamType;
			this._audioStreamOffset = ullAudioStreamOffset;
			this._wParam = (ulong)wParam.ToInt64();
			this._lParam = (ulong)(long)lParam;
			if (this._paramType == SPEVENTLPARAMTYPE.SPET_LPARAM_IS_POINTER || this._paramType == SPEVENTLPARAMTYPE.SPET_LPARAM_IS_STRING)
			{
				GC.AddMemoryPressure((long)(this._sizeMemoryPressure = Marshal.SizeOf(this._lParam)));
			}
		}

		// Token: 0x060002DF RID: 735 RVA: 0x00009D40 File Offset: 0x00008D40
		private SpeechEvent(SPEVENT sapiEvent, SpeechAudioFormatInfo audioFormat)
			: this(sapiEvent.eEventId, sapiEvent.elParamType, sapiEvent.ullAudioStreamOffset, sapiEvent.wParam, sapiEvent.lParam)
		{
			if (audioFormat == null || audioFormat.EncodingFormat == (EncodingFormat)0)
			{
				this._audioPosition = TimeSpan.Zero;
				return;
			}
			this._audioPosition = ((audioFormat.AverageBytesPerSecond > 0) ? new TimeSpan((long)(sapiEvent.ullAudioStreamOffset * 10000000UL / (ulong)((long)audioFormat.AverageBytesPerSecond))) : TimeSpan.Zero);
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x00009DBE File Offset: 0x00008DBE
		private SpeechEvent(SPEVENTEX sapiEventEx)
			: this(sapiEventEx.eEventId, sapiEventEx.elParamType, sapiEventEx.ullAudioStreamOffset, sapiEventEx.wParam, sapiEventEx.lParam)
		{
			this._audioPosition = new TimeSpan((long)sapiEventEx.ullAudioTimeOffset);
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x00009DFC File Offset: 0x00008DFC
		~SpeechEvent()
		{
			this.Dispose();
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x00009E28 File Offset: 0x00008E28
		public void Dispose()
		{
			if (this._lParam != 0UL)
			{
				if (this._paramType == SPEVENTLPARAMTYPE.SPET_LPARAM_IS_TOKEN || this._paramType == SPEVENTLPARAMTYPE.SPET_LPARAM_IS_OBJECT)
				{
					Marshal.Release((IntPtr)((long)this._lParam));
				}
				else if (this._paramType == SPEVENTLPARAMTYPE.SPET_LPARAM_IS_POINTER || this._paramType == SPEVENTLPARAMTYPE.SPET_LPARAM_IS_STRING)
				{
					Marshal.FreeCoTaskMem((IntPtr)((long)this._lParam));
				}
				if (this._sizeMemoryPressure > 0)
				{
					GC.RemoveMemoryPressure((long)this._sizeMemoryPressure);
					this._sizeMemoryPressure = 0;
				}
				this._lParam = 0UL;
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x00009EB0 File Offset: 0x00008EB0
		internal static SpeechEvent TryCreateSpeechEvent(ISpEventSource sapiEventSource, bool additionalSapiFeatures, SpeechAudioFormatInfo audioFormat)
		{
			SpeechEvent speechEvent = null;
			if (additionalSapiFeatures)
			{
				SPEVENTEX speventex;
				uint num;
				((ISpEventSource2)sapiEventSource).GetEventsEx(1U, out speventex, out num);
				if (num == 1U)
				{
					speechEvent = new SpeechEvent(speventex);
				}
			}
			else
			{
				uint num;
				SPEVENT spevent;
				sapiEventSource.GetEvents(1U, out spevent, out num);
				if (num == 1U)
				{
					speechEvent = new SpeechEvent(spevent, audioFormat);
				}
			}
			return speechEvent;
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060002E4 RID: 740 RVA: 0x00009EF7 File Offset: 0x00008EF7
		internal SPEVENTENUM EventId
		{
			get
			{
				return this._eventId;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060002E5 RID: 741 RVA: 0x00009EFF File Offset: 0x00008EFF
		internal ulong AudioStreamOffset
		{
			get
			{
				return this._audioStreamOffset;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060002E6 RID: 742 RVA: 0x00009F07 File Offset: 0x00008F07
		internal ulong WParam
		{
			get
			{
				return this._wParam;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060002E7 RID: 743 RVA: 0x00009F0F File Offset: 0x00008F0F
		internal ulong LParam
		{
			get
			{
				return this._lParam;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060002E8 RID: 744 RVA: 0x00009F17 File Offset: 0x00008F17
		internal TimeSpan AudioPosition
		{
			get
			{
				return this._audioPosition;
			}
		}

		// Token: 0x040002BE RID: 702
		private SPEVENTENUM _eventId;

		// Token: 0x040002BF RID: 703
		private SPEVENTLPARAMTYPE _paramType;

		// Token: 0x040002C0 RID: 704
		private ulong _audioStreamOffset;

		// Token: 0x040002C1 RID: 705
		private ulong _wParam;

		// Token: 0x040002C2 RID: 706
		private ulong _lParam;

		// Token: 0x040002C3 RID: 707
		private TimeSpan _audioPosition;

		// Token: 0x040002C4 RID: 708
		private int _sizeMemoryPressure;
	}
}
