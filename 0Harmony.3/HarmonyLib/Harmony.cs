using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace HarmonyLib
{
	// Token: 0x0200005D RID: 93
	public class Harmony
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600019C RID: 412 RVA: 0x0000A344 File Offset: 0x00008544
		// (set) Token: 0x0600019D RID: 413 RVA: 0x0000A34C File Offset: 0x0000854C
		public string Id { get; private set; }

		// Token: 0x0600019E RID: 414 RVA: 0x0000A358 File Offset: 0x00008558
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
				FileLog.Log(string.Format("### Harmony id={0}, version={1}, location={2}, env/clr={3}, platform={4}", new object[] { id, version, text2, text3, text4 }));
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

		// Token: 0x0600019F RID: 415 RVA: 0x0000A4E4 File Offset: 0x000086E4
		public void PatchAll()
		{
			Assembly assembly = new StackTrace().GetFrame(1).GetMethod().ReflectedType.Assembly;
			this.PatchAll(assembly);
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x0000A513 File Offset: 0x00008713
		public PatchProcessor CreateProcessor(MethodBase original)
		{
			return new PatchProcessor(this, original);
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x0000A51C File Offset: 0x0000871C
		public PatchClassProcessor CreateClassProcessor(Type type)
		{
			return new PatchClassProcessor(this, type);
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x0000A525 File Offset: 0x00008725
		public ReversePatcher CreateReversePatcher(MethodBase original, HarmonyMethod standin)
		{
			return new ReversePatcher(this, original, standin);
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x0000A52F File Offset: 0x0000872F
		public void PatchAll(Assembly assembly)
		{
			AccessTools.GetTypesFromAssembly(assembly).Do(delegate(Type type)
			{
				this.CreateClassProcessor(type).Patch();
			});
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x0000A548 File Offset: 0x00008748
		public MethodInfo Patch(MethodBase original, HarmonyMethod prefix = null, HarmonyMethod postfix = null, HarmonyMethod transpiler = null, HarmonyMethod finalizer = null)
		{
			PatchProcessor patchProcessor = this.CreateProcessor(original);
			patchProcessor.AddPrefix(prefix);
			patchProcessor.AddPostfix(postfix);
			patchProcessor.AddTranspiler(transpiler);
			patchProcessor.AddFinalizer(finalizer);
			return patchProcessor.Patch();
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x0000A578 File Offset: 0x00008778
		public static MethodInfo ReversePatch(MethodBase original, HarmonyMethod standin, MethodInfo transpiler = null)
		{
			return PatchFunctions.ReversePatch(standin, original, transpiler);
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x0000A584 File Offset: 0x00008784
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

		// Token: 0x060001A7 RID: 423 RVA: 0x0000A6BC File Offset: 0x000088BC
		public void Unpatch(MethodBase original, HarmonyPatchType type, string harmonyID = null)
		{
			this.CreateProcessor(original).Unpatch(type, harmonyID);
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x0000A6CD File Offset: 0x000088CD
		public void Unpatch(MethodBase original, MethodInfo patch)
		{
			this.CreateProcessor(original).Unpatch(patch);
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x0000A6E0 File Offset: 0x000088E0
		public static bool HasAnyPatches(string harmonyID)
		{
			return (from original in Harmony.GetAllPatchedMethods()
				select Harmony.GetPatchInfo(original)).Any((Patches info) => info.Owners.Contains(harmonyID));
		}

		// Token: 0x060001AA RID: 426 RVA: 0x0000A734 File Offset: 0x00008934
		public static Patches GetPatchInfo(MethodBase method)
		{
			return PatchProcessor.GetPatchInfo(method);
		}

		// Token: 0x060001AB RID: 427 RVA: 0x0000A73C File Offset: 0x0000893C
		public IEnumerable<MethodBase> GetPatchedMethods()
		{
			return from original in Harmony.GetAllPatchedMethods()
				where Harmony.GetPatchInfo(original).Owners.Contains(this.Id)
				select original;
		}

		// Token: 0x060001AC RID: 428 RVA: 0x0000A754 File Offset: 0x00008954
		public static IEnumerable<MethodBase> GetAllPatchedMethods()
		{
			return PatchProcessor.GetAllPatchedMethods();
		}

		// Token: 0x060001AD RID: 429 RVA: 0x0000A75B File Offset: 0x0000895B
		public static Dictionary<string, Version> VersionInfo(out Version currentVersion)
		{
			return PatchProcessor.VersionInfo(out currentVersion);
		}

		// Token: 0x0400010D RID: 269
		public static bool DEBUG;
	}
}
