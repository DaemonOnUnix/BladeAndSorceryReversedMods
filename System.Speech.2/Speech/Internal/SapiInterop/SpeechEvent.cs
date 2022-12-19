using System;
using System.Runtime.InteropServices;
using System.Speech.AudioFormat;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000168 RID: 360
	internal class SpeechEvent : IDisposable
	{
		// Token: 0x06000AD3 RID: 2771 RVA: 0x0002BED4 File Offset: 0x0002A0D4
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

		// Token: 0x06000AD4 RID: 2772 RVA: 0x0002BF48 File Offset: 0x0002A148
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

		// Token: 0x06000AD5 RID: 2773 RVA: 0x0002BFC0 File Offset: 0x0002A1C0
		private SpeechEvent(SPEVENTEX sapiEventEx)
			: this(sapiEventEx.eEventId, sapiEventEx.elParamType, sapiEventEx.ullAudioStreamOffset, sapiEventEx.wParam, sapiEventEx.lParam)
		{
			this._audioPosition = new TimeSpan((long)sapiEventEx.ullAudioTimeOffset);
		}

		// Token: 0x06000AD6 RID: 2774 RVA: 0x0002BFF8 File Offset: 0x0002A1F8
		~SpeechEvent()
		{
			this.Dispose();
		}

		// Token: 0x06000AD7 RID: 2775 RVA: 0x0002C024 File Offset: 0x0002A224
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

		// Token: 0x06000AD8 RID: 2776 RVA: 0x0002C0AC File Offset: 0x0002A2AC
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

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000AD9 RID: 2777 RVA: 0x0002C0F3 File Offset: 0x0002A2F3
		internal SPEVENTENUM EventId
		{
			get
			{
				return this._eventId;
			}
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000ADA RID: 2778 RVA: 0x0002C0FB File Offset: 0x0002A2FB
		internal ulong AudioStreamOffset
		{
			get
			{
				return this._audioStreamOffset;
			}
		}

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000ADB RID: 2779 RVA: 0x0002C103 File Offset: 0x0002A303
		internal ulong WParam
		{
			get
			{
				return this._wParam;
			}
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000ADC RID: 2780 RVA: 0x0002C10B File Offset: 0x0002A30B
		internal ulong LParam
		{
			get
			{
				return this._lParam;
			}
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06000ADD RID: 2781 RVA: 0x0002C113 File Offset: 0x0002A313
		internal TimeSpan AudioPosition
		{
			get
			{
				return this._audioPosition;
			}
		}

		// Token: 0x04000827 RID: 2087
		private SPEVENTENUM _eventId;

		// Token: 0x04000828 RID: 2088
		private SPEVENTLPARAMTYPE _paramType;

		// Token: 0x04000829 RID: 2089
		private ulong _audioStreamOffset;

		// Token: 0x0400082A RID: 2090
		private ulong _wParam;

		// Token: 0x0400082B RID: 2091
		private ulong _lParam;

		// Token: 0x0400082C RID: 2092
		private TimeSpan _audioPosition;

		// Token: 0x0400082D RID: 2093
		private int _sizeMemoryPressure;
	}
}
