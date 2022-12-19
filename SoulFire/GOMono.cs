using System;
using System.Threading.Tasks;
using UnityEngine;

namespace SoulFireSpell
{
	// Token: 0x02000005 RID: 5
	public class GOMono : MonoBehaviour
	{
		// Token: 0x0600000B RID: 11 RVA: 0x000026B4 File Offset: 0x000008B4
		public static async void DestroyGo(GameObject go)
		{
			await Task.Delay(6000);
			Object.Destroy(go);
		}
	}
}
