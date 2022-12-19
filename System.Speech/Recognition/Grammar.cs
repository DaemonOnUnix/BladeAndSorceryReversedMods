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
	// Token: 0x0200010A RID: 266
	[DebuggerDisplay("Grammar: {(_uri != null ? \"uri=\" + _uri.ToString () + \" \" : \"\") + \"rule=\" + _ruleName }")]
	public class Grammar
	{
		// Token: 0x0600068C RID: 1676 RVA: 0x0001E74C File Offset: 0x0001D74C
		internal Grammar(Uri uri, string ruleName, object[] parameters)
		{
			Helpers.ThrowIfNull(uri, "uri");
			this._uri = uri;
			this.InitialGrammarLoad(ruleName, parameters, false);
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x0001E78C File Offset: 0x0001D78C
		public Grammar(string path)
			: this(path, null, null)
		{
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x0001E797 File Offset: 0x0001D797
		public Grammar(string path, string ruleName)
			: this(path, ruleName, null)
		{
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x0001E7A4 File Offset: 0x0001D7A4
		public Grammar(string path, string ruleName, object[] parameters)
		{
			try
			{
				this._uri = new Uri(path, 2);
			}
			catch (UriFormatException ex)
			{
				throw new ArgumentException(SR.Get(SRID.RecognizerGrammarNotFound, new object[0]), "path", ex);
			}
			this.InitialGrammarLoad(ruleName, parameters, false);
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x0001E81C File Offset: 0x0001D81C
		public Grammar(SrgsDocument srgsDocument)
			: this(srgsDocument, null, null, null)
		{
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x0001E828 File Offset: 0x0001D828
		public Grammar(SrgsDocument srgsDocument, string ruleName)
			: this(srgsDocument, ruleName, null, null)
		{
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x0001E834 File Offset: 0x0001D834
		public Grammar(SrgsDocument srgsDocument, string ruleName, object[] parameters)
			: this(srgsDocument, ruleName, null, parameters)
		{
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x0001E840 File Offset: 0x0001D840
		[EditorBrowsable(2)]
		public Grammar(SrgsDocument srgsDocument, string ruleName, Uri baseUri)
			: this(srgsDocument, ruleName, baseUri, null)
		{
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x0001E84C File Offset: 0x0001D84C
		[EditorBrowsable(2)]
		public Grammar(SrgsDocument srgsDocument, string ruleName, Uri baseUri, object[] parameters)
		{
			Helpers.ThrowIfNull(srgsDocument, "srgsDocument");
			this._srgsDocument = srgsDocument;
			this._isSrgsDocument = srgsDocument != null;
			this._baseUri = baseUri;
			this.InitialGrammarLoad(ruleName, parameters, false);
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x0001E8AC File Offset: 0x0001D8AC
		public Grammar(Stream stream)
			: this(stream, null, null, null)
		{
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x0001E8B8 File Offset: 0x0001D8B8
		public Grammar(Stream stream, string ruleName)
			: this(stream, ruleName, null, null)
		{
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x0001E8C4 File Offset: 0x0001D8C4
		public Grammar(Stream stream, string ruleName, object[] parameters)
			: this(stream, ruleName, null, parameters)
		{
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x0001E8D0 File Offset: 0x0001D8D0
		[EditorBrowsable(2)]
		public Grammar(Stream stream, string ruleName, Uri baseUri)
			: this(stream, ruleName, baseUri, null)
		{
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x0001E8DC File Offset: 0x0001D8DC
		[EditorBrowsable(2)]
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

		// Token: 0x0600069A RID: 1690 RVA: 0x0001E94F File Offset: 0x0001D94F
		public Grammar(GrammarBuilder builder)
		{
			Helpers.ThrowIfNull(builder, "builder");
			this._grammarBuilder = builder;
			this.InitialGrammarLoad(null, null, false);
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x0001E98F File Offset: 0x0001D98F
		private Grammar(string onInitParameters, Stream stream, string ruleName)
		{
			this._appStream = stream;
			this._onInitParameters = onInitParameters;
			this.InitialGrammarLoad(ruleName, null, true);
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x0001E9CB File Offset: 0x0001D9CB
		protected Grammar()
		{
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x0001E9F0 File Offset: 0x0001D9F0
		protected void StgInit(object[] parameters)
		{
			this._parameters = parameters;
			this.LoadAndCompileCfgData(false, true);
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x0001EA04 File Offset: 0x0001DA04
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
						text = (string)type2.InvokeMember("__cultureId", 1024, null, null, null, null);
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
							return (Grammar)assembly.CreateInstance(type2.FullName, false, 512, null, onInitParameters, null, null);
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

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x0600069F RID: 1695 RVA: 0x0001EB54 File Offset: 0x0001DB54
		// (set) Token: 0x060006A0 RID: 1696 RVA: 0x0001EB5C File Offset: 0x0001DB5C
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

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060006A1 RID: 1697 RVA: 0x0001EB83 File Offset: 0x0001DB83
		// (set) Token: 0x060006A2 RID: 1698 RVA: 0x0001EB8C File Offset: 0x0001DB8C
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

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060006A3 RID: 1699 RVA: 0x0001EBF8 File Offset: 0x0001DBF8
		// (set) Token: 0x060006A4 RID: 1700 RVA: 0x0001EC00 File Offset: 0x0001DC00
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

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060006A5 RID: 1701 RVA: 0x0001EC57 File Offset: 0x0001DC57
		// (set) Token: 0x060006A6 RID: 1702 RVA: 0x0001EC5F File Offset: 0x0001DC5F
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

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060006A7 RID: 1703 RVA: 0x0001EC72 File Offset: 0x0001DC72
		public string RuleName
		{
			get
			{
				return this._ruleName;
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060006A8 RID: 1704 RVA: 0x0001EC7A File Offset: 0x0001DC7A
		public bool Loaded
		{
			get
			{
				return this._grammarState == GrammarState.Loaded;
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060006A9 RID: 1705 RVA: 0x0001EC85 File Offset: 0x0001DC85
		internal Uri Uri
		{
			get
			{
				return this._uri;
			}
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060006AA RID: 1706 RVA: 0x0001EC8D File Offset: 0x0001DC8D
		// (remove) Token: 0x060006AB RID: 1707 RVA: 0x0001ECA6 File Offset: 0x0001DCA6
		public event EventHandler<SpeechRecognizedEventArgs> SpeechRecognized;

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060006AC RID: 1708 RVA: 0x0001ECBF File Offset: 0x0001DCBF
		// (set) Token: 0x060006AD RID: 1709 RVA: 0x0001ECC7 File Offset: 0x0001DCC7
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

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060006AE RID: 1710 RVA: 0x0001ECD0 File Offset: 0x0001DCD0
		// (set) Token: 0x060006AF RID: 1711 RVA: 0x0001ECD8 File Offset: 0x0001DCD8
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

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060006B0 RID: 1712 RVA: 0x0001ED16 File Offset: 0x0001DD16
		// (set) Token: 0x060006B1 RID: 1713 RVA: 0x0001ED1E File Offset: 0x0001DD1E
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

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060006B2 RID: 1714 RVA: 0x0001ED27 File Offset: 0x0001DD27
		internal byte[] CfgData
		{
			get
			{
				return this._cfgData;
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060006B3 RID: 1715 RVA: 0x0001ED2F File Offset: 0x0001DD2F
		internal Uri BaseUri
		{
			get
			{
				return this._baseUri;
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060006B4 RID: 1716 RVA: 0x0001ED37 File Offset: 0x0001DD37
		internal bool Sapi53Only
		{
			get
			{
				return this._sapi53Only;
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060006B6 RID: 1718 RVA: 0x0001ED48 File Offset: 0x0001DD48
		// (set) Token: 0x060006B5 RID: 1717 RVA: 0x0001ED3F File Offset: 0x0001DD3F
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

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060006B7 RID: 1719 RVA: 0x0001ED50 File Offset: 0x0001DD50
		protected internal virtual bool IsStg
		{
			get
			{
				return this._isStg;
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060006B8 RID: 1720 RVA: 0x0001ED58 File Offset: 0x0001DD58
		internal bool IsSrgsDocument
		{
			get
			{
				return this._isSrgsDocument;
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060006B9 RID: 1721 RVA: 0x0001ED60 File Offset: 0x0001DD60
		// (set) Token: 0x060006BA RID: 1722 RVA: 0x0001ED68 File Offset: 0x0001DD68
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

		// Token: 0x060006BB RID: 1723 RVA: 0x0001ED74 File Offset: 0x0001DD74
		internal static Grammar Create(string grammarName, string ruleName, string onInitParameter, out Uri redirectUri)
		{
			redirectUri = null;
			grammarName = grammarName.Trim();
			Uri uri;
			bool flag = Uri.TryCreate(grammarName, 1, ref uri);
			int num = grammarName.IndexOf(".dll", 5);
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

		// Token: 0x060006BC RID: 1724 RVA: 0x0001EE5C File Offset: 0x0001DE5C
		internal void OnRecognitionInternal(SpeechRecognizedEventArgs eventArgs)
		{
			EventHandler<SpeechRecognizedEventArgs> speechRecognized = this.SpeechRecognized;
			if (speechRecognized != null)
			{
				speechRecognized.Invoke(this, eventArgs);
			}
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x0001EE7C File Offset: 0x0001DE7C
		internal static bool IsDictationGrammar(Uri uri)
		{
			return !(uri == null) && uri.IsAbsoluteUri && !(uri.Scheme != "grammar") && string.IsNullOrEmpty(uri.Host) && string.IsNullOrEmpty(uri.Authority) && string.IsNullOrEmpty(uri.Query) && !(uri.PathAndQuery != "dictation");
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x0001EEE8 File Offset: 0x0001DEE8
		internal bool IsDictation(Uri uri)
		{
			bool flag = Grammar.IsDictationGrammar(uri);
			if (!flag && this is DictationGrammar)
			{
				throw new ArgumentException(SR.Get(SRID.DictationInvalidTopic, new object[0]), "uri");
			}
			return flag;
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x0001EF24 File Offset: 0x0001DF24
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

		// Token: 0x060006C0 RID: 1728 RVA: 0x0001EF94 File Offset: 0x0001DF94
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

		// Token: 0x060006C1 RID: 1729 RVA: 0x0001F008 File Offset: 0x0001E008
		internal void AddRuleRef(Grammar ruleRef, uint grammarId)
		{
			if (this._ruleRefs == null)
			{
				this._ruleRefs = new Collection<Grammar>();
			}
			this._ruleRefs.Add(ruleRef);
			this._sapiGrammarId = grammarId;
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x0001F030 File Offset: 0x0001E030
		internal MethodInfo MethodInfo(string method)
		{
			return base.GetType().GetMethod(method, 52);
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060006C3 RID: 1731 RVA: 0x0001F040 File Offset: 0x0001E040
		// (set) Token: 0x060006C4 RID: 1732 RVA: 0x0001F048 File Offset: 0x0001E048
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

		// Token: 0x060006C5 RID: 1733 RVA: 0x0001F05C File Offset: 0x0001E05C
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

		// Token: 0x060006C6 RID: 1734 RVA: 0x0001F0D0 File Offset: 0x0001E0D0
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

		// Token: 0x060006C7 RID: 1735 RVA: 0x0001F1EC File Offset: 0x0001E1EC
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
							text = (string)type2.InvokeMember("__cultureId", 1024, null, null, null, null);
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
								return (Grammar)assembly.CreateInstance(type2.FullName, false, 512, null, array, null, null);
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

		// Token: 0x060006C8 RID: 1736 RVA: 0x0001F33C File Offset: 0x0001E33C
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

		// Token: 0x060006C9 RID: 1737 RVA: 0x0001F458 File Offset: 0x0001E458
		private static object ParseValue(Type type, string value)
		{
			if (type == typeof(string))
			{
				return value;
			}
			return type.InvokeMember("Parse", 256, null, null, new object[] { value }, CultureInfo.InvariantCulture);
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x0001F498 File Offset: 0x0001E498
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

		// Token: 0x060006CB RID: 1739 RVA: 0x0001F538 File Offset: 0x0001E538
		private static Grammar.NameValuePair[] ParseInitParams(string initParameters)
		{
			if (string.IsNullOrEmpty(initParameters))
			{
				return new Grammar.NameValuePair[0];
			}
			string[] array = initParameters.Split(new char[] { ';' }, 0);
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

		// Token: 0x060006CC RID: 1740 RVA: 0x0001F5D1 File Offset: 0x0001E5D1
		private void InitialGrammarLoad(string ruleName, object[] parameters, bool isImportedGrammar)
		{
			this._ruleName = ruleName;
			this._parameters = parameters;
			if (!this.IsDictation(this._uri))
			{
				this.LoadAndCompileCfgData(isImportedGrammar, false);
			}
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x0001F5F8 File Offset: 0x0001E5F8
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

		// Token: 0x060006CE RID: 1742 RVA: 0x0001F674 File Offset: 0x0001E674
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

		// Token: 0x060006CF RID: 1743 RVA: 0x0001F734 File Offset: 0x0001E734
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

		// Token: 0x060006D0 RID: 1744 RVA: 0x0001F860 File Offset: 0x0001E860
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
					if (method == null)
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

		// Token: 0x060006D1 RID: 1745 RVA: 0x0001F980 File Offset: 0x0001E980
		private static string CheckRuleName(Stream stream, string rulename, bool isImportedGrammar, bool stgInit, out bool sapi53Only, out GrammarOptions grammarOptions)
		{
			sapi53Only = false;
			long position = stream.Position;
			using (StreamMarshaler streamMarshaler = new StreamMarshaler(stream))
			{
				CfgGrammar.CfgSerializedHeader cfgSerializedHeader = null;
				CfgGrammar.CfgHeader cfgHeader = CfgGrammar.ConvertCfgHeader(streamMarshaler, false, true, out cfgSerializedHeader);
				StringBlob pszSymbols = cfgHeader.pszSymbols;
				string text = ((cfgHeader.ulRootRuleIndex != uint.MaxValue && (ulong)cfgHeader.ulRootRuleIndex < (ulong)((long)cfgHeader.rules.Length)) ? pszSymbols.FromOffset(cfgHeader.rules[(int)((UIntPtr)cfgHeader.ulRootRuleIndex)]._nameOffset) : null);
				sapi53Only = (cfgHeader.GrammarOptions & (GrammarOptions.MssV1 | GrammarOptions.IpaPhoneme | GrammarOptions.W3cV1 | GrammarOptions.STG)) != GrammarOptions.KeyValuePairs;
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

		// Token: 0x0400051C RID: 1308
		internal GrammarOptions _semanticTag;

		// Token: 0x0400051D RID: 1309
		internal AppDomain _appDomain;

		// Token: 0x0400051E RID: 1310
		internal AppDomainGrammarProxy _proxy;

		// Token: 0x0400051F RID: 1311
		internal ScriptRef[] _scripts;

		// Token: 0x04000520 RID: 1312
		private byte[] _cfgData;

		// Token: 0x04000521 RID: 1313
		private Stream _appStream;

		// Token: 0x04000522 RID: 1314
		private bool _isSrgsDocument;

		// Token: 0x04000523 RID: 1315
		private SrgsDocument _srgsDocument;

		// Token: 0x04000524 RID: 1316
		private GrammarBuilder _grammarBuilder;

		// Token: 0x04000525 RID: 1317
		private IRecognizerInternal _recognizer;

		// Token: 0x04000526 RID: 1318
		private GrammarState _grammarState;

		// Token: 0x04000527 RID: 1319
		private Exception _loadException;

		// Token: 0x04000528 RID: 1320
		private Uri _uri;

		// Token: 0x04000529 RID: 1321
		private Uri _baseUri;

		// Token: 0x0400052A RID: 1322
		private string _ruleName;

		// Token: 0x0400052B RID: 1323
		private string _resources;

		// Token: 0x0400052C RID: 1324
		private object[] _parameters;

		// Token: 0x0400052D RID: 1325
		private string _onInitParameters;

		// Token: 0x0400052E RID: 1326
		private bool _enabled = true;

		// Token: 0x0400052F RID: 1327
		private bool _isStg;

		// Token: 0x04000530 RID: 1328
		private bool _sapi53Only;

		// Token: 0x04000531 RID: 1329
		private uint _sapiGrammarId;

		// Token: 0x04000532 RID: 1330
		private float _weight = 1f;

		// Token: 0x04000533 RID: 1331
		private int _priority;

		// Token: 0x04000534 RID: 1332
		private InternalGrammarData _internalData;

		// Token: 0x04000535 RID: 1333
		private string _grammarName = string.Empty;

		// Token: 0x04000536 RID: 1334
		private Collection<Grammar> _ruleRefs;

		// Token: 0x04000537 RID: 1335
		private static ResourceLoader _resourceLoader = new ResourceLoader();

		// Token: 0x0200010B RID: 267
		private struct NameValuePair
		{
			// Token: 0x04000538 RID: 1336
			internal string _name;

			// Token: 0x04000539 RID: 1337
			internal string _value;
		}
	}
}
