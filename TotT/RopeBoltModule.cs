using System;
using System.Collections;
using ThunderRoad;
using UnityEngine;

namespace TotT
{
	// Token: 0x0200003F RID: 63
	public class RopeBoltModule : MonoBehaviour
	{
		// Token: 0x060001C4 RID: 452 RVA: 0x0000C9EA File Offset: 0x0000ABEA
		public void Start()
		{
			this.item = base.GetComponent<Item>();
			this.item.mainCollisionHandler.OnCollisionStartEvent += new CollisionHandler.CollisionEvent(this.MainCollisionHandler_OnCollisionStartEvent);
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0000CA18 File Offset: 0x0000AC18
		private void MainCollisionHandler_OnCollisionStartEvent(CollisionInstance collisionInstance)
		{
			RagdollPart hitRagdollPart = collisionInstance.damageStruct.hitRagdollPart;
			Creature creature2;
			if (hitRagdollPart == null)
			{
				creature2 = null;
			}
			else
			{
				Ragdoll ragdoll = hitRagdollPart.ragdoll;
				creature2 = ((ragdoll != null) ? ragdoll.creature : null);
			}
			Creature creature = creature2;
			bool flag = creature == null && this.item.isPenetrating;
			if (flag)
			{
				this.item.mainHandleRight.data.disableTouch = true;
				this.item.mainHandleRight.data.allowTelekinesis = false;
				base.StartCoroutine(this.RopeBoltMethod());
			}
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x0000CAA0 File Offset: 0x0000ACA0
		private IEnumerator RopeBoltMethod()
		{
			Transform BoltEnd = this.item.GetCustomReference("RopeObject", true);
			BoltEnd.gameObject.SetActive(true);
			BoltEnd.Rotate(0f, 0f, 0f);
			Rope rope = BoltEnd.gameObject.GetComponent<Rope>();
			RaycastHit hit;
			Physics.Raycast(this.item.transform.position, -Vector3.up, ref hit, float.PositiveInfinity, LayerMask.GetMask(new string[] { "NPC", "Ragdoll", "Dropped Object", "Default", "SkyDome" }));
			bool flag = hit.transform == null;
			Transform RopeStart;
			if (flag)
			{
				RopeStart = new GameObject().transform;
				RopeStart.position = BoltEnd.position;
				RopeStart.Translate(new Vector3(this.item.transform.position.x, this.item.transform.position.y - 15f, this.item.transform.position.z));
			}
			else
			{
				RopeStart = hit.transform;
			}
			rope.ropeStart = RopeStart;
			rope.ropeTarget = this.item.transform;
			rope.Generate();
			yield break;
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x0000CAAF File Offset: 0x0000ACAF
		public void Update()
		{
		}

		// Token: 0x04000144 RID: 324
		private Item item;
	}
}
