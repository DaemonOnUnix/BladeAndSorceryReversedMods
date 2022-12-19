using System;
using System.Collections.Generic;

namespace System.Speech.Internal.GrammarBuilding
{
	// Token: 0x020000A0 RID: 160
	internal class IdentifierCollection
	{
		// Token: 0x06000554 RID: 1364 RVA: 0x000155B4 File Offset: 0x000137B4
		internal IdentifierCollection()
		{
			this._identifiers = new List<string>();
			this.CreateNewIdentifier("_");
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x000155D4 File Offset: 0x000137D4
		internal string CreateNewIdentifier(string id)
		{
			if (!this._identifiers.Contains(id))
			{
				this._identifiers.Add(id);
				return id;
			}
			int num = 1;
			string text;
			do
			{
				text = id + num;
				num++;
			}
			while (this._identifiers.Contains(text));
			this._identifiers.Add(text);
			return text;
		}

		// Token: 0x04000454 RID: 1108
		protected List<string> _identifiers;
	}
}
