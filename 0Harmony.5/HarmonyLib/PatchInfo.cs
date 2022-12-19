using System;
using System.Linq;
using System.Reflection;

namespace HarmonyLib
{
	// Token: 0x02000075 RID: 117
	[Serializable]
	public class PatchInfo
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000226 RID: 550 RVA: 0x0000C70C File Offset: 0x0000A90C
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

		// Token: 0x06000227 RID: 551 RVA: 0x0000C7CC File Offset: 0x0000A9CC
		internal void AddPrefixes(string owner, params HarmonyMethod[] methods)
		{
			this.prefixes = PatchInfo.Add(owner, methods, this.prefixes);
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000C7E4 File Offset: 0x0000A9E4
		[Obsolete("This method only exists for backwards compatibility since the class is public.")]
		public void AddPrefix(MethodInfo patch, string owner, int priority, string[] before, string[] after, bool debug)
		{
			this.AddPrefixes(owner, new HarmonyMethod[]
			{
				new HarmonyMethod(patch, priority, before, after, new bool?(debug))
			});
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000C813 File Offset: 0x0000AA13
		public void RemovePrefix(string owner)
		{
			this.prefixes = PatchInfo.Remove(owner, this.prefixes);
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0000C827 File Offset: 0x0000AA27
		internal void AddPostfixes(string owner, params HarmonyMethod[] methods)
		{
			this.postfixes = PatchInfo.Add(owner, methods, this.postfixes);
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000C83C File Offset: 0x0000AA3C
		[Obsolete("This method only exists for backwards compatibility since the class is public.")]
		public void AddPostfix(MethodInfo patch, string owner, int priority, string[] before, string[] after, bool debug)
		{
			this.AddPostfixes(owner, new HarmonyMethod[]
			{
				new HarmonyMethod(patch, priority, before, after, new bool?(debug))
			});
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000C86B File Offset: 0x0000AA6B
		public void RemovePostfix(string owner)
		{
			this.postfixes = PatchInfo.Remove(owner, this.postfixes);
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0000C87F File Offset: 0x0000AA7F
		internal void AddTranspilers(string owner, params HarmonyMethod[] methods)
		{
			this.transpilers = PatchInfo.Add(owner, methods, this.transpilers);
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0000C894 File Offset: 0x0000AA94
		[Obsolete("This method only exists for backwards compatibility since the class is public.")]
		public void AddTranspiler(MethodInfo patch, string owner, int priority, string[] before, string[] after, bool debug)
		{
			this.AddTranspilers(owner, new HarmonyMethod[]
			{
				new HarmonyMethod(patch, priority, before, after, new bool?(debug))
			});
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0000C8C3 File Offset: 0x0000AAC3
		public void RemoveTranspiler(string owner)
		{
			this.transpilers = PatchInfo.Remove(owner, this.transpilers);
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0000C8D7 File Offset: 0x0000AAD7
		internal void AddFinalizers(string owner, params HarmonyMethod[] methods)
		{
			this.finalizers = PatchInfo.Add(owner, methods, this.finalizers);
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0000C8EC File Offset: 0x0000AAEC
		[Obsolete("This method only exists for backwards compatibility since the class is public.")]
		public void AddFinalizer(MethodInfo patch, string owner, int priority, string[] before, string[] after, bool debug)
		{
			this.AddFinalizers(owner, new HarmonyMethod[]
			{
				new HarmonyMethod(patch, priority, before, after, new bool?(debug))
			});
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0000C91B File Offset: 0x0000AB1B
		public void RemoveFinalizer(string owner)
		{
			this.finalizers = PatchInfo.Remove(owner, this.finalizers);
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0000C930 File Offset: 0x0000AB30
		public void RemovePatch(MethodInfo patch)
		{
			this.prefixes = this.prefixes.Where((Patch p) => p.PatchMethod != patch).ToArray<Patch>();
			this.postfixes = this.postfixes.Where((Patch p) => p.PatchMethod != patch).ToArray<Patch>();
			this.transpilers = this.transpilers.Where((Patch p) => p.PatchMethod != patch).ToArray<Patch>();
			this.finalizers = this.finalizers.Where((Patch p) => p.PatchMethod != patch).ToArray<Patch>();
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000C9D4 File Offset: 0x0000ABD4
		private static Patch[] Add(string owner, HarmonyMethod[] add, Patch[] current)
		{
			if (add.Length == 0)
			{
				return current;
			}
			int initialIndex = current.Length;
			return current.Concat(add.Where((HarmonyMethod method) => method != null).Select((HarmonyMethod method, int i) => new Patch(method, i + initialIndex, owner))).ToArray<Patch>();
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000CA40 File Offset: 0x0000AC40
		private static Patch[] Remove(string owner, Patch[] current)
		{
			if (!(owner == "*"))
			{
				return current.Where((Patch patch) => patch.owner != owner).ToArray<Patch>();
			}
			return new Patch[0];
		}

		// Token: 0x04000153 RID: 339
		public Patch[] prefixes = new Patch[0];

		// Token: 0x04000154 RID: 340
		public Patch[] postfixes = new Patch[0];

		// Token: 0x04000155 RID: 341
		public Patch[] transpilers = new Patch[0];

		// Token: 0x04000156 RID: 342
		public Patch[] finalizers = new Patch[0];
	}
}
