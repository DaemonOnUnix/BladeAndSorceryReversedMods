using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace HarmonyLib
{
	// Token: 0x0200002C RID: 44
	internal static class PatchArgumentExtensions
	{
		// Token: 0x060000F0 RID: 240 RVA: 0x00008314 File Offset: 0x00006514
		private static HarmonyArgument[] AllHarmonyArguments(object[] attributes)
		{
			return (from harg in attributes.Select(delegate(object attr)
				{
					if (attr.GetType().Name != "HarmonyArgument")
					{
						return null;
					}
					return AccessTools.MakeDeepCopy<HarmonyArgument>(attr);
				})
				where harg != null
				select harg).ToArray<HarmonyArgument>();
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x0000836F File Offset: 0x0000656F
		private static HarmonyArgument GetArgumentAttribute(this ParameterInfo parameter)
		{
			return PatchArgumentExtensions.AllHarmonyArguments(parameter.GetCustomAttributes(false)).FirstOrDefault<HarmonyArgument>();
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00008382 File Offset: 0x00006582
		private static HarmonyArgument[] GetArgumentAttributes(this MethodInfo method)
		{
			if (method == null || method is DynamicMethod)
			{
				return null;
			}
			return PatchArgumentExtensions.AllHarmonyArguments(method.GetCustomAttributes(false));
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x0000839D File Offset: 0x0000659D
		private static HarmonyArgument[] GetArgumentAttributes(this Type type)
		{
			return PatchArgumentExtensions.AllHarmonyArguments(type.GetCustomAttributes(false));
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x000083AC File Offset: 0x000065AC
		private static string GetOriginalArgumentName(this ParameterInfo parameter, string[] originalParameterNames)
		{
			HarmonyArgument argumentAttribute = parameter.GetArgumentAttribute();
			if (argumentAttribute == null)
			{
				return null;
			}
			if (!string.IsNullOrEmpty(argumentAttribute.OriginalName))
			{
				return argumentAttribute.OriginalName;
			}
			if (argumentAttribute.Index >= 0 && argumentAttribute.Index < originalParameterNames.Length)
			{
				return originalParameterNames[argumentAttribute.Index];
			}
			return null;
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x000083F8 File Offset: 0x000065F8
		private static string GetOriginalArgumentName(HarmonyArgument[] attributes, string name, string[] originalParameterNames)
		{
			if (((attributes != null) ? attributes.Length : 0) <= 0)
			{
				return null;
			}
			HarmonyArgument harmonyArgument = attributes.SingleOrDefault((HarmonyArgument p) => p.NewName == name);
			if (harmonyArgument == null)
			{
				return null;
			}
			if (!string.IsNullOrEmpty(harmonyArgument.OriginalName))
			{
				return harmonyArgument.OriginalName;
			}
			if (originalParameterNames != null && harmonyArgument.Index >= 0 && harmonyArgument.Index < originalParameterNames.Length)
			{
				return originalParameterNames[harmonyArgument.Index];
			}
			return null;
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00008470 File Offset: 0x00006670
		private static string GetOriginalArgumentName(this MethodInfo method, string[] originalParameterNames, string name)
		{
			string text = PatchArgumentExtensions.GetOriginalArgumentName((method != null) ? method.GetArgumentAttributes() : null, name, originalParameterNames);
			if (text != null)
			{
				return text;
			}
			HarmonyArgument[] array;
			if (method == null)
			{
				array = null;
			}
			else
			{
				Type declaringType = method.DeclaringType;
				array = ((declaringType != null) ? declaringType.GetArgumentAttributes() : null);
			}
			text = PatchArgumentExtensions.GetOriginalArgumentName(array, name, originalParameterNames);
			if (text != null)
			{
				return text;
			}
			return name;
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x000084BC File Offset: 0x000066BC
		internal static int GetArgumentIndex(this MethodInfo patch, string[] originalParameterNames, ParameterInfo patchParam)
		{
			if (patch is DynamicMethod)
			{
				return Array.IndexOf<string>(originalParameterNames, patchParam.Name);
			}
			string text = patchParam.GetOriginalArgumentName(originalParameterNames);
			if (text != null)
			{
				return Array.IndexOf<string>(originalParameterNames, text);
			}
			text = patch.GetOriginalArgumentName(originalParameterNames, patchParam.Name);
			if (text != null)
			{
				return Array.IndexOf<string>(originalParameterNames, text);
			}
			return -1;
		}
	}
}
