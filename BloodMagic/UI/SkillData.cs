using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BloodMagic.UI
{
	// Token: 0x02000023 RID: 35
	public class SkillData : MonoBehaviour
	{
		// Token: 0x060000BF RID: 191 RVA: 0x00005AD4 File Offset: 0x00003CD4
		public void SkillBTNClicked()
		{
			BookUIHandler.AbilityInfo.gameObject.SetActive(true);
			bool flag = base.GetComponentInParent<SkillTreeInfo>();
			if (flag)
			{
				BookUIHandler.AbilityInfo.SetSelectedSkill(this, base.GetComponentInParent<SkillTreeInfo>().skillTreeName == "Light");
			}
			Debug.Log("SkillBTN pressed");
		}

		// Token: 0x0400005A RID: 90
		public int cost = 1;

		// Token: 0x0400005B RID: 91
		public string skillName;

		// Token: 0x0400005C RID: 92
		public Image ringIMG;

		// Token: 0x0400005D RID: 93
		public List<SkillData> required;

		// Token: 0x0400005E RID: 94
		public string description;

		// Token: 0x0400005F RID: 95
		public bool unlocked;
	}
}
