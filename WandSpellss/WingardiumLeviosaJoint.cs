using System;
using ThunderRoad;
using UnityEngine;

namespace WandSpellss
{
	// Token: 0x02000023 RID: 35
	internal class WingardiumLeviosaJoint : MonoBehaviour
	{
		// Token: 0x0600008A RID: 138 RVA: 0x0000789A File Offset: 0x00005A9A
		public void Start()
		{
			this.item = base.GetComponent<Item>();
		}

		// Token: 0x0600008B RID: 139 RVA: 0x000078AC File Offset: 0x00005AAC
		internal void CastRay()
		{
			RaycastHit hit;
			bool flag = Physics.Raycast(this.item.flyDirRef.position, this.item.flyDirRef.forward, ref hit);
			if (flag)
			{
				Transform parent = hit.collider.gameObject.transform.parent;
				this.parentLocal = parent.gameObject;
				Item hitItem = this.parentLocal.gameObject.GetComponent<Item>();
				bool flag2 = hitItem != null;
				if (flag2)
				{
					JointDrive yDrive = default(JointDrive);
					yDrive.positionSpring = 10000f;
					yDrive.mode = 1;
					JointDrive xDrive = default(JointDrive);
					xDrive.positionSpring = 10000f;
					xDrive.mode = 1;
					JointDrive zDrive = default(JointDrive);
					zDrive.positionSpring = 10000f;
					zDrive.mode = 1;
					this.targetPos = hitItem.transform.position - this.wand.flyDirRef.transform.position;
					hitItem.gameObject.AddComponent<ConfigurableJoint>();
					hitItem.gameObject.GetComponent<ConfigurableJoint>().connectedBody = this.wand.gameObject.GetComponent<Rigidbody>();
					hitItem.gameObject.GetComponent<ConfigurableJoint>().autoConfigureConnectedAnchor = false;
					hitItem.gameObject.GetComponent<ConfigurableJoint>().connectedAnchor = this.wand.flyDirRef.transform.position;
					hitItem.gameObject.GetComponent<ConfigurableJoint>().yDrive = yDrive;
					hitItem.gameObject.GetComponent<ConfigurableJoint>().xDrive = xDrive;
					hitItem.gameObject.GetComponent<ConfigurableJoint>().zDrive = zDrive;
					this.hitObjectItem = hitItem;
					this.objectIsHovering = true;
				}
			}
		}

		// Token: 0x040000FE RID: 254
		private Item item;

		// Token: 0x040000FF RID: 255
		internal Item wand;

		// Token: 0x04000100 RID: 256
		private Item npcItem;

		// Token: 0x04000101 RID: 257
		private Creature creature;

		// Token: 0x04000102 RID: 258
		internal Item hitObjectItem;

		// Token: 0x04000103 RID: 259
		internal bool objectIsHovering;

		// Token: 0x04000104 RID: 260
		internal GameObject parentLocal;

		// Token: 0x04000105 RID: 261
		internal Vector3 targetPos;
	}
}
