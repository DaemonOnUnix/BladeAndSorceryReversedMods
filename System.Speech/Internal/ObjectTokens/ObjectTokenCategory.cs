using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Speech.Internal.SapiInterop;
using Microsoft.Win32;

namespace System.Speech.Internal.ObjectTokens
{
	// Token: 0x02000027 RID: 39
	internal class ObjectTokenCategory : RegistryDataKey, IEnumerable<ObjectToken>, IEnumerable
	{
		// Token: 0x06000119 RID: 281 RVA: 0x00007648 File Offset: 0x00006648
		protected ObjectTokenCategory(string keyId, RegistryKey hkey)
			: base(keyId, hkey)
		{
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00007652 File Offset: 0x00006652
		protected ObjectTokenCategory(string keyId, RegistryDataKey key)
			: base(keyId, key)
		{
		}

		// Token: 0x0600011B RID: 283 RVA: 0x0000765C File Offset: 0x0000665C
		internal static ObjectTokenCategory Create(string sCategoryId)
		{
			RegistryDataKey registryDataKey = RegistryDataKey.Open(sCategoryId, true);
			return new ObjectTokenCategory(sCategoryId, registryDataKey);
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00007678 File Offset: 0x00006678
		internal ObjectToken OpenToken(string keyName)
		{
			string text = keyName;
			if (!string.IsNullOrEmpty(text) && text.IndexOf("HKEY_", 4) != 0)
			{
				text = string.Format(CultureInfo.InvariantCulture, "{0}\\Tokens\\{1}", new object[] { base.Id, text });
			}
			return ObjectToken.Open(null, text, false);
		}

		// Token: 0x0600011D RID: 285 RVA: 0x000076CC File Offset: 0x000066CC
		internal IList<ObjectToken> FindMatchingTokens(string requiredAttributes, string optionalAttributes)
		{
			IList<ObjectToken> list = new List<ObjectToken>();
			ISpObjectTokenCategory spObjectTokenCategory = null;
			IEnumSpObjectTokens enumSpObjectTokens = null;
			try
			{
				spObjectTokenCategory = (ISpObjectTokenCategory)new SpObjectTokenCategory();
				spObjectTokenCategory.SetId(this._sKeyId, false);
				spObjectTokenCategory.EnumTokens(requiredAttributes, optionalAttributes, out enumSpObjectTokens);
				uint num;
				enumSpObjectTokens.GetCount(out num);
				for (uint num2 = 0U; num2 < num; num2 += 1U)
				{
					ISpObjectToken spObjectToken = null;
					enumSpObjectTokens.Item(num2, out spObjectToken);
					ObjectToken objectToken = ObjectToken.Open(spObjectToken);
					list.Add(objectToken);
				}
			}
			finally
			{
				if (enumSpObjectTokens != null)
				{
					Marshal.ReleaseComObject(enumSpObjectTokens);
				}
				if (spObjectTokenCategory != null)
				{
					Marshal.ReleaseComObject(spObjectTokenCategory);
				}
			}
			return list;
		}

		// Token: 0x0600011E RID: 286 RVA: 0x000078B4 File Offset: 0x000068B4
		IEnumerator<ObjectToken> IEnumerable<ObjectToken>.GetEnumerator()
		{
			ObjectTokenCategory.GetEnumerator>d__0 getEnumerator>d__ = new ObjectTokenCategory.GetEnumerator>d__0(0);
			getEnumerator>d__.<>4__this = this;
			return getEnumerator>d__;
		}

		// Token: 0x0600011F RID: 287 RVA: 0x000078D0 File Offset: 0x000068D0
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06000120 RID: 288 RVA: 0x000078D8 File Offset: 0x000068D8
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}
	}
}
