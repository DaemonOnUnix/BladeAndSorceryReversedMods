using System;

namespace System.Speech.Recognition
{
	// Token: 0x0200018C RID: 396
	public class DictationGrammar : Grammar
	{
		// Token: 0x060009CD RID: 2509 RVA: 0x0002AE49 File Offset: 0x00029E49
		public DictationGrammar()
			: base(DictationGrammar._defaultDictationUri, null, null)
		{
		}

		// Token: 0x060009CE RID: 2510 RVA: 0x0002AE58 File Offset: 0x00029E58
		public DictationGrammar(string topic)
			: base(new Uri(topic, 0), null, null)
		{
		}

		// Token: 0x060009CF RID: 2511 RVA: 0x0002AE69 File Offset: 0x00029E69
		public void SetDictationContext(string precedingText, string subsequentText)
		{
			if (base.State != GrammarState.Loaded)
			{
				throw new InvalidOperationException(SR.Get(SRID.GrammarNotLoaded, new object[0]));
			}
			base.Recognizer.SetDictationContext(this, precedingText, subsequentText);
		}

		// Token: 0x04000900 RID: 2304
		private static Uri _defaultDictationUri = new Uri("grammar:dictation");
	}
}
