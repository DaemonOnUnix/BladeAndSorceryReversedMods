using System;
using System.Threading.Tasks;
using UnityEngine;

namespace LuckyBlocksLite
{
	// Token: 0x02000002 RID: 2
	public class GOMono : MonoBehaviour
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static async void DestroyGO(GameObject go)
		{
			await Task.Delay(2000);
			Object.Destroy(go);
		}
	}
}
