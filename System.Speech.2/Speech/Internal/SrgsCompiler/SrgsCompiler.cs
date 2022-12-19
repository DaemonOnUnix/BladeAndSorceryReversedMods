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
	// Token: 0x020000F9 RID: 249
	internal static class SrgsCompiler
	{
		// Token: 0x060008C0 RID: 2240 RVA: 0x00027A8C File Offset: 0x00025C8C
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

		// Token: 0x060008C1 RID: 2241 RVA: 0x00027B80 File Offset: 0x00025D80
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

		// Token: 0x060008C2 RID: 2242 RVA: 0x00027BEC File Offset: 0x00025DEC
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
					stream = new FileStream(filename, FileMode.Create, FileAccess.Write);
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

		// Token: 0x060008C3 RID: 2243 RVA: 0x00027DD4 File Offset: 0x00025FD4
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

		// Token: 0x060008C4 RID: 2244 RVA: 0x00027E20 File Offset: 0x00026020
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
