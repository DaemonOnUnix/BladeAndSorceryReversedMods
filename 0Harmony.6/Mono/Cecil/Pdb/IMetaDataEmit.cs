using System;
using System.Runtime.InteropServices;

namespace Mono.Cecil.Pdb
{
	// Token: 0x0200022B RID: 555
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("BA3FEE4C-ECB9-4e41-83B7-183FA41CD859")]
	[ComImport]
	internal interface IMetaDataEmit
	{
		// Token: 0x060010C8 RID: 4296
		void SetModuleProps(string szName);

		// Token: 0x060010C9 RID: 4297
		void Save(string szFile, uint dwSaveFlags);

		// Token: 0x060010CA RID: 4298
		void SaveToStream(IntPtr pIStream, uint dwSaveFlags);

		// Token: 0x060010CB RID: 4299
		uint GetSaveSize(uint fSave);

		// Token: 0x060010CC RID: 4300
		uint DefineTypeDef(IntPtr szTypeDef, uint dwTypeDefFlags, uint tkExtends, IntPtr rtkImplements);

		// Token: 0x060010CD RID: 4301
		uint DefineNestedType(IntPtr szTypeDef, uint dwTypeDefFlags, uint tkExtends, IntPtr rtkImplements, uint tdEncloser);

		// Token: 0x060010CE RID: 4302
		void SetHandler([MarshalAs(UnmanagedType.IUnknown)] [In] object pUnk);

		// Token: 0x060010CF RID: 4303
		uint DefineMethod(uint td, IntPtr zName, uint dwMethodFlags, IntPtr pvSigBlob, uint cbSigBlob, uint ulCodeRVA, uint dwImplFlags);

		// Token: 0x060010D0 RID: 4304
		void DefineMethodImpl(uint td, uint tkBody, uint tkDecl);

		// Token: 0x060010D1 RID: 4305
		uint DefineTypeRefByName(uint tkResolutionScope, IntPtr szName);

		// Token: 0x060010D2 RID: 4306
		uint DefineImportType(IntPtr pAssemImport, IntPtr pbHashValue, uint cbHashValue, IMetaDataImport pImport, uint tdImport, IntPtr pAssemEmit);

		// Token: 0x060010D3 RID: 4307
		uint DefineMemberRef(uint tkImport, string szName, IntPtr pvSigBlob, uint cbSigBlob);

		// Token: 0x060010D4 RID: 4308
		uint DefineImportMember(IntPtr pAssemImport, IntPtr pbHashValue, uint cbHashValue, IMetaDataImport pImport, uint mbMember, IntPtr pAssemEmit, uint tkParent);

		// Token: 0x060010D5 RID: 4309
		uint DefineEvent(uint td, string szEvent, uint dwEventFlags, uint tkEventType, uint mdAddOn, uint mdRemoveOn, uint mdFire, IntPtr rmdOtherMethods);

		// Token: 0x060010D6 RID: 4310
		void SetClassLayout(uint td, uint dwPackSize, IntPtr rFieldOffsets, uint ulClassSize);

		// Token: 0x060010D7 RID: 4311
		void DeleteClassLayout(uint td);

		// Token: 0x060010D8 RID: 4312
		void SetFieldMarshal(uint tk, IntPtr pvNativeType, uint cbNativeType);

		// Token: 0x060010D9 RID: 4313
		void DeleteFieldMarshal(uint tk);

		// Token: 0x060010DA RID: 4314
		uint DefinePermissionSet(uint tk, uint dwAction, IntPtr pvPermission, uint cbPermission);

		// Token: 0x060010DB RID: 4315
		void SetRVA(uint md, uint ulRVA);

		// Token: 0x060010DC RID: 4316
		uint GetTokenFromSig(IntPtr pvSig, uint cbSig);

		// Token: 0x060010DD RID: 4317
		uint DefineModuleRef(string szName);

		// Token: 0x060010DE RID: 4318
		void SetParent(uint mr, uint tk);

		// Token: 0x060010DF RID: 4319
		uint GetTokenFromTypeSpec(IntPtr pvSig, uint cbSig);

		// Token: 0x060010E0 RID: 4320
		void SaveToMemory(IntPtr pbData, uint cbData);

		// Token: 0x060010E1 RID: 4321
		uint DefineUserString(string szString, uint cchString);

		// Token: 0x060010E2 RID: 4322
		void DeleteToken(uint tkObj);

		// Token: 0x060010E3 RID: 4323
		void SetMethodProps(uint md, uint dwMethodFlags, uint ulCodeRVA, uint dwImplFlags);

		// Token: 0x060010E4 RID: 4324
		void SetTypeDefProps(uint td, uint dwTypeDefFlags, uint tkExtends, IntPtr rtkImplements);

		// Token: 0x060010E5 RID: 4325
		void SetEventProps(uint ev, uint dwEventFlags, uint tkEventType, uint mdAddOn, uint mdRemoveOn, uint mdFire, IntPtr rmdOtherMethods);

		// Token: 0x060010E6 RID: 4326
		uint SetPermissionSetProps(uint tk, uint dwAction, IntPtr pvPermission, uint cbPermission);

		// Token: 0x060010E7 RID: 4327
		void DefinePinvokeMap(uint tk, uint dwMappingFlags, string szImportName, uint mrImportDLL);

		// Token: 0x060010E8 RID: 4328
		void SetPinvokeMap(uint tk, uint dwMappingFlags, string szImportName, uint mrImportDLL);

		// Token: 0x060010E9 RID: 4329
		void DeletePinvokeMap(uint tk);

		// Token: 0x060010EA RID: 4330
		uint DefineCustomAttribute(uint tkObj, uint tkType, IntPtr pCustomAttribute, uint cbCustomAttribute);

		// Token: 0x060010EB RID: 4331
		void SetCustomAttributeValue(uint pcv, IntPtr pCustomAttribute, uint cbCustomAttribute);

		// Token: 0x060010EC RID: 4332
		uint DefineField(uint td, string szName, uint dwFieldFlags, IntPtr pvSigBlob, uint cbSigBlob, uint dwCPlusTypeFlag, IntPtr pValue, uint cchValue);

		// Token: 0x060010ED RID: 4333
		uint DefineProperty(uint td, string szProperty, uint dwPropFlags, IntPtr pvSig, uint cbSig, uint dwCPlusTypeFlag, IntPtr pValue, uint cchValue, uint mdSetter, uint mdGetter, IntPtr rmdOtherMethods);

		// Token: 0x060010EE RID: 4334
		uint DefineParam(uint md, uint ulParamSeq, string szName, uint dwParamFlags, uint dwCPlusTypeFlag, IntPtr pValue, uint cchValue);

		// Token: 0x060010EF RID: 4335
		void SetFieldProps(uint fd, uint dwFieldFlags, uint dwCPlusTypeFlag, IntPtr pValue, uint cchValue);

		// Token: 0x060010F0 RID: 4336
		void SetPropertyProps(uint pr, uint dwPropFlags, uint dwCPlusTypeFlag, IntPtr pValue, uint cchValue, uint mdSetter, uint mdGetter, IntPtr rmdOtherMethods);

		// Token: 0x060010F1 RID: 4337
		void SetParamProps(uint pd, string szName, uint dwParamFlags, uint dwCPlusTypeFlag, IntPtr pValue, uint cchValue);

		// Token: 0x060010F2 RID: 4338
		uint DefineSecurityAttributeSet(uint tkObj, IntPtr rSecAttrs, uint cSecAttrs);

		// Token: 0x060010F3 RID: 4339
		void ApplyEditAndContinue([MarshalAs(UnmanagedType.IUnknown)] object pImport);

		// Token: 0x060010F4 RID: 4340
		uint TranslateSigWithScope(IntPtr pAssemImport, IntPtr pbHashValue, uint cbHashValue, IMetaDataImport import, IntPtr pbSigBlob, uint cbSigBlob, IntPtr pAssemEmit, IMetaDataEmit emit, IntPtr pvTranslatedSig, uint cbTranslatedSigMax);

		// Token: 0x060010F5 RID: 4341
		void SetMethodImplFlags(uint md, uint dwImplFlags);

		// Token: 0x060010F6 RID: 4342
		void SetFieldRVA(uint fd, uint ulRVA);

		// Token: 0x060010F7 RID: 4343
		void Merge(IMetaDataImport pImport, IntPtr pHostMapToken, [MarshalAs(UnmanagedType.IUnknown)] object pHandler);

		// Token: 0x060010F8 RID: 4344
		void MergeEnd();
	}
}
