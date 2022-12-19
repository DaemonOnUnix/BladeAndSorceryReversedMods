using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using MonoMod.RuntimeDetour;
using MonoMod.Utils;

namespace HarmonyLib
{
	// Token: 0x02000019 RID: 25
	internal static class HarmonySharedState
	{
		// Token: 0x0600008E RID: 142 RVA: 0x00004684 File Offset: 0x00002884
		static HarmonySharedState()
		{
			Type orCreateSharedStateType = HarmonySharedState.GetOrCreateSharedStateType();
			FieldInfo field = orCreateSharedStateType.GetField("version");
			if ((int)field.GetValue(null) == 0)
			{
				field.SetValue(null, 101);
			}
			HarmonySharedState.actualVersion = (int)field.GetValue(null);
			FieldInfo field2 = orCreateSharedStateType.GetField("state");
			if (field2.GetValue(null) == null)
			{
				field2.SetValue(null, new Dictionary<MethodBase, byte[]>());
			}
			FieldInfo field3 = orCreateSharedStateType.GetField("originals");
			if (field3 != null && field3.GetValue(null) == null)
			{
				field3.SetValue(null, new Dictionary<MethodInfo, MethodBase>());
			}
			HarmonySharedState.state = (Dictionary<MethodBase, byte[]>)field2.GetValue(null);
			HarmonySharedState.originals = new Dictionary<MethodInfo, MethodBase>();
			if (field3 != null)
			{
				HarmonySharedState.originals = (Dictionary<MethodInfo, MethodBase>)field3.GetValue(null);
			}
			DetourHelper.Runtime.OnMethodCompiled += delegate(MethodBase method, IntPtr codeStart, ulong codeLen)
			{
				if (method == null)
				{
					return;
				}
				PatchInfo patchInfo = HarmonySharedState.GetPatchInfo(method);
				if (patchInfo == null)
				{
					return;
				}
				PatchFunctions.UpdateRecompiledMethod(method, codeStart, patchInfo);
			};
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00004784 File Offset: 0x00002984
		private static Type GetOrCreateSharedStateType()
		{
			Type type = Type.GetType("HarmonySharedState", false);
			if (type != null)
			{
				return type;
			}
			Type type2;
			using (ModuleDefinition moduleDefinition = ModuleDefinition.CreateModule("HarmonySharedState", new ModuleParameters
			{
				Kind = ModuleKind.Dll,
				ReflectionImporterProvider = MMReflectionImporter.Provider
			}))
			{
				Mono.Cecil.TypeAttributes typeAttributes = Mono.Cecil.TypeAttributes.Public | Mono.Cecil.TypeAttributes.Abstract | Mono.Cecil.TypeAttributes.Sealed;
				TypeDefinition typeDefinition = new TypeDefinition("", "HarmonySharedState", typeAttributes)
				{
					BaseType = moduleDefinition.TypeSystem.Object
				};
				moduleDefinition.Types.Add(typeDefinition);
				typeDefinition.Fields.Add(new FieldDefinition("state", Mono.Cecil.FieldAttributes.FamANDAssem | Mono.Cecil.FieldAttributes.Family | Mono.Cecil.FieldAttributes.Static, moduleDefinition.ImportReference(typeof(Dictionary<MethodBase, byte[]>))));
				typeDefinition.Fields.Add(new FieldDefinition("originals", Mono.Cecil.FieldAttributes.FamANDAssem | Mono.Cecil.FieldAttributes.Family | Mono.Cecil.FieldAttributes.Static, moduleDefinition.ImportReference(typeof(Dictionary<MethodInfo, MethodBase>))));
				typeDefinition.Fields.Add(new FieldDefinition("version", Mono.Cecil.FieldAttributes.FamANDAssem | Mono.Cecil.FieldAttributes.Family | Mono.Cecil.FieldAttributes.Static, moduleDefinition.ImportReference(typeof(int))));
				type2 = ReflectionHelper.Load(moduleDefinition).GetType("HarmonySharedState");
			}
			return type2;
		}

		// Token: 0x06000090 RID: 144 RVA: 0x000048A4 File Offset: 0x00002AA4
		internal static PatchInfo GetPatchInfo(MethodBase method)
		{
			Dictionary<MethodBase, byte[]> dictionary = HarmonySharedState.state;
			byte[] valueSafe;
			lock (dictionary)
			{
				valueSafe = HarmonySharedState.state.GetValueSafe(method);
			}
			if (valueSafe == null)
			{
				return null;
			}
			return PatchInfoSerialization.Deserialize(valueSafe);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x000048F4 File Offset: 0x00002AF4
		internal static IEnumerable<MethodBase> GetPatchedMethods()
		{
			Dictionary<MethodBase, byte[]> dictionary = HarmonySharedState.state;
			IEnumerable<MethodBase> enumerable;
			lock (dictionary)
			{
				enumerable = HarmonySharedState.state.Keys.ToArray<MethodBase>();
			}
			return enumerable;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00004940 File Offset: 0x00002B40
		internal static void UpdatePatchInfo(MethodBase original, MethodInfo replacement, PatchInfo patchInfo)
		{
			byte[] array = patchInfo.Serialize();
			Dictionary<MethodBase, byte[]> dictionary = HarmonySharedState.state;
			lock (dictionary)
			{
				HarmonySharedState.state[original] = array;
			}
			Dictionary<MethodInfo, MethodBase> dictionary2 = HarmonySharedState.originals;
			lock (dictionary2)
			{
				HarmonySharedState.originals[replacement] = original;
			}
		}

		// Token: 0x06000093 RID: 147 RVA: 0x000049C0 File Offset: 0x00002BC0
		internal static MethodBase GetOriginal(MethodInfo replacement)
		{
			Dictionary<MethodInfo, MethodBase> dictionary = HarmonySharedState.originals;
			MethodBase valueSafe;
			lock (dictionary)
			{
				valueSafe = HarmonySharedState.originals.GetValueSafe(replacement);
			}
			return valueSafe;
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00004A08 File Offset: 0x00002C08
		internal static MethodBase FindReplacement(StackFrame frame)
		{
			MethodBase method = frame.GetMethod();
			long methodStart = 0L;
			if (method == null || method.IsGenericMethod)
			{
				if (HarmonySharedState.methodAddress == null)
				{
					return null;
				}
				methodStart = (long)HarmonySharedState.methodAddress.GetValue(frame);
			}
			else
			{
				MethodBase identifiable = DetourHelper.Runtime.GetIdentifiable(method);
				methodStart = identifiable.GetNativeStart().ToInt64();
			}
			if (methodStart == 0L)
			{
				return method;
			}
			Dictionary<MethodInfo, MethodBase> dictionary = HarmonySharedState.originals;
			MethodBase methodBase;
			lock (dictionary)
			{
				methodBase = HarmonySharedState.originals.Keys.FirstOrDefault((MethodInfo replacement) => replacement.GetNativeStart().ToInt64() == methodStart);
			}
			return methodBase;
		}

		// Token: 0x04000049 RID: 73
		private const string name = "HarmonySharedState";

		// Token: 0x0400004A RID: 74
		internal const int internalVersion = 101;

		// Token: 0x0400004B RID: 75
		private static readonly Dictionary<MethodBase, byte[]> state;

		// Token: 0x0400004C RID: 76
		private static readonly Dictionary<MethodInfo, MethodBase> originals;

		// Token: 0x0400004D RID: 77
		internal static readonly int actualVersion;

		// Token: 0x0400004E RID: 78
		private static readonly FieldInfo methodAddress = typeof(StackFrame).GetField("methodAddress", BindingFlags.Instance | BindingFlags.NonPublic);
	}
}
