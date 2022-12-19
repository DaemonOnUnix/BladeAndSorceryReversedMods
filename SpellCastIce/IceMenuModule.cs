using System;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;
using UnityEngine.UI;

namespace SpellCastIce
{
	// Token: 0x02000004 RID: 4
	internal class IceMenuModule : MenuModule
	{
		// Token: 0x06000014 RID: 20 RVA: 0x000027E0 File Offset: 0x000009E0
		public override void Init(MenuData menuData, Menu menu)
		{
			base.Init(menuData, menu);
			this.levelTxt = menu.GetCustomReference("Level", true).GetComponent<Text>();
			this.xpTxt = menu.GetCustomReference("XP", true).GetComponent<Text>();
			this.pointsTxt = menu.GetCustomReference("Points", true).GetComponent<Text>();
			this.unlockBTN = menu.GetCustomReference("AbilityUnlock", true).GetComponent<Button>();
			this.abilityTitle = menu.GetCustomReference("AbilityTitle", true).GetComponent<Text>();
			this.abilityDescription = menu.GetCustomReference("AbilityDescription", true).GetComponent<Text>();
			this.cost = menu.GetCustomReference("Cost", true).GetComponent<Text>();
			this.menu = menu;
			this.SetUpSkillButton(IceManager.AbilitiesEnum.iceSpikeAim);
			this.SetUpSkillButton(IceManager.AbilitiesEnum.pickUpIceSpikes);
			this.SetUpSkillButton(IceManager.AbilitiesEnum.noGravity);
			this.SetUpSkillButton(IceManager.AbilitiesEnum.IceImbue);
			this.SetUpSkillButton(IceManager.AbilitiesEnum.IceMergeIce);
			this.SetUpSkillButton(IceManager.AbilitiesEnum.IceMergeFire);
			this.SetUpSkillButton(IceManager.AbilitiesEnum.IceMergeLightning);
			this.SetUpSkillButton(IceManager.AbilitiesEnum.IceMergeGrav);
			this.unlockBTN.onClick.AddListener(delegate()
			{
				bool flag = IceManager.UnlockAbility(this.selectedAbility);
				if (flag)
				{
					this.ReloadUnlocks();
					this.unlockBTN.interactable = false;
					this.unlockBTN.GetComponentInChildren<Text>().text = "Owned";
				}
			});
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000028FC File Offset: 0x00000AFC
		public override void OnShow(bool show)
		{
			base.OnShow(show);
			this.menuData.page2.gameObject.SetActive(false);
			this.ReloadUnlocks();
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002928 File Offset: 0x00000B28
		private void ReloadUnlocks()
		{
			this.levelTxt.text = IceManager.level.ToString();
			this.xpTxt.text = IceManager.xp.ToString("0.0") + " / " + IceManager.XpForNextLevel(IceManager.level).ToString();
			this.pointsTxt.text = IceManager.levelPoints.ToString();
			foreach (KeyValuePair<IceManager.AbilitiesEnum, IceManager.Ability> keyValuePair in IceManager.abilityDict)
			{
				this.SetUnlockedState(keyValuePair.Value.customRefName, keyValuePair.Key);
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000029F8 File Offset: 0x00000BF8
		public void SetUnlockedState(string customRefName, IceManager.AbilitiesEnum ab)
		{
			Image componentInChildren = this.menu.GetCustomReference(customRefName, true).GetComponentInChildren<Image>();
			IceManager.Ability ability;
			IceManager.abilityDict.TryGetValue(ab, out ability);
			bool unlocked = ability.unlocked;
			if (unlocked)
			{
				componentInChildren.color = Color.green;
			}
			else
			{
				componentInChildren.color = Color.red;
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002A50 File Offset: 0x00000C50
		private void SetUpSkillButton(IceManager.AbilitiesEnum abilitiesEnum)
		{
			IceManager.Ability ability;
			IceManager.abilityDict.TryGetValue(abilitiesEnum, out ability);
			this.menu.GetCustomReference(ability.customRefName, true).GetComponentInChildren<Button>().onClick.AddListener(delegate()
			{
				this.LoadAbilityPage(ability.uiTitle, ability.uiDescript, ability.levelPointCost, abilitiesEnum);
			});
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002ABC File Offset: 0x00000CBC
		private void LoadAbilityPage(string abilityName, string abilityDsc, int cost, IceManager.AbilitiesEnum abilitiesEnum)
		{
			this.menuData.page2.gameObject.SetActive(true);
			this.abilityTitle.text = abilityName;
			this.abilityDescription.text = abilityDsc;
			this.cost.text = cost.ToString();
			bool flag = IceManager.IsAbilityUnlocked(abilitiesEnum);
			if (flag)
			{
				this.unlockBTN.interactable = false;
				this.unlockBTN.GetComponentInChildren<Text>().text = "Owned";
			}
			else
			{
				this.unlockBTN.interactable = true;
				this.unlockBTN.GetComponentInChildren<Text>().text = "Unlock";
			}
			this.selectedAbility = abilitiesEnum;
		}

		// Token: 0x0400000D RID: 13
		private Text levelTxt;

		// Token: 0x0400000E RID: 14
		private Text xpTxt;

		// Token: 0x0400000F RID: 15
		private Text pointsTxt;

		// Token: 0x04000010 RID: 16
		private Button unlockBTN;

		// Token: 0x04000011 RID: 17
		private Text abilityTitle;

		// Token: 0x04000012 RID: 18
		private Text abilityDescription;

		// Token: 0x04000013 RID: 19
		private Text cost;

		// Token: 0x04000014 RID: 20
		private IceManager.AbilitiesEnum selectedAbility;

		// Token: 0x04000015 RID: 21
		private Menu menu;
	}
}
