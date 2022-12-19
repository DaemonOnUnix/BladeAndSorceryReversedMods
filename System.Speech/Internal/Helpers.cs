using System;
using System.Globalization;
using System.IO;

namespace System.Speech.Internal
{
	// Token: 0x0200000D RID: 13
	internal static class Helpers
	{
		// Token: 0x06000029 RID: 41 RVA: 0x00002F34 File Offset: 0x00001F34
		internal static void ThrowIfEmptyOrNull(string s, string paramName)
		{
			if (!string.IsNullOrEmpty(s))
			{
				return;
			}
			if (s == null)
			{
				throw new ArgumentNullException(paramName);
			}
			throw new ArgumentException(SR.Get(SRID.StringCanNotBeEmpty, new object[] { paramName }), paramName);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002F6C File Offset: 0x00001F6C
		internal static void ThrowIfNull(object value, string paramName)
		{
			if (value == null)
			{
				throw new ArgumentNullException(paramName);
			}
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002F78 File Offset: 0x00001F78
		internal static bool CompareInvariantCulture(CultureInfo culture1, CultureInfo culture2)
		{
			if (culture1.Equals(culture2))
			{
				return true;
			}
			while (!culture1.IsNeutralCulture)
			{
				culture1 = culture1.Parent;
			}
			while (!culture2.IsNeutralCulture)
			{
				culture2 = culture2.Parent;
			}
			return culture1.Equals(culture2);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002FB0 File Offset: 0x00001FB0
		internal static void CopyStream(Stream inputStream, Stream outputStream, int bytesToCopy)
		{
			int num = ((bytesToCopy > 4096) ? 4096 : bytesToCopy);
			byte[] array = new byte[num];
			while (bytesToCopy > 0)
			{
				int num2 = inputStream.Read(array, 0, num);
				if (num2 <= 0)
				{
					throw new EndOfStreamException(SR.Get(SRID.StreamEndedUnexpectedly, new object[0]));
				}
				outputStream.Write(array, 0, num2);
				bytesToCopy -= num2;
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x0000300C File Offset: 0x0000200C
		internal static byte[] ReadStreamToByteArray(Stream inputStream, int bytesToCopy)
		{
			byte[] array = new byte[bytesToCopy];
			Helpers.BlockingRead(inputStream, array, 0, bytesToCopy);
			return array;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x0000302C File Offset: 0x0000202C
		internal static void BlockingRead(Stream stream, byte[] buffer, int offset, int count)
		{
			while (count > 0)
			{
				int num = stream.Read(buffer, offset, count);
				if (num <= 0)
				{
					throw new EndOfStreamException();
				}
				count -= num;
				offset += num;
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00003060 File Offset: 0x00002060
		internal static void CombineCulture(string language, string location, CultureInfo parentCulture, int layoutId)
		{
			CultureInfo cultureInfo = new CultureInfo(language);
			RegionInfo regionInfo = new RegionInfo(location);
			string text = cultureInfo.TwoLetterISOLanguageName + "-" + regionInfo.TwoLetterISORegionName;
			CultureAndRegionInfoBuilder cultureAndRegionInfoBuilder = new CultureAndRegionInfoBuilder(text, 0);
			cultureAndRegionInfoBuilder.LoadDataFromRegionInfo(regionInfo);
			cultureAndRegionInfoBuilder.LoadDataFromCultureInfo(cultureInfo);
			cultureAndRegionInfoBuilder.IetfLanguageTag = text;
			cultureAndRegionInfoBuilder.Parent = parentCulture;
			cultureAndRegionInfoBuilder.ThreeLetterISOLanguageName = cultureInfo.ThreeLetterISOLanguageName;
			cultureAndRegionInfoBuilder.TwoLetterISOLanguageName = cultureInfo.TwoLetterISOLanguageName;
			cultureAndRegionInfoBuilder.ThreeLetterWindowsLanguageName = cultureInfo.ThreeLetterWindowsLanguageName;
			cultureAndRegionInfoBuilder.RegionNativeName = regionInfo.NativeName;
			string text2 = cultureInfo.EnglishName.Substring(0, cultureInfo.EnglishName.IndexOf("(", 4) - 1);
			string text3 = cultureInfo.NativeName.Substring(0, cultureInfo.NativeName.IndexOf("(", 4) - 1);
			cultureAndRegionInfoBuilder.CultureEnglishName = text2 + " (" + cultureAndRegionInfoBuilder.RegionEnglishName + ")";
			cultureAndRegionInfoBuilder.CultureNativeName = text3 + " (" + cultureAndRegionInfoBuilder.RegionNativeName + ")";
			cultureAndRegionInfoBuilder.CurrencyNativeName = regionInfo.CurrencyNativeName;
			cultureAndRegionInfoBuilder.NumberFormat.PositiveInfinitySymbol = cultureInfo.NumberFormat.PositiveInfinitySymbol;
			cultureAndRegionInfoBuilder.NumberFormat.NegativeInfinitySymbol = cultureInfo.NumberFormat.NegativeInfinitySymbol;
			cultureAndRegionInfoBuilder.NumberFormat.NaNSymbol = cultureInfo.NumberFormat.NaNSymbol;
			DateTimeFormatInfo gregorianDateTimeFormat = cultureAndRegionInfoBuilder.GregorianDateTimeFormat;
			DateTimeFormatInfo dateTimeFormat = cultureInfo.DateTimeFormat;
			dateTimeFormat.Calendar = new GregorianCalendar();
			cultureAndRegionInfoBuilder.KeyboardLayoutId = layoutId;
			gregorianDateTimeFormat.AbbreviatedDayNames = dateTimeFormat.AbbreviatedDayNames;
			gregorianDateTimeFormat.AbbreviatedMonthGenitiveNames = dateTimeFormat.AbbreviatedMonthGenitiveNames;
			gregorianDateTimeFormat.AbbreviatedMonthNames = dateTimeFormat.AbbreviatedMonthNames;
			gregorianDateTimeFormat.DayNames = dateTimeFormat.DayNames;
			gregorianDateTimeFormat.MonthGenitiveNames = dateTimeFormat.MonthGenitiveNames;
			gregorianDateTimeFormat.MonthNames = dateTimeFormat.MonthNames;
			gregorianDateTimeFormat.ShortestDayNames = dateTimeFormat.ShortestDayNames;
			cultureAndRegionInfoBuilder.Register();
		}

		// Token: 0x04000073 RID: 115
		internal const int _sizeOfChar = 2;

		// Token: 0x04000074 RID: 116
		internal static readonly char[] _achTrimChars = new char[] { ' ', '\t', '\n', '\r' };
	}
}
