using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HarmonyLib
{
	// Token: 0x02000031 RID: 49
	internal class PatchJobs<T>
	{
		// Token: 0x06000110 RID: 272 RVA: 0x00009550 File Offset: 0x00007750
		internal PatchJobs<T>.Job GetJob(MethodBase method)
		{
			if (method == null)
			{
				return null;
			}
			PatchJobs<T>.Job job;
			if (!this.state.TryGetValue(method, out job))
			{
				job = new PatchJobs<T>.Job
				{
					original = method
				};
				this.state[method] = job;
			}
			return job;
		}

		// Token: 0x06000111 RID: 273 RVA: 0x0000958D File Offset: 0x0000778D
		internal List<PatchJobs<T>.Job> GetJobs()
		{
			return this.state.Values.Where((PatchJobs<T>.Job job) => job.prefixes.Count + job.postfixes.Count + job.transpilers.Count + job.finalizers.Count > 0).ToList<PatchJobs<T>.Job>();
		}

		// Token: 0x06000112 RID: 274 RVA: 0x000095C3 File Offset: 0x000077C3
		internal List<T> GetReplacements()
		{
			return this.state.Values.Select((PatchJobs<T>.Job job) => job.replacement).ToList<T>();
		}

		// Token: 0x040000B2 RID: 178
		internal Dictionary<MethodBase, PatchJobs<T>.Job> state = new Dictionary<MethodBase, PatchJobs<T>.Job>();

		// Token: 0x02000032 RID: 50
		internal class Job
		{
			// Token: 0x06000114 RID: 276 RVA: 0x0000960C File Offset: 0x0000780C
			internal void AddPatch(AttributePatch patch)
			{
				HarmonyPatchType? type = patch.type;
				if (type != null)
				{
					switch (type.GetValueOrDefault())
					{
					case HarmonyPatchType.Prefix:
						this.prefixes.Add(patch.info);
						return;
					case HarmonyPatchType.Postfix:
						this.postfixes.Add(patch.info);
						return;
					case HarmonyPatchType.Transpiler:
						this.transpilers.Add(patch.info);
						return;
					case HarmonyPatchType.Finalizer:
						this.finalizers.Add(patch.info);
						break;
					default:
						return;
					}
				}
			}

			// Token: 0x040000B3 RID: 179
			internal MethodBase original;

			// Token: 0x040000B4 RID: 180
			internal T replacement;

			// Token: 0x040000B5 RID: 181
			internal List<HarmonyMethod> prefixes = new List<HarmonyMethod>();

			// Token: 0x040000B6 RID: 182
			internal List<HarmonyMethod> postfixes = new List<HarmonyMethod>();

			// Token: 0x040000B7 RID: 183
			internal List<HarmonyMethod> transpilers = new List<HarmonyMethod>();

			// Token: 0x040000B8 RID: 184
			internal List<HarmonyMethod> finalizers = new List<HarmonyMethod>();
		}
	}
}
