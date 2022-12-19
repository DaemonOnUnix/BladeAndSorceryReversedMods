using System;
using System.Text;
using Mono.Cecil.Cil;
using Mono.Cecil.Metadata;
using Mono.Cecil.PE;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x020001EE RID: 494
	internal sealed class SignatureWriter : ByteBuffer
	{
		// Token: 0x060009D1 RID: 2513 RVA: 0x000247A0 File Offset: 0x000229A0
		public SignatureWriter(MetadataBuilder metadata)
			: base(6)
		{
			this.metadata = metadata;
		}

		// Token: 0x060009D2 RID: 2514 RVA: 0x000247B0 File Offset: 0x000229B0
		public void WriteElementType(ElementType element_type)
		{
			base.WriteByte((byte)element_type);
		}

		// Token: 0x060009D3 RID: 2515 RVA: 0x000247BC File Offset: 0x000229BC
		public void WriteUTF8String(string @string)
		{
			if (@string == null)
			{
				base.WriteByte(byte.MaxValue);
				return;
			}
			byte[] bytes = Encoding.UTF8.GetBytes(@string);
			base.WriteCompressedUInt32((uint)bytes.Length);
			base.WriteBytes(bytes);
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x000247F4 File Offset: 0x000229F4
		public void WriteMethodSignature(IMethodSignature method)
		{
			byte b = (byte)method.CallingConvention;
			if (method.HasThis)
			{
				b |= 32;
			}
			if (method.ExplicitThis)
			{
				b |= 64;
			}
			IGenericParameterProvider genericParameterProvider = method as IGenericParameterProvider;
			int num = ((genericParameterProvider != null && genericParameterProvider.HasGenericParameters) ? genericParameterProvider.GenericParameters.Count : 0);
			if (num > 0)
			{
				b |= 16;
			}
			int num2 = (method.HasParameters ? method.Parameters.Count : 0);
			base.WriteByte(b);
			if (num > 0)
			{
				base.WriteCompressedUInt32((uint)num);
			}
			base.WriteCompressedUInt32((uint)num2);
			this.WriteTypeSignature(method.ReturnType);
			if (num2 == 0)
			{
				return;
			}
			Collection<ParameterDefinition> parameters = method.Parameters;
			for (int i = 0; i < num2; i++)
			{
				this.WriteTypeSignature(parameters[i].ParameterType);
			}
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x000248BB File Offset: 0x00022ABB
		private uint MakeTypeDefOrRefCodedRID(TypeReference type)
		{
			return CodedIndex.TypeDefOrRef.CompressMetadataToken(this.metadata.LookupToken(type));
		}

		// Token: 0x060009D6 RID: 2518 RVA: 0x000248CF File Offset: 0x00022ACF
		public void WriteTypeToken(TypeReference type)
		{
			base.WriteCompressedUInt32(this.MakeTypeDefOrRefCodedRID(type));
		}

		// Token: 0x060009D7 RID: 2519 RVA: 0x000248E0 File Offset: 0x00022AE0
		public void WriteTypeSignature(TypeReference type)
		{
			if (type == null)
			{
				throw new ArgumentNullException();
			}
			ElementType etype = type.etype;
			if (etype <= ElementType.GenericInst)
			{
				if (etype == ElementType.None)
				{
					this.WriteElementType(type.IsValueType ? ElementType.ValueType : ElementType.Class);
					base.WriteCompressedUInt32(this.MakeTypeDefOrRefCodedRID(type));
					return;
				}
				switch (etype)
				{
				case ElementType.Ptr:
				case ElementType.ByRef:
					goto IL_D7;
				case ElementType.ValueType:
				case ElementType.Class:
					goto IL_16F;
				case ElementType.Var:
					break;
				case ElementType.Array:
				{
					ArrayType arrayType = (ArrayType)type;
					if (!arrayType.IsVector)
					{
						this.WriteArrayTypeSignature(arrayType);
						return;
					}
					this.WriteElementType(ElementType.SzArray);
					this.WriteTypeSignature(arrayType.ElementType);
					return;
				}
				case ElementType.GenericInst:
				{
					GenericInstanceType genericInstanceType = (GenericInstanceType)type;
					this.WriteElementType(ElementType.GenericInst);
					this.WriteElementType(genericInstanceType.IsValueType ? ElementType.ValueType : ElementType.Class);
					base.WriteCompressedUInt32(this.MakeTypeDefOrRefCodedRID(genericInstanceType.ElementType));
					this.WriteGenericInstanceSignature(genericInstanceType);
					return;
				}
				default:
					goto IL_16F;
				}
			}
			else
			{
				switch (etype)
				{
				case ElementType.FnPtr:
				{
					FunctionPointerType functionPointerType = (FunctionPointerType)type;
					this.WriteElementType(ElementType.FnPtr);
					this.WriteMethodSignature(functionPointerType);
					return;
				}
				case ElementType.Object:
				case ElementType.SzArray:
					goto IL_16F;
				case ElementType.MVar:
					break;
				case ElementType.CModReqD:
				case ElementType.CModOpt:
				{
					IModifierType modifierType = (IModifierType)type;
					this.WriteModifierSignature(etype, modifierType);
					return;
				}
				default:
					if (etype != ElementType.Sentinel && etype != ElementType.Pinned)
					{
						goto IL_16F;
					}
					goto IL_D7;
				}
			}
			GenericParameter genericParameter = (GenericParameter)type;
			this.WriteElementType(etype);
			int position = genericParameter.Position;
			if (position == -1)
			{
				throw new NotSupportedException();
			}
			base.WriteCompressedUInt32((uint)position);
			return;
			IL_D7:
			TypeSpecification typeSpecification = (TypeSpecification)type;
			this.WriteElementType(etype);
			this.WriteTypeSignature(typeSpecification.ElementType);
			return;
			IL_16F:
			if (!this.TryWriteElementType(type))
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x060009D8 RID: 2520 RVA: 0x00024A6C File Offset: 0x00022C6C
		private void WriteArrayTypeSignature(ArrayType array)
		{
			this.WriteElementType(ElementType.Array);
			this.WriteTypeSignature(array.ElementType);
			Collection<ArrayDimension> dimensions = array.Dimensions;
			int count = dimensions.Count;
			base.WriteCompressedUInt32((uint)count);
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < count; i++)
			{
				ArrayDimension arrayDimension = dimensions[i];
				if (arrayDimension.UpperBound != null)
				{
					num++;
					num2++;
				}
				else if (arrayDimension.LowerBound != null)
				{
					num2++;
				}
			}
			int[] array2 = new int[num];
			int[] array3 = new int[num2];
			for (int j = 0; j < num2; j++)
			{
				ArrayDimension arrayDimension2 = dimensions[j];
				array3[j] = arrayDimension2.LowerBound.GetValueOrDefault();
				if (arrayDimension2.UpperBound != null)
				{
					array2[j] = arrayDimension2.UpperBound.Value - array3[j] + 1;
				}
			}
			base.WriteCompressedUInt32((uint)num);
			for (int k = 0; k < num; k++)
			{
				base.WriteCompressedUInt32((uint)array2[k]);
			}
			base.WriteCompressedUInt32((uint)num2);
			for (int l = 0; l < num2; l++)
			{
				base.WriteCompressedInt32(array3[l]);
			}
		}

		// Token: 0x060009D9 RID: 2521 RVA: 0x00024BA4 File Offset: 0x00022DA4
		public void WriteGenericInstanceSignature(IGenericInstance instance)
		{
			Collection<TypeReference> genericArguments = instance.GenericArguments;
			int count = genericArguments.Count;
			base.WriteCompressedUInt32((uint)count);
			for (int i = 0; i < count; i++)
			{
				this.WriteTypeSignature(genericArguments[i]);
			}
		}

		// Token: 0x060009DA RID: 2522 RVA: 0x00024BDF File Offset: 0x00022DDF
		private void WriteModifierSignature(ElementType element_type, IModifierType type)
		{
			this.WriteElementType(element_type);
			base.WriteCompressedUInt32(this.MakeTypeDefOrRefCodedRID(type.ModifierType));
			this.WriteTypeSignature(type.ElementType);
		}

		// Token: 0x060009DB RID: 2523 RVA: 0x00024C08 File Offset: 0x00022E08
		private bool TryWriteElementType(TypeReference type)
		{
			ElementType etype = type.etype;
			if (etype == ElementType.None)
			{
				return false;
			}
			this.WriteElementType(etype);
			return true;
		}

		// Token: 0x060009DC RID: 2524 RVA: 0x00024C29 File Offset: 0x00022E29
		public void WriteConstantString(string value)
		{
			if (value != null)
			{
				base.WriteBytes(Encoding.Unicode.GetBytes(value));
				return;
			}
			base.WriteByte(byte.MaxValue);
		}

		// Token: 0x060009DD RID: 2525 RVA: 0x00024C4B File Offset: 0x00022E4B
		public void WriteConstantPrimitive(object value)
		{
			this.WritePrimitiveValue(value);
		}

		// Token: 0x060009DE RID: 2526 RVA: 0x00024C54 File Offset: 0x00022E54
		public void WriteCustomAttributeConstructorArguments(CustomAttribute attribute)
		{
			if (!attribute.HasConstructorArguments)
			{
				return;
			}
			Collection<CustomAttributeArgument> constructorArguments = attribute.ConstructorArguments;
			Collection<ParameterDefinition> parameters = attribute.Constructor.Parameters;
			if (parameters.Count != constructorArguments.Count)
			{
				throw new InvalidOperationException();
			}
			for (int i = 0; i < constructorArguments.Count; i++)
			{
				this.WriteCustomAttributeFixedArgument(parameters[i].ParameterType, constructorArguments[i]);
			}
		}

		// Token: 0x060009DF RID: 2527 RVA: 0x00024CBB File Offset: 0x00022EBB
		private void WriteCustomAttributeFixedArgument(TypeReference type, CustomAttributeArgument argument)
		{
			if (type.IsArray)
			{
				this.WriteCustomAttributeFixedArrayArgument((ArrayType)type, argument);
				return;
			}
			this.WriteCustomAttributeElement(type, argument);
		}

		// Token: 0x060009E0 RID: 2528 RVA: 0x00024CDC File Offset: 0x00022EDC
		private void WriteCustomAttributeFixedArrayArgument(ArrayType type, CustomAttributeArgument argument)
		{
			CustomAttributeArgument[] array = argument.Value as CustomAttributeArgument[];
			if (array == null)
			{
				base.WriteUInt32(uint.MaxValue);
				return;
			}
			base.WriteInt32(array.Length);
			if (array.Length == 0)
			{
				return;
			}
			TypeReference elementType = type.ElementType;
			for (int i = 0; i < array.Length; i++)
			{
				this.WriteCustomAttributeElement(elementType, array[i]);
			}
		}

		// Token: 0x060009E1 RID: 2529 RVA: 0x00024D34 File Offset: 0x00022F34
		private void WriteCustomAttributeElement(TypeReference type, CustomAttributeArgument argument)
		{
			if (type.IsArray)
			{
				this.WriteCustomAttributeFixedArrayArgument((ArrayType)type, argument);
				return;
			}
			if (type.etype == ElementType.Object)
			{
				argument = (CustomAttributeArgument)argument.Value;
				type = argument.Type;
				this.WriteCustomAttributeFieldOrPropType(type);
				this.WriteCustomAttributeElement(type, argument);
				return;
			}
			this.WriteCustomAttributeValue(type, argument.Value);
		}

		// Token: 0x060009E2 RID: 2530 RVA: 0x00024D98 File Offset: 0x00022F98
		private void WriteCustomAttributeValue(TypeReference type, object value)
		{
			ElementType etype = type.etype;
			if (etype != ElementType.None)
			{
				if (etype != ElementType.String)
				{
					this.WritePrimitiveValue(value);
					return;
				}
				string text = (string)value;
				if (text == null)
				{
					base.WriteByte(byte.MaxValue);
					return;
				}
				this.WriteUTF8String(text);
				return;
			}
			else
			{
				if (type.IsTypeOf("System", "Type"))
				{
					this.WriteCustomAttributeTypeValue((TypeReference)value);
					return;
				}
				this.WriteCustomAttributeEnumValue(type, value);
				return;
			}
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x00024E04 File Offset: 0x00023004
		private void WriteCustomAttributeTypeValue(TypeReference value)
		{
			TypeDefinition typeDefinition = value as TypeDefinition;
			if (typeDefinition != null)
			{
				TypeDefinition typeDefinition2 = typeDefinition;
				while (typeDefinition2.DeclaringType != null)
				{
					typeDefinition2 = typeDefinition2.DeclaringType;
				}
				if (WindowsRuntimeProjections.IsClrImplementationType(typeDefinition2))
				{
					WindowsRuntimeProjections.Project(typeDefinition2);
					this.WriteTypeReference(value);
					WindowsRuntimeProjections.RemoveProjection(typeDefinition2);
					return;
				}
			}
			this.WriteTypeReference(value);
		}

		// Token: 0x060009E4 RID: 2532 RVA: 0x00024E54 File Offset: 0x00023054
		private void WritePrimitiveValue(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException();
			}
			switch (Type.GetTypeCode(value.GetType()))
			{
			case TypeCode.Boolean:
				base.WriteByte(((bool)value) ? 1 : 0);
				return;
			case TypeCode.Char:
				base.WriteInt16((short)((char)value));
				return;
			case TypeCode.SByte:
				base.WriteSByte((sbyte)value);
				return;
			case TypeCode.Byte:
				base.WriteByte((byte)value);
				return;
			case TypeCode.Int16:
				base.WriteInt16((short)value);
				return;
			case TypeCode.UInt16:
				base.WriteUInt16((ushort)value);
				return;
			case TypeCode.Int32:
				base.WriteInt32((int)value);
				return;
			case TypeCode.UInt32:
				base.WriteUInt32((uint)value);
				return;
			case TypeCode.Int64:
				base.WriteInt64((long)value);
				return;
			case TypeCode.UInt64:
				base.WriteUInt64((ulong)value);
				return;
			case TypeCode.Single:
				base.WriteSingle((float)value);
				return;
			case TypeCode.Double:
				base.WriteDouble((double)value);
				return;
			default:
				throw new NotSupportedException(value.GetType().FullName);
			}
		}

		// Token: 0x060009E5 RID: 2533 RVA: 0x00024F68 File Offset: 0x00023168
		private void WriteCustomAttributeEnumValue(TypeReference enum_type, object value)
		{
			TypeDefinition typeDefinition = enum_type.CheckedResolve();
			if (!typeDefinition.IsEnum)
			{
				throw new ArgumentException();
			}
			this.WriteCustomAttributeValue(typeDefinition.GetEnumUnderlyingType(), value);
		}

		// Token: 0x060009E6 RID: 2534 RVA: 0x00024F98 File Offset: 0x00023198
		private void WriteCustomAttributeFieldOrPropType(TypeReference type)
		{
			if (type.IsArray)
			{
				ArrayType arrayType = (ArrayType)type;
				this.WriteElementType(ElementType.SzArray);
				this.WriteCustomAttributeFieldOrPropType(arrayType.ElementType);
				return;
			}
			ElementType etype = type.etype;
			if (etype != ElementType.None)
			{
				if (etype == ElementType.Object)
				{
					this.WriteElementType(ElementType.Boxed);
					return;
				}
				this.WriteElementType(etype);
				return;
			}
			else
			{
				if (type.IsTypeOf("System", "Type"))
				{
					this.WriteElementType(ElementType.Type);
					return;
				}
				this.WriteElementType(ElementType.Enum);
				this.WriteTypeReference(type);
				return;
			}
		}

		// Token: 0x060009E7 RID: 2535 RVA: 0x00025014 File Offset: 0x00023214
		public void WriteCustomAttributeNamedArguments(CustomAttribute attribute)
		{
			int namedArgumentCount = SignatureWriter.GetNamedArgumentCount(attribute);
			base.WriteUInt16((ushort)namedArgumentCount);
			if (namedArgumentCount == 0)
			{
				return;
			}
			this.WriteICustomAttributeNamedArguments(attribute);
		}

		// Token: 0x060009E8 RID: 2536 RVA: 0x0002503C File Offset: 0x0002323C
		private static int GetNamedArgumentCount(ICustomAttribute attribute)
		{
			int num = 0;
			if (attribute.HasFields)
			{
				num += attribute.Fields.Count;
			}
			if (attribute.HasProperties)
			{
				num += attribute.Properties.Count;
			}
			return num;
		}

		// Token: 0x060009E9 RID: 2537 RVA: 0x00025078 File Offset: 0x00023278
		private void WriteICustomAttributeNamedArguments(ICustomAttribute attribute)
		{
			if (attribute.HasFields)
			{
				this.WriteCustomAttributeNamedArguments(83, attribute.Fields);
			}
			if (attribute.HasProperties)
			{
				this.WriteCustomAttributeNamedArguments(84, attribute.Properties);
			}
		}

		// Token: 0x060009EA RID: 2538 RVA: 0x000250A8 File Offset: 0x000232A8
		private void WriteCustomAttributeNamedArguments(byte kind, Collection<CustomAttributeNamedArgument> named_arguments)
		{
			for (int i = 0; i < named_arguments.Count; i++)
			{
				this.WriteCustomAttributeNamedArgument(kind, named_arguments[i]);
			}
		}

		// Token: 0x060009EB RID: 2539 RVA: 0x000250D4 File Offset: 0x000232D4
		private void WriteCustomAttributeNamedArgument(byte kind, CustomAttributeNamedArgument named_argument)
		{
			CustomAttributeArgument argument = named_argument.Argument;
			base.WriteByte(kind);
			this.WriteCustomAttributeFieldOrPropType(argument.Type);
			this.WriteUTF8String(named_argument.Name);
			this.WriteCustomAttributeFixedArgument(argument.Type, argument);
		}

		// Token: 0x060009EC RID: 2540 RVA: 0x00025118 File Offset: 0x00023318
		private void WriteSecurityAttribute(SecurityAttribute attribute)
		{
			this.WriteTypeReference(attribute.AttributeType);
			int namedArgumentCount = SignatureWriter.GetNamedArgumentCount(attribute);
			if (namedArgumentCount == 0)
			{
				base.WriteCompressedUInt32(1U);
				base.WriteCompressedUInt32(0U);
				return;
			}
			SignatureWriter signatureWriter = new SignatureWriter(this.metadata);
			signatureWriter.WriteCompressedUInt32((uint)namedArgumentCount);
			signatureWriter.WriteICustomAttributeNamedArguments(attribute);
			base.WriteCompressedUInt32((uint)signatureWriter.length);
			base.WriteBytes(signatureWriter);
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x00025178 File Offset: 0x00023378
		public void WriteSecurityDeclaration(SecurityDeclaration declaration)
		{
			base.WriteByte(46);
			Collection<SecurityAttribute> security_attributes = declaration.security_attributes;
			if (security_attributes == null)
			{
				throw new NotSupportedException();
			}
			base.WriteCompressedUInt32((uint)security_attributes.Count);
			for (int i = 0; i < security_attributes.Count; i++)
			{
				this.WriteSecurityAttribute(security_attributes[i]);
			}
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x000251C8 File Offset: 0x000233C8
		public void WriteXmlSecurityDeclaration(SecurityDeclaration declaration)
		{
			string xmlSecurityDeclaration = SignatureWriter.GetXmlSecurityDeclaration(declaration);
			if (xmlSecurityDeclaration == null)
			{
				throw new NotSupportedException();
			}
			base.WriteBytes(Encoding.Unicode.GetBytes(xmlSecurityDeclaration));
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x000251F8 File Offset: 0x000233F8
		private static string GetXmlSecurityDeclaration(SecurityDeclaration declaration)
		{
			if (declaration.security_attributes == null || declaration.security_attributes.Count != 1)
			{
				return null;
			}
			SecurityAttribute securityAttribute = declaration.security_attributes[0];
			if (!securityAttribute.AttributeType.IsTypeOf("System.Security.Permissions", "PermissionSetAttribute"))
			{
				return null;
			}
			if (securityAttribute.properties == null || securityAttribute.properties.Count != 1)
			{
				return null;
			}
			CustomAttributeNamedArgument customAttributeNamedArgument = securityAttribute.properties[0];
			if (customAttributeNamedArgument.Name != "XML")
			{
				return null;
			}
			return (string)customAttributeNamedArgument.Argument.Value;
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x00025291 File Offset: 0x00023491
		private void WriteTypeReference(TypeReference type)
		{
			this.WriteUTF8String(TypeParser.ToParseable(type, false));
		}

		// Token: 0x060009F1 RID: 2545 RVA: 0x000252A0 File Offset: 0x000234A0
		public void WriteMarshalInfo(MarshalInfo marshal_info)
		{
			this.WriteNativeType(marshal_info.native);
			NativeType native = marshal_info.native;
			if (native <= NativeType.SafeArray)
			{
				if (native == NativeType.FixedSysString)
				{
					FixedSysStringMarshalInfo fixedSysStringMarshalInfo = (FixedSysStringMarshalInfo)marshal_info;
					if (fixedSysStringMarshalInfo.size > -1)
					{
						base.WriteCompressedUInt32((uint)fixedSysStringMarshalInfo.size);
					}
					return;
				}
				if (native != NativeType.SafeArray)
				{
					return;
				}
				SafeArrayMarshalInfo safeArrayMarshalInfo = (SafeArrayMarshalInfo)marshal_info;
				if (safeArrayMarshalInfo.element_type != VariantType.None)
				{
					this.WriteVariantType(safeArrayMarshalInfo.element_type);
				}
				return;
			}
			else
			{
				if (native == NativeType.FixedArray)
				{
					FixedArrayMarshalInfo fixedArrayMarshalInfo = (FixedArrayMarshalInfo)marshal_info;
					if (fixedArrayMarshalInfo.size > -1)
					{
						base.WriteCompressedUInt32((uint)fixedArrayMarshalInfo.size);
					}
					if (fixedArrayMarshalInfo.element_type != NativeType.None)
					{
						this.WriteNativeType(fixedArrayMarshalInfo.element_type);
					}
					return;
				}
				if (native == NativeType.Array)
				{
					ArrayMarshalInfo arrayMarshalInfo = (ArrayMarshalInfo)marshal_info;
					if (arrayMarshalInfo.element_type != NativeType.None)
					{
						this.WriteNativeType(arrayMarshalInfo.element_type);
					}
					if (arrayMarshalInfo.size_parameter_index > -1)
					{
						base.WriteCompressedUInt32((uint)arrayMarshalInfo.size_parameter_index);
					}
					if (arrayMarshalInfo.size > -1)
					{
						base.WriteCompressedUInt32((uint)arrayMarshalInfo.size);
					}
					if (arrayMarshalInfo.size_parameter_multiplier > -1)
					{
						base.WriteCompressedUInt32((uint)arrayMarshalInfo.size_parameter_multiplier);
					}
					return;
				}
				if (native != NativeType.CustomMarshaler)
				{
					return;
				}
				CustomMarshalInfo customMarshalInfo = (CustomMarshalInfo)marshal_info;
				this.WriteUTF8String((customMarshalInfo.guid != Guid.Empty) ? customMarshalInfo.guid.ToString() : string.Empty);
				this.WriteUTF8String(customMarshalInfo.unmanaged_type);
				this.WriteTypeReference(customMarshalInfo.managed_type);
				this.WriteUTF8String(customMarshalInfo.cookie);
				return;
			}
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x00025415 File Offset: 0x00023615
		private void WriteNativeType(NativeType native)
		{
			base.WriteByte((byte)native);
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x00025415 File Offset: 0x00023615
		private void WriteVariantType(VariantType variant)
		{
			base.WriteByte((byte)variant);
		}

		// Token: 0x060009F4 RID: 2548 RVA: 0x00025420 File Offset: 0x00023620
		public void WriteSequencePoints(MethodDebugInformation info)
		{
			int num = -1;
			int num2 = -1;
			base.WriteCompressedUInt32(info.local_var_token.RID);
			Document document;
			if (!info.TryGetUniqueDocument(out document))
			{
				document = null;
			}
			for (int i = 0; i < info.SequencePoints.Count; i++)
			{
				SequencePoint sequencePoint = info.SequencePoints[i];
				Document document2 = sequencePoint.Document;
				if (document != document2)
				{
					MetadataToken documentToken = this.metadata.GetDocumentToken(document2);
					if (document != null)
					{
						base.WriteCompressedUInt32(0U);
					}
					base.WriteCompressedUInt32(documentToken.RID);
					document = document2;
				}
				if (i > 0)
				{
					base.WriteCompressedUInt32((uint)(sequencePoint.Offset - info.SequencePoints[i - 1].Offset));
				}
				else
				{
					base.WriteCompressedUInt32((uint)sequencePoint.Offset);
				}
				if (sequencePoint.IsHidden)
				{
					base.WriteInt16(0);
				}
				else
				{
					int num3 = sequencePoint.EndLine - sequencePoint.StartLine;
					int num4 = sequencePoint.EndColumn - sequencePoint.StartColumn;
					base.WriteCompressedUInt32((uint)num3);
					if (num3 == 0)
					{
						base.WriteCompressedUInt32((uint)num4);
					}
					else
					{
						base.WriteCompressedInt32(num4);
					}
					if (num < 0)
					{
						base.WriteCompressedUInt32((uint)sequencePoint.StartLine);
						base.WriteCompressedUInt32((uint)sequencePoint.StartColumn);
					}
					else
					{
						base.WriteCompressedInt32(sequencePoint.StartLine - num);
						base.WriteCompressedInt32(sequencePoint.StartColumn - num2);
					}
					num = sequencePoint.StartLine;
					num2 = sequencePoint.StartColumn;
				}
			}
		}

		// Token: 0x040002CF RID: 719
		private readonly MetadataBuilder metadata;
	}
}
