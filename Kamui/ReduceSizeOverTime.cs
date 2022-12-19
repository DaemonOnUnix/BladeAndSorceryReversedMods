using System;
using ThunderRoad;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Kamui
{
	// Token: 0x02000006 RID: 6
	internal class ReduceSizeOverTime : MonoBehaviour
	{
		// Token: 0x06000011 RID: 17 RVA: 0x0000290C File Offset: 0x00000B0C
		public void Start()
		{
			bool flag = base.GetComponent<Item>() != null;
			if (flag)
			{
				this.item = base.GetComponent<Item>();
			}
			else
			{
				bool flag2 = base.GetComponent<Creature>() != null;
				if (flag2)
				{
					this.creature = base.GetComponent<Creature>();
				}
			}
			this.minScale = new Vector3(0.01f, 0.01f, 0.01f);
			Addressables.LoadAssetAsync<Material>("apoz123Sharingan.Mat.KamuiDistortion").Completed += this.Op_Completed;
			this.distortionValue = 0f;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x0000299C File Offset: 0x00000B9C
		private void Op_Completed(AsyncOperationHandle<Material> obj)
		{
			bool flag = obj.Status == 1;
			if (flag)
			{
				this.kamuiDistortion = obj.Result;
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000029C8 File Offset: 0x00000BC8
		private void Update()
		{
			bool flag = this.isSucked;
			if (flag)
			{
				bool flag2 = this.distortionValue < 1f;
				if (flag2)
				{
					this.distortionValue += 0.01f;
					this.item.gameObject.GetComponent<Renderer>().sharedMaterial = this.kamuiDistortion;
					this.item.gameObject.GetComponent<Renderer>().sharedMaterial.SetFloat("Vector1_07761f96fcf147a7b17d362b38af7e11", this.distortionValue);
				}
				this.elapsedTime += Time.deltaTime;
				float percentageComplete = this.elapsedTime / 0.2f;
				bool flag3 = this.item != null;
				if (flag3)
				{
					this.item.transform.localScale = Vector3.Lerp(this.item.transform.localScale, this.minScale, Mathf.SmoothStep(0f, 1f, percentageComplete));
				}
				else
				{
					bool flag4 = this.creature != null;
					if (flag4)
					{
						this.creature.transform.localScale = Vector3.Lerp(this.creature.transform.localScale, this.minScale, Mathf.SmoothStep(0f, 1f, percentageComplete));
					}
				}
				this.elapsedTime = 0f;
				bool flag5 = this.item != null;
				if (flag5)
				{
					bool flag6 = this.item.transform.localScale == this.minScale;
					if (flag6)
					{
						this.isSucked = false;
						this.item.Despawn();
					}
				}
				else
				{
					bool flag7 = this.creature != null;
					if (flag7)
					{
						bool flag8 = this.creature.transform.localScale == this.minScale;
						if (flag8)
						{
							this.isSucked = false;
							this.creature.Despawn();
						}
					}
				}
			}
		}

		// Token: 0x04000019 RID: 25
		private Item item;

		// Token: 0x0400001A RID: 26
		internal bool isSucked;

		// Token: 0x0400001B RID: 27
		private float elapsedTime;

		// Token: 0x0400001C RID: 28
		private Vector3 minScale;

		// Token: 0x0400001D RID: 29
		private Material kamuiDistortion;

		// Token: 0x0400001E RID: 30
		private float distortionValue;

		// Token: 0x0400001F RID: 31
		private Creature creature;
	}
}
