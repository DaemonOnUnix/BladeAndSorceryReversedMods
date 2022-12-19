using System;
using System.Text;
using Mono.Cecil.Cil;
using Mono.Cecil.Metadata;
using Mono.Cecil.PE;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x020000C9 RID: 201
	internal sealed class SignatureReader : ByteBuffer
	{
		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600058C RID: 1420 RVA: 0x0001980A File Offset: 0x00017A0A
		private TypeSystem TypeSystem
		{
			get
			{
				return this.reader.module.TypeSystem;
			}
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x0001981C File Offset: 0x00017A1C
		public SignatureReader(uint blob, MetadataReader reader)
			: base(reader.image.BlobHeap.data)
		{
			this.reader = reader;
			this.position = (int)blob;
			this.sig_length = base.ReadCompressedUInt32();
			this.start = (uint)this.position;
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x0001985A File Offset: 0x00017A5A
		private MetadataToken ReadTypeTokenSignature()
		{
			return CodedIndex.TypeDefOrRef.GetMetadataToken(base.ReadCompressedUInt32());
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x00019868 File Offset: 0x00017A68
		private GenericParameter GetGenericParameter(GenericParameterType type, uint var)
		{
			IGenericContext context = this.reader.context;
			if (context == null)
			{
				return this.GetUnboundGenericParameter(type, (int)var);
			}
			IGenericParameterProvider genericParameterProvider;
			if (type != GenericParameterType.Type)
			{
				if (type != GenericParameterType.Method)
				{
					throw new NotSupportedException();
				}
				genericParameterProvider = context.Method;
			}
			else
			{
				genericParameterProvider = context.Type;
			}
			if (!context.IsDefinition)
			{
				SignatureReader.CheckGenericContext(genericParameterProvider, (int)var);
			}
			if (var >= (uint)genericParameterProvider.GenericParameters.Count)
			{
				return this.GetUnboundGenericParameter(type, (int)var);
			}
			return genericParameterProvider.GenericParameters[(int)var];
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x000198E2 File Offset: 0x00017AE2
		private GenericParameter GetUnboundGenericParameter(GenericParameterType type, int index)
		{
			return new GenericParameter(index, type, this.reader.module);
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x000198F8 File Offset: 0x00017AF8
		private static void CheckGenericContext(IGenericParameterProvider owner, int index)
		{
			Collection<GenericParameter> genericParameters = owner.GenericParameters;
			for (int i = genericParameters.Count; i <= index; i++)
			{
				genericParameters.Add(new GenericParameter(owner));
			}
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x0001992C File Offset: 0x00017B2C
		public void ReadGenericInstanceSignature(IGenericParameterProvider provider, IGenericInstance instance, uint arity)
		{
			if (!provider.IsDefinition)
			{
				SignatureReader.CheckGenericContext(provider, (int)(arity - 1U));
			}
			Collection<TypeReference> genericArguments = instance.GenericArguments;
			int num = 0;
			while ((long)num < (long)((ulong)arity))
			{
				genericArguments.Add(this.ReadTypeSignature());
				num++;
			}
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x0001996C File Offset: 0x00017B6C
		private ArrayType ReadArrayTypeSignature()
		{
			ArrayType arrayType = new ArrayType(this.ReadTypeSignature());
			uint num = base.ReadCompressedUInt32();
			uint[] array = new uint[base.ReadCompressedUInt32()];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = base.ReadCompressedUInt32();
			}
			int[] array2 = new int[base.ReadCompressedUInt32()];
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j] = base.ReadCompressedInt32();
			}
			arrayType.Dimensions.Clear();
			int num2 = 0;
			while ((long)num2 < (long)((ulong)num))
			{
				int? num3 = null;
				int? num4 = null;
				if (num2 < array2.Length)
				{
					num3 = new int?(array2[num2]);
				}
				if (num2 < array.Length)
				{
					int? num5 = num3;
					int num6 = (int)array[num2];
					num4 = ((num5 != null) ? new int?(num5.GetValueOrDefault() + num6 - 1) : null);
				}
				arrayType.Dimensions.Add(new ArrayDimension(num3, num4));
				num2++;
			}
			return arrayType;
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x00019A69 File Offset: 0x00017C69
		private TypeReference GetTypeDefOrRef(MetadataToken token)
		{
			return this.reader.GetTypeDefOrRef(token);
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x00019A77 File Offset: 0x00017C77
		public TypeReference ReadTypeSignature()
		{
			return this.ReadTypeSignature((ElementType)base.ReadByte());
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x00019A85 File Offset: 0x00017C85
		public TypeReference ReadTypeToken()
		{
			return this.GetTypeDefOrRef(this.ReadTypeTokenSignature());
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x00019A94 File Offset: 0x00017C94
		private TypeReference ReadTypeSignature(ElementType etype)
		{
			switch (etype)
			{
			case ElementType.Void:
				return this.TypeSystem.Void;
			case ElementType.Boolean:
			case ElementType.Char:
			case ElementType.I1:
			case ElementType.U1:
			case ElementType.I2:
			case ElementType.U2:
			case ElementType.I4:
			case ElementType.U4:
			case ElementType.I8:
			case ElementType.U8:
			case ElementType.R4:
			case ElementType.R8:
			case ElementType.String:
			case (ElementType)23:
			case (ElementType)26:
				break;
			case ElementType.Ptr:
				return new PointerType(this.ReadTypeSignature());
			case ElementType.ByRef:
				return new ByReferenceType(this.ReadTypeSignature());
			case ElementType.ValueType:
			{
				TypeReference typeDefOrRef = this.GetTypeDefOrRef(this.ReadTypeTokenSignature());
				typeDefOrRef.KnownValueType();
				return typeDefOrRef;
			}
			case ElementType.Class:
				return this.GetTypeDefOrRef(this.ReadTypeTokenSignature());
			case ElementType.Var:
				return this.GetGenericParameter(GenericParameterType.Type, base.ReadCompressedUInt32());
			case ElementType.Array:
				return this.ReadArrayTypeSignature();
			case ElementType.GenericInst:
			{
				bool flag = base.ReadByte() == 17;
				TypeReference typeDefOrRef2 = this.GetTypeDefOrRef(this.ReadTypeTokenSignature());
				uint num = base.ReadCompressedUInt32();
				GenericInstanceType genericInstanceType = new GenericInstanceType(typeDefOrRef2, (int)num);
				this.ReadGenericInstanceSignature(typeDefOrRef2, genericInstanceType, num);
				if (flag)
				{
					genericInstanceType.KnownValueType();
					typeDefOrRef2.GetElementType().KnownValueType();
				}
				return genericInstanceType;
			}
			case ElementType.TypedByRef:
				return this.TypeSystem.TypedReference;
			case ElementType.I:
				return this.TypeSystem.IntPtr;
			case ElementType.U:
				return this.TypeSystem.UIntPtr;
			case ElementType.FnPtr:
			{
				FunctionPointerType functionPointerType = new FunctionPointerType();
				this.ReadMethodSignature(functionPointerType);
				return functionPointerType;
			}
			case ElementType.Object:
				return this.TypeSystem.Object;
			case ElementType.SzArray:
				return new ArrayType(this.ReadTypeSignature());
			case ElementType.MVar:
				return this.GetGenericParameter(GenericParameterType.Method, base.ReadCompressedUInt32());
			case ElementType.CModReqD:
				return new RequiredModifierType(this.GetTypeDefOrRef(this.ReadTypeTokenSignature()), this.ReadTypeSignature());
			case ElementType.CModOpt:
				return new OptionalModifierType(this.GetTypeDefOrRef(this.ReadTypeTokenSignature()), this.ReadTypeSignature());
			default:
				if (etype == ElementType.Sentinel)
				{
					return new SentinelType(this.ReadTypeSignature());
				}
				if (etype == ElementType.Pinned)
				{
					return new PinnedType(this.ReadTypeSignature());
				}
				break;
			}
			return this.GetPrimitiveType(etype);
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x00019C80 File Offset: 0x00017E80
		public void ReadMethodSignature(IMethodSignature method)
		{
			byte b = base.ReadByte();
			if ((b & 32) != 0)
			{
				method.HasThis = true;
				b = (byte)((int)b & -33);
			}
			if ((b & 64) != 0)
			{
				method.ExplicitThis = true;
				b = (byte)((int)b & -65);
			}
			method.CallingConvention = (MethodCallingConvention)b;
			MethodReference methodReference = method as MethodReference;
			if (methodReference != null && !methodReference.DeclaringType.IsArray)
			{
				this.reader.context = methodReference;
			}
			if ((b & 16) != 0)
			{
				uint num = base.ReadCompressedUInt32();
				if (methodReference != null && !methodReference.IsDefinition)
				{
					SignatureReader.CheckGenericContext(methodReference, (int)(num - 1U));
				}
			}
			uint num2 = base.ReadCompressedUInt32();
			method.MethodReturnType.ReturnType = this.ReadTypeSignature();
			if (num2 == 0U)
			{
				return;
			}
			MethodReference methodReference2 = method as MethodReference;
			Collection<ParameterDefinition> collection;
			if (methodReference2 != null)
			{
				collection = (methodReference2.parameters = new ParameterDefinitionCollection(method, (int)num2));
			}
			else
			{
				collection = method.Parameters;
			}
			int num3 = 0;
			while ((long)num3 < (long)((ulong)num2))
			{
				collection.Add(new ParameterDefinition(this.ReadTypeSignature()));
				num3++;
			}
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x00019D6F File Offset: 0x00017F6F
		public object ReadConstantSignature(ElementType type)
		{
			return this.ReadPrimitiveValue(type);
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x00019D78 File Offset: 0x00017F78
		public void ReadCustomAttributeConstructorArguments(CustomAttribute attribute, Collection<ParameterDefinition> parameters)
		{
			int count = parameters.Count;
			if (count == 0)
			{
				return;
			}
			attribute.arguments = new Collection<CustomAttributeArgument>(count);
			for (int i = 0; i < count; i++)
			{
				attribute.arguments.Add(this.ReadCustomAttributeFixedArgument(parameters[i].ParameterType));
			}
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x00019DC5 File Offset: 0x00017FC5
		private CustomAttributeArgument ReadCustomAttributeFixedArgument(TypeReference type)
		{
			if (type.IsArray)
			{
				return this.ReadCustomAttributeFixedArrayArgument((ArrayType)type);
			}
			return this.ReadCustomAttributeElement(type);
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x00019DE4 File Offset: 0x00017FE4
		public void ReadCustomAttributeNamedArguments(ushort count, ref Collection<CustomAttributeNamedArgument> fields, ref Collection<CustomAttributeNamedArgument> properties)
		{
			for (int i = 0; i < (int)count; i++)
			{
				if (!this.CanReadMore())
				{
					return;
				}
				this.ReadCustomAttributeNamedArgument(ref fields, ref properties);
			}
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x00019E10 File Offset: 0x00018010
		private void ReadCustomAttributeNamedArgument(ref Collection<CustomAttributeNamedArgument> fields, ref Collection<CustomAttributeNamedArgument> properties)
		{
			byte b = base.ReadByte();
			TypeReference typeReference = this.ReadCustomAttributeFieldOrPropType();
			string text = this.ReadUTF8String();
			Collection<CustomAttributeNamedArgument> collection;
			if (b != 83)
			{
				if (b != 84)
				{
					throw new NotSupportedException();
				}
				collection = SignatureReader.GetCustomAttributeNamedArgumentCollection(ref properties);
			}
			else
			{
				collection = SignatureReader.GetCustomAttributeNamedArgumentCollection(ref fields);
			}
			collection.Add(new CustomAttributeNamedArgument(text, this.ReadCustomAttributeFixedArgument(typeReference)));
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x00019E6C File Offset: 0x0001806C
		private static Collection<CustomAttributeNamedArgument> GetCustomAttributeNamedArgumentCollection(ref Collection<CustomAttributeNamedArgument> collection)
		{
			if (collection != null)
			{
				return collection;
			}
			Collection<CustomAttributeNamedArgument> collection2;
			collection = (collection2 = new Collection<CustomAttributeNamedArgument>());
			return collection2;
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x00019E8C File Offset: 0x0001808C
		private CustomAttributeArgument ReadCustomAttributeFixedArrayArgument(ArrayType type)
		{
			uint num = base.ReadUInt32();
			if (num == 4294967295U)
			{
				return new CustomAttributeArgument(type, null);
			}
			if (num == 0U)
			{
				return new CustomAttributeArgument(type, Empty<CustomAttributeArgument>.Array);
			}
			CustomAttributeArgument[] array = new CustomAttributeArgument[num];
			TypeReference elementType = type.ElementType;
			int num2 = 0;
			while ((long)num2 < (long)((ulong)num))
			{
				array[num2] = this.ReadCustomAttributeElement(elementType);
				num2++;
			}
			return new CustomAttributeArgument(type, array);
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x00019EEC File Offset: 0x000180EC
		private CustomAttributeArgument ReadCustomAttributeElement(TypeReference type)
		{
			if (type.IsArray)
			{
				return this.ReadCustomAttributeFixedArrayArgument((ArrayType)type);
			}
			return new CustomAttributeArgument(type, (type.etype == ElementType.Object) ? this.ReadCustomAttributeElement(this.ReadCustomAttributeFieldOrPropType()) : this.ReadCustomAttributeElementValue(type));
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x00019F38 File Offset: 0x00018138
		private object ReadCustomAttributeElementValue(TypeReference type)
		{
			ElementType etype = type.etype;
			if (etype != ElementType.None)
			{
				if (etype == ElementType.String)
				{
					return this.ReadUTF8String();
				}
				return this.ReadPrimitiveValue(etype);
			}
			else
			{
				if (type.IsTypeOf("System", "Type"))
				{
					return this.ReadTypeReference();
				}
				return this.ReadCustomAttributeEnum(type);
			}
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x00019F84 File Offset: 0x00018184
		private object ReadPrimitiveValue(ElementType type)
		{
			switch (type)
			{
			case ElementType.Boolean:
				return base.ReadByte() == 1;
			case ElementType.Char:
				return (char)base.ReadUInt16();
			case ElementType.I1:
				return (sbyte)base.ReadByte();
			case ElementType.U1:
				return base.ReadByte();
			case ElementType.I2:
				return base.ReadInt16();
			case ElementType.U2:
				return base.ReadUInt16();
			case ElementType.I4:
				return base.ReadInt32();
			case ElementType.U4:
				return base.ReadUInt32();
			case ElementType.I8:
				return base.ReadInt64();
			case ElementType.U8:
				return base.ReadUInt64();
			case ElementType.R4:
				return base.ReadSingle();
			case ElementType.R8:
				return base.ReadDouble();
			default:
				throw new NotImplementedException(type.ToString());
			}
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x0001A074 File Offset: 0x00018274
		private TypeReference GetPrimitiveType(ElementType etype)
		{
			switch (etype)
			{
			case ElementType.Boolean:
				return this.TypeSystem.Boolean;
			case ElementType.Char:
				return this.TypeSystem.Char;
			case ElementType.I1:
				return this.TypeSystem.SByte;
			case ElementType.U1:
				return this.TypeSystem.Byte;
			case ElementType.I2:
				return this.TypeSystem.Int16;
			case ElementType.U2:
				return this.TypeSystem.UInt16;
			case ElementType.I4:
				return this.TypeSystem.Int32;
			case ElementType.U4:
				return this.TypeSystem.UInt32;
			case ElementType.I8:
				return this.TypeSystem.Int64;
			case ElementType.U8:
				return this.TypeSystem.UInt64;
			case ElementType.R4:
				return this.TypeSystem.Single;
			case ElementType.R8:
				return this.TypeSystem.Double;
			case ElementType.String:
				return this.TypeSystem.String;
			default:
				throw new NotImplementedException(etype.ToString());
			}
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x0001A170 File Offset: 0x00018370
		private TypeReference ReadCustomAttributeFieldOrPropType()
		{
			ElementType elementType = (ElementType)base.ReadByte();
			if (elementType <= ElementType.Type)
			{
				if (elementType == ElementType.SzArray)
				{
					return new ArrayType(this.ReadCustomAttributeFieldOrPropType());
				}
				if (elementType == ElementType.Type)
				{
					return this.TypeSystem.LookupType("System", "Type");
				}
			}
			else
			{
				if (elementType == ElementType.Boxed)
				{
					return this.TypeSystem.Object;
				}
				if (elementType == ElementType.Enum)
				{
					return this.ReadTypeReference();
				}
			}
			return this.GetPrimitiveType(elementType);
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x0001A1DD File Offset: 0x000183DD
		public TypeReference ReadTypeReference()
		{
			return TypeParser.ParseType(this.reader.module, this.ReadUTF8String(), false);
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x0001A1F8 File Offset: 0x000183F8
		private object ReadCustomAttributeEnum(TypeReference enum_type)
		{
			TypeDefinition typeDefinition = enum_type.CheckedResolve();
			if (!typeDefinition.IsEnum)
			{
				throw new ArgumentException();
			}
			return this.ReadCustomAttributeElementValue(typeDefinition.GetEnumUnderlyingType());
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x0001A228 File Offset: 0x00018428
		public SecurityAttribute ReadSecurityAttribute()
		{
			SecurityAttribute securityAttribute = new SecurityAttribute(this.ReadTypeReference());
			base.ReadCompressedUInt32();
			this.ReadCustomAttributeNamedArguments((ushort)base.ReadCompressedUInt32(), ref securityAttribute.fields, ref securityAttribute.properties);
			return securityAttribute;
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x0001A264 File Offset: 0x00018464
		public MarshalInfo ReadMarshalInfo()
		{
			NativeType nativeType = this.ReadNativeType();
			if (nativeType <= NativeType.SafeArray)
			{
				if (nativeType == NativeType.FixedSysString)
				{
					FixedSysStringMarshalInfo fixedSysStringMarshalInfo = new FixedSysStringMarshalInfo();
					if (this.CanReadMore())
					{
						fixedSysStringMarshalInfo.size = (int)base.ReadCompressedUInt32();
					}
					return fixedSysStringMarshalInfo;
				}
				if (nativeType == NativeType.SafeArray)
				{
					SafeArrayMarshalInfo safeArrayMarshalInfo = new SafeArrayMarshalInfo();
					if (this.CanReadMore())
					{
						safeArrayMarshalInfo.element_type = this.ReadVariantType();
					}
					return safeArrayMarshalInfo;
				}
			}
			else
			{
				if (nativeType == NativeType.FixedArray)
				{
					FixedArrayMarshalInfo fixedArrayMarshalInfo = new FixedArrayMarshalInfo();
					if (this.CanReadMore())
					{
						fixedArrayMarshalInfo.size = (int)base.ReadCompressedUInt32();
					}
					if (this.CanReadMore())
					{
						fixedArrayMarshalInfo.element_type = this.ReadNativeType();
					}
					return fixedArrayMarshalInfo;
				}
				if (nativeType == NativeType.Array)
				{
					ArrayMarshalInfo arrayMarshalInfo = new ArrayMarshalInfo();
					if (this.CanReadMore())
					{
						arrayMarshalInfo.element_type = this.ReadNativeType();
					}
					if (this.CanReadMore())
					{
						arrayMarshalInfo.size_parameter_index = (int)base.ReadCompressedUInt32();
					}
					if (this.CanReadMore())
					{
						arrayMarshalInfo.size = (int)base.ReadCompressedUInt32();
					}
					if (this.CanReadMore())
					{
						arrayMarshalInfo.size_parameter_multiplier = (int)base.ReadCompressedUInt32();
					}
					return arrayMarshalInfo;
				}
				if (nativeType == NativeType.CustomMarshaler)
				{
					CustomMarshalInfo customMarshalInfo = new CustomMarshalInfo();
					string text = this.ReadUTF8String();
					customMarshalInfo.guid = ((!string.IsNullOrEmpty(text)) ? new Guid(text) : Guid.Empty);
					customMarshalInfo.unmanaged_type = this.ReadUTF8String();
					customMarshalInfo.managed_type = this.ReadTypeReference();
					customMarshalInfo.cookie = this.ReadUTF8String();
					return customMarshalInfo;
				}
			}
			return new MarshalInfo(nativeType);
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x0001A3C1 File Offset: 0x000185C1
		private NativeType ReadNativeType()
		{
			return (NativeType)base.ReadByte();
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x0001A3C1 File Offset: 0x000185C1
		private VariantType ReadVariantType()
		{
			return (VariantType)base.ReadByte();
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x0001A3CC File Offset: 0x000185CC
		private string ReadUTF8String()
		{
			if (this.buffer[this.position] == 255)
			{
				this.position++;
				return null;
			}
			int num = (int)base.ReadCompressedUInt32();
			if (num == 0)
			{
				return string.Empty;
			}
			if (this.position + num > this.buffer.Length)
			{
				return string.Empty;
			}
			string @string = Encoding.UTF8.GetString(this.buffer, this.position, num);
			this.position += num;
			return @string;
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x0001A44C File Offset: 0x0001864C
		public string ReadDocumentName()
		{
			char c = (char)this.buffer[this.position];
			this.position++;
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			while (this.CanReadMore())
			{
				if (num > 0 && c != '\0')
				{
					stringBuilder.Append(c);
				}
				uint num2 = base.ReadCompressedUInt32();
				if (num2 != 0U)
				{
					stringBuilder.Append(this.reader.ReadUTF8StringBlob(num2));
				}
				num++;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x0001A4C0 File Offset: 0x000186C0
		public Collection<SequencePoint> ReadSequencePoints(Document document)
		{
			base.ReadCompressedUInt32();
			if (document == null)
			{
				document = this.reader.GetDocument(base.ReadCompressedUInt32());
			}
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			bool flag = true;
			Collection<SequencePoint> collection = new Collection<SequencePoint>((int)((ulong)this.sig_length - (ulong)((long)this.position - (long)((ulong)this.start))) / 5);
			int num4 = 0;
			while (this.CanReadMore())
			{
				int num5 = (int)base.ReadCompressedUInt32();
				if (num4 > 0 && num5 == 0)
				{
					document = this.reader.GetDocument(base.ReadCompressedUInt32());
				}
				else
				{
					num += num5;
					int num6 = (int)base.ReadCompressedUInt32();
					int num7 = (int)((num6 == 0) ? base.ReadCompressedUInt32() : ((uint)base.ReadCompressedInt32()));
					if (num6 == 0 && num7 == 0)
					{
						collection.Add(new SequencePoint(num, document)
						{
							StartLine = 16707566,
							EndLine = 16707566,
							StartColumn = 0,
							EndColumn = 0
						});
					}
					else
					{
						if (flag)
						{
							num2 = (int)base.ReadCompressedUInt32();
							num3 = (int)base.ReadCompressedUInt32();
						}
						else
						{
							num2 += base.ReadCompressedInt32();
							num3 += base.ReadCompressedInt32();
						}
						collection.Add(new SequencePoint(num, document)
						{
							StartLine = num2,
							StartColumn = num3,
							EndLine = num2 + num6,
							EndColumn = num3 + num7
						});
						flag = false;
					}
				}
				num4++;
			}
			return collection;
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x0001A60B File Offset: 0x0001880B
		public bool CanReadMore()
		{
			return (long)this.position - (long)((ulong)this.start) < (long)((ulong)this.sig_length);
		}

		// Token: 0x0400025B RID: 603
		private readonly MetadataReader reader;

		// Token: 0x0400025C RID: 604
		internal readonly uint start;

		// Token: 0x0400025D RID: 605
		internal readonly uint sig_length;
	}
}
