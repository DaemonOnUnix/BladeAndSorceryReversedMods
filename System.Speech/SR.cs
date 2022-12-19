using System;
using System.Globalization;
using System.Resources;

namespace System.Speech
{
	// Token: 0x0200017C RID: 380
	internal static class SR
	{
		// Token: 0x06000978 RID: 2424 RVA: 0x00028B98 File Offset: 0x00027B98
		internal static string Get(SRID id, params object[] args)
		{
			string text = SR._resourceManager.GetString(id.ToString());
			if (string.IsNullOrEmpty(text))
			{
				text = SR._resourceManager.GetString("Unavailable");
			}
			else if (args != null && args.Length > 0)
			{
				text = string.Format(CultureInfo.InvariantCulture, text, args);
			}
			return text;
		}

		// Token: 0x040008CC RID: 2252
		private static ResourceManager _resourceManager = new ResourceManager("ExceptionStringTable", typeof(SR).Assembly);
	}
}
