using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Mono.Cecil.Pdb
{
	// Token: 0x0200022D RID: 557
	internal class ModuleMetadata : IMetaDataEmit, IMetaDataImport
	{
		// Token: 0x06001137 RID: 4407 RVA: 0x00039477 File Offset: 0x00037677
		public ModuleMetadata(ModuleDefinition module)
		{
			this.module = module;
		}

		// Token: 0x06001138 RID: 4408 RVA: 0x00039486 File Offset: 0x00037686
		private bool TryGetType(uint token, out TypeDefinition type)
		{
			if (this.types == null)
			{
				this.InitializeMetadata(this.module);
			}
			return this.types.TryGetValue(token, out type);
		}

		// Token: 0x06001139 RID: 4409 RVA: 0x000394A9 File Offset: 0x000376A9
		private bool TryGetMethod(uint token, out MethodDefinition method)
		{
			if (this.methods == null)
			{
				this.InitializeMetadata(this.module);
			}
			return this.methods.TryGetValue(token, out method);
		}

		// Token: 0x0600113A RID: 4410 RVA: 0x000394CC File Offset: 0x000376CC
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

		// Token: 0x0600113B RID: 4411 RVA: 0x0003954C File Offset: 0x0003774C
		private void InitializeMethods(TypeDefinition type)
		{
			foreach (MethodDefinition methodDefinition in type.Methods)
			{
				this.methods.Add(methodDefinition.MetadataToken.ToUInt32(), methodDefinition);
			}
		}

		// Token: 0x0600113C RID: 4412 RVA: 0x000390D3 File Offset: 0x000372D3
		public void SetModuleProps(string szName)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600113D RID: 4413 RVA: 0x000390D3 File Offset: 0x000372D3
		public void Save(string szFile, uint dwSaveFlags)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600113E RID: 4414 RVA: 0x000390D3 File Offset: 0x000372D3
		public void SaveToStream(IntPtr pIStream, uint dwSaveFlags)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600113F RID: 4415 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint GetSaveSize(uint fSave)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001140 RID: 4416 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint DefineTypeDef(IntPtr szTypeDef, uint dwTypeDefFlags, uint tkExtends, IntPtr rtkImplements)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001141 RID: 4417 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint DefineNestedType(IntPtr szTypeDef, uint dwTypeDefFlags, uint tkExtends, IntPtr rtkImplements, uint tdEncloser)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001142 RID: 4418 RVA: 0x000390D3 File Offset: 0x000372D3
		public void SetHandler(object pUnk)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001143 RID: 4419 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint DefineMethod(uint td, IntPtr zName, uint dwMethodFlags, IntPtr pvSigBlob, uint cbSigBlob, uint ulCodeRVA, uint dwImplFlags)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001144 RID: 4420 RVA: 0x000390D3 File Offset: 0x000372D3
		public void DefineMethodImpl(uint td, uint tkBody, uint tkDecl)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001145 RID: 4421 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint DefineTypeRefByName(uint tkResolutionScope, IntPtr szName)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001146 RID: 4422 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint DefineImportType(IntPtr pAssemImport, IntPtr pbHashValue, uint cbHashValue, IMetaDataImport pImport, uint tdImport, IntPtr pAssemEmit)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001147 RID: 4423 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint DefineMemberRef(uint tkImport, string szName, IntPtr pvSigBlob, uint cbSigBlob)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001148 RID: 4424 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint DefineImportMember(IntPtr pAssemImport, IntPtr pbHashValue, uint cbHashValue, IMetaDataImport pImport, uint mbMember, IntPtr pAssemEmit, uint tkParent)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001149 RID: 4425 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint DefineEvent(uint td, string szEvent, uint dwEventFlags, uint tkEventType, uint mdAddOn, uint mdRemoveOn, uint mdFire, IntPtr rmdOtherMethods)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600114A RID: 4426 RVA: 0x000390D3 File Offset: 0x000372D3
		public void SetClassLayout(uint td, uint dwPackSize, IntPtr rFieldOffsets, uint ulClassSize)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600114B RID: 4427 RVA: 0x000390D3 File Offset: 0x000372D3
		public void DeleteClassLayout(uint td)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600114C RID: 4428 RVA: 0x000390D3 File Offset: 0x000372D3
		public void SetFieldMarshal(uint tk, IntPtr pvNativeType, uint cbNativeType)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600114D RID: 4429 RVA: 0x000390D3 File Offset: 0x000372D3
		public void DeleteFieldMarshal(uint tk)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600114E RID: 4430 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint DefinePermissionSet(uint tk, uint dwAction, IntPtr pvPermission, uint cbPermission)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600114F RID: 4431 RVA: 0x000390D3 File Offset: 0x000372D3
		public void SetRVA(uint md, uint ulRVA)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001150 RID: 4432 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint GetTokenFromSig(IntPtr pvSig, uint cbSig)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001151 RID: 4433 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint DefineModuleRef(string szName)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001152 RID: 4434 RVA: 0x000390D3 File Offset: 0x000372D3
		public void SetParent(uint mr, uint tk)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001153 RID: 4435 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint GetTokenFromTypeSpec(IntPtr pvSig, uint cbSig)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001154 RID: 4436 RVA: 0x000390D3 File Offset: 0x000372D3
		public void SaveToMemory(IntPtr pbData, uint cbData)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001155 RID: 4437 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint DefineUserString(string szString, uint cchString)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001156 RID: 4438 RVA: 0x000390D3 File Offset: 0x000372D3
		public void DeleteToken(uint tkObj)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001157 RID: 4439 RVA: 0x000390D3 File Offset: 0x000372D3
		public void SetMethodProps(uint md, uint dwMethodFlags, uint ulCodeRVA, uint dwImplFlags)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001158 RID: 4440 RVA: 0x000390D3 File Offset: 0x000372D3
		public void SetTypeDefProps(uint td, uint dwTypeDefFlags, uint tkExtends, IntPtr rtkImplements)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001159 RID: 4441 RVA: 0x000390D3 File Offset: 0x000372D3
		public void SetEventProps(uint ev, uint dwEventFlags, uint tkEventType, uint mdAddOn, uint mdRemoveOn, uint mdFire, IntPtr rmdOtherMethods)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600115A RID: 4442 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint SetPermissionSetProps(uint tk, uint dwAction, IntPtr pvPermission, uint cbPermission)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600115B RID: 4443 RVA: 0x000390D3 File Offset: 0x000372D3
		public void DefinePinvokeMap(uint tk, uint dwMappingFlags, string szImportName, uint mrImportDLL)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600115C RID: 4444 RVA: 0x000390D3 File Offset: 0x000372D3
		public void SetPinvokeMap(uint tk, uint dwMappingFlags, string szImportName, uint mrImportDLL)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600115D RID: 4445 RVA: 0x000390D3 File Offset: 0x000372D3
		public void DeletePinvokeMap(uint tk)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600115E RID: 4446 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint DefineCustomAttribute(uint tkObj, uint tkType, IntPtr pCustomAttribute, uint cbCustomAttribute)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600115F RID: 4447 RVA: 0x000390D3 File Offset: 0x000372D3
		public void SetCustomAttributeValue(uint pcv, IntPtr pCustomAttribute, uint cbCustomAttribute)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001160 RID: 4448 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint DefineField(uint td, string szName, uint dwFieldFlags, IntPtr pvSigBlob, uint cbSigBlob, uint dwCPlusTypeFlag, IntPtr pValue, uint cchValue)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001161 RID: 4449 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint DefineProperty(uint td, string szProperty, uint dwPropFlags, IntPtr pvSig, uint cbSig, uint dwCPlusTypeFlag, IntPtr pValue, uint cchValue, uint mdSetter, uint mdGetter, IntPtr rmdOtherMethods)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001162 RID: 4450 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint DefineParam(uint md, uint ulParamSeq, string szName, uint dwParamFlags, uint dwCPlusTypeFlag, IntPtr pValue, uint cchValue)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001163 RID: 4451 RVA: 0x000390D3 File Offset: 0x000372D3
		public void SetFieldProps(uint fd, uint dwFieldFlags, uint dwCPlusTypeFlag, IntPtr pValue, uint cchValue)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001164 RID: 4452 RVA: 0x000390D3 File Offset: 0x000372D3
		public void SetPropertyProps(uint pr, uint dwPropFlags, uint dwCPlusTypeFlag, IntPtr pValue, uint cchValue, uint mdSetter, uint mdGetter, IntPtr rmdOtherMethods)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001165 RID: 4453 RVA: 0x000390D3 File Offset: 0x000372D3
		public void SetParamProps(uint pd, string szName, uint dwParamFlags, uint dwCPlusTypeFlag, IntPtr pValue, uint cchValue)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001166 RID: 4454 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint DefineSecurityAttributeSet(uint tkObj, IntPtr rSecAttrs, uint cSecAttrs)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001167 RID: 4455 RVA: 0x000390D3 File Offset: 0x000372D3
		public void ApplyEditAndContinue(object pImport)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001168 RID: 4456 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint TranslateSigWithScope(IntPtr pAssemImport, IntPtr pbHashValue, uint cbHashValue, IMetaDataImport import, IntPtr pbSigBlob, uint cbSigBlob, IntPtr pAssemEmit, IMetaDataEmit emit, IntPtr pvTranslatedSig, uint cbTranslatedSigMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001169 RID: 4457 RVA: 0x000390D3 File Offset: 0x000372D3
		public void SetMethodImplFlags(uint md, uint dwImplFlags)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600116A RID: 4458 RVA: 0x000390D3 File Offset: 0x000372D3
		public void SetFieldRVA(uint fd, uint ulRVA)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600116B RID: 4459 RVA: 0x000390D3 File Offset: 0x000372D3
		public void Merge(IMetaDataImport pImport, IntPtr pHostMapToken, object pHandler)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600116C RID: 4460 RVA: 0x000390D3 File Offset: 0x000372D3
		public void MergeEnd()
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600116D RID: 4461 RVA: 0x000390D3 File Offset: 0x000372D3
		public void CloseEnum(uint hEnum)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600116E RID: 4462 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint CountEnum(uint hEnum)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600116F RID: 4463 RVA: 0x000390D3 File Offset: 0x000372D3
		public void ResetEnum(uint hEnum, uint ulPos)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001170 RID: 4464 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint EnumTypeDefs(ref uint phEnum, uint[] rTypeDefs, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001171 RID: 4465 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint EnumInterfaceImpls(ref uint phEnum, uint td, uint[] rImpls, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001172 RID: 4466 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint EnumTypeRefs(ref uint phEnum, uint[] rTypeRefs, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001173 RID: 4467 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint FindTypeDefByName(string szTypeDef, uint tkEnclosingClass)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001174 RID: 4468 RVA: 0x000390D3 File Offset: 0x000372D3
		public Guid GetScopeProps(StringBuilder szName, uint cchName, out uint pchName)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001175 RID: 4469 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint GetModuleFromScope()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001176 RID: 4470 RVA: 0x000395B4 File Offset: 0x000377B4
		public uint GetTypeDefProps(uint td, IntPtr szTypeDef, uint cchTypeDef, out uint pchTypeDef, IntPtr pdwTypeDefFlags)
		{
			TypeDefinition typeDefinition;
			if (!this.TryGetType(td, out typeDefinition))
			{
				Marshal.WriteInt16(szTypeDef, 0);
				pchTypeDef = 1U;
				return 0U;
			}
			ModuleMetadata.WriteString(typeDefinition.IsNested ? typeDefinition.Name : typeDefinition.FullName, szTypeDef, cchTypeDef, out pchTypeDef);
			ModuleMetadata.WriteIntPtr(pdwTypeDefFlags, (uint)typeDefinition.Attributes);
			if (typeDefinition.BaseType == null)
			{
				return 0U;
			}
			return typeDefinition.BaseType.MetadataToken.ToUInt32();
		}

		// Token: 0x06001177 RID: 4471 RVA: 0x00039622 File Offset: 0x00037822
		private static void WriteIntPtr(IntPtr ptr, uint value)
		{
			if (ptr == IntPtr.Zero)
			{
				return;
			}
			Marshal.WriteInt32(ptr, (int)value);
		}

		// Token: 0x06001178 RID: 4472 RVA: 0x0003963C File Offset: 0x0003783C
		private static void WriteString(string str, IntPtr buffer, uint bufferSize, out uint chars)
		{
			uint num = (((long)(str.Length + 1) >= (long)((ulong)bufferSize)) ? (bufferSize - 1U) : ((uint)str.Length));
			chars = num + 1U;
			int num2 = 0;
			int num3 = 0;
			while ((long)num3 < (long)((ulong)num))
			{
				Marshal.WriteInt16(buffer, num2, str[num3]);
				num2 += 2;
				num3++;
			}
			Marshal.WriteInt16(buffer, num2, 0);
		}

		// Token: 0x06001179 RID: 4473 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint GetInterfaceImplProps(uint iiImpl, out uint pClass)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600117A RID: 4474 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint GetTypeRefProps(uint tr, out uint ptkResolutionScope, StringBuilder szName, uint cchName)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600117B RID: 4475 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint ResolveTypeRef(uint tr, ref Guid riid, out object ppIScope)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600117C RID: 4476 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint EnumMembers(ref uint phEnum, uint cl, uint[] rMembers, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600117D RID: 4477 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint EnumMembersWithName(ref uint phEnum, uint cl, string szName, uint[] rMembers, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600117E RID: 4478 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint EnumMethods(ref uint phEnum, uint cl, IntPtr rMethods, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600117F RID: 4479 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint EnumMethodsWithName(ref uint phEnum, uint cl, string szName, uint[] rMethods, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001180 RID: 4480 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint EnumFields(ref uint phEnum, uint cl, IntPtr rFields, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001181 RID: 4481 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint EnumFieldsWithName(ref uint phEnum, uint cl, string szName, uint[] rFields, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001182 RID: 4482 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint EnumParams(ref uint phEnum, uint mb, uint[] rParams, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001183 RID: 4483 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint EnumMemberRefs(ref uint phEnum, uint tkParent, uint[] rMemberRefs, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001184 RID: 4484 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint EnumMethodImpls(ref uint phEnum, uint td, uint[] rMethodBody, uint[] rMethodDecl, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001185 RID: 4485 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint EnumPermissionSets(ref uint phEnum, uint tk, uint dwActions, uint[] rPermission, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001186 RID: 4486 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint FindMember(uint td, string szName, byte[] pvSigBlob, uint cbSigBlob)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001187 RID: 4487 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint FindMethod(uint td, string szName, byte[] pvSigBlob, uint cbSigBlob)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001188 RID: 4488 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint FindField(uint td, string szName, byte[] pvSigBlob, uint cbSigBlob)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001189 RID: 4489 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint FindMemberRef(uint td, string szName, byte[] pvSigBlob, uint cbSigBlob)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600118A RID: 4490 RVA: 0x00039694 File Offset: 0x00037894
		public uint GetMethodProps(uint mb, out uint pClass, IntPtr szMethod, uint cchMethod, out uint pchMethod, IntPtr pdwAttr, IntPtr ppvSigBlob, IntPtr pcbSigBlob, IntPtr pulCodeRVA)
		{
			MethodDefinition methodDefinition;
			if (!this.TryGetMethod(mb, out methodDefinition))
			{
				Marshal.WriteInt16(szMethod, 0);
				pchMethod = 1U;
				pClass = 0U;
				return 0U;
			}
			pClass = methodDefinition.DeclaringType.MetadataToken.ToUInt32();
			ModuleMetadata.WriteString(methodDefinition.Name, szMethod, cchMethod, out pchMethod);
			ModuleMetadata.WriteIntPtr(pdwAttr, (uint)methodDefinition.Attributes);
			ModuleMetadata.WriteIntPtr(pulCodeRVA, (uint)methodDefinition.RVA);
			return (uint)methodDefinition.ImplAttributes;
		}

		// Token: 0x0600118B RID: 4491 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint GetMemberRefProps(uint mr, ref uint ptk, StringBuilder szMember, uint cchMember, out uint pchMember, out IntPtr ppvSigBlob)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600118C RID: 4492 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint EnumProperties(ref uint phEnum, uint td, IntPtr rProperties, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600118D RID: 4493 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint EnumEvents(ref uint phEnum, uint td, IntPtr rEvents, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600118E RID: 4494 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint GetEventProps(uint ev, out uint pClass, StringBuilder szEvent, uint cchEvent, out uint pchEvent, out uint pdwEventFlags, out uint ptkEventType, out uint pmdAddOn, out uint pmdRemoveOn, out uint pmdFire, uint[] rmdOtherMethod, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600118F RID: 4495 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint EnumMethodSemantics(ref uint phEnum, uint mb, uint[] rEventProp, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001190 RID: 4496 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint GetMethodSemantics(uint mb, uint tkEventProp)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001191 RID: 4497 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint GetClassLayout(uint td, out uint pdwPackSize, IntPtr rFieldOffset, uint cMax, out uint pcFieldOffset)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001192 RID: 4498 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint GetFieldMarshal(uint tk, out IntPtr ppvNativeType)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001193 RID: 4499 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint GetRVA(uint tk, out uint pulCodeRVA)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001194 RID: 4500 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint GetPermissionSetProps(uint pm, out uint pdwAction, out IntPtr ppvPermission)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001195 RID: 4501 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint GetSigFromToken(uint mdSig, out IntPtr ppvSig)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001196 RID: 4502 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint GetModuleRefProps(uint mur, StringBuilder szName, uint cchName)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001197 RID: 4503 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint EnumModuleRefs(ref uint phEnum, uint[] rModuleRefs, uint cmax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001198 RID: 4504 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint GetTypeSpecFromToken(uint typespec, out IntPtr ppvSig)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001199 RID: 4505 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint GetNameFromToken(uint tk)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600119A RID: 4506 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint EnumUnresolvedMethods(ref uint phEnum, uint[] rMethods, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600119B RID: 4507 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint GetUserString(uint stk, StringBuilder szString, uint cchString)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600119C RID: 4508 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint GetPinvokeMap(uint tk, out uint pdwMappingFlags, StringBuilder szImportName, uint cchImportName, out uint pchImportName)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600119D RID: 4509 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint EnumSignatures(ref uint phEnum, uint[] rSignatures, uint cmax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600119E RID: 4510 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint EnumTypeSpecs(ref uint phEnum, uint[] rTypeSpecs, uint cmax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600119F RID: 4511 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint EnumUserStrings(ref uint phEnum, uint[] rStrings, uint cmax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060011A0 RID: 4512 RVA: 0x000390D3 File Offset: 0x000372D3
		public int GetParamForMethodIndex(uint md, uint ulParamSeq, out uint pParam)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060011A1 RID: 4513 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint EnumCustomAttributes(ref uint phEnum, uint tk, uint tkType, uint[] rCustomAttributes, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060011A2 RID: 4514 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint GetCustomAttributeProps(uint cv, out uint ptkObj, out uint ptkType, out IntPtr ppBlob)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060011A3 RID: 4515 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint FindTypeRef(uint tkResolutionScope, string szName)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060011A4 RID: 4516 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint GetMemberProps(uint mb, out uint pClass, StringBuilder szMember, uint cchMember, out uint pchMember, out uint pdwAttr, out IntPtr ppvSigBlob, out uint pcbSigBlob, out uint pulCodeRVA, out uint pdwImplFlags, out uint pdwCPlusTypeFlag, out IntPtr ppValue)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060011A5 RID: 4517 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint GetFieldProps(uint mb, out uint pClass, StringBuilder szField, uint cchField, out uint pchField, out uint pdwAttr, out IntPtr ppvSigBlob, out uint pcbSigBlob, out uint pdwCPlusTypeFlag, out IntPtr ppValue)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060011A6 RID: 4518 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint GetPropertyProps(uint prop, out uint pClass, StringBuilder szProperty, uint cchProperty, out uint pchProperty, out uint pdwPropFlags, out IntPtr ppvSig, out uint pbSig, out uint pdwCPlusTypeFlag, out IntPtr ppDefaultValue, out uint pcchDefaultValue, out uint pmdSetter, out uint pmdGetter, uint[] rmdOtherMethod, uint cMax)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060011A7 RID: 4519 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint GetParamProps(uint tk, out uint pmd, out uint pulSequence, StringBuilder szName, uint cchName, out uint pchName, out uint pdwAttr, out uint pdwCPlusTypeFlag, out IntPtr ppValue)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060011A8 RID: 4520 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint GetCustomAttributeByName(uint tkObj, string szName, out IntPtr ppData)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060011A9 RID: 4521 RVA: 0x000390D3 File Offset: 0x000372D3
		public bool IsValidToken(uint tk)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060011AA RID: 4522 RVA: 0x00039704 File Offset: 0x00037904
		public uint GetNestedClassProps(uint tdNestedClass)
		{
			TypeDefinition typeDefinition;
			if (!this.TryGetType(tdNestedClass, out typeDefinition))
			{
				return 0U;
			}
			if (!typeDefinition.IsNested)
			{
				return 0U;
			}
			return typeDefinition.DeclaringType.MetadataToken.ToUInt32();
		}

		// Token: 0x060011AB RID: 4523 RVA: 0x000390D3 File Offset: 0x000372D3
		public uint GetNativeCallConvFromSig(IntPtr pvSig, uint cbSig)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060011AC RID: 4524 RVA: 0x000390D3 File Offset: 0x000372D3
		public int IsGlobal(uint pd)
		{
			throw new NotImplementedException();
		}

		// Token: 0x04000A27 RID: 2599
		private readonly ModuleDefinition module;

		// Token: 0x04000A28 RID: 2600
		private Dictionary<uint, TypeDefinition> types;

		// Token: 0x04000A29 RID: 2601
		private Dictionary<uint, MethodDefinition> methods;
	}
}
