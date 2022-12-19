using System;
using System.Diagnostics;

namespace System.Speech.Synthesis
{
	// Token: 0x0200013B RID: 315
	[DebuggerDisplay("{_text}")]
	public class FilePrompt : Prompt
	{
		// Token: 0x06000863 RID: 2147 RVA: 0x00025B90 File Offset: 0x00024B90
		public FilePrompt(string path, SynthesisMediaType media)
			: this(new Uri(path, 2), media)
		{
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x00025BA0 File Offset: 0x00024BA0
		public FilePrompt(Uri promptFile, SynthesisMediaType media)
			: base(promptFile, media)
		{
		}
	}
}
