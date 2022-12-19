using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HarmonyLib
{
	// Token: 0x02000035 RID: 53
	internal class PatchSorter
	{
		// Token: 0x06000116 RID: 278 RVA: 0x00008B4C File Offset: 0x00006D4C
		internal PatchSorter(Patch[] patches, bool debug)
		{
			this.patches = patches.Select((Patch x) => new PatchSorter.PatchSortingWrapper(x)).ToList<PatchSorter.PatchSortingWrapper>();
			this.debug = debug;
			using (List<PatchSorter.PatchSortingWrapper>.Enumerator enumerator = this.patches.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PatchSorter.PatchSortingWrapper node = enumerator.Current;
					node.AddBeforeDependency(this.patches.Where((PatchSorter.PatchSortingWrapper x) => node.innerPatch.before.Contains(x.innerPatch.owner)));
					node.AddAfterDependency(this.patches.Where((PatchSorter.PatchSortingWrapper x) => node.innerPatch.after.Contains(x.innerPatch.owner)));
				}
			}
			this.patches.Sort();
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00008C30 File Offset: 0x00006E30
		internal List<MethodInfo> Sort(MethodBase original)
		{
			if (this.sortedPatchArray != null)
			{
				return this.sortedPatchArray.Select((Patch x) => x.GetMethod(original)).ToList<MethodInfo>();
			}
			this.handledPatches = new HashSet<PatchSorter.PatchSortingWrapper>();
			this.waitingList = new List<PatchSorter.PatchSortingWrapper>();
			this.result = new List<PatchSorter.PatchSortingWrapper>(this.patches.Count);
			Queue<PatchSorter.PatchSortingWrapper> queue = new Queue<PatchSorter.PatchSortingWrapper>(this.patches);
			Func<PatchSorter.PatchSortingWrapper, bool> <>9__3;
			while (queue.Count != 0)
			{
				foreach (PatchSorter.PatchSortingWrapper patchSortingWrapper in queue)
				{
					IEnumerable<PatchSorter.PatchSortingWrapper> after = patchSortingWrapper.after;
					Func<PatchSorter.PatchSortingWrapper, bool> func;
					if ((func = <>9__3) == null)
					{
						func = (<>9__3 = (PatchSorter.PatchSortingWrapper x) => this.handledPatches.Contains(x));
					}
					if (after.All(func))
					{
						this.AddNodeToResult(patchSortingWrapper);
						if (patchSortingWrapper.before.Count != 0)
						{
							this.ProcessWaitingList();
						}
					}
					else
					{
						this.waitingList.Add(patchSortingWrapper);
					}
				}
				this.CullDependency();
				queue = new Queue<PatchSorter.PatchSortingWrapper>(this.waitingList);
				this.waitingList.Clear();
			}
			this.sortedPatchArray = this.result.Select((PatchSorter.PatchSortingWrapper x) => x.innerPatch).ToArray<Patch>();
			this.handledPatches = null;
			this.waitingList = null;
			this.patches = null;
			return this.sortedPatchArray.Select((Patch x) => x.GetMethod(original)).ToList<MethodInfo>();
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00008DD4 File Offset: 0x00006FD4
		internal bool ComparePatchLists(Patch[] patches)
		{
			if (this.sortedPatchArray == null)
			{
				this.Sort(null);
			}
			return patches != null && this.sortedPatchArray.Length == patches.Length && this.sortedPatchArray.All((Patch x) => patches.Contains(x, new PatchSorter.PatchDetailedComparer()));
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00008E34 File Offset: 0x00007034
		private void CullDependency()
		{
			for (int i = this.waitingList.Count - 1; i >= 0; i--)
			{
				foreach (PatchSorter.PatchSortingWrapper patchSortingWrapper in this.waitingList[i].after)
				{
					if (!this.handledPatches.Contains(patchSortingWrapper))
					{
						this.waitingList[i].RemoveAfterDependency(patchSortingWrapper);
						if (this.debug)
						{
							string text = patchSortingWrapper.innerPatch.PatchMethod.FullDescription();
							string text2 = this.waitingList[i].innerPatch.PatchMethod.FullDescription();
							FileLog.LogBuffered("Breaking dependance between " + text + " and " + text2);
						}
						return;
					}
				}
			}
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00008F18 File Offset: 0x00007118
		private void ProcessWaitingList()
		{
			int num = this.waitingList.Count;
			int i = 0;
			while (i < num)
			{
				PatchSorter.PatchSortingWrapper patchSortingWrapper = this.waitingList[i];
				if (patchSortingWrapper.after.All(new Func<PatchSorter.PatchSortingWrapper, bool>(this.handledPatches.Contains)))
				{
					this.waitingList.Remove(patchSortingWrapper);
					this.AddNodeToResult(patchSortingWrapper);
					num--;
					i = 0;
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00008F85 File Offset: 0x00007185
		private void AddNodeToResult(PatchSorter.PatchSortingWrapper node)
		{
			this.result.Add(node);
			this.handledPatches.Add(node);
		}

		// Token: 0x040000BB RID: 187
		private List<PatchSorter.PatchSortingWrapper> patches;

		// Token: 0x040000BC RID: 188
		private HashSet<PatchSorter.PatchSortingWrapper> handledPatches;

		// Token: 0x040000BD RID: 189
		private List<PatchSorter.PatchSortingWrapper> result;

		// Token: 0x040000BE RID: 190
		private List<PatchSorter.PatchSortingWrapper> waitingList;

		// Token: 0x040000BF RID: 191
		internal Patch[] sortedPatchArray;

		// Token: 0x040000C0 RID: 192
		private readonly bool debug;

		// Token: 0x02000036 RID: 54
		private class PatchSortingWrapper : IComparable
		{
			// Token: 0x0600011C RID: 284 RVA: 0x00008FA0 File Offset: 0x000071A0
			internal PatchSortingWrapper(Patch patch)
			{
				this.innerPatch = patch;
				this.before = new HashSet<PatchSorter.PatchSortingWrapper>();
				this.after = new HashSet<PatchSorter.PatchSortingWrapper>();
			}

			// Token: 0x0600011D RID: 285 RVA: 0x00008FC5 File Offset: 0x000071C5
			public int CompareTo(object obj)
			{
				PatchSorter.PatchSortingWrapper patchSortingWrapper = obj as PatchSorter.PatchSortingWrapper;
				return PatchInfoSerialization.PriorityComparer((patchSortingWrapper != null) ? patchSortingWrapper.innerPatch : null, this.innerPatch.index, this.innerPatch.priority);
			}

			// Token: 0x0600011E RID: 286 RVA: 0x00008FF4 File Offset: 0x000071F4
			public override bool Equals(object obj)
			{
				PatchSorter.PatchSortingWrapper patchSortingWrapper = obj as PatchSorter.PatchSortingWrapper;
				return patchSortingWrapper != null && this.innerPatch.PatchMethod == patchSortingWrapper.innerPatch.PatchMethod;
			}

			// Token: 0x0600011F RID: 287 RVA: 0x00009028 File Offset: 0x00007228
			public override int GetHashCode()
			{
				return this.innerPatch.PatchMethod.GetHashCode();
			}

			// Token: 0x06000120 RID: 288 RVA: 0x0000903C File Offset: 0x0000723C
			internal void AddBeforeDependency(IEnumerable<PatchSorter.PatchSortingWrapper> dependencies)
			{
				foreach (PatchSorter.PatchSortingWrapper patchSortingWrapper in dependencies)
				{
					this.before.Add(patchSortingWrapper);
					patchSortingWrapper.after.Add(this);
				}
			}

			// Token: 0x06000121 RID: 289 RVA: 0x00009098 File Offset: 0x00007298
			internal void AddAfterDependency(IEnumerable<PatchSorter.PatchSortingWrapper> dependencies)
			{
				foreach (PatchSorter.PatchSortingWrapper patchSortingWrapper in dependencies)
				{
					this.after.Add(patchSortingWrapper);
					patchSortingWrapper.before.Add(this);
				}
			}

			// Token: 0x06000122 RID: 290 RVA: 0x000090F4 File Offset: 0x000072F4
			internal void RemoveAfterDependency(PatchSorter.PatchSortingWrapper afterNode)
			{
				this.after.Remove(afterNode);
				afterNode.before.Remove(this);
			}

			// Token: 0x06000123 RID: 291 RVA: 0x00009110 File Offset: 0x00007310
			internal void RemoveBeforeDependency(PatchSorter.PatchSortingWrapper beforeNode)
			{
				this.before.Remove(beforeNode);
				beforeNode.after.Remove(this);
			}

			// Token: 0x040000C1 RID: 193
			internal readonly HashSet<PatchSorter.PatchSortingWrapper> after;

			// Token: 0x040000C2 RID: 194
			internal readonly HashSet<PatchSorter.PatchSortingWrapper> before;

			// Token: 0x040000C3 RID: 195
			internal readonly Patch innerPatch;
		}

		// Token: 0x02000037 RID: 55
		internal class PatchDetailedComparer : IEqualityComparer<Patch>
		{
			// Token: 0x06000124 RID: 292 RVA: 0x0000912C File Offset: 0x0000732C
			public bool Equals(Patch x, Patch y)
			{
				return y != null && x != null && x.owner == y.owner && x.PatchMethod == y.PatchMethod && x.index == y.index && x.priority == y.priority && x.before.Length == y.before.Length && x.after.Length == y.after.Length && x.before.All(new Func<string, bool>(y.before.Contains<string>)) && x.after.All(new Func<string, bool>(y.after.Contains<string>));
			}

			// Token: 0x06000125 RID: 293 RVA: 0x000091EA File Offset: 0x000073EA
			public int GetHashCode(Patch obj)
			{
				return obj.GetHashCode();
			}
		}
	}
}
