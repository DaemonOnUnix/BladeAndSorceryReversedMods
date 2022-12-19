using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Mono.Cecil.Pdb
{
	// Token: 0x0200022C RID: 556
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("7DAC8207-D3AE-4c75-9B67-92801A497D44")]
	[ComImport]
	internal interface IMetaDataImport
	{
		// Token: 0x060010F9 RID: 4345
		[PreserveSig]
		void CloseEnum(uint hEnum);

		// Token: 0x060010FA RID: 4346
		uint CountEnum(uint hEnum);

		// Token: 0x060010FB RID: 4347
		void ResetEnum(uint hEnum, uint ulPos);

		// Token: 0x060010FC RID: 4348
		uint EnumTypeDefs(ref uint phEnum, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] uint[] rTypeDefs, uint cMax);

		// Token: 0x060010FD RID: 4349
		uint EnumInterfaceImpls(ref uint phEnum, uint td, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] uint[] rImpls, uint cMax);

		// Token: 0x060010FE RID: 4350
		uint EnumTypeRefs(ref uint phEnum, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] uint[] rTypeRefs, uint cMax);

		// Token: 0x060010FF RID: 4351
		uint FindTypeDefByName(string szTypeDef, uint tkEnclosingClass);

		// Token: 0x06001100 RID: 4352
		Guid GetScopeProps(StringBuilder szName, uint cchName, out uint pchName);

		// Token: 0x06001101 RID: 4353
		uint GetModuleFromScope();

		// Token: 0x06001102 RID: 4354
		uint GetTypeDefProps(uint td, IntPtr szTypeDef, uint cchTypeDef, out uint pchTypeDef, IntPtr pdwTypeDefFlags);

		// Token: 0x06001103 RID: 4355
		uint GetInterfaceImplProps(uint iiImpl, out uint pClass);

		// Token: 0x06001104 RID: 4356
		uint GetTypeRefProps(uint tr, out uint ptkResolutionScope, StringBuilder szName, uint cchName);

		// Token: 0x06001105 RID: 4357
		uint ResolveTypeRef(uint tr, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppIScope);

		// Token: 0x06001106 RID: 4358
		uint EnumMembers(ref uint phEnum, uint cl, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] uint[] rMembers, uint cMax);

		// Token: 0x06001107 RID: 4359
		uint EnumMembersWithName(ref uint phEnum, uint cl, string szName, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] uint[] rMembers, uint cMax);

		// Token: 0x06001108 RID: 4360
		uint EnumMethods(ref uint phEnum, uint cl, IntPtr rMethods, uint cMax);

		// Token: 0x06001109 RID: 4361
		uint EnumMethodsWithName(ref uint phEnum, uint cl, string szName, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] uint[] rMethods, uint cMax);

		// Token: 0x0600110A RID: 4362
		uint EnumFields(ref uint phEnum, uint cl, IntPtr rFields, uint cMax);

		// Token: 0x0600110B RID: 4363
		uint EnumFieldsWithName(ref uint phEnum, uint cl, string szName, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] uint[] rFields, uint cMax);

		// Token: 0x0600110C RID: 4364
		uint EnumParams(ref uint phEnum, uint mb, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] uint[] rParams, uint cMax);

		// Token: 0x0600110D RID: 4365
		uint EnumMemberRefs(ref uint phEnum, uint tkParent, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] uint[] rMemberRefs, uint cMax);

		// Token: 0x0600110E RID: 4366
		uint EnumMethodImpls(ref uint phEnum, uint td, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] uint[] rMethodBody, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] uint[] rMethodDecl, uint cMax);

		// Token: 0x0600110F RID: 4367
		uint EnumPermissionSets(ref uint phEnum, uint tk, uint dwActions, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] uint[] rPermission, uint cMax);

		// Token: 0x06001110 RID: 4368
		uint FindMember(uint td, string szName, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] byte[] pvSigBlob, uint cbSigBlob);

		// Token: 0x06001111 RID: 4369
		uint FindMethod(uint td, string szName, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] byte[] pvSigBlob, uint cbSigBlob);

		// Token: 0x06001112 RID: 4370
		uint FindField(uint td, string szName, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] byte[] pvSigBlob, uint cbSigBlob);

		// Token: 0x06001113 RID: 4371
		uint FindMemberRef(uint td, string szName, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] byte[] pvSigBlob, uint cbSigBlob);

		// Token: 0x06001114 RID: 4372
		uint GetMethodProps(uint mb, out uint pClass, IntPtr szMethod, uint cchMethod, out uint pchMethod, IntPtr pdwAttr, IntPtr ppvSigBlob, IntPtr pcbSigBlob, IntPtr pulCodeRVA);

		// Token: 0x06001115 RID: 4373
		uint GetMemberRefProps(uint mr, ref uint ptk, StringBuilder szMember, uint cchMember, out uint pchMember, out IntPtr ppvSigBlob);

		// Token: 0x06001116 RID: 4374
		uint EnumProperties(ref uint phEnum, uint td, IntPtr rProperties, uint cMax);

		// Token: 0x06001117 RID: 4375
		uint EnumEvents(ref uint phEnum, uint td, IntPtr rEvents, uint cMax);

		// Token: 0x06001118 RID: 4376
		uint GetEventProps(uint ev, out uint pClass, StringBuilder szEvent, uint cchEvent, out uint pchEvent, out uint pdwEventFlags, out uint ptkEventType, out uint pmdAddOn, out uint pmdRemoveOn, out uint pmdFire, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 11)] uint[] rmdOtherMethod, uint cMax);

		// Token: 0x06001119 RID: 4377
		uint EnumMethodSemantics(ref uint phEnum, uint mb, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] uint[] rEventProp, uint cMax);

		// Token: 0x0600111A RID: 4378
		uint GetMethodSemantics(uint mb, uint tkEventProp);

		// Token: 0x0600111B RID: 4379
		uint GetClassLayout(uint td, out uint pdwPackSize, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] IntPtr rFieldOffset, uint cMax, out uint pcFieldOffset);

		// Token: 0x0600111C RID: 4380
		uint GetFieldMarshal(uint tk, out IntPtr ppvNativeType);

		// Token: 0x0600111D RID: 4381
		uint GetRVA(uint tk, out uint pulCodeRVA);

		// Token: 0x0600111E RID: 4382
		uint GetPermissionSetProps(uint pm, out uint pdwAction, out IntPtr ppvPermission);

		// Token: 0x0600111F RID: 4383
		uint GetSigFromToken(uint mdSig, out IntPtr ppvSig);

		// Token: 0x06001120 RID: 4384
		uint GetModuleRefProps(uint mur, StringBuilder szName, uint cchName);

		// Token: 0x06001121 RID: 4385
		uint EnumModuleRefs(ref uint phEnum, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] uint[] rModuleRefs, uint cmax);

		// Token: 0x06001122 RID: 4386
		uint GetTypeSpecFromToken(uint typespec, out IntPtr ppvSig);

		// Token: 0x06001123 RID: 4387
		uint GetNameFromToken(uint tk);

		// Token: 0x06001124 RID: 4388
		uint EnumUnresolvedMethods(ref uint phEnum, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] uint[] rMethods, uint cMax);

		// Token: 0x06001125 RID: 4389
		uint GetUserString(uint stk, StringBuilder szString, uint cchString);

		// Token: 0x06001126 RID: 4390
		uint GetPinvokeMap(uint tk, out uint pdwMappingFlags, StringBuilder szImportName, uint cchImportName, out uint pchImportName);

		// Token: 0x06001127 RID: 4391
		uint EnumSignatures(ref uint phEnum, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] uint[] rSignatures, uint cmax);

		// Token: 0x06001128 RID: 4392
		uint EnumTypeSpecs(ref uint phEnum, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] uint[] rTypeSpecs, uint cmax);

		// Token: 0x06001129 RID: 4393
		uint EnumUserStrings(ref uint phEnum, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] uint[] rStrings, uint cmax);

		// Token: 0x0600112A RID: 4394
		[PreserveSig]
		int GetParamForMethodIndex(uint md, uint ulParamSeq, out uint pParam);

		// Token: 0x0600112B RID: 4395
		uint EnumCustomAttributes(ref uint phEnum, uint tk, uint tkType, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] uint[] rCustomAttributes, uint cMax);

		// Token: 0x0600112C RID: 4396
		uint GetCustomAttributeProps(uint cv, out uint ptkObj, out uint ptkType, out IntPtr ppBlob);

		// Token: 0x0600112D RID: 4397
		uint FindTypeRef(uint tkResolutionScope, string szName);

		// Token: 0x0600112E RID: 4398
		uint GetMemberProps(uint mb, out uint pClass, StringBuilder szMember, uint cchMember, out uint pchMember, out uint pdwAttr, out IntPtr ppvSigBlob, out uint pcbSigBlob, out uint pulCodeRVA, out uint pdwImplFlags, out uint pdwCPlusTypeFlag, out IntPtr ppValue);

		// Token: 0x0600112F RID: 4399
		uint GetFieldProps(uint mb, out uint pClass, StringBuilder szField, uint cchField, out uint pchField, out uint pdwAttr, out IntPtr ppvSigBlob, out uint pcbSigBlob, out uint pdwCPlusTypeFlag, out IntPtr ppValue);

		// Token: 0x06001130 RID: 4400
		uint GetPropertyProps(uint prop, out uint pClass, StringBuilder szProperty, uint cchProperty, out uint pchProperty, out uint pdwPropFlags, out IntPtr ppvSig, out uint pbSig, out uint pdwCPlusTypeFlag, out IntPtr ppDefaultValue, out uint pcchDefaultValue, out uint pmdSetter, out uint pmdGetter, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 14)] uint[] rmdOtherMethod, uint cMax);

		// Token: 0x06001131 RID: 4401
		uint GetParamProps(uint tk, out uint pmd, out uint pulSequence, StringBuilder szName, uint cchName, out uint pchName, out uint pdwAttr, out uint pdwCPlusTypeFlag, out IntPtr ppValue);

		// Token: 0x06001132 RID: 4402
		uint GetCustomAttributeByName(uint tkObj, string szName, out IntPtr ppData);

		// Token: 0x06001133 RID: 4403
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.Bool)]
		bool IsValidToken(uint tk);

		// Token: 0x06001134 RID: 4404
		uint GetNestedClassProps(uint tdNestedClass);

		// Token: 0x06001135 RID: 4405
		uint GetNativeCallConvFromSig(IntPtr pvSig, uint cbSig);

		// Token: 0x06001136 RID: 4406
		int IsGlobal(uint pd);
	}
}
