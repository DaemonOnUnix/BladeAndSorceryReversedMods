using System;
using System.Collections.Generic;
using System.Globalization;
using System.Speech.AudioFormat;

namespace System.Speech.Internal
{
	// Token: 0x02000096 RID: 150
	internal static class SapiAttributeParser
	{
		// Token: 0x060004F5 RID: 1269 RVA: 0x000141D8 File Offset: 0x000123D8
		internal static CultureInfo GetCultureInfoFromLanguageString(string valueString)
		{
			string[] array = valueString.Split(new char[] { ';' });
			string text = array[0].Trim();
			if (!string.IsNullOrEmpty(text))
			{
				try
				{
					return new CultureInfo(int.Parse(text, NumberStyles.HexNumber, CultureInfo.InvariantCulture), false);
				}
				catch (ArgumentException)
				{
					return null;
				}
			}
			return null;
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x0001423C File Offset: 0x0001243C
		internal static List<SpeechAudioFormatInfo> GetAudioFormatsFromString(string valueString)
		{
			List<SpeechAudioFormatInfo> list = new List<SpeechAudioFormatInfo>();
			string[] array = valueString.Split(new char[] { ';' });
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i].Trim();
				if (!string.IsNullOrEmpty(text))
				{
					SpeechAudioFormatInfo speechAudioFormatInfo = AudioFormatConverter.ToSpeechAudioFormatInfo(text);
					if (speechAudioFormatInfo != null)
					{
						list.Add(speechAudioFormatInfo);
					}
				}
			}
			return list;
		}
	}
}
