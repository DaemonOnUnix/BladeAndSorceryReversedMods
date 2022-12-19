using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Speech.Internal.SrgsParser;

namespace System.Speech.Internal.SrgsCompiler
{
	// Token: 0x0200009C RID: 156
	internal sealed class CfgGrammar
	{
		// Token: 0x06000358 RID: 856 RVA: 0x0000CA4B File Offset: 0x0000BA4B
		internal CfgGrammar()
		{
		}

		// Token: 0x06000359 RID: 857 RVA: 0x0000CA54 File Offset: 0x0000BA54
		internal static CfgGrammar.CfgHeader ConvertCfgHeader(StreamMarshaler streamHelper)
		{
			CfgGrammar.CfgSerializedHeader cfgSerializedHeader = null;
			return CfgGrammar.ConvertCfgHeader(streamHelper, true, true, out cfgSerializedHeader);
		}

		// Token: 0x0600035A RID: 858 RVA: 0x0000CA70 File Offset: 0x0000BA70
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

		// Token: 0x0600035B RID: 859 RVA: 0x0000CC58 File Offset: 0x0000BC58
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

		// Token: 0x0600035C RID: 860 RVA: 0x0000CD0C File Offset: 0x0000BD0C
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

		// Token: 0x0600035D RID: 861 RVA: 0x0000CD54 File Offset: 0x0000BD54
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

		// Token: 0x0600035E RID: 862 RVA: 0x0000CDF0 File Offset: 0x0000BDF0
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
						if (startArcIndex == 0 || startArcIndex >= num || endArcIndex == 0 || endArcIndex >= num || (header.tags[i].PropVariantType != null && header.tags[i].PropVariantType == 8 && header.tags[i].PropVariantType == 11 && header.tags[i].PropVariantType == 5 && header.tags[i].PropVariantType == 3))
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

		// Token: 0x0600035F RID: 863 RVA: 0x0000D08C File Offset: 0x0000C08C
		private static void CheckSetOffsets(uint offset, int size, ref int start, uint max)
		{
			if (offset < (uint)start || (start = (int)(offset + (uint)size)) > (int)max)
			{
				XmlParser.ThrowSrgsException(SRID.UnsupportedFormat, new object[0]);
			}
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0000D0B8 File Offset: 0x0000C0B8
		private static StringBlob LoadStringBlob(StreamMarshaler streamHelper, uint iPos, int c)
		{
			char[] array = new char[c];
			streamHelper.Position = iPos;
			streamHelper.ReadArrayChar(array, c);
			return new StringBlob(array);
		}

		// Token: 0x06000361 RID: 865 RVA: 0x0000D0E4 File Offset: 0x0000C0E4
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

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000362 RID: 866 RVA: 0x0000D10E File Offset: 0x0000C10E
		internal static uint NextHandle
		{
			get
			{
				return CfgGrammar._lastHandle += 1U;
			}
		}

		// Token: 0x040002F5 RID: 757
		internal const int INFINITE = -1;

		// Token: 0x040002F6 RID: 758
		internal const int SPTEXTBUFFERTRANSITION = 4194303;

		// Token: 0x040002F7 RID: 759
		internal const int SPWILDCARDTRANSITION = 4194302;

		// Token: 0x040002F8 RID: 760
		internal const int SPDICTATIONTRANSITION = 4194301;

		// Token: 0x040002F9 RID: 761
		internal const int MAX_TRANSITIONS_COUNT = 256;

		// Token: 0x040002FA RID: 762
		internal const float DEFAULT_WEIGHT = 1f;

		// Token: 0x040002FB RID: 763
		internal const int SP_LOW_CONFIDENCE = -1;

		// Token: 0x040002FC RID: 764
		internal const int SP_NORMAL_CONFIDENCE = 0;

		// Token: 0x040002FD RID: 765
		internal const int SP_HIGH_CONFIDENCE = 1;

		// Token: 0x040002FE RID: 766
		private const int SP_SPCFGSERIALIZEDHEADER_500 = 100;

		// Token: 0x040002FF RID: 767
		internal static Guid _SPGDF_ContextFree = new Guid(1306301037, 27879, 19904, 153, 167, 175, 158, 107, 106, 78, 145);

		// Token: 0x04000300 RID: 768
		internal static readonly Rule SPRULETRANS_TEXTBUFFER = new Rule(-1);

		// Token: 0x04000301 RID: 769
		internal static readonly Rule SPRULETRANS_WILDCARD = new Rule(-2);

		// Token: 0x04000302 RID: 770
		internal static readonly Rule SPRULETRANS_DICTATION = new Rule(-3);

		// Token: 0x04000303 RID: 771
		private static uint _lastHandle;

		// Token: 0x0200009D RID: 157
		internal struct CfgHeader
		{
			// Token: 0x04000304 RID: 772
			internal Guid FormatId;

			// Token: 0x04000305 RID: 773
			internal Guid GrammarGUID;

			// Token: 0x04000306 RID: 774
			internal ushort langId;

			// Token: 0x04000307 RID: 775
			internal ushort pszGlobalTags;

			// Token: 0x04000308 RID: 776
			internal int cArcsInLargestState;

			// Token: 0x04000309 RID: 777
			internal StringBlob pszWords;

			// Token: 0x0400030A RID: 778
			internal StringBlob pszSymbols;

			// Token: 0x0400030B RID: 779
			internal CfgRule[] rules;

			// Token: 0x0400030C RID: 780
			internal CfgArc[] arcs;

			// Token: 0x0400030D RID: 781
			internal float[] weights;

			// Token: 0x0400030E RID: 782
			internal CfgSemanticTag[] tags;

			// Token: 0x0400030F RID: 783
			internal CfgScriptRef[] scripts;

			// Token: 0x04000310 RID: 784
			internal uint ulRootRuleIndex;

			// Token: 0x04000311 RID: 785
			internal GrammarOptions GrammarOptions;

			// Token: 0x04000312 RID: 786
			internal GrammarType GrammarMode;

			// Token: 0x04000313 RID: 787
			internal string BasePath;
		}

		// Token: 0x0200009E RID: 158
		[StructLayout(0)]
		internal class CfgSerializedHeader
		{
			// Token: 0x06000364 RID: 868 RVA: 0x0000D188 File Offset: 0x0000C188
			internal CfgSerializedHeader()
			{
			}

			// Token: 0x06000365 RID: 869 RVA: 0x0000D190 File Offset: 0x0000C190
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

			// Token: 0x06000366 RID: 870 RVA: 0x0000D3B0 File Offset: 0x0000C3B0
			internal static bool IsCfg(Stream stream, out int cfgLength)
			{
				cfgLength = 0;
				BinaryReader binaryReader = new BinaryReader(stream);
				uint num = binaryReader.ReadUInt32();
				if (num < 100U || num > 2147483647U)
				{
					return false;
				}
				Guid guid;
				guid..ctor(binaryReader.ReadBytes(16));
				if (guid != CfgGrammar._SPGDF_ContextFree)
				{
					return false;
				}
				cfgLength = (int)num;
				return true;
			}

			// Token: 0x04000314 RID: 788
			internal uint ulTotalSerializedSize;

			// Token: 0x04000315 RID: 789
			internal Guid FormatId;

			// Token: 0x04000316 RID: 790
			internal Guid GrammarGUID;

			// Token: 0x04000317 RID: 791
			internal ushort LangID;

			// Token: 0x04000318 RID: 792
			internal ushort pszSemanticInterpretationGlobals;

			// Token: 0x04000319 RID: 793
			internal int cArcsInLargestState;

			// Token: 0x0400031A RID: 794
			internal int cchWords;

			// Token: 0x0400031B RID: 795
			internal int cWords;

			// Token: 0x0400031C RID: 796
			internal uint pszWords;

			// Token: 0x0400031D RID: 797
			internal int cchSymbols;

			// Token: 0x0400031E RID: 798
			internal uint pszSymbols;

			// Token: 0x0400031F RID: 799
			internal int cRules;

			// Token: 0x04000320 RID: 800
			internal uint pRules;

			// Token: 0x04000321 RID: 801
			internal int cArcs;

			// Token: 0x04000322 RID: 802
			internal uint pArcs;

			// Token: 0x04000323 RID: 803
			internal uint pWeights;

			// Token: 0x04000324 RID: 804
			internal int cTags;

			// Token: 0x04000325 RID: 805
			internal uint tags;

			// Token: 0x04000326 RID: 806
			internal uint ulReservered1;

			// Token: 0x04000327 RID: 807
			internal uint ulReservered2;

			// Token: 0x04000328 RID: 808
			internal int cScripts;

			// Token: 0x04000329 RID: 809
			internal uint pScripts;

			// Token: 0x0400032A RID: 810
			internal int cIL;

			// Token: 0x0400032B RID: 811
			internal uint pIL;

			// Token: 0x0400032C RID: 812
			internal int cPDB;

			// Token: 0x0400032D RID: 813
			internal uint pPDB;

			// Token: 0x0400032E RID: 814
			internal uint ulRootRuleIndex;

			// Token: 0x0400032F RID: 815
			internal GrammarOptions GrammarOptions;

			// Token: 0x04000330 RID: 816
			internal uint cBasePath;

			// Token: 0x04000331 RID: 817
			internal uint GrammarMode;

			// Token: 0x04000332 RID: 818
			internal uint ulReservered3;

			// Token: 0x04000333 RID: 819
			internal uint ulReservered4;
		}

		// Token: 0x0200009F RID: 159
		internal class CfgProperty
		{
			// Token: 0x04000334 RID: 820
			internal string _pszName;

			// Token: 0x04000335 RID: 821
			internal uint _ulId;

			// Token: 0x04000336 RID: 822
			internal VarEnum _comType;

			// Token: 0x04000337 RID: 823
			internal object _comValue;
		}
	}
}
