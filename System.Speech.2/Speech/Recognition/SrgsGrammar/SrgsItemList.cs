using System;
using System.Collections.ObjectModel;
using System.Speech.Internal;

namespace System.Speech.Recognition.SrgsGrammar
{
	// Token: 0x0200007B RID: 123
	[Serializable]
	internal class SrgsItemList : Collection<SrgsItem>
	{
		// Token: 0x0600041F RID: 1055 RVA: 0x00010BF8 File Offset: 0x0000EDF8
		protected override void InsertItem(int index, SrgsItem item)
		{
			Helpers.ThrowIfNull(item, "item");
			base.InsertItem(index, item);
		}
	}
}
