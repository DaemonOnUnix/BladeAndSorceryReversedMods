using System;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace Mono.Cecil.Rocks
{
	// Token: 0x02000321 RID: 801
	public static class ILParser
	{
		// Token: 0x060012B0 RID: 4784 RVA: 0x0003EC48 File Offset: 0x0003CE48
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

		// Token: 0x060012B1 RID: 4785 RVA: 0x0003ECB4 File Offset: 0x0003CEB4
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

		// Token: 0x060012B2 RID: 4786 RVA: 0x0003ED14 File Offset: 0x0003CF14
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

		// Token: 0x060012B3 RID: 4787 RVA: 0x0003ED7C File Offset: 0x0003CF7C
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

		// Token: 0x060012B4 RID: 4788 RVA: 0x0003EDC4 File Offset: 0x0003CFC4
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

		// Token: 0x060012B5 RID: 4789 RVA: 0x0003F100 File Offset: 0x0003D300
		private static VariableDefinition GetVariable(ILParser.ParseContext context, int index)
		{
			return context.Variables[index];
		}

		// Token: 0x02000322 RID: 802
		private class ParseContext
		{
			// Token: 0x17000391 RID: 913
			// (get) Token: 0x060012B6 RID: 4790 RVA: 0x0003F10E File Offset: 0x0003D30E
			// (set) Token: 0x060012B7 RID: 4791 RVA: 0x0003F116 File Offset: 0x0003D316
			public CodeReader Code { get; set; }

			// Token: 0x17000392 RID: 914
			// (get) Token: 0x060012B8 RID: 4792 RVA: 0x0003F11F File Offset: 0x0003D31F
			// (set) Token: 0x060012B9 RID: 4793 RVA: 0x0003F127 File Offset: 0x0003D327
			public int Position { get; set; }

			// Token: 0x17000393 RID: 915
			// (get) Token: 0x060012BA RID: 4794 RVA: 0x0003F130 File Offset: 0x0003D330
			// (set) Token: 0x060012BB RID: 4795 RVA: 0x0003F138 File Offset: 0x0003D338
			public MetadataReader Metadata { get; set; }

			// Token: 0x17000394 RID: 916
			// (get) Token: 0x060012BC RID: 4796 RVA: 0x0003F141 File Offset: 0x0003D341
			// (set) Token: 0x060012BD RID: 4797 RVA: 0x0003F149 File Offset: 0x0003D349
			public Collection<VariableDefinition> Variables { get; set; }

			// Token: 0x17000395 RID: 917
			// (get) Token: 0x060012BE RID: 4798 RVA: 0x0003F152 File Offset: 0x0003D352
			// (set) Token: 0x060012BF RID: 4799 RVA: 0x0003F15A File Offset: 0x0003D35A
			public IILVisitor Visitor { get; set; }
		}
	}
}
