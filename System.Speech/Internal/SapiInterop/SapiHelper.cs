using System;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200003E RID: 62
	internal class SapiHelper
	{
		// Token: 0x06000189 RID: 393 RVA: 0x0000830A File Offset: 0x0000730A
		internal static void SpGetCategoryFromId(string pszCategoryId, out ISpObjectTokenCategory ppCategory, bool fCreateIfNotExist)
		{
			ppCategory = (ISpObjectTokenCategory)new SpObjectTokenCategory();
			ppCategory.SetId(pszCategoryId, fCreateIfNotExist);
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00008324 File Offset: 0x00007324
		internal static void SpEnumTokens(string pszCategoryId, string pszReqAttribs, string pszOptAttribs, out IEnumSpObjectTokens ppEnum)
		{
			ISpObjectTokenCategory spObjectTokenCategory;
			SapiHelper.SpGetCategoryFromId(pszCategoryId, out spObjectTokenCategory, false);
			spObjectTokenCategory.EnumTokens(pszReqAttribs, pszOptAttribs, out ppEnum);
		}
	}
}
