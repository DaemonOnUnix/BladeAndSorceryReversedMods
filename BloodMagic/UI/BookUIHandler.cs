using System;
using System.IO;
using System.Linq;
using BloodMagic.Quest;
using BloodMagic.Quest.Conditions;
using BloodMagic.Spell;
using IngameDebugConsole;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ThunderRoad;
using UnityEngine;
using UnityEngine.UI;

namespace BloodMagic.UI
{
	// Token: 0x02000021 RID: 33
	public class BookUIHandler : MenuModule
	{
		// Token: 0x060000A4 RID: 164 RVA: 0x00004D58 File Offset: 0x00002F58
		public override void Init(MenuData menuData, Menu menu)
		{
			base.Init(menuData, menu);
			DebugLogConsole.AddCommand("reloadblood", "Reloads the blood save json", delegate()
			{
				this.InitSaveJSON();
				this.SetupAllQuests();
				this.UpdateAllStats();
			});
			BookUIHandler.Instance = this;
			this.questUI1 = menu.GetCustomReference("Quest1").GetComponent<QuestUIComponets>();
			this.questUI2 = menu.GetCustomReference("Quest2").GetComponent<QuestUIComponets>();
			this.questUI3 = menu.GetCustomReference("Quest3").GetComponent<QuestUIComponets>();
			this.darkChoicePage = menu.GetCustomReference("DarkChoice").gameObject;
			this.lightChoicePage = menu.GetCustomReference("LightChoice").gameObject;
			this.page1Picked = menu.GetCustomReference("Page1Picked").gameObject;
			this.page2Picked = menu.GetCustomReference("Page2Picked").gameObject;
			this.lightSkillTree = menu.GetCustomReference("LightSkillTree").gameObject;
			this.darkSkillTree = menu.GetCustomReference("DarkSkillTree").gameObject;
			this.LP = menu.GetCustomReference("LP").GetComponent<Text>();
			this.DP = menu.GetCustomReference("DP").GetComponent<Text>();
			this.InitSaveJSON();
			QuestHandler.globalRandom = new Random(Random.Range(1, 10000000));
			menu.GetCustomReference("SkillTreeBTN").GetComponent<Button>().onClick.AddListener(delegate()
			{
				this.LoadSkillTree();
				BookUIHandler.AbilityInfo.gameObject.SetActive(false);
			});
			menu.GetCustomReference("OtherSkill").GetComponent<Button>().onClick.AddListener(delegate()
			{
				this.LoadSkillTree();
				BookUIHandler.AbilityInfo.gameObject.SetActive(false);
			});
			menu.GetCustomReference("QuestBTN").GetComponent<Button>().onClick.AddListener(delegate()
			{
				this.LoadQuestPage();
				BookUIHandler.AbilityInfo.gameObject.SetActive(false);
			});
			this.SetupAllQuests();
			menu.GetCustomReference("DebugNewQuest").GetComponent<Button>().onClick.AddListener(delegate()
			{
				BookUIHandler.saveData.quest1 = null;
				BookUIHandler.saveData.quest2 = null;
				BookUIHandler.saveData.quest3 = null;
				this.questUI1.quest = null;
				this.questUI2.quest = null;
				this.questUI3.quest = null;
				this.SetupAllQuests();
			});
			menu.GetCustomReference("DebugNewQuest").gameObject.SetActive(false);
			this.UpdateAllStats();
			MainCondition.OnAnyMainConditionProgressedEvent += this.OnAnyMainConditionCompleted;
			EventManager.onCreatureKill += new EventManager.CreatureKillEvent(this.AddDrainComponentOnKill);
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00004F84 File Offset: 0x00003184
		private void AddDrainComponentOnKill(Creature creature, Player player, CollisionInstance collisionInstance, EventTime eventTime)
		{
			bool isPlayer = creature.isPlayer;
			if (!isPlayer)
			{
				(creature.GetComponent<CreatureDrainComponent>() ? creature.gameObject.GetComponent<CreatureDrainComponent>() : creature.gameObject.AddComponent<CreatureDrainComponent>()).health = creature.maxHealth;
			}
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00004FCE File Offset: 0x000031CE
		private void OnAnyMainConditionCompleted(MainCondition main)
		{
			this.UpdateQuestProgress(this.questUI1);
			this.UpdateQuestProgress(this.questUI2);
			this.UpdateQuestProgress(this.questUI3);
			this.SaveJson();
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00005000 File Offset: 0x00003200
		public void UpdateQuestProgress(QuestUIComponets p_questUIC)
		{
			bool flag = (double)p_questUIC.quest.mainCondition.progress >= (double)p_questUIC.quest.mainCondition.progessGoal;
			if (flag)
			{
				p_questUIC.ShowQuestData(p_questUIC.quest);
			}
			else
			{
				p_questUIC.progressText.text = string.Format("{0} / {1}", Math.Round((double)p_questUIC.quest.mainCondition.progress, 1), p_questUIC.quest.mainCondition.progessGoal);
			}
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00005090 File Offset: 0x00003290
		private void SetupAllQuests()
		{
			BookUIHandler.saveData.quest1 = this.LoadQuestItem(this.questUI1, BookUIHandler.saveData.quest1, 1).GetQuestInfo();
			BookUIHandler.saveData.quest2 = this.LoadQuestItem(this.questUI2, BookUIHandler.saveData.quest2, 2).GetQuestInfo();
			BookUIHandler.saveData.quest3 = this.LoadQuestItem(this.questUI3, BookUIHandler.saveData.quest3, 3).GetQuestInfo();
			this.SaveJson();
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x0000511A File Offset: 0x0000331A
		private void LoadQuestPage()
		{
			this.UpdateAllQuestDisplays();
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00005124 File Offset: 0x00003324
		private Quest LoadQuestItem(QuestUIComponets questUIComponent, SaveData.QuestInfo savedQuestInfo = null, int lvl = 1)
		{
			bool flag = questUIComponent.quest == null;
			if (flag)
			{
				bool flag2 = savedQuestInfo != null;
				if (flag2)
				{
					Quest quest = new Quest(savedQuestInfo.id, savedQuestInfo.level).SetupRandomQuest();
					quest.mainCondition.UpdateProgress(savedQuestInfo.progress);
					questUIComponent.quest = quest;
					questUIComponent.ShowQuestData(questUIComponent.quest);
				}
				else
				{
					questUIComponent.quest = new Quest(0, lvl).SetupRandomQuest();
					questUIComponent.ShowQuestData(questUIComponent.quest);
				}
			}
			else
			{
				Debug.Log(base.GetType().FullName + " :: Quest in component is not null");
				questUIComponent.ShowQuestData(questUIComponent.quest);
			}
			questUIComponent.claimBTN.onClick.AddListener(delegate()
			{
				this.OnClaimClicked(questUIComponent, questUIComponent.quest);
			});
			return questUIComponent.quest;
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00005250 File Offset: 0x00003450
		private void OnClaimClicked(QuestUIComponets questUIComponent, Quest quest)
		{
			bool completed = quest.completed;
			if (completed)
			{
				questUIComponent.claimBTN.onClick.RemoveAllListeners();
				questUIComponent.quest = null;
				bool flag = BookUIHandler.saveData.quest1.id == quest.id;
				if (flag)
				{
					BookUIHandler.saveData.quest1 = null;
					BookUIHandler.saveData.quest1 = this.LoadQuestItem(questUIComponent, null, 1).GetQuestInfo();
				}
				bool flag2 = BookUIHandler.saveData.quest2.id == quest.id;
				if (flag2)
				{
					BookUIHandler.saveData.quest2 = null;
					BookUIHandler.saveData.quest2 = this.LoadQuestItem(questUIComponent, null, 2).GetQuestInfo();
				}
				bool flag3 = BookUIHandler.saveData.quest3.id == quest.id;
				if (flag3)
				{
					BookUIHandler.saveData.quest3 = null;
					BookUIHandler.saveData.quest3 = this.LoadQuestItem(questUIComponent, null, 3).GetQuestInfo();
				}
				quest.mainCondition.GiveAllRewards();
				this.UpdateAllQuestDisplays();
				this.SaveJson();
			}
			else
			{
				Debug.LogError(base.GetType().FullName + " :: Quest is not completed and is trying to be redeemed");
			}
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00005384 File Offset: 0x00003584
		private void UpdateAllQuestDisplays()
		{
			this.questUI1.ShowQuestData(this.questUI1.quest);
			this.questUI2.ShowQuestData(this.questUI2.quest);
			this.questUI3.ShowQuestData(this.questUI3.quest);
			this.UpdateAllStats();
		}

		// Token: 0x060000AD RID: 173 RVA: 0x000053E0 File Offset: 0x000035E0
		private void UpdateAllStats()
		{
			this.LP.text = string.Format("{0} LP", BookUIHandler.saveData.lightPoints);
			this.DP.text = string.Format("{0} DP", BookUIHandler.saveData.darkPoints);
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00005438 File Offset: 0x00003638
		public void LoadSkillTree()
		{
			GameObject gameObject = null;
			bool flag = this.darkSide;
			if (flag)
			{
				this.lightSkillTree.SetActive(true);
				this.darkSkillTree.SetActive(false);
				this.darkSide = false;
				gameObject = this.lightSkillTree;
			}
			else
			{
				bool flag2 = !this.darkSide;
				if (flag2)
				{
					this.lightSkillTree.SetActive(false);
					this.darkSkillTree.SetActive(true);
					this.darkSide = true;
					gameObject = this.darkSkillTree;
				}
			}
			this.UpdateSkilltree(gameObject);
		}

		// Token: 0x060000AF RID: 175 RVA: 0x000054C0 File Offset: 0x000036C0
		public void UpdateSkilltree(GameObject skilltree)
		{
			foreach (SkillData skillData in skilltree.GetComponentsInChildren<SkillData>(true))
			{
				bool flag = skillData.required.Count > 0;
				if (flag)
				{
					bool flag2 = skillData.required.All((SkillData skill) => BookUIHandler.saveData.unlockedSkills.Contains(skill.skillName));
					if (flag2)
					{
						skillData.ShowSkill();
					}
					else
					{
						skillData.HideSkill();
					}
				}
				else
				{
					skillData.ShowSkill();
				}
			}
			this.UpdateAllStats();
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00005550 File Offset: 0x00003750
		private void InitSaveJSON()
		{
			bool flag = File.Exists(Path.Combine(Application.streamingAssetsPath, this.saveFileName));
			if (flag)
			{
				bool flag2 = BookUIHandler.IsValidJson(File.ReadAllText(Path.Combine(Application.streamingAssetsPath, this.saveFileName)));
				if (flag2)
				{
					BookUIHandler.saveData = JsonConvert.DeserializeObject<SaveData>(File.ReadAllText(Path.Combine(Application.streamingAssetsPath, this.saveFileName)));
				}
				else
				{
					this.GenerateNewSave();
				}
			}
			else
			{
				BookUIHandler.saveData = new SaveData();
				this.SaveJson();
			}
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x000055D4 File Offset: 0x000037D4
		private static bool IsValidJson(string strInput)
		{
			bool flag = string.IsNullOrWhiteSpace(strInput);
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				strInput = strInput.Trim();
				bool flag3 = (!strInput.StartsWith("{") || !strInput.EndsWith("}")) && (!strInput.StartsWith("[") || !strInput.EndsWith("]"));
				if (flag3)
				{
					flag2 = false;
				}
				else
				{
					try
					{
						JToken.Parse(strInput);
						flag2 = true;
					}
					catch (JsonReaderException ex)
					{
						Console.WriteLine(ex.Message);
						flag2 = false;
					}
					catch (Exception ex2)
					{
						Console.WriteLine(ex2.ToString());
						flag2 = false;
					}
				}
			}
			return flag2;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x0000568C File Offset: 0x0000388C
		private void GenerateNewSave()
		{
			Debug.LogError(base.GetType().FullName + " :: Error in savedat? Generating new one, and creating backup of old. Please contact Davi3684 for help");
			File.Move(Path.Combine(Application.streamingAssetsPath, this.saveFileName), Path.Combine(Application.streamingAssetsPath, "CorruptedSaveBackup.json"));
			BookUIHandler.saveData = new SaveData();
			this.SaveJson();
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x000056EC File Offset: 0x000038EC
		public override void OnShow(bool show)
		{
			base.OnShow(show);
			Button darkBTN = this.darkChoicePage.GetComponentInChildren<Button>();
			Button lightBTN = this.lightChoicePage.GetComponentInChildren<Button>();
			if (show)
			{
				bool flag = BookUIHandler.saveData.pathChosen == PathEnum.None;
				if (flag)
				{
					this.darkChoicePage.SetActive(true);
					this.lightChoicePage.SetActive(true);
					this.page1Picked.SetActive(false);
					this.page2Picked.SetActive(false);
					darkBTN.onClick.AddListener(delegate()
					{
						this.ChoseDark();
						darkBTN.onClick.RemoveAllListeners();
					});
					lightBTN.onClick.AddListener(delegate()
					{
						this.ChoseLight();
						lightBTN.onClick.RemoveAllListeners();
					});
				}
				else
				{
					this.ShowDefaultPages();
				}
			}
			else
			{
				darkBTN.onClick.RemoveAllListeners();
				lightBTN.onClick.RemoveAllListeners();
			}
			BookUIHandler.AbilityInfo.gameObject.SetActive(false);
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x000057FC File Offset: 0x000039FC
		private void ChoseLight()
		{
			BookUIHandler.saveData.pathChosen = PathEnum.Light;
			BookUIHandler.saveData.unlockedSkills.Add("LightPath");
			BookUIHandler.saveData.unlockedSkills.Add("DarkPath");
			BookUIHandler.saveData.unlockedSkills.Add("LightSideChosen");
			BookUIHandler.saveData.unlockedSkills.Add("LightSide");
			this.ShowDefaultPages();
			this.SaveJson();
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00005878 File Offset: 0x00003A78
		private void ChoseDark()
		{
			BookUIHandler.saveData.pathChosen = PathEnum.Dark;
			BookUIHandler.saveData.unlockedSkills.Add("DarkPath");
			BookUIHandler.saveData.unlockedSkills.Add("LightPath");
			BookUIHandler.saveData.unlockedSkills.Add("DarkSideChosen");
			BookUIHandler.saveData.unlockedSkills.Add("DarkSide");
			this.ShowDefaultPages();
			this.SaveJson();
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x000058F4 File Offset: 0x00003AF4
		private void ShowDefaultPages()
		{
			this.darkChoicePage.SetActive(false);
			this.lightChoicePage.SetActive(false);
			this.page1Picked.SetActive(true);
			this.page2Picked.SetActive(true);
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x0000592C File Offset: 0x00003B2C
		public void SaveJson()
		{
			SaveData saveData = BookUIHandler.saveData;
			Quest quest = this.questUI1.quest;
			SaveData.QuestInfo questInfo = ((quest != null) ? quest.GetQuestInfo() : null);
			saveData.quest1 = questInfo;
			SaveData saveData2 = BookUIHandler.saveData;
			Quest quest2 = this.questUI2.quest;
			SaveData.QuestInfo questInfo2 = ((quest2 != null) ? quest2.GetQuestInfo() : null);
			saveData2.quest2 = questInfo2;
			SaveData saveData3 = BookUIHandler.saveData;
			Quest quest3 = this.questUI3.quest;
			SaveData.QuestInfo questInfo3 = ((quest3 != null) ? quest3.GetQuestInfo() : null);
			saveData3.quest3 = questInfo3;
			bool flag = BookUIHandler.saveData == null;
			if (!flag)
			{
				Directory.CreateDirectory(Path.Combine(Application.streamingAssetsPath, "Mods/BloodMagic/Saves"));
				File.WriteAllText(Path.Combine(Application.streamingAssetsPath, this.saveFileName), JsonConvert.SerializeObject(BookUIHandler.saveData, 1));
			}
		}

		// Token: 0x04000044 RID: 68
		public QuestUIComponets questUI1;

		// Token: 0x04000045 RID: 69
		public QuestUIComponets questUI2;

		// Token: 0x04000046 RID: 70
		public QuestUIComponets questUI3;

		// Token: 0x04000047 RID: 71
		public GameObject darkChoicePage;

		// Token: 0x04000048 RID: 72
		public GameObject lightChoicePage;

		// Token: 0x04000049 RID: 73
		public GameObject page1Picked;

		// Token: 0x0400004A RID: 74
		public GameObject page2Picked;

		// Token: 0x0400004B RID: 75
		public Text LP;

		// Token: 0x0400004C RID: 76
		public Text DP;

		// Token: 0x0400004D RID: 77
		public GameObject lightSkillTree;

		// Token: 0x0400004E RID: 78
		public GameObject darkSkillTree;

		// Token: 0x0400004F RID: 79
		public static SaveData saveData;

		// Token: 0x04000050 RID: 80
		public static BookUIHandler Instance;

		// Token: 0x04000051 RID: 81
		private string saveFileName = "Mods/BloodMagic/Saves/BloodSaveData.json";

		// Token: 0x04000052 RID: 82
		public static AbilityInfo AbilityInfo;

		// Token: 0x04000053 RID: 83
		private bool darkSide;
	}
}
