using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HarmonyLib
{
	// Token: 0x02000033 RID: 51
	internal class AttributePatch
	{
		// Token: 0x0600010B RID: 267 RVA: 0x000088FC File Offset: 0x00006AFC
		internal static AttributePatch Create(MethodInfo patch)
		{
			if (patch == null)
			{
				throw new NullReferenceException("Patch method cannot be null");
			}
			object[] customAttributes = patch.GetCustomAttributes(true);
			HarmonyPatchType? patchType = AttributePatch.GetPatchType(patch.Name, customAttributes);
			if (patchType == null)
			{
				return null;
			}
			HarmonyPatchType? harmonyPatchType = patchType;
			HarmonyPatchType harmonyPatchType2 = HarmonyPatchType.ReversePatch;
			if (!((harmonyPatchType.GetValueOrDefault() == harmonyPatchType2) & (harmonyPatchType != null)) && !patch.IsStatic)
			{
				throw new ArgumentException("Patch method " + patch.FullDescription() + " must be static");
			}
			HarmonyMethod harmonyMethod = HarmonyMethod.Merge((from attr in customAttributes
				where attr.GetType().BaseType.FullName == AttributePatch.harmonyAttributeName
				select AccessTools.Field(attr.GetType(), "info").GetValue(attr) into harmonyInfo
				select AccessTools.MakeDeepCopy<HarmonyMethod>(harmonyInfo)).ToList<HarmonyMethod>());
			harmonyMethod.method = patch;
			return new AttributePatch
			{
				info = harmonyMethod,
				type = patchType
			};
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00008A08 File Offset: 0x00006C08
		private static HarmonyPatchType? GetPatchType(string methodName, object[] allAttributes)
		{
			HashSet<string> hashSet = new HashSet<string>(from attr in allAttributes
				select attr.GetType().FullName into name
				where name.StartsWith("Harmony")
				select name);
			HarmonyPatchType? harmonyPatchType = null;
			foreach (HarmonyPatchType harmonyPatchType2 in AttributePatch.allPatchTypes)
			{
				string text = harmonyPatchType2.ToString();
				if (text == methodName || hashSet.Contains("HarmonyLib.Harmony" + text))
				{
					harmonyPatchType = new HarmonyPatchType?(harmonyPatchType2);
					break;
				}
			}
			return harmonyPatchType;
		}

		// Token: 0x040000B1 RID: 177
		private static readonly HarmonyPatchType[] allPatchTypes = new HarmonyPatchType[]
		{
			HarmonyPatchType.Prefix,
			HarmonyPatchType.Postfix,
			HarmonyPatchType.Transpiler,
			HarmonyPatchType.Finalizer,
			HarmonyPatchType.ReversePatch
		};

		// Token: 0x040000B2 RID: 178
		internal HarmonyMethod info;

		// Token: 0x040000B3 RID: 179
		internal HarmonyPatchType? type;

		// Token: 0x040000B4 RID: 180
		private static readonly string harmonyAttributeName = typeof(HarmonyAttribute).FullName;
	}
}
