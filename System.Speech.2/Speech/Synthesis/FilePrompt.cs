using System;
using System.Diagnostics;

namespace System.Speech.Synthesis
{
	// Token: 0x02000006 RID: 6
	[DebuggerDisplay("{_text}")]
	public class FilePrompt : Prompt
	{
		// Token: 0x06000008 RID: 8 RVA: 0x0000222B File Offset: 0x0000042B
		public FilePrompt(string path, SynthesisMediaType media)
			: this(new Uri(path, UriKind.Relative), media)
		{
		}

		// Token: 0x06000009 RID: 9 RVA: 0x0000223B File Offset: 0x0000043B
		public FilePrompt(Uri promptFile, SynthesisMediaType media)
			: base(promptFile, media)
		{
		}
	}
}
