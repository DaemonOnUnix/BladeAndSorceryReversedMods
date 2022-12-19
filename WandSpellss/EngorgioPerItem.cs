using System;
using ThunderRoad;
using UnityEngine;

namespace WandSpellss
{
	// Token: 0x02000008 RID: 8
	internal class EngorgioPerItem : MonoBehaviour
	{
		// Token: 0x06000015 RID: 21 RVA: 0x000025E8 File Offset: 0x000007E8
		private void Start()
		{
			bool flag = base.GetComponent<Item>() != null;
			if (flag)
			{
				this.item = base.GetComponent<Item>();
			}
			else
			{
				bool flag2 = base.GetComponentInParent<Item>() != null;
				if (flag2)
				{
					this.item = base.GetComponentInParent<Item>();
				}
			}
			this.cantEngorgio = false;
			this.engorgioMaxSize = new Vector3(2f, 2f, 2f);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002658 File Offset: 0x00000858
		private void Update()
		{
			bool flag = !this.cantEngorgio;
			if (flag)
			{
				this.elapsedTime += Time.deltaTime;
				float percentageComplete = this.elapsedTime / 0.2f;
				this.item.transform.localScale = Vector3.Lerp(this.item.transform.localScale, this.engorgioMaxSize, Mathf.SmoothStep(0f, 1f, percentageComplete));
				this.elapsedTime = 0f;
				bool flag2 = this.item.transform.localScale == this.engorgioMaxSize;
				if (flag2)
				{
					this.cantEngorgio = true;
				}
			}
		}

		// Token: 0x0400001D RID: 29
		private bool cantEngorgio;

		// Token: 0x0400001E RID: 30
		private Item item;

		// Token: 0x0400001F RID: 31
		private float elapsedTime;

		// Token: 0x04000020 RID: 32
		private Vector3 engorgioMaxSize;

		// Token: 0x04000021 RID: 33
		private GameObject hit;
	}
}
