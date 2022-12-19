using System;
using System.Collections;
using ThunderRoad;
using UnityEngine;

namespace LioScripts
{
	// Token: 0x02000007 RID: 7
	public class CanyonManager : MonoBehaviour
	{
		// Token: 0x06000011 RID: 17 RVA: 0x00002628 File Offset: 0x00000828
		private void Start()
		{
			this.hasRespawned = (this.hasRespawnedSkletals = false);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002646 File Offset: 0x00000846
		public void ShowStartMessage()
		{
			this.startMsgEvent.ShowMessage();
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002658 File Offset: 0x00000858
		public void RespawnAllMobs()
		{
			bool flag = !this.hasRespawned;
			if (flag)
			{
				this.hasRespawned = true;
				this.LevelEndZone.SetActive(true);
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
					this.MobsRoot.transform.GetChild(j).gameObject.GetComponent<CreatureSpawner>().Spawn();
				}
			}
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000272C File Offset: 0x0000092C
		public void SpawnSkeltals(GameObject zone)
		{
			bool flag = !this.hasRespawnedSkletals;
			if (flag)
			{
				this.hasRespawnedSkletals = true;
				zone.SetActive(false);
				foreach (GameObject gameObject in Object.FindObjectsOfType<GameObject>())
				{
					bool flag2 = gameObject.activeInHierarchy && gameObject.GetComponent<Creature>() != null && !gameObject.name.Contains("Player");
					if (flag2)
					{
						bool isKilled = gameObject.GetComponent<Creature>().isKilled;
						if (isKilled)
						{
							gameObject.GetComponent<Creature>().Despawn();
						}
					}
				}
				for (int j = 0; j < this.SkeltalsRoot.transform.childCount; j++)
				{
					this.SkeltalsRoot.transform.GetChild(j).gameObject.GetComponent<CreatureSpawner>().Spawn();
				}
				base.StartCoroutine(this.ResetNPCBehaviour());
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002819 File Offset: 0x00000A19
		public void EndGame()
		{
			this.EndMsgEvent.ShowMessage();
			this.levelLoader.LoadLevel();
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002834 File Offset: 0x00000A34
		private IEnumerator ResetNPCBehaviour()
		{
			yield return new WaitForSeconds(0.5f);
			foreach (GameObject g in Object.FindObjectsOfType<GameObject>())
			{
				bool flag = g.activeInHierarchy && g.GetComponent<Creature>() != null && !g.name.Contains("Player");
				if (flag)
				{
					g.GetComponent<Creature>().brain.SetState(0);
				}
				g = null;
			}
			GameObject[] array = null;
			yield return new WaitForSeconds(1f);
			foreach (GameObject g2 in Object.FindObjectsOfType<GameObject>())
			{
				bool flag2 = g2.activeInHierarchy && g2.GetComponent<Creature>() != null && !g2.name.Contains("Player");
				if (flag2)
				{
					g2.GetComponent<Creature>().brain.SetState(0);
				}
				g2 = null;
			}
			GameObject[] array2 = null;
			yield break;
		}

		// Token: 0x0400000F RID: 15
		[SerializeField]
		private GameObject MobsRoot;

		// Token: 0x04000010 RID: 16
		[SerializeField]
		private GameObject SkeltalsRoot;

		// Token: 0x04000011 RID: 17
		[SerializeField]
		private GameObject LevelEndZone;

		// Token: 0x04000012 RID: 18
		[SerializeField]
		private EventLoadLevel levelLoader;

		// Token: 0x04000013 RID: 19
		[SerializeField]
		private EventMessage startMsgEvent;

		// Token: 0x04000014 RID: 20
		[SerializeField]
		private EventMessage LootMsgEvent;

		// Token: 0x04000015 RID: 21
		[SerializeField]
		private EventMessage EndMsgEvent;

		// Token: 0x04000016 RID: 22
		private bool hasRespawned;

		// Token: 0x04000017 RID: 23
		private bool hasRespawnedSkletals;
	}
}
