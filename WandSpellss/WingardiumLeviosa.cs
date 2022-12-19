using System;
using System.Collections;
using ThunderRoad;
using UnityEngine;

namespace WandSpellss
{
	// Token: 0x02000022 RID: 34
	public class WingardiumLeviosa : MonoBehaviour
	{
		// Token: 0x06000084 RID: 132 RVA: 0x0000766C File Offset: 0x0000586C
		public void Start()
		{
			this.item = base.GetComponent<Item>();
			this.canLift = false;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00007684 File Offset: 0x00005884
		private void Item_OnHeldActionEvent(RagdollHand ragdollHand, Handle handle, Interactable.Action action)
		{
			bool flag = action == 2 && this.canLift;
			if (flag)
			{
				this.canLift = false;
			}
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000076AC File Offset: 0x000058AC
		internal void CastRay()
		{
			RaycastHit hit;
			bool flag = Physics.Raycast(this.item.flyDirRef.position, this.item.flyDirRef.forward, ref hit);
			if (flag)
			{
				Debug.Log("Did hit.");
				Debug.Log(hit.collider.gameObject.transform.parent.name);
				Transform parent = hit.collider.gameObject.transform.parent;
				this.parentLocal = parent.gameObject;
				bool flag2 = this.parentLocal.gameObject.GetComponent<Item>() != null;
				if (flag2)
				{
					this.canLift = true;
					this.radius = this.parentLocal.transform.position - this.item.flyDirRef.position;
					this.distance = Math.Abs(Vector3.Distance(this.parentLocal.transform.position, this.item.flyDirRef.position));
				}
				else
				{
					bool flag3 = this.parentLocal.GetComponentInParent<Creature>() != null;
					if (flag3)
					{
						this.canLift = true;
						this.distance = Math.Abs(Vector3.Distance(this.parentLocal.transform.position, this.item.flyDirRef.position));
					}
				}
			}
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00007809 File Offset: 0x00005A09
		private IEnumerator LevitateCoroutine()
		{
			yield return new WaitForSeconds(3f);
			yield break;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00007818 File Offset: 0x00005A18
		private void Update()
		{
			this.direction = this.item.flyDirRef.forward;
			bool flag = this.parentLocal != null;
			if (flag)
			{
				bool flag2 = this.canLift;
				if (flag2)
				{
					this.parentLocal.transform.position = this.item.flyDirRef.position + this.direction * this.distance;
				}
			}
		}

		// Token: 0x040000F8 RID: 248
		private Item item;

		// Token: 0x040000F9 RID: 249
		internal bool canLift;

		// Token: 0x040000FA RID: 250
		internal GameObject parentLocal;

		// Token: 0x040000FB RID: 251
		private Vector3 radius;

		// Token: 0x040000FC RID: 252
		private Vector3 direction;

		// Token: 0x040000FD RID: 253
		private float distance;
	}
}
