using System;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001C5 RID: 453
	public sealed class MethodBody
	{
		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000E85 RID: 3717 RVA: 0x00031F59 File Offset: 0x00030159
		public MethodDefinition Method
		{
			get
			{
				return this.method;
			}
		}

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06000E86 RID: 3718 RVA: 0x00031F61 File Offset: 0x00030161
		// (set) Token: 0x06000E87 RID: 3719 RVA: 0x00031F69 File Offset: 0x00030169
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

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06000E88 RID: 3720 RVA: 0x00031F72 File Offset: 0x00030172
		public int CodeSize
		{
			get
			{
				return this.code_size;
			}
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06000E89 RID: 3721 RVA: 0x00031F7A File Offset: 0x0003017A
		// (set) Token: 0x06000E8A RID: 3722 RVA: 0x00031F82 File Offset: 0x00030182
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

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06000E8B RID: 3723 RVA: 0x00031F8B File Offset: 0x0003018B
		// (set) Token: 0x06000E8C RID: 3724 RVA: 0x00031F93 File Offset: 0x00030193
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

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06000E8D RID: 3725 RVA: 0x00031F9C File Offset: 0x0003019C
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

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06000E8E RID: 3726 RVA: 0x00031FC4 File Offset: 0x000301C4
		public bool HasExceptionHandlers
		{
			get
			{
				return !this.exceptions.IsNullOrEmpty<ExceptionHandler>();
			}
		}

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06000E8F RID: 3727 RVA: 0x00031FD4 File Offset: 0x000301D4
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

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x06000E90 RID: 3728 RVA: 0x00031FF6 File Offset: 0x000301F6
		public bool HasVariables
		{
			get
			{
				return !this.variables.IsNullOrEmpty<VariableDefinition>();
			}
		}

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x06000E91 RID: 3729 RVA: 0x00032006 File Offset: 0x00030206
		public Collection<VariableDefinition> Variables
		{
			get
			{
				if (this.variables == null)
				{
					Interlocked.CompareExchange<Collection<VariableDefinition>>(ref this.variables, new VariableDefinitionCollection(), null);
				}
				return this.variables;
			}
		}

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06000E92 RID: 3730 RVA: 0x00032028 File Offset: 0x00030228
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

		// Token: 0x06000E93 RID: 3731 RVA: 0x00032088 File Offset: 0x00030288
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

		// Token: 0x06000E94 RID: 3732 RVA: 0x00032103 File Offset: 0x00030303
		public MethodBody(MethodDefinition method)
		{
			this.method = method;
		}

		// Token: 0x06000E95 RID: 3733 RVA: 0x00032112 File Offset: 0x00030312
		public ILProcessor GetILProcessor()
		{
			return new ILProcessor(this);
		}

		// Token: 0x040007B8 RID: 1976
		internal readonly MethodDefinition method;

		// Token: 0x040007B9 RID: 1977
		internal ParameterDefinition this_parameter;

		// Token: 0x040007BA RID: 1978
		internal int max_stack_size;

		// Token: 0x040007BB RID: 1979
		internal int code_size;

		// Token: 0x040007BC RID: 1980
		internal bool init_locals;

		// Token: 0x040007BD RID: 1981
		internal MetadataToken local_var_token;

		// Token: 0x040007BE RID: 1982
		internal Collection<Instruction> instructions;

		// Token: 0x040007BF RID: 1983
		internal Collection<ExceptionHandler> exceptions;

		// Token: 0x040007C0 RID: 1984
		internal Collection<VariableDefinition> variables;
	}
}
