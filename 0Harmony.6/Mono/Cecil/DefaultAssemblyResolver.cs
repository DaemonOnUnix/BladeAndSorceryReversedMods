using System;
using System.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000107 RID: 263
	internal class DefaultAssemblyResolver : BaseAssemblyResolver
	{
		// Token: 0x06000716 RID: 1814 RVA: 0x000203D6 File Offset: 0x0001E5D6
		public DefaultAssemblyResolver()
		{
			this.cache = new Dictionary<string, AssemblyDefinition>(StringComparer.Ordinal);
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x000203F0 File Offset: 0x0001E5F0
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

		// Token: 0x06000718 RID: 1816 RVA: 0x00020438 File Offset: 0x0001E638
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

		// Token: 0x06000719 RID: 1817 RVA: 0x0002047C File Offset: 0x0001E67C
		protected override void Dispose(bool disposing)
		{
			foreach (AssemblyDefinition assemblyDefinition in this.cache.Values)
			{
				assemblyDefinition.Dispose();
			}
			this.cache.Clear();
			base.Dispose(disposing);
		}

		// Token: 0x040002B3 RID: 691
		private readonly IDictionary<string, AssemblyDefinition> cache;
	}
}
