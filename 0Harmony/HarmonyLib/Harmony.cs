using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using MonoMod.Utils;

namespace HarmonyLib
{
	// Token: 0x02000060 RID: 96
	public class Harmony
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x060001B4 RID: 436 RVA: 0x0000B471 File Offset: 0x00009671
		// (set) Token: 0x060001B5 RID: 437 RVA: 0x0000B479 File Offset: 0x00009679
		public string Id { get; private set; }

		// Token: 0x060001B6 RID: 438 RVA: 0x0000B484 File Offset: 0x00009684
		public Harmony(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				throw new ArgumentException("id cannot be null or empty");
			}
			try
			{
				string text = Environment.GetEnvironmentVariable("HARMONY_DEBUG");
				if (text != null && text.Length > 0)
				{
					text = text.Trim();
					Harmony.DEBUG = text == "1" || bool.Parse(text);
				}
			}
			catch
			{
			}
			if (Harmony.DEBUG)
			{
				Assembly assembly = typeof(Harmony).Assembly;
				Version version = assembly.GetName().Version;
				string text2 = assembly.Location;
				string text3 = Environment.Version.ToString();
				string text4 = Environment.OSVersion.Platform.ToString();
				if (string.IsNullOrEmpty(text2))
				{
					text2 = new Uri(assembly.CodeBase).LocalPath;
				}
				int size = IntPtr.Size;
				Platform platform = PlatformHelper.Current;
				FileLog.Log(string.Format("### Harmony id={0}, version={1}, location={2}, env/clr={3}, platform={4}, ptrsize:runtime/env={5}/{6}", new object[] { id, version, text2, text3, text4, size, platform }));
				MethodBase outsideCaller = AccessTools.GetOutsideCaller();
				if (outsideCaller.DeclaringType != null)
				{
					Assembly assembly2 = outsideCaller.DeclaringType.Assembly;
					text2 = assembly2.Location;
					if (string.IsNullOrEmpty(text2))
					{
						text2 = new Uri(assembly2.CodeBase).LocalPath;
					}
					FileLog.Log("### Started from " + outsideCaller.FullDescription() + ", location " + text2);
					FileLog.Log(string.Format("### At {0:yyyy-MM-dd hh.mm.ss}", DateTime.Now));
				}
			}
			this.Id = id;
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x0000B630 File Offset: 0x00009830
		public void PatchAll()
		{
			Assembly assembly = new StackTrace().GetFrame(1).GetMethod().ReflectedType.Assembly;
			this.PatchAll(assembly);
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x0000B65F File Offset: 0x0000985F
		public PatchProcessor CreateProcessor(MethodBase original)
		{
			return new PatchProcessor(this, original);
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x0000B668 File Offset: 0x00009868
		public PatchClassProcessor CreateClassProcessor(Type type)
		{
			return new PatchClassProcessor(this, type);
		}

		// Token: 0x060001BA RID: 442 RVA: 0x0000B671 File Offset: 0x00009871
		public ReversePatcher CreateReversePatcher(MethodBase original, HarmonyMethod standin)
		{
			return new ReversePatcher(this, original, standin);
		}

		// Token: 0x060001BB RID: 443 RVA: 0x0000B67B File Offset: 0x0000987B
		public void PatchAll(Assembly assembly)
		{
			AccessTools.GetTypesFromAssembly(assembly).Do(delegate(Type type)
			{
				this.CreateClassProcessor(type).Patch();
			});
		}

		// Token: 0x060001BC RID: 444 RVA: 0x0000B694 File Offset: 0x00009894
		public MethodInfo Patch(MethodBase original, HarmonyMethod prefix = null, HarmonyMethod postfix = null, HarmonyMethod transpiler = null, HarmonyMethod finalizer = null)
		{
			PatchProcessor patchProcessor = this.CreateProcessor(original);
			patchProcessor.AddPrefix(prefix);
			patchProcessor.AddPostfix(postfix);
			patchProcessor.AddTranspiler(transpiler);
			patchProcessor.AddFinalizer(finalizer);
			return patchProcessor.Patch();
		}

		// Token: 0x060001BD RID: 445 RVA: 0x0000B6C4 File Offset: 0x000098C4
		public static MethodInfo ReversePatch(MethodBase original, HarmonyMethod standin, MethodInfo transpiler = null)
		{
			return PatchFunctions.ReversePatch(standin, original, transpiler);
		}

		// Token: 0x060001BE RID: 446 RVA: 0x0000B6D0 File Offset: 0x000098D0
		public void UnpatchAll(string harmonyID = null)
		{
			Harmony.<>c__DisplayClass13_0 CS$<>8__locals1 = new Harmony.<>c__DisplayClass13_0();
			CS$<>8__locals1.harmonyID = harmonyID;
			CS$<>8__locals1.<>4__this = this;
			using (List<MethodBase>.Enumerator enumerator = Harmony.GetAllPatchedMethods().ToList<MethodBase>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MethodBase original = enumerator.Current;
					bool flag = original.HasMethodBody();
					Patches patchInfo2 = Harmony.GetPatchInfo(original);
					if (flag)
					{
						patchInfo2.Postfixes.DoIf(new Func<Patch, bool>(CS$<>8__locals1.<UnpatchAll>g__IDCheck|0), delegate(Patch patchInfo)
						{
							CS$<>8__locals1.<>4__this.Unpatch(original, patchInfo.PatchMethod);
						});
						patchInfo2.Prefixes.DoIf(new Func<Patch, bool>(CS$<>8__locals1.<UnpatchAll>g__IDCheck|0), delegate(Patch patchInfo)
						{
							CS$<>8__locals1.<>4__this.Unpatch(original, patchInfo.PatchMethod);
						});
					}
					patchInfo2.Transpilers.DoIf(new Func<Patch, bool>(CS$<>8__locals1.<UnpatchAll>g__IDCheck|0), delegate(Patch patchInfo)
					{
						CS$<>8__locals1.<>4__this.Unpatch(original, patchInfo.PatchMethod);
					});
					if (flag)
					{
						patchInfo2.Finalizers.DoIf(new Func<Patch, bool>(CS$<>8__locals1.<UnpatchAll>g__IDCheck|0), delegate(Patch patchInfo)
						{
							CS$<>8__locals1.<>4__this.Unpatch(original, patchInfo.PatchMethod);
						});
					}
				}
			}
		}

		// Token: 0x060001BF RID: 447 RVA: 0x0000B808 File Offset: 0x00009A08
		public void Unpatch(MethodBase original, HarmonyPatchType type, string harmonyID = null)
		{
			this.CreateProcessor(original).Unpatch(type, harmonyID);
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x0000B819 File Offset: 0x00009A19
		public void Unpatch(MethodBase original, MethodInfo patch)
		{
			this.CreateProcessor(original).Unpatch(patch);
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x0000B82C File Offset: 0x00009A2C
		public static bool HasAnyPatches(string harmonyID)
		{
			return (from original in Harmony.GetAllPatchedMethods()
				select Harmony.GetPatchInfo(original)).Any((Patches info) => info.Owners.Contains(harmonyID));
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x0000B880 File Offset: 0x00009A80
		public static Patches GetPatchInfo(MethodBase method)
		{
			return PatchProcessor.GetPatchInfo(method);
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x0000B888 File Offset: 0x00009A88
		public IEnumerable<MethodBase> GetPatchedMethods()
		{
			return from original in Harmony.GetAllPatchedMethods()
				where Harmony.GetPatchInfo(original).Owners.Contains(this.Id)
				select original;
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x0000B8A0 File Offset: 0x00009AA0
		public static IEnumerable<MethodBase> GetAllPatchedMethods()
		{
			return PatchProcessor.GetAllPatchedMethods();
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0000B8A7 File Offset: 0x00009AA7
		public static MethodBase GetOriginalMethod(MethodInfo replacement)
		{
			if (replacement == null)
			{
				throw new ArgumentNullException("replacement");
			}
			return HarmonySharedState.GetOriginal(replacement);
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x0000B8C3 File Offset: 0x00009AC3
		public static MethodBase GetMethodFromStackframe(StackFrame frame)
		{
			if (frame == null)
			{
				throw new ArgumentNullException("frame");
			}
			return HarmonySharedState.FindReplacement(frame) ?? frame.GetMethod();
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x0000B8E4 File Offset: 0x00009AE4
		public static MethodBase GetOriginalMethodFromStackframe(StackFrame frame)
		{
			MethodBase methodBase = Harmony.GetMethodFromStackframe(frame);
			MethodInfo methodInfo = methodBase as MethodInfo;
			if (methodInfo != null)
			{
				methodBase = Harmony.GetOriginalMethod(methodInfo) ?? methodBase;
			}
			return methodBase;
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x0000B90F File Offset: 0x00009B0F
		public static Dictionary<string, Version> VersionInfo(out Version currentVersion)
		{
			return PatchProcessor.VersionInfo(out currentVersion);
		}

		// Token: 0x0400011E RID: 286
		public static bool DEBUG;
	}
}
