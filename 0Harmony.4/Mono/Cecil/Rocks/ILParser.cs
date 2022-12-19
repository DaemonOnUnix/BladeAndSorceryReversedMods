using System;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace Mono.Cecil.Rocks
{
	// Token: 0x02000417 RID: 1047
	public static class ILParser
	{
		// Token: 0x0600161F RID: 5663 RVA: 0x00046B90 File Offset: 0x00044D90
		public static void Parse(MethodDefinition method, IILVisitor visitor)
		{
			if (method == null)
			{
				throw new ArgumentNullException("method");
			}
			if (visitor == null)
			{
				throw new ArgumentNullException("visitor");
			}
			if (!method.HasBody || !method.HasImage)
			{
				throw new ArgumentException();
			}
			method.Module.Read<MethodDefinition, bool>(method, delegate(MethodDefinition m, MetadataReader _)
			{
				ILParser.ParseMethod(m, visitor);
				return true;
			});
		}

		// Token: 0x06001620 RID: 5664 RVA: 0x00046BFC File Offset: 0x00044DFC
		private static void ParseMethod(MethodDefinition method, IILVisitor visitor)
		{
			ILParser.ParseContext parseContext = ILParser.CreateContext(method, visitor);
			CodeReader code = parseContext.Code;
			byte b = code.ReadByte();
			int num = (int)(b & 3);
			if (num != 2)
			{
				if (num != 3)
				{
					throw new NotSupportedException();
				}
				code.Advance(-1);
				ILParser.ParseFatMethod(parseContext);
			}
			else
			{
				ILParser.ParseCode(b >> 2, parseContext);
			}
			code.MoveBackTo(parseContext.Position);
		}

		// Token: 0x06001621 RID: 5665 RVA: 0x00046C5C File Offset: 0x00044E5C
		private static ILParser.ParseContext CreateContext(MethodDefinition method, IILVisitor visitor)
		{
			CodeReader codeReader = method.Module.Read<MethodDefinition, CodeReader>(method, (MethodDefinition _, MetadataReader reader) => reader.code);
			int num = codeReader.MoveTo(method);
			return new ILParser.ParseContext
			{
				Code = codeReader,
				Position = num,
				Metadata = codeReader.reader,
				Visitor = visitor
			};
		}

		// Token: 0x06001622 RID: 5666 RVA: 0x00046CC4 File Offset: 0x00044EC4
		private static void ParseFatMethod(ILParser.ParseContext context)
		{
			CodeReader code = context.Code;
			code.Advance(4);
			int num = code.ReadInt32();
			MetadataToken metadataToken = code.ReadToken();
			if (metadataToken != MetadataToken.Zero)
			{
				context.Variables = code.ReadVariables(metadataToken);
			}
			ILParser.ParseCode(num, context);
		}

		// Token: 0x06001623 RID: 5667 RVA: 0x00046D0C File Offset: 0x00044F0C
		private static void ParseCode(int code_size, ILParser.ParseContext context)
		{
			CodeReader code = context.Code;
			MetadataReader metadata = context.Metadata;
			IILVisitor visitor = context.Visitor;
			int num = code.Position + code_size;
			while (code.Position < num)
			{
				byte b = code.ReadByte();
				OpCode opCode = ((b != 254) ? OpCodes.OneByteOpCode[(int)b] : OpCodes.TwoBytesOpCode[(int)code.ReadByte()]);
				switch (opCode.OperandType)
				{
				case OperandType.InlineBrTarget:
					visitor.OnInlineBranch(opCode, code.ReadInt32());
					break;
				case OperandType.InlineField:
				case OperandType.InlineMethod:
				case OperandType.InlineTok:
				case OperandType.InlineType:
				{
					IMetadataTokenProvider metadataTokenProvider = metadata.LookupToken(code.ReadToken());
					TokenType tokenType = metadataTokenProvider.MetadataToken.TokenType;
					if (tokenType > TokenType.Field)
					{
						if (tokenType <= TokenType.MemberRef)
						{
							if (tokenType != TokenType.Method)
							{
								if (tokenType != TokenType.MemberRef)
								{
									break;
								}
								FieldReference fieldReference = metadataTokenProvider as FieldReference;
								if (fieldReference != null)
								{
									visitor.OnInlineField(opCode, fieldReference);
									break;
								}
								MethodReference methodReference = metadataTokenProvider as MethodReference;
								if (methodReference != null)
								{
									visitor.OnInlineMethod(opCode, methodReference);
									break;
								}
								throw new InvalidOperationException();
							}
						}
						else
						{
							if (tokenType == TokenType.TypeSpec)
							{
								goto IL_2B8;
							}
							if (tokenType != TokenType.MethodSpec)
							{
								break;
							}
						}
						visitor.OnInlineMethod(opCode, (MethodReference)metadataTokenProvider);
						break;
					}
					if (tokenType != TokenType.TypeRef && tokenType != TokenType.TypeDef)
					{
						if (tokenType != TokenType.Field)
						{
							break;
						}
						visitor.OnInlineField(opCode, (FieldReference)metadataTokenProvider);
						break;
					}
					IL_2B8:
					visitor.OnInlineType(opCode, (TypeReference)metadataTokenProvider);
					break;
				}
				case OperandType.InlineI:
					visitor.OnInlineInt32(opCode, code.ReadInt32());
					break;
				case OperandType.InlineI8:
					visitor.OnInlineInt64(opCode, code.ReadInt64());
					break;
				case OperandType.InlineNone:
					visitor.OnInlineNone(opCode);
					break;
				case OperandType.InlineR:
					visitor.OnInlineDouble(opCode, code.ReadDouble());
					break;
				case OperandType.InlineSig:
					visitor.OnInlineSignature(opCode, code.GetCallSite(code.ReadToken()));
					break;
				case OperandType.InlineString:
					visitor.OnInlineString(opCode, code.GetString(code.ReadToken()));
					break;
				case OperandType.InlineSwitch:
				{
					int num2 = code.ReadInt32();
					int[] array = new int[num2];
					for (int i = 0; i < num2; i++)
					{
						array[i] = code.ReadInt32();
					}
					visitor.OnInlineSwitch(opCode, array);
					break;
				}
				case OperandType.InlineVar:
					visitor.OnInlineVariable(opCode, ILParser.GetVariable(context, (int)code.ReadInt16()));
					break;
				case OperandType.InlineArg:
					visitor.OnInlineArgument(opCode, code.GetParameter((int)code.ReadInt16()));
					break;
				case OperandType.ShortInlineBrTarget:
					visitor.OnInlineBranch(opCode, (int)code.ReadSByte());
					break;
				case OperandType.ShortInlineI:
					if (opCode == OpCodes.Ldc_I4_S)
					{
						visitor.OnInlineSByte(opCode, code.ReadSByte());
					}
					else
					{
						visitor.OnInlineByte(opCode, code.ReadByte());
					}
					break;
				case OperandType.ShortInlineR:
					visitor.OnInlineSingle(opCode, code.ReadSingle());
					break;
				case OperandType.ShortInlineVar:
					visitor.OnInlineVariable(opCode, ILParser.GetVariable(context, (int)code.ReadByte()));
					break;
				case OperandType.ShortInlineArg:
					visitor.OnInlineArgument(opCode, code.GetParameter((int)code.ReadByte()));
					break;
				}
			}
		}

		// Token: 0x06001624 RID: 5668 RVA: 0x00047048 File Offset: 0x00045248
		private static VariableDefinition GetVariable(ILParser.ParseContext context, int index)
		{
			return context.Variables[index];
		}

		// Token: 0x02000418 RID: 1048
		private class ParseContext
		{
			// Token: 0x17000565 RID: 1381
			// (get) Token: 0x06001625 RID: 5669 RVA: 0x00047056 File Offset: 0x00045256
			// (set) Token: 0x06001626 RID: 5670 RVA: 0x0004705E File Offset: 0x0004525E
			public CodeReader Code { get; set; }

			// Token: 0x17000566 RID: 1382
			// (get) Token: 0x06001627 RID: 5671 RVA: 0x00047067 File Offset: 0x00045267
			// (set) Token: 0x06001628 RID: 5672 RVA: 0x0004706F File Offset: 0x0004526F
			public int Position { get; set; }

			// Token: 0x17000567 RID: 1383
			// (get) Token: 0x06001629 RID: 5673 RVA: 0x00047078 File Offset: 0x00045278
			// (set) Token: 0x0600162A RID: 5674 RVA: 0x00047080 File Offset: 0x00045280
			public MetadataReader Metadata { get; set; }

			// Token: 0x17000568 RID: 1384
			// (get) Token: 0x0600162B RID: 5675 RVA: 0x00047089 File Offset: 0x00045289
			// (set) Token: 0x0600162C RID: 5676 RVA: 0x00047091 File Offset: 0x00045291
			public Collection<VariableDefinition> Variables { get; set; }

			// Token: 0x17000569 RID: 1385
			// (get) Token: 0x0600162D RID: 5677 RVA: 0x0004709A File Offset: 0x0004529A
			// (set) Token: 0x0600162E RID: 5678 RVA: 0x000470A2 File Offset: 0x000452A2
			public IILVisitor Visitor { get; set; }
		}
	}
}
