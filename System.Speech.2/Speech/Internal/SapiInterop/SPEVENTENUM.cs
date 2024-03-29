﻿using System;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x02000108 RID: 264
	internal enum SPEVENTENUM : ushort
	{
		// Token: 0x04000668 RID: 1640
		SPEI_UNDEFINED,
		// Token: 0x04000669 RID: 1641
		SPEI_START_INPUT_STREAM,
		// Token: 0x0400066A RID: 1642
		SPEI_END_INPUT_STREAM,
		// Token: 0x0400066B RID: 1643
		SPEI_VOICE_CHANGE,
		// Token: 0x0400066C RID: 1644
		SPEI_TTS_BOOKMARK,
		// Token: 0x0400066D RID: 1645
		SPEI_WORD_BOUNDARY,
		// Token: 0x0400066E RID: 1646
		SPEI_PHONEME,
		// Token: 0x0400066F RID: 1647
		SPEI_SENTENCE_BOUNDARY,
		// Token: 0x04000670 RID: 1648
		SPEI_VISEME,
		// Token: 0x04000671 RID: 1649
		SPEI_TTS_AUDIO_LEVEL,
		// Token: 0x04000672 RID: 1650
		SPEI_TTS_PRIVATE = 15,
		// Token: 0x04000673 RID: 1651
		SPEI_MIN_TTS = 1,
		// Token: 0x04000674 RID: 1652
		SPEI_MAX_TTS = 15,
		// Token: 0x04000675 RID: 1653
		SPEI_END_SR_STREAM = 34,
		// Token: 0x04000676 RID: 1654
		SPEI_SOUND_START,
		// Token: 0x04000677 RID: 1655
		SPEI_SOUND_END,
		// Token: 0x04000678 RID: 1656
		SPEI_PHRASE_START,
		// Token: 0x04000679 RID: 1657
		SPEI_RECOGNITION,
		// Token: 0x0400067A RID: 1658
		SPEI_HYPOTHESIS,
		// Token: 0x0400067B RID: 1659
		SPEI_SR_BOOKMARK,
		// Token: 0x0400067C RID: 1660
		SPEI_PROPERTY_NUM_CHANGE,
		// Token: 0x0400067D RID: 1661
		SPEI_PROPERTY_STRING_CHANGE,
		// Token: 0x0400067E RID: 1662
		SPEI_FALSE_RECOGNITION,
		// Token: 0x0400067F RID: 1663
		SPEI_INTERFERENCE,
		// Token: 0x04000680 RID: 1664
		SPEI_REQUEST_UI,
		// Token: 0x04000681 RID: 1665
		SPEI_RECO_STATE_CHANGE,
		// Token: 0x04000682 RID: 1666
		SPEI_ADAPTATION,
		// Token: 0x04000683 RID: 1667
		SPEI_START_SR_STREAM,
		// Token: 0x04000684 RID: 1668
		SPEI_RECO_OTHER_CONTEXT,
		// Token: 0x04000685 RID: 1669
		SPEI_SR_AUDIO_LEVEL,
		// Token: 0x04000686 RID: 1670
		SPEI_SR_RETAINEDAUDIO,
		// Token: 0x04000687 RID: 1671
		SPEI_SR_PRIVATE,
		// Token: 0x04000688 RID: 1672
		SPEI_ACTIVE_CATEGORY_CHANGED,
		// Token: 0x04000689 RID: 1673
		SPEI_TEXTFEEDBACK,
		// Token: 0x0400068A RID: 1674
		SPEI_RECOGNITION_ALL,
		// Token: 0x0400068B RID: 1675
		SPEI_BARGE_IN,
		// Token: 0x0400068C RID: 1676
		SPEI_RESERVED1 = 30,
		// Token: 0x0400068D RID: 1677
		SPEI_RESERVED2 = 33,
		// Token: 0x0400068E RID: 1678
		SPEI_RESERVED3 = 63
	}
}
