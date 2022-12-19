using System;
using System.Collections.ObjectModel;
using System.Speech.Internal;

namespace System.Speech.Recognition.SrgsGrammar
{
	// Token: 0x02000077 RID: 119
	[Serializable]
	internal class SrgsElementList : Collection<SrgsElement>
	{
		// Token: 0x060003D3 RID: 979 RVA: 0x0000FA4C File Offset: 0x0000DC4C
		protected override void InsertItem(int index, SrgsElement element)
		{
			Helpers.ThrowIfNull(element, "element");
			base.InsertItem(index, element);
		}
	}
}
