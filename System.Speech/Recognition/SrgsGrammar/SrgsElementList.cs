using System;
using System.Collections.ObjectModel;
using System.Speech.Internal;

namespace System.Speech.Recognition.SrgsGrammar
{
	// Token: 0x02000116 RID: 278
	[Serializable]
	internal class SrgsElementList : Collection<SrgsElement>
	{
		// Token: 0x0600071B RID: 1819 RVA: 0x000201C2 File Offset: 0x0001F1C2
		protected override void InsertItem(int index, SrgsElement element)
		{
			Helpers.ThrowIfNull(element, "element");
			base.InsertItem(index, element);
		}
	}
}
