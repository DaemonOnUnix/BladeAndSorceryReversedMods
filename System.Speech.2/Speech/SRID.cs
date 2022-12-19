using System;

namespace System.Speech
{
	// Token: 0x02000003 RID: 3
	internal enum SRID
	{
		// Token: 0x04000002 RID: 2
		NullParamIllegal,
		// Token: 0x04000003 RID: 3
		ArrayOfNullIllegal,
		// Token: 0x04000004 RID: 4
		ParamsEntryNullIllegal,
		// Token: 0x04000005 RID: 5
		Unavailable,
		// Token: 0x04000006 RID: 6
		UnexpectedError,
		// Token: 0x04000007 RID: 7
		CollectionReadOnly,
		// Token: 0x04000008 RID: 8
		StringCanNotBeEmpty,
		// Token: 0x04000009 RID: 9
		EnumInvalid,
		// Token: 0x0400000A RID: 10
		NotSupportedWithThisVersionOfSAPI,
		// Token: 0x0400000B RID: 11
		NotSupportedWithThisVersionOfSAPI2,
		// Token: 0x0400000C RID: 12
		NotSupportedWithThisVersionOfSAPIBaseUri,
		// Token: 0x0400000D RID: 13
		NotSupportedWithThisVersionOfSAPITagFormat,
		// Token: 0x0400000E RID: 14
		NotSupportedWithThisVersionOfSAPICompareOption,
		// Token: 0x0400000F RID: 15
		MustBeGreaterThanZero,
		// Token: 0x04000010 RID: 16
		InvalidXml,
		// Token: 0x04000011 RID: 17
		OperationAborted,
		// Token: 0x04000012 RID: 18
		InvariantCultureInfo,
		// Token: 0x04000013 RID: 19
		DuplicatedEntry,
		// Token: 0x04000014 RID: 20
		StreamMustBeReadable,
		// Token: 0x04000015 RID: 21
		StreamMustBeWriteable,
		// Token: 0x04000016 RID: 22
		StreamMustBeSeekable,
		// Token: 0x04000017 RID: 23
		StreamEndedUnexpectedly,
		// Token: 0x04000018 RID: 24
		CannotReadFromDirectory,
		// Token: 0x04000019 RID: 25
		UnknownMimeFormat,
		// Token: 0x0400001A RID: 26
		CannotLoadResourceFromManifest,
		// Token: 0x0400001B RID: 27
		TokenInUse,
		// Token: 0x0400001C RID: 28
		TokenDeleted,
		// Token: 0x0400001D RID: 29
		TokenUninitialized,
		// Token: 0x0400001E RID: 30
		InvalidTokenId,
		// Token: 0x0400001F RID: 31
		NotFound,
		// Token: 0x04000020 RID: 32
		NoBackSlash,
		// Token: 0x04000021 RID: 33
		InvalidRegistryEntry,
		// Token: 0x04000022 RID: 34
		TokenCannotCreateInstance,
		// Token: 0x04000023 RID: 35
		InvalidXmlFormat,
		// Token: 0x04000024 RID: 36
		IncorrectAttributeValue,
		// Token: 0x04000025 RID: 37
		MissingRequiredAttribute,
		// Token: 0x04000026 RID: 38
		InvalidRuleRefSelf,
		// Token: 0x04000027 RID: 39
		InvalidDynamicExport,
		// Token: 0x04000028 RID: 40
		InvalidToken,
		// Token: 0x04000029 RID: 41
		MetaNameHTTPEquiv,
		// Token: 0x0400002A RID: 42
		EmptyRule,
		// Token: 0x0400002B RID: 43
		InvalidTokenString,
		// Token: 0x0400002C RID: 44
		InvalidQuotedString,
		// Token: 0x0400002D RID: 45
		ExportDynamicRule,
		// Token: 0x0400002E RID: 46
		EmptyDisplayString,
		// Token: 0x0400002F RID: 47
		EmptyPronunciationString,
		// Token: 0x04000030 RID: 48
		InvalidPhoneme,
		// Token: 0x04000031 RID: 49
		MuliplePronunciationString,
		// Token: 0x04000032 RID: 50
		MultipleDisplayString,
		// Token: 0x04000033 RID: 51
		RuleRedefinition,
		// Token: 0x04000034 RID: 52
		EmptyOneOf,
		// Token: 0x04000035 RID: 53
		InvalidGrammarOrdering,
		// Token: 0x04000036 RID: 54
		MinMaxOutOfRange,
		// Token: 0x04000037 RID: 55
		InvalidExampleOrdering,
		// Token: 0x04000038 RID: 56
		GrammarDefTwice,
		// Token: 0x04000039 RID: 57
		UnsupportedFormat,
		// Token: 0x0400003A RID: 58
		InvalidImport,
		// Token: 0x0400003B RID: 59
		DuplicatedRuleName,
		// Token: 0x0400003C RID: 60
		RootRuleAlreadyDefined,
		// Token: 0x0400003D RID: 61
		RuleNameIdConflict,
		// Token: 0x0400003E RID: 62
		RuleNotDynamic,
		// Token: 0x0400003F RID: 63
		StateWithNoArcs,
		// Token: 0x04000040 RID: 64
		NoTerminatingRulePath,
		// Token: 0x04000041 RID: 65
		RuleRefNoUri,
		// Token: 0x04000042 RID: 66
		UnavailableProperty,
		// Token: 0x04000043 RID: 67
		MinGreaterThanMax,
		// Token: 0x04000044 RID: 68
		ReqConfidenceNotSupported,
		// Token: 0x04000045 RID: 69
		SapiPropertiesAndSemantics,
		// Token: 0x04000046 RID: 70
		InvalidAttributeDefinedTwice,
		// Token: 0x04000047 RID: 71
		GrammarCompilerError,
		// Token: 0x04000048 RID: 72
		RuleScriptNotFound,
		// Token: 0x04000049 RID: 73
		DynamicRuleNotFound,
		// Token: 0x0400004A RID: 74
		RuleScriptInvalidParameters,
		// Token: 0x0400004B RID: 75
		RuleScriptInvalidReturnType,
		// Token: 0x0400004C RID: 76
		NoClassname,
		// Token: 0x0400004D RID: 77
		EmbeddedClassLibraryFailed,
		// Token: 0x0400004E RID: 78
		CannotFindClass,
		// Token: 0x0400004F RID: 79
		StrongTypedGrammarNotAGrammar,
		// Token: 0x04000050 RID: 80
		NoScriptsForRules,
		// Token: 0x04000051 RID: 81
		ClassNotPublic,
		// Token: 0x04000052 RID: 82
		MethodNotPublic,
		// Token: 0x04000053 RID: 83
		IncompatibleLanguageProperties,
		// Token: 0x04000054 RID: 84
		IncompatibleNamespaceProperties,
		// Token: 0x04000055 RID: 85
		IncompatibleDebugProperties,
		// Token: 0x04000056 RID: 86
		CannotLoadDotNetSemanticCode,
		// Token: 0x04000057 RID: 87
		InvalidSemanticProcessingType,
		// Token: 0x04000058 RID: 88
		InvalidScriptDefinition,
		// Token: 0x04000059 RID: 89
		InvalidMethodName,
		// Token: 0x0400005A RID: 90
		ConstructorNotAllowed,
		// Token: 0x0400005B RID: 91
		OverloadNotAllowed,
		// Token: 0x0400005C RID: 92
		OnInitOnPublicRule,
		// Token: 0x0400005D RID: 93
		ArgumentMismatch,
		// Token: 0x0400005E RID: 94
		CantGetPropertyFromSerializedInfo,
		// Token: 0x0400005F RID: 95
		CantFindAConstructor,
		// Token: 0x04000060 RID: 96
		TooManyArcs,
		// Token: 0x04000061 RID: 97
		TooManyRulesWithSemanticsGlobals,
		// Token: 0x04000062 RID: 98
		MaxTransitionsCount,
		// Token: 0x04000063 RID: 99
		UnknownElement,
		// Token: 0x04000064 RID: 100
		CircularRuleRef,
		// Token: 0x04000065 RID: 101
		InvalidFlagsSet,
		// Token: 0x04000066 RID: 102
		RuleDefinedMultipleTimes,
		// Token: 0x04000067 RID: 103
		RuleDefinedMultipleTimes2,
		// Token: 0x04000068 RID: 104
		RuleNotDefined,
		// Token: 0x04000069 RID: 105
		RootNotDefined,
		// Token: 0x0400006A RID: 106
		InvalidLanguage,
		// Token: 0x0400006B RID: 107
		InvalidRuleId,
		// Token: 0x0400006C RID: 108
		InvalidRepeatProbability,
		// Token: 0x0400006D RID: 109
		InvalidConfidence,
		// Token: 0x0400006E RID: 110
		InvalidMinRepeat,
		// Token: 0x0400006F RID: 111
		InvalidMaxRepeat,
		// Token: 0x04000070 RID: 112
		InvalidWeight,
		// Token: 0x04000071 RID: 113
		InvalidName,
		// Token: 0x04000072 RID: 114
		InvalidValueType,
		// Token: 0x04000073 RID: 115
		TagFormatNotSet,
		// Token: 0x04000074 RID: 116
		NoName,
		// Token: 0x04000075 RID: 117
		NoName1,
		// Token: 0x04000076 RID: 118
		InvalidSpecialRuleRef,
		// Token: 0x04000077 RID: 119
		InvalidRuleRef,
		// Token: 0x04000078 RID: 120
		InvalidNotEmptyElement,
		// Token: 0x04000079 RID: 121
		InvalidEmptyElement,
		// Token: 0x0400007A RID: 122
		InvalidEmptyRule,
		// Token: 0x0400007B RID: 123
		UndefRuleRef,
		// Token: 0x0400007C RID: 124
		UnsupportedLanguage,
		// Token: 0x0400007D RID: 125
		UnsupportedPhoneticAlphabet,
		// Token: 0x0400007E RID: 126
		UnsupportedLexicon,
		// Token: 0x0400007F RID: 127
		InvalidScriptAttribute,
		// Token: 0x04000080 RID: 128
		NoLanguageSet,
		// Token: 0x04000081 RID: 129
		MethodAttributeDefinedMultipeTimes,
		// Token: 0x04000082 RID: 130
		RuleAttributeDefinedMultipeTimes,
		// Token: 0x04000083 RID: 131
		InvalidAssemblyReferenceAttribute,
		// Token: 0x04000084 RID: 132
		InvalidImportNamespaceAttribute,
		// Token: 0x04000085 RID: 133
		NoUriForSpecialRuleRef,
		// Token: 0x04000086 RID: 134
		NoAliasForSpecialRuleRef,
		// Token: 0x04000087 RID: 135
		NoSmlData,
		// Token: 0x04000088 RID: 136
		InvalidNameValueProperty,
		// Token: 0x04000089 RID: 137
		InvalidTagInAnEmptyItem,
		// Token: 0x0400008A RID: 138
		InvalidSrgs,
		// Token: 0x0400008B RID: 139
		InvalidSrgsNamespace,
		// Token: 0x0400008C RID: 140
		Line,
		// Token: 0x0400008D RID: 141
		Position,
		// Token: 0x0400008E RID: 142
		InvalidVersion,
		// Token: 0x0400008F RID: 143
		InvalidTagFormat,
		// Token: 0x04000090 RID: 144
		MissingTagFormat,
		// Token: 0x04000091 RID: 145
		InvalidGrammarMode,
		// Token: 0x04000092 RID: 146
		InvalidGrammarAttribute,
		// Token: 0x04000093 RID: 147
		InvalidRuleAttribute,
		// Token: 0x04000094 RID: 148
		InvalidRulerefAttribute,
		// Token: 0x04000095 RID: 149
		InvalidOneOfAttribute,
		// Token: 0x04000096 RID: 150
		InvalidItemAttribute,
		// Token: 0x04000097 RID: 151
		InvalidTokenAttribute,
		// Token: 0x04000098 RID: 152
		InvalidItemRepeatAttribute,
		// Token: 0x04000099 RID: 153
		InvalidReqConfAttribute,
		// Token: 0x0400009A RID: 154
		InvalidTagAttribute,
		// Token: 0x0400009B RID: 155
		InvalidLexiconAttribute,
		// Token: 0x0400009C RID: 156
		InvalidMetaAttribute,
		// Token: 0x0400009D RID: 157
		InvalidItemAttribute2,
		// Token: 0x0400009E RID: 158
		InvalidElement,
		// Token: 0x0400009F RID: 159
		InvalidRuleScope,
		// Token: 0x040000A0 RID: 160
		InvalidDynamicSetting,
		// Token: 0x040000A1 RID: 161
		InvalidSubsetAttribute,
		// Token: 0x040000A2 RID: 162
		InvalidVoiceElementInPromptOutput,
		// Token: 0x040000A3 RID: 163
		NoRuleId,
		// Token: 0x040000A4 RID: 164
		PromptBuilderInvalideState,
		// Token: 0x040000A5 RID: 165
		PromptBuilderStateEnded,
		// Token: 0x040000A6 RID: 166
		PromptBuilderStateSentence,
		// Token: 0x040000A7 RID: 167
		PromptBuilderStateParagraph,
		// Token: 0x040000A8 RID: 168
		PromptBuilderStateVoice,
		// Token: 0x040000A9 RID: 169
		PromptBuilderStateStyle,
		// Token: 0x040000AA RID: 170
		PromptBuilderAgeOutOfRange,
		// Token: 0x040000AB RID: 171
		PromptBuilderMismatchStyle,
		// Token: 0x040000AC RID: 172
		PromptBuilderMismatchVoice,
		// Token: 0x040000AD RID: 173
		PromptBuilderMismatchParagraph,
		// Token: 0x040000AE RID: 174
		PromptBuilderMismatchSentence,
		// Token: 0x040000AF RID: 175
		PromptBuilderNestedParagraph,
		// Token: 0x040000B0 RID: 176
		PromptBuilderNestedSentence,
		// Token: 0x040000B1 RID: 177
		PromptBuilderInvalidAttribute,
		// Token: 0x040000B2 RID: 178
		PromptBuilderInvalidElement,
		// Token: 0x040000B3 RID: 179
		PromptBuilderInvalidVariant,
		// Token: 0x040000B4 RID: 180
		PromptBuilderDatabaseName,
		// Token: 0x040000B5 RID: 181
		PromptAsyncOperationCancelled,
		// Token: 0x040000B6 RID: 182
		SynthesizerPauseResumeMismatched,
		// Token: 0x040000B7 RID: 183
		SynthesizerInvalidMediaType,
		// Token: 0x040000B8 RID: 184
		SynthesizerUnknownMediaType,
		// Token: 0x040000B9 RID: 185
		SynthesizerSpeakError,
		// Token: 0x040000BA RID: 186
		SynthesizerInvalidWaveFile,
		// Token: 0x040000BB RID: 187
		SynthesizerPromptInUse,
		// Token: 0x040000BC RID: 188
		SynthesizerUnknownPriority,
		// Token: 0x040000BD RID: 189
		SynthesizerUnknownEvent,
		// Token: 0x040000BE RID: 190
		SynthesizerVoiceFailed,
		// Token: 0x040000BF RID: 191
		SynthesizerSetVoiceNoMatch,
		// Token: 0x040000C0 RID: 192
		SynthesizerNoCulture,
		// Token: 0x040000C1 RID: 193
		SynthesizerSyncSpeakWhilePaused,
		// Token: 0x040000C2 RID: 194
		SynthesizerSyncSetOutputWhilePaused,
		// Token: 0x040000C3 RID: 195
		SynthesizerNoCulture2,
		// Token: 0x040000C4 RID: 196
		SynthesizerNoSpeak,
		// Token: 0x040000C5 RID: 197
		SynthesizerSetOutputSpeaking,
		// Token: 0x040000C6 RID: 198
		InvalidSpeakAttribute,
		// Token: 0x040000C7 RID: 199
		UnsupportedAlphabet,
		// Token: 0x040000C8 RID: 200
		GrammarInvalidWeight,
		// Token: 0x040000C9 RID: 201
		GrammarInvalidPriority,
		// Token: 0x040000CA RID: 202
		DictationInvalidTopic,
		// Token: 0x040000CB RID: 203
		DictationTopicNotFound,
		// Token: 0x040000CC RID: 204
		RecognizerGrammarNotFound,
		// Token: 0x040000CD RID: 205
		RecognizerRuleNotFound,
		// Token: 0x040000CE RID: 206
		RecognizerInvalidBinaryGrammar,
		// Token: 0x040000CF RID: 207
		RecognizerRuleNotFoundStream,
		// Token: 0x040000D0 RID: 208
		RecognizerNoRootRuleToActivate,
		// Token: 0x040000D1 RID: 209
		RecognizerNoRootRuleToActivate1,
		// Token: 0x040000D2 RID: 210
		RecognizerRuleActivationFailed,
		// Token: 0x040000D3 RID: 211
		RecognizerAlreadyRecognizing,
		// Token: 0x040000D4 RID: 212
		RecognizerHasNoGrammar,
		// Token: 0x040000D5 RID: 213
		NegativeTimesNotSupported,
		// Token: 0x040000D6 RID: 214
		AudioDeviceFormatError,
		// Token: 0x040000D7 RID: 215
		AudioDeviceError,
		// Token: 0x040000D8 RID: 216
		AudioDeviceInternalError,
		// Token: 0x040000D9 RID: 217
		RecognizerNotFound,
		// Token: 0x040000DA RID: 218
		RecognizerNotEnabled,
		// Token: 0x040000DB RID: 219
		RecognitionNotSupported,
		// Token: 0x040000DC RID: 220
		RecognitionNotSupportedOn64bit,
		// Token: 0x040000DD RID: 221
		GrammarAlreadyLoaded,
		// Token: 0x040000DE RID: 222
		RecognizerNoInputSource,
		// Token: 0x040000DF RID: 223
		GrammarNotLoaded,
		// Token: 0x040000E0 RID: 224
		GrammarLoadingInProgress,
		// Token: 0x040000E1 RID: 225
		GrammarLoadFailed,
		// Token: 0x040000E2 RID: 226
		GrammarWrongRecognizer,
		// Token: 0x040000E3 RID: 227
		NotSupportedOnDictationGrammars,
		// Token: 0x040000E4 RID: 228
		LocalFilesOnly,
		// Token: 0x040000E5 RID: 229
		NotValidAudioFile,
		// Token: 0x040000E6 RID: 230
		NotValidAudioStream,
		// Token: 0x040000E7 RID: 231
		FileNotFound,
		// Token: 0x040000E8 RID: 232
		CannotSetPriorityOnDictation,
		// Token: 0x040000E9 RID: 233
		RecognizerUpdateTableTooLarge,
		// Token: 0x040000EA RID: 234
		MaxAlternatesInvalid,
		// Token: 0x040000EB RID: 235
		RecognizerSettingGetError,
		// Token: 0x040000EC RID: 236
		RecognizerSettingUpdateError,
		// Token: 0x040000ED RID: 237
		RecognizerSettingNotSupported,
		// Token: 0x040000EE RID: 238
		ResourceUsageOutOfRange,
		// Token: 0x040000EF RID: 239
		RateOutOfRange,
		// Token: 0x040000F0 RID: 240
		EndSilenceOutOfRange,
		// Token: 0x040000F1 RID: 241
		RejectionThresholdOutOfRange,
		// Token: 0x040000F2 RID: 242
		ReferencedGrammarNotFound,
		// Token: 0x040000F3 RID: 243
		SapiErrorRuleNotFound2,
		// Token: 0x040000F4 RID: 244
		NoAudioAvailable,
		// Token: 0x040000F5 RID: 245
		ResultNotGrammarAvailable,
		// Token: 0x040000F6 RID: 246
		ResultInvalidFormat,
		// Token: 0x040000F7 RID: 247
		UnhandledVariant,
		// Token: 0x040000F8 RID: 248
		DupSemanticKey,
		// Token: 0x040000F9 RID: 249
		DupSemanticValue,
		// Token: 0x040000FA RID: 250
		CannotUseCustomFormat,
		// Token: 0x040000FB RID: 251
		NoPromptEngine,
		// Token: 0x040000FC RID: 252
		NoPromptEngineInterface,
		// Token: 0x040000FD RID: 253
		SeekNotSupported,
		// Token: 0x040000FE RID: 254
		ExtraDataNotPresent,
		// Token: 0x040000FF RID: 255
		BitsPerSampleInvalid,
		// Token: 0x04000100 RID: 256
		DataBlockSizeInvalid,
		// Token: 0x04000101 RID: 257
		NotWholeNumberBlocks,
		// Token: 0x04000102 RID: 258
		BlockSignatureInvalid,
		// Token: 0x04000103 RID: 259
		NumberOfSamplesInvalid,
		// Token: 0x04000104 RID: 260
		SapiErrorUninitialized,
		// Token: 0x04000105 RID: 261
		SapiErrorAlreadyInitialized,
		// Token: 0x04000106 RID: 262
		SapiErrorNotSupportedFormat,
		// Token: 0x04000107 RID: 263
		SapiErrorInvalidFlags,
		// Token: 0x04000108 RID: 264
		SapiErrorEndOfStream,
		// Token: 0x04000109 RID: 265
		SapiErrorDeviceBusy,
		// Token: 0x0400010A RID: 266
		SapiErrorDeviceNotSupported,
		// Token: 0x0400010B RID: 267
		SapiErrorDeviceNotEnabled,
		// Token: 0x0400010C RID: 268
		SapiErrorNoDriver,
		// Token: 0x0400010D RID: 269
		SapiErrorFileMustBeUnicode,
		// Token: 0x0400010E RID: 270
		InsufficientData,
		// Token: 0x0400010F RID: 271
		SapiErrorInvalidPhraseID,
		// Token: 0x04000110 RID: 272
		SapiErrorBufferTooSmall,
		// Token: 0x04000111 RID: 273
		SapiErrorFormatNotSpecified,
		// Token: 0x04000112 RID: 274
		SapiErrorAudioStopped0,
		// Token: 0x04000113 RID: 275
		AudioPaused,
		// Token: 0x04000114 RID: 276
		SapiErrorRuleNotFound,
		// Token: 0x04000115 RID: 277
		SapiErrorTTSEngineException,
		// Token: 0x04000116 RID: 278
		SapiErrorTTSNLPException,
		// Token: 0x04000117 RID: 279
		SapiErrorEngineBUSY,
		// Token: 0x04000118 RID: 280
		AudioConversionEnabled,
		// Token: 0x04000119 RID: 281
		NoHypothesisAvailable,
		// Token: 0x0400011A RID: 282
		SapiErrorCantCreate,
		// Token: 0x0400011B RID: 283
		AlreadyInLex,
		// Token: 0x0400011C RID: 284
		SapiErrorNotInLex,
		// Token: 0x0400011D RID: 285
		LexNothingToSync,
		// Token: 0x0400011E RID: 286
		SapiErrorLexVeryOutOfSync,
		// Token: 0x0400011F RID: 287
		SapiErrorUndefinedForwardRuleRef,
		// Token: 0x04000120 RID: 288
		SapiErrorEmptyRule,
		// Token: 0x04000121 RID: 289
		SapiErrorGrammarCompilerInternalError,
		// Token: 0x04000122 RID: 290
		SapiErrorRuleNotDynamic,
		// Token: 0x04000123 RID: 291
		SapiErrorDuplicateRuleName,
		// Token: 0x04000124 RID: 292
		SapiErrorDuplicateResourceName,
		// Token: 0x04000125 RID: 293
		SapiErrorTooManyGrammars,
		// Token: 0x04000126 RID: 294
		SapiErrorCircularReference,
		// Token: 0x04000127 RID: 295
		SapiErrorInvalidImport,
		// Token: 0x04000128 RID: 296
		SapiErrorInvalidWAVFile,
		// Token: 0x04000129 RID: 297
		RequestPending,
		// Token: 0x0400012A RID: 298
		SapiErrorAllWordsOptional,
		// Token: 0x0400012B RID: 299
		SapiErrorInstanceChangeInvalid,
		// Token: 0x0400012C RID: 300
		SapiErrorRuleNameIdConflict,
		// Token: 0x0400012D RID: 301
		SapiErrorNoRules,
		// Token: 0x0400012E RID: 302
		SapiErrorCircularRuleRef,
		// Token: 0x0400012F RID: 303
		NoParseFound,
		// Token: 0x04000130 RID: 304
		SapiErrorInvalidHandle,
		// Token: 0x04000131 RID: 305
		SapiErrorRemoteCallTimedout,
		// Token: 0x04000132 RID: 306
		SapiErrorAudioBufferOverflow,
		// Token: 0x04000133 RID: 307
		SapiErrorNoAudioData,
		// Token: 0x04000134 RID: 308
		SapiErrorDeadAlternate,
		// Token: 0x04000135 RID: 309
		SapiErrorHighLowConfidence,
		// Token: 0x04000136 RID: 310
		SapiErrorInvalidFormatString,
		// Token: 0x04000137 RID: 311
		SPNotSupportedOnStreamInput,
		// Token: 0x04000138 RID: 312
		SapiErrorAppLexReadOnly,
		// Token: 0x04000139 RID: 313
		SapiErrorNoTerminatingRulePath,
		// Token: 0x0400013A RID: 314
		WordExistsWithoutPronunciation,
		// Token: 0x0400013B RID: 315
		SapiErrorStreamClosed,
		// Token: 0x0400013C RID: 316
		SapiErrorNoMoreItems,
		// Token: 0x0400013D RID: 317
		SapiErrorNotFound,
		// Token: 0x0400013E RID: 318
		SapiErrorInvalidAudioState,
		// Token: 0x0400013F RID: 319
		SapiErrorGenericMMSYS,
		// Token: 0x04000140 RID: 320
		SapiErrorMarshalerException,
		// Token: 0x04000141 RID: 321
		SapiErrorNotDynamicGrammar,
		// Token: 0x04000142 RID: 322
		SapiErrorAmbiguousProperty,
		// Token: 0x04000143 RID: 323
		SapiErrorInvalidRegistrykey,
		// Token: 0x04000144 RID: 324
		SapiErrorInvalidTokenId,
		// Token: 0x04000145 RID: 325
		SapiErrorXMLBadSyntax,
		// Token: 0x04000146 RID: 326
		SapiErrorXMLResourceNotFound,
		// Token: 0x04000147 RID: 327
		SapiErrorTokenInUse,
		// Token: 0x04000148 RID: 328
		SapiErrorTokenDeleted,
		// Token: 0x04000149 RID: 329
		SapiErrorMultilingualNotSupported,
		// Token: 0x0400014A RID: 330
		SapiErrorExportDynamicRule,
		// Token: 0x0400014B RID: 331
		SapiErrorSTGF,
		// Token: 0x0400014C RID: 332
		SapiErrorWordFormat,
		// Token: 0x0400014D RID: 333
		SapiErrorStreamNotActive,
		// Token: 0x0400014E RID: 334
		SapiErrorEngineResponseInvalid,
		// Token: 0x0400014F RID: 335
		SapiErrorSREngineException,
		// Token: 0x04000150 RID: 336
		SapiErrorStreamPosInvalid,
		// Token: 0x04000151 RID: 337
		SapiErrorRecognizerInactive,
		// Token: 0x04000152 RID: 338
		SapiErrorRemoteCallOnWrongThread,
		// Token: 0x04000153 RID: 339
		SapiErrorRemoteProcessTerminated,
		// Token: 0x04000154 RID: 340
		SapiErrorRemoteProcessAlreadyRunning,
		// Token: 0x04000155 RID: 341
		SapiErrorLangIdMismatch,
		// Token: 0x04000156 RID: 342
		SapiErrorPartialParseFound,
		// Token: 0x04000157 RID: 343
		SapiErrorNotTopLevelRule,
		// Token: 0x04000158 RID: 344
		SapiErrorNoRuleActive,
		// Token: 0x04000159 RID: 345
		SapiErrorLexRequiresCookie,
		// Token: 0x0400015A RID: 346
		SapiErrorStreamUninitialized,
		// Token: 0x0400015B RID: 347
		SapiErrorUnused0,
		// Token: 0x0400015C RID: 348
		SapiErrorNotSupportedLang,
		// Token: 0x0400015D RID: 349
		SapiErrorVoicePaused,
		// Token: 0x0400015E RID: 350
		SapiErrorAudioBufferUnderflow,
		// Token: 0x0400015F RID: 351
		SapiErrorAudioStoppedUnexpectedly,
		// Token: 0x04000160 RID: 352
		SapiErrorNoWordPronunciation,
		// Token: 0x04000161 RID: 353
		SapiErrorAlternatesWouldBeInconsistent,
		// Token: 0x04000162 RID: 354
		SapiErrorNotSupportedForSharedRecognizer,
		// Token: 0x04000163 RID: 355
		SapiErrorTimeOut,
		// Token: 0x04000164 RID: 356
		SapiErrorReenterSynchronize,
		// Token: 0x04000165 RID: 357
		SapiErrorStateWithNoArcs,
		// Token: 0x04000166 RID: 358
		SapiErrorNotActiveSession,
		// Token: 0x04000167 RID: 359
		SapiErrorAlreadyDeleted,
		// Token: 0x04000168 RID: 360
		SapiErrorAudioStopped,
		// Token: 0x04000169 RID: 361
		SapiErrorRecoXMLGenerationFail,
		// Token: 0x0400016A RID: 362
		SapiErrorSMLGenerationFail,
		// Token: 0x0400016B RID: 363
		SapiErrorNotPromptVoice,
		// Token: 0x0400016C RID: 364
		SapiErrorRootRuleAlreadyDefined,
		// Token: 0x0400016D RID: 365
		SapiErrorUnused1,
		// Token: 0x0400016E RID: 366
		SapiErrorUnused2,
		// Token: 0x0400016F RID: 367
		SapiErrorUnused3,
		// Token: 0x04000170 RID: 368
		SapiErrorUnused4,
		// Token: 0x04000171 RID: 369
		SapiErrorUnused5,
		// Token: 0x04000172 RID: 370
		SapiErrorUnused6,
		// Token: 0x04000173 RID: 371
		SapiErrorScriptDisallowed,
		// Token: 0x04000174 RID: 372
		SapiErrorRemoteCallTimedOutStart,
		// Token: 0x04000175 RID: 373
		SapiErrorRemoteCallTimedOutConnect,
		// Token: 0x04000176 RID: 374
		SapiErrorSecMgrChangeNotAllowed,
		// Token: 0x04000177 RID: 375
		SapiErrorCompleteButExtendable,
		// Token: 0x04000178 RID: 376
		SapiErrorFailedToDeleteFile,
		// Token: 0x04000179 RID: 377
		SapiErrorSharedEngineDisabled,
		// Token: 0x0400017A RID: 378
		SapiErrorRecognizerNotFound,
		// Token: 0x0400017B RID: 379
		SapiErrorAudioNotFound,
		// Token: 0x0400017C RID: 380
		SapiErrorNoVowel,
		// Token: 0x0400017D RID: 381
		SapiErrorNotSupportedPhoneme,
		// Token: 0x0400017E RID: 382
		SapiErrorNoRulesToActivate,
		// Token: 0x0400017F RID: 383
		SapiErrorNoWordEntryNotification,
		// Token: 0x04000180 RID: 384
		SapiErrorWordNeedsNormalization,
		// Token: 0x04000181 RID: 385
		SapiErrorCannotNormalize,
		// Token: 0x04000182 RID: 386
		LimitReached,
		// Token: 0x04000183 RID: 387
		NotSupported,
		// Token: 0x04000184 RID: 388
		SapiErrorTopicNotADaptable,
		// Token: 0x04000185 RID: 389
		SapiErrorPhonemeConversion,
		// Token: 0x04000186 RID: 390
		SapiErrorNotSupportedForInprocRecognizer
	}
}
