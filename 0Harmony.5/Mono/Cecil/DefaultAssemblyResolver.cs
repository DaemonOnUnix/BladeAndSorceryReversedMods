using System;
using System.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x020001F9 RID: 505
	internal class DefaultAssemblyResolver : BaseAssemblyResolver
	{
		// Token: 0x06000A4E RID: 2638 RVA: 0x0002627E File Offset: 0x0002447E
		public DefaultAssemblyResolver()
		{
			this.cache = new Dictionary<string, AssemblyDefinition>(StringComparer.Ordinal);
		}

		// Token: 0x06000A4F RID: 2639 RVA: 0x00026298 File Offset: 0x00024498
		public override AssemblyDefinition Resolve(AssemblyNameReference name)
		{
			Mixin.CheckName(name);
			AssemblyDefinition assemblyDefinition;
			if (this.cache.TryGetValue(name.FullName, out assemblyDefinition))
			{
				return assemblyDefinition;
			}
			assemblyDefinition = base.Resolve(name);
			this.cache[name.FullName] = assemblyDefinition;
			return assemblyDefinition;
		}

		// Token: 0x06000A50 RID: 2640 RVA: 0x000262E0 File Offset: 0x000244E0
		protected void RegisterAssembly(AssemblyDefinition assembly)
		{
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}
			string fullName = assembly.Name.FullName;
			if (this.cache.ContainsKey(fullName))
			{
				return;
			}
			this.cache[fullName] = assembly;
		}

		// Token: 0x06000A51 RID: 2641 RVA: 0x00026324 File Offset: 0x00024524
		protected override void Dispose(bool disposing)
		{
			foreach (AssemblyDefinition assemblyDefinition in this.cache.Values)
			{
				assemblyDefinition.Dispose();
			}
			this.cache.Clear();
			base.Dispose(disposing);
		}

		// Token: 0x040002E5 RID: 741
		private readonly IDictionary<string, AssemblyDefinition> cache;
	}
}
