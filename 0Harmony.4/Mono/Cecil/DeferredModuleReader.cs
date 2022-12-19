using System;
using Mono.Cecil.PE;

namespace Mono.Cecil
{
	// Token: 0x020001B8 RID: 440
	internal sealed class DeferredModuleReader : ModuleReader
	{
		// Token: 0x06000809 RID: 2057 RVA: 0x0001B6CC File Offset: 0x000198CC
		public DeferredModuleReader(Image image)
			: base(image, ReadingMode.Deferred)
		{
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x0001B6D6 File Offset: 0x000198D6
		protected override void ReadModule()
		{
			this.module.Read<ModuleDefinition>(this.module, delegate(ModuleDefinition _, MetadataReader reader)
			{
				base.ReadModuleManifest(reader);
			});
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x00018105 File Offset: 0x00016305
		public override void ReadSymbols(ModuleDefinition module)
		{
		}
	}
}
