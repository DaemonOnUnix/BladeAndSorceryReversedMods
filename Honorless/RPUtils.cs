using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Wully
{
	// Token: 0x02000004 RID: 4
	public class RPUtils : MonoBehaviour
	{
		// Token: 0x0600000C RID: 12 RVA: 0x000024C4 File Offset: 0x000006C4
		public static bool TryGetScriptableRendererData(out ScriptableRendererData scriptableRendererData)
		{
			UniversalRenderPipelineAsset pipeline = (UniversalRenderPipelineAsset)GraphicsSettings.renderPipelineAsset;
			FieldInfo propertyInfo = pipeline.GetType().GetField("m_RendererDataList", BindingFlags.Instance | BindingFlags.NonPublic);
			ScriptableRendererData[] array = (ScriptableRendererData[])((propertyInfo != null) ? propertyInfo.GetValue(pipeline) : null);
			scriptableRendererData = ((array != null) ? array[0] : null);
			return scriptableRendererData != null;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002514 File Offset: 0x00000714
		public static int GetRendererIndex(string name)
		{
			int idx = -1;
			ScriptableRendererData scriptableRendererData;
			if (RPUtils.TryGetScriptableRendererData(out scriptableRendererData))
			{
				int i = 0;
				for (;;)
				{
					int num = i;
					int? num2 = ((scriptableRendererData != null) ? new int?(scriptableRendererData.rendererFeatures.Count) : null);
					if (!((num < num2.GetValueOrDefault()) & (num2 != null)))
					{
						return idx;
					}
					if (scriptableRendererData.rendererFeatures[i].name.Equals(name))
					{
						break;
					}
					i++;
				}
				scriptableRendererData.rendererFeatures[i].SetActive(false);
				idx = i;
			}
			return idx;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002598 File Offset: 0x00000798
		public static bool TryGetFeature(string name, out ScriptableRendererFeature feature, out ScriptableRendererData scriptableRendererData)
		{
			feature = null;
			if (RPUtils.TryGetScriptableRendererData(out scriptableRendererData))
			{
				feature = scriptableRendererData.rendererFeatures.Where((ScriptableRendererFeature f) => f.name == name).First<ScriptableRendererFeature>();
			}
			return feature != null && scriptableRendererData != null;
		}
	}
}
