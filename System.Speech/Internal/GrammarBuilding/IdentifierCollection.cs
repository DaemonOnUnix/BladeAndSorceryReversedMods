using System;
using System.Collections.Generic;

namespace System.Speech.Internal.GrammarBuilding
{
	// Token: 0x020001AA RID: 426
	internal class IdentifierCollection
	{
		// Token: 0x06000BA6 RID: 2982 RVA: 0x00031394 File Offset: 0x00030394
		internal IdentifierCollection()
		{
			this._identifiers = new List<string>();
			this.CreateNewIdentifier("_");
		}

		// Token: 0x06000BA7 RID: 2983 RVA: 0x000313B4 File Offset: 0x000303B4
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

		// Token: 0x0400098A RID: 2442
		protected List<string> _identifiers;
	}
}
