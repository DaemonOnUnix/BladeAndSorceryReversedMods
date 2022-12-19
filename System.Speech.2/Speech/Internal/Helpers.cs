using System;
using System.Globalization;
using System.IO;

namespace System.Speech.Internal
{
	// Token: 0x0200008F RID: 143
	internal static class Helpers
	{
		// Token: 0x060004B0 RID: 1200 RVA: 0x00012E18 File Offset: 0x00011018
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

		// Token: 0x060004B1 RID: 1201 RVA: 0x00012E43 File Offset: 0x00011043
		internal static void ThrowIfNull(object value, string paramName)
		{
			if (value == null)
			{
				throw new ArgumentNullException(paramName);
			}
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x00012E4F File Offset: 0x0001104F
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

		// Token: 0x060004B3 RID: 1203 RVA: 0x00012E88 File Offset: 0x00011088
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

		// Token: 0x060004B4 RID: 1204 RVA: 0x00012EE4 File Offset: 0x000110E4
		internal static byte[] ReadStreamToByteArray(Stream inputStream, int bytesToCopy)
		{
			byte[] array = new byte[bytesToCopy];
			Helpers.BlockingRead(inputStream, array, 0, bytesToCopy);
			return array;
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x00012F04 File Offset: 0x00011104
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

		// Token: 0x0400042B RID: 1067
		internal static readonly char[] _achTrimChars = new char[] { ' ', '\t', '\n', '\r' };

		// Token: 0x0400042C RID: 1068
		internal const int _sizeOfChar = 2;
	}
}
