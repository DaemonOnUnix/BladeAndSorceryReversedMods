using System;
using ThunderRoad;
using UnityEngine;

namespace Yamato
{
	// Token: 0x02000002 RID: 2
	public class JudgementCutPosition : MonoBehaviour
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public void Update()
		{
			this.time += Time.deltaTime;
			bool flag = (double)this.time >= 0.2 && !this.spawning;
			if (flag)
			{
				Catalog.GetData<ItemData>("JudgementCut", true).SpawnAsync(new Action<Item>(this.ShootJudgementCut), new Vector3?(this.position), new Quaternion?(Quaternion.identity), null, true, null);
				this.spawning = true;
			}
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020D0 File Offset: 0x000002D0
		public void ShootJudgementCut(Item spawnedItem)
		{
			spawnedItem.gameObject.AddComponent<JudgementCutHit>();
			Object.Destroy(base.gameObject);
		}

		// Token: 0x04000001 RID: 1
		public Vector3 position = default(Vector3);

		// Token: 0x04000002 RID: 2
		private float time = 0f;

		// Token: 0x04000003 RID: 3
		private bool spawning = false;
	}
}
