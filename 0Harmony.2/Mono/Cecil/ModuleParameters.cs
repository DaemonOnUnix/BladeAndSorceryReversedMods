using System;

namespace Mono.Cecil
{
	// Token: 0x02000242 RID: 578
	public sealed class ModuleParameters
	{
		// Token: 0x1700038F RID: 911
		// (get) Token: 0x06000D6A RID: 3434 RVA: 0x0002BA79 File Offset: 0x00029C79
		// (set) Token: 0x06000D6B RID: 3435 RVA: 0x0002BA81 File Offset: 0x00029C81
		public ModuleKind Kind
		{
			get
			{
				return this.kind;
			}
			set
			{
				this.kind = value;
			}
		}

		// Token: 0x17000390 RID: 912
		// (get) Token: 0x06000D6C RID: 3436 RVA: 0x0002BA8A File Offset: 0x00029C8A
		// (set) Token: 0x06000D6D RID: 3437 RVA: 0x0002BA92 File Offset: 0x00029C92
		public TargetRuntime Runtime
		{
			get
			{
				return this.runtime;
			}
			set
			{
				this.runtime = value;
			}
		}

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x06000D6E RID: 3438 RVA: 0x0002BA9B File Offset: 0x00029C9B
		// (set) Token: 0x06000D6F RID: 3439 RVA: 0x0002BAA3 File Offset: 0x00029CA3
		public uint? Timestamp
		{
			get
			{
				return this.timestamp;
			}
			set
			{
				this.timestamp = value;
			}
		}

		// Token: 0x17000392 RID: 914
		// (get) Token: 0x06000D70 RID: 3440 RVA: 0x0002BAAC File Offset: 0x00029CAC
		// (set) Token: 0x06000D71 RID: 3441 RVA: 0x0002BAB4 File Offset: 0x00029CB4
		public TargetArchitecture Architecture
		{
			get
			{
				return this.architecture;
			}
			set
			{
				this.architecture = value;
			}
		}

		// Token: 0x17000393 RID: 915
		// (get) Token: 0x06000D72 RID: 3442 RVA: 0x0002BABD File Offset: 0x00029CBD
		// (set) Token: 0x06000D73 RID: 3443 RVA: 0x0002BAC5 File Offset: 0x00029CC5
		public IAssemblyResolver AssemblyResolver
		{
			get
			{
				return this.assembly_resolver;
			}
			set
			{
				this.assembly_resolver = value;
			}
		}

		// Token: 0x17000394 RID: 916
		// (get) Token: 0x06000D74 RID: 3444 RVA: 0x0002BACE File Offset: 0x00029CCE
		// (set) Token: 0x06000D75 RID: 3445 RVA: 0x0002BAD6 File Offset: 0x00029CD6
		public IMetadataResolver MetadataResolver
		{
			get
			{
				return this.metadata_resolver;
			}
			set
			{
				this.metadata_resolver = value;
			}
		}

		// Token: 0x17000395 RID: 917
		// (get) Token: 0x06000D76 RID: 3446 RVA: 0x0002BADF File Offset: 0x00029CDF
		// (set) Token: 0x06000D77 RID: 3447 RVA: 0x0002BAE7 File Offset: 0x00029CE7
		public IMetadataImporterProvider MetadataImporterProvider
		{
			get
			{
				return this.metadata_importer_provider;
			}
			set
			{
				this.metadata_importer_provider = value;
			}
		}

		// Token: 0x17000396 RID: 918
		// (get) Token: 0x06000D78 RID: 3448 RVA: 0x0002BAF0 File Offset: 0x00029CF0
		// (set) Token: 0x06000D79 RID: 3449 RVA: 0x0002BAF8 File Offset: 0x00029CF8
		public IReflectionImporterProvider ReflectionImporterProvider
		{
			get
			{
				return this.reflection_importer_provider;
			}
			set
			{
				this.reflection_importer_provider = value;
			}
		}

		// Token: 0x06000D7A RID: 3450 RVA: 0x0002BB01 File Offset: 0x00029D01
		public ModuleParameters()
		{
			this.kind = ModuleKind.Dll;
			this.Runtime = ModuleParameters.GetCurrentRuntime();
			this.architecture = TargetArchitecture.I386;
		}

		// Token: 0x06000D7B RID: 3451 RVA: 0x0002BB26 File Offset: 0x00029D26
		private static TargetRuntime GetCurrentRuntime()
		{
			return typeof(object).Assembly.ImageRuntimeVersion.ParseRuntime();
		}

		// Token: 0x040003EA RID: 1002
		private ModuleKind kind;

		// Token: 0x040003EB RID: 1003
		private TargetRuntime runtime;

		// Token: 0x040003EC RID: 1004
		private uint? timestamp;

		// Token: 0x040003ED RID: 1005
		private TargetArchitecture architecture;

		// Token: 0x040003EE RID: 1006
		private IAssemblyResolver assembly_resolver;

		// Token: 0x040003EF RID: 1007
		private IMetadataResolver metadata_resolver;

		// Token: 0x040003F0 RID: 1008
		private IMetadataImporterProvider metadata_importer_provider;

		// Token: 0x040003F1 RID: 1009
		private IReflectionImporterProvider reflection_importer_provider;
	}
}
