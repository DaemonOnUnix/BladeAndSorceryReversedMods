using System;
using System.Speech.Internal.SrgsParser;

namespace System.Speech.Internal.SrgsCompiler
{
	// Token: 0x020000BF RID: 191
	internal class Subset : ParseElement, ISubset, IElement
	{
		// Token: 0x0600044D RID: 1101 RVA: 0x00010DCC File Offset: 0x0000FDCC
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

		// Token: 0x0600044E RID: 1102 RVA: 0x00010E26 File Offset: 0x0000FE26
		void IElement.PostParse(IElement parentElement)
		{
		}
	}
}
