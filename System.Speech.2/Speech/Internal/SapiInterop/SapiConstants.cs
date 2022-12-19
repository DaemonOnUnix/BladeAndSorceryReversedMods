using System;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200010F RID: 271
	internal static class SapiConstants
	{
		// Token: 0x06000956 RID: 2390 RVA: 0x0002AE84 File Offset: 0x00029084
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

		// Token: 0x040006B1 RID: 1713
		internal const string SPPROP_RESPONSE_SPEED = "ResponseSpeed";

		// Token: 0x040006B2 RID: 1714
		internal const string SPPROP_COMPLEX_RESPONSE_SPEED = "ComplexResponseSpeed";

		// Token: 0x040006B3 RID: 1715
		internal const string SPPROP_CFG_CONFIDENCE_REJECTION_THRESHOLD = "CFGConfidenceRejectionThreshold";

		// Token: 0x040006B4 RID: 1716
		internal const uint SPDF_ALL = 255U;
	}
}
