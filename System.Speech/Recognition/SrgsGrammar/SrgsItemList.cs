using System;
using System.Collections.ObjectModel;
using System.Speech.Internal;

namespace System.Speech.Recognition.SrgsGrammar
{
	// Token: 0x0200011B RID: 283
	[Serializable]
	internal class SrgsItemList : Collection<SrgsItem>
	{
		// Token: 0x0600076E RID: 1902 RVA: 0x000214E4 File Offset: 0x000204E4
		protected override void InsertItem(int index, SrgsItem item)
		{
			Helpers.ThrowIfNull(item, "item");
			base.InsertItem(index, item);
		}
	}
}
