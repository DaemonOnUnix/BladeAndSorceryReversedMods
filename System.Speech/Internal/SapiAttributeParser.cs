using System;
using System.Collections.Generic;
using System.Globalization;
using System.Speech.AudioFormat;

namespace System.Speech.Internal
{
	// Token: 0x0200001C RID: 28
	internal static class SapiAttributeParser
	{
		// Token: 0x06000089 RID: 137 RVA: 0x000060A8 File Offset: 0x000050A8
		internal static CultureInfo GetCultureInfoFromLanguageString(string valueString)
		{
			string[] array = valueString.Split(new char[] { ';' });
			string text = array[0].Trim();
			if (!string.IsNullOrEmpty(text))
			{
				try
				{
					return new CultureInfo(int.Parse(text, 515, CultureInfo.InvariantCulture), false);
				}
				catch (ArgumentException)
				{
					return null;
				}
			}
			return null;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x0000610C File Offset: 0x0000510C
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
