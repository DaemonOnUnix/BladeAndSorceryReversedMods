using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using MonoMod.Utils;

namespace HarmonyLib
{
	// Token: 0x0200007D RID: 125
	public class PatchProcessor
	{
		// Token: 0x06000256 RID: 598 RVA: 0x0000C989 File Offset: 0x0000AB89
		public PatchProcessor(Harmony instance, MethodBase original)
		{
			this.instance = instance;
			this.original = original;
		}

		// Token: 0x06000257 RID: 599 RVA: 0x0000C99F File Offset: 0x0000AB9F
		public PatchProcessor AddPrefix(HarmonyMethod prefix)
		{
			this.prefix = prefix;
			return this;
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000C9A9 File Offset: 0x0000ABA9
		public PatchProcessor AddPrefix(MethodInfo fixMethod)
		{
			this.prefix = new HarmonyMethod(fixMethod);
			return this;
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000C9B8 File Offset: 0x0000ABB8
		public PatchProcessor AddPostfix(HarmonyMethod postfix)
		{
			this.postfix = postfix;
			return this;
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000C9C2 File Offset: 0x0000ABC2
		public PatchProcessor AddPostfix(MethodInfo fixMethod)
		{
			this.postfix = new HarmonyMethod(fixMethod);
			return this;
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000C9D1 File Offset: 0x0000ABD1
		public PatchProcessor AddTranspiler(HarmonyMethod transpiler)
		{
			this.transpiler = transpiler;
			return this;
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000C9DB File Offset: 0x0000ABDB
		public PatchProcessor AddTranspiler(MethodInfo fixMethod)
		{
			this.transpiler = new HarmonyMethod(fixMethod);
			return this;
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0000C9EA File Offset: 0x0000ABEA
		public PatchProcessor AddFinalizer(HarmonyMethod finalizer)
		{
			this.finalizer = finalizer;
			return this;
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0000C9F4 File Offset: 0x0000ABF4
		public PatchProcessor AddFinalizer(MethodInfo fixMethod)
		{
			this.finalizer = new HarmonyMethod(fixMethod);
			return this;
		}

		// Token: 0x0600025F RID: 607 RVA: 0x0000CA04 File Offset: 0x0000AC04
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

		// Token: 0x06000260 RID: 608 RVA: 0x0000CA44 File Offset: 0x0000AC44
		public MethodInfo Patch()
		{
			if (this.original == null)
			{
				throw new NullReferenceException("Null method for " + this.instance.Id);
			}
			if (!this.original.IsDeclaredMember<MethodBase>())
			{
				MethodBase declaredMember = this.original.GetDeclaredMember<MethodBase>();
				throw new ArgumentException("You can only patch implemented methods/constructors. Path the declared method " + declaredMember.FullDescription() + " instead.");
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
				HarmonySharedState.UpdatePatchInfo(this.original, patchInfo);
				methodInfo2 = methodInfo;
			}
			return methodInfo2;
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0000CB88 File Offset: 0x0000AD88
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
				PatchFunctions.UpdateWrapper(this.original, patchInfo);
				HarmonySharedState.UpdatePatchInfo(this.original, patchInfo);
			}
			return this;
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000CC2C File Offset: 0x0000AE2C
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
				PatchFunctions.UpdateWrapper(this.original, patchInfo);
				HarmonySharedState.UpdatePatchInfo(this.original, patchInfo);
			}
			return this;
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000CCA0 File Offset: 0x0000AEA0
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

		// Token: 0x06000264 RID: 612 RVA: 0x0000CD04 File Offset: 0x0000AF04
		public static List<MethodInfo> GetSortedPatchMethods(MethodBase original, Patch[] patches)
		{
			return PatchFunctions.GetSortedPatchMethods(original, patches, false);
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0000CD10 File Offset: 0x0000AF10
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

		// Token: 0x06000266 RID: 614 RVA: 0x0000CD87 File Offset: 0x0000AF87
		public static ILGenerator CreateILGenerator()
		{
			return new DynamicMethodDefinition(string.Format("ILGenerator_{0}", Guid.NewGuid()), typeof(void), new Type[0]).GetILGenerator();
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000CDB8 File Offset: 0x0000AFB8
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

		// Token: 0x06000268 RID: 616 RVA: 0x0000CE48 File Offset: 0x0000B048
		public static List<CodeInstruction> GetOriginalInstructions(MethodBase original, ILGenerator generator = null)
		{
			return MethodCopier.GetInstructions(generator ?? PatchProcessor.CreateILGenerator(original), original, 0);
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000CE5C File Offset: 0x0000B05C
		public static List<CodeInstruction> GetOriginalInstructions(MethodBase original, out ILGenerator generator)
		{
			generator = PatchProcessor.CreateILGenerator(original);
			return MethodCopier.GetInstructions(generator, original, 0);
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000CE6F File Offset: 0x0000B06F
		public static List<CodeInstruction> GetCurrentInstructions(MethodBase original, int maxTranspilers = 2147483647, ILGenerator generator = null)
		{
			return MethodCopier.GetInstructions(generator ?? PatchProcessor.CreateILGenerator(original), original, maxTranspilers);
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000CE83 File Offset: 0x0000B083
		public static List<CodeInstruction> GetCurrentInstructions(MethodBase original, out ILGenerator generator, int maxTranspilers = 2147483647)
		{
			generator = PatchProcessor.CreateILGenerator(original);
			return MethodCopier.GetInstructions(generator, original, maxTranspilers);
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000CE96 File Offset: 0x0000B096
		public static IEnumerable<KeyValuePair<OpCode, object>> ReadMethodBody(MethodBase method)
		{
			return from instr in MethodBodyReader.GetInstructions(PatchProcessor.CreateILGenerator(method), method)
				select new KeyValuePair<OpCode, object>(instr.opcode, instr.operand);
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000CEC8 File Offset: 0x0000B0C8
		public static IEnumerable<KeyValuePair<OpCode, object>> ReadMethodBody(MethodBase method, ILGenerator generator)
		{
			return from instr in MethodBodyReader.GetInstructions(generator, method)
				select new KeyValuePair<OpCode, object>(instr.opcode, instr.operand);
		}

		// Token: 0x04000173 RID: 371
		private readonly Harmony instance;

		// Token: 0x04000174 RID: 372
		private readonly MethodBase original;

		// Token: 0x04000175 RID: 373
		private HarmonyMethod prefix;

		// Token: 0x04000176 RID: 374
		private HarmonyMethod postfix;

		// Token: 0x04000177 RID: 375
		private HarmonyMethod transpiler;

		// Token: 0x04000178 RID: 376
		private HarmonyMethod finalizer;

		// Token: 0x04000179 RID: 377
		internal static readonly object locker = new object();
	}
}
