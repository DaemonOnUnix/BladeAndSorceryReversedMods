using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Speech.Internal;
using System.Speech.Internal.SrgsCompiler;
using System.Speech.Recognition.SrgsGrammar;
using System.Text;

namespace System.Speech.Recognition
{
	// Token: 0x02000046 RID: 70
	[DebuggerDisplay("Grammar: {(_uri != null ? \"uri=\" + _uri.ToString () + \" \" : \"\") + \"rule=\" + _ruleName }")]
	public class Grammar
	{
		// Token: 0x06000137 RID: 311 RVA: 0x000053D2 File Offset: 0x000035D2
		internal Grammar(Uri uri, string ruleName, object[] parameters)
		{
			Helpers.ThrowIfNull(uri, "uri");
			this._uri = uri;
			this.InitialGrammarLoad(ruleName, parameters, false);
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00005412 File Offset: 0x00003612
		public Grammar(string path)
			: this(path, null, null)
		{
		}

		// Token: 0x06000139 RID: 313 RVA: 0x0000541D File Offset: 0x0000361D
		public Grammar(string path, string ruleName)
			: this(path, ruleName, null)
		{
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00005428 File Offset: 0x00003628
		public Grammar(string path, string ruleName, object[] parameters)
		{
			try
			{
				this._uri = new Uri(path, UriKind.Relative);
			}
			catch (UriFormatException ex)
			{
				throw new ArgumentException(SR.Get(SRID.RecognizerGrammarNotFound, new object[0]), "path", ex);
			}
			this.InitialGrammarLoad(ruleName, parameters, false);
		}

		// Token: 0x0600013B RID: 315 RVA: 0x000054A0 File Offset: 0x000036A0
		public Grammar(SrgsDocument srgsDocument)
			: this(srgsDocument, null, null, null)
		{
		}

		// Token: 0x0600013C RID: 316 RVA: 0x000054AC File Offset: 0x000036AC
		public Grammar(SrgsDocument srgsDocument, string ruleName)
			: this(srgsDocument, ruleName, null, null)
		{
		}

		// Token: 0x0600013D RID: 317 RVA: 0x000054B8 File Offset: 0x000036B8
		public Grammar(SrgsDocument srgsDocument, string ruleName, object[] parameters)
			: this(srgsDocument, ruleName, null, parameters)
		{
		}

		// Token: 0x0600013E RID: 318 RVA: 0x000054C4 File Offset: 0x000036C4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public Grammar(SrgsDocument srgsDocument, string ruleName, Uri baseUri)
			: this(srgsDocument, ruleName, baseUri, null)
		{
		}

		// Token: 0x0600013F RID: 319 RVA: 0x000054D0 File Offset: 0x000036D0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public Grammar(SrgsDocument srgsDocument, string ruleName, Uri baseUri, object[] parameters)
		{
			Helpers.ThrowIfNull(srgsDocument, "srgsDocument");
			this._srgsDocument = srgsDocument;
			this._isSrgsDocument = srgsDocument != null;
			this._baseUri = baseUri;
			this.InitialGrammarLoad(ruleName, parameters, false);
		}

		// Token: 0x06000140 RID: 320 RVA: 0x0000552D File Offset: 0x0000372D
		public Grammar(Stream stream)
			: this(stream, null, null, null)
		{
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00005539 File Offset: 0x00003739
		public Grammar(Stream stream, string ruleName)
			: this(stream, ruleName, null, null)
		{
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00005545 File Offset: 0x00003745
		public Grammar(Stream stream, string ruleName, object[] parameters)
			: this(stream, ruleName, null, parameters)
		{
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00005551 File Offset: 0x00003751
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public Grammar(Stream stream, string ruleName, Uri baseUri)
			: this(stream, ruleName, baseUri, null)
		{
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00005560 File Offset: 0x00003760
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public Grammar(Stream stream, string ruleName, Uri baseUri, object[] parameters)
		{
			Helpers.ThrowIfNull(stream, "stream");
			if (!stream.CanRead)
			{
				throw new ArgumentException(SR.Get(SRID.StreamMustBeReadable, new object[0]), "stream");
			}
			this._appStream = stream;
			this._baseUri = baseUri;
			this.InitialGrammarLoad(ruleName, parameters, false);
		}

		// Token: 0x06000145 RID: 325 RVA: 0x000055D3 File Offset: 0x000037D3
		public Grammar(GrammarBuilder builder)
		{
			Helpers.ThrowIfNull(builder, "builder");
			this._grammarBuilder = builder;
			this.InitialGrammarLoad(null, null, false);
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00005613 File Offset: 0x00003813
		private Grammar(string onInitParameters, Stream stream, string ruleName)
		{
			this._appStream = stream;
			this._onInitParameters = onInitParameters;
			this.InitialGrammarLoad(ruleName, null, true);
		}

		// Token: 0x06000147 RID: 327 RVA: 0x0000564F File Offset: 0x0000384F
		protected Grammar()
		{
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00005674 File Offset: 0x00003874
		protected void StgInit(object[] parameters)
		{
			this._parameters = parameters;
			this.LoadAndCompileCfgData(false, true);
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00005688 File Offset: 0x00003888
		public static Grammar LoadLocalizedGrammarFromType(Type type, params object[] onInitParameters)
		{
			Helpers.ThrowIfNull(type, "type");
			if (type == typeof(Grammar) || !type.IsSubclassOf(typeof(Grammar)))
			{
				throw new ArgumentException(SR.Get(SRID.StrongTypedGrammarNotAGrammar, new object[0]), "type");
			}
			Assembly assembly = Assembly.GetAssembly(type);
			foreach (Type type2 in assembly.GetTypes())
			{
				string text = null;
				if ((type2 == type || type2.IsSubclassOf(type)) && type2.GetField("__cultureId") != null)
				{
					try
					{
						text = (string)type2.InvokeMember("__cultureId", BindingFlags.GetField, null, null, null, null);
					}
					catch (Exception ex)
					{
						if (!(ex is MissingFieldException))
						{
							throw;
						}
					}
					if (Helpers.CompareInvariantCulture(new CultureInfo(int.Parse(text, CultureInfo.InvariantCulture)), CultureInfo.CurrentUICulture))
					{
						try
						{
							return (Grammar)assembly.CreateInstance(type2.FullName, false, BindingFlags.CreateInstance, null, onInitParameters, null, null);
						}
						catch (MissingMemberException)
						{
							throw new ArgumentException(SR.Get(SRID.RuleScriptInvalidParameters, new object[] { type2.Name, type2.Name }));
						}
					}
				}
			}
			return null;
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x0600014A RID: 330 RVA: 0x000057E0 File Offset: 0x000039E0
		// (set) Token: 0x0600014B RID: 331 RVA: 0x000057E8 File Offset: 0x000039E8
		public bool Enabled
		{
			get
			{
				return this._enabled;
			}
			set
			{
				if (this._grammarState != GrammarState.Unloaded && this._enabled != value)
				{
					this._recognizer.SetGrammarState(this, value);
				}
				this._enabled = value;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x0600014C RID: 332 RVA: 0x0000580F File Offset: 0x00003A0F
		// (set) Token: 0x0600014D RID: 333 RVA: 0x00005818 File Offset: 0x00003A18
		public float Weight
		{
			get
			{
				return this._weight;
			}
			set
			{
				if ((double)value < 0.0 || (double)value > 1.0)
				{
					throw new ArgumentOutOfRangeException("value", SR.Get(SRID.GrammarInvalidWeight, new object[0]));
				}
				if (this._grammarState != GrammarState.Unloaded && !this._weight.Equals(value))
				{
					this._recognizer.SetGrammarWeight(this, value);
				}
				this._weight = value;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x0600014E RID: 334 RVA: 0x00005884 File Offset: 0x00003A84
		// (set) Token: 0x0600014F RID: 335 RVA: 0x0000588C File Offset: 0x00003A8C
		public int Priority
		{
			get
			{
				return this._priority;
			}
			set
			{
				if (value < -128 || value > 127)
				{
					throw new ArgumentOutOfRangeException("value", SR.Get(SRID.GrammarInvalidPriority, new object[0]));
				}
				if (this._grammarState != GrammarState.Unloaded && this._priority != value)
				{
					this._recognizer.SetGrammarPriority(this, value);
				}
				this._priority = value;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000150 RID: 336 RVA: 0x000058E3 File Offset: 0x00003AE3
		// (set) Token: 0x06000151 RID: 337 RVA: 0x000058EB File Offset: 0x00003AEB
		public string Name
		{
			get
			{
				return this._grammarName;
			}
			set
			{
				if (value == null)
				{
					value = string.Empty;
				}
				this._grammarName = value;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000152 RID: 338 RVA: 0x000058FE File Offset: 0x00003AFE
		public string RuleName
		{
			get
			{
				return this._ruleName;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000153 RID: 339 RVA: 0x00005906 File Offset: 0x00003B06
		public bool Loaded
		{
			get
			{
				return this._grammarState == GrammarState.Loaded;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000154 RID: 340 RVA: 0x00005911 File Offset: 0x00003B11
		internal Uri Uri
		{
			get
			{
				return this._uri;
			}
		}

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000155 RID: 341 RVA: 0x0000591C File Offset: 0x00003B1C
		// (remove) Token: 0x06000156 RID: 342 RVA: 0x00005954 File Offset: 0x00003B54
		public event EventHandler<SpeechRecognizedEventArgs> SpeechRecognized;

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000157 RID: 343 RVA: 0x00005989 File Offset: 0x00003B89
		// (set) Token: 0x06000158 RID: 344 RVA: 0x00005991 File Offset: 0x00003B91
		internal IRecognizerInternal Recognizer
		{
			get
			{
				return this._recognizer;
			}
			set
			{
				this._recognizer = value;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000159 RID: 345 RVA: 0x0000599A File Offset: 0x00003B9A
		// (set) Token: 0x0600015A RID: 346 RVA: 0x000059A2 File Offset: 0x00003BA2
		internal GrammarState State
		{
			get
			{
				return this._grammarState;
			}
			set
			{
				if (value == GrammarState.Unloaded)
				{
					this._loadException = null;
					this._recognizer = null;
					if (this._appDomain != null)
					{
						AppDomain.Unload(this._appDomain);
						this._appDomain = null;
					}
				}
				else if (value != GrammarState.Loaded)
				{
				}
				this._grammarState = value;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600015B RID: 347 RVA: 0x000059E0 File Offset: 0x00003BE0
		// (set) Token: 0x0600015C RID: 348 RVA: 0x000059E8 File Offset: 0x00003BE8
		internal Exception LoadException
		{
			get
			{
				return this._loadException;
			}
			set
			{
				this._loadException = value;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600015D RID: 349 RVA: 0x000059F1 File Offset: 0x00003BF1
		internal byte[] CfgData
		{
			get
			{
				return this._cfgData;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600015E RID: 350 RVA: 0x000059F9 File Offset: 0x00003BF9
		internal Uri BaseUri
		{
			get
			{
				return this._baseUri;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600015F RID: 351 RVA: 0x00005A01 File Offset: 0x00003C01
		internal bool Sapi53Only
		{
			get
			{
				return this._sapi53Only;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000161 RID: 353 RVA: 0x00005A12 File Offset: 0x00003C12
		// (set) Token: 0x06000160 RID: 352 RVA: 0x00005A09 File Offset: 0x00003C09
		internal uint SapiGrammarId
		{
			get
			{
				return this._sapiGrammarId;
			}
			set
			{
				this._sapiGrammarId = value;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000162 RID: 354 RVA: 0x00005A1A File Offset: 0x00003C1A
		protected internal virtual bool IsStg
		{
			get
			{
				return this._isStg;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000163 RID: 355 RVA: 0x00005A22 File Offset: 0x00003C22
		internal bool IsSrgsDocument
		{
			get
			{
				return this._isSrgsDocument;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000164 RID: 356 RVA: 0x00005A2A File Offset: 0x00003C2A
		// (set) Token: 0x06000165 RID: 357 RVA: 0x00005A32 File Offset: 0x00003C32
		internal InternalGrammarData InternalData
		{
			get
			{
				return this._internalData;
			}
			set
			{
				this._internalData = value;
			}
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00005A3C File Offset: 0x00003C3C
		internal static Grammar Create(string grammarName, string ruleName, string onInitParameter, out Uri redirectUri)
		{
			redirectUri = null;
			grammarName = grammarName.Trim();
			Uri uri;
			bool flag = Uri.TryCreate(grammarName, UriKind.Absolute, out uri);
			int num = grammarName.IndexOf(".dll", StringComparison.OrdinalIgnoreCase);
			if (!flag || (num > 0 && num == grammarName.Length - 4))
			{
				Assembly assembly;
				if (flag)
				{
					if (!uri.IsFile)
					{
						throw new InvalidOperationException();
					}
					assembly = Assembly.LoadFrom(uri.LocalPath);
				}
				else
				{
					assembly = Assembly.Load(grammarName);
				}
				return Grammar.LoadGrammarFromAssembly(assembly, ruleName, onInitParameter);
			}
			Grammar grammar;
			try
			{
				string text;
				using (Stream stream = Grammar._resourceLoader.LoadFile(uri, out text, out redirectUri))
				{
					try
					{
						grammar = new Grammar(onInitParameter, stream, ruleName);
					}
					finally
					{
						Grammar._resourceLoader.UnloadFile(text);
					}
				}
			}
			catch
			{
				Assembly assembly2 = Assembly.LoadFrom(grammarName);
				grammar = Grammar.LoadGrammarFromAssembly(assembly2, ruleName, onInitParameter);
			}
			return grammar;
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00005B24 File Offset: 0x00003D24
		internal void OnRecognitionInternal(SpeechRecognizedEventArgs eventArgs)
		{
			EventHandler<SpeechRecognizedEventArgs> speechRecognized = this.SpeechRecognized;
			if (speechRecognized != null)
			{
				speechRecognized(this, eventArgs);
			}
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00005B44 File Offset: 0x00003D44
		internal static bool IsDictationGrammar(Uri uri)
		{
			return !(uri == null) && uri.IsAbsoluteUri && !(uri.Scheme != "grammar") && string.IsNullOrEmpty(uri.Host) && string.IsNullOrEmpty(uri.Authority) && string.IsNullOrEmpty(uri.Query) && !(uri.PathAndQuery != "dictation");
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00005BB0 File Offset: 0x00003DB0
		internal bool IsDictation(Uri uri)
		{
			bool flag = Grammar.IsDictationGrammar(uri);
			if (!flag && this is DictationGrammar)
			{
				throw new ArgumentException(SR.Get(SRID.DictationInvalidTopic, new object[0]), "uri");
			}
			return flag;
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00005BEC File Offset: 0x00003DEC
		internal Grammar Find(long grammarId)
		{
			if (this._ruleRefs != null)
			{
				foreach (Grammar grammar in this._ruleRefs)
				{
					if (grammarId == (long)((ulong)grammar._sapiGrammarId))
					{
						return grammar;
					}
					Grammar grammar2;
					if ((grammar2 = grammar.Find(grammarId)) != null)
					{
						return grammar2;
					}
				}
			}
			return null;
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00005C5C File Offset: 0x00003E5C
		internal Grammar Find(string ruleName)
		{
			if (this._ruleRefs != null)
			{
				foreach (Grammar grammar in this._ruleRefs)
				{
					if (ruleName == grammar.RuleName)
					{
						return grammar;
					}
					Grammar grammar2;
					if ((grammar2 = grammar.Find(ruleName)) != null)
					{
						return grammar2;
					}
				}
			}
			return null;
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00005CD0 File Offset: 0x00003ED0
		internal void AddRuleRef(Grammar ruleRef, uint grammarId)
		{
			if (this._ruleRefs == null)
			{
				this._ruleRefs = new Collection<Grammar>();
			}
			this._ruleRefs.Add(ruleRef);
			this._sapiGrammarId = grammarId;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00005CF8 File Offset: 0x00003EF8
		internal MethodInfo MethodInfo(string method)
		{
			return base.GetType().GetMethod(method, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600016E RID: 366 RVA: 0x00005D08 File Offset: 0x00003F08
		// (set) Token: 0x0600016F RID: 367 RVA: 0x00005D10 File Offset: 0x00003F10
		protected string ResourceName
		{
			get
			{
				return this._resources;
			}
			set
			{
				Helpers.ThrowIfEmptyOrNull(value, "value");
				this._resources = value;
			}
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00005D24 File Offset: 0x00003F24
		private void LoadAndCompileCfgData(bool isImportedGrammar, bool stgInit)
		{
			Stream stream = (this.IsStg ? this.LoadCfgFromResource(stgInit) : this.LoadCfg(isImportedGrammar, stgInit));
			SrgsRule[] array = this.RunOnInit(this.IsStg);
			if (array != null)
			{
				MemoryStream memoryStream = Grammar.CombineCfg(this._ruleName, stream, array);
				stream.Close();
				stream = memoryStream;
			}
			this._cfgData = Helpers.ReadStreamToByteArray(stream, (int)stream.Length);
			stream.Close();
			this._srgsDocument = null;
			this._appStream = null;
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00005D98 File Offset: 0x00003F98
		private MemoryStream LoadCfg(bool isImportedGrammar, bool stgInit)
		{
			Uri uri = this.Uri;
			MemoryStream memoryStream = new MemoryStream();
			if (uri != null)
			{
				string text;
				string text2;
				using (Stream stream = Grammar._resourceLoader.LoadFile(uri, out text, out this._baseUri, out text2))
				{
					stream.Position = 0L;
					SrgsGrammarCompiler.CompileXmlOrCopyCfg(stream, memoryStream, uri);
				}
				Grammar._resourceLoader.UnloadFile(text2);
			}
			else if (this._srgsDocument != null)
			{
				SrgsGrammarCompiler.Compile(this._srgsDocument, memoryStream);
				if (this._baseUri == null && this._srgsDocument.BaseUri != null)
				{
					this._baseUri = this._srgsDocument.BaseUri;
				}
			}
			else if (this._grammarBuilder != null)
			{
				this._grammarBuilder.Compile(memoryStream);
			}
			else
			{
				SrgsGrammarCompiler.CompileXmlOrCopyCfg(this._appStream, memoryStream, null);
			}
			memoryStream.Position = 0L;
			this._ruleName = Grammar.CheckRuleName(memoryStream, this._ruleName, isImportedGrammar, stgInit, out this._sapi53Only, out this._semanticTag);
			this.CreateSandbox(memoryStream);
			memoryStream.Position = 0L;
			return memoryStream;
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00005EB4 File Offset: 0x000040B4
		private static Grammar LoadGrammarFromAssembly(Assembly assembly, string ruleName, string onInitParameters)
		{
			Type typeFromHandle = typeof(Grammar);
			Type type = null;
			foreach (Type type2 in assembly.GetTypes())
			{
				if (type2.IsSubclassOf(typeFromHandle))
				{
					string text = null;
					if (type2.Name == ruleName)
					{
						type = type2;
					}
					if ((type2 == type || (type != null && type2.IsSubclassOf(type))) && type2.GetField("__cultureId") != null)
					{
						try
						{
							text = (string)type2.InvokeMember("__cultureId", BindingFlags.GetField, null, null, null, null);
						}
						catch (Exception ex)
						{
							if (!(ex is MissingFieldException))
							{
								throw;
							}
						}
						if (Helpers.CompareInvariantCulture(new CultureInfo(int.Parse(text, CultureInfo.InvariantCulture)), CultureInfo.CurrentUICulture))
						{
							try
							{
								object[] array = Grammar.MatchInitParameters(type2, onInitParameters, assembly.GetName().Name, ruleName);
								return (Grammar)assembly.CreateInstance(type2.FullName, false, BindingFlags.CreateInstance, null, array, null, null);
							}
							catch (MissingMemberException)
							{
								throw new ArgumentException(SR.Get(SRID.RuleScriptInvalidParameters, new object[] { type2.Name, type2.Name }));
							}
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00006014 File Offset: 0x00004214
		private static object[] MatchInitParameters(Type type, string onInitParameters, string grammar, string rule)
		{
			ConstructorInfo[] constructors = type.GetConstructors();
			Grammar.NameValuePair[] array = Grammar.ParseInitParams(onInitParameters);
			object[] array2 = new object[array.Length];
			bool flag = false;
			int num = 0;
			while (num < constructors.Length && !flag)
			{
				ParameterInfo[] parameters = constructors[num].GetParameters();
				if (parameters.Length <= array.Length)
				{
					flag = true;
					int num2 = 0;
					while (num2 < array.Length && flag)
					{
						Grammar.NameValuePair nameValuePair = array[num2];
						if (nameValuePair._name == null)
						{
							array2[num2] = nameValuePair._value;
						}
						else
						{
							bool flag2 = false;
							for (int i = 0; i < parameters.Length; i++)
							{
								if (parameters[i].Name == nameValuePair._name)
								{
									array2[i] = Grammar.ParseValue(parameters[i].ParameterType, nameValuePair._value);
									flag2 = true;
									break;
								}
							}
							if (!flag2)
							{
								flag = false;
							}
						}
						num2++;
					}
				}
				num++;
			}
			if (!flag)
			{
				throw new FormatException(SR.Get(SRID.CantFindAConstructor, new object[]
				{
					grammar,
					rule,
					Grammar.FormatConstructorParameters(constructors)
				}));
			}
			return array2;
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00006124 File Offset: 0x00004324
		private static object ParseValue(Type type, string value)
		{
			if (type == typeof(string))
			{
				return value;
			}
			return type.InvokeMember("Parse", BindingFlags.InvokeMethod, null, null, new object[] { value }, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00006168 File Offset: 0x00004368
		private static string FormatConstructorParameters(ConstructorInfo[] cis)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < cis.Length; i++)
			{
				stringBuilder.Append((i > 0) ? " or sapi:parms=\"" : "sapi:parms=\"");
				ParameterInfo[] parameters = cis[i].GetParameters();
				for (int j = 0; j < parameters.Length; j++)
				{
					if (j > 0)
					{
						stringBuilder.Append(';');
					}
					ParameterInfo parameterInfo = parameters[j];
					stringBuilder.Append(parameterInfo.Name);
					stringBuilder.Append(':');
					stringBuilder.Append(parameterInfo.ParameterType.Name);
				}
				stringBuilder.Append("\"");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00006208 File Offset: 0x00004408
		private static Grammar.NameValuePair[] ParseInitParams(string initParameters)
		{
			if (string.IsNullOrEmpty(initParameters))
			{
				return new Grammar.NameValuePair[0];
			}
			string[] array = initParameters.Split(new char[] { ';' }, StringSplitOptions.None);
			Grammar.NameValuePair[] array2 = new Grammar.NameValuePair[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				int num = text.IndexOf(':');
				if (num >= 0)
				{
					array2[i]._name = text.Substring(0, num);
					array2[i]._value = text.Substring(num + 1);
				}
				else
				{
					array2[i]._value = text;
				}
			}
			return array2;
		}

		// Token: 0x06000177 RID: 375 RVA: 0x0000629C File Offset: 0x0000449C
		private void InitialGrammarLoad(string ruleName, object[] parameters, bool isImportedGrammar)
		{
			this._ruleName = ruleName;
			this._parameters = parameters;
			if (!this.IsDictation(this._uri))
			{
				this.LoadAndCompileCfgData(isImportedGrammar, false);
			}
		}

		// Token: 0x06000178 RID: 376 RVA: 0x000062C4 File Offset: 0x000044C4
		private void CreateSandbox(MemoryStream stream)
		{
			stream.Position = 0L;
			byte[] array;
			byte[] array2;
			ScriptRef[] array3;
			if (CfgGrammar.LoadIL(stream, out array, out array2, out array3))
			{
				Assembly executingAssembly = Assembly.GetExecutingAssembly();
				this._appDomain = AppDomain.CreateDomain("sandbox");
				this._proxy = (AppDomainGrammarProxy)this._appDomain.CreateInstanceFromAndUnwrap(executingAssembly.GetName().CodeBase, "System.Speech.Internal.SrgsCompiler.AppDomainGrammarProxy");
				this._proxy.Init(this._ruleName, array, array2);
				this._scripts = array3;
			}
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00006340 File Offset: 0x00004540
		private Stream LoadCfgFromResource(bool stgInit)
		{
			Assembly assembly = Assembly.GetAssembly(base.GetType());
			Stream manifestResourceStream = assembly.GetManifestResourceStream(this.ResourceName);
			if (manifestResourceStream == null)
			{
				throw new FormatException(SR.Get(SRID.RecognizerInvalidBinaryGrammar, new object[0]));
			}
			try
			{
				ScriptRef[] array = CfgGrammar.LoadIL(manifestResourceStream);
				if (array == null)
				{
					throw new ArgumentException(SR.Get(SRID.CannotLoadDotNetSemanticCode, new object[0]));
				}
				this._scripts = array;
			}
			catch (Exception ex)
			{
				throw new ArgumentException(SR.Get(SRID.CannotLoadDotNetSemanticCode, new object[0]), ex);
			}
			manifestResourceStream.Position = 0L;
			this._ruleName = Grammar.CheckRuleName(manifestResourceStream, base.GetType().Name, false, stgInit, out this._sapi53Only, out this._semanticTag);
			this._isStg = true;
			return manifestResourceStream;
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00006400 File Offset: 0x00004600
		private static MemoryStream CombineCfg(string rule, Stream stream, SrgsRule[] extraRules)
		{
			MemoryStream memoryStream3;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				SrgsDocument srgsDocument = new SrgsDocument();
				srgsDocument.TagFormat = SrgsTagFormat.KeyValuePairs;
				foreach (SrgsRule srgsRule in extraRules)
				{
					srgsDocument.Rules.Add(new SrgsRule[] { srgsRule });
				}
				SrgsGrammarCompiler.Compile(srgsDocument, memoryStream);
				using (StreamMarshaler streamMarshaler = new StreamMarshaler(stream))
				{
					long position = stream.Position;
					Backend backend = new Backend(streamMarshaler);
					stream.Position = position;
					memoryStream.Position = 0L;
					MemoryStream memoryStream2 = new MemoryStream();
					using (StreamMarshaler streamMarshaler2 = new StreamMarshaler(memoryStream))
					{
						Backend backend2 = new Backend(streamMarshaler2);
						Backend backend3 = Backend.CombineGrammar(rule, backend, backend2);
						using (StreamMarshaler streamMarshaler3 = new StreamMarshaler(memoryStream2))
						{
							backend3.Commit(streamMarshaler3);
							memoryStream2.Position = 0L;
							memoryStream3 = memoryStream2;
						}
					}
				}
			}
			return memoryStream3;
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00006524 File Offset: 0x00004724
		private SrgsRule[] RunOnInit(bool stg)
		{
			SrgsRule[] array = null;
			bool flag = false;
			string text = ScriptRef.OnInitMethod(this._scripts, this._ruleName);
			if (text != null)
			{
				if (this._proxy != null)
				{
					Exception ex;
					array = this._proxy.OnInit(text, this._parameters, this._onInitParameters, out ex);
					flag = true;
					if (ex != null)
					{
						throw ex;
					}
				}
				else
				{
					Type[] array2 = new Type[this._parameters.Length];
					for (int i = 0; i < this._parameters.Length; i++)
					{
						array2[i] = this._parameters[i].GetType();
					}
					MethodInfo method = base.GetType().GetMethod(text, array2);
					if (!(method != null))
					{
						throw new ArgumentException(SR.Get(SRID.RuleScriptInvalidParameters, new object[] { this._ruleName, this._ruleName }));
					}
					array = (SrgsRule[])method.Invoke(this, this._parameters);
					flag = true;
				}
			}
			if (!stg && !flag && this._parameters != null)
			{
				throw new ArgumentException(SR.Get(SRID.RuleScriptInvalidParameters, new object[] { this._ruleName, this._ruleName }));
			}
			return array;
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00006640 File Offset: 0x00004840
		private static string CheckRuleName(Stream stream, string rulename, bool isImportedGrammar, bool stgInit, out bool sapi53Only, out GrammarOptions grammarOptions)
		{
			sapi53Only = false;
			long position = stream.Position;
			using (StreamMarshaler streamMarshaler = new StreamMarshaler(stream))
			{
				CfgGrammar.CfgSerializedHeader cfgSerializedHeader = null;
				CfgGrammar.CfgHeader cfgHeader = CfgGrammar.ConvertCfgHeader(streamMarshaler, false, true, out cfgSerializedHeader);
				StringBlob pszSymbols = cfgHeader.pszSymbols;
				string text = ((cfgHeader.ulRootRuleIndex != uint.MaxValue && (ulong)cfgHeader.ulRootRuleIndex < (ulong)((long)cfgHeader.rules.Length)) ? pszSymbols.FromOffset(cfgHeader.rules[(int)cfgHeader.ulRootRuleIndex]._nameOffset) : null);
				sapi53Only = (cfgHeader.GrammarOptions & (GrammarOptions.MssV1 | GrammarOptions.IpaPhoneme | GrammarOptions.W3cV1 | GrammarOptions.STG)) > GrammarOptions.KeyValuePairs;
				if (text == null && string.IsNullOrEmpty(rulename))
				{
					throw new ArgumentException(SR.Get(SRID.SapiErrorNoRulesToActivate, new object[0]));
				}
				if (!string.IsNullOrEmpty(rulename))
				{
					bool flag = false;
					foreach (CfgRule cfgRule in cfgHeader.rules)
					{
						if (pszSymbols.FromOffset(cfgRule._nameOffset) == rulename)
						{
							flag = cfgRule.Export || stgInit || (!isImportedGrammar && (cfgRule.TopLevel || rulename == text));
							break;
						}
					}
					if (!flag)
					{
						throw new ArgumentException(SR.Get(SRID.RecognizerRuleNotFoundStream, new object[] { rulename }));
					}
				}
				else
				{
					rulename = text;
				}
				grammarOptions = cfgHeader.GrammarOptions & GrammarOptions.TagFormat;
			}
			stream.Position = position;
			return rulename;
		}

		// Token: 0x040002CE RID: 718
		internal GrammarOptions _semanticTag;

		// Token: 0x040002CF RID: 719
		internal AppDomain _appDomain;

		// Token: 0x040002D0 RID: 720
		internal AppDomainGrammarProxy _proxy;

		// Token: 0x040002D1 RID: 721
		internal ScriptRef[] _scripts;

		// Token: 0x040002D2 RID: 722
		private byte[] _cfgData;

		// Token: 0x040002D3 RID: 723
		private Stream _appStream;

		// Token: 0x040002D4 RID: 724
		private bool _isSrgsDocument;

		// Token: 0x040002D5 RID: 725
		private SrgsDocument _srgsDocument;

		// Token: 0x040002D6 RID: 726
		private GrammarBuilder _grammarBuilder;

		// Token: 0x040002D7 RID: 727
		private IRecognizerInternal _recognizer;

		// Token: 0x040002D8 RID: 728
		private GrammarState _grammarState;

		// Token: 0x040002D9 RID: 729
		private Exception _loadException;

		// Token: 0x040002DA RID: 730
		private Uri _uri;

		// Token: 0x040002DB RID: 731
		private Uri _baseUri;

		// Token: 0x040002DC RID: 732
		private string _ruleName;

		// Token: 0x040002DD RID: 733
		private string _resources;

		// Token: 0x040002DE RID: 734
		private object[] _parameters;

		// Token: 0x040002DF RID: 735
		private string _onInitParameters;

		// Token: 0x040002E0 RID: 736
		private bool _enabled = true;

		// Token: 0x040002E1 RID: 737
		private bool _isStg;

		// Token: 0x040002E2 RID: 738
		private bool _sapi53Only;

		// Token: 0x040002E3 RID: 739
		private uint _sapiGrammarId;

		// Token: 0x040002E4 RID: 740
		private float _weight = 1f;

		// Token: 0x040002E5 RID: 741
		private int _priority;

		// Token: 0x040002E6 RID: 742
		private InternalGrammarData _internalData;

		// Token: 0x040002E7 RID: 743
		private string _grammarName = string.Empty;

		// Token: 0x040002E8 RID: 744
		private Collection<Grammar> _ruleRefs;

		// Token: 0x040002E9 RID: 745
		private static ResourceLoader _resourceLoader = new ResourceLoader();

		// Token: 0x02000176 RID: 374
		private struct NameValuePair
		{
			// Token: 0x0400089A RID: 2202
			internal string _name;

			// Token: 0x0400089B RID: 2203
			internal string _value;
		}
	}
}
