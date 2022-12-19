using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using BloodMagic.Quest.Rewards;

namespace BloodMagic.Quest.Conditions
{
	// Token: 0x0200000F RID: 15
	public class MainCondition
	{
		// Token: 0x0600005A RID: 90 RVA: 0x00002D28 File Offset: 0x00000F28
		public virtual MainCondition SetupMainCondition(int p_id, int p_level)
		{
			this.seed = p_id;
			this.level = p_level;
			this.random = new Random(this.seed);
			int num = this.random.Next(0, this.level * 2 + 1);
			int num2 = this.random.Next(0, this.level);
			this.totalCost = p_level;
			List<Type> list = new List<Type>();
			while (num > 0 && list.Count < num2)
			{
				List<Type> subConditions = this.GetSubConditions(num, list);
				bool flag = subConditions.Count > 0;
				if (!flag)
				{
					break;
				}
				Type type = subConditions[this.random.Next(0, subConditions.Count)];
				Condition condition = (Condition)type.GetMethod("SetupCondition", new Type[]
				{
					typeof(int),
					typeof(int)
				}).Invoke(Activator.CreateInstance(type), new object[] { this.seed, this.level });
				list.Add(type);
				num -= condition.conditionCost;
				this.totalCost += condition.conditionCost;
				this.conditions.Add(condition);
			}
			this.SetupRewards();
			return null;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00002E8B File Offset: 0x0000108B
		public virtual void SetupRewards()
		{
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00002E90 File Offset: 0x00001090
		public void GiveAllRewards()
		{
			foreach (Reward reward in this.rewards)
			{
				reward.GiveReward();
			}
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00002EE8 File Offset: 0x000010E8
		private List<Type> GetSubConditions(int maxCost, List<Type> existing)
		{
			Type parentType = typeof(Condition);
			return Assembly.GetExecutingAssembly().GetTypes().Where(delegate(Type t)
			{
				bool flag = !(t.BaseType == parentType) || (int)t.GetProperty("conditionCost").GetValue(Activator.CreateInstance(t)) > maxCost || existing.Contains(t);
				return !flag && (bool)t.GetMethod("CanBeUsedWithType", new Type[] { typeof(Type) }).Invoke(Activator.CreateInstance(t), new object[] { this.GetType() });
			})
				.ToList<Type>();
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600005E RID: 94 RVA: 0x00002F48 File Offset: 0x00001148
		// (remove) Token: 0x0600005F RID: 95 RVA: 0x00002F80 File Offset: 0x00001180
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event MainCondition.MainCondtionAction OnMainConditionCompleted;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000060 RID: 96 RVA: 0x00002FB8 File Offset: 0x000011B8
		// (remove) Token: 0x06000061 RID: 97 RVA: 0x00002FEC File Offset: 0x000011EC
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event MainCondition.MainCondtionAction OnAnyMainConditionProgressedEvent;

		// Token: 0x06000062 RID: 98 RVA: 0x00003020 File Offset: 0x00001220
		public void IsAllConditionsMet(Type mainType, object[] parseProperties = null, float p_progress = 1f)
		{
			foreach (Condition condition in this.conditions)
			{
				bool flag = !condition.IsConditionMet(mainType, parseProperties);
				if (flag)
				{
					return;
				}
			}
			this.UpdateProgress(p_progress);
		}

		// Token: 0x06000063 RID: 99 RVA: 0x0000308C File Offset: 0x0000128C
		public void UpdateProgress(float p_progress)
		{
			this.progress += p_progress;
			bool flag = (double)this.progress >= (double)this.progessGoal;
			if (flag)
			{
				this.progress = this.progessGoal;
				this.CompleteMainCondition();
			}
			bool flag2 = MainCondition.OnAnyMainConditionProgressedEvent == null;
			if (!flag2)
			{
				MainCondition.OnAnyMainConditionProgressedEvent(this);
			}
		}

		// Token: 0x06000064 RID: 100 RVA: 0x000030F0 File Offset: 0x000012F0
		public virtual void CompleteMainCondition()
		{
			bool flag = this.OnMainConditionCompleted == null;
			if (!flag)
			{
				this.OnMainConditionCompleted(this);
			}
		}

		// Token: 0x04000021 RID: 33
		public float progress = 0f;

		// Token: 0x04000022 RID: 34
		public float progessGoal = 0f;

		// Token: 0x04000023 RID: 35
		public List<Condition> conditions = new List<Condition>();

		// Token: 0x04000024 RID: 36
		public string conditionText = "";

		// Token: 0x04000025 RID: 37
		public int seed;

		// Token: 0x04000026 RID: 38
		public int level;

		// Token: 0x04000027 RID: 39
		protected Random random;

		// Token: 0x04000028 RID: 40
		public List<Reward> rewards = new List<Reward>();

		// Token: 0x04000029 RID: 41
		protected int totalCost;

		// Token: 0x02000029 RID: 41
		// (Invoke) Token: 0x060000D3 RID: 211
		public delegate void MainCondtionAction(MainCondition main);
	}
}
