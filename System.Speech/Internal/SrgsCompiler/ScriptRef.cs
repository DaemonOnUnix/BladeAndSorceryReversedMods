using System;
using System.Speech.Internal.SrgsParser;

namespace System.Speech.Internal.SrgsCompiler
{
	// Token: 0x02000181 RID: 385
	internal class ScriptRef
	{
		// Token: 0x060009A6 RID: 2470 RVA: 0x00029E64 File Offset: 0x00028E64
		internal ScriptRef(string rule, string sMethod, RuleMethodScript method)
		{
			this._rule = rule;
			this._sMethod = sMethod;
			this._method = method;
		}

		// Token: 0x060009A7 RID: 2471 RVA: 0x00029E84 File Offset: 0x00028E84
		internal void Serialize(StringBlob symbols, StreamMarshaler streamBuffer)
		{
			streamBuffer.WriteStream(new CfgScriptRef
			{
				_idRule = symbols.Find(this._rule),
				_method = this._method,
				_idMethod = this._idSymbol
			});
		}

		// Token: 0x060009A8 RID: 2472 RVA: 0x00029ED4 File Offset: 0x00028ED4
		internal static string OnInitMethod(ScriptRef[] scriptRefs, string rule)
		{
			if (scriptRefs != null)
			{
				foreach (ScriptRef scriptRef in scriptRefs)
				{
					if (scriptRef._rule == rule && scriptRef._method == RuleMethodScript.onInit)
					{
						return scriptRef._sMethod;
					}
				}
			}
			return null;
		}

		// Token: 0x040008DF RID: 2271
		internal string _rule;

		// Token: 0x040008E0 RID: 2272
		internal string _sMethod;

		// Token: 0x040008E1 RID: 2273
		internal RuleMethodScript _method;

		// Token: 0x040008E2 RID: 2274
		internal int _idSymbol;
	}
}
