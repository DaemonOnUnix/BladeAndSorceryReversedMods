using System;
using System.Collections.Generic;
using System.Linq;
using ThunderRoad;
using UnityEngine;

namespace WandSpellss
{
	// Token: 0x02000009 RID: 9
	public class Evanesco : MonoBehaviour
	{
		// Token: 0x06000018 RID: 24 RVA: 0x0000270F File Offset: 0x0000090F
		public void Start()
		{
			this.item = base.GetComponent<Item>();
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002720 File Offset: 0x00000920
		internal void CastRay()
		{
			RaycastHit hit;
			bool flag = Physics.Raycast(this.item.flyDirRef.position, this.item.flyDirRef.forward, ref hit);
			if (flag)
			{
				Debug.Log("Did hit.");
				Debug.Log(hit.collider.gameObject.transform.parent.name);
				Transform parent = hit.collider.gameObject.transform.parent;
				this.parentLocal = hit.collider.gameObject;
				bool flag2 = this.parentLocal.GetComponentInParent<Item>() != null;
				if (flag2)
				{
					bool flag3 = this.parentLocal.GetComponent<Renderer>() != null;
					if (flag3)
					{
						this.myMaterials = this.parentLocal.GetComponent<Renderer>().materials.ToList<Material>();
						Material[] matDefGood = new Material[this.myMaterials.Count];
						for (int i = 0; i < this.myMaterials.Count; i++)
						{
							this.evanescoDissolve.SetTexture("_Albedo", this.myMaterials[i].GetTexture("_BaseMap"));
							this.evanescoDissolve.SetColor("_color", this.myMaterials[i].GetColor("_BaseColor"));
							this.evanescoDissolve.SetTexture("_Normal", this.myMaterials[i].GetTexture("_BumpMap"));
							this.evanescoDissolve.SetTexture("_Metallic", this.myMaterials[i].GetTexture("_MetallicGlossMap"));
							matDefGood[i] = this.evanescoDissolve;
						}
						this.parentLocal.GetComponent<Renderer>().materials = matDefGood;
						this.cantEvanesco = false;
						this.parentLocal.AddComponent<EvanescoPerItem>();
					}
					else
					{
						bool flag4 = this.parentLocal.GetComponentInParent<Renderer>() != null;
						if (flag4)
						{
							this.myMaterials = this.parentLocal.GetComponentInParent<Renderer>().materials.ToList<Material>();
							Material[] matDefGood2 = new Material[this.myMaterials.Count];
							for (int j = 0; j < this.myMaterials.Count; j++)
							{
								this.evanescoDissolve.SetTexture("_Albedo", this.myMaterials[j].GetTexture("_BaseMap"));
								this.evanescoDissolve.SetColor("_color", this.myMaterials[j].GetColor("_BaseColor"));
								this.evanescoDissolve.SetTexture("_Normal", this.myMaterials[j].GetTexture("_BumpMap"));
								this.evanescoDissolve.SetTexture("_Metallic", this.myMaterials[j].GetTexture("_MetallicGlossMap"));
								matDefGood2[j] = this.evanescoDissolve;
							}
							this.parentLocal.GetComponentInParent<Renderer>().materials = matDefGood2;
							this.cantEvanesco = false;
							this.parentLocal.AddComponent<EvanescoPerItem>();
						}
						else
						{
							bool flag5 = this.parentLocal.GetComponentInChildren<Renderer>() != null;
							if (flag5)
							{
								this.myMaterials = this.parentLocal.GetComponentInChildren<Renderer>().materials.ToList<Material>();
								Material[] matDefGood3 = new Material[this.myMaterials.Count];
								for (int k = 0; k < this.myMaterials.Count; k++)
								{
									this.evanescoDissolve.SetTexture("_Albedo", this.myMaterials[k].GetTexture("_BaseMap"));
									this.evanescoDissolve.SetColor("_color", this.myMaterials[k].GetColor("_BaseColor"));
									this.evanescoDissolve.SetTexture("_Normal", this.myMaterials[k].GetTexture("_BumpMap"));
									this.evanescoDissolve.SetTexture("_Metallic", this.myMaterials[k].GetTexture("_MetallicGlossMap"));
									matDefGood3[k] = this.evanescoDissolve;
								}
								this.parentLocal.GetComponentInChildren<Renderer>().materials = matDefGood3;
								this.cantEvanesco = false;
								this.parentLocal.AddComponent<EvanescoPerItem>();
							}
						}
					}
				}
				else
				{
					bool flag6 = this.parentLocal.GetComponent<Item>() != null;
					if (flag6)
					{
						bool flag7 = this.parentLocal.GetComponent<Renderer>() != null;
						if (flag7)
						{
							this.myMaterials = this.parentLocal.GetComponent<Renderer>().materials.ToList<Material>();
							Material[] matDefGood4 = new Material[this.myMaterials.Count];
							for (int l = 0; l < this.myMaterials.Count; l++)
							{
								this.evanescoDissolve.SetTexture("_Albedo", this.myMaterials[l].GetTexture("_BaseMap"));
								this.evanescoDissolve.SetColor("_color", this.myMaterials[l].GetColor("_BaseColor"));
								this.evanescoDissolve.SetTexture("_Normal", this.myMaterials[l].GetTexture("_BumpMap"));
								this.evanescoDissolve.SetTexture("_Metallic", this.myMaterials[l].GetTexture("_MetallicGlossMap"));
								matDefGood4[l] = this.evanescoDissolve;
							}
							this.parentLocal.GetComponent<Renderer>().materials = matDefGood4;
							this.cantEvanesco = false;
							this.parentLocal.AddComponent<EvanescoPerItem>();
						}
						else
						{
							bool flag8 = this.parentLocal.GetComponentInParent<Renderer>() != null;
							if (flag8)
							{
								this.myMaterials = this.parentLocal.GetComponentInParent<Renderer>().materials.ToList<Material>();
								Material[] matDefGood5 = new Material[this.myMaterials.Count];
								for (int m = 0; m < this.myMaterials.Count; m++)
								{
									this.evanescoDissolve.SetTexture("_Albedo", this.myMaterials[m].GetTexture("_BaseMap"));
									this.evanescoDissolve.SetColor("_color", this.myMaterials[m].GetColor("_BaseColor"));
									this.evanescoDissolve.SetTexture("_Normal", this.myMaterials[m].GetTexture("_BumpMap"));
									this.evanescoDissolve.SetTexture("_Metallic", this.myMaterials[m].GetTexture("_MetallicGlossMap"));
									matDefGood5[m] = this.evanescoDissolve;
								}
								this.parentLocal.GetComponentInParent<Renderer>().materials = matDefGood5;
								this.cantEvanesco = false;
								this.parentLocal.AddComponent<EvanescoPerItem>();
							}
							else
							{
								bool flag9 = this.parentLocal.GetComponentInChildren<Renderer>() != null;
								if (flag9)
								{
									this.myMaterials = this.parentLocal.GetComponentInChildren<Renderer>().materials.ToList<Material>();
									Material[] matDefGood6 = new Material[this.myMaterials.Count];
									for (int n = 0; n < this.myMaterials.Count; n++)
									{
										this.evanescoDissolve.SetTexture("_Albedo", this.myMaterials[n].GetTexture("_BaseMap"));
										this.evanescoDissolve.SetColor("_color", this.myMaterials[n].GetColor("_BaseColor"));
										this.evanescoDissolve.SetTexture("_Normal", this.myMaterials[n].GetTexture("_BumpMap"));
										this.evanescoDissolve.SetTexture("_Metallic", this.myMaterials[n].GetTexture("_MetallicGlossMap"));
										matDefGood6[n] = this.evanescoDissolve;
									}
									this.parentLocal.GetComponentInChildren<Renderer>().materials = matDefGood6;
									this.cantEvanesco = false;
									this.parentLocal.AddComponent<EvanescoPerItem>();
								}
							}
						}
					}
					else
					{
						this.cantEvanesco = true;
					}
				}
			}
		}

		// Token: 0x04000022 RID: 34
		private Item item;

		// Token: 0x04000023 RID: 35
		private Item npcItem;

		// Token: 0x04000024 RID: 36
		internal Vector3 startPoint;

		// Token: 0x04000025 RID: 37
		internal Vector3 endPoint;

		// Token: 0x04000026 RID: 38
		internal GameObject parentLocal;

		// Token: 0x04000027 RID: 39
		internal Vector3 ogScale;

		// Token: 0x04000028 RID: 40
		internal bool cantEvanesco;

		// Token: 0x04000029 RID: 41
		internal Material evanescoDissolve;

		// Token: 0x0400002A RID: 42
		private List<Material> myMaterials;
	}
}
