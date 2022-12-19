using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace HarmonyLib
{
	// Token: 0x0200007E RID: 126
	public class Patches
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600026B RID: 619 RVA: 0x0000DA9C File Offset: 0x0000BC9C
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

		// Token: 0x0600026C RID: 620 RVA: 0x0000DB78 File Offset: 0x0000BD78
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

		// Token: 0x0400017C RID: 380
		public readonly ReadOnlyCollection<Patch> Prefixes;

		// Token: 0x0400017D RID: 381
		public readonly ReadOnlyCollection<Patch> Postfixes;

		// Token: 0x0400017E RID: 382
		public readonly ReadOnlyCollection<Patch> Transpilers;

		// Token: 0x0400017F RID: 383
		public readonly ReadOnlyCollection<Patch> Finalizers;
	}
}
