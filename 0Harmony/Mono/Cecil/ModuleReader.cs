using System;
using Mono.Cecil.Cil;
using Mono.Cecil.PE;

namespace Mono.Cecil
{
	// Token: 0x020001B6 RID: 438
	internal abstract class ModuleReader
	{
		// Token: 0x060007ED RID: 2029 RVA: 0x0001AEB1 File Offset: 0x000190B1
		protected ModuleReader(Image image, ReadingMode mode)
		{
			this.module = new ModuleDefinition(image);
			this.module.ReadingMode = mode;
		}

		// Token: 0x060007EE RID: 2030
		protected abstract void ReadModule();

		// Token: 0x060007EF RID: 2031
		public abstract void ReadSymbols(ModuleDefinition module);

		// Token: 0x060007F0 RID: 2032 RVA: 0x0001AED1 File Offset: 0x000190D1
		protected void ReadModuleManifest(MetadataReader reader)
		{
			reader.Populate(this.module);
			this.ReadAssembly(reader);
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x0001AEE8 File Offset: 0x000190E8
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

		// Token: 0x060007F2 RID: 2034 RVA: 0x0001AF34 File Offset: 0x00019134
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

		// Token: 0x060007F3 RID: 2035 RVA: 0x0001AFE4 File Offset: 0x000191E4
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

		// Token: 0x060007F4 RID: 2036 RVA: 0x0001B084 File Offset: 0x00019284
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

		// Token: 0x060007F5 RID: 2037 RVA: 0x0001B0D9 File Offset: 0x000192D9
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

		// Token: 0x04000282 RID: 642
		protected readonly ModuleDefinition module;
	}
}
