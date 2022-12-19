using System;

namespace Mono.Cecil
{
	// Token: 0x0200014E RID: 334
	public sealed class ModuleParameters
	{
		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000A21 RID: 2593 RVA: 0x00025445 File Offset: 0x00023645
		// (set) Token: 0x06000A22 RID: 2594 RVA: 0x0002544D File Offset: 0x0002364D
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

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000A23 RID: 2595 RVA: 0x00025456 File Offset: 0x00023656
		// (set) Token: 0x06000A24 RID: 2596 RVA: 0x0002545E File Offset: 0x0002365E
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

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000A25 RID: 2597 RVA: 0x00025467 File Offset: 0x00023667
		// (set) Token: 0x06000A26 RID: 2598 RVA: 0x0002546F File Offset: 0x0002366F
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

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000A27 RID: 2599 RVA: 0x00025478 File Offset: 0x00023678
		// (set) Token: 0x06000A28 RID: 2600 RVA: 0x00025480 File Offset: 0x00023680
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

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000A29 RID: 2601 RVA: 0x00025489 File Offset: 0x00023689
		// (set) Token: 0x06000A2A RID: 2602 RVA: 0x00025491 File Offset: 0x00023691
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

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000A2B RID: 2603 RVA: 0x0002549A File Offset: 0x0002369A
		// (set) Token: 0x06000A2C RID: 2604 RVA: 0x000254A2 File Offset: 0x000236A2
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

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000A2D RID: 2605 RVA: 0x000254AB File Offset: 0x000236AB
		// (set) Token: 0x06000A2E RID: 2606 RVA: 0x000254B3 File Offset: 0x000236B3
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

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000A2F RID: 2607 RVA: 0x000254BC File Offset: 0x000236BC
		// (set) Token: 0x06000A30 RID: 2608 RVA: 0x000254C4 File Offset: 0x000236C4
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

		// Token: 0x06000A31 RID: 2609 RVA: 0x000254CD File Offset: 0x000236CD
		public ModuleParameters()
		{
			this.kind = ModuleKind.Dll;
			this.Runtime = ModuleParameters.GetCurrentRuntime();
			this.architecture = TargetArchitecture.I386;
		}

		// Token: 0x06000A32 RID: 2610 RVA: 0x000254F2 File Offset: 0x000236F2
		private static TargetRuntime GetCurrentRuntime()
		{
			return typeof(object).Assembly.ImageRuntimeVersion.ParseRuntime();
		}

		// Token: 0x040003B6 RID: 950
		private ModuleKind kind;

		// Token: 0x040003B7 RID: 951
		private TargetRuntime runtime;

		// Token: 0x040003B8 RID: 952
		private uint? timestamp;

		// Token: 0x040003B9 RID: 953
		private TargetArchitecture architecture;

		// Token: 0x040003BA RID: 954
		private IAssemblyResolver assembly_resolver;

		// Token: 0x040003BB RID: 955
		private IMetadataResolver metadata_resolver;

		// Token: 0x040003BC RID: 956
		private IMetadataImporterProvider metadata_importer_provider;

		// Token: 0x040003BD RID: 957
		private IReflectionImporterProvider reflection_importer_provider;
	}
}
