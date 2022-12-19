using System;
using System.Collections.Generic;
using System.Linq;
using ThunderRoad;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Spider
{
	// Token: 0x02000003 RID: 3
	public class SpiderMenu : MenuModule
	{
		// Token: 0x0600000D RID: 13 RVA: 0x00003510 File Offset: 0x00001710
		public override void Init(MenuData menuData, Menu menu)
		{
			base.Init(menuData, menu);
			this.climbToggle = menu.GetCustomReference("Climb").GetComponent<Button>();
			this.clearTether = menu.GetCustomReference("ClearTether").GetComponent<Button>();
			this.powerSlider = menu.GetCustomReference("Power").GetComponent<Slider>();
			this.tetherSlider = menu.GetCustomReference("Tether").GetComponent<Slider>();
			this.timeSlider = menu.GetCustomReference("Time").GetComponent<Slider>();
			this.elasticitySlider = menu.GetCustomReference("ElasticitySlider").GetComponent<Slider>();
			this.powerText = menu.GetCustomReference("PowerText").GetComponent<Text>();
			this.timeText = menu.GetCustomReference("TimeText").GetComponent<Text>();
			this.tetherText = menu.GetCustomReference("TetherText").GetComponent<Text>();
			this.elasticityText = menu.GetCustomReference("ElasticityText").GetComponent<Text>();
			this.powerSlider.onValueChanged.AddListener(new UnityAction<float>(this.PowerSlider));
			this.tetherSlider.onValueChanged.AddListener(new UnityAction<float>(this.TetherSlider));
			this.timeSlider.onValueChanged.AddListener(new UnityAction<float>(this.TimeSlider));
			this.elasticitySlider.onValueChanged.AddListener(new UnityAction<float>(this.ElasticitySlider));
			this.climbToggle.onClick.AddListener(new UnityAction(this.ToggleClimb));
			this.clearTether.onClick.AddListener(new UnityAction(this.Wipe));
			this.powerSlider.value = Spell.webPower;
			this.tetherSlider.value = Spell.tetherPower;
			this.timeSlider.value = Spell.tetherTime;
			this.elasticitySlider.value = Spell.webElasticity;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000036F8 File Offset: 0x000018F8
		private void Wipe()
		{
			HashSet<Spell.Tether> hashSet = Spell.tetherToTime.Keys.ToHashSet<Spell.Tether>();
			foreach (Spell.Tether tether in hashSet)
			{
				tether.Destroy();
			}
			Spell.tetherToTime.Clear();
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00003764 File Offset: 0x00001964
		private void ToggleClimb()
		{
			bool climbFree = RagdollHandClimb.climbFree;
			if (climbFree)
			{
				RagdollHandClimb.climbFree = false;
				this.climbToggle.GetComponentInChildren<Text>().text = "Climb is off";
			}
			else
			{
				RagdollHandClimb.climbFree = true;
				this.climbToggle.GetComponentInChildren<Text>().text = "Climb is on";
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000037B9 File Offset: 0x000019B9
		private void PowerSlider(float value)
		{
			Spell.webPower = value;
			this.powerText.text = "WebPower: " + value.ToString();
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000037DF File Offset: 0x000019DF
		private void TetherSlider(float value)
		{
			Spell.tetherPower = value;
			this.tetherText.text = "TetherPower: " + value.ToString();
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00003805 File Offset: 0x00001A05
		private void TimeSlider(float value)
		{
			Spell.tetherTime = value;
			this.timeText.text = "TetherTime: " + value.ToString();
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0000382B File Offset: 0x00001A2B
		private void ElasticitySlider(float value)
		{
			Spell.webElasticity = value;
			this.elasticityText.text = "WebElasticity: " + value.ToString();
		}

		// Token: 0x0400001A RID: 26
		private Button climbToggle;

		// Token: 0x0400001B RID: 27
		private Button clearTether;

		// Token: 0x0400001C RID: 28
		private Slider powerSlider;

		// Token: 0x0400001D RID: 29
		private Slider tetherSlider;

		// Token: 0x0400001E RID: 30
		private Slider timeSlider;

		// Token: 0x0400001F RID: 31
		private Slider elasticitySlider;

		// Token: 0x04000020 RID: 32
		private Text elasticityText;

		// Token: 0x04000021 RID: 33
		private Text powerText;

		// Token: 0x04000022 RID: 34
		private Text tetherText;

		// Token: 0x04000023 RID: 35
		private Text timeText;
	}
}
