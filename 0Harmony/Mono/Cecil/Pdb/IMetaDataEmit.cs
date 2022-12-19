using System;
using System.Runtime.InteropServices;

namespace Mono.Cecil.Pdb
{
	// Token: 0x02000321 RID: 801
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("BA3FEE4C-ECB9-4e41-83B7-183FA41CD859")]
	[ComImport]
	internal interface IMetaDataEmit
	{
		// Token: 0x06001438 RID: 5176
		void SetModuleProps(string szName);

		// Token: 0x06001439 RID: 5177
		void Save(string szFile, uint dwSaveFlags);

		// Token: 0x0600143A RID: 5178
		void SaveToStream(IntPtr pIStream, uint dwSaveFlags);

		// Token: 0x0600143B RID: 5179
		uint GetSaveSize(uint fSave);

		// Token: 0x0600143C RID: 5180
		uint DefineTypeDef(IntPtr szTypeDef, uint dwTypeDefFlags, uint tkExtends, IntPtr rtkImplements);

		// Token: 0x0600143D RID: 5181
		uint DefineNestedType(IntPtr szTypeDef, uint dwTypeDefFlags, uint tkExtends, IntPtr rtkImplements, uint tdEncloser);

		// Token: 0x0600143E RID: 5182
		void SetHandler([MarshalAs(UnmanagedType.IUnknown)] [In] object pUnk);

		// Token: 0x0600143F RID: 5183
		uint DefineMethod(uint td, IntPtr zName, uint dwMethodFlags, IntPtr pvSigBlob, uint cbSigBlob, uint ulCodeRVA, uint dwImplFlags);

		// Token: 0x06001440 RID: 5184
		void DefineMethodImpl(uint td, uint tkBody, uint tkDecl);

		// Token: 0x06001441 RID: 5185
		uint DefineTypeRefByName(uint tkResolutionScope, IntPtr szName);

		// Token: 0x06001442 RID: 5186
		uint DefineImportType(IntPtr pAssemImport, IntPtr pbHashValue, uint cbHashValue, IMetaDataImport pImport, uint tdImport, IntPtr pAssemEmit);

		// Token: 0x06001443 RID: 5187
		uint DefineMemberRef(uint tkImport, string szName, IntPtr pvSigBlob, uint cbSigBlob);

		// Token: 0x06001444 RID: 5188
		uint DefineImportMember(IntPtr pAssemImport, IntPtr pbHashValue, uint cbHashValue, IMetaDataImport pImport, uint mbMember, IntPtr pAssemEmit, uint tkParent);

		// Token: 0x06001445 RID: 5189
		uint DefineEvent(uint td, string szEvent, uint dwEventFlags, uint tkEventType, uint mdAddOn, uint mdRemoveOn, uint mdFire, IntPtr rmdOtherMethods);

		// Token: 0x06001446 RID: 5190
		void SetClassLayout(uint td, uint dwPackSize, IntPtr rFieldOffsets, uint ulClassSize);

		// Token: 0x06001447 RID: 5191
		void DeleteClassLayout(uint td);

		// Token: 0x06001448 RID: 5192
		void SetFieldMarshal(uint tk, IntPtr pvNativeType, uint cbNativeType);

		// Token: 0x06001449 RID: 5193
		void DeleteFieldMarshal(uint tk);

		// Token: 0x0600144A RID: 5194
		uint DefinePermissionSet(uint tk, uint dwAction, IntPtr pvPermission, uint cbPermission);

		// Token: 0x0600144B RID: 5195
		void SetRVA(uint md, uint ulRVA);

		// Token: 0x0600144C RID: 5196
		uint GetTokenFromSig(IntPtr pvSig, uint cbSig);

		// Token: 0x0600144D RID: 5197
		uint DefineModuleRef(string szName);

		// Token: 0x0600144E RID: 5198
		void SetParent(uint mr, uint tk);

		// Token: 0x0600144F RID: 5199
		uint GetTokenFromTypeSpec(IntPtr pvSig, uint cbSig);

		// Token: 0x06001450 RID: 5200
		void SaveToMemory(IntPtr pbData, uint cbData);

		// Token: 0x06001451 RID: 5201
		uint DefineUserString(string szString, uint cchString);

		// Token: 0x06001452 RID: 5202
		void DeleteToken(uint tkObj);

		// Token: 0x06001453 RID: 5203
		void SetMethodProps(uint md, uint dwMethodFlags, uint ulCodeRVA, uint dwImplFlags);

		// Token: 0x06001454 RID: 5204
		void SetTypeDefProps(uint td, uint dwTypeDefFlags, uint tkExtends, IntPtr rtkImplements);

		// Token: 0x06001455 RID: 5205
		void SetEventProps(uint ev, uint dwEventFlags, uint tkEventType, uint mdAddOn, uint mdRemoveOn, uint mdFire, IntPtr rmdOtherMethods);

		// Token: 0x06001456 RID: 5206
		uint SetPermissionSetProps(uint tk, uint dwAction, IntPtr pvPermission, uint cbPermission);

		// Token: 0x06001457 RID: 5207
		void DefinePinvokeMap(uint tk, uint dwMappingFlags, string szImportName, uint mrImportDLL);

		// Token: 0x06001458 RID: 5208
		void SetPinvokeMap(uint tk, uint dwMappingFlags, string szImportName, uint mrImportDLL);

		// Token: 0x06001459 RID: 5209
		void DeletePinvokeMap(uint tk);

		// Token: 0x0600145A RID: 5210
		uint DefineCustomAttribute(uint tkObj, uint tkType, IntPtr pCustomAttribute, uint cbCustomAttribute);

		// Token: 0x0600145B RID: 5211
		void SetCustomAttributeValue(uint pcv, IntPtr pCustomAttribute, uint cbCustomAttribute);

		// Token: 0x0600145C RID: 5212
		uint DefineField(uint td, string szName, uint dwFieldFlags, IntPtr pvSigBlob, uint cbSigBlob, uint dwCPlusTypeFlag, IntPtr pValue, uint cchValue);

		// Token: 0x0600145D RID: 5213
		uint DefineProperty(uint td, string szProperty, uint dwPropFlags, IntPtr pvSig, uint cbSig, uint dwCPlusTypeFlag, IntPtr pValue, uint cchValue, uint mdSetter, uint mdGetter, IntPtr rmdOtherMethods);

		// Token: 0x0600145E RID: 5214
		uint DefineParam(uint md, uint ulParamSeq, string szName, uint dwParamFlags, uint dwCPlusTypeFlag, IntPtr pValue, uint cchValue);

		// Token: 0x0600145F RID: 5215
		void SetFieldProps(uint fd, uint dwFieldFlags, uint dwCPlusTypeFlag, IntPtr pValue, uint cchValue);

		// Token: 0x06001460 RID: 5216
		void SetPropertyProps(uint pr, uint dwPropFlags, uint dwCPlusTypeFlag, IntPtr pValue, uint cchValue, uint mdSetter, uint mdGetter, IntPtr rmdOtherMethods);

		// Token: 0x06001461 RID: 5217
		void SetParamProps(uint pd, string szName, uint dwParamFlags, uint dwCPlusTypeFlag, IntPtr pValue, uint cchValue);

		// Token: 0x06001462 RID: 5218
		uint DefineSecurityAttributeSet(uint tkObj, IntPtr rSecAttrs, uint cSecAttrs);

		// Token: 0x06001463 RID: 5219
		void ApplyEditAndContinue([MarshalAs(UnmanagedType.IUnknown)] object pImport);

		// Token: 0x06001464 RID: 5220
		uint TranslateSigWithScope(IntPtr pAssemImport, IntPtr pbHashValue, uint cbHashValue, IMetaDataImport import, IntPtr pbSigBlob, uint cbSigBlob, IntPtr pAssemEmit, IMetaDataEmit emit, IntPtr pvTranslatedSig, uint cbTranslatedSigMax);

		// Token: 0x06001465 RID: 5221
		void SetMethodImplFlags(uint md, uint dwImplFlags);

		// Token: 0x06001466 RID: 5222
		void SetFieldRVA(uint fd, uint ulRVA);

		// Token: 0x06001467 RID: 5223
		void Merge(IMetaDataImport pImport, IntPtr pHostMapToken, [MarshalAs(UnmanagedType.IUnknown)] object pHandler);

		// Token: 0x06001468 RID: 5224
		void MergeEnd();
	}
}
