using System;
using ThunderRoad;
using UnityEngine;

namespace Wully.Module
{
	// Token: 0x0200000D RID: 13
	public class BrainModuleAlert : BrainData.Module
	{
		// Token: 0x06000055 RID: 85 RVA: 0x00004878 File Offset: 0x00002A78
		private void Alert(Brain.State state)
		{
			if (state == 3)
			{
				GameObject gameObject = this.level1;
				if (gameObject != null)
				{
					gameObject.SetActive(true);
				}
				GameObject gameObject2 = this.level2;
				if (gameObject2 != null)
				{
					gameObject2.SetActive(false);
				}
				GameObject gameObject3 = this.level3;
				if (gameObject3 == null)
				{
					return;
				}
				gameObject3.SetActive(false);
				return;
			}
			else if (state == 4)
			{
				GameObject gameObject4 = this.level1;
				if (gameObject4 != null)
				{
					gameObject4.SetActive(true);
				}
				GameObject gameObject5 = this.level2;
				if (gameObject5 != null)
				{
					gameObject5.SetActive(true);
				}
				GameObject gameObject6 = this.level3;
				if (gameObject6 == null)
				{
					return;
				}
				gameObject6.SetActive(false);
				return;
			}
			else if (state == 5)
			{
				GameObject gameObject7 = this.level1;
				if (gameObject7 != null)
				{
					gameObject7.SetActive(true);
				}
				GameObject gameObject8 = this.level2;
				if (gameObject8 != null)
				{
					gameObject8.SetActive(true);
				}
				GameObject gameObject9 = this.level3;
				if (gameObject9 == null)
				{
					return;
				}
				gameObject9.SetActive(true);
				return;
			}
			else
			{
				GameObject gameObject10 = this.level1;
				if (gameObject10 != null)
				{
					gameObject10.SetActive(false);
				}
				GameObject gameObject11 = this.level2;
				if (gameObject11 != null)
				{
					gameObject11.SetActive(false);
				}
				GameObject gameObject12 = this.level3;
				if (gameObject12 == null)
				{
					return;
				}
				gameObject12.SetActive(false);
				return;
			}
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00004968 File Offset: 0x00002B68
		public override void Load(Creature creature)
		{
			base.Load(creature);
			if (this.loaded)
			{
				return;
			}
			if (this.alertGameObject == null)
			{
				Catalog.GetData<ItemData>(this.alertItem, true).SpawnAsync(delegate(Item alert)
				{
					this.alertGameObject = alert;
					this.level1 = this.alertGameObject.GetCustomReference("Level1", true).gameObject;
					this.level2 = this.alertGameObject.GetCustomReference("Level2", true).gameObject;
					this.level3 = this.alertGameObject.GetCustomReference("Level3", true).gameObject;
					this.alertGameObject.transform.parent = creature.ragdoll.headPart.transform;
					this.alertGameObject.transform.localPosition = Vector3.zero;
					this.alertGameObject.rb.isKinematic = true;
					this.alertGameObject.rb.useGravity = false;
					this.alertGameObject.gameObject.SetActive(true);
					Debug.Log("Added alert item to creature " + creature.data.id);
				}, null, null, null, true, null);
			}
			else
			{
				this.level1 = this.alertGameObject.GetCustomReference("Level1", true).gameObject;
				this.level2 = this.alertGameObject.GetCustomReference("Level2", true).gameObject;
				this.level3 = this.alertGameObject.GetCustomReference("Level3", true).gameObject;
				this.alertGameObject.transform.parent = creature.ragdoll.headPart.transform;
				this.alertGameObject.transform.localPosition = Vector3.zero;
				this.alertGameObject.rb.isKinematic = true;
				this.alertGameObject.rb.useGravity = false;
				this.alertGameObject.gameObject.SetActive(true);
			}
			this.loaded = true;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00004AAB File Offset: 0x00002CAB
		public override void Unload()
		{
			base.Unload();
			bool flag = this.loaded;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00004ABC File Offset: 0x00002CBC
		public override void Update()
		{
			base.Update();
			this.Alert(this.creature.brain.state);
			if (this.alertGameObject && Player.local)
			{
				this.alertGameObject.transform.LookAt(Player.local.head.cam.transform.position, -Player.local.head.cam.transform.right);
			}
		}

		// Token: 0x04000072 RID: 114
		private string alertItem = "HonorlessAlert";

		// Token: 0x04000073 RID: 115
		private Item alertGameObject;

		// Token: 0x04000074 RID: 116
		private GameObject level1;

		// Token: 0x04000075 RID: 117
		private GameObject level2;

		// Token: 0x04000076 RID: 118
		private GameObject level3;

		// Token: 0x04000077 RID: 119
		protected bool loaded;
	}
}
