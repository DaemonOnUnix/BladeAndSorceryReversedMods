using System;
using System.Linq;
using System.Reflection;

namespace HarmonyLib
{
	// Token: 0x02000072 RID: 114
	[Serializable]
	public class PatchInfo
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600020A RID: 522 RVA: 0x0000B4FC File Offset: 0x000096FC
		public bool Debugging
		{
			get
			{
				if (!this.prefixes.Any((Patch p) => p.debug))
				{
					if (!this.postfixes.Any((Patch p) => p.debug))
					{
						if (!this.transpilers.Any((Patch p) => p.debug))
						{
							return this.finalizers.Any((Patch p) => p.debug);
						}
					}
				}
				return true;
			}
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0000B5BC File Offset: 0x000097BC
		internal void AddPrefixes(string owner, params HarmonyMethod[] methods)
		{
			this.prefixes = PatchInfo.Add(owner, methods, this.prefixes);
		}

		// Token: 0x0600020C RID: 524 RVA: 0x0000B5D4 File Offset: 0x000097D4
		[Obsolete("This method only exists for backwards compatibility since the class is public.")]
		public void AddPrefix(MethodInfo patch, string owner, int priority, string[] before, string[] after, bool debug)
		{
			this.AddPrefixes(owner, new HarmonyMethod[]
			{
				new HarmonyMethod(patch, priority, before, after, new bool?(debug))
			});
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0000B603 File Offset: 0x00009803
		public void RemovePrefix(string owner)
		{
			this.prefixes = PatchInfo.Remove(owner, this.prefixes);
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000B617 File Offset: 0x00009817
		internal void AddPostfixes(string owner, params HarmonyMethod[] methods)
		{
			this.postfixes = PatchInfo.Add(owner, methods, this.postfixes);
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000B62C File Offset: 0x0000982C
		[Obsolete("This method only exists for backwards compatibility since the class is public.")]
		public void AddPostfix(MethodInfo patch, string owner, int priority, string[] before, string[] after, bool debug)
		{
			this.AddPostfixes(owner, new HarmonyMethod[]
			{
				new HarmonyMethod(patch, priority, before, after, new bool?(debug))
			});
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0000B65B File Offset: 0x0000985B
		public void RemovePostfix(string owner)
		{
			this.postfixes = PatchInfo.Remove(owner, this.postfixes);
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0000B66F File Offset: 0x0000986F
		internal void AddTranspilers(string owner, params HarmonyMethod[] methods)
		{
			this.transpilers = PatchInfo.Add(owner, methods, this.transpilers);
		}

		// Token: 0x06000212 RID: 530 RVA: 0x0000B684 File Offset: 0x00009884
		[Obsolete("This method only exists for backwards compatibility since the class is public.")]
		public void AddTranspiler(MethodInfo patch, string owner, int priority, string[] before, string[] after, bool debug)
		{
			this.AddTranspilers(owner, new HarmonyMethod[]
			{
				new HarmonyMethod(patch, priority, before, after, new bool?(debug))
			});
		}

		// Token: 0x06000213 RID: 531 RVA: 0x0000B6B3 File Offset: 0x000098B3
		public void RemoveTranspiler(string owner)
		{
			this.transpilers = PatchInfo.Remove(owner, this.transpilers);
		}

		// Token: 0x06000214 RID: 532 RVA: 0x0000B6C7 File Offset: 0x000098C7
		internal void AddFinalizers(string owner, params HarmonyMethod[] methods)
		{
			this.finalizers = PatchInfo.Add(owner, methods, this.finalizers);
		}

		// Token: 0x06000215 RID: 533 RVA: 0x0000B6DC File Offset: 0x000098DC
		[Obsolete("This method only exists for backwards compatibility since the class is public.")]
		public void AddFinalizer(MethodInfo patch, string owner, int priority, string[] before, string[] after, bool debug)
		{
			this.AddFinalizers(owner, new HarmonyMethod[]
			{
				new HarmonyMethod(patch, priority, before, after, new bool?(debug))
			});
		}

		// Token: 0x06000216 RID: 534 RVA: 0x0000B70B File Offset: 0x0000990B
		public void RemoveFinalizer(string owner)
		{
			this.finalizers = PatchInfo.Remove(owner, this.finalizers);
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000B720 File Offset: 0x00009920
		public void RemovePatch(MethodInfo patch)
		{
			this.prefixes = this.prefixes.Where((Patch p) => p.PatchMethod != patch).ToArray<Patch>();
			this.postfixes = this.postfixes.Where((Patch p) => p.PatchMethod != patch).ToArray<Patch>();
			this.transpilers = this.transpilers.Where((Patch p) => p.PatchMethod != patch).ToArray<Patch>();
			this.finalizers = this.finalizers.Where((Patch p) => p.PatchMethod != patch).ToArray<Patch>();
		}

		// Token: 0x06000218 RID: 536 RVA: 0x0000B7C4 File Offset: 0x000099C4
		private static Patch[] Add(string owner, HarmonyMethod[] add, Patch[] current)
		{
			if (add.Length == 0)
			{
				return current;
			}
			int initialIndex = current.Length;
			return current.Concat(add.Where((HarmonyMethod method) => method != null).Select((HarmonyMethod method, int i) => new Patch(method, i + initialIndex, owner))).ToArray<Patch>();
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000B830 File Offset: 0x00009A30
		private static Patch[] Remove(string owner, Patch[] current)
		{
			if (!(owner == "*"))
			{
				return current.Where((Patch patch) => patch.owner != owner).ToArray<Patch>();
			}
			return new Patch[0];
		}

		// Token: 0x04000141 RID: 321
		public Patch[] prefixes = new Patch[0];

		// Token: 0x04000142 RID: 322
		public Patch[] postfixes = new Patch[0];

		// Token: 0x04000143 RID: 323
		public Patch[] transpilers = new Patch[0];

		// Token: 0x04000144 RID: 324
		public Patch[] finalizers = new Patch[0];
	}
}
