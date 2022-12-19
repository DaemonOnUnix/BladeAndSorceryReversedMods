using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CustomAvatarFramework
{
	// Token: 0x0200002C RID: 44
	public class WardrobeApparel
	{
		// Token: 0x060000CE RID: 206 RVA: 0x00007C6C File Offset: 0x00005E6C
		public static WardrobeApparel randomizeWardrobe(List<WardrobeApparel> wardrobeApparels)
		{
			float num = 0f;
			List<WardrobeApparel> list = new List<WardrobeApparel>();
			foreach (WardrobeApparel wardrobeApparel in wardrobeApparels)
			{
				if (wardrobeApparel.weight > 0f)
				{
					num += wardrobeApparel.weight;
					list.Add(new WardrobeApparel
					{
						weight = num,
						apparels = wardrobeApparel.apparels
					});
				}
			}
			float random = Random.Range(0f, num);
			return list.FirstOrDefault((WardrobeApparel weightedWardrobeApparel) => weightedWardrobeApparel.weight >= random);
		}

		// Token: 0x040000D7 RID: 215
		public float weight = 1f;

		// Token: 0x040000D8 RID: 216
		public List<string> apparels = new List<string>();
	}
}
