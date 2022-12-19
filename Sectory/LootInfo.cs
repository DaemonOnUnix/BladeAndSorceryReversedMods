using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Sectory
{
	// Token: 0x0200000D RID: 13
	[Serializable]
	public class LootInfo
	{
		// Token: 0x0400005B RID: 91
		public int dynamicItemAdditionAmount;

		// Token: 0x0400005C RID: 92
		public int dynamicLootChance;

		// Token: 0x0400005D RID: 93
		public string dynamicLootSeed;

		// Token: 0x0400005E RID: 94
		public bool dynamicLoot;

		// Token: 0x0400005F RID: 95
		public bool dynamicLootRandomPerSession;

		// Token: 0x04000060 RID: 96
		public LootInfo.TableInfo[] tableInfos;

		// Token: 0x02000026 RID: 38
		public class TableInfo
		{
			// Token: 0x0600008D RID: 141 RVA: 0x00007618 File Offset: 0x00005818
			[JsonConstructor]
			public TableInfo(string lootTableID, bool oneHanded, bool twoHanded, bool mustBeAvailableInBook, float massBelowThis, float massAboveThis, string[] idMustContain, string[] idCannotContain, string[] nameMustContain, string[] nameCannotContain)
			{
				this.lootTableID = lootTableID;
				this.oneHanded = oneHanded;
				this.twoHanded = twoHanded;
				this.mustBeAvailableInBook = mustBeAvailableInBook;
				this.massBelowThis = massBelowThis;
				this.massAboveThis = massAboveThis;
				this.idMustContain = idMustContain;
				this.idCannotContain = idCannotContain;
				this.nameMustContain = nameMustContain;
				this.nameCannotContain = nameCannotContain;
				bool flag = oneHanded && twoHanded;
				if (flag)
				{
					Debug.LogWarning("Sectory: Caught a table info with both one handed and two handed required! That doesn't make sense. Set two handed to false to remedy this.");
					this.twoHanded = false;
				}
			}

			// Token: 0x040000CC RID: 204
			public string lootTableID;

			// Token: 0x040000CD RID: 205
			public bool oneHanded;

			// Token: 0x040000CE RID: 206
			public bool twoHanded;

			// Token: 0x040000CF RID: 207
			public bool mustBeAvailableInBook;

			// Token: 0x040000D0 RID: 208
			public float massBelowThis;

			// Token: 0x040000D1 RID: 209
			public float massAboveThis;

			// Token: 0x040000D2 RID: 210
			public string[] idMustContain;

			// Token: 0x040000D3 RID: 211
			public string[] idCannotContain;

			// Token: 0x040000D4 RID: 212
			public string[] nameMustContain;

			// Token: 0x040000D5 RID: 213
			public string[] nameCannotContain;
		}
	}
}
