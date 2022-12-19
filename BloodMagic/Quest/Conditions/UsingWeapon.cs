using System;
using System.Collections.Generic;
using System.Linq;
using BloodMagic.UI;
using ThunderRoad;
using UnityEngine;

namespace BloodMagic.Quest.Conditions
{
	// Token: 0x02000006 RID: 6
	public class UsingWeapon : Condition
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600002C RID: 44 RVA: 0x000025A0 File Offset: 0x000007A0
		public override int conditionCost
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000025A4 File Offset: 0x000007A4
		public override Condition SetupCondition(int p_seed, int p_level)
		{
			base.SetupCondition(p_seed, p_level);
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			bool flag = SkillHandler.IsSkillUnlocked("Blood Sword");
			if (flag)
			{
				dictionary.Add("BloodSword", "Blood Sword");
			}
			bool flag2 = SkillHandler.IsSkillUnlocked("Blood Bow");
			if (flag2)
			{
				dictionary.Add("BloodArrow", "Blood Bow");
			}
			dictionary.Add("BloodDagger", "Blood Dagger");
			dictionary.Add("BloodBullet", "Blood Bullet");
			this.chosenItemKey = dictionary.ToList<KeyValuePair<string, string>>()[this.random.Next(0, dictionary.Count)].Key;
			this.conditionText = "using a " + dictionary[this.chosenItemKey];
			return this;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x0000266F File Offset: 0x0000086F
		public override bool CanBeUsedWithType(Type mainType)
		{
			return mainType == typeof(Kill);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002684 File Offset: 0x00000884
		public override bool IsConditionMet(Type mainType, object[] parseProperties = null)
		{
			Debug.Log(base.GetType().FullName + " :: Using weapon called");
			ColliderGroup sourceColliderGroup = (parseProperties[2] as CollisionInstance).sourceColliderGroup;
			Item item;
			if (sourceColliderGroup == null)
			{
				item = null;
			}
			else
			{
				CollisionHandler collisionHandler = sourceColliderGroup.collisionHandler;
				item = ((collisionHandler != null) ? collisionHandler.item : null);
			}
			Item item2 = item;
			return item2 != null && item2.itemId == this.chosenItemKey;
		}

		// Token: 0x0400001D RID: 29
		public string chosenItemKey;
	}
}
