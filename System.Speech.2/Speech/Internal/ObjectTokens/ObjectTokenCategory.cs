using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Speech.Internal.SapiInterop;

namespace System.Speech.Internal.ObjectTokens
{
	// Token: 0x0200016D RID: 365
	internal class ObjectTokenCategory : RegistryDataKey, IEnumerable<ObjectToken>, IEnumerable
	{
		// Token: 0x06000B04 RID: 2820 RVA: 0x0002C608 File Offset: 0x0002A808
		protected ObjectTokenCategory(string keyId, RegistryDataKey key)
			: base(keyId, key)
		{
		}

		// Token: 0x06000B05 RID: 2821 RVA: 0x0002C614 File Offset: 0x0002A814
		internal static ObjectTokenCategory Create(string sCategoryId)
		{
			RegistryDataKey registryDataKey = RegistryDataKey.Open(sCategoryId, true);
			return new ObjectTokenCategory(sCategoryId, registryDataKey);
		}

		// Token: 0x06000B06 RID: 2822 RVA: 0x0002C630 File Offset: 0x0002A830
		internal ObjectToken OpenToken(string keyName)
		{
			string text = keyName;
			if (!string.IsNullOrEmpty(text) && text.IndexOf("HKEY_", StringComparison.Ordinal) != 0)
			{
				text = string.Format(CultureInfo.InvariantCulture, "{0}\\Tokens\\{1}", new object[] { base.Id, text });
			}
			return ObjectToken.Open(null, text, false);
		}

		// Token: 0x06000B07 RID: 2823 RVA: 0x0002C680 File Offset: 0x0002A880
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

		// Token: 0x06000B08 RID: 2824 RVA: 0x0002C718 File Offset: 0x0002A918
		IEnumerator<ObjectToken> IEnumerable<ObjectToken>.GetEnumerator()
		{
			IList<ObjectToken> list = this.FindMatchingTokens(null, null);
			foreach (ObjectToken objectToken in list)
			{
				yield return objectToken;
			}
			IEnumerator<ObjectToken> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000B09 RID: 2825 RVA: 0x0002C727 File Offset: 0x0002A927
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ObjectToken>)this).GetEnumerator();
		}

		// Token: 0x06000B0A RID: 2826 RVA: 0x0002C72F File Offset: 0x0002A92F
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}
	}
}
