using System;
using Mono.Cecil.Cil;
using Mono.Cecil.PE;

namespace Mono.Cecil
{
	// Token: 0x020000C4 RID: 196
	internal abstract class ModuleReader
	{
		// Token: 0x060004B7 RID: 1207 RVA: 0x0001502C File Offset: 0x0001322C
		protected ModuleReader(Image image, ReadingMode mode)
		{
			this.module = new ModuleDefinition(image);
			this.module.ReadingMode = mode;
		}

		// Token: 0x060004B8 RID: 1208
		protected abstract void ReadModule();

		// Token: 0x060004B9 RID: 1209
		public abstract void ReadSymbols(ModuleDefinition module);

		// Token: 0x060004BA RID: 1210 RVA: 0x0001504C File Offset: 0x0001324C
		protected void ReadModuleManifest(MetadataReader reader)
		{
			reader.Populate(this.module);
			this.ReadAssembly(reader);
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x00015064 File Offset: 0x00013264
		private void ReadAssembly(MetadataReader reader)
		{
			AssemblyNameDefinition assemblyNameDefinition = reader.ReadAssemblyNameDefinition();
			if (assemblyNameDefinition == null)
			{
				this.module.kind = ModuleKind.NetModule;
				return;
			}
			AssemblyDefinition assemblyDefinition = new AssemblyDefinition();
			assemblyDefinition.Name = assemblyNameDefinition;
			this.module.assembly = assemblyDefinition;
			assemblyDefinition.main_module = this.module;
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x000150B0 File Offset: 0x000132B0
		public static ModuleDefinition CreateModule(Image image, ReaderParameters parameters)
		{
			ModuleReader moduleReader = ModuleReader.CreateModuleReader(image, parameters.ReadingMode);
			ModuleDefinition moduleDefinition = moduleReader.module;
			if (parameters.assembly_resolver != null)
			{
				moduleDefinition.assembly_resolver = Disposable.NotOwned<IAssemblyResolver>(parameters.assembly_resolver);
			}
			if (parameters.metadata_resolver != null)
			{
				moduleDefinition.metadata_resolver = parameters.metadata_resolver;
			}
			if (parameters.metadata_importer_provider != null)
			{
				moduleDefinition.metadata_importer = parameters.metadata_importer_provider.GetMetadataImporter(moduleDefinition);
			}
			if (parameters.reflection_importer_provider != null)
			{
				moduleDefinition.reflection_importer = parameters.reflection_importer_provider.GetReflectionImporter(moduleDefinition);
			}
			ModuleReader.GetMetadataKind(moduleDefinition, parameters);
			moduleReader.ReadModule();
			ModuleReader.ReadSymbols(moduleDefinition, parameters);
			moduleReader.ReadSymbols(moduleDefinition);
			if (parameters.ReadingMode == ReadingMode.Immediate)
			{
				moduleDefinition.MetadataSystem.Clear();
			}
			return moduleDefinition;
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x00015160 File Offset: 0x00013360
		private static void ReadSymbols(ModuleDefinition module, ReaderParameters parameters)
		{
			ISymbolReaderProvider symbolReaderProvider = parameters.SymbolReaderProvider;
			if (symbolReaderProvider == null && parameters.ReadSymbols)
			{
				symbolReaderProvider = new DefaultSymbolReaderProvider();
			}
			if (symbolReaderProvider != null)
			{
				module.SymbolReaderProvider = symbolReaderProvider;
				ISymbolReader symbolReader = ((parameters.SymbolStream != null) ? symbolReaderProvider.GetSymbolReader(module, parameters.SymbolStream) : symbolReaderProvider.GetSymbolReader(module, module.FileName));
				if (symbolReader != null)
				{
					try
					{
						module.ReadSymbols(symbolReader, parameters.ThrowIfSymbolsAreNotMatching);
					}
					catch (Exception)
					{
						symbolReader.Dispose();
						throw;
					}
				}
			}
			if (module.Image.HasDebugTables())
			{
				module.ReadSymbols(new PortablePdbReader(module.Image, module));
			}
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x00015200 File Offset: 0x00013400
		private static void GetMetadataKind(ModuleDefinition module, ReaderParameters parameters)
		{
			if (!parameters.ApplyWindowsRuntimeProjections)
			{
				module.MetadataKind = MetadataKind.Ecma335;
				return;
			}
			string runtimeVersion = module.RuntimeVersion;
			if (!runtimeVersion.Contains("WindowsRuntime"))
			{
				module.MetadataKind = MetadataKind.Ecma335;
				return;
			}
			if (runtimeVersion.Contains("CLR"))
			{
				module.MetadataKind = MetadataKind.ManagedWindowsMetadata;
				return;
			}
			module.MetadataKind = MetadataKind.WindowsMetadata;
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x00015255 File Offset: 0x00013455
		private static ModuleReader CreateModuleReader(Image image, ReadingMode mode)
		{
			if (mode == ReadingMode.Immediate)
			{
				return new ImmediateModuleReader(image);
			}
			if (mode != ReadingMode.Deferred)
			{
				throw new ArgumentException();
			}
			return new DeferredModuleReader(image);
		}

		// Token: 0x04000250 RID: 592
		protected readonly ModuleDefinition module;
	}
}
