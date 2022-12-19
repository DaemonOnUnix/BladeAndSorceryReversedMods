using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace HarmonyLib
{
	// Token: 0x0200002D RID: 45
	internal static class PatchArgumentExtensions
	{
		// Token: 0x060000FE RID: 254 RVA: 0x00009090 File Offset: 0x00007290
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

		// Token: 0x060000FF RID: 255 RVA: 0x000090EB File Offset: 0x000072EB
		private static HarmonyArgument GetArgumentAttribute(this ParameterInfo parameter)
		{
			return PatchArgumentExtensions.AllHarmonyArguments(parameter.GetCustomAttributes(false)).FirstOrDefault<HarmonyArgument>();
		}

		// Token: 0x06000100 RID: 256 RVA: 0x000090FE File Offset: 0x000072FE
		private static HarmonyArgument[] GetArgumentAttributes(this MethodInfo method)
		{
			if (method == null || method is DynamicMethod)
			{
				return null;
			}
			return PatchArgumentExtensions.AllHarmonyArguments(method.GetCustomAttributes(false));
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00009119 File Offset: 0x00007319
		private static HarmonyArgument[] GetArgumentAttributes(this Type type)
		{
			return PatchArgumentExtensions.AllHarmonyArguments(type.GetCustomAttributes(false));
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00009128 File Offset: 0x00007328
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

		// Token: 0x06000103 RID: 259 RVA: 0x00009174 File Offset: 0x00007374
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

		// Token: 0x06000104 RID: 260 RVA: 0x000091EC File Offset: 0x000073EC
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

		// Token: 0x06000105 RID: 261 RVA: 0x00009238 File Offset: 0x00007438
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
