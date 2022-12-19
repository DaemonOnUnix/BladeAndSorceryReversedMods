using System;
using System.Speech.Synthesis;

namespace System.Speech.Internal.Synthesis
{
	// Token: 0x020000FB RID: 251
	internal static class SsmlParserHelpers
	{
		// Token: 0x060005C2 RID: 1474 RVA: 0x0001AC4C File Offset: 0x00019C4C
		internal static bool TryConvertAge(string sAge, out VoiceAge age)
		{
			bool flag = false;
			age = VoiceAge.NotSet;
			if (sAge != null)
			{
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
			}
			int num;
			if (age != VoiceAge.NotSet)
			{
				flag = true;
			}
			else if (int.TryParse(sAge, ref num))
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

		// Token: 0x060005C3 RID: 1475 RVA: 0x0001AD04 File Offset: 0x00019D04
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

		// Token: 0x04000499 RID: 1177
		private static readonly string[] _genderNames = new string[] { "female", "male", "neutral" };

		// Token: 0x0400049A RID: 1178
		private static readonly VoiceGender[] _genders = new VoiceGender[]
		{
			VoiceGender.Female,
			VoiceGender.Male,
			VoiceGender.Neutral
		};
	}
}
