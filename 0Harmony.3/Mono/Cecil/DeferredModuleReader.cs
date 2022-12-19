using System;
using Mono.Cecil.PE;

namespace Mono.Cecil
{
	// Token: 0x020000C6 RID: 198
	internal sealed class DeferredModuleReader : ModuleReader
	{
		// Token: 0x060004D3 RID: 1235 RVA: 0x0001583C File Offset: 0x00013A3C
		public DeferredModuleReader(Image image)
			: base(image, ReadingMode.Deferred)
		{
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x00015846 File Offset: 0x00013A46
		protected override void ReadModule()
		{
			this.module.Read<ModuleDefinition>(this.module, delegate(ModuleDefinition _, MetadataReader reader)
			{
				base.ReadModuleManifest(reader);
			});
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x00012279 File Offset: 0x00010479
		public override void ReadSymbols(ModuleDefinition module)
		{
		}
	}
}
