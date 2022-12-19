using System;
using UnityEngine;

namespace BloodMagic.UI
{
	// Token: 0x02000024 RID: 36
	public static class SkillHandler
	{
		// Token: 0x060000C1 RID: 193 RVA: 0x00005B40 File Offset: 0x00003D40
		public static SkillData ShowSkill(this SkillData skillData)
		{
			bool flag = skillData.cost == -1;
			SkillData skillData2;
			if (flag)
			{
				skillData2 = skillData;
			}
			else
			{
				skillData.gameObject.SetActive(true);
				Debug.Log(string.Format("ShowSkill :: Does save data contain {0} = {1}", skillData.skillName, BookUIHandler.saveData.unlockedSkills.Contains(skillData.skillName)));
				bool flag2 = BookUIHandler.saveData.unlockedSkills.Contains(skillData.skillName);
				if (flag2)
				{
					skillData.ringIMG.color = new Color(0f, 1f, 0f);
					skillData.unlocked = true;
				}
				else
				{
					skillData.ringIMG.color = new Color(1f, 0f, 0f);
					skillData.unlocked = false;
				}
				skillData2 = skillData;
			}
			return skillData2;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00005C14 File Offset: 0x00003E14
		public static SkillData HideSkill(this SkillData skillData)
		{
			skillData.gameObject.SetActive(false);
			return skillData;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00005C34 File Offset: 0x00003E34
		public static bool IsSkillUnlocked(string skillName)
		{
			return BookUIHandler.saveData.unlockedSkills.Contains(skillName);
		}
	}
}
