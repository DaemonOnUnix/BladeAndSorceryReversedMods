using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Wully.Render
{
	// Token: 0x02000008 RID: 8
	[ExecuteInEditMode]
	public class CustomPass<T> : MonoBehaviour
	{
		// Token: 0x06000024 RID: 36 RVA: 0x00002C5A File Offset: 0x00000E5A
		private void GetScriptableRenderer()
		{
			Debug.Log("Getting scriptable render data");
			if (RPUtils.TryGetScriptableRendererData(out this.scriptableRendererData))
			{
				Debug.Log("Got scriptable render data");
				this.forwardRendererData = (ForwardRendererData)this.scriptableRendererData;
				return;
			}
			Debug.LogError("Failed to get scriptable render data");
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002C99 File Offset: 0x00000E99
		public virtual void Awake()
		{
			this.GetScriptableRenderer();
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002CA1 File Offset: 0x00000EA1
		public virtual void OnDisable()
		{
			this.DestroyFeature();
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002CA9 File Offset: 0x00000EA9
		public virtual void UpdateSettings()
		{
			ScriptableRendererData scriptableRendererData = this.scriptableRendererData;
			if (scriptableRendererData == null)
			{
				return;
			}
			scriptableRendererData.SetDirty();
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002CBC File Offset: 0x00000EBC
		public virtual void CreateFeature()
		{
			Debug.Log("Creating feature..");
			if (this.feature != null)
			{
				this.DestroyFeature();
			}
			this.feature = (ScriptableRendererFeature)ScriptableObject.CreateInstance(typeof(T));
			if (this.feature)
			{
				this.featureName = base.gameObject.name;
				this.feature.name = this.featureName;
				this.feature.SetActive(false);
				if (!this.scriptableRendererData)
				{
					this.GetScriptableRenderer();
				}
				this.UpdateSettings();
				ScriptableRendererData scriptableRendererData = this.scriptableRendererData;
				if (scriptableRendererData != null)
				{
					List<ScriptableRendererFeature> rendererFeatures = scriptableRendererData.rendererFeatures;
					if (rendererFeatures != null)
					{
						rendererFeatures.Add(this.feature);
					}
				}
				ScriptableRendererData scriptableRendererData2 = this.scriptableRendererData;
				if (scriptableRendererData2 != null)
				{
					scriptableRendererData2.SetDirty();
				}
				Debug.Log("Created feature: " + this.feature.name);
				return;
			}
			Debug.LogError("Unable to create feature!");
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002DB0 File Offset: 0x00000FB0
		public virtual void DestroyFeature()
		{
			Debug.Log("Attempting to destroy feature: " + this.featureName);
			ScriptableRendererFeature scriptableRendererFeature = this.feature;
			if (scriptableRendererFeature != null)
			{
				scriptableRendererFeature.SetActive(false);
			}
			int idx = RPUtils.GetRendererIndex(this.featureName);
			if (idx < 0)
			{
				Debug.LogError("Could not find feature to destroy feature: " + this.featureName);
				return;
			}
			Debug.Log("Destroying feature: " + this.featureName);
			ScriptableRendererData scriptableRendererData = this.scriptableRendererData;
			if (scriptableRendererData == null)
			{
				return;
			}
			scriptableRendererData.rendererFeatures.RemoveAt(idx);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002E35 File Offset: 0x00001035
		public virtual void DisableFeature()
		{
			Debug.Log("Attempting to disable feature: " + this.featureName);
			ScriptableRendererFeature scriptableRendererFeature = this.feature;
			if (scriptableRendererFeature != null)
			{
				scriptableRendererFeature.SetActive(false);
			}
			ScriptableRendererData scriptableRendererData = this.scriptableRendererData;
			if (scriptableRendererData == null)
			{
				return;
			}
			scriptableRendererData.SetDirty();
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002E70 File Offset: 0x00001070
		public virtual void EnableFeature()
		{
			Debug.Log("Attempting to enable feature: " + this.featureName);
			if (this.feature == null)
			{
				this.CreateFeature();
			}
			else if (RPUtils.GetRendererIndex(this.featureName) == -1)
			{
				Debug.Log("Adding feature: " + this.featureName);
				ScriptableRendererData scriptableRendererData = this.scriptableRendererData;
				if (scriptableRendererData != null)
				{
					List<ScriptableRendererFeature> rendererFeatures = scriptableRendererData.rendererFeatures;
					if (rendererFeatures != null)
					{
						rendererFeatures.Add(this.feature);
					}
				}
			}
			this.UpdateSettings();
			this.feature.SetActive(true);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002EFF File Offset: 0x000010FF
		public virtual void OnDestroy()
		{
			this.DestroyFeature();
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002F07 File Offset: 0x00001107
		public virtual void OnValidate()
		{
			this.UpdateSettings();
		}

		// Token: 0x04000029 RID: 41
		public ScriptableRendererFeature feature;

		// Token: 0x0400002A RID: 42
		public ScriptableRendererData scriptableRendererData;

		// Token: 0x0400002B RID: 43
		public ForwardRendererData forwardRendererData;

		// Token: 0x0400002C RID: 44
		private string featureName;
	}
}
