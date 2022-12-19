using System;
using System.Speech.Internal.SrgsParser;

namespace System.Speech.Internal.SrgsCompiler
{
	// Token: 0x02000101 RID: 257
	internal class ScriptRef
	{
		// Token: 0x0600090F RID: 2319 RVA: 0x00029CAC File Offset: 0x00027EAC
		internal ScriptRef(string rule, string sMethod, RuleMethodScript method)
		{
			this._rule = rule;
			this._sMethod = sMethod;
			this._method = method;
		}

		// Token: 0x06000910 RID: 2320 RVA: 0x00029CCC File Offset: 0x00027ECC
		internal void Serialize(StringBlob symbols, StreamMarshaler streamBuffer)
		{
			streamBuffer.WriteStream(new CfgScriptRef
			{
				_idRule = symbols.Find(this._rule),
				_method = this._method,
				_idMethod = this._idSymbol
			});
		}

		// Token: 0x06000911 RID: 2321 RVA: 0x00029D1C File Offset: 0x00027F1C
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

		// Token: 0x0400064A RID: 1610
		internal string _rule;

		// Token: 0x0400064B RID: 1611
		internal string _sMethod;

		// Token: 0x0400064C RID: 1612
		internal RuleMethodScript _method;

		// Token: 0x0400064D RID: 1613
		internal int _idSymbol;
	}
}
