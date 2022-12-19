using System;
using UnityEngine;
using UnityEngine.UI;

namespace BloodMagic.UI
{
	// Token: 0x02000020 RID: 32
	public class AbilityInfo : MonoBehaviour
	{
		// Token: 0x0600009F RID: 159 RVA: 0x000049A8 File Offset: 0x00002BA8
		private void Awake()
		{
			BookUIHandler.AbilityInfo = this;
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x000049B0 File Offset: 0x00002BB0
		public void OnUnlockClicked()
		{
			bool flag = !(this.selectedSkillData != null) || this.selectedSkillData.unlocked;
			if (!flag)
			{
				bool flag2 = this.selectedSkillData.GetComponentInParent<SkillTreeInfo>().skillTreeName == "Light";
				if (flag2)
				{
					bool flag3 = BookUIHandler.saveData.lightPoints >= this.selectedSkillData.cost && this.UnlockSkill();
					if (flag3)
					{
						BookUIHandler.saveData.lightPoints -= this.selectedSkillData.cost;
						this.button.interactable = false;
						this.button.GetComponentInChildren<Text>().text = "Unlocked";
						BookUIHandler.Instance.UpdateSkilltree(BookUIHandler.Instance.lightSkillTree);
						BookUIHandler.Instance.SaveJson();
					}
				}
				else
				{
					bool flag4 = this.selectedSkillData.GetComponentInParent<SkillTreeInfo>().skillTreeName == "Dark" && BookUIHandler.saveData.darkPoints >= this.selectedSkillData.cost && this.UnlockSkill();
					if (flag4)
					{
						BookUIHandler.saveData.darkPoints -= this.selectedSkillData.cost;
						this.button.interactable = false;
						this.button.GetComponentInChildren<Text>().text = "Unlocked";
						BookUIHandler.Instance.UpdateSkilltree(BookUIHandler.Instance.darkSkillTree);
						BookUIHandler.Instance.SaveJson();
					}
				}
			}
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00004B34 File Offset: 0x00002D34
		private bool UnlockSkill()
		{
			bool flag = !BookUIHandler.saveData.unlockedSkills.Contains(this.selectedSkillData.skillName);
			bool flag2;
			if (flag)
			{
				BookUIHandler.saveData.unlockedSkills.Add(this.selectedSkillData.skillName);
				flag2 = true;
			}
			else
			{
				Debug.LogWarning(base.GetType().FullName + " :: Trying to unlock skill that is already unlocked");
				flag2 = false;
			}
			return flag2;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00004BA4 File Offset: 0x00002DA4
		public void SetSelectedSkill(SkillData p_skilldata, bool lightSide)
		{
			BookUIHandler.AbilityInfo.gameObject.SetActive(true);
			this.selectedSkillData = p_skilldata;
			bool unlocked = p_skilldata.unlocked;
			if (unlocked)
			{
				this.button.interactable = false;
				this.button.GetComponentInChildren<Text>().text = "Unlocked";
			}
			else if (lightSide)
			{
				bool flag = BookUIHandler.saveData.lightPoints >= this.selectedSkillData.cost;
				if (flag)
				{
					this.button.GetComponentInChildren<Text>().text = "Unlock";
					this.button.interactable = true;
				}
				else
				{
					this.button.GetComponentInChildren<Text>().text = "Not enough points";
					this.button.interactable = false;
				}
			}
			else
			{
				bool flag2 = !lightSide;
				if (flag2)
				{
					bool flag3 = BookUIHandler.saveData.darkPoints >= this.selectedSkillData.cost;
					if (flag3)
					{
						this.button.GetComponentInChildren<Text>().text = "Unlock";
						this.button.interactable = true;
					}
					else
					{
						this.button.GetComponentInChildren<Text>().text = "Not enough points";
						this.button.interactable = false;
					}
				}
			}
			this.title.text = p_skilldata.skillName;
			this.description.text = p_skilldata.description;
			if (lightSide)
			{
				this.cost.text = string.Format("{0} LP", p_skilldata.cost);
			}
			else
			{
				this.cost.text = string.Format("{0} DP", p_skilldata.cost);
			}
		}

		// Token: 0x0400003F RID: 63
		public SkillData selectedSkillData;

		// Token: 0x04000040 RID: 64
		public Text title;

		// Token: 0x04000041 RID: 65
		public Text description;

		// Token: 0x04000042 RID: 66
		public Text cost;

		// Token: 0x04000043 RID: 67
		public Button button;
	}
}
