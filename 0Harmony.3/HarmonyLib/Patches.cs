using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace HarmonyLib
{
	// Token: 0x0200007B RID: 123
	public class Patches
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600024E RID: 590 RVA: 0x0000C814 File Offset: 0x0000AA14
		public ReadOnlyCollection<string> Owners
		{
			get
			{
				HashSet<string> hashSet = new HashSet<string>();
				hashSet.UnionWith(this.Prefixes.Select((Patch p) => p.owner));
				hashSet.UnionWith(this.Postfixes.Select((Patch p) => p.owner));
				hashSet.UnionWith(this.Transpilers.Select((Patch p) => p.owner));
				hashSet.UnionWith(this.Finalizers.Select((Patch p) => p.owner));
				return hashSet.ToList<string>().AsReadOnly();
			}
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000C8F0 File Offset: 0x0000AAF0
		public Patches(Patch[] prefixes, Patch[] postfixes, Patch[] transpilers, Patch[] finalizers)
		{
			if (prefixes == null)
			{
				prefixes = new Patch[0];
			}
			if (postfixes == null)
			{
				postfixes = new Patch[0];
			}
			if (transpilers == null)
			{
				transpilers = new Patch[0];
			}
			if (finalizers == null)
			{
				finalizers = new Patch[0];
			}
			this.Prefixes = prefixes.ToList<Patch>().AsReadOnly();
			this.Postfixes = postfixes.ToList<Patch>().AsReadOnly();
			this.Transpilers = transpilers.ToList<Patch>().AsReadOnly();
			this.Finalizers = finalizers.ToList<Patch>().AsReadOnly();
		}

		// Token: 0x0400016A RID: 362
		public readonly ReadOnlyCollection<Patch> Prefixes;

		// Token: 0x0400016B RID: 363
		public readonly ReadOnlyCollection<Patch> Postfixes;

		// Token: 0x0400016C RID: 364
		public readonly ReadOnlyCollection<Patch> Transpilers;

		// Token: 0x0400016D RID: 365
		public readonly ReadOnlyCollection<Patch> Finalizers;
	}
}
