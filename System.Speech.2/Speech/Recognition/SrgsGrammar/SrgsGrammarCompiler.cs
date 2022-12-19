using System;
using System.IO;
using System.Speech.Internal;
using System.Speech.Internal.SrgsCompiler;
using System.Xml;

namespace System.Speech.Recognition.SrgsGrammar
{
	// Token: 0x02000079 RID: 121
	public static class SrgsGrammarCompiler
	{
		// Token: 0x06000403 RID: 1027 RVA: 0x0001036C File Offset: 0x0000E56C
		public static void Compile(string inputPath, Stream outputStream)
		{
			Helpers.ThrowIfEmptyOrNull(inputPath, "inputPath");
			Helpers.ThrowIfNull(outputStream, "outputStream");
			using (XmlTextReader xmlTextReader = new XmlTextReader(new Uri(inputPath, UriKind.RelativeOrAbsolute).ToString()))
			{
				SrgsCompiler.CompileStream(new XmlReader[] { xmlTextReader }, null, outputStream, true, null, null, null);
			}
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x000103D4 File Offset: 0x0000E5D4
		public static void Compile(SrgsDocument srgsGrammar, Stream outputStream)
		{
			Helpers.ThrowIfNull(srgsGrammar, "srgsGrammar");
			Helpers.ThrowIfNull(outputStream, "outputStream");
			SrgsCompiler.CompileStream(srgsGrammar, null, outputStream, true, null, null);
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x000103F7 File Offset: 0x0000E5F7
		public static void Compile(XmlReader reader, Stream outputStream)
		{
			Helpers.ThrowIfNull(reader, "reader");
			Helpers.ThrowIfNull(outputStream, "outputStream");
			SrgsCompiler.CompileStream(new XmlReader[] { reader }, null, outputStream, true, null, null, null);
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x00010424 File Offset: 0x0000E624
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
					array[i] = new XmlTextReader(new Uri(inputPaths[i], UriKind.RelativeOrAbsolute).ToString());
				}
				SrgsCompiler.CompileStream(array, outputPath, null, false, null, referencedAssemblies, keyFile);
			}
			finally
			{
				foreach (XmlTextReader xmlTextReader in array)
				{
					if (xmlTextReader != null)
					{
						((IDisposable)xmlTextReader).Dispose();
					}
				}
			}
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x000104CC File Offset: 0x0000E6CC
		public static void CompileClassLibrary(SrgsDocument srgsGrammar, string outputPath, string[] referencedAssemblies, string keyFile)
		{
			Helpers.ThrowIfNull(srgsGrammar, "srgsGrammar");
			Helpers.ThrowIfEmptyOrNull(outputPath, "outputPath");
			SrgsCompiler.CompileStream(srgsGrammar, outputPath, null, false, referencedAssemblies, keyFile);
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x000104EF File Offset: 0x0000E6EF
		public static void CompileClassLibrary(XmlReader reader, string outputPath, string[] referencedAssemblies, string keyFile)
		{
			Helpers.ThrowIfNull(reader, "reader");
			Helpers.ThrowIfEmptyOrNull(outputPath, "outputPath");
			SrgsCompiler.CompileStream(new XmlReader[] { reader }, outputPath, null, false, null, referencedAssemblies, keyFile);
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x0001051C File Offset: 0x0000E71C
		private static bool CheckIfCfg(Stream stream, out int cfgLength)
		{
			long position = stream.Position;
			bool flag = CfgGrammar.CfgSerializedHeader.IsCfg(stream, out cfgLength);
			stream.Position = position;
			return flag;
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x00010540 File Offset: 0x0000E740
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
