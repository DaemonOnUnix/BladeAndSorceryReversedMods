using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using MonoMod.Utils;

namespace HarmonyLib
{
	// Token: 0x02000080 RID: 128
	public class PatchProcessor
	{
		// Token: 0x06000273 RID: 627 RVA: 0x0000DC11 File Offset: 0x0000BE11
		public PatchProcessor(Harmony instance, MethodBase original)
		{
			this.instance = instance;
			this.original = original;
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000DC27 File Offset: 0x0000BE27
		public PatchProcessor AddPrefix(HarmonyMethod prefix)
		{
			this.prefix = prefix;
			return this;
		}

		// Token: 0x06000275 RID: 629 RVA: 0x0000DC31 File Offset: 0x0000BE31
		public PatchProcessor AddPrefix(MethodInfo fixMethod)
		{
			this.prefix = new HarmonyMethod(fixMethod);
			return this;
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000DC40 File Offset: 0x0000BE40
		public PatchProcessor AddPostfix(HarmonyMethod postfix)
		{
			this.postfix = postfix;
			return this;
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000DC4A File Offset: 0x0000BE4A
		public PatchProcessor AddPostfix(MethodInfo fixMethod)
		{
			this.postfix = new HarmonyMethod(fixMethod);
			return this;
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000DC59 File Offset: 0x0000BE59
		public PatchProcessor AddTranspiler(HarmonyMethod transpiler)
		{
			this.transpiler = transpiler;
			return this;
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000DC63 File Offset: 0x0000BE63
		public PatchProcessor AddTranspiler(MethodInfo fixMethod)
		{
			this.transpiler = new HarmonyMethod(fixMethod);
			return this;
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000DC72 File Offset: 0x0000BE72
		public PatchProcessor AddFinalizer(HarmonyMethod finalizer)
		{
			this.finalizer = finalizer;
			return this;
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000DC7C File Offset: 0x0000BE7C
		public PatchProcessor AddFinalizer(MethodInfo fixMethod)
		{
			this.finalizer = new HarmonyMethod(fixMethod);
			return this;
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000DC8C File Offset: 0x0000BE8C
		public static IEnumerable<MethodBase> GetAllPatchedMethods()
		{
			object obj = PatchProcessor.locker;
			IEnumerable<MethodBase> patchedMethods;
			lock (obj)
			{
				patchedMethods = HarmonySharedState.GetPatchedMethods();
			}
			return patchedMethods;
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000DCCC File Offset: 0x0000BECC
		public MethodInfo Patch()
		{
			if (this.original == null)
			{
				throw new NullReferenceException("Null method for " + this.instance.Id);
			}
			if (!this.original.IsDeclaredMember<MethodBase>())
			{
				MethodBase declaredMember = this.original.GetDeclaredMember<MethodBase>();
				throw new ArgumentException("You can only patch implemented methods/constructors. Patch the declared method " + declaredMember.FullDescription() + " instead.");
			}
			object obj = PatchProcessor.locker;
			MethodInfo methodInfo2;
			lock (obj)
			{
				PatchInfo patchInfo = HarmonySharedState.GetPatchInfo(this.original) ?? new PatchInfo();
				patchInfo.AddPrefixes(this.instance.Id, new HarmonyMethod[] { this.prefix });
				patchInfo.AddPostfixes(this.instance.Id, new HarmonyMethod[] { this.postfix });
				patchInfo.AddTranspilers(this.instance.Id, new HarmonyMethod[] { this.transpiler });
				patchInfo.AddFinalizers(this.instance.Id, new HarmonyMethod[] { this.finalizer });
				MethodInfo methodInfo = PatchFunctions.UpdateWrapper(this.original, patchInfo);
				HarmonySharedState.UpdatePatchInfo(this.original, methodInfo, patchInfo);
				methodInfo2 = methodInfo;
			}
			return methodInfo2;
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000DE14 File Offset: 0x0000C014
		public PatchProcessor Unpatch(HarmonyPatchType type, string harmonyID)
		{
			object obj = PatchProcessor.locker;
			lock (obj)
			{
				PatchInfo patchInfo = HarmonySharedState.GetPatchInfo(this.original);
				if (patchInfo == null)
				{
					patchInfo = new PatchInfo();
				}
				if (type == HarmonyPatchType.All || type == HarmonyPatchType.Prefix)
				{
					patchInfo.RemovePrefix(harmonyID);
				}
				if (type == HarmonyPatchType.All || type == HarmonyPatchType.Postfix)
				{
					patchInfo.RemovePostfix(harmonyID);
				}
				if (type == HarmonyPatchType.All || type == HarmonyPatchType.Transpiler)
				{
					patchInfo.RemoveTranspiler(harmonyID);
				}
				if (type == HarmonyPatchType.All || type == HarmonyPatchType.Finalizer)
				{
					patchInfo.RemoveFinalizer(harmonyID);
				}
				MethodInfo methodInfo = PatchFunctions.UpdateWrapper(this.original, patchInfo);
				HarmonySharedState.UpdatePatchInfo(this.original, methodInfo, patchInfo);
			}
			return this;
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000DEBC File Offset: 0x0000C0BC
		public PatchProcessor Unpatch(MethodInfo patch)
		{
			object obj = PatchProcessor.locker;
			lock (obj)
			{
				PatchInfo patchInfo = HarmonySharedState.GetPatchInfo(this.original);
				if (patchInfo == null)
				{
					patchInfo = new PatchInfo();
				}
				patchInfo.RemovePatch(patch);
				MethodInfo methodInfo = PatchFunctions.UpdateWrapper(this.original, patchInfo);
				HarmonySharedState.UpdatePatchInfo(this.original, methodInfo, patchInfo);
			}
			return this;
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000DF30 File Offset: 0x0000C130
		public static Patches GetPatchInfo(MethodBase method)
		{
			object obj = PatchProcessor.locker;
			PatchInfo patchInfo;
			lock (obj)
			{
				patchInfo = HarmonySharedState.GetPatchInfo(method);
			}
			if (patchInfo == null)
			{
				return null;
			}
			return new Patches(patchInfo.prefixes, patchInfo.postfixes, patchInfo.transpilers, patchInfo.finalizers);
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000DF94 File Offset: 0x0000C194
		public static List<MethodInfo> GetSortedPatchMethods(MethodBase original, Patch[] patches)
		{
			return PatchFunctions.GetSortedPatchMethods(original, patches, false);
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000DFA0 File Offset: 0x0000C1A0
		public static Dictionary<string, Version> VersionInfo(out Version currentVersion)
		{
			currentVersion = typeof(Harmony).Assembly.GetName().Version;
			Dictionary<string, Assembly> assemblies = new Dictionary<string, Assembly>();
			Action<Patch> <>9__2;
			Action<Patch> <>9__3;
			Action<Patch> <>9__4;
			Action<Patch> <>9__5;
			PatchProcessor.GetAllPatchedMethods().Do(delegate(MethodBase method)
			{
				object obj = PatchProcessor.locker;
				PatchInfo patchInfo;
				lock (obj)
				{
					patchInfo = HarmonySharedState.GetPatchInfo(method);
				}
				IEnumerable<Patch> prefixes = patchInfo.prefixes;
				Action<Patch> action;
				if ((action = <>9__2) == null)
				{
					action = (<>9__2 = delegate(Patch fix)
					{
						assemblies[fix.owner] = fix.PatchMethod.DeclaringType.Assembly;
					});
				}
				prefixes.Do(action);
				IEnumerable<Patch> postfixes = patchInfo.postfixes;
				Action<Patch> action2;
				if ((action2 = <>9__3) == null)
				{
					action2 = (<>9__3 = delegate(Patch fix)
					{
						assemblies[fix.owner] = fix.PatchMethod.DeclaringType.Assembly;
					});
				}
				postfixes.Do(action2);
				IEnumerable<Patch> transpilers = patchInfo.transpilers;
				Action<Patch> action3;
				if ((action3 = <>9__4) == null)
				{
					action3 = (<>9__4 = delegate(Patch fix)
					{
						assemblies[fix.owner] = fix.PatchMethod.DeclaringType.Assembly;
					});
				}
				transpilers.Do(action3);
				IEnumerable<Patch> finalizers = patchInfo.finalizers;
				Action<Patch> action4;
				if ((action4 = <>9__5) == null)
				{
					action4 = (<>9__5 = delegate(Patch fix)
					{
						assemblies[fix.owner] = fix.PatchMethod.DeclaringType.Assembly;
					});
				}
				finalizers.Do(action4);
			});
			Dictionary<string, Version> result = new Dictionary<string, Version>();
			assemblies.Do(delegate(KeyValuePair<string, Assembly> info)
			{
				AssemblyName assemblyName = info.Value.GetReferencedAssemblies().FirstOrDefault((AssemblyName a) => a.FullName.StartsWith("0Harmony, Version", StringComparison.Ordinal));
				if (assemblyName != null)
				{
					result[info.Key] = assemblyName.Version;
				}
			});
			return result;
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000E017 File Offset: 0x0000C217
		public static ILGenerator CreateILGenerator()
		{
			return new DynamicMethodDefinition(string.Format("ILGenerator_{0}", Guid.NewGuid()), typeof(void), new Type[0]).GetILGenerator();
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000E048 File Offset: 0x0000C248
		public static ILGenerator CreateILGenerator(MethodBase original)
		{
			MethodInfo methodInfo = original as MethodInfo;
			Type type = ((methodInfo != null) ? methodInfo.ReturnType : typeof(void));
			List<Type> list = (from pi in original.GetParameters()
				select pi.ParameterType).ToList<Type>();
			if (!original.IsStatic)
			{
				list.Insert(0, original.DeclaringType);
			}
			return new DynamicMethodDefinition("ILGenerator_" + original.Name, type, list.ToArray()).GetILGenerator();
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000E0D8 File Offset: 0x0000C2D8
		public static List<CodeInstruction> GetOriginalInstructions(MethodBase original, ILGenerator generator = null)
		{
			return MethodCopier.GetInstructions(generator ?? PatchProcessor.CreateILGenerator(original), original, 0);
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000E0EC File Offset: 0x0000C2EC
		public static List<CodeInstruction> GetOriginalInstructions(MethodBase original, out ILGenerator generator)
		{
			generator = PatchProcessor.CreateILGenerator(original);
			return MethodCopier.GetInstructions(generator, original, 0);
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000E0FF File Offset: 0x0000C2FF
		public static List<CodeInstruction> GetCurrentInstructions(MethodBase original, int maxTranspilers = 2147483647, ILGenerator generator = null)
		{
			return MethodCopier.GetInstructions(generator ?? PatchProcessor.CreateILGenerator(original), original, maxTranspilers);
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000E113 File Offset: 0x0000C313
		public static List<CodeInstruction> GetCurrentInstructions(MethodBase original, out ILGenerator generator, int maxTranspilers = 2147483647)
		{
			generator = PatchProcessor.CreateILGenerator(original);
			return MethodCopier.GetInstructions(generator, original, maxTranspilers);
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000E126 File Offset: 0x0000C326
		public static IEnumerable<KeyValuePair<OpCode, object>> ReadMethodBody(MethodBase method)
		{
			return from instr in MethodBodyReader.GetInstructions(PatchProcessor.CreateILGenerator(method), method)
				select new KeyValuePair<OpCode, object>(instr.opcode, instr.operand);
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000E158 File Offset: 0x0000C358
		public static IEnumerable<KeyValuePair<OpCode, object>> ReadMethodBody(MethodBase method, ILGenerator generator)
		{
			return from instr in MethodBodyReader.GetInstructions(generator, method)
				select new KeyValuePair<OpCode, object>(instr.opcode, instr.operand);
		}

		// Token: 0x04000185 RID: 389
		private readonly Harmony instance;

		// Token: 0x04000186 RID: 390
		private readonly MethodBase original;

		// Token: 0x04000187 RID: 391
		private HarmonyMethod prefix;

		// Token: 0x04000188 RID: 392
		private HarmonyMethod postfix;

		// Token: 0x04000189 RID: 393
		private HarmonyMethod transpiler;

		// Token: 0x0400018A RID: 394
		private HarmonyMethod finalizer;

		// Token: 0x0400018B RID: 395
		internal static readonly object locker = new object();
	}
}
