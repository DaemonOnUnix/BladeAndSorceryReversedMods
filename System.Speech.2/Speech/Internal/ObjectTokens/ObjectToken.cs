using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Speech.Internal.SapiInterop;

namespace System.Speech.Internal.ObjectTokens
{
	// Token: 0x0200016B RID: 363
	internal class ObjectToken : RegistryDataKey, ISpObjectToken, ISpDataKey
	{
		// Token: 0x06000AEB RID: 2795 RVA: 0x0002C239 File Offset: 0x0002A439
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

		// Token: 0x06000AEC RID: 2796 RVA: 0x0002C25E File Offset: 0x0002A45E
		internal static ObjectToken Open(ISpObjectToken sapiObjectToken)
		{
			return new ObjectToken(sapiObjectToken, false);
		}

		// Token: 0x06000AED RID: 2797 RVA: 0x0002C268 File Offset: 0x0002A468
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

		// Token: 0x06000AEE RID: 2798 RVA: 0x0002C2B0 File Offset: 0x0002A4B0
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

		// Token: 0x06000AEF RID: 2799 RVA: 0x0002C318 File Offset: 0x0002A518
		public override bool Equals(object obj)
		{
			ObjectToken objectToken = obj as ObjectToken;
			return objectToken != null && string.Compare(base.Id, objectToken.Id, StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x06000AF0 RID: 2800 RVA: 0x0002C346 File Offset: 0x0002A546
		public override int GetHashCode()
		{
			return base.Id.GetHashCode();
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06000AF1 RID: 2801 RVA: 0x0002C354 File Offset: 0x0002A554
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

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000AF2 RID: 2802 RVA: 0x0002C384 File Offset: 0x0002A584
		internal ISpObjectToken SAPIToken
		{
			get
			{
				return this._sapiObjectToken;
			}
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000AF3 RID: 2803 RVA: 0x0002C38C File Offset: 0x0002A58C
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

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000AF4 RID: 2804 RVA: 0x0002C3BC File Offset: 0x0002A5BC
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

		// Token: 0x06000AF5 RID: 2805 RVA: 0x0002C3EC File Offset: 0x0002A5EC
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

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000AF6 RID: 2806 RVA: 0x0002C430 File Offset: 0x0002A630
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

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000AF7 RID: 2807 RVA: 0x0002C45C File Offset: 0x0002A65C
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

		// Token: 0x06000AF8 RID: 2808 RVA: 0x00027EC5 File Offset: 0x000260C5
		public void SetId([MarshalAs(UnmanagedType.LPWStr)] string pszCategoryId, [MarshalAs(UnmanagedType.LPWStr)] string pszTokenId, [MarshalAs(UnmanagedType.Bool)] bool fCreateIfNotExist)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x0002C4AD File Offset: 0x0002A6AD
		public void GetId([MarshalAs(UnmanagedType.LPWStr)] out IntPtr ppszCoMemTokenId)
		{
			ppszCoMemTokenId = Marshal.StringToCoTaskMemUni(base.Id);
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x00027EC5 File Offset: 0x000260C5
		public void Slot15()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x00027EC5 File Offset: 0x000260C5
		public void Slot16()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000AFC RID: 2812 RVA: 0x00027EC5 File Offset: 0x000260C5
		public void Slot17()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x00027EC5 File Offset: 0x000260C5
		public void Slot18()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000AFE RID: 2814 RVA: 0x00027EC5 File Offset: 0x000260C5
		public void Slot19()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000AFF RID: 2815 RVA: 0x00027EC5 File Offset: 0x000260C5
		public void Slot20()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000B00 RID: 2816 RVA: 0x00027EC5 File Offset: 0x000260C5
		public void Slot21()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000B01 RID: 2817 RVA: 0x00027EC5 File Offset: 0x000260C5
		public void MatchesAttributes([MarshalAs(UnmanagedType.LPWStr)] string pszAttributes, [MarshalAs(UnmanagedType.Bool)] out bool pfMatches)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000B02 RID: 2818 RVA: 0x0002C4BC File Offset: 0x0002A6BC
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

		// Token: 0x06000B03 RID: 2819 RVA: 0x0002C508 File Offset: 0x0002A708
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

		// Token: 0x04000836 RID: 2102
		private const string sGenerateFileNameSpecifier = "{0}";

		// Token: 0x04000837 RID: 2103
		private const string SPTOKENVALUE_CLSID = "CLSID";

		// Token: 0x04000838 RID: 2104
		private ISpObjectToken _sapiObjectToken;

		// Token: 0x04000839 RID: 2105
		private bool _disposeSapiObjectToken;

		// Token: 0x0400083A RID: 2106
		private RegistryDataKey _attributes;

		// Token: 0x020001D2 RID: 466
		[Guid("5B559F40-E952-11D2-BB91-00C04F8EE6C0")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		private interface ISpObjectWithToken
		{
			// Token: 0x06000C0F RID: 3087
			[PreserveSig]
			int SetObjectToken(ISpObjectToken pToken);

			// Token: 0x06000C10 RID: 3088
			[PreserveSig]
			int GetObjectToken(IntPtr ppToken);
		}
	}
}
