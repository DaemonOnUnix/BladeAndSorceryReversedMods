using System;

namespace System.Speech.Synthesis
{
	// Token: 0x02000005 RID: 5
	public class BookmarkReachedEventArgs : PromptEventArgs
	{
		// Token: 0x06000005 RID: 5 RVA: 0x00002204 File Offset: 0x00000404
		internal BookmarkReachedEventArgs(Prompt prompt, string bookmark, TimeSpan audioPosition)
			: base(prompt)
		{
			this._bookmark = bookmark;
			this._audioPosition = audioPosition;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000006 RID: 6 RVA: 0x0000221B File Offset: 0x0000041B
		public string Bookmark
		{
			get
			{
				return this._bookmark;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000007 RID: 7 RVA: 0x00002223 File Offset: 0x00000423
		public TimeSpan AudioPosition
		{
			get
			{
				return this._audioPosition;
			}
		}

		// Token: 0x04000188 RID: 392
		private string _bookmark;

		// Token: 0x04000189 RID: 393
		private TimeSpan _audioPosition;
	}
}
