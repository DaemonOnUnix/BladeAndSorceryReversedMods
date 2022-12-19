using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Speech.Internal.SapiInterop;
using System.Speech.Synthesis.TtsEngine;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000B6 RID: 182
	[ComVisible(true)]
	internal class EngineSiteSapi : ISpEngineSite
	{
		// Token: 0x06000622 RID: 1570 RVA: 0x000185FA File Offset: 0x000167FA
		internal EngineSiteSapi(EngineSite site, ResourceLoader resourceLoader)
		{
			this._site = site;
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x0001860C File Offset: 0x0001680C
		void ISpEngineSite.AddEvents([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] SpeechEventSapi[] eventsSapi, int ulCount)
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

		// Token: 0x06000624 RID: 1572 RVA: 0x00018692 File Offset: 0x00016892
		void ISpEngineSite.GetEventInterest(out long eventInterest)
		{
			eventInterest = (long)((ulong)this._site.EventInterest);
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x000186A2 File Offset: 0x000168A2
		[PreserveSig]
		int ISpEngineSite.GetActions()
		{
			return this._site.Actions;
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x000186AF File Offset: 0x000168AF
		void ISpEngineSite.Write(IntPtr pBuff, int cb, IntPtr pcbWritten)
		{
			pcbWritten = (IntPtr)this._site.Write(pBuff, cb);
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x000186C5 File Offset: 0x000168C5
		void ISpEngineSite.GetRate(out int pRateAdjust)
		{
			pRateAdjust = this._site.Rate;
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x000186D4 File Offset: 0x000168D4
		void ISpEngineSite.GetVolume(out short pusVolume)
		{
			pusVolume = (short)this._site.Volume;
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x000186E4 File Offset: 0x000168E4
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

		// Token: 0x0600062A RID: 1578 RVA: 0x00018717 File Offset: 0x00016917
		void ISpEngineSite.CompleteSkip(int ulNumSkipped)
		{
			this._site.CompleteSkip(ulNumSkipped);
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x00018728 File Offset: 0x00016928
		void ISpEngineSite.LoadResource(string uri, ref string mediaType, out IStream stream)
		{
			mediaType = null;
			try
			{
				Stream stream2 = this._site.LoadResource(new Uri(uri, UriKind.RelativeOrAbsolute), mediaType);
				BinaryReader binaryReader = new BinaryReader(stream2);
				byte[] waveFormat = AudioBase.GetWaveFormat(binaryReader);
				mediaType = null;
				if (waveFormat != null)
				{
					WAVEFORMATEX waveformatex = WAVEFORMATEX.ToWaveHeader(waveFormat);
					EngineSiteSapi.WaveFormatId wFormatTag = (EngineSiteSapi.WaveFormatId)waveformatex.wFormatTag;
					if (wFormatTag == EngineSiteSapi.WaveFormatId.Pcm || wFormatTag - EngineSiteSapi.WaveFormatId.Alaw <= 1)
					{
						mediaType = "audio/x-wav";
					}
				}
				stream2.Position = 0L;
				stream = new SpStreamWrapper(stream2);
			}
			catch
			{
				stream = null;
			}
		}

		// Token: 0x040004AB RID: 1195
		private EngineSite _site;

		// Token: 0x02000195 RID: 405
		private enum WaveFormatId
		{
			// Token: 0x0400093B RID: 2363
			Pcm = 1,
			// Token: 0x0400093C RID: 2364
			Alaw = 6,
			// Token: 0x0400093D RID: 2365
			Mulaw
		}
	}
}
