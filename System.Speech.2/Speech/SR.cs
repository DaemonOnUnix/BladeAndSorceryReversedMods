using System;
using System.Globalization;
using System.Resources;

namespace System.Speech
{
	// Token: 0x02000004 RID: 4
	internal static class SR
	{
		// Token: 0x06000003 RID: 3 RVA: 0x00002190 File Offset: 0x00000390
		internal static string Get(SRID id, params object[] args)
		{
			string text = SR._resourceManager.GetString(id.ToString());
			if (string.IsNullOrEmpty(text))
			{
				text = SR._resourceManager.GetString("Unavailable");
			}
			else if (args != null && args.Length != 0)
			{
				text = string.Format(CultureInfo.InvariantCulture, text, args);
			}
			return text;
		}

		// Token: 0x04000187 RID: 391
		private static ResourceManager _resourceManager = new ResourceManager("ExceptionStringTable", typeof(SR).Assembly);
	}
}
