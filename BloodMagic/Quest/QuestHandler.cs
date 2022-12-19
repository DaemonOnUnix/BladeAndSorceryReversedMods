using System;
using BloodMagic.Quest.Conditions;
using BloodMagic.UI;

namespace BloodMagic.Quest
{
	// Token: 0x02000005 RID: 5
	public static class QuestHandler
	{
		// Token: 0x0600002A RID: 42 RVA: 0x000023F4 File Offset: 0x000005F4
		public static void ShowQuestData(this QuestUIComponets questData, Quest quest)
		{
			string text = quest.mainCondition.conditionText;
			foreach (Condition condition in quest.mainCondition.conditions)
			{
				text = text + " " + condition.conditionText;
			}
			questData.questInfoText.text = text;
			questData.progressText.text = string.Format("{0} / {1}", Math.Round((double)quest.mainCondition.progress, 1), quest.mainCondition.progessGoal);
			string text2 = "";
			for (int i = 0; i < quest.mainCondition.rewards.Count; i++)
			{
				text2 = ((i == 0) ? (text2 + quest.mainCondition.rewards[i].rewardText + " ") : (text2 + "& " + quest.mainCondition.rewards[i].rewardText));
			}
			bool completed = quest.completed;
			if (completed)
			{
				questData.claimBTN.gameObject.SetActive(true);
			}
			else
			{
				questData.claimBTN.gameObject.SetActive(false);
			}
			questData.xpRewardText.text = text2;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002568 File Offset: 0x00000768
		public static SaveData.QuestInfo GetQuestInfo(this Quest quest)
		{
			return new SaveData.QuestInfo
			{
				id = quest.id,
				level = quest.mainCondition.level,
				progress = quest.mainCondition.progress
			};
		}

		// Token: 0x0400001C RID: 28
		public static Random globalRandom;
	}
}
