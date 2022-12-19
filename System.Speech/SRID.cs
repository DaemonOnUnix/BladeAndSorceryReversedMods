using System;

namespace System.Speech
{
	// Token: 0x0200017B RID: 379
	internal enum SRID
	{
		// Token: 0x04000748 RID: 1864
		NullParamIllegal,
		// Token: 0x04000749 RID: 1865
		ArrayOfNullIllegal,
		// Token: 0x0400074A RID: 1866
		ParamsEntryNullIllegal,
		// Token: 0x0400074B RID: 1867
		Unavailable,
		// Token: 0x0400074C RID: 1868
		UnexpectedError,
		// Token: 0x0400074D RID: 1869
		CollectionReadOnly,
		// Token: 0x0400074E RID: 1870
		StringCanNotBeEmpty,
		// Token: 0x0400074F RID: 1871
		EnumInvalid,
		// Token: 0x04000750 RID: 1872
		NotSupportedWithThisVersionOfSAPI,
		// Token: 0x04000751 RID: 1873
		NotSupportedWithThisVersionOfSAPI2,
		// Token: 0x04000752 RID: 1874
		NotSupportedWithThisVersionOfSAPIBaseUri,
		// Token: 0x04000753 RID: 1875
		NotSupportedWithThisVersionOfSAPITagFormat,
		// Token: 0x04000754 RID: 1876
		NotSupportedWithThisVersionOfSAPICompareOption,
		// Token: 0x04000755 RID: 1877
		MustBeGreaterThanZero,
		// Token: 0x04000756 RID: 1878
		InvalidXml,
		// Token: 0x04000757 RID: 1879
		OperationAborted,
		// Token: 0x04000758 RID: 1880
		InvariantCultureInfo,
		// Token: 0x04000759 RID: 1881
		DuplicatedEntry,
		// Token: 0x0400075A RID: 1882
		StreamMustBeReadable,
		// Token: 0x0400075B RID: 1883
		StreamMustBeWriteable,
		// Token: 0x0400075C RID: 1884
		StreamMustBeSeekable,
		// Token: 0x0400075D RID: 1885
		StreamEndedUnexpectedly,
		// Token: 0x0400075E RID: 1886
		CannotReadFromDirectory,
		// Token: 0x0400075F RID: 1887
		UnknownMimeFormat,
		// Token: 0x04000760 RID: 1888
		CannotLoadResourceFromManifest,
		// Token: 0x04000761 RID: 1889
		TokenInUse,
		// Token: 0x04000762 RID: 1890
		TokenDeleted,
		// Token: 0x04000763 RID: 1891
		TokenUninitialized,
		// Token: 0x04000764 RID: 1892
		InvalidTokenId,
		// Token: 0x04000765 RID: 1893
		NotFound,
		// Token: 0x04000766 RID: 1894
		NoBackSlash,
		// Token: 0x04000767 RID: 1895
		InvalidRegistryEntry,
		// Token: 0x04000768 RID: 1896
		TokenCannotCreateInstance,
		// Token: 0x04000769 RID: 1897
		InvalidXmlFormat,
		// Token: 0x0400076A RID: 1898
		IncorrectAttributeValue,
		// Token: 0x0400076B RID: 1899
		MissingRequiredAttribute,
		// Token: 0x0400076C RID: 1900
		InvalidRuleRefSelf,
		// Token: 0x0400076D RID: 1901
		InvalidDynamicExport,
		// Token: 0x0400076E RID: 1902
		InvalidToken,
		// Token: 0x0400076F RID: 1903
		MetaNameHTTPEquiv,
		// Token: 0x04000770 RID: 1904
		EmptyRule,
		// Token: 0x04000771 RID: 1905
		InvalidTokenString,
		// Token: 0x04000772 RID: 1906
		InvalidQuotedString,
		// Token: 0x04000773 RID: 1907
		ExportDynamicRule,
		// Token: 0x04000774 RID: 1908
		EmptyDisplayString,
		// Token: 0x04000775 RID: 1909
		EmptyPronunciationString,
		// Token: 0x04000776 RID: 1910
		InvalidPhoneme,
		// Token: 0x04000777 RID: 1911
		MuliplePronunciationString,
		// Token: 0x04000778 RID: 1912
		MultipleDisplayString,
		// Token: 0x04000779 RID: 1913
		RuleRedefinition,
		// Token: 0x0400077A RID: 1914
		EmptyOneOf,
		// Token: 0x0400077B RID: 1915
		InvalidGrammarOrdering,
		// Token: 0x0400077C RID: 1916
		MinMaxOutOfRange,
		// Token: 0x0400077D RID: 1917
		InvalidExampleOrdering,
		// Token: 0x0400077E RID: 1918
		GrammarDefTwice,
		// Token: 0x0400077F RID: 1919
		UnsupportedFormat,
		// Token: 0x04000780 RID: 1920
		InvalidImport,
		// Token: 0x04000781 RID: 1921
		DuplicatedRuleName,
		// Token: 0x04000782 RID: 1922
		RootRuleAlreadyDefined,
		// Token: 0x04000783 RID: 1923
		RuleNameIdConflict,
		// Token: 0x04000784 RID: 1924
		RuleNotDynamic,
		// Token: 0x04000785 RID: 1925
		StateWithNoArcs,
		// Token: 0x04000786 RID: 1926
		NoTerminatingRulePath,
		// Token: 0x04000787 RID: 1927
		RuleRefNoUri,
		// Token: 0x04000788 RID: 1928
		UnavailableProperty,
		// Token: 0x04000789 RID: 1929
		MinGreaterThanMax,
		// Token: 0x0400078A RID: 1930
		ReqConfidenceNotSupported,
		// Token: 0x0400078B RID: 1931
		SapiPropertiesAndSemantics,
		// Token: 0x0400078C RID: 1932
		InvalidAttributeDefinedTwice,
		// Token: 0x0400078D RID: 1933
		GrammarCompilerError,
		// Token: 0x0400078E RID: 1934
		RuleScriptNotFound,
		// Token: 0x0400078F RID: 1935
		DynamicRuleNotFound,
		// Token: 0x04000790 RID: 1936
		RuleScriptInvalidParameters,
		// Token: 0x04000791 RID: 1937
		RuleScriptInvalidReturnType,
		// Token: 0x04000792 RID: 1938
		NoClassname,
		// Token: 0x04000793 RID: 1939
		EmbeddedClassLibraryFailed,
		// Token: 0x04000794 RID: 1940
		CannotFindClass,
		// Token: 0x04000795 RID: 1941
		StrongTypedGrammarNotAGrammar,
		// Token: 0x04000796 RID: 1942
		NoScriptsForRules,
		// Token: 0x04000797 RID: 1943
		ClassNotPublic,
		// Token: 0x04000798 RID: 1944
		MethodNotPublic,
		// Token: 0x04000799 RID: 1945
		IncompatibleLanguageProperties,
		// Token: 0x0400079A RID: 1946
		IncompatibleNamespaceProperties,
		// Token: 0x0400079B RID: 1947
		IncompatibleDebugProperties,
		// Token: 0x0400079C RID: 1948
		CannotLoadDotNetSemanticCode,
		// Token: 0x0400079D RID: 1949
		InvalidSemanticProcessingType,
		// Token: 0x0400079E RID: 1950
		InvalidScriptDefinition,
		// Token: 0x0400079F RID: 1951
		InvalidMethodName,
		// Token: 0x040007A0 RID: 1952
		ConstructorNotAllowed,
		// Token: 0x040007A1 RID: 1953
		OverloadNotAllowed,
		// Token: 0x040007A2 RID: 1954
		OnInitOnPublicRule,
		// Token: 0x040007A3 RID: 1955
		ArgumentMismatch,
		// Token: 0x040007A4 RID: 1956
		CantGetPropertyFromSerializedInfo,
		// Token: 0x040007A5 RID: 1957
		CantFindAConstructor,
		// Token: 0x040007A6 RID: 1958
		TooManyArcs,
		// Token: 0x040007A7 RID: 1959
		TooManyRulesWithSemanticsGlobals,
		// Token: 0x040007A8 RID: 1960
		MaxTransitionsCount,
		// Token: 0x040007A9 RID: 1961
		UnknownElement,
		// Token: 0x040007AA RID: 1962
		CircularRuleRef,
		// Token: 0x040007AB RID: 1963
		RuleDefinedMultipleTimes,
		// Token: 0x040007AC RID: 1964
		RuleDefinedMultipleTimes2,
		// Token: 0x040007AD RID: 1965
		RuleNotDefined,
		// Token: 0x040007AE RID: 1966
		RootNotDefined,
		// Token: 0x040007AF RID: 1967
		InvalidLanguage,
		// Token: 0x040007B0 RID: 1968
		InvalidRuleId,
		// Token: 0x040007B1 RID: 1969
		InvalidRepeatProbability,
		// Token: 0x040007B2 RID: 1970
		InvalidConfidence,
		// Token: 0x040007B3 RID: 1971
		InvalidMinRepeat,
		// Token: 0x040007B4 RID: 1972
		InvalidMaxRepeat,
		// Token: 0x040007B5 RID: 1973
		InvalidWeight,
		// Token: 0x040007B6 RID: 1974
		InvalidName,
		// Token: 0x040007B7 RID: 1975
		InvalidValueType,
		// Token: 0x040007B8 RID: 1976
		TagFormatNotSet,
		// Token: 0x040007B9 RID: 1977
		NoName,
		// Token: 0x040007BA RID: 1978
		NoName1,
		// Token: 0x040007BB RID: 1979
		InvalidSpecialRuleRef,
		// Token: 0x040007BC RID: 1980
		InvalidRuleRef,
		// Token: 0x040007BD RID: 1981
		InvalidNotEmptyElement,
		// Token: 0x040007BE RID: 1982
		InvalidEmptyElement,
		// Token: 0x040007BF RID: 1983
		InvalidEmptyRule,
		// Token: 0x040007C0 RID: 1984
		UndefRuleRef,
		// Token: 0x040007C1 RID: 1985
		UnsupportedLanguage,
		// Token: 0x040007C2 RID: 1986
		UnsupportedPhoneticAlphabet,
		// Token: 0x040007C3 RID: 1987
		UnsupportedLexicon,
		// Token: 0x040007C4 RID: 1988
		InvalidScriptAttribute,
		// Token: 0x040007C5 RID: 1989
		NoLanguageSet,
		// Token: 0x040007C6 RID: 1990
		MethodAttributeDefinedMultipeTimes,
		// Token: 0x040007C7 RID: 1991
		RuleAttributeDefinedMultipeTimes,
		// Token: 0x040007C8 RID: 1992
		InvalidAssemblyReferenceAttribute,
		// Token: 0x040007C9 RID: 1993
		InvalidImportNamespaceAttribute,
		// Token: 0x040007CA RID: 1994
		NoUriForSpecialRuleRef,
		// Token: 0x040007CB RID: 1995
		NoAliasForSpecialRuleRef,
		// Token: 0x040007CC RID: 1996
		NoSmlData,
		// Token: 0x040007CD RID: 1997
		InvalidNameValueProperty,
		// Token: 0x040007CE RID: 1998
		InvalidTagInAnEmptyItem,
		// Token: 0x040007CF RID: 1999
		InvalidSrgs,
		// Token: 0x040007D0 RID: 2000
		InvalidSrgsNamespace,
		// Token: 0x040007D1 RID: 2001
		Line,
		// Token: 0x040007D2 RID: 2002
		Position,
		// Token: 0x040007D3 RID: 2003
		InvalidVersion,
		// Token: 0x040007D4 RID: 2004
		InvalidTagFormat,
		// Token: 0x040007D5 RID: 2005
		MissingTagFormat,
		// Token: 0x040007D6 RID: 2006
		InvalidGrammarMode,
		// Token: 0x040007D7 RID: 2007
		InvalidGrammarAttribute,
		// Token: 0x040007D8 RID: 2008
		InvalidRuleAttribute,
		// Token: 0x040007D9 RID: 2009
		InvalidRulerefAttribute,
		// Token: 0x040007DA RID: 2010
		InvalidOneOfAttribute,
		// Token: 0x040007DB RID: 2011
		InvalidItemAttribute,
		// Token: 0x040007DC RID: 2012
		InvalidTokenAttribute,
		// Token: 0x040007DD RID: 2013
		InvalidItemRepeatAttribute,
		// Token: 0x040007DE RID: 2014
		InvalidReqConfAttribute,
		// Token: 0x040007DF RID: 2015
		InvalidTagAttribute,
		// Token: 0x040007E0 RID: 2016
		InvalidLexiconAttribute,
		// Token: 0x040007E1 RID: 2017
		InvalidMetaAttribute,
		// Token: 0x040007E2 RID: 2018
		InvalidItemAttribute2,
		// Token: 0x040007E3 RID: 2019
		InvalidElement,
		// Token: 0x040007E4 RID: 2020
		InvalidRuleScope,
		// Token: 0x040007E5 RID: 2021
		InvalidDynamicSetting,
		// Token: 0x040007E6 RID: 2022
		InvalidSubsetAttribute,
		// Token: 0x040007E7 RID: 2023
		InvalidVoiceElementInPromptOutput,
		// Token: 0x040007E8 RID: 2024
		NoRuleId,
		// Token: 0x040007E9 RID: 2025
		PromptBuilderInvalideState,
		// Token: 0x040007EA RID: 2026
		PromptBuilderStateEnded,
		// Token: 0x040007EB RID: 2027
		PromptBuilderStateSentence,
		// Token: 0x040007EC RID: 2028
		PromptBuilderStateParagraph,
		// Token: 0x040007ED RID: 2029
		PromptBuilderStateVoice,
		// Token: 0x040007EE RID: 2030
		PromptBuilderStateStyle,
		// Token: 0x040007EF RID: 2031
		PromptBuilderAgeOutOfRange,
		// Token: 0x040007F0 RID: 2032
		PromptBuilderMismatchStyle,
		// Token: 0x040007F1 RID: 2033
		PromptBuilderMismatchVoice,
		// Token: 0x040007F2 RID: 2034
		PromptBuilderMismatchParagraph,
		// Token: 0x040007F3 RID: 2035
		PromptBuilderMismatchSentence,
		// Token: 0x040007F4 RID: 2036
		PromptBuilderNestedParagraph,
		// Token: 0x040007F5 RID: 2037
		PromptBuilderNestedSentence,
		// Token: 0x040007F6 RID: 2038
		PromptBuilderInvalidAttribute,
		// Token: 0x040007F7 RID: 2039
		PromptBuilderInvalidElement,
		// Token: 0x040007F8 RID: 2040
		PromptBuilderInvalidVariant,
		// Token: 0x040007F9 RID: 2041
		PromptBuilderDatabaseName,
		// Token: 0x040007FA RID: 2042
		PromptAsyncOperationCancelled,
		// Token: 0x040007FB RID: 2043
		SynthesizerPauseResumeMismatched,
		// Token: 0x040007FC RID: 2044
		SynthesizerInvalidMediaType,
		// Token: 0x040007FD RID: 2045
		SynthesizerUnknownMediaType,
		// Token: 0x040007FE RID: 2046
		SynthesizerSpeakError,
		// Token: 0x040007FF RID: 2047
		SynthesizerInvalidWaveFile,
		// Token: 0x04000800 RID: 2048
		SynthesizerPromptInUse,
		// Token: 0x04000801 RID: 2049
		SynthesizerUnknownPriority,
		// Token: 0x04000802 RID: 2050
		SynthesizerUnknownEvent,
		// Token: 0x04000803 RID: 2051
		SynthesizerVoiceFailed,
		// Token: 0x04000804 RID: 2052
		SynthesizerSetVoiceNoMatch,
		// Token: 0x04000805 RID: 2053
		SynthesizerNoCulture,
		// Token: 0x04000806 RID: 2054
		SynthesizerSyncSpeakWhilePaused,
		// Token: 0x04000807 RID: 2055
		SynthesizerSyncSetOutputWhilePaused,
		// Token: 0x04000808 RID: 2056
		SynthesizerNoCulture2,
		// Token: 0x04000809 RID: 2057
		SynthesizerNoSpeak,
		// Token: 0x0400080A RID: 2058
		SynthesizerSetOutputSpeaking,
		// Token: 0x0400080B RID: 2059
		InvalidSpeakAttribute,
		// Token: 0x0400080C RID: 2060
		UnsupportedAlphabet,
		// Token: 0x0400080D RID: 2061
		GrammarInvalidWeight,
		// Token: 0x0400080E RID: 2062
		GrammarInvalidPriority,
		// Token: 0x0400080F RID: 2063
		DictationInvalidTopic,
		// Token: 0x04000810 RID: 2064
		DictationTopicNotFound,
		// Token: 0x04000811 RID: 2065
		RecognizerGrammarNotFound,
		// Token: 0x04000812 RID: 2066
		RecognizerRuleNotFound,
		// Token: 0x04000813 RID: 2067
		RecognizerInvalidBinaryGrammar,
		// Token: 0x04000814 RID: 2068
		RecognizerRuleNotFoundStream,
		// Token: 0x04000815 RID: 2069
		RecognizerNoRootRuleToActivate,
		// Token: 0x04000816 RID: 2070
		RecognizerNoRootRuleToActivate1,
		// Token: 0x04000817 RID: 2071
		RecognizerRuleActivationFailed,
		// Token: 0x04000818 RID: 2072
		RecognizerAlreadyRecognizing,
		// Token: 0x04000819 RID: 2073
		RecognizerHasNoGrammar,
		// Token: 0x0400081A RID: 2074
		NegativeTimesNotSupported,
		// Token: 0x0400081B RID: 2075
		AudioDeviceFormatError,
		// Token: 0x0400081C RID: 2076
		AudioDeviceError,
		// Token: 0x0400081D RID: 2077
		AudioDeviceInternalError,
		// Token: 0x0400081E RID: 2078
		RecognizerNotFound,
		// Token: 0x0400081F RID: 2079
		RecognizerNotEnabled,
		// Token: 0x04000820 RID: 2080
		RecognitionNotSupported,
		// Token: 0x04000821 RID: 2081
		RecognitionNotSupportedOn64bit,
		// Token: 0x04000822 RID: 2082
		GrammarAlreadyLoaded,
		// Token: 0x04000823 RID: 2083
		RecognizerNoInputSource,
		// Token: 0x04000824 RID: 2084
		GrammarNotLoaded,
		// Token: 0x04000825 RID: 2085
		GrammarLoadingInProgress,
		// Token: 0x04000826 RID: 2086
		GrammarLoadFailed,
		// Token: 0x04000827 RID: 2087
		GrammarWrongRecognizer,
		// Token: 0x04000828 RID: 2088
		NotSupportedOnDictationGrammars,
		// Token: 0x04000829 RID: 2089
		LocalFilesOnly,
		// Token: 0x0400082A RID: 2090
		NotValidAudioFile,
		// Token: 0x0400082B RID: 2091
		NotValidAudioStream,
		// Token: 0x0400082C RID: 2092
		FileNotFound,
		// Token: 0x0400082D RID: 2093
		CannotSetPriorityOnDictation,
		// Token: 0x0400082E RID: 2094
		RecognizerUpdateTableTooLarge,
		// Token: 0x0400082F RID: 2095
		MaxAlternatesInvalid,
		// Token: 0x04000830 RID: 2096
		RecognizerSettingGetError,
		// Token: 0x04000831 RID: 2097
		RecognizerSettingUpdateError,
		// Token: 0x04000832 RID: 2098
		RecognizerSettingNotSupported,
		// Token: 0x04000833 RID: 2099
		ResourceUsageOutOfRange,
		// Token: 0x04000834 RID: 2100
		RateOutOfRange,
		// Token: 0x04000835 RID: 2101
		EndSilenceOutOfRange,
		// Token: 0x04000836 RID: 2102
		RejectionThresholdOutOfRange,
		// Token: 0x04000837 RID: 2103
		ReferencedGrammarNotFound,
		// Token: 0x04000838 RID: 2104
		SapiErrorRuleNotFound2,
		// Token: 0x04000839 RID: 2105
		NoAudioAvailable,
		// Token: 0x0400083A RID: 2106
		ResultNotGrammarAvailable,
		// Token: 0x0400083B RID: 2107
		ResultInvalidFormat,
		// Token: 0x0400083C RID: 2108
		UnhandledVariant,
		// Token: 0x0400083D RID: 2109
		DupSemanticKey,
		// Token: 0x0400083E RID: 2110
		DupSemanticValue,
		// Token: 0x0400083F RID: 2111
		CannotUseCustomFormat,
		// Token: 0x04000840 RID: 2112
		NoPromptEngine,
		// Token: 0x04000841 RID: 2113
		NoPromptEngineInterface,
		// Token: 0x04000842 RID: 2114
		SeekNotSupported,
		// Token: 0x04000843 RID: 2115
		ExtraDataNotPresent,
		// Token: 0x04000844 RID: 2116
		BitsPerSampleInvalid,
		// Token: 0x04000845 RID: 2117
		DataBlockSizeInvalid,
		// Token: 0x04000846 RID: 2118
		NotWholeNumberBlocks,
		// Token: 0x04000847 RID: 2119
		BlockSignatureInvalid,
		// Token: 0x04000848 RID: 2120
		NumberOfSamplesInvalid,
		// Token: 0x04000849 RID: 2121
		SapiErrorUninitialized,
		// Token: 0x0400084A RID: 2122
		SapiErrorAlreadyInitialized,
		// Token: 0x0400084B RID: 2123
		SapiErrorNotSupportedFormat,
		// Token: 0x0400084C RID: 2124
		SapiErrorInvalidFlags,
		// Token: 0x0400084D RID: 2125
		SapiErrorEndOfStream,
		// Token: 0x0400084E RID: 2126
		SapiErrorDeviceBusy,
		// Token: 0x0400084F RID: 2127
		SapiErrorDeviceNotSupported,
		// Token: 0x04000850 RID: 2128
		SapiErrorDeviceNotEnabled,
		// Token: 0x04000851 RID: 2129
		SapiErrorNoDriver,
		// Token: 0x04000852 RID: 2130
		SapiErrorFileMustBeUnicode,
		// Token: 0x04000853 RID: 2131
		InsufficientData,
		// Token: 0x04000854 RID: 2132
		SapiErrorInvalidPhraseID,
		// Token: 0x04000855 RID: 2133
		SapiErrorBufferTooSmall,
		// Token: 0x04000856 RID: 2134
		SapiErrorFormatNotSpecified,
		// Token: 0x04000857 RID: 2135
		SapiErrorAudioStopped0,
		// Token: 0x04000858 RID: 2136
		AudioPaused,
		// Token: 0x04000859 RID: 2137
		SapiErrorRuleNotFound,
		// Token: 0x0400085A RID: 2138
		SapiErrorTTSEngineException,
		// Token: 0x0400085B RID: 2139
		SapiErrorTTSNLPException,
		// Token: 0x0400085C RID: 2140
		SapiErrorEngineBUSY,
		// Token: 0x0400085D RID: 2141
		AudioConversionEnabled,
		// Token: 0x0400085E RID: 2142
		NoHypothesisAvailable,
		// Token: 0x0400085F RID: 2143
		SapiErrorCantCreate,
		// Token: 0x04000860 RID: 2144
		AlreadyInLex,
		// Token: 0x04000861 RID: 2145
		SapiErrorNotInLex,
		// Token: 0x04000862 RID: 2146
		LexNothingToSync,
		// Token: 0x04000863 RID: 2147
		SapiErrorLexVeryOutOfSync,
		// Token: 0x04000864 RID: 2148
		SapiErrorUndefinedForwardRuleRef,
		// Token: 0x04000865 RID: 2149
		SapiErrorEmptyRule,
		// Token: 0x04000866 RID: 2150
		SapiErrorGrammarCompilerInternalError,
		// Token: 0x04000867 RID: 2151
		SapiErrorRuleNotDynamic,
		// Token: 0x04000868 RID: 2152
		SapiErrorDuplicateRuleName,
		// Token: 0x04000869 RID: 2153
		SapiErrorDuplicateResourceName,
		// Token: 0x0400086A RID: 2154
		SapiErrorTooManyGrammars,
		// Token: 0x0400086B RID: 2155
		SapiErrorCircularReference,
		// Token: 0x0400086C RID: 2156
		SapiErrorInvalidImport,
		// Token: 0x0400086D RID: 2157
		SapiErrorInvalidWAVFile,
		// Token: 0x0400086E RID: 2158
		RequestPending,
		// Token: 0x0400086F RID: 2159
		SapiErrorAllWordsOptional,
		// Token: 0x04000870 RID: 2160
		SapiErrorInstanceChangeInvalid,
		// Token: 0x04000871 RID: 2161
		SapiErrorRuleNameIdConflict,
		// Token: 0x04000872 RID: 2162
		SapiErrorNoRules,
		// Token: 0x04000873 RID: 2163
		SapiErrorCircularRuleRef,
		// Token: 0x04000874 RID: 2164
		NoParseFound,
		// Token: 0x04000875 RID: 2165
		SapiErrorInvalidHandle,
		// Token: 0x04000876 RID: 2166
		SapiErrorRemoteCallTimedout,
		// Token: 0x04000877 RID: 2167
		SapiErrorAudioBufferOverflow,
		// Token: 0x04000878 RID: 2168
		SapiErrorNoAudioData,
		// Token: 0x04000879 RID: 2169
		SapiErrorDeadAlternate,
		// Token: 0x0400087A RID: 2170
		SapiErrorHighLowConfidence,
		// Token: 0x0400087B RID: 2171
		SapiErrorInvalidFormatString,
		// Token: 0x0400087C RID: 2172
		SPNotSupportedOnStreamInput,
		// Token: 0x0400087D RID: 2173
		SapiErrorAppLexReadOnly,
		// Token: 0x0400087E RID: 2174
		SapiErrorNoTerminatingRulePath,
		// Token: 0x0400087F RID: 2175
		WordExistsWithoutPronunciation,
		// Token: 0x04000880 RID: 2176
		SapiErrorStreamClosed,
		// Token: 0x04000881 RID: 2177
		SapiErrorNoMoreItems,
		// Token: 0x04000882 RID: 2178
		SapiErrorNotFound,
		// Token: 0x04000883 RID: 2179
		SapiErrorInvalidAudioState,
		// Token: 0x04000884 RID: 2180
		SapiErrorGenericMMSYS,
		// Token: 0x04000885 RID: 2181
		SapiErrorMarshalerException,
		// Token: 0x04000886 RID: 2182
		SapiErrorNotDynamicGrammar,
		// Token: 0x04000887 RID: 2183
		SapiErrorAmbiguousProperty,
		// Token: 0x04000888 RID: 2184
		SapiErrorInvalidRegistrykey,
		// Token: 0x04000889 RID: 2185
		SapiErrorInvalidTokenId,
		// Token: 0x0400088A RID: 2186
		SapiErrorXMLBadSyntax,
		// Token: 0x0400088B RID: 2187
		SapiErrorXMLResourceNotFound,
		// Token: 0x0400088C RID: 2188
		SapiErrorTokenInUse,
		// Token: 0x0400088D RID: 2189
		SapiErrorTokenDeleted,
		// Token: 0x0400088E RID: 2190
		SapiErrorMultilingualNotSupported,
		// Token: 0x0400088F RID: 2191
		SapiErrorExportDynamicRule,
		// Token: 0x04000890 RID: 2192
		SapiErrorSTGF,
		// Token: 0x04000891 RID: 2193
		SapiErrorWordFormat,
		// Token: 0x04000892 RID: 2194
		SapiErrorStreamNotActive,
		// Token: 0x04000893 RID: 2195
		SapiErrorEngineResponseInvalid,
		// Token: 0x04000894 RID: 2196
		SapiErrorSREngineException,
		// Token: 0x04000895 RID: 2197
		SapiErrorStreamPosInvalid,
		// Token: 0x04000896 RID: 2198
		SapiErrorRecognizerInactive,
		// Token: 0x04000897 RID: 2199
		SapiErrorRemoteCallOnWrongThread,
		// Token: 0x04000898 RID: 2200
		SapiErrorRemoteProcessTerminated,
		// Token: 0x04000899 RID: 2201
		SapiErrorRemoteProcessAlreadyRunning,
		// Token: 0x0400089A RID: 2202
		SapiErrorLangIdMismatch,
		// Token: 0x0400089B RID: 2203
		SapiErrorPartialParseFound,
		// Token: 0x0400089C RID: 2204
		SapiErrorNotTopLevelRule,
		// Token: 0x0400089D RID: 2205
		SapiErrorNoRuleActive,
		// Token: 0x0400089E RID: 2206
		SapiErrorLexRequiresCookie,
		// Token: 0x0400089F RID: 2207
		SapiErrorStreamUninitialized,
		// Token: 0x040008A0 RID: 2208
		SapiErrorUnused0,
		// Token: 0x040008A1 RID: 2209
		SapiErrorNotSupportedLang,
		// Token: 0x040008A2 RID: 2210
		SapiErrorVoicePaused,
		// Token: 0x040008A3 RID: 2211
		SapiErrorAudioBufferUnderflow,
		// Token: 0x040008A4 RID: 2212
		SapiErrorAudioStoppedUnexpectedly,
		// Token: 0x040008A5 RID: 2213
		SapiErrorNoWordPronunciation,
		// Token: 0x040008A6 RID: 2214
		SapiErrorAlternatesWouldBeInconsistent,
		// Token: 0x040008A7 RID: 2215
		SapiErrorNotSupportedForSharedRecognizer,
		// Token: 0x040008A8 RID: 2216
		SapiErrorTimeOut,
		// Token: 0x040008A9 RID: 2217
		SapiErrorReenterSynchronize,
		// Token: 0x040008AA RID: 2218
		SapiErrorStateWithNoArcs,
		// Token: 0x040008AB RID: 2219
		SapiErrorNotActiveSession,
		// Token: 0x040008AC RID: 2220
		SapiErrorAlreadyDeleted,
		// Token: 0x040008AD RID: 2221
		SapiErrorAudioStopped,
		// Token: 0x040008AE RID: 2222
		SapiErrorRecoXMLGenerationFail,
		// Token: 0x040008AF RID: 2223
		SapiErrorSMLGenerationFail,
		// Token: 0x040008B0 RID: 2224
		SapiErrorNotPromptVoice,
		// Token: 0x040008B1 RID: 2225
		SapiErrorRootRuleAlreadyDefined,
		// Token: 0x040008B2 RID: 2226
		SapiErrorUnused1,
		// Token: 0x040008B3 RID: 2227
		SapiErrorUnused2,
		// Token: 0x040008B4 RID: 2228
		SapiErrorUnused3,
		// Token: 0x040008B5 RID: 2229
		SapiErrorUnused4,
		// Token: 0x040008B6 RID: 2230
		SapiErrorUnused5,
		// Token: 0x040008B7 RID: 2231
		SapiErrorUnused6,
		// Token: 0x040008B8 RID: 2232
		SapiErrorScriptDisallowed,
		// Token: 0x040008B9 RID: 2233
		SapiErrorRemoteCallTimedOutStart,
		// Token: 0x040008BA RID: 2234
		SapiErrorRemoteCallTimedOutConnect,
		// Token: 0x040008BB RID: 2235
		SapiErrorSecMgrChangeNotAllowed,
		// Token: 0x040008BC RID: 2236
		SapiErrorCompleteButExtendable,
		// Token: 0x040008BD RID: 2237
		SapiErrorFailedToDeleteFile,
		// Token: 0x040008BE RID: 2238
		SapiErrorSharedEngineDisabled,
		// Token: 0x040008BF RID: 2239
		SapiErrorRecognizerNotFound,
		// Token: 0x040008C0 RID: 2240
		SapiErrorAudioNotFound,
		// Token: 0x040008C1 RID: 2241
		SapiErrorNoVowel,
		// Token: 0x040008C2 RID: 2242
		SapiErrorNotSupportedPhoneme,
		// Token: 0x040008C3 RID: 2243
		SapiErrorNoRulesToActivate,
		// Token: 0x040008C4 RID: 2244
		SapiErrorNoWordEntryNotification,
		// Token: 0x040008C5 RID: 2245
		SapiErrorWordNeedsNormalization,
		// Token: 0x040008C6 RID: 2246
		SapiErrorCannotNormalize,
		// Token: 0x040008C7 RID: 2247
		LimitReached,
		// Token: 0x040008C8 RID: 2248
		NotSupported,
		// Token: 0x040008C9 RID: 2249
		SapiErrorTopicNotADaptable,
		// Token: 0x040008CA RID: 2250
		SapiErrorPhonemeConversion,
		// Token: 0x040008CB RID: 2251
		SapiErrorNotSupportedForInprocRecognizer
	}
}
