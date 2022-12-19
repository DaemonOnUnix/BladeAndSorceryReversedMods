using System;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil.Cil
{
	// Token: 0x020002BA RID: 698
	public sealed class MethodBody
	{
		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x060011E9 RID: 4585 RVA: 0x00039991 File Offset: 0x00037B91
		public MethodDefinition Method
		{
			get
			{
				return this.method;
			}
		}

		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x060011EA RID: 4586 RVA: 0x00039999 File Offset: 0x00037B99
		// (set) Token: 0x060011EB RID: 4587 RVA: 0x000399A1 File Offset: 0x00037BA1
		public int MaxStackSize
		{
			get
			{
				return this.max_stack_size;
			}
			set
			{
				this.max_stack_size = value;
			}
		}

		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x060011EC RID: 4588 RVA: 0x000399AA File Offset: 0x00037BAA
		public int CodeSize
		{
			get
			{
				return this.code_size;
			}
		}

		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x060011ED RID: 4589 RVA: 0x000399B2 File Offset: 0x00037BB2
		// (set) Token: 0x060011EE RID: 4590 RVA: 0x000399BA File Offset: 0x00037BBA
		public bool InitLocals
		{
			get
			{
				return this.init_locals;
			}
			set
			{
				this.init_locals = value;
			}
		}

		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x060011EF RID: 4591 RVA: 0x000399C3 File Offset: 0x00037BC3
		// (set) Token: 0x060011F0 RID: 4592 RVA: 0x000399CB File Offset: 0x00037BCB
		public MetadataToken LocalVarToken
		{
			get
			{
				return this.local_var_token;
			}
			set
			{
				this.local_var_token = value;
			}
		}

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x060011F1 RID: 4593 RVA: 0x000399D4 File Offset: 0x00037BD4
		public Collection<Instruction> Instructions
		{
			get
			{
				if (this.instructions == null)
				{
					Interlocked.CompareExchange<Collection<Instruction>>(ref this.instructions, new InstructionCollection(this.method), null);
				}
				return this.instructions;
			}
		}

		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x060011F2 RID: 4594 RVA: 0x000399FC File Offset: 0x00037BFC
		public bool HasExceptionHandlers
		{
			get
			{
				return !this.exceptions.IsNullOrEmpty<ExceptionHandler>();
			}
		}

		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x060011F3 RID: 4595 RVA: 0x00039A0C File Offset: 0x00037C0C
		public Collection<ExceptionHandler> ExceptionHandlers
		{
			get
			{
				if (this.exceptions == null)
				{
					Interlocked.CompareExchange<Collection<ExceptionHandler>>(ref this.exceptions, new Collection<ExceptionHandler>(), null);
				}
				return this.exceptions;
			}
		}

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x060011F4 RID: 4596 RVA: 0x00039A2E File Offset: 0x00037C2E
		public bool HasVariables
		{
			get
			{
				return !this.variables.IsNullOrEmpty<VariableDefinition>();
			}
		}

		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x060011F5 RID: 4597 RVA: 0x00039A3E File Offset: 0x00037C3E
		public Collection<VariableDefinition> Variables
		{
			get
			{
				if (this.variables == null)
				{
					Interlocked.CompareExchange<Collection<VariableDefinition>>(ref this.variables, new VariableDefinitionCollection(this.method), null);
				}
				return this.variables;
			}
		}

		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x060011F6 RID: 4598 RVA: 0x00039A68 File Offset: 0x00037C68
		public ParameterDefinition ThisParameter
		{
			get
			{
				if (this.method == null || this.method.DeclaringType == null)
				{
					throw new NotSupportedException();
				}
				if (!this.method.HasThis)
				{
					return null;
				}
				if (this.this_parameter == null)
				{
					Interlocked.CompareExchange<ParameterDefinition>(ref this.this_parameter, MethodBody.CreateThisParameter(this.method), null);
				}
				return this.this_parameter;
			}
		}

		// Token: 0x060011F7 RID: 4599 RVA: 0x00039AC8 File Offset: 0x00037CC8
		private static ParameterDefinition CreateThisParameter(MethodDefinition method)
		{
			TypeReference typeReference = method.DeclaringType;
			if (typeReference.HasGenericParameters)
			{
				GenericInstanceType genericInstanceType = new GenericInstanceType(typeReference, typeReference.GenericParameters.Count);
				for (int i = 0; i < typeReference.GenericParameters.Count; i++)
				{
					genericInstanceType.GenericArguments.Add(typeReference.GenericParameters[i]);
				}
				typeReference = genericInstanceType;
			}
			if (typeReference.IsValueType || typeReference.IsPrimitive)
			{
				typeReference = new ByReferenceType(typeReference);
			}
			return new ParameterDefinition(typeReference, method);
		}

		// Token: 0x060011F8 RID: 4600 RVA: 0x00039B43 File Offset: 0x00037D43
		public MethodBody(MethodDefinition method)
		{
			this.method = method;
		}

		// Token: 0x060011F9 RID: 4601 RVA: 0x00039B52 File Offset: 0x00037D52
		public ILProcessor GetILProcessor()
		{
			return new ILProcessor(this);
		}

		// Token: 0x040007F0 RID: 2032
		internal readonly MethodDefinition method;

		// Token: 0x040007F1 RID: 2033
		internal ParameterDefinition this_parameter;

		// Token: 0x040007F2 RID: 2034
		internal int max_stack_size;

		// Token: 0x040007F3 RID: 2035
		internal int code_size;

		// Token: 0x040007F4 RID: 2036
		internal bool init_locals;

		// Token: 0x040007F5 RID: 2037
		internal MetadataToken local_var_token;

		// Token: 0x040007F6 RID: 2038
		internal Collection<Instruction> instructions;

		// Token: 0x040007F7 RID: 2039
		internal Collection<ExceptionHandler> exceptions;

		// Token: 0x040007F8 RID: 2040
		internal Collection<VariableDefinition> variables;
	}
}
