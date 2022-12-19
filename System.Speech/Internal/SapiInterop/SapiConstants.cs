using System;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000034 RID: 52
	internal static class SapiConstants
	{
		// Token: 0x0600015B RID: 347 RVA: 0x00008280 File Offset: 0x00007280
		internal static SRID SapiErrorCode2SRID(SAPIErrorCodes code)
		{
			if (code >= SAPIErrorCodes.SPERR_FIRST && code <= SAPIErrorCodes.SPERR_LAST)
			{
				return SRID.SapiErrorUninitialized + (code - SAPIErrorCodes.SPERR_FIRST);
			}
			if (code <= SAPIErrorCodes.SP_NO_PARSE_FOUND)
			{
				if (code == SAPIErrorCodes.S_FALSE)
				{
					return SRID.UnexpectedError;
				}
				if (code == SAPIErrorCodes.SP_NO_PARSE_FOUND)
				{
					return SRID.NoParseFound;
				}
			}
			else
			{
				if (code == SAPIErrorCodes.SP_NO_RULE_ACTIVE)
				{
					return SRID.SapiErrorNoRuleActive;
				}
				if (code == SAPIErrorCodes.SP_NO_RULES_TO_ACTIVATE)
				{
					return SRID.SapiErrorNoRulesToActivate;
				}
			}
			return (SRID)(-1);
		}

		// Token: 0x04000135 RID: 309
		internal const string SPPROP_RESPONSE_SPEED = "ResponseSpeed";

		// Token: 0x04000136 RID: 310
		internal const string SPPROP_COMPLEX_RESPONSE_SPEED = "ComplexResponseSpeed";

		// Token: 0x04000137 RID: 311
		internal const string SPPROP_CFG_CONFIDENCE_REJECTION_THRESHOLD = "CFGConfidenceRejectionThreshold";

		// Token: 0x04000138 RID: 312
		internal const uint SPDF_ALL = 255U;
	}
}
