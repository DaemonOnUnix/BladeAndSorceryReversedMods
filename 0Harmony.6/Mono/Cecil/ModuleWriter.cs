﻿using System;
using System.IO;
using Mono.Cecil.Cil;
using Mono.Cecil.PE;

namespace Mono.Cecil
{
	// Token: 0x020000CA RID: 202
	internal static class ModuleWriter
	{
		// Token: 0x060005AF RID: 1455 RVA: 0x0001A628 File Offset: 0x00018828
		public static void WriteModule(ModuleDefinition module, Disposable<Stream> stream, WriterParameters parameters)
		{
			using (stream)
			{
				ModuleWriter.Write(module, stream, parameters);
			}
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x0001A660 File Offset: 0x00018860
		private static void Write(ModuleDefinition module, Disposable<Stream> stream, WriterParameters parameters)
		{
			if ((module.Attributes & ModuleAttributes.ILOnly) == (ModuleAttributes)0)
			{
				throw new NotSupportedException("Writing mixed-mode assemblies is not supported");
			}
			if (module.HasImage && module.ReadingMode == ReadingMode.Deferred)
			{
				ImmediateModuleReader immediateModuleReader = new ImmediateModuleReader(module.Image);
				immediateModuleReader.ReadModule(module, false);
				immediateModuleReader.ReadSymbols(module);
			}
			module.MetadataSystem.Clear();
			if (module.symbol_reader != null)
			{
				module.symbol_reader.Dispose();
			}
			AssemblyNameDefinition assemblyNameDefinition = ((module.assembly != null) ? module.assembly.Name : null);
			string fileName = stream.value.GetFileName();
			uint num = parameters.Timestamp ?? module.timestamp;
			ISymbolWriterProvider symbolWriterProvider = parameters.SymbolWriterProvider;
			if (symbolWriterProvider == null && parameters.WriteSymbols)
			{
				symbolWriterProvider = new DefaultSymbolWriterProvider();
			}
			if (parameters.HasStrongNameKey && assemblyNameDefinition != null)
			{
				assemblyNameDefinition.PublicKey = CryptoService.GetPublicKey(parameters);
				module.Attributes |= ModuleAttributes.StrongNameSigned;
			}
			if (parameters.DeterministicMvid)
			{
				module.Mvid = Guid.Empty;
			}
			MetadataBuilder metadataBuilder = new MetadataBuilder(module, fileName, num, symbolWriterProvider);
			try
			{
				module.metadata_builder = metadataBuilder;
				using (ISymbolWriter symbolWriter = ModuleWriter.GetSymbolWriter(module, fileName, symbolWriterProvider, parameters))
				{
					metadataBuilder.SetSymbolWriter(symbolWriter);
					ModuleWriter.BuildMetadata(module, metadataBuilder);
					if (parameters.DeterministicMvid)
					{
						metadataBuilder.ComputeDeterministicMvid();
					}
					ImageWriter imageWriter = ImageWriter.CreateWriter(module, metadataBuilder, stream);
					stream.value.SetLength(0L);
					imageWriter.WriteImage();
					if (parameters.HasStrongNameKey)
					{
						CryptoService.StrongName(stream.value, imageWriter, parameters);
					}
				}
			}
			finally
			{
				module.metadata_builder = null;
			}
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x0001A804 File Offset: 0x00018A04
		private static void BuildMetadata(ModuleDefinition module, MetadataBuilder metadata)
		{
			if (!module.HasImage)
			{
				metadata.BuildMetadata();
				return;
			}
			module.Read<MetadataBuilder, MetadataBuilder>(metadata, delegate(MetadataBuilder builder, MetadataReader _)
			{
				builder.BuildMetadata();
				return builder;
			});
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x0001A83C File Offset: 0x00018A3C
		private static ISymbolWriter GetSymbolWriter(ModuleDefinition module, string fq_name, ISymbolWriterProvider symbol_writer_provider, WriterParameters parameters)
		{
			if (symbol_writer_provider == null)
			{
				return null;
			}
			if (parameters.SymbolStream != null)
			{
				return symbol_writer_provider.GetSymbolWriter(module, parameters.SymbolStream);
			}
			return symbol_writer_provider.GetSymbolWriter(module, fq_name);
		}
	}
}
