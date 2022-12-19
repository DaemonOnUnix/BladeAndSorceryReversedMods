using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Speech.Internal.SrgsParser;

namespace System.Speech.Internal.SrgsCompiler
{
	// Token: 0x020000EA RID: 234
	internal sealed class CfgGrammar
	{
		// Token: 0x06000831 RID: 2097 RVA: 0x00003BF5 File Offset: 0x00001DF5
		internal CfgGrammar()
		{
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x00024B7C File Offset: 0x00022D7C
		internal static CfgGrammar.CfgHeader ConvertCfgHeader(StreamMarshaler streamHelper)
		{
			CfgGrammar.CfgSerializedHeader cfgSerializedHeader = null;
			return CfgGrammar.ConvertCfgHeader(streamHelper, true, true, out cfgSerializedHeader);
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x00024B98 File Offset: 0x00022D98
		internal static CfgGrammar.CfgHeader ConvertCfgHeader(StreamMarshaler streamHelper, bool includeAllGrammarData, bool loadSymbols, out CfgGrammar.CfgSerializedHeader cfgSerializedHeader)
		{
			cfgSerializedHeader = new CfgGrammar.CfgSerializedHeader(streamHelper.Stream);
			CfgGrammar.CfgHeader cfgHeader = default(CfgGrammar.CfgHeader);
			cfgHeader.FormatId = cfgSerializedHeader.FormatId;
			cfgHeader.GrammarGUID = cfgSerializedHeader.GrammarGUID;
			cfgHeader.langId = cfgSerializedHeader.LangID;
			cfgHeader.pszGlobalTags = cfgSerializedHeader.pszSemanticInterpretationGlobals;
			cfgHeader.cArcsInLargestState = cfgSerializedHeader.cArcsInLargestState;
			cfgHeader.rules = CfgGrammar.Load<CfgRule>(streamHelper, cfgSerializedHeader.pRules, cfgSerializedHeader.cRules);
			if (includeAllGrammarData || loadSymbols)
			{
				cfgHeader.pszSymbols = CfgGrammar.LoadStringBlob(streamHelper, cfgSerializedHeader.pszSymbols, cfgSerializedHeader.cchSymbols);
			}
			if (includeAllGrammarData)
			{
				cfgHeader.pszWords = CfgGrammar.LoadStringBlob(streamHelper, cfgSerializedHeader.pszWords, cfgSerializedHeader.cchWords);
				cfgHeader.arcs = CfgGrammar.Load<CfgArc>(streamHelper, cfgSerializedHeader.pArcs, cfgSerializedHeader.cArcs);
				cfgHeader.tags = CfgGrammar.Load<CfgSemanticTag>(streamHelper, cfgSerializedHeader.tags, cfgSerializedHeader.cTags);
				cfgHeader.weights = CfgGrammar.Load<float>(streamHelper, cfgSerializedHeader.pWeights, cfgSerializedHeader.cArcs);
			}
			if ((ulong)cfgSerializedHeader.pszWords < (ulong)((long)Marshal.SizeOf(typeof(CfgGrammar.CfgSerializedHeader))))
			{
				cfgHeader.ulRootRuleIndex = uint.MaxValue;
				cfgHeader.GrammarOptions = GrammarOptions.KeyValuePairs;
				cfgHeader.BasePath = null;
				cfgHeader.GrammarMode = GrammarType.VoiceGrammar;
			}
			else
			{
				cfgHeader.ulRootRuleIndex = cfgSerializedHeader.ulRootRuleIndex;
				cfgHeader.GrammarOptions = cfgSerializedHeader.GrammarOptions;
				cfgHeader.GrammarMode = (GrammarType)cfgSerializedHeader.GrammarMode;
				if (includeAllGrammarData)
				{
					cfgHeader.scripts = CfgGrammar.Load<CfgScriptRef>(streamHelper, cfgSerializedHeader.pScripts, cfgSerializedHeader.cScripts);
				}
				if (cfgSerializedHeader.cBasePath > 0U)
				{
					streamHelper.Stream.Position = (long)(cfgSerializedHeader.pRules + (uint)(cfgHeader.rules.Length * Marshal.SizeOf(typeof(CfgRule))));
					cfgHeader.BasePath = streamHelper.ReadNullTerminatedString();
				}
			}
			CfgGrammar.CheckValidCfgFormat(cfgSerializedHeader, cfgHeader, includeAllGrammarData);
			return cfgHeader;
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x00024D80 File Offset: 0x00022F80
		internal static ScriptRef[] LoadScriptRefs(StreamMarshaler streamHelper, CfgGrammar.CfgSerializedHeader pFH)
		{
			if (pFH.FormatId != CfgGrammar._SPGDF_ContextFree)
			{
				return null;
			}
			if ((ulong)pFH.pszWords < (ulong)((long)Marshal.SizeOf(typeof(CfgGrammar.CfgSerializedHeader))))
			{
				return null;
			}
			StringBlob stringBlob = CfgGrammar.LoadStringBlob(streamHelper, pFH.pszSymbols, pFH.cchSymbols);
			CfgScriptRef[] array = CfgGrammar.Load<CfgScriptRef>(streamHelper, pFH.pScripts, pFH.cScripts);
			ScriptRef[] array2 = new ScriptRef[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				CfgScriptRef cfgScriptRef = array[i];
				array2[i] = new ScriptRef(stringBlob[cfgScriptRef._idRule], stringBlob[cfgScriptRef._idMethod], cfgScriptRef._method);
			}
			return array2;
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x00024E2C File Offset: 0x0002302C
		internal static ScriptRef[] LoadIL(Stream stream)
		{
			ScriptRef[] array;
			using (StreamMarshaler streamMarshaler = new StreamMarshaler(stream))
			{
				CfgGrammar.CfgSerializedHeader cfgSerializedHeader = new CfgGrammar.CfgSerializedHeader();
				streamMarshaler.ReadStream(cfgSerializedHeader);
				array = CfgGrammar.LoadScriptRefs(streamMarshaler, cfgSerializedHeader);
			}
			return array;
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x00024E74 File Offset: 0x00023074
		internal static bool LoadIL(Stream stream, out byte[] assemblyContent, out byte[] assemblyDebugSymbols, out ScriptRef[] scripts)
		{
			byte[] array;
			assemblyDebugSymbols = (array = null);
			assemblyContent = array;
			scripts = null;
			using (StreamMarshaler streamMarshaler = new StreamMarshaler(stream))
			{
				CfgGrammar.CfgSerializedHeader cfgSerializedHeader = new CfgGrammar.CfgSerializedHeader();
				streamMarshaler.ReadStream(cfgSerializedHeader);
				scripts = CfgGrammar.LoadScriptRefs(streamMarshaler, cfgSerializedHeader);
				if (scripts == null)
				{
					return false;
				}
				if (cfgSerializedHeader.cIL == 0)
				{
					return false;
				}
				assemblyContent = CfgGrammar.Load<byte>(streamMarshaler, cfgSerializedHeader.pIL, cfgSerializedHeader.cIL);
				assemblyDebugSymbols = ((cfgSerializedHeader.cPDB > 0) ? CfgGrammar.Load<byte>(streamMarshaler, cfgSerializedHeader.pPDB, cfgSerializedHeader.cPDB) : null);
			}
			return true;
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x00024F10 File Offset: 0x00023110
		private static void CheckValidCfgFormat(CfgGrammar.CfgSerializedHeader pFH, CfgGrammar.CfgHeader header, bool includeAllGrammarData)
		{
			if (pFH.pszWords < 100U)
			{
				XmlParser.ThrowSrgsException(SRID.UnsupportedFormat, new object[0]);
			}
			int pszWords = (int)pFH.pszWords;
			CfgGrammar.CheckSetOffsets(pFH.pszWords, pFH.cchWords * 2, ref pszWords, pFH.ulTotalSerializedSize);
			CfgGrammar.CheckSetOffsets(pFH.pszSymbols, pFH.cchSymbols * 2, ref pszWords, pFH.ulTotalSerializedSize);
			if (pFH.cRules > 0)
			{
				CfgGrammar.CheckSetOffsets(pFH.pRules, pFH.cRules * Marshal.SizeOf(typeof(CfgRule)), ref pszWords, pFH.ulTotalSerializedSize);
			}
			if (pFH.cArcs > 0)
			{
				CfgGrammar.CheckSetOffsets(pFH.pArcs, pFH.cArcs * Marshal.SizeOf(typeof(CfgArc)), ref pszWords, pFH.ulTotalSerializedSize);
			}
			if (pFH.pWeights > 0U)
			{
				CfgGrammar.CheckSetOffsets(pFH.pWeights, pFH.cArcs * Marshal.SizeOf(typeof(float)), ref pszWords, pFH.ulTotalSerializedSize);
			}
			if (pFH.cTags > 0)
			{
				CfgGrammar.CheckSetOffsets(pFH.tags, pFH.cTags * Marshal.SizeOf(typeof(CfgSemanticTag)), ref pszWords, pFH.ulTotalSerializedSize);
				if (includeAllGrammarData)
				{
					for (int i = 0; i < header.tags.Length; i++)
					{
						int startArcIndex = (int)header.tags[i].StartArcIndex;
						int endArcIndex = (int)header.tags[i].EndArcIndex;
						int num = header.arcs.Length;
						if (startArcIndex == 0 || startArcIndex >= num || endArcIndex == 0 || endArcIndex >= num || (header.tags[i].PropVariantType != VarEnum.VT_EMPTY && header.tags[i].PropVariantType == VarEnum.VT_BSTR && header.tags[i].PropVariantType == VarEnum.VT_BOOL && header.tags[i].PropVariantType == VarEnum.VT_R8 && header.tags[i].PropVariantType == VarEnum.VT_I4))
						{
							XmlParser.ThrowSrgsException(SRID.UnsupportedFormat, new object[0]);
						}
					}
				}
			}
			if (pFH.cScripts > 0)
			{
				CfgGrammar.CheckSetOffsets(pFH.pScripts, pFH.cScripts * Marshal.SizeOf(typeof(CfgScriptRef)), ref pszWords, pFH.ulTotalSerializedSize);
			}
			if (pFH.cIL > 0)
			{
				CfgGrammar.CheckSetOffsets(pFH.pIL, pFH.cIL * Marshal.SizeOf(typeof(byte)), ref pszWords, pFH.ulTotalSerializedSize);
			}
			if (pFH.cPDB > 0)
			{
				CfgGrammar.CheckSetOffsets(pFH.pPDB, pFH.cPDB * Marshal.SizeOf(typeof(byte)), ref pszWords, pFH.ulTotalSerializedSize);
			}
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x000251A0 File Offset: 0x000233A0
		private static void CheckSetOffsets(uint offset, int size, ref int start, uint max)
		{
			if (offset < (uint)start || (start = (int)(offset + (uint)size)) > (int)max)
			{
				XmlParser.ThrowSrgsException(SRID.UnsupportedFormat, new object[0]);
			}
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x000251CC File Offset: 0x000233CC
		private static StringBlob LoadStringBlob(StreamMarshaler streamHelper, uint iPos, int c)
		{
			char[] array = new char[c];
			streamHelper.Position = iPos;
			streamHelper.ReadArrayChar(array, c);
			return new StringBlob(array);
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x000251F8 File Offset: 0x000233F8
		private static T[] Load<T>(StreamMarshaler streamHelper, uint iPos, int c)
		{
			T[] array = new T[c];
			if (c > 0)
			{
				streamHelper.Position = iPos;
				streamHelper.ReadArray<T>(array, c);
			}
			return array;
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x0600083B RID: 2107 RVA: 0x00025222 File Offset: 0x00023422
		internal static uint NextHandle
		{
			get
			{
				return CfgGrammar._lastHandle += 1U;
			}
		}

		// Token: 0x040005DA RID: 1498
		internal static Guid _SPGDF_ContextFree = new Guid(1306301037, 27879, 19904, 153, 167, 175, 158, 107, 106, 78, 145);

		// Token: 0x040005DB RID: 1499
		internal const int INFINITE = -1;

		// Token: 0x040005DC RID: 1500
		internal static readonly Rule SPRULETRANS_TEXTBUFFER = new Rule(-1);

		// Token: 0x040005DD RID: 1501
		internal static readonly Rule SPRULETRANS_WILDCARD = new Rule(-2);

		// Token: 0x040005DE RID: 1502
		internal static readonly Rule SPRULETRANS_DICTATION = new Rule(-3);

		// Token: 0x040005DF RID: 1503
		internal const int SPTEXTBUFFERTRANSITION = 4194303;

		// Token: 0x040005E0 RID: 1504
		internal const int SPWILDCARDTRANSITION = 4194302;

		// Token: 0x040005E1 RID: 1505
		internal const int SPDICTATIONTRANSITION = 4194301;

		// Token: 0x040005E2 RID: 1506
		internal const int MAX_TRANSITIONS_COUNT = 256;

		// Token: 0x040005E3 RID: 1507
		internal const float DEFAULT_WEIGHT = 1f;

		// Token: 0x040005E4 RID: 1508
		internal const int SP_LOW_CONFIDENCE = -1;

		// Token: 0x040005E5 RID: 1509
		internal const int SP_NORMAL_CONFIDENCE = 0;

		// Token: 0x040005E6 RID: 1510
		internal const int SP_HIGH_CONFIDENCE = 1;

		// Token: 0x040005E7 RID: 1511
		private const int SP_SPCFGSERIALIZEDHEADER_500 = 100;

		// Token: 0x040005E8 RID: 1512
		private static uint _lastHandle;

		// Token: 0x020001A2 RID: 418
		internal struct CfgHeader
		{
			// Token: 0x04000960 RID: 2400
			internal Guid FormatId;

			// Token: 0x04000961 RID: 2401
			internal Guid GrammarGUID;

			// Token: 0x04000962 RID: 2402
			internal ushort langId;

			// Token: 0x04000963 RID: 2403
			internal ushort pszGlobalTags;

			// Token: 0x04000964 RID: 2404
			internal int cArcsInLargestState;

			// Token: 0x04000965 RID: 2405
			internal StringBlob pszWords;

			// Token: 0x04000966 RID: 2406
			internal StringBlob pszSymbols;

			// Token: 0x04000967 RID: 2407
			internal CfgRule[] rules;

			// Token: 0x04000968 RID: 2408
			internal CfgArc[] arcs;

			// Token: 0x04000969 RID: 2409
			internal float[] weights;

			// Token: 0x0400096A RID: 2410
			internal CfgSemanticTag[] tags;

			// Token: 0x0400096B RID: 2411
			internal CfgScriptRef[] scripts;

			// Token: 0x0400096C RID: 2412
			internal uint ulRootRuleIndex;

			// Token: 0x0400096D RID: 2413
			internal GrammarOptions GrammarOptions;

			// Token: 0x0400096E RID: 2414
			internal GrammarType GrammarMode;

			// Token: 0x0400096F RID: 2415
			internal string BasePath;
		}

		// Token: 0x020001A3 RID: 419
		[StructLayout(LayoutKind.Sequential)]
		internal class CfgSerializedHeader
		{
			// Token: 0x06000BAB RID: 2987 RVA: 0x00003BF5 File Offset: 0x00001DF5
			internal CfgSerializedHeader()
			{
			}

			// Token: 0x06000BAC RID: 2988 RVA: 0x0002DE54 File Offset: 0x0002C054
			internal CfgSerializedHeader(Stream stream)
			{
				BinaryReader binaryReader = new BinaryReader(stream);
				this.ulTotalSerializedSize = binaryReader.ReadUInt32();
				if (this.ulTotalSerializedSize < 100U || this.ulTotalSerializedSize > 2147483647U)
				{
					XmlParser.ThrowSrgsException(SRID.UnsupportedFormat, new object[0]);
				}
				this.FormatId = new Guid(binaryReader.ReadBytes(16));
				if (this.FormatId != CfgGrammar._SPGDF_ContextFree)
				{
					XmlParser.ThrowSrgsException(SRID.UnsupportedFormat, new object[0]);
				}
				this.GrammarGUID = new Guid(binaryReader.ReadBytes(16));
				this.LangID = binaryReader.ReadUInt16();
				this.pszSemanticInterpretationGlobals = binaryReader.ReadUInt16();
				this.cArcsInLargestState = binaryReader.ReadInt32();
				this.cchWords = binaryReader.ReadInt32();
				this.cWords = binaryReader.ReadInt32();
				this.pszWords = binaryReader.ReadUInt32();
				if (this.pszWords < 100U || this.pszWords > this.ulTotalSerializedSize)
				{
					XmlParser.ThrowSrgsException(SRID.UnsupportedFormat, new object[0]);
				}
				this.cchSymbols = binaryReader.ReadInt32();
				this.pszSymbols = binaryReader.ReadUInt32();
				this.cRules = binaryReader.ReadInt32();
				this.pRules = binaryReader.ReadUInt32();
				this.cArcs = binaryReader.ReadInt32();
				this.pArcs = binaryReader.ReadUInt32();
				this.pWeights = binaryReader.ReadUInt32();
				this.cTags = binaryReader.ReadInt32();
				this.tags = binaryReader.ReadUInt32();
				this.ulReservered1 = binaryReader.ReadUInt32();
				this.ulReservered2 = binaryReader.ReadUInt32();
				if (this.pszWords > 100U)
				{
					this.cScripts = binaryReader.ReadInt32();
					this.pScripts = binaryReader.ReadUInt32();
					this.cIL = binaryReader.ReadInt32();
					this.pIL = binaryReader.ReadUInt32();
					this.cPDB = binaryReader.ReadInt32();
					this.pPDB = binaryReader.ReadUInt32();
					this.ulRootRuleIndex = binaryReader.ReadUInt32();
					this.GrammarOptions = (GrammarOptions)binaryReader.ReadUInt32();
					this.cBasePath = binaryReader.ReadUInt32();
					this.GrammarMode = binaryReader.ReadUInt32();
					this.ulReservered3 = binaryReader.ReadUInt32();
					this.ulReservered4 = binaryReader.ReadUInt32();
				}
			}

			// Token: 0x06000BAD RID: 2989 RVA: 0x0002E074 File Offset: 0x0002C274
			internal static bool IsCfg(Stream stream, out int cfgLength)
			{
				cfgLength = 0;
				BinaryReader binaryReader = new BinaryReader(stream);
				uint num = binaryReader.ReadUInt32();
				if (num < 100U || num > 2147483647U)
				{
					return false;
				}
				Guid guid = new Guid(binaryReader.ReadBytes(16));
				if (guid != CfgGrammar._SPGDF_ContextFree)
				{
					return false;
				}
				cfgLength = (int)num;
				return true;
			}

			// Token: 0x04000970 RID: 2416
			internal uint ulTotalSerializedSize;

			// Token: 0x04000971 RID: 2417
			internal Guid FormatId;

			// Token: 0x04000972 RID: 2418
			internal Guid GrammarGUID;

			// Token: 0x04000973 RID: 2419
			internal ushort LangID;

			// Token: 0x04000974 RID: 2420
			internal ushort pszSemanticInterpretationGlobals;

			// Token: 0x04000975 RID: 2421
			internal int cArcsInLargestState;

			// Token: 0x04000976 RID: 2422
			internal int cchWords;

			// Token: 0x04000977 RID: 2423
			internal int cWords;

			// Token: 0x04000978 RID: 2424
			internal uint pszWords;

			// Token: 0x04000979 RID: 2425
			internal int cchSymbols;

			// Token: 0x0400097A RID: 2426
			internal uint pszSymbols;

			// Token: 0x0400097B RID: 2427
			internal int cRules;

			// Token: 0x0400097C RID: 2428
			internal uint pRules;

			// Token: 0x0400097D RID: 2429
			internal int cArcs;

			// Token: 0x0400097E RID: 2430
			internal uint pArcs;

			// Token: 0x0400097F RID: 2431
			internal uint pWeights;

			// Token: 0x04000980 RID: 2432
			internal int cTags;

			// Token: 0x04000981 RID: 2433
			internal uint tags;

			// Token: 0x04000982 RID: 2434
			internal uint ulReservered1;

			// Token: 0x04000983 RID: 2435
			internal uint ulReservered2;

			// Token: 0x04000984 RID: 2436
			internal int cScripts;

			// Token: 0x04000985 RID: 2437
			internal uint pScripts;

			// Token: 0x04000986 RID: 2438
			internal int cIL;

			// Token: 0x04000987 RID: 2439
			internal uint pIL;

			// Token: 0x04000988 RID: 2440
			internal int cPDB;

			// Token: 0x04000989 RID: 2441
			internal uint pPDB;

			// Token: 0x0400098A RID: 2442
			internal uint ulRootRuleIndex;

			// Token: 0x0400098B RID: 2443
			internal GrammarOptions GrammarOptions;

			// Token: 0x0400098C RID: 2444
			internal uint cBasePath;

			// Token: 0x0400098D RID: 2445
			internal uint GrammarMode;

			// Token: 0x0400098E RID: 2446
			internal uint ulReservered3;

			// Token: 0x0400098F RID: 2447
			internal uint ulReservered4;
		}

		// Token: 0x020001A4 RID: 420
		internal class CfgProperty
		{
			// Token: 0x04000990 RID: 2448
			internal string _pszName;

			// Token: 0x04000991 RID: 2449
			internal uint _ulId;

			// Token: 0x04000992 RID: 2450
			internal VarEnum _comType;

			// Token: 0x04000993 RID: 2451
			internal object _comValue;
		}
	}
}
