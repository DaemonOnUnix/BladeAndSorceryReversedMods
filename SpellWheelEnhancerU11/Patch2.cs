using System;
using HarmonyLib;
using ThunderRoad;
using UnityEngine;

namespace SpellWheelEnhancer
{
	// Token: 0x02000005 RID: 5
	[HarmonyPatch(typeof(WheelMenu), "OnOrbSelected")]
	internal class Patch2
	{
		// Token: 0x06000006 RID: 6 RVA: 0x000020B4 File Offset: 0x000002B4
		private static bool Prefix(WheelMenu.Orb orb, bool active)
		{
			if (active)
			{
				TextMesh textMesh = new GameObject("orbText").AddComponent<TextMesh>();
				textMesh.transform.parent = orb.transform;
				textMesh.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
				textMesh.transform.localPosition = Vector3.zero;
				textMesh.fontSize = SpellWheelGlobalVariables.fontSize;
				textMesh.transform.localScale /= 160f;
				textMesh.alignment = 1;
				textMesh.anchor = 4;
				ContainerData data = Catalog.GetData<ContainerData>("PlayerDefault", true);
				foreach (ContainerData.Content content in Player.currentCreature.container.contents)
				{
					bool flag = orb.linkedObject == content;
					if (flag)
					{
						textMesh.text = content.itemData.displayName;
						string text = null;
						bool flag2 = text != null;
						Color white;
						if (flag2)
						{
							ColorUtility.TryParseHtmlString(text, ref white);
						}
						else
						{
							white = Color.white;
						}
						textMesh.color = white;
						break;
					}
				}
			}
			else
			{
				TextMesh componentInChildren = orb.transform.gameObject.GetComponentInChildren<TextMesh>();
				bool flag3 = componentInChildren;
				if (flag3)
				{
					Object.Destroy(componentInChildren);
				}
			}
			return true;
		}
	}
}
