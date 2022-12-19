using System;
using System.IO;
using System.Speech.Internal;
using System.Speech.Internal.SrgsCompiler;
using System.Xml;

namespace System.Speech.Recognition.SrgsGrammar
{
	// Token: 0x02000118 RID: 280
	public static class SrgsGrammarCompiler
	{
		// Token: 0x0600074B RID: 1867 RVA: 0x00020B00 File Offset: 0x0001FB00
		public static void Compile(string inputPath, Stream outputStream)
		{
			Helpers.ThrowIfEmptyOrNull(inputPath, "inputPath");
			Helpers.ThrowIfNull(outputStream, "outputStream");
			using (XmlTextReader xmlTextReader = new XmlTextReader(new Uri(inputPath, 0).ToString()))
			{
				SrgsCompiler.CompileStream(new XmlReader[] { xmlTextReader }, null, outputStream, true, null, null, null);
			}
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x00020B68 File Offset: 0x0001FB68
		public static void Compile(SrgsDocument srgsGrammar, Stream outputStream)
		{
			Helpers.ThrowIfNull(srgsGrammar, "srgsGrammar");
			Helpers.ThrowIfNull(outputStream, "outputStream");
			SrgsCompiler.CompileStream(srgsGrammar, null, outputStream, true, null, null);
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x00020B8C File Offset: 0x0001FB8C
		public static void Compile(XmlReader reader, Stream outputStream)
		{
			Helpers.ThrowIfNull(reader, "reader");
			Helpers.ThrowIfNull(outputStream, "outputStream");
			SrgsCompiler.CompileStream(new XmlReader[] { reader }, null, outputStream, true, null, null, null);
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x00020BC8 File Offset: 0x0001FBC8
		public static void CompileClassLibrary(string[] inputPaths, string outputPath, string[] referencedAssemblies, string keyFile)
		{
			Helpers.ThrowIfNull(inputPaths, "inputPaths");
			Helpers.ThrowIfEmptyOrNull(outputPath, "outputPath");
			XmlTextReader[] array = new XmlTextReader[inputPaths.Length];
			try
			{
				for (int i = 0; i < inputPaths.Length; i++)
				{
					if (inputPaths[i] == null)
					{
						throw new ArgumentException(SR.Get(SRID.ArrayOfNullIllegal, new object[0]), "inputPaths");
					}
					array[i] = new XmlTextReader(new Uri(inputPaths[i], 0).ToString());
				}
				SrgsCompiler.CompileStream(array, outputPath, null, false, null, referencedAssemblies, keyFile);
			}
			finally
			{
				foreach (XmlTextReader xmlTextReader in array)
				{
					if (xmlTextReader != null)
					{
						xmlTextReader.Dispose();
					}
				}
			}
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x00020C70 File Offset: 0x0001FC70
		public static void CompileClassLibrary(SrgsDocument srgsGrammar, string outputPath, string[] referencedAssemblies, string keyFile)
		{
			Helpers.ThrowIfNull(srgsGrammar, "srgsGrammar");
			Helpers.ThrowIfEmptyOrNull(outputPath, "outputPath");
			SrgsCompiler.CompileStream(srgsGrammar, outputPath, null, false, referencedAssemblies, keyFile);
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x00020C94 File Offset: 0x0001FC94
		public static void CompileClassLibrary(XmlReader reader, string outputPath, string[] referencedAssemblies, string keyFile)
		{
			Helpers.ThrowIfNull(reader, "reader");
			Helpers.ThrowIfEmptyOrNull(outputPath, "outputPath");
			SrgsCompiler.CompileStream(new XmlReader[] { reader }, outputPath, null, false, null, referencedAssemblies, keyFile);
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x00020CD0 File Offset: 0x0001FCD0
		private static bool CheckIfCfg(Stream stream, out int cfgLength)
		{
			long position = stream.Position;
			bool flag = CfgGrammar.CfgSerializedHeader.IsCfg(stream, out cfgLength);
			stream.Position = position;
			return flag;
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x00020CF4 File Offset: 0x0001FCF4
		internal static void CompileXmlOrCopyCfg(Stream inputStream, Stream outputStream, Uri orginalUri)
		{
			SeekableReadStream seekableReadStream = new SeekableReadStream(inputStream);
			int num;
			bool flag = SrgsGrammarCompiler.CheckIfCfg(seekableReadStream, out num);
			seekableReadStream.CacheDataForSeeking = false;
			if (flag)
			{
				Helpers.CopyStream(seekableReadStream, outputStream, num);
				return;
			}
			SrgsCompiler.CompileStream(new XmlReader[]
			{
				new XmlTextReader(seekableReadStream)
			}, null, outputStream, true, orginalUri, null, null);
		}
	}
}
