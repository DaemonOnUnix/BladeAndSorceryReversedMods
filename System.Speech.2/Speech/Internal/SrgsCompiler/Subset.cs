using System;
using System.Speech.Internal.SrgsParser;

namespace System.Speech.Internal.SrgsCompiler
{
	// Token: 0x020000FD RID: 253
	internal class Subset : ParseElement, ISubset, IElement
	{
		// Token: 0x060008F1 RID: 2289 RVA: 0x00028ABC File Offset: 0x00026CBC
		public Subset(ParseElementCollection parent, Backend backend, string text, MatchMode mode)
			: base(parent._rule)
		{
			foreach (char c in Helpers._achTrimChars)
			{
				if (c != ' ' && text.IndexOf(c) >= 0)
				{
					text = text.Replace(c, ' ');
				}
			}
			parent.AddArc(backend.SubsetTransition(text, mode));
		}

		// Token: 0x060008F2 RID: 2290 RVA: 0x0000BB6D File Offset: 0x00009D6D
		void IElement.PostParse(IElement parentElement)
		{
		}
	}
}
