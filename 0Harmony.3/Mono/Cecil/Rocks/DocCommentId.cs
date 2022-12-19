using System;
using System.Collections.Generic;
using System.Text;

namespace Mono.Cecil.Rocks
{
	// Token: 0x0200031C RID: 796
	internal class DocCommentId
	{
		// Token: 0x0600127C RID: 4732 RVA: 0x0003E3BD File Offset: 0x0003C5BD
		private DocCommentId()
		{
			this.id = new StringBuilder();
		}

		// Token: 0x0600127D RID: 4733 RVA: 0x0003E3D0 File Offset: 0x0003C5D0
		private void WriteField(FieldDefinition field)
		{
			this.WriteDefinition('F', field);
		}

		// Token: 0x0600127E RID: 4734 RVA: 0x0003E3DB File Offset: 0x0003C5DB
		private void WriteEvent(EventDefinition @event)
		{
			this.WriteDefinition('E', @event);
		}

		// Token: 0x0600127F RID: 4735 RVA: 0x0003E3E6 File Offset: 0x0003C5E6
		private void WriteType(TypeDefinition type)
		{
			this.id.Append('T').Append(':');
			this.WriteTypeFullName(type, false);
		}

		// Token: 0x06001280 RID: 4736 RVA: 0x0003E408 File Offset: 0x0003C608
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

		// Token: 0x06001281 RID: 4737 RVA: 0x0003E475 File Offset: 0x0003C675
		private static bool IsConversionOperator(MethodDefinition self)
		{
			if (self == null)
			{
				throw new ArgumentNullException("self");
			}
			return self.IsSpecialName && (self.Name == "op_Explicit" || self.Name == "op_Implicit");
		}

		// Token: 0x06001282 RID: 4738 RVA: 0x0003E4B3 File Offset: 0x0003C6B3
		private void WriteReturnType(MethodDefinition method)
		{
			this.id.Append('~');
			this.WriteTypeSignature(method.ReturnType);
		}

		// Token: 0x06001283 RID: 4739 RVA: 0x0003E4CF File Offset: 0x0003C6CF
		private void WriteProperty(PropertyDefinition property)
		{
			this.WriteDefinition('P', property);
			if (property.HasParameters)
			{
				this.WriteParameters(property.Parameters);
			}
		}

		// Token: 0x06001284 RID: 4740 RVA: 0x0003E4EE File Offset: 0x0003C6EE
		private void WriteParameters(IList<ParameterDefinition> parameters)
		{
			this.id.Append('(');
			this.WriteList<ParameterDefinition>(parameters, delegate(ParameterDefinition p)
			{
				this.WriteTypeSignature(p.ParameterType);
			});
			this.id.Append(')');
		}

		// Token: 0x06001285 RID: 4741 RVA: 0x0003E520 File Offset: 0x0003C720
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

		// Token: 0x06001286 RID: 4742 RVA: 0x0003E660 File Offset: 0x0003C860
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

		// Token: 0x06001287 RID: 4743 RVA: 0x0003E6C4 File Offset: 0x0003C8C4
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

		// Token: 0x06001288 RID: 4744 RVA: 0x0003E701 File Offset: 0x0003C901
		private void WriteModiferTypeSignature(IModifierType type, char id)
		{
			this.WriteTypeSignature(type.ElementType);
			this.id.Append(id);
			this.WriteTypeSignature(type.ModifierType);
		}

		// Token: 0x06001289 RID: 4745 RVA: 0x0003E728 File Offset: 0x0003C928
		private void WriteFunctionPointerTypeSignature(FunctionPointerType type)
		{
			this.id.Append("=FUNC:");
			this.WriteTypeSignature(type.ReturnType);
			if (type.HasParameters)
			{
				this.WriteParameters(type.Parameters);
			}
		}

		// Token: 0x0600128A RID: 4746 RVA: 0x0003E75C File Offset: 0x0003C95C
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

		// Token: 0x0600128B RID: 4747 RVA: 0x0003E7C9 File Offset: 0x0003C9C9
		private void WriteDefinition(char id, IMemberDefinition member)
		{
			this.id.Append(id).Append(':');
			this.WriteTypeFullName(member.DeclaringType, false);
			this.id.Append('.');
			this.WriteItemName(member.Name);
		}

		// Token: 0x0600128C RID: 4748 RVA: 0x0003E808 File Offset: 0x0003CA08
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

		// Token: 0x0600128D RID: 4749 RVA: 0x0003E892 File Offset: 0x0003CA92
		private void WriteItemName(string name)
		{
			this.id.Append(name.Replace('.', '#').Replace('<', '{').Replace('>', '}'));
		}

		// Token: 0x0600128E RID: 4750 RVA: 0x0003E8BC File Offset: 0x0003CABC
		public override string ToString()
		{
			return this.id.ToString();
		}

		// Token: 0x0600128F RID: 4751 RVA: 0x0003E8CC File Offset: 0x0003CACC
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

		// Token: 0x04000F49 RID: 3913
		private StringBuilder id;
	}
}
