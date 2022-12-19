using System;
using ThunderRoad;
using UnityEngine;

namespace WandSpellss
{
	// Token: 0x0200000A RID: 10
	internal class EvanescoPerItem : MonoBehaviour
	{
		// Token: 0x0600001B RID: 27 RVA: 0x00002FA4 File Offset: 0x000011A4
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
			this.cantEvanesco = false;
			this.dissolveVal = 0f;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00003004 File Offset: 0x00001204
		private void Update()
		{
			bool flag = !this.cantEvanesco;
			if (flag)
			{
				bool flag2 = this.item.gameObject.GetComponent<Renderer>() != null;
				if (flag2)
				{
					bool flag3 = this.dissolveVal < 1f;
					if (flag3)
					{
						this.dissolveVal += 0.01f;
						foreach (Material mat in this.item.gameObject.GetComponent<Renderer>().materials)
						{
							mat.SetFloat("_dissolve", this.dissolveVal);
						}
					}
					else
					{
						bool flag4 = this.dissolveVal >= 1f;
						if (flag4)
						{
							this.dissolveVal = 0f;
							this.cantEvanesco = true;
							Object.Destroy(this.item.gameObject);
						}
					}
				}
				else
				{
					bool flag5 = this.item.gameObject.GetComponentInChildren<Renderer>() != null;
					if (flag5)
					{
						bool flag6 = this.dissolveVal < 1f;
						if (flag6)
						{
							Debug.Log(this.dissolveVal);
							this.dissolveVal += 0.01f;
							foreach (Material mat2 in this.item.gameObject.GetComponentInChildren<Renderer>().materials)
							{
								mat2.SetFloat("_dissolve", this.dissolveVal);
							}
						}
						else
						{
							bool flag7 = this.dissolveVal >= 1f;
							if (flag7)
							{
								this.dissolveVal = 0f;
								this.cantEvanesco = true;
								Object.Destroy(this.item.gameObject);
							}
						}
					}
				}
			}
		}

		// Token: 0x0400002B RID: 43
		private bool cantEvanesco;

		// Token: 0x0400002C RID: 44
		private Item item;

		// Token: 0x0400002D RID: 45
		private float elapsedTime;

		// Token: 0x0400002E RID: 46
		private float dissolveVal;
	}
}
