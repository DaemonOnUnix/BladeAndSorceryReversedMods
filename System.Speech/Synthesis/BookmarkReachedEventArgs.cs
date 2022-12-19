using System;

namespace System.Speech.Synthesis
{
	// Token: 0x02000139 RID: 313
	public class BookmarkReachedEventArgs : PromptEventArgs
	{
		// Token: 0x06000858 RID: 2136 RVA: 0x0002599E File Offset: 0x0002499E
		internal BookmarkReachedEventArgs(Prompt prompt, string bookmark, TimeSpan audioPosition)
			: base(prompt)
		{
			this._bookmark = bookmark;
			this._audioPosition = audioPosition;
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000859 RID: 2137 RVA: 0x000259B5 File Offset: 0x000249B5
		public string Bookmark
		{
			get
			{
				return this._bookmark;
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x0600085A RID: 2138 RVA: 0x000259BD File Offset: 0x000249BD
		public TimeSpan AudioPosition
		{
			get
			{
				return this._audioPosition;
			}
		}

		// Token: 0x040005E9 RID: 1513
		private string _bookmark;

		// Token: 0x040005EA RID: 1514
		private TimeSpan _audioPosition;
	}
}
