using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Speech.Internal.SrgsParser;
using System.Speech.Recognition.SrgsGrammar;
using System.Text;
using System.Xml;

namespace System.Speech.Internal.SrgsCompiler
{
	// Token: 0x020000B8 RID: 184
	internal static class SrgsCompiler
	{
		// Token: 0x06000407 RID: 1031 RVA: 0x0000FD98 File Offset: 0x0000ED98
		internal static void CompileStream(XmlReader[] xmlReaders, string filename, Stream stream, bool fOutputCfg, Uri originalUri, string[] referencedAssemblies, string keyFile)
		{
			int num = xmlReaders.Length;
			List<CustomGrammar.CfgResource> list = new List<CustomGrammar.CfgResource>();
			CustomGrammar customGrammar = new CustomGrammar();
			for (int i = 0; i < num; i++)
			{
				string text = null;
				Uri uri = originalUri;
				if (uri == null && xmlReaders[i].BaseURI != null && xmlReaders[i].BaseURI.Length > 0)
				{
					uri = new Uri(xmlReaders[i].BaseURI);
				}
				if (uri != null && (!uri.IsAbsoluteUri || uri.IsFile))
				{
					text = Path.GetDirectoryName(uri.IsAbsoluteUri ? uri.AbsolutePath : uri.OriginalString);
				}
				StringBuilder stringBuilder = new StringBuilder();
				ISrgsParser srgsParser = new XmlParser(xmlReaders[i], uri);
				CultureInfo cultureInfo;
				object obj = SrgsCompiler.CompileStream(i + 1, srgsParser, text, filename, stream, fOutputCfg, stringBuilder, list, out cultureInfo, referencedAssemblies, keyFile);
				if (!fOutputCfg)
				{
					customGrammar.Combine((CustomGrammar)obj, stringBuilder.ToString());
				}
			}
			if (!fOutputCfg)
			{
				customGrammar.CreateAssembly(filename, list);
			}
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x0000FE8C File Offset: 0x0000EE8C
		internal static void CompileStream(SrgsDocument srgsGrammar, string filename, Stream stream, bool fOutputCfg, string[] referencedAssemblies, string keyFile)
		{
			ISrgsParser srgsParser = new SrgsDocumentParser(srgsGrammar.Grammar);
			List<CustomGrammar.CfgResource> list = new List<CustomGrammar.CfgResource>();
			StringBuilder stringBuilder = new StringBuilder();
			srgsGrammar.Grammar.Validate();
			CultureInfo cultureInfo;
			object obj = SrgsCompiler.CompileStream(1, srgsParser, null, filename, stream, fOutputCfg, stringBuilder, list, out cultureInfo, referencedAssemblies, keyFile);
			if (!fOutputCfg)
			{
				CustomGrammar customGrammar = new CustomGrammar();
				customGrammar.Combine((CustomGrammar)obj, stringBuilder.ToString());
				customGrammar.CreateAssembly(filename, list);
			}
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x0000FEF8 File Offset: 0x0000EEF8
		private static object CompileStream(int iCfg, ISrgsParser srgsParser, string srgsPath, string filename, Stream stream, bool fOutputCfg, StringBuilder innerCode, object cfgResources, out CultureInfo culture, string[] referencedAssemblies, string keyFile)
		{
			Backend backend = new Backend();
			CustomGrammar customGrammar = new CustomGrammar();
			SrgsElementCompilerFactory srgsElementCompilerFactory = new SrgsElementCompilerFactory(backend, customGrammar);
			srgsParser.ElementFactory = srgsElementCompilerFactory;
			srgsParser.Parse();
			backend.Optimize();
			culture = ((backend.LangId == 21514) ? new CultureInfo("es-us") : new CultureInfo(backend.LangId));
			if (customGrammar._codebehind.Count > 0 && !string.IsNullOrEmpty(srgsPath))
			{
				for (int i = 0; i < customGrammar._codebehind.Count; i++)
				{
					if (!File.Exists(customGrammar._codebehind[i]))
					{
						customGrammar._codebehind[i] = srgsPath + "\\" + customGrammar._codebehind[i];
					}
				}
			}
			if (referencedAssemblies != null)
			{
				foreach (string text in referencedAssemblies)
				{
					customGrammar._assemblyReferences.Add(text);
				}
			}
			customGrammar._keyFile = keyFile;
			backend.ScriptRefs = customGrammar._scriptRefs;
			if (!fOutputCfg)
			{
				CustomGrammar.CfgResource cfgResource = new CustomGrammar.CfgResource();
				cfgResource.data = SrgsCompiler.BuildCfg(backend).ToArray();
				cfgResource.name = iCfg.ToString(CultureInfo.InvariantCulture) + ".CFG";
				((List<CustomGrammar.CfgResource>)cfgResources).Add(cfgResource);
				innerCode.Append(customGrammar.CreateAssembly(iCfg, filename, culture));
			}
			else
			{
				if (customGrammar._scriptRefs.Count > 0 && !customGrammar.HasScript)
				{
					XmlParser.ThrowSrgsException(SRID.NoScriptsForRules, new object[0]);
				}
				SrgsCompiler.CreateAssembly(backend, customGrammar);
				if (!string.IsNullOrEmpty(filename))
				{
					stream = new FileStream(filename, 2, 2);
				}
				try
				{
					using (StreamMarshaler streamMarshaler = new StreamMarshaler(stream))
					{
						backend.Commit(streamMarshaler);
					}
				}
				finally
				{
					if (!string.IsNullOrEmpty(filename))
					{
						stream.Close();
					}
				}
			}
			return customGrammar;
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x000100E0 File Offset: 0x0000F0E0
		private static MemoryStream BuildCfg(Backend backend)
		{
			MemoryStream memoryStream = new MemoryStream();
			using (StreamMarshaler streamMarshaler = new StreamMarshaler(memoryStream))
			{
				backend.IL = null;
				backend.PDB = null;
				backend.Commit(streamMarshaler);
			}
			return memoryStream;
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x0001012C File Offset: 0x0000F12C
		private static void CreateAssembly(Backend backend, CustomGrammar cg)
		{
			if (cg.HasScript)
			{
				byte[] array;
				byte[] array2;
				cg.CreateAssembly(out array, out array2);
				backend.IL = array;
				backend.PDB = array2;
			}
		}
	}
}
