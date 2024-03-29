﻿using System;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200010E RID: 270
	internal enum SAPIErrorCodes
	{
		// Token: 0x04000694 RID: 1684
		S_OK,
		// Token: 0x04000695 RID: 1685
		S_FALSE,
		// Token: 0x04000696 RID: 1686
		SP_NO_RULE_ACTIVE = 282709,
		// Token: 0x04000697 RID: 1687
		SP_NO_RULES_TO_ACTIVATE = 282747,
		// Token: 0x04000698 RID: 1688
		S_LIMIT_REACHED = 282751,
		// Token: 0x04000699 RID: 1689
		E_FAIL = -2147467259,
		// Token: 0x0400069A RID: 1690
		SP_NO_PARSE_FOUND = 282668,
		// Token: 0x0400069B RID: 1691
		SP_WORD_EXISTS_WITHOUT_PRONUNCIATION = 282679,
		// Token: 0x0400069C RID: 1692
		SPERR_FIRST = -2147201023,
		// Token: 0x0400069D RID: 1693
		SPERR_LAST = -2147200890,
		// Token: 0x0400069E RID: 1694
		STG_E_FILENOTFOUND = -2147287038,
		// Token: 0x0400069F RID: 1695
		CLASS_E_CLASSNOTAVAILABLE = -2147221231,
		// Token: 0x040006A0 RID: 1696
		REGDB_E_CLASSNOTREG = -2147221164,
		// Token: 0x040006A1 RID: 1697
		SPERR_UNSUPPORTED_FORMAT = -2147201021,
		// Token: 0x040006A2 RID: 1698
		SPERR_UNSUPPORTED_PHONEME = -2147200902,
		// Token: 0x040006A3 RID: 1699
		SPERR_VOICE_NOT_FOUND = -2147200877,
		// Token: 0x040006A4 RID: 1700
		SPERR_NOT_IN_LEX = -2147200999,
		// Token: 0x040006A5 RID: 1701
		SPERR_TOO_MANY_GRAMMARS = -2147200990,
		// Token: 0x040006A6 RID: 1702
		SPERR_INVALID_IMPORT = -2147200988,
		// Token: 0x040006A7 RID: 1703
		SPERR_STREAM_CLOSED = -2147200968,
		// Token: 0x040006A8 RID: 1704
		SPERR_NO_MORE_ITEMS,
		// Token: 0x040006A9 RID: 1705
		SPERR_NOT_FOUND,
		// Token: 0x040006AA RID: 1706
		SPERR_NOT_TOPLEVEL_RULE = -2147200940,
		// Token: 0x040006AB RID: 1707
		SPERR_SHARED_ENGINE_DISABLED = -2147200906,
		// Token: 0x040006AC RID: 1708
		SPERR_RECOGNIZER_NOT_FOUND,
		// Token: 0x040006AD RID: 1709
		SPERR_AUDIO_NOT_FOUND,
		// Token: 0x040006AE RID: 1710
		SPERR_NOT_SUPPORTED_FOR_INPROC_RECOGNIZER = -2147200893,
		// Token: 0x040006AF RID: 1711
		SPERR_LEX_INVALID_DATA = -2147200891,
		// Token: 0x040006B0 RID: 1712
		SPERR_CFG_INVALID_DATA
	}
}
