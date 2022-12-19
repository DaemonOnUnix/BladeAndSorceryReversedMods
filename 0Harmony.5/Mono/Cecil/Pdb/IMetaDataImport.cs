using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Mono.Cecil.Pdb
{
	// Token: 0x02000322 RID: 802
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("7DAC8207-D3AE-4c75-9B67-92801A497D44")]
	[ComImport]
	internal interface IMetaDataImport
	{
		// Token: 0x06001469 RID: 5225
		[PreserveSig]
		void CloseEnum(uint hEnum);

		// Token: 0x0600146A RID: 5226
		uint CountEnum(uint hEnum);

		// Token: 0x0600146B RID: 5227
		void ResetEnum(uint hEnum, uint ulPos);

		// Token: 0x0600146C RID: 5228
		uint EnumTypeDefs(ref uint phEnum, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] uint[] rTypeDefs, uint cMax);

		// Token: 0x0600146D RID: 5229
		uint EnumInterfaceImpls(ref uint phEnum, uint td, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] uint[] rImpls, uint cMax);

		// Token: 0x0600146E RID: 5230
		uint EnumTypeRefs(ref uint phEnum, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] uint[] rTypeRefs, uint cMax);

		// Token: 0x0600146F RID: 5231
		uint FindTypeDefByName(string szTypeDef, uint tkEnclosingClass);

		// Token: 0x06001470 RID: 5232
		Guid GetScopeProps(StringBuilder szName, uint cchName, out uint pchName);

		// Token: 0x06001471 RID: 5233
		uint GetModuleFromScope();

		// Token: 0x06001472 RID: 5234
		[PreserveSig]
		unsafe uint GetTypeDefProps(uint td, char* szTypeDef, uint cchTypeDef, uint* pchTypeDef, uint* pdwTypeDefFlags, uint* ptkExtends);

		// Token: 0x06001473 RID: 5235
		uint GetInterfaceImplProps(uint iiImpl, out uint pClass);

		// Token: 0x06001474 RID: 5236
		uint GetTypeRefProps(uint tr, out uint ptkResolutionScope, StringBuilder szName, uint cchName);

		// Token: 0x06001475 RID: 5237
		uint ResolveTypeRef(uint tr, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppIScope);

		// Token: 0x06001476 RID: 5238
		uint EnumMembers(ref uint phEnum, uint cl, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] uint[] rMembers, uint cMax);

		// Token: 0x06001477 RID: 5239
		uint EnumMembersWithName(ref uint phEnum, uint cl, string szName, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] uint[] rMembers, uint cMax);

		// Token: 0x06001478 RID: 5240
		uint EnumMethods(ref uint phEnum, uint cl, IntPtr rMethods, uint cMax);

		// Token: 0x06001479 RID: 5241
		uint EnumMethodsWithName(ref uint phEnum, uint cl, string szName, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] uint[] rMethods, uint cMax);

		// Token: 0x0600147A RID: 5242
		uint EnumFields(ref uint phEnum, uint cl, IntPtr rFields, uint cMax);

		// Token: 0x0600147B RID: 5243
		uint EnumFieldsWithName(ref uint phEnum, uint cl, string szName, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] uint[] rFields, uint cMax);

		// Token: 0x0600147C RID: 5244
		uint EnumParams(ref uint phEnum, uint mb, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] uint[] rParams, uint cMax);

		// Token: 0x0600147D RID: 5245
		uint EnumMemberRefs(ref uint phEnum, uint tkParent, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] uint[] rMemberRefs, uint cMax);

		// Token: 0x0600147E RID: 5246
		uint EnumMethodImpls(ref uint phEnum, uint td, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] uint[] rMethodBody, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] uint[] rMethodDecl, uint cMax);

		// Token: 0x0600147F RID: 5247
		uint EnumPermissionSets(ref uint phEnum, uint tk, uint dwActions, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] uint[] rPermission, uint cMax);

		// Token: 0x06001480 RID: 5248
		uint FindMember(uint td, string szName, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] byte[] pvSigBlob, uint cbSigBlob);

		// Token: 0x06001481 RID: 5249
		uint FindMethod(uint td, string szName, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] byte[] pvSigBlob, uint cbSigBlob);

		// Token: 0x06001482 RID: 5250
		uint FindField(uint td, string szName, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] byte[] pvSigBlob, uint cbSigBlob);

		// Token: 0x06001483 RID: 5251
		uint FindMemberRef(uint td, string szName, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] byte[] pvSigBlob, uint cbSigBlob);

		// Token: 0x06001484 RID: 5252
		[PreserveSig]
		unsafe uint GetMethodProps(uint mb, uint* pClass, char* szMethod, uint cchMethod, uint* pchMethod, uint* pdwAttr, IntPtr ppvSigBlob, IntPtr pcbSigBlob, uint* pulCodeRVA, uint* pdwImplFlags);

		// Token: 0x06001485 RID: 5253
		uint GetMemberRefProps(uint mr, ref uint ptk, StringBuilder szMember, uint cchMember, out uint pchMember, out IntPtr ppvSigBlob);

		// Token: 0x06001486 RID: 5254
		uint EnumProperties(ref uint phEnum, uint td, IntPtr rProperties, uint cMax);

		// Token: 0x06001487 RID: 5255
		uint EnumEvents(ref uint phEnum, uint td, IntPtr rEvents, uint cMax);

		// Token: 0x06001488 RID: 5256
		uint GetEventProps(uint ev, out uint pClass, StringBuilder szEvent, uint cchEvent, out uint pchEvent, out uint pdwEventFlags, out uint ptkEventType, out uint pmdAddOn, out uint pmdRemoveOn, out uint pmdFire, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 11)] uint[] rmdOtherMethod, uint cMax);

		// Token: 0x06001489 RID: 5257
		uint EnumMethodSemantics(ref uint phEnum, uint mb, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] uint[] rEventProp, uint cMax);

		// Token: 0x0600148A RID: 5258
		uint GetMethodSemantics(uint mb, uint tkEventProp);

		// Token: 0x0600148B RID: 5259
		uint GetClassLayout(uint td, out uint pdwPackSize, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] IntPtr rFieldOffset, uint cMax, out uint pcFieldOffset);

		// Token: 0x0600148C RID: 5260
		uint GetFieldMarshal(uint tk, out IntPtr ppvNativeType);

		// Token: 0x0600148D RID: 5261
		uint GetRVA(uint tk, out uint pulCodeRVA);

		// Token: 0x0600148E RID: 5262
		uint GetPermissionSetProps(uint pm, out uint pdwAction, out IntPtr ppvPermission);

		// Token: 0x0600148F RID: 5263
		uint GetSigFromToken(uint mdSig, out IntPtr ppvSig);

		// Token: 0x06001490 RID: 5264
		uint GetModuleRefProps(uint mur, StringBuilder szName, uint cchName);

		// Token: 0x06001491 RID: 5265
		uint EnumModuleRefs(ref uint phEnum, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] uint[] rModuleRefs, uint cmax);

		// Token: 0x06001492 RID: 5266
		uint GetTypeSpecFromToken(uint typespec, out IntPtr ppvSig);

		// Token: 0x06001493 RID: 5267
		uint GetNameFromToken(uint tk);

		// Token: 0x06001494 RID: 5268
		uint EnumUnresolvedMethods(ref uint phEnum, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] uint[] rMethods, uint cMax);

		// Token: 0x06001495 RID: 5269
		uint GetUserString(uint stk, StringBuilder szString, uint cchString);

		// Token: 0x06001496 RID: 5270
		uint GetPinvokeMap(uint tk, out uint pdwMappingFlags, StringBuilder szImportName, uint cchImportName, out uint pchImportName);

		// Token: 0x06001497 RID: 5271
		uint EnumSignatures(ref uint phEnum, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] uint[] rSignatures, uint cmax);

		// Token: 0x06001498 RID: 5272
		uint EnumTypeSpecs(ref uint phEnum, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] uint[] rTypeSpecs, uint cmax);

		// Token: 0x06001499 RID: 5273
		uint EnumUserStrings(ref uint phEnum, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] uint[] rStrings, uint cmax);

		// Token: 0x0600149A RID: 5274
		[PreserveSig]
		int GetParamForMethodIndex(uint md, uint ulParamSeq, out uint pParam);

		// Token: 0x0600149B RID: 5275
		uint EnumCustomAttributes(ref uint phEnum, uint tk, uint tkType, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] uint[] rCustomAttributes, uint cMax);

		// Token: 0x0600149C RID: 5276
		uint GetCustomAttributeProps(uint cv, out uint ptkObj, out uint ptkType, out IntPtr ppBlob);

		// Token: 0x0600149D RID: 5277
		uint FindTypeRef(uint tkResolutionScope, string szName);

		// Token: 0x0600149E RID: 5278
		uint GetMemberProps(uint mb, out uint pClass, StringBuilder szMember, uint cchMember, out uint pchMember, out uint pdwAttr, out IntPtr ppvSigBlob, out uint pcbSigBlob, out uint pulCodeRVA, out uint pdwImplFlags, out uint pdwCPlusTypeFlag, out IntPtr ppValue);

		// Token: 0x0600149F RID: 5279
		uint GetFieldProps(uint mb, out uint pClass, StringBuilder szField, uint cchField, out uint pchField, out uint pdwAttr, out IntPtr ppvSigBlob, out uint pcbSigBlob, out uint pdwCPlusTypeFlag, out IntPtr ppValue);

		// Token: 0x060014A0 RID: 5280
		uint GetPropertyProps(uint prop, out uint pClass, StringBuilder szProperty, uint cchProperty, out uint pchProperty, out uint pdwPropFlags, out IntPtr ppvSig, out uint pbSig, out uint pdwCPlusTypeFlag, out IntPtr ppDefaultValue, out uint pcchDefaultValue, out uint pmdSetter, out uint pmdGetter, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 14)] uint[] rmdOtherMethod, uint cMax);

		// Token: 0x060014A1 RID: 5281
		uint GetParamProps(uint tk, out uint pmd, out uint pulSequence, StringBuilder szName, uint cchName, out uint pchName, out uint pdwAttr, out uint pdwCPlusTypeFlag, out IntPtr ppValue);

		// Token: 0x060014A2 RID: 5282
		uint GetCustomAttributeByName(uint tkObj, string szName, out IntPtr ppData);

		// Token: 0x060014A3 RID: 5283
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.Bool)]
		bool IsValidToken(uint tk);

		// Token: 0x060014A4 RID: 5284
		[PreserveSig]
		unsafe uint GetNestedClassProps(uint tdNestedClass, uint* ptdEnclosingClass);

		// Token: 0x060014A5 RID: 5285
		uint GetNativeCallConvFromSig(IntPtr pvSig, uint cbSig);

		// Token: 0x060014A6 RID: 5286
		int IsGlobal(uint pd);
	}
}
