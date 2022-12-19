using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BloodMagic
{
	// Token: 0x02000002 RID: 2
	public class SaveData
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		// (set) Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
		[JsonProperty(Required = 2)]
		public string version { get; set; } = "1.1.1";

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000003 RID: 3 RVA: 0x00002061 File Offset: 0x00000261
		// (set) Token: 0x06000004 RID: 4 RVA: 0x00002069 File Offset: 0x00000269
		public int lightPoints { get; set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000005 RID: 5 RVA: 0x00002072 File Offset: 0x00000272
		// (set) Token: 0x06000006 RID: 6 RVA: 0x0000207A File Offset: 0x0000027A
		public int darkPoints { get; set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000007 RID: 7 RVA: 0x00002083 File Offset: 0x00000283
		// (set) Token: 0x06000008 RID: 8 RVA: 0x0000208B File Offset: 0x0000028B
		public bool useHealth { get; set; } = true;

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000009 RID: 9 RVA: 0x00002094 File Offset: 0x00000294
		// (set) Token: 0x0600000A RID: 10 RVA: 0x0000209C File Offset: 0x0000029C
		public float xp { get; set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000B RID: 11 RVA: 0x000020A5 File Offset: 0x000002A5
		// (set) Token: 0x0600000C RID: 12 RVA: 0x000020AD File Offset: 0x000002AD
		public int level { get; set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600000D RID: 13 RVA: 0x000020B6 File Offset: 0x000002B6
		// (set) Token: 0x0600000E RID: 14 RVA: 0x000020BE File Offset: 0x000002BE
		public float bulletSpeed { get; set; } = 9f;

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600000F RID: 15 RVA: 0x000020C7 File Offset: 0x000002C7
		// (set) Token: 0x06000010 RID: 16 RVA: 0x000020CF File Offset: 0x000002CF
		public float gesturePrescision { get; set; } = 0.7f;

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000011 RID: 17 RVA: 0x000020D8 File Offset: 0x000002D8
		// (set) Token: 0x06000012 RID: 18 RVA: 0x000020E0 File Offset: 0x000002E0
		public float drainDistance { get; set; } = 5f;

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000013 RID: 19 RVA: 0x000020E9 File Offset: 0x000002E9
		// (set) Token: 0x06000014 RID: 20 RVA: 0x000020F1 File Offset: 0x000002F1
		public float drainPower { get; set; } = 5f;

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000015 RID: 21 RVA: 0x000020FA File Offset: 0x000002FA
		// (set) Token: 0x06000016 RID: 22 RVA: 0x00002102 File Offset: 0x00000302
		public float wavePushStrenght { get; set; } = 20f;

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000017 RID: 23 RVA: 0x0000210B File Offset: 0x0000030B
		// (set) Token: 0x06000018 RID: 24 RVA: 0x00002113 File Offset: 0x00000313
		public float xpMultiplier { get; set; } = 0.4f;

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000019 RID: 25 RVA: 0x0000211C File Offset: 0x0000031C
		// (set) Token: 0x0600001A RID: 26 RVA: 0x00002124 File Offset: 0x00000324
		public SaveData.QuestInfo quest1 { get; set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600001B RID: 27 RVA: 0x0000212D File Offset: 0x0000032D
		// (set) Token: 0x0600001C RID: 28 RVA: 0x00002135 File Offset: 0x00000335
		public SaveData.QuestInfo quest2 { get; set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600001D RID: 29 RVA: 0x0000213E File Offset: 0x0000033E
		// (set) Token: 0x0600001E RID: 30 RVA: 0x00002146 File Offset: 0x00000346
		public SaveData.QuestInfo quest3 { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600001F RID: 31 RVA: 0x0000214F File Offset: 0x0000034F
		// (set) Token: 0x06000020 RID: 32 RVA: 0x00002157 File Offset: 0x00000357
		public PathEnum pathChosen { get; set; } = PathEnum.None;

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000021 RID: 33 RVA: 0x00002160 File Offset: 0x00000360
		// (set) Token: 0x06000022 RID: 34 RVA: 0x00002168 File Offset: 0x00000368
		public List<string> unlockedSkills { get; set; } = new List<string>();

		// Token: 0x02000026 RID: 38
		public class QuestInfo
		{
			// Token: 0x17000018 RID: 24
			// (get) Token: 0x060000C5 RID: 197 RVA: 0x00005C46 File Offset: 0x00003E46
			// (set) Token: 0x060000C6 RID: 198 RVA: 0x00005C4E File Offset: 0x00003E4E
			public int id { get; set; }

			// Token: 0x17000019 RID: 25
			// (get) Token: 0x060000C7 RID: 199 RVA: 0x00005C57 File Offset: 0x00003E57
			// (set) Token: 0x060000C8 RID: 200 RVA: 0x00005C5F File Offset: 0x00003E5F
			public int level { get; set; }

			// Token: 0x1700001A RID: 26
			// (get) Token: 0x060000C9 RID: 201 RVA: 0x00005C68 File Offset: 0x00003E68
			// (set) Token: 0x060000CA RID: 202 RVA: 0x00005C70 File Offset: 0x00003E70
			public float progress { get; set; }
		}
	}
}
