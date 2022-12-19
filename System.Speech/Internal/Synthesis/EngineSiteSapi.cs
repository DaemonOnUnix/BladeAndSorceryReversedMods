using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Speech.Internal.SapiInterop;
using System.Speech.Synthesis.TtsEngine;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000E7 RID: 231
	[ComVisible(true)]
	internal class EngineSiteSapi : ISpEngineSite
	{
		// Token: 0x0600054B RID: 1355 RVA: 0x0001729E File Offset: 0x0001629E
		internal EngineSiteSapi(EngineSite site, ResourceLoader resourceLoader)
		{
			this._site = site;
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x000172B0 File Offset: 0x000162B0
		void ISpEngineSite.AddEvents([MarshalAs(42, SizeParamIndex = 1)] SpeechEventSapi[] eventsSapi, int ulCount)
		{
			SpeechEventInfo[] array = new SpeechEventInfo[eventsSapi.Length];
			for (int i = 0; i < eventsSapi.Length; i++)
			{
				SpeechEventSapi speechEventSapi = eventsSapi[i];
				array[i].EventId = speechEventSapi.EventId;
				array[i].ParameterType = speechEventSapi.ParameterType;
				array[i].Param1 = (int)speechEventSapi.Param1;
				array[i].Param2 = speechEventSapi.Param2;
			}
			this._site.AddEvents(array, ulCount);
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x0001733F File Offset: 0x0001633F
		void ISpEngineSite.GetEventInterest(out long eventInterest)
		{
			eventInterest = (long)((ulong)this._site.EventInterest);
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x0001734F File Offset: 0x0001634F
		[PreserveSig]
		int ISpEngineSite.GetActions()
		{
			return this._site.Actions;
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x0001735C File Offset: 0x0001635C
		void ISpEngineSite.Write(IntPtr pBuff, int cb, IntPtr pcbWritten)
		{
			pcbWritten = (IntPtr)this._site.Write(pBuff, cb);
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x00017372 File Offset: 0x00016372
		void ISpEngineSite.GetRate(out int pRateAdjust)
		{
			pRateAdjust = this._site.Rate;
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x00017381 File Offset: 0x00016381
		void ISpEngineSite.GetVolume(out short pusVolume)
		{
			pusVolume = (short)this._site.Volume;
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x00017394 File Offset: 0x00016394
		void ISpEngineSite.GetSkipInfo(out int peType, out int plNumItems)
		{
			SkipInfo skipInfo = this._site.GetSkipInfo();
			if (skipInfo != null)
			{
				peType = skipInfo.Type;
				plNumItems = skipInfo.Count;
				return;
			}
			peType = 1;
			plNumItems = 0;
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x000173C7 File Offset: 0x000163C7
		void ISpEngineSite.CompleteSkip(int ulNumSkipped)
		{
			this._site.CompleteSkip(ulNumSkipped);
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x000173D8 File Offset: 0x000163D8
		void ISpEngineSite.LoadResource(string uri, ref string mediaType, out IStream stream)
		{
			mediaType = null;
			try
			{
				Stream stream2 = this._site.LoadResource(new Uri(uri, 0), mediaType);
				BinaryReader binaryReader = new BinaryReader(stream2);
				byte[] waveFormat = AudioBase.GetWaveFormat(binaryReader);
				mediaType = null;
				if (waveFormat != null)
				{
					EngineSiteSapi.WaveFormatId wFormatTag = (EngineSiteSapi.WaveFormatId)WAVEFORMATEX.ToWaveHeader(waveFormat).wFormatTag;
					if (wFormatTag != EngineSiteSapi.WaveFormatId.Pcm)
					{
						switch (wFormatTag)
						{
						case EngineSiteSapi.WaveFormatId.Alaw:
						case EngineSiteSapi.WaveFormatId.Mulaw:
							break;
						default:
							goto IL_5B;
						}
					}
					mediaType = "audio/x-wav";
				}
				IL_5B:
				stream2.Position = 0L;
				stream = new SpStreamWrapper(stream2);
			}
			catch
			{
				stream = null;
			}
		}

		// Token: 0x0400041C RID: 1052
		private EngineSite _site;

		// Token: 0x020000E8 RID: 232
		private enum WaveFormatId
		{
			// Token: 0x0400041E RID: 1054
			Pcm = 1,
			// Token: 0x0400041F RID: 1055
			Alaw = 6,
			// Token: 0x04000420 RID: 1056
			Mulaw
		}
	}
}
