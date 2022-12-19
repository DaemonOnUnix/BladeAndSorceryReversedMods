using System;
using System.Collections.Generic;
using System.Text;

namespace Mono.Cecil.Rocks
{
	// Token: 0x02000412 RID: 1042
	internal class DocCommentId
	{
		// Token: 0x060015EB RID: 5611 RVA: 0x00046305 File Offset: 0x00044505
		private DocCommentId()
		{
			this.id = new StringBuilder();
		}

		// Token: 0x060015EC RID: 5612 RVA: 0x00046318 File Offset: 0x00044518
		private void WriteField(FieldDefinition field)
		{
			this.WriteDefinition('F', field);
		}

		// Token: 0x060015ED RID: 5613 RVA: 0x00046323 File Offset: 0x00044523
		private void WriteEvent(EventDefinition @event)
		{
			this.WriteDefinition('E', @event);
		}

		// Token: 0x060015EE RID: 5614 RVA: 0x0004632E File Offset: 0x0004452E
		private void WriteType(TypeDefinition type)
		{
			this.id.Append('T').Append(':');
			this.WriteTypeFullName(type, false);
		}

		// Token: 0x060015EF RID: 5615 RVA: 0x00046350 File Offset: 0x00044550
		private void WriteMethod(MethodDefinition method)
		{
			this.WriteDefinition('M', method);
			if (method.HasGenericParameters)
			{
				this.id.Append('`').Append('`');
				this.id.Append(method.GenericParameters.Count);
			}
			if (method.HasParameters)
			{
				this.WriteParameters(method.Parameters);
			}
			if (DocCommentId.IsConversionOperator(method))
			{
				this.WriteReturnType(method);
			}
		}

		// Token: 0x060015F0 RID: 5616 RVA: 0x000463BD File Offset: 0x000445BD
		private static bool IsConversionOperator(MethodDefinition self)
		{
			if (self == null)
			{
				throw new ArgumentNullException("self");
			}
			return self.IsSpecialName && (self.Name == "op_Explicit" || self.Name == "op_Implicit");
		}

		// Token: 0x060015F1 RID: 5617 RVA: 0x000463FB File Offset: 0x000445FB
		private void WriteReturnType(MethodDefinition method)
		{
			this.id.Append('~');
			this.WriteTypeSignature(method.ReturnType);
		}

		// Token: 0x060015F2 RID: 5618 RVA: 0x00046417 File Offset: 0x00044617
		private void WriteProperty(PropertyDefinition property)
		{
			this.WriteDefinition('P', property);
			if (property.HasParameters)
			{
				this.WriteParameters(property.Parameters);
			}
		}

		// Token: 0x060015F3 RID: 5619 RVA: 0x00046436 File Offset: 0x00044636
		private void WriteParameters(IList<ParameterDefinition> parameters)
		{
			this.id.Append('(');
			this.WriteList<ParameterDefinition>(parameters, delegate(ParameterDefinition p)
			{
				this.WriteTypeSignature(p.ParameterType);
			});
			this.id.Append(')');
		}

		// Token: 0x060015F4 RID: 5620 RVA: 0x00046468 File Offset: 0x00044668
		private void WriteTypeSignature(TypeReference type)
		{
			MetadataType metadataType = type.MetadataType;
			switch (metadataType)
			{
			case MetadataType.Pointer:
				this.WriteTypeSignature(((PointerType)type).ElementType);
				this.id.Append('*');
				return;
			case MetadataType.ByReference:
				this.WriteTypeSignature(((ByReferenceType)type).ElementType);
				this.id.Append('@');
				return;
			case MetadataType.ValueType:
			case MetadataType.Class:
				break;
			case MetadataType.Var:
				this.id.Append('`');
				this.id.Append(((GenericParameter)type).Position);
				return;
			case MetadataType.Array:
				this.WriteArrayTypeSignature((ArrayType)type);
				return;
			case MetadataType.GenericInstance:
				this.WriteGenericInstanceTypeSignature((GenericInstanceType)type);
				return;
			default:
				switch (metadataType)
				{
				case MetadataType.FunctionPointer:
					this.WriteFunctionPointerTypeSignature((FunctionPointerType)type);
					return;
				case MetadataType.MVar:
					this.id.Append('`').Append('`');
					this.id.Append(((GenericParameter)type).Position);
					return;
				case MetadataType.RequiredModifier:
					this.WriteModiferTypeSignature((RequiredModifierType)type, '|');
					return;
				case MetadataType.OptionalModifier:
					this.WriteModiferTypeSignature((OptionalModifierType)type, '!');
					return;
				}
				break;
			}
			this.WriteTypeFullName(type, false);
		}

		// Token: 0x060015F5 RID: 5621 RVA: 0x000465A8 File Offset: 0x000447A8
		private void WriteGenericInstanceTypeSignature(GenericInstanceType type)
		{
			if (type.ElementType.IsTypeSpecification())
			{
				throw new NotSupportedException();
			}
			this.WriteTypeFullName(type.ElementType, true);
			this.id.Append('{');
			this.WriteList<TypeReference>(type.GenericArguments, new Action<TypeReference>(this.WriteTypeSignature));
			this.id.Append('}');
		}

		// Token: 0x060015F6 RID: 5622 RVA: 0x0004660C File Offset: 0x0004480C
		private void WriteList<T>(IList<T> list, Action<T> action)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (i > 0)
				{
					this.id.Append(',');
				}
				action(list[i]);
			}
		}

		// Token: 0x060015F7 RID: 5623 RVA: 0x00046649 File Offset: 0x00044849
		private void WriteModiferTypeSignature(IModifierType type, char id)
		{
			this.WriteTypeSignature(type.ElementType);
			this.id.Append(id);
			this.WriteTypeSignature(type.ModifierType);
		}

		// Token: 0x060015F8 RID: 5624 RVA: 0x00046670 File Offset: 0x00044870
		private void WriteFunctionPointerTypeSignature(FunctionPointerType type)
		{
			this.id.Append("=FUNC:");
			this.WriteTypeSignature(type.ReturnType);
			if (type.HasParameters)
			{
				this.WriteParameters(type.Parameters);
			}
		}

		// Token: 0x060015F9 RID: 5625 RVA: 0x000466A4 File Offset: 0x000448A4
		private void WriteArrayTypeSignature(ArrayType type)
		{
			this.WriteTypeSignature(type.ElementType);
			if (type.IsVector)
			{
				this.id.Append("[]");
				return;
			}
			this.id.Append("[");
			this.WriteList<ArrayDimension>(type.Dimensions, delegate(ArrayDimension dimension)
			{
				if (dimension.LowerBound != null)
				{
					this.id.Append(dimension.LowerBound.Value);
				}
				this.id.Append(':');
				if (dimension.UpperBound != null)
				{
					this.id.Append(dimension.UpperBound.Value - (dimension.LowerBound.GetValueOrDefault() + 1));
				}
			});
			this.id.Append("]");
		}

		// Token: 0x060015FA RID: 5626 RVA: 0x00046711 File Offset: 0x00044911
		private void WriteDefinition(char id, IMemberDefinition member)
		{
			this.id.Append(id).Append(':');
			this.WriteTypeFullName(member.DeclaringType, false);
			this.id.Append('.');
			this.WriteItemName(member.Name);
		}

		// Token: 0x060015FB RID: 5627 RVA: 0x00046750 File Offset: 0x00044950
		private void WriteTypeFullName(TypeReference type, bool stripGenericArity = false)
		{
			if (type.DeclaringType != null)
			{
				this.WriteTypeFullName(type.DeclaringType, false);
				this.id.Append('.');
			}
			if (!string.IsNullOrEmpty(type.Namespace))
			{
				this.id.Append(type.Namespace);
				this.id.Append('.');
			}
			string text = type.Name;
			if (stripGenericArity)
			{
				int num = text.LastIndexOf('`');
				if (num > 0)
				{
					text = text.Substring(0, num);
				}
			}
			this.id.Append(text);
		}

		// Token: 0x060015FC RID: 5628 RVA: 0x000467DA File Offset: 0x000449DA
		private void WriteItemName(string name)
		{
			this.id.Append(name.Replace('.', '#').Replace('<', '{').Replace('>', '}'));
		}

		// Token: 0x060015FD RID: 5629 RVA: 0x00046804 File Offset: 0x00044A04
		public override string ToString()
		{
			return this.id.ToString();
		}

		// Token: 0x060015FE RID: 5630 RVA: 0x00046814 File Offset: 0x00044A14
		public static string GetDocCommentId(IMemberDefinition member)
		{
			if (member == null)
			{
				throw new ArgumentNullException("member");
			}
			DocCommentId docCommentId = new DocCommentId();
			TokenType tokenType = member.MetadataToken.TokenType;
			if (tokenType <= TokenType.Field)
			{
				if (tokenType == TokenType.TypeDef)
				{
					docCommentId.WriteType((TypeDefinition)member);
					goto IL_A9;
				}
				if (tokenType == TokenType.Field)
				{
					docCommentId.WriteField((FieldDefinition)member);
					goto IL_A9;
				}
			}
			else
			{
				if (tokenType == TokenType.Method)
				{
					docCommentId.WriteMethod((MethodDefinition)member);
					goto IL_A9;
				}
				if (tokenType == TokenType.Event)
				{
					docCommentId.WriteEvent((EventDefinition)member);
					goto IL_A9;
				}
				if (tokenType == TokenType.Property)
				{
					docCommentId.WriteProperty((PropertyDefinition)member);
					goto IL_A9;
				}
			}
			throw new NotSupportedException(member.FullName);
			IL_A9:
			return docCommentId.ToString();
		}

		// Token: 0x04000F87 RID: 3975
		private StringBuilder id;
	}
}
