using System;
using ThunderRoad;
using UnityEngine;

namespace LioScripts
{
	// Token: 0x02000002 RID: 2
	public class BanditCampMapManager : MonoBehaviour
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		private void Start()
		{
			this.hasRespawned = false;
			this.startMsgEvent.ShowMessage();
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002068 File Offset: 0x00000268
		public void RespawnAllMobs()
		{
			bool flag = !this.hasRespawned;
			if (flag)
			{
				this.hasRespawned = true;
				foreach (GameObject gameObject in Object.FindObjectsOfType<GameObject>())
				{
					bool flag2 = gameObject.activeInHierarchy && gameObject.GetComponent<Creature>() != null && !gameObject.name.Contains("Player");
					if (flag2)
					{
						gameObject.GetComponent<Creature>().Despawn();
					}
				}
				for (int j = 0; j < this.MobsRoot.transform.childCount; j++)
				{
					this.MobsRoot.transform.GetChild(j).gameObject.GetComponent<CreatureSpawner>().ResetSpawner();
					this.MobsRoot.transform.GetChild(j).gameObject.GetComponent<CreatureSpawner>().Spawn();
				}
				this.LevelEndZone.SetActive(true);
				this.LootMsgEvent.ShowMessage();
			}
		}

		// Token: 0x06000003 RID: 3 RVA: 0x0000216C File Offset: 0x0000036C
		public void EndGame()
		{
			this.EndMsgEvent.ShowMessage();
			this.levelLoader.LoadLevel("Home");
		}

		// Token: 0x04000001 RID: 1
		[SerializeField]
		private GameObject MobsRoot;

		// Token: 0x04000002 RID: 2
		[SerializeField]
		private GameObject LevelEndZone;

		// Token: 0x04000003 RID: 3
		[SerializeField]
		private EventLoadLevel levelLoader;

		// Token: 0x04000004 RID: 4
		[SerializeField]
		private EventMessage startMsgEvent;

		// Token: 0x04000005 RID: 5
		[SerializeField]
		private EventMessage LootMsgEvent;

		// Token: 0x04000006 RID: 6
		[SerializeField]
		private EventMessage EndMsgEvent;

		// Token: 0x04000007 RID: 7
		private bool hasRespawned;
	}
}
