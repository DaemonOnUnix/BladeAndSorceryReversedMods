using System;
using System.Collections.Generic;
using System.Text;

namespace Mono.Cecil.Pdb
{
	// Token: 0x02000323 RID: 803
	internal class ModuleMetadata : IMetaDataEmit, IMetaDataImport
	{
		// Token: 0x060014A7 RID: 5287 RVA: 0x000413C3 File Offset: 0x0003F5C3
		public ModuleMetadata(ModuleDefinition module)
		{
			this.module = module;
		}

		// Token: 0x060014A8 RID: 5288 RVA: 0x000413D2 File Offset: 0x0003F5D2
		private bool TryGetType(uint token, out TypeDefinition type)
		{
			if (this.types == null)
			{
				this.InitializeMetadata(this.module);
			}
			return this.types.TryGetValue(token, out type);
		}

		// Token: 0x060014A9 RID: 5289 RVA: 0x000413F5 File Offset: 0x0003F5F5
		private bool TryGetMethod(uint token, out MethodDefinition method)
		{
			if (this.methods == null)
			{
				this.InitializeMetadata(this.module);
			}
			return this.methods.TryGetValue(token, out method);
		}

		// Token: 0x060014AA RID: 5290 RVA: 0x00041418 File Offset: 0x0003F618
		private void InitializeMetadata(ModuleDefinition module)
		{
			this.types = new Dictionary<uint, TypeDefinition>();
			this.methods = new Dictionary<uint, MethodDefinition>();
			foreach (TypeDefinition typeDefinition in module.GetTypes())
			{
				this.types.Add(typeDefinition.MetadataToken.ToUInt32(), typeDefinition);
				this.InitializeMethods(typeDefinition);
			}
		}

		// Token: 0x060014AB RID: 5291 RVA: 0x00041498 File Offset: 0x0003F698
		private void InitializeMethods(TypeDefinition type)
		{
			foreach (MethodDefinition methodDefinition in type.Methods)
			{
				this.methods.Add(methodDefinition.MetadataToken.ToUInt32(), methodDefinition);
			}
		}

		// Token: 0x060014AC RID: 5292 RVA: 0x0004101A File Offset: 0x0003F21A
		public void SetModuleProps(string szName)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014AD RID: 5293 RVA: 0x0004101A File Offset: 0x0003F21A
		public void Save(string szFile, uint dwSaveFlags)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014AE RID: 5294 RVA: 0x0004101A File Offset: 0x0003F21A
		public void SaveToStream(IntPtr pIStream, uint dwSaveFlags)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014AF RID: 5295 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint GetSaveSize(uint fSave)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014B0 RID: 5296 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint DefineTypeDef(IntPtr szTypeDef, uint dwTypeDefFlags, uint tkExtends, IntPtr rtkImplements)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014B1 RID: 5297 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint DefineNestedType(IntPtr szTypeDef, uint dwTypeDefFlags, uint tkExtends, IntPtr rtkImplements, uint tdEncloser)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014B2 RID: 5298 RVA: 0x0004101A File Offset: 0x0003F21A
		public void SetHandler(object pUnk)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014B3 RID: 5299 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint DefineMethod(uint td, IntPtr zName, uint dwMethodFlags, IntPtr pvSigBlob, uint cbSigBlob, uint ulCodeRVA, uint dwImplFlags)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014B4 RID: 5300 RVA: 0x0004101A File Offset: 0x0003F21A
		public void DefineMethodImpl(uint td, uint tkBody, uint tkDecl)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014B5 RID: 5301 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint DefineTypeRefByName(uint tkResolutionScope, IntPtr szName)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014B6 RID: 5302 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint DefineImportType(IntPtr pAssemImport, IntPtr pbHashValue, uint cbHashValue, IMetaDataImport pImport, uint tdImport, IntPtr pAssemEmit)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014B7 RID: 5303 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint DefineMemberRef(uint tkImport, string szName, IntPtr pvSigBlob, uint cbSigBlob)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014B8 RID: 5304 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint DefineImportMember(IntPtr pAssemImport, IntPtr pbHashValue, uint cbHashValue, IMetaDataImport pImport, uint mbMember, IntPtr pAssemEmit, uint tkParent)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014B9 RID: 5305 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint DefineEvent(uint td, string szEvent, uint dwEventFlags, uint tkEventType, uint mdAddOn, uint mdRemoveOn, uint mdFire, IntPtr rmdOtherMethods)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014BA RID: 5306 RVA: 0x0004101A File Offset: 0x0003F21A
		public void SetClassLayout(uint td, uint dwPackSize, IntPtr rFieldOffsets, uint ulClassSize)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014BB RID: 5307 RVA: 0x0004101A File Offset: 0x0003F21A
		public void DeleteClassLayout(uint td)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014BC RID: 5308 RVA: 0x0004101A File Offset: 0x0003F21A
		public void SetFieldMarshal(uint tk, IntPtr pvNativeType, uint cbNativeType)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014BD RID: 5309 RVA: 0x0004101A File Offset: 0x0003F21A
		public void DeleteFieldMarshal(uint tk)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014BE RID: 5310 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint DefinePermissionSet(uint tk, uint dwAction, IntPtr pvPermission, uint cbPermission)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014BF RID: 5311 RVA: 0x0004101A File Offset: 0x0003F21A
		public void SetRVA(uint md, uint ulRVA)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014C0 RID: 5312 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint GetTokenFromSig(IntPtr pvSig, uint cbSig)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014C1 RID: 5313 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint DefineModuleRef(string szName)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014C2 RID: 5314 RVA: 0x0004101A File Offset: 0x0003F21A
		public void SetParent(uint mr, uint tk)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014C3 RID: 5315 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint GetTokenFromTypeSpec(IntPtr pvSig, uint cbSig)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014C4 RID: 5316 RVA: 0x0004101A File Offset: 0x0003F21A
		public void SaveToMemory(IntPtr pbData, uint cbData)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014C5 RID: 5317 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint DefineUserString(string szString, uint cchString)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014C6 RID: 5318 RVA: 0x0004101A File Offset: 0x0003F21A
		public void DeleteToken(uint tkObj)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014C7 RID: 5319 RVA: 0x0004101A File Offset: 0x0003F21A
		public void SetMethodProps(uint md, uint dwMethodFlags, uint ulCodeRVA, uint dwImplFlags)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014C8 RID: 5320 RVA: 0x0004101A File Offset: 0x0003F21A
		public void SetTypeDefProps(uint td, uint dwTypeDefFlags, uint tkExtends, IntPtr rtkImplements)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014C9 RID: 5321 RVA: 0x0004101A File Offset: 0x0003F21A
		public void SetEventProps(uint ev, uint dwEventFlags, uint tkEventType, uint mdAddOn, uint mdRemoveOn, uint mdFire, IntPtr rmdOtherMethods)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014CA RID: 5322 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint SetPermissionSetProps(uint tk, uint dwAction, IntPtr pvPermission, uint cbPermission)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014CB RID: 5323 RVA: 0x0004101A File Offset: 0x0003F21A
		public void DefinePinvokeMap(uint tk, uint dwMappingFlags, string szImportName, uint mrImportDLL)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014CC RID: 5324 RVA: 0x0004101A File Offset: 0x0003F21A
		public void SetPinvokeMap(uint tk, uint dwMappingFlags, string szImportName, uint mrImportDLL)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014CD RID: 5325 RVA: 0x0004101A File Offset: 0x0003F21A
		public void DeletePinvokeMap(uint tk)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014CE RID: 5326 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint DefineCustomAttribute(uint tkObj, uint tkType, IntPtr pCustomAttribute, uint cbCustomAttribute)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014CF RID: 5327 RVA: 0x0004101A File Offset: 0x0003F21A
		public void SetCustomAttributeValue(uint pcv, IntPtr pCustomAttribute, uint cbCustomAttribute)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014D0 RID: 5328 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint DefineField(uint td, string szName, uint dwFieldFlags, IntPtr pvSigBlob, uint cbSigBlob, uint dwCPlusTypeFlag, IntPtr pValue, uint cchValue)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014D1 RID: 5329 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint DefineProperty(uint td, string szProperty, uint dwPropFlags, IntPtr pvSig, uint cbSig, uint dwCPlusTypeFlag, IntPtr pValue, uint cchValue, uint mdSetter, uint mdGetter, IntPtr rmdOtherMethods)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014D2 RID: 5330 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint DefineParam(uint md, uint ulParamSeq, string szName, uint dwParamFlags, uint dwCPlusTypeFlag, IntPtr pValue, uint cchValue)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014D3 RID: 5331 RVA: 0x0004101A File Offset: 0x0003F21A
		public void SetFieldProps(uint fd, uint dwFieldFlags, uint dwCPlusTypeFlag, IntPtr pValue, uint cchValue)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014D4 RID: 5332 RVA: 0x0004101A File Offset: 0x0003F21A
		public void SetPropertyProps(uint pr, uint dwPropFlags, uint dwCPlusTypeFlag, IntPtr pValue, uint cchValue, uint mdSetter, uint mdGetter, IntPtr rmdOtherMethods)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014D5 RID: 5333 RVA: 0x0004101A File Offset: 0x0003F21A
		public void SetParamProps(uint pd, string szName, uint dwParamFlags, uint dwCPlusTypeFlag, IntPtr pValue, uint cchValue)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014D6 RID: 5334 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint DefineSecurityAttributeSet(uint tkObj, IntPtr rSecAttrs, uint cSecAttrs)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014D7 RID: 5335 RVA: 0x0004101A File Offset: 0x0003F21A
		public void ApplyEditAndContinue(object pImport)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014D8 RID: 5336 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint TranslateSigWithScope(IntPtr pAssemImport, IntPtr pbHashValue, uint cbHashValue, IMetaDataImport import, IntPtr pbSigBlob, uint cbSigBlob, IntPtr pAssemEmit, IMetaDataEmit emit, IntPtr pvTranslatedSig, uint cbTranslatedSigMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014D9 RID: 5337 RVA: 0x0004101A File Offset: 0x0003F21A
		public void SetMethodImplFlags(uint md, uint dwImplFlags)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014DA RID: 5338 RVA: 0x0004101A File Offset: 0x0003F21A
		public void SetFieldRVA(uint fd, uint ulRVA)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014DB RID: 5339 RVA: 0x0004101A File Offset: 0x0003F21A
		public void Merge(IMetaDataImport pImport, IntPtr pHostMapToken, object pHandler)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014DC RID: 5340 RVA: 0x0004101A File Offset: 0x0003F21A
		public void MergeEnd()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014DD RID: 5341 RVA: 0x0004101A File Offset: 0x0003F21A
		public void CloseEnum(uint hEnum)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014DE RID: 5342 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint CountEnum(uint hEnum)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014DF RID: 5343 RVA: 0x0004101A File Offset: 0x0003F21A
		public void ResetEnum(uint hEnum, uint ulPos)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014E0 RID: 5344 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint EnumTypeDefs(ref uint phEnum, uint[] rTypeDefs, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014E1 RID: 5345 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint EnumInterfaceImpls(ref uint phEnum, uint td, uint[] rImpls, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014E2 RID: 5346 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint EnumTypeRefs(ref uint phEnum, uint[] rTypeRefs, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014E3 RID: 5347 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint FindTypeDefByName(string szTypeDef, uint tkEnclosingClass)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014E4 RID: 5348 RVA: 0x0004101A File Offset: 0x0003F21A
		public Guid GetScopeProps(StringBuilder szName, uint cchName, out uint pchName)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014E5 RID: 5349 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint GetModuleFromScope()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014E6 RID: 5350 RVA: 0x00041500 File Offset: 0x0003F700
		public unsafe uint GetTypeDefProps(uint td, char* szTypeDef, uint cchTypeDef, uint* pchTypeDef, uint* pdwTypeDefFlags, uint* ptkExtends)
		{
			TypeDefinition typeDefinition;
			if (!this.TryGetType(td, out typeDefinition))
			{
				return 2147500037U;
			}
			ModuleMetadata.WriteNameBuffer(typeDefinition.IsNested ? typeDefinition.Name : typeDefinition.FullName, szTypeDef, cchTypeDef, pchTypeDef);
			if (pdwTypeDefFlags != null)
			{
				*pdwTypeDefFlags = (uint)typeDefinition.Attributes;
			}
			if (ptkExtends != null)
			{
				*ptkExtends = ((typeDefinition.BaseType != null) ? typeDefinition.BaseType.MetadataToken.ToUInt32() : 0U);
			}
			return 0U;
		}

		// Token: 0x060014E7 RID: 5351 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint GetInterfaceImplProps(uint iiImpl, out uint pClass)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014E8 RID: 5352 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint GetTypeRefProps(uint tr, out uint ptkResolutionScope, StringBuilder szName, uint cchName)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014E9 RID: 5353 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint ResolveTypeRef(uint tr, ref Guid riid, out object ppIScope)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014EA RID: 5354 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint EnumMembers(ref uint phEnum, uint cl, uint[] rMembers, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014EB RID: 5355 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint EnumMembersWithName(ref uint phEnum, uint cl, string szName, uint[] rMembers, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014EC RID: 5356 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint EnumMethods(ref uint phEnum, uint cl, IntPtr rMethods, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014ED RID: 5357 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint EnumMethodsWithName(ref uint phEnum, uint cl, string szName, uint[] rMethods, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014EE RID: 5358 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint EnumFields(ref uint phEnum, uint cl, IntPtr rFields, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014EF RID: 5359 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint EnumFieldsWithName(ref uint phEnum, uint cl, string szName, uint[] rFields, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014F0 RID: 5360 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint EnumParams(ref uint phEnum, uint mb, uint[] rParams, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014F1 RID: 5361 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint EnumMemberRefs(ref uint phEnum, uint tkParent, uint[] rMemberRefs, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014F2 RID: 5362 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint EnumMethodImpls(ref uint phEnum, uint td, uint[] rMethodBody, uint[] rMethodDecl, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014F3 RID: 5363 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint EnumPermissionSets(ref uint phEnum, uint tk, uint dwActions, uint[] rPermission, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014F4 RID: 5364 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint FindMember(uint td, string szName, byte[] pvSigBlob, uint cbSigBlob)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014F5 RID: 5365 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint FindMethod(uint td, string szName, byte[] pvSigBlob, uint cbSigBlob)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014F6 RID: 5366 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint FindField(uint td, string szName, byte[] pvSigBlob, uint cbSigBlob)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014F7 RID: 5367 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint FindMemberRef(uint td, string szName, byte[] pvSigBlob, uint cbSigBlob)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014F8 RID: 5368 RVA: 0x00041574 File Offset: 0x0003F774
		public unsafe uint GetMethodProps(uint mb, uint* pClass, char* szMethod, uint cchMethod, uint* pchMethod, uint* pdwAttr, IntPtr ppvSigBlob, IntPtr pcbSigBlob, uint* pulCodeRVA, uint* pdwImplFlags)
		{
			MethodDefinition methodDefinition;
			if (!this.TryGetMethod(mb, out methodDefinition))
			{
				return 2147500037U;
			}
			if (pClass != null)
			{
				*pClass = methodDefinition.DeclaringType.MetadataToken.ToUInt32();
			}
			ModuleMetadata.WriteNameBuffer(methodDefinition.Name, szMethod, cchMethod, pchMethod);
			if (pdwAttr != null)
			{
				*pdwAttr = (uint)methodDefinition.Attributes;
			}
			if (pulCodeRVA != null)
			{
				*pulCodeRVA = (uint)methodDefinition.RVA;
			}
			if (pdwImplFlags != null)
			{
				*pdwImplFlags = (uint)methodDefinition.ImplAttributes;
			}
			return 0U;
		}

		// Token: 0x060014F9 RID: 5369 RVA: 0x000415EC File Offset: 0x0003F7EC
		private unsafe static void WriteNameBuffer(string name, char* buffer, uint bufferLength, uint* actualLength)
		{
			long num = Math.Min((long)name.Length, (long)((ulong)(bufferLength - 1U)));
			if (actualLength != null)
			{
				*actualLength = (uint)num;
			}
			if (buffer != null && bufferLength > 0U)
			{
				int num2 = 0;
				while ((long)num2 < num)
				{
					buffer[num2] = name[num2];
					num2++;
				}
				buffer[(num + 1L) * 2L / 2L] = '\0';
			}
		}

		// Token: 0x060014FA RID: 5370 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint GetMemberRefProps(uint mr, ref uint ptk, StringBuilder szMember, uint cchMember, out uint pchMember, out IntPtr ppvSigBlob)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014FB RID: 5371 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint EnumProperties(ref uint phEnum, uint td, IntPtr rProperties, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014FC RID: 5372 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint EnumEvents(ref uint phEnum, uint td, IntPtr rEvents, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014FD RID: 5373 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint GetEventProps(uint ev, out uint pClass, StringBuilder szEvent, uint cchEvent, out uint pchEvent, out uint pdwEventFlags, out uint ptkEventType, out uint pmdAddOn, out uint pmdRemoveOn, out uint pmdFire, uint[] rmdOtherMethod, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014FE RID: 5374 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint EnumMethodSemantics(ref uint phEnum, uint mb, uint[] rEventProp, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060014FF RID: 5375 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint GetMethodSemantics(uint mb, uint tkEventProp)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001500 RID: 5376 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint GetClassLayout(uint td, out uint pdwPackSize, IntPtr rFieldOffset, uint cMax, out uint pcFieldOffset)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001501 RID: 5377 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint GetFieldMarshal(uint tk, out IntPtr ppvNativeType)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001502 RID: 5378 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint GetRVA(uint tk, out uint pulCodeRVA)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001503 RID: 5379 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint GetPermissionSetProps(uint pm, out uint pdwAction, out IntPtr ppvPermission)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001504 RID: 5380 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint GetSigFromToken(uint mdSig, out IntPtr ppvSig)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001505 RID: 5381 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint GetModuleRefProps(uint mur, StringBuilder szName, uint cchName)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001506 RID: 5382 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint EnumModuleRefs(ref uint phEnum, uint[] rModuleRefs, uint cmax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001507 RID: 5383 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint GetTypeSpecFromToken(uint typespec, out IntPtr ppvSig)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001508 RID: 5384 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint GetNameFromToken(uint tk)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001509 RID: 5385 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint EnumUnresolvedMethods(ref uint phEnum, uint[] rMethods, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600150A RID: 5386 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint GetUserString(uint stk, StringBuilder szString, uint cchString)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600150B RID: 5387 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint GetPinvokeMap(uint tk, out uint pdwMappingFlags, StringBuilder szImportName, uint cchImportName, out uint pchImportName)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600150C RID: 5388 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint EnumSignatures(ref uint phEnum, uint[] rSignatures, uint cmax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600150D RID: 5389 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint EnumTypeSpecs(ref uint phEnum, uint[] rTypeSpecs, uint cmax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600150E RID: 5390 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint EnumUserStrings(ref uint phEnum, uint[] rStrings, uint cmax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600150F RID: 5391 RVA: 0x0004101A File Offset: 0x0003F21A
		public int GetParamForMethodIndex(uint md, uint ulParamSeq, out uint pParam)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001510 RID: 5392 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint EnumCustomAttributes(ref uint phEnum, uint tk, uint tkType, uint[] rCustomAttributes, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001511 RID: 5393 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint GetCustomAttributeProps(uint cv, out uint ptkObj, out uint ptkType, out IntPtr ppBlob)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001512 RID: 5394 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint FindTypeRef(uint tkResolutionScope, string szName)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001513 RID: 5395 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint GetMemberProps(uint mb, out uint pClass, StringBuilder szMember, uint cchMember, out uint pchMember, out uint pdwAttr, out IntPtr ppvSigBlob, out uint pcbSigBlob, out uint pulCodeRVA, out uint pdwImplFlags, out uint pdwCPlusTypeFlag, out IntPtr ppValue)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001514 RID: 5396 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint GetFieldProps(uint mb, out uint pClass, StringBuilder szField, uint cchField, out uint pchField, out uint pdwAttr, out IntPtr ppvSigBlob, out uint pcbSigBlob, out uint pdwCPlusTypeFlag, out IntPtr ppValue)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001515 RID: 5397 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint GetPropertyProps(uint prop, out uint pClass, StringBuilder szProperty, uint cchProperty, out uint pchProperty, out uint pdwPropFlags, out IntPtr ppvSig, out uint pbSig, out uint pdwCPlusTypeFlag, out IntPtr ppDefaultValue, out uint pcchDefaultValue, out uint pmdSetter, out uint pmdGetter, uint[] rmdOtherMethod, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001516 RID: 5398 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint GetParamProps(uint tk, out uint pmd, out uint pulSequence, StringBuilder szName, uint cchName, out uint pchName, out uint pdwAttr, out uint pdwCPlusTypeFlag, out IntPtr ppValue)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001517 RID: 5399 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint GetCustomAttributeByName(uint tkObj, string szName, out IntPtr ppData)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001518 RID: 5400 RVA: 0x0004101A File Offset: 0x0003F21A
		public bool IsValidToken(uint tk)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001519 RID: 5401 RVA: 0x00041644 File Offset: 0x0003F844
		public unsafe uint GetNestedClassProps(uint tdNestedClass, uint* ptdEnclosingClass)
		{
			TypeDefinition typeDefinition;
			if (!this.TryGetType(tdNestedClass, out typeDefinition))
			{
				return 2147500037U;
			}
			if (ptdEnclosingClass != null)
			{
				*ptdEnclosingClass = (typeDefinition.IsNested ? typeDefinition.DeclaringType.MetadataToken.ToUInt32() : 0U);
			}
			return 0U;
		}

		// Token: 0x0600151A RID: 5402 RVA: 0x0004101A File Offset: 0x0003F21A
		public uint GetNativeCallConvFromSig(IntPtr pvSig, uint cbSig)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600151B RID: 5403 RVA: 0x0004101A File Offset: 0x0003F21A
		public int IsGlobal(uint pd)
		{
			throw new NotImplementedException();
		}

		// Token: 0x04000A66 RID: 2662
		private readonly ModuleDefinition module;

		// Token: 0x04000A67 RID: 2663
		private Dictionary<uint, TypeDefinition> types;

		// Token: 0x04000A68 RID: 2664
		private Dictionary<uint, MethodDefinition> methods;

		// Token: 0x04000A69 RID: 2665
		private const uint S_OK = 0U;

		// Token: 0x04000A6A RID: 2666
		private const uint E_FAIL = 2147500037U;
	}
}
