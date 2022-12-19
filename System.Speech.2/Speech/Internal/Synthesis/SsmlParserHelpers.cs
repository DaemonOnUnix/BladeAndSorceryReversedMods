using System;
using System.Speech.Synthesis;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000C3 RID: 195
	internal static class SsmlParserHelpers
	{
		// Token: 0x0600068F RID: 1679 RVA: 0x0001BB80 File Offset: 0x00019D80
		internal static bool TryConvertAge(string sAge, out VoiceAge age)
		{
			bool flag = false;
			age = VoiceAge.NotSet;
			if (!(sAge == "child"))
			{
				if (!(sAge == "teenager") && !(sAge == "teen"))
				{
					if (!(sAge == "adult"))
					{
						if (sAge == "elder" || sAge == "senior")
						{
							age = VoiceAge.Senior;
						}
					}
					else
					{
						age = VoiceAge.Adult;
					}
				}
				else
				{
					age = VoiceAge.Teen;
				}
			}
			else
			{
				age = VoiceAge.Child;
			}
			int num;
			if (age != VoiceAge.NotSet)
			{
				flag = true;
			}
			else if (int.TryParse(sAge, out num))
			{
				if (num <= 12)
				{
					age = VoiceAge.Child;
				}
				else if (num <= 22)
				{
					age = VoiceAge.Teen;
				}
				else if (num <= 47)
				{
					age = VoiceAge.Adult;
				}
				else
				{
					age = VoiceAge.Senior;
				}
				flag = true;
			}
			return flag;
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x0001BC34 File Offset: 0x00019E34
		internal static bool TryConvertGender(string sGender, out VoiceGender gender)
		{
			bool flag = false;
			gender = VoiceGender.NotSet;
			int num = Array.BinarySearch<string>(SsmlParserHelpers._genderNames, sGender);
			if (num >= 0)
			{
				gender = SsmlParserHelpers._genders[num];
				flag = true;
			}
			return flag;
		}

		// Token: 0x04000510 RID: 1296
		private static readonly string[] _genderNames = new string[] { "female", "male", "neutral" };

		// Token: 0x04000511 RID: 1297
		private static readonly VoiceGender[] _genders = new VoiceGender[]
		{
			VoiceGender.Female,
			VoiceGender.Male,
			VoiceGender.Neutral
		};
	}
}
