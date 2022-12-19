using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Speech.Internal.SrgsParser;
using System.Text;
using Microsoft.CSharp;
using Microsoft.VisualBasic;

namespace System.Speech.Internal.SrgsCompiler
{
	// Token: 0x020000FF RID: 255
	internal class CustomGrammar
	{
		// Token: 0x060008F8 RID: 2296 RVA: 0x00028B9C File Offset: 0x00026D9C
		internal CustomGrammar()
		{
		}

		// Token: 0x060008F9 RID: 2297 RVA: 0x00028C08 File Offset: 0x00026E08
		internal string CreateAssembly(int iCfg, string outputFile, CultureInfo culture)
		{
			string text = null;
			FileHelper.DeleteTemporaryFile(outputFile);
			try
			{
				this.CreateAssembly(outputFile, false, null);
				this.CheckValidAssembly(iCfg, CustomGrammar.ExtractCodeGenerated(outputFile));
				text = this.GenerateCode(true, culture);
			}
			finally
			{
				FileHelper.DeleteTemporaryFile(outputFile);
			}
			return text;
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x00028C58 File Offset: 0x00026E58
		internal void CreateAssembly(out byte[] il, out byte[] pdb)
		{
			string text;
			using (FileHelper.CreateAndOpenTemporaryFile(out text, FileAccess.Write, FileOptions.DeleteOnClose, "dll", "WPF"))
			{
			}
			try
			{
				this.CreateAssembly(text, this._fDebugScript, null);
				il = CustomGrammar.ExtractCodeGenerated(text);
				pdb = null;
				if (this._fDebugScript)
				{
					string text2 = text.Substring(0, text.LastIndexOf('.')) + ".pdb";
					pdb = CustomGrammar.ExtractCodeGenerated(text2);
					FileHelper.DeleteTemporaryFile(text2);
				}
				this.CheckValidAssembly(0, il);
			}
			finally
			{
				FileHelper.DeleteTemporaryFile(text);
			}
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x00028D04 File Offset: 0x00026F04
		internal void CreateAssembly(string path, List<CustomGrammar.CfgResource> cfgResources)
		{
			this.CreateAssembly(path, this._fDebugScript, cfgResources);
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x00028D14 File Offset: 0x00026F14
		internal void Combine(CustomGrammar cg, string innerCode)
		{
			if (this._rules.Count == 0)
			{
				this._language = cg._language;
			}
			else if (this._language != cg._language)
			{
				XmlParser.ThrowSrgsException(SRID.IncompatibleLanguageProperties, new object[0]);
			}
			if (this._namespace == null)
			{
				this._namespace = cg._namespace;
			}
			else if (this._namespace != cg._namespace)
			{
				XmlParser.ThrowSrgsException(SRID.IncompatibleNamespaceProperties, new object[0]);
			}
			this._fDebugScript |= cg._fDebugScript;
			foreach (string text in cg._codebehind)
			{
				if (!this._codebehind.Contains(text))
				{
					this._codebehind.Add(text);
				}
			}
			foreach (string text2 in cg._assemblyReferences)
			{
				if (!this._assemblyReferences.Contains(text2))
				{
					this._assemblyReferences.Add(text2);
				}
			}
			foreach (string text3 in cg._importNamespaces)
			{
				if (!this._importNamespaces.Contains(text3))
				{
					this._importNamespaces.Add(text3);
				}
			}
			this._keyFile = cg._keyFile;
			this._types.AddRange(cg._types);
			foreach (Rule rule in cg._rules)
			{
				if (this._types.Contains(rule.Name))
				{
					XmlParser.ThrowSrgsException(SRID.RuleDefinedMultipleTimes2, new object[] { rule.Name });
				}
			}
			this._script.Append(innerCode);
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x060008FD RID: 2301 RVA: 0x00028F30 File Offset: 0x00027130
		internal bool HasScript
		{
			get
			{
				bool flag = this._script.Length > 0 || this._codebehind.Count > 0;
				if (!flag)
				{
					foreach (Rule rule in this._rules)
					{
						if (rule.Script.Length > 0)
						{
							flag = true;
							break;
						}
					}
				}
				return flag;
			}
		}

		// Token: 0x060008FE RID: 2302 RVA: 0x00028FB4 File Offset: 0x000271B4
		private void CreateAssembly(string outputFile, bool debug, List<CustomGrammar.CfgResource> cfgResources)
		{
			if (this._language == null)
			{
				XmlParser.ThrowSrgsException(SRID.NoLanguageSet, new object[0]);
			}
			string text = this.GenerateCode(false, null);
			string text2 = null;
			string[] array = null;
			try
			{
				if (this._codebehind.Count > 0)
				{
					int num = this._codebehind.Count + ((text != null) ? 1 : 0);
					array = new string[num];
					for (int i = 0; i < this._codebehind.Count; i++)
					{
						array[i] = this._codebehind[i];
					}
					if (text != null)
					{
						using (FileStream fileStream = FileHelper.CreateAndOpenTemporaryFile(out text2, FileAccess.Write, FileOptions.None, null, "WPF"))
						{
							array[array.Length - 1] = text2;
							using (StreamWriter streamWriter = new StreamWriter(fileStream))
							{
								streamWriter.Write(text);
							}
						}
					}
				}
				this.CompileScript(outputFile, debug, text, array, cfgResources);
			}
			finally
			{
				FileHelper.DeleteTemporaryFile(text2);
			}
		}

		// Token: 0x060008FF RID: 2303 RVA: 0x000290C0 File Offset: 0x000272C0
		private void CompileScript(string outputFile, bool debug, string code, string[] codeFiles, List<CustomGrammar.CfgResource> cfgResouces)
		{
			using (CodeDomProvider codeDomProvider = this.CodeProvider())
			{
				CompilerParameters compilerParameters = CustomGrammar.GetCompilerParameters(outputFile, cfgResouces, debug, this._assemblyReferences, this._keyFile);
				CompilerResults compilerResults;
				if (codeFiles != null)
				{
					compilerResults = codeDomProvider.CompileAssemblyFromFile(compilerParameters, codeFiles);
				}
				else
				{
					compilerResults = codeDomProvider.CompileAssemblyFromSource(compilerParameters, new string[] { code });
				}
				if (compilerResults.Errors.Count > 0)
				{
					CustomGrammar.ThrowCompilationErrors(compilerResults);
				}
				if (compilerResults.NativeCompilerReturnValue != 0)
				{
					XmlParser.ThrowSrgsException(SRID.UnexpectedError, new object[] { compilerResults.NativeCompilerReturnValue });
				}
			}
		}

		// Token: 0x06000900 RID: 2304 RVA: 0x00029160 File Offset: 0x00027360
		private CodeDomProvider CodeProvider()
		{
			CodeDomProvider codeDomProvider = null;
			string language = this._language;
			if (!(language == "C#"))
			{
				if (!(language == "VB.Net"))
				{
					XmlParser.ThrowSrgsException(SRID.UnsupportedLanguage, new object[] { this._language });
				}
				else
				{
					codeDomProvider = CustomGrammar.CreateVBCompiler();
				}
			}
			else
			{
				codeDomProvider = CustomGrammar.CreateCSharpCompiler();
			}
			return codeDomProvider;
		}

		// Token: 0x06000901 RID: 2305 RVA: 0x000291BC File Offset: 0x000273BC
		private string GenerateCode(bool classDefinitionOnly, CultureInfo culture)
		{
			string text = string.Empty;
			string language = this._language;
			if (!(language == "C#"))
			{
				if (!(language == "VB.Net"))
				{
					XmlParser.ThrowSrgsException(SRID.UnsupportedLanguage, new object[] { this._language });
				}
				else
				{
					text = this.WrapScriptVB(classDefinitionOnly, culture);
				}
			}
			else
			{
				text = this.WrapScriptCSharp(classDefinitionOnly, culture);
			}
			return text;
		}

		// Token: 0x06000902 RID: 2306 RVA: 0x00029220 File Offset: 0x00027420
		private string WrapScriptCSharp(bool classDefinitionOnly, CultureInfo culture)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Rule rule in this._rules)
			{
				if (rule.Script != null)
				{
					CustomGrammar.WrapClassCSharp(stringBuilder, rule.Name, rule.BaseClass, culture, rule.Script.ToString(), rule.Constructors.ToString());
				}
			}
			if (this._script.Length > 0)
			{
				stringBuilder.Append(this._script);
			}
			if (stringBuilder.Length <= 0)
			{
				return null;
			}
			if (classDefinitionOnly)
			{
				return stringBuilder.ToString();
			}
			return this.WrapScriptOuterCSharp(stringBuilder.ToString());
		}

		// Token: 0x06000903 RID: 2307 RVA: 0x000292E0 File Offset: 0x000274E0
		private string WrapScriptVB(bool classDefinitionOnly, CultureInfo culture)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Rule rule in this._rules)
			{
				if (rule.Script != null)
				{
					CustomGrammar.WrapClassVB(stringBuilder, rule.Name, rule.BaseClass, culture, rule.Script.ToString(), rule.Constructors.ToString());
				}
			}
			if (this._script.Length > 0)
			{
				stringBuilder.Append(this._script);
			}
			if (stringBuilder.Length <= 0)
			{
				return null;
			}
			if (classDefinitionOnly)
			{
				return stringBuilder.ToString();
			}
			return this.WrapScriptOuterVB(stringBuilder.ToString());
		}

		// Token: 0x06000904 RID: 2308 RVA: 0x000293A0 File Offset: 0x000275A0
		private static CodeDomProvider CreateCSharpCompiler()
		{
			return new CSharpCodeProvider();
		}

		// Token: 0x06000905 RID: 2309 RVA: 0x000293A8 File Offset: 0x000275A8
		private string WrapScriptOuterCSharp(string innerCode)
		{
			if (!string.IsNullOrEmpty(innerCode))
			{
				int num = 0;
				foreach (string text in this._importNamespaces)
				{
					num += text.Length;
				}
				SRID srid = SRID.ArrayOfNullIllegal;
				string @namespace = srid.GetType().Namespace;
				string text2 = string.Format(CultureInfo.InvariantCulture, "#line 1 \"{0}\"\nusing System;\nusing System.Collections.Generic;\nusing System.Diagnostics;\nusing {1};\nusing {1}.Recognition;\nusing {1}.Recognition.SrgsGrammar;\n", new object[] { "<Does Not Exist>", @namespace });
				StringBuilder stringBuilder = new StringBuilder(this._script.Length + text2.Length + 200);
				stringBuilder.Append(text2);
				foreach (string text3 in this._importNamespaces)
				{
					stringBuilder.Append("using ");
					stringBuilder.Append(text3);
					stringBuilder.Append(";\n");
				}
				if (this._namespace != null)
				{
					stringBuilder.Append("namespace ");
					stringBuilder.Append(this._namespace);
					stringBuilder.Append("\n{\n");
				}
				stringBuilder.Append(innerCode);
				if (this._namespace != null)
				{
					stringBuilder.Append("}\n");
				}
				return stringBuilder.ToString();
			}
			return null;
		}

		// Token: 0x06000906 RID: 2310 RVA: 0x00029520 File Offset: 0x00027720
		private static void WrapClassCSharp(StringBuilder sb, string classname, string baseclass, CultureInfo culture, string script, string constructor)
		{
			sb.Append("public partial class ");
			sb.Append(classname);
			sb.Append(" : ");
			sb.Append((!string.IsNullOrEmpty(baseclass)) ? baseclass : "Grammar");
			sb.Append(" \n {\n");
			if (culture != null)
			{
				sb.Append("[DebuggerBrowsable (DebuggerBrowsableState.Never)]public static string __cultureId = \"");
				sb.Append(culture.LCID.ToString(CultureInfo.InvariantCulture));
				sb.Append("\";\n");
			}
			sb.Append(constructor);
			sb.Append(script);
			sb.Append("override protected bool IsStg { get { return true; }}\n\n");
			sb.Append("\n}\n");
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x000295CF File Offset: 0x000277CF
		private static CodeDomProvider CreateVBCompiler()
		{
			return new VBCodeProvider();
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x000295D8 File Offset: 0x000277D8
		private string WrapScriptOuterVB(string innerCode)
		{
			if (!string.IsNullOrEmpty(innerCode))
			{
				int num = 0;
				foreach (string text in this._importNamespaces)
				{
					num += text.Length;
				}
				SRID srid = SRID.ArrayOfNullIllegal;
				string @namespace = srid.GetType().Namespace;
				string text2 = string.Format(CultureInfo.InvariantCulture, "#ExternalSource (\"{0}\", 1)\nImports System\nImports System.Collections.Generic\nImports System.Diagnostics\nImports {1}\nImports {1}.Recognition\nImports {1}.Recognition.SrgsGrammar\n", new object[] { "<Does Not Exist>", @namespace });
				StringBuilder stringBuilder = new StringBuilder(this._script.Length + text2.Length + 200);
				stringBuilder.Append(text2);
				foreach (string text3 in this._importNamespaces)
				{
					stringBuilder.Append("Imports ");
					stringBuilder.Append(text3);
					stringBuilder.Append("\n");
				}
				if (this._namespace != null)
				{
					stringBuilder.Append("Namespace ");
					stringBuilder.Append(this._namespace);
					stringBuilder.Append("\n");
				}
				stringBuilder.Append("#End ExternalSource\n");
				stringBuilder.Append(innerCode);
				if (this._namespace != null)
				{
					stringBuilder.Append("End Namespace\n");
				}
				return stringBuilder.ToString();
			}
			return null;
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x0002975C File Offset: 0x0002795C
		private static void WrapClassVB(StringBuilder sb, string classname, string baseclass, CultureInfo culture, string script, string constructor)
		{
			sb.Append("Public Partial class ");
			sb.Append(classname);
			sb.Append("\n Inherits ");
			sb.Append((!string.IsNullOrEmpty(baseclass)) ? baseclass : "Grammar");
			sb.Append(" \n");
			if (culture != null)
			{
				sb.Append("<DebuggerBrowsable (DebuggerBrowsableState.Never)>Public Shared __cultureId as String = \"");
				sb.Append(culture.LCID.ToString(CultureInfo.InvariantCulture));
				sb.Append("\"\n");
			}
			sb.Append(constructor);
			sb.Append(script);
			sb.Append("Protected Overrides ReadOnly Property IsStg() As Boolean\nGet\nReturn True\nEnd Get\nEnd Property\n");
			sb.Append("\nEnd Class\n");
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x0002980C File Offset: 0x00027A0C
		private static void ThrowCompilationErrors(CompilerResults results)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (object obj in results.Errors)
			{
				CompilerError compilerError = (CompilerError)obj;
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append("\n");
				}
				if (compilerError.FileName.IndexOf("<Does Not Exist>", StringComparison.Ordinal) == -1)
				{
					stringBuilder.Append(compilerError.FileName);
					stringBuilder.Append("(");
					stringBuilder.Append(compilerError.Line);
					stringBuilder.Append(",");
					stringBuilder.Append(compilerError.Column);
					stringBuilder.Append("): ");
				}
				stringBuilder.Append("error ");
				stringBuilder.Append(compilerError.ErrorNumber);
				stringBuilder.Append(": ");
				stringBuilder.Append(compilerError.ErrorText);
			}
			XmlParser.ThrowSrgsException(SRID.GrammarCompilerError, new object[] { stringBuilder.ToString() });
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x00029928 File Offset: 0x00027B28
		private static CompilerParameters GetCompilerParameters(string outputFile, List<CustomGrammar.CfgResource> cfgResources, bool debug, Collection<string> assemblyReferences, string keyfile)
		{
			CompilerParameters compilerParameters = new CompilerParameters();
			StringBuilder stringBuilder = new StringBuilder();
			compilerParameters.GenerateInMemory = false;
			compilerParameters.OutputAssembly = outputFile;
			compilerParameters.IncludeDebugInformation = debug;
			if (debug)
			{
				stringBuilder.Append("/define:DEBUG ");
			}
			if (keyfile != null)
			{
				stringBuilder.Append("/keyfile:");
				stringBuilder.Append(keyfile);
			}
			compilerParameters.CompilerOptions = stringBuilder.ToString();
			compilerParameters.ReferencedAssemblies.Add("System.dll");
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			compilerParameters.ReferencedAssemblies.Add(executingAssembly.Location);
			foreach (string text in assemblyReferences)
			{
				compilerParameters.ReferencedAssemblies.Add(text);
			}
			if (cfgResources != null)
			{
				foreach (CustomGrammar.CfgResource cfgResource in cfgResources)
				{
					using (FileStream fileStream = new FileStream(cfgResource.name, FileMode.Create, FileAccess.Write))
					{
						using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
						{
							binaryWriter.Write(cfgResource.data, 0, cfgResource.data.Length);
							compilerParameters.EmbeddedResources.Add(cfgResource.name);
						}
					}
				}
			}
			return compilerParameters;
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x00029AB4 File Offset: 0x00027CB4
		private void CheckValidAssembly(int iCfg, byte[] il)
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			AppDomain appDomain = null;
			try
			{
				appDomain = AppDomain.CreateDomain("Loading Domain");
				AppDomainCompilerProxy appDomainCompilerProxy = (AppDomainCompilerProxy)appDomain.CreateInstanceFromAndUnwrap(executingAssembly.GetName().CodeBase, "System.Speech.Internal.SrgsCompiler.AppDomainCompilerProxy");
				int count = this._scriptRefs.Count;
				string[] array = new string[count];
				string[] array2 = new string[count];
				int[] array3 = new int[count];
				for (int i = 0; i < count; i++)
				{
					ScriptRef scriptRef = this._scriptRefs[i];
					array[i] = scriptRef._rule;
					array2[i] = scriptRef._sMethod;
					array3[i] = (int)scriptRef._method;
				}
				Exception ex = appDomainCompilerProxy.CheckAssembly(il, iCfg, this._language, this._namespace, array, array2, array3);
				if (ex != null)
				{
					throw ex;
				}
				CustomGrammar.AssociateConstructorsWithRules(appDomainCompilerProxy, array, this._rules, iCfg, this._language);
			}
			finally
			{
				if (appDomain != null)
				{
					AppDomain.Unload(appDomain);
					appDomain = null;
				}
			}
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x00029BB0 File Offset: 0x00027DB0
		private static void AssociateConstructorsWithRules(AppDomainCompilerProxy proxy, string[] names, List<Rule> rules, int iCfg, string language)
		{
			string[] array = proxy.Constructors();
			foreach (Rule rule in rules)
			{
				int num = 0;
				while (num < names.Length && (num = Array.IndexOf<string>(names, rule.Name, num)) >= 0)
				{
					if (array[num] != null)
					{
						rule.Constructors.Append(array[num]);
					}
					num++;
				}
				if (rule.Constructors.Length == 0)
				{
					rule.Constructors.Append(proxy.GenerateConstructor(iCfg, new ParameterInfo[0], language, rule.Name));
				}
			}
		}

		// Token: 0x0600090E RID: 2318 RVA: 0x00029C60 File Offset: 0x00027E60
		private static byte[] ExtractCodeGenerated(string path)
		{
			byte[] array = null;
			if (!string.IsNullOrEmpty(path))
			{
				using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
				{
					array = Helpers.ReadStreamToByteArray(fileStream, (int)fileStream.Length);
				}
			}
			return array;
		}

		// Token: 0x0400063B RID: 1595
		internal string _language = "C#";

		// Token: 0x0400063C RID: 1596
		internal string _namespace;

		// Token: 0x0400063D RID: 1597
		internal List<Rule> _rules = new List<Rule>();

		// Token: 0x0400063E RID: 1598
		internal Collection<string> _codebehind = new Collection<string>();

		// Token: 0x0400063F RID: 1599
		internal bool _fDebugScript;

		// Token: 0x04000640 RID: 1600
		internal Collection<string> _assemblyReferences = new Collection<string>();

		// Token: 0x04000641 RID: 1601
		internal Collection<string> _importNamespaces = new Collection<string>();

		// Token: 0x04000642 RID: 1602
		internal string _keyFile;

		// Token: 0x04000643 RID: 1603
		internal Collection<ScriptRef> _scriptRefs = new Collection<ScriptRef>();

		// Token: 0x04000644 RID: 1604
		internal List<string> _types = new List<string>();

		// Token: 0x04000645 RID: 1605
		internal StringBuilder _script = new StringBuilder();

		// Token: 0x04000646 RID: 1606
		private const string _preambuleMarker = "<Does Not Exist>";

		// Token: 0x020001AA RID: 426
		internal class CfgResource
		{
			// Token: 0x040009A8 RID: 2472
			internal string name;

			// Token: 0x040009A9 RID: 2473
			internal byte[] data;
		}
	}
}
