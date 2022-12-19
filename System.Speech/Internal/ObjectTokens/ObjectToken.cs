using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Speech.Internal.SapiInterop;
using System.Text;

namespace System.Speech.Internal.ObjectTokens
{
	// Token: 0x02000024 RID: 36
	internal class ObjectToken : RegistryDataKey, ISpObjectToken, ISpDataKey
	{
		// Token: 0x060000FA RID: 250 RVA: 0x00007178 File Offset: 0x00006178
		protected ObjectToken(ISpObjectToken sapiObjectToken, bool disposeSapiToken)
			: base(sapiObjectToken)
		{
			if (sapiObjectToken == null)
			{
				throw new ArgumentNullException("sapiObjectToken");
			}
			this._sapiObjectToken = sapiObjectToken;
			this._disposeSapiObjectToken = disposeSapiToken;
		}

		// Token: 0x060000FB RID: 251 RVA: 0x000071A0 File Offset: 0x000061A0
		internal static ObjectToken FindBestMatch(string categoryId, string requiredAttributes, string optionalAttributes)
		{
			if (string.IsNullOrEmpty(categoryId))
			{
				throw new ArgumentNullException("categoryId");
			}
			ObjectToken objectToken;
			using (ObjectTokenCategory objectTokenCategory = ObjectTokenCategory.Create(categoryId))
			{
				if (objectTokenCategory != null)
				{
					StringBuilder stringBuilder = new StringBuilder(optionalAttributes);
					if (!string.IsNullOrEmpty(optionalAttributes))
					{
						stringBuilder.Append(";");
					}
					stringBuilder.Append("VendorPreferred");
					IList<ObjectToken> list = objectTokenCategory.FindMatchingTokens(requiredAttributes, stringBuilder.ToString());
					objectToken = ((list.Count > 0) ? list[0] : null);
				}
				else
				{
					objectToken = null;
				}
			}
			return objectToken;
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00007234 File Offset: 0x00006234
		internal static ObjectToken Open(ISpObjectToken sapiObjectToken)
		{
			return new ObjectToken(sapiObjectToken, false);
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00007240 File Offset: 0x00006240
		internal static ObjectToken Open(string sCategoryId, string sTokenId, bool fCreateIfNotExist)
		{
			ISpObjectToken spObjectToken = (ISpObjectToken)new SpObjectToken();
			try
			{
				spObjectToken.SetId(sCategoryId, sTokenId, fCreateIfNotExist);
			}
			catch (Exception)
			{
				Marshal.ReleaseComObject(spObjectToken);
				return null;
			}
			return new ObjectToken(spObjectToken, true);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00007288 File Offset: 0x00006288
		internal static ObjectToken Create(string sCategoryId, string sTokenId)
		{
			return ObjectToken.Open(sCategoryId, sTokenId, true);
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00007294 File Offset: 0x00006294
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					if (this._disposeSapiObjectToken && this._sapiObjectToken != null)
					{
						Marshal.ReleaseComObject(this._sapiObjectToken);
						this._sapiObjectToken = null;
					}
					if (this._attributes != null)
					{
						this._attributes.Dispose();
						this._attributes = null;
					}
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x06000100 RID: 256 RVA: 0x000072FC File Offset: 0x000062FC
		public override bool Equals(object obj)
		{
			ObjectToken objectToken = obj as ObjectToken;
			return objectToken != null && string.Compare(base.Id, objectToken.Id, 5) == 0;
		}

		// Token: 0x06000101 RID: 257 RVA: 0x0000732A File Offset: 0x0000632A
		public override int GetHashCode()
		{
			return base.Id.GetHashCode();
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000102 RID: 258 RVA: 0x00007338 File Offset: 0x00006338
		internal RegistryDataKey Attributes
		{
			get
			{
				if (this._attributes == null)
				{
					return this._attributes = base.OpenKey("Attributes");
				}
				return this._attributes;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000103 RID: 259 RVA: 0x00007368 File Offset: 0x00006368
		internal ISpObjectToken SAPIToken
		{
			get
			{
				return this._sapiObjectToken;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000104 RID: 260 RVA: 0x00007370 File Offset: 0x00006370
		internal string Age
		{
			get
			{
				string empty;
				if (this.Attributes == null || !this.Attributes.TryGetString("Age", out empty))
				{
					empty = string.Empty;
				}
				return empty;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000105 RID: 261 RVA: 0x000073A0 File Offset: 0x000063A0
		internal string Gender
		{
			get
			{
				string empty;
				if (this.Attributes == null || !this.Attributes.TryGetString("Gender", out empty))
				{
					empty = string.Empty;
				}
				return empty;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000107 RID: 263 RVA: 0x000073D9 File Offset: 0x000063D9
		// (set) Token: 0x06000106 RID: 262 RVA: 0x000073D0 File Offset: 0x000063D0
		internal VoiceCategory VoiceCategory
		{
			get
			{
				return this._category;
			}
			set
			{
				this._category = value;
			}
		}

		// Token: 0x06000108 RID: 264 RVA: 0x000073E4 File Offset: 0x000063E4
		internal string TokenName()
		{
			string empty = string.Empty;
			if (this.Attributes != null)
			{
				this.Attributes.TryGetString("Name", out empty);
				if (string.IsNullOrEmpty(empty))
				{
					base.TryGetString(null, out empty);
				}
			}
			return empty;
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000109 RID: 265 RVA: 0x00007428 File Offset: 0x00006428
		internal CultureInfo Culture
		{
			get
			{
				CultureInfo cultureInfo = null;
				string text;
				if (this.Attributes.TryGetString("Language", out text))
				{
					cultureInfo = SapiAttributeParser.GetCultureInfoFromLanguageString(text);
				}
				return cultureInfo;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600010A RID: 266 RVA: 0x00007454 File Offset: 0x00006454
		internal string Description
		{
			get
			{
				string empty = string.Empty;
				string text = string.Format(CultureInfo.InvariantCulture, "{0:x}", new object[] { CultureInfo.CurrentUICulture.LCID });
				if (!base.TryGetString(text, out empty))
				{
					base.TryGetString(null, out empty);
				}
				return empty;
			}
		}

		// Token: 0x0600010B RID: 267 RVA: 0x000074A7 File Offset: 0x000064A7
		public void SetId([MarshalAs(21)] string pszCategoryId, [MarshalAs(21)] string pszTokenId, [MarshalAs(2)] bool fCreateIfNotExist)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600010C RID: 268 RVA: 0x000074AE File Offset: 0x000064AE
		public void GetId([MarshalAs(21)] out IntPtr ppszCoMemTokenId)
		{
			ppszCoMemTokenId = Marshal.StringToCoTaskMemUni(base.Id);
		}

		// Token: 0x0600010D RID: 269 RVA: 0x000074C1 File Offset: 0x000064C1
		public void Slot15()
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600010E RID: 270 RVA: 0x000074C8 File Offset: 0x000064C8
		public void Slot16()
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600010F RID: 271 RVA: 0x000074CF File Offset: 0x000064CF
		public void Slot17()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000110 RID: 272 RVA: 0x000074D6 File Offset: 0x000064D6
		public void Slot18()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000111 RID: 273 RVA: 0x000074DD File Offset: 0x000064DD
		public void Slot19()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000112 RID: 274 RVA: 0x000074E4 File Offset: 0x000064E4
		public void Slot20()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000113 RID: 275 RVA: 0x000074EB File Offset: 0x000064EB
		public void Slot21()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000114 RID: 276 RVA: 0x000074F2 File Offset: 0x000064F2
		public void MatchesAttributes([MarshalAs(21)] string pszAttributes, [MarshalAs(2)] out bool pfMatches)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000115 RID: 277 RVA: 0x000074FC File Offset: 0x000064FC
		internal bool MatchesAttributes(string[] sAttributes)
		{
			bool flag = true;
			foreach (string text in sAttributes)
			{
				flag &= base.HasValue(text) || (this.Attributes != null && this.Attributes.HasValue(text));
				if (!flag)
				{
					break;
				}
			}
			return flag;
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00007548 File Offset: 0x00006548
		internal T CreateObjectFromToken<T>(string name)
		{
			T t = default(T);
			string text;
			if (!base.TryGetString(name, out text))
			{
				throw new ArgumentException(SR.Get(SRID.TokenCannotCreateInstance, new object[0]));
			}
			try
			{
				Type typeFromCLSID = Type.GetTypeFromCLSID(new Guid(text));
				t = (T)((object)Activator.CreateInstance(typeFromCLSID));
				ObjectToken.ISpObjectWithToken spObjectWithToken = t as ObjectToken.ISpObjectWithToken;
				if (spObjectWithToken != null)
				{
					int num = spObjectWithToken.SetObjectToken(this);
					if (num < 0)
					{
						throw new ArgumentException(SR.Get(SRID.TokenCannotCreateInstance, new object[0]));
					}
				}
			}
			catch (Exception ex)
			{
				if (ex is MissingMethodException || ex is TypeLoadException || ex is FileLoadException || ex is FileNotFoundException || ex is MethodAccessException || ex is MemberAccessException || ex is TargetInvocationException || ex is InvalidComObjectException || ex is NotSupportedException || ex is FormatException)
				{
					throw new ArgumentException(SR.Get(SRID.TokenCannotCreateInstance, new object[0]));
				}
				throw;
			}
			return t;
		}

		// Token: 0x040000C8 RID: 200
		private const string sGenerateFileNameSpecifier = "{0}";

		// Token: 0x040000C9 RID: 201
		private const string SPTOKENVALUE_CLSID = "CLSID";

		// Token: 0x040000CA RID: 202
		private ISpObjectToken _sapiObjectToken;

		// Token: 0x040000CB RID: 203
		private bool _disposeSapiObjectToken;

		// Token: 0x040000CC RID: 204
		private VoiceCategory _category;

		// Token: 0x040000CD RID: 205
		private RegistryDataKey _attributes;

		// Token: 0x02000025 RID: 37
		[Guid("5B559F40-E952-11D2-BB91-00C04F8EE6C0")]
		[InterfaceType(1)]
		[ComImport]
		private interface ISpObjectWithToken
		{
			// Token: 0x06000117 RID: 279
			[PreserveSig]
			int SetObjectToken(ISpObjectToken pToken);

			// Token: 0x06000118 RID: 280
			[PreserveSig]
			int GetObjectToken(IntPtr ppToken);
		}
	}
}
