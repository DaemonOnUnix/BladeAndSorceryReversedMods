using System;

namespace System.Speech.Recognition
{
	// Token: 0x0200005C RID: 92
	public class DictationGrammar : Grammar
	{
		// Token: 0x060001FD RID: 509 RVA: 0x00009519 File Offset: 0x00007719
		public DictationGrammar()
			: base(DictationGrammar._defaultDictationUri, null, null)
		{
		}

		// Token: 0x060001FE RID: 510 RVA: 0x00009528 File Offset: 0x00007728
		public DictationGrammar(string topic)
			: base(new Uri(topic, UriKind.RelativeOrAbsolute), null, null)
		{
		}

		// Token: 0x060001FF RID: 511 RVA: 0x00009539 File Offset: 0x00007739
		public void SetDictationContext(string precedingText, string subsequentText)
		{
			if (base.State != GrammarState.Loaded)
			{
				throw new InvalidOperationException(SR.Get(SRID.GrammarNotLoaded, new object[0]));
			}
			base.Recognizer.SetDictationContext(this, precedingText, subsequentText);
		}

		// Token: 0x04000341 RID: 833
		private static Uri _defaultDictationUri = new Uri("grammar:dictation");
	}
}
