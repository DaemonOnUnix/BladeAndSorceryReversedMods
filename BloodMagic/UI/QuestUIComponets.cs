using System;
using BloodMagic.Quest;
using UnityEngine;
using UnityEngine.UI;

namespace BloodMagic.UI
{
	// Token: 0x02000022 RID: 34
	public class QuestUIComponets : MonoBehaviour
	{
		// Token: 0x04000054 RID: 84
		public Text questTitleText;

		// Token: 0x04000055 RID: 85
		public Text questInfoText;

		// Token: 0x04000056 RID: 86
		public Text progressText;

		// Token: 0x04000057 RID: 87
		public Text xpRewardText;

		// Token: 0x04000058 RID: 88
		public Button claimBTN;

		// Token: 0x04000059 RID: 89
		public Quest quest = null;
	}
}
