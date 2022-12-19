using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace CustomAvatarFramework.Editor.Items
{
	// Token: 0x02000024 RID: 36
	public class SliderToText : MonoBehaviour
	{
		// Token: 0x060000A2 RID: 162 RVA: 0x00006EB8 File Offset: 0x000050B8
		private void Start()
		{
			this.textSliderValue = base.GetComponent<Text>();
			this.ShowSliderValue();
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00006ECC File Offset: 0x000050CC
		public void ShowSliderValue()
		{
			this.textSliderValue.text = this.sliderUI.value.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x040000C1 RID: 193
		public Slider sliderUI;

		// Token: 0x040000C2 RID: 194
		private Text textSliderValue;
	}
}
