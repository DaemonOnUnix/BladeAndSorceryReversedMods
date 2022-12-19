using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using BloodMagic.Quest.Conditions;
using UnityEngine;

namespace BloodMagic.Quest
{
	// Token: 0x02000004 RID: 4
	public class Quest
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000024 RID: 36 RVA: 0x000021F0 File Offset: 0x000003F0
		// (remove) Token: 0x06000025 RID: 37 RVA: 0x00002228 File Offset: 0x00000428
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Quest.QuestAction OnQuestCompleted;

		// Token: 0x06000026 RID: 38 RVA: 0x0000225D File Offset: 0x0000045D
		public Quest(int p_id, int p_level)
		{
			this.level = p_level;
			this.id = ((p_id == 0) ? QuestHandler.globalRandom.Next(1, 100000) : p_id);
			this.random = new Random(this.id);
		}

		// Token: 0x06000027 RID: 39 RVA: 0x0000229C File Offset: 0x0000049C
		public Quest SetupRandomQuest()
		{
			List<Type> list = this.MainConditions();
			Type type = list[this.random.Next(0, list.Count)];
			this.mainCondition = (MainCondition)type.GetMethod("SetupMainCondition", new Type[]
			{
				typeof(int),
				typeof(int)
			}).Invoke(Activator.CreateInstance(type), new object[] { this.id, this.level });
			this.mainCondition.OnMainConditionCompleted += this.FinishQuest;
			return this;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x0000234C File Offset: 0x0000054C
		private void FinishQuest(MainCondition main)
		{
			Debug.Log(base.GetType().FullName + " :: Quest finished");
			this.completed = true;
			bool flag = this.OnQuestCompleted != null;
			if (flag)
			{
				this.OnQuestCompleted(this);
			}
			main.OnMainConditionCompleted -= this.FinishQuest;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000023AC File Offset: 0x000005AC
		public List<Type> MainConditions()
		{
			Type parentType = typeof(MainCondition);
			return (from t in Assembly.GetExecutingAssembly().GetTypes()
				where t.BaseType == parentType
				select t).ToList<Type>();
		}

		// Token: 0x04000016 RID: 22
		public int id;

		// Token: 0x04000017 RID: 23
		public Random random;

		// Token: 0x04000018 RID: 24
		public bool completed;

		// Token: 0x04000019 RID: 25
		public MainCondition mainCondition;

		// Token: 0x0400001A RID: 26
		private int level;

		// Token: 0x02000027 RID: 39
		// (Invoke) Token: 0x060000CD RID: 205
		public delegate void QuestAction(Quest quest);
	}
}
