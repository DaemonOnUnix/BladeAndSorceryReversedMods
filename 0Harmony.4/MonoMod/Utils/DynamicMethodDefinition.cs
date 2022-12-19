using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Security;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.Utils.Cil;

namespace MonoMod.Utils
{
	// Token: 0x02000435 RID: 1077
	public sealed class DynamicMethodDefinition : IDisposable
	{
		// Token: 0x060016A1 RID: 5793 RVA: 0x0004ACE4 File Offset: 0x00048EE4
		private static void _InitCopier()
		{
			DynamicMethodDefinition._CecilOpCodes1X = new Mono.Cecil.Cil.OpCode[225];
			DynamicMethodDefinition._CecilOpCodes2X = new Mono.Cecil.Cil.OpCode[31];
			FieldInfo[] fields = typeof(Mono.Cecil.Cil.OpCodes).GetFields(BindingFlags.Static | BindingFlags.Public);
			for (int i = 0; i < fields.Length; i++)
			{
				Mono.Cecil.Cil.OpCode opCode = (Mono.Cecil.Cil.OpCode)fields[i].GetValue(null);
				if (opCode.OpCodeType != Mono.Cecil.Cil.OpCodeType.Nternal)
				{
					if (opCode.Size == 1)
					{
						DynamicMethodDefinition._CecilOpCodes1X[(int)opCode.Value] = opCode;
					}
					else
					{
						DynamicMethodDefinition._CecilOpCodes2X[(int)(opCode.Value & 255)] = opCode;
					}
				}
			}
		}

		// Token: 0x060016A2 RID: 5794 RVA: 0x0004AD7C File Offset: 0x00048F7C
		private void _CopyMethodToDefinition()
		{
			DynamicMethodDefinition.<>c__DisplayClass3_0 CS$<>8__locals1 = new DynamicMethodDefinition.<>c__DisplayClass3_0();
			MethodBase originalMethod = this.OriginalMethod;
			CS$<>8__locals1.moduleFrom = originalMethod.Module;
			System.Reflection.MethodBody methodBody = originalMethod.GetMethodBody();
			byte[] array = ((methodBody != null) ? methodBody.GetILAsByteArray() : null);
			if (array == null)
			{
				throw new NotSupportedException("Body-less method");
			}
			CS$<>8__locals1.def = this.Definition;
			CS$<>8__locals1.moduleTo = CS$<>8__locals1.def.Module;
			CS$<>8__locals1.bodyTo = CS$<>8__locals1.def.Body;
			CS$<>8__locals1.bodyTo.GetILProcessor();
			CS$<>8__locals1.typeArguments = null;
			if (originalMethod.DeclaringType.IsGenericType)
			{
				CS$<>8__locals1.typeArguments = originalMethod.DeclaringType.GetGenericArguments();
			}
			CS$<>8__locals1.methodArguments = null;
			if (originalMethod.IsGenericMethod)
			{
				CS$<>8__locals1.methodArguments = originalMethod.GetGenericArguments();
			}
			foreach (LocalVariableInfo localVariableInfo in methodBody.LocalVariables)
			{
				TypeReference typeReference = CS$<>8__locals1.moduleTo.ImportReference(localVariableInfo.LocalType);
				if (localVariableInfo.IsPinned)
				{
					typeReference = new PinnedType(typeReference);
				}
				CS$<>8__locals1.bodyTo.Variables.Add(new VariableDefinition(typeReference));
			}
			using (BinaryReader binaryReader = new BinaryReader(new MemoryStream(array)))
			{
				Instruction instruction = null;
				while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
				{
					int num = (int)binaryReader.BaseStream.Position;
					Instruction instruction2 = Instruction.Create(Mono.Cecil.Cil.OpCodes.Nop);
					byte b = binaryReader.ReadByte();
					instruction2.OpCode = ((b != 254) ? DynamicMethodDefinition._CecilOpCodes1X[(int)b] : DynamicMethodDefinition._CecilOpCodes2X[(int)binaryReader.ReadByte()]);
					instruction2.Offset = num;
					if (instruction != null)
					{
						instruction.Next = instruction2;
					}
					instruction2.Previous = instruction;
					CS$<>8__locals1.<_CopyMethodToDefinition>g__ReadOperand|0(binaryReader, instruction2);
					CS$<>8__locals1.bodyTo.Instructions.Add(instruction2);
					instruction = instruction2;
				}
			}
			foreach (Instruction instruction3 in CS$<>8__locals1.bodyTo.Instructions)
			{
				Mono.Cecil.Cil.OperandType operandType = instruction3.OpCode.OperandType;
				if (operandType != Mono.Cecil.Cil.OperandType.InlineBrTarget)
				{
					if (operandType == Mono.Cecil.Cil.OperandType.InlineSwitch)
					{
						int[] array2 = (int[])instruction3.Operand;
						Instruction[] array3 = new Instruction[array2.Length];
						for (int i = 0; i < array2.Length; i++)
						{
							array3[i] = CS$<>8__locals1.<_CopyMethodToDefinition>g__GetInstruction|2(array2[i]);
						}
						instruction3.Operand = array3;
						continue;
					}
					if (operandType != Mono.Cecil.Cil.OperandType.ShortInlineBrTarget)
					{
						continue;
					}
				}
				instruction3.Operand = CS$<>8__locals1.<_CopyMethodToDefinition>g__GetInstruction|2((int)instruction3.Operand);
			}
			foreach (ExceptionHandlingClause exceptionHandlingClause in methodBody.ExceptionHandlingClauses)
			{
				Mono.Cecil.Cil.ExceptionHandler exceptionHandler = new Mono.Cecil.Cil.ExceptionHandler((ExceptionHandlerType)exceptionHandlingClause.Flags);
				CS$<>8__locals1.bodyTo.ExceptionHandlers.Add(exceptionHandler);
				exceptionHandler.TryStart = CS$<>8__locals1.<_CopyMethodToDefinition>g__GetInstruction|2(exceptionHandlingClause.TryOffset);
				exceptionHandler.TryEnd = CS$<>8__locals1.<_CopyMethodToDefinition>g__GetInstruction|2(exceptionHandlingClause.TryOffset + exceptionHandlingClause.TryLength);
				exceptionHandler.FilterStart = ((exceptionHandler.HandlerType != ExceptionHandlerType.Filter) ? null : CS$<>8__locals1.<_CopyMethodToDefinition>g__GetInstruction|2(exceptionHandlingClause.FilterOffset));
				exceptionHandler.HandlerStart = CS$<>8__locals1.<_CopyMethodToDefinition>g__GetInstruction|2(exceptionHandlingClause.HandlerOffset);
				exceptionHandler.HandlerEnd = CS$<>8__locals1.<_CopyMethodToDefinition>g__GetInstruction|2(exceptionHandlingClause.HandlerOffset + exceptionHandlingClause.HandlerLength);
				exceptionHandler.CatchType = ((exceptionHandler.HandlerType != ExceptionHandlerType.Catch) ? null : ((exceptionHandlingClause.CatchType == null) ? null : CS$<>8__locals1.moduleTo.ImportReference(exceptionHandlingClause.CatchType)));
			}
		}

		// Token: 0x060016A3 RID: 5795 RVA: 0x0004B180 File Offset: 0x00049380
		static DynamicMethodDefinition()
		{
			bool flag;
			if (!ReflectionHelper.IsMono || DynamicMethodDefinition._IsNewMonoSRE || DynamicMethodDefinition._IsOldMonoSRE)
			{
				if (!ReflectionHelper.IsMono)
				{
					Type type = typeof(ILGenerator).Assembly.GetType("System.Reflection.Emit.DynamicILGenerator");
					flag = ((type != null) ? type.GetField("m_scope", BindingFlags.Instance | BindingFlags.NonPublic) : null) == null;
				}
				else
				{
					flag = false;
				}
			}
			else
			{
				flag = true;
			}
			DynamicMethodDefinition._PreferCecil = flag;
			DynamicMethodDefinition.c_DebuggableAttribute = typeof(DebuggableAttribute).GetConstructor(new Type[] { typeof(DebuggableAttribute.DebuggingModes) });
			DynamicMethodDefinition.c_UnverifiableCodeAttribute = typeof(UnverifiableCodeAttribute).GetConstructor(new Type[0]);
			DynamicMethodDefinition.c_IgnoresAccessChecksToAttribute = typeof(IgnoresAccessChecksToAttribute).GetConstructor(new Type[] { typeof(string) });
			DynamicMethodDefinition.t__IDMDGenerator = typeof(_IDMDGenerator);
			DynamicMethodDefinition._DMDGeneratorCache = new Dictionary<string, _IDMDGenerator>();
			DynamicMethodDefinition._InitCopier();
		}

		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x060016A4 RID: 5796 RVA: 0x0004B2C9 File Offset: 0x000494C9
		public static bool IsDynamicILAvailable
		{
			get
			{
				return !DynamicMethodDefinition._PreferCecil;
			}
		}

		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x060016A5 RID: 5797 RVA: 0x0004B2D3 File Offset: 0x000494D3
		[Obsolete("Use OriginalMethod instead.")]
		public MethodBase Method
		{
			get
			{
				return this.OriginalMethod;
			}
		}

		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x060016A6 RID: 5798 RVA: 0x0004B2DB File Offset: 0x000494DB
		// (set) Token: 0x060016A7 RID: 5799 RVA: 0x0004B2E3 File Offset: 0x000494E3
		public MethodBase OriginalMethod { get; private set; }

		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x060016A8 RID: 5800 RVA: 0x0004B2EC File Offset: 0x000494EC
		public MethodDefinition Definition
		{
			get
			{
				return this._Definition;
			}
		}

		// Token: 0x1700056F RID: 1391
		// (get) Token: 0x060016A9 RID: 5801 RVA: 0x0004B2F4 File Offset: 0x000494F4
		public ModuleDefinition Module
		{
			get
			{
				return this._Module;
			}
		}

		// Token: 0x060016AA RID: 5802 RVA: 0x0004B2FC File Offset: 0x000494FC
		internal DynamicMethodDefinition()
		{
			this.Debug = Environment.GetEnvironmentVariable("MONOMOD_DMD_DEBUG") == "1";
		}

		// Token: 0x060016AB RID: 5803 RVA: 0x0004B329 File Offset: 0x00049529
		public DynamicMethodDefinition(MethodBase method)
			: this()
		{
			if (method == null)
			{
				throw new ArgumentNullException("method");
			}
			this.OriginalMethod = method;
			this.Reload();
		}

		// Token: 0x060016AC RID: 5804 RVA: 0x0004B34D File Offset: 0x0004954D
		public DynamicMethodDefinition(string name, Type returnType, Type[] parameterTypes)
			: this()
		{
			this.Name = name;
			this.OriginalMethod = null;
			this._CreateDynModule(name, returnType, parameterTypes);
		}

		// Token: 0x060016AD RID: 5805 RVA: 0x0004B36D File Offset: 0x0004956D
		public ILProcessor GetILProcessor()
		{
			return this.Definition.Body.GetILProcessor();
		}

		// Token: 0x060016AE RID: 5806 RVA: 0x0004B37F File Offset: 0x0004957F
		public ILGenerator GetILGenerator()
		{
			return new CecilILGenerator(this.Definition.Body.GetILProcessor()).GetProxy();
		}

		// Token: 0x060016AF RID: 5807 RVA: 0x0004B39C File Offset: 0x0004959C
		private ModuleDefinition _CreateDynModule(string name, Type returnType, Type[] parameterTypes)
		{
			ModuleDefinition moduleDefinition = (this._Module = ModuleDefinition.CreateModule(string.Format("DMD:DynModule<{0}>?{1}", name, this.GetHashCode()), new ModuleParameters
			{
				Kind = ModuleKind.Dll,
				ReflectionImporterProvider = MMReflectionImporter.ProviderNoDefault
			}));
			TypeDefinition typeDefinition = new TypeDefinition("", string.Format("DMD<{0}>?{1}", name, this.GetHashCode()), Mono.Cecil.TypeAttributes.Public);
			moduleDefinition.Types.Add(typeDefinition);
			MethodDefinition methodDefinition = (this._Definition = new MethodDefinition(name, Mono.Cecil.MethodAttributes.FamANDAssem | Mono.Cecil.MethodAttributes.Family | Mono.Cecil.MethodAttributes.Static | Mono.Cecil.MethodAttributes.HideBySig, (returnType != null) ? moduleDefinition.ImportReference(returnType) : moduleDefinition.TypeSystem.Void));
			foreach (Type type in parameterTypes)
			{
				methodDefinition.Parameters.Add(new ParameterDefinition(moduleDefinition.ImportReference(type)));
			}
			typeDefinition.Methods.Add(methodDefinition);
			return moduleDefinition;
		}

		// Token: 0x060016B0 RID: 5808 RVA: 0x0004B48C File Offset: 0x0004968C
		public void Reload()
		{
			MethodBase originalMethod = this.OriginalMethod;
			if (originalMethod == null)
			{
				throw new InvalidOperationException();
			}
			ModuleDefinition moduleDefinition = null;
			try
			{
				this._Definition = null;
				ModuleDefinition module = this._Module;
				if (module != null)
				{
					module.Dispose();
				}
				this._Module = null;
				ParameterInfo[] parameters = originalMethod.GetParameters();
				int num = 0;
				Type[] array;
				if (!originalMethod.IsStatic)
				{
					num++;
					array = new Type[parameters.Length + 1];
					array[0] = originalMethod.GetThisParamType();
				}
				else
				{
					array = new Type[parameters.Length];
				}
				for (int i = 0; i < parameters.Length; i++)
				{
					array[i + num] = parameters[i].ParameterType;
				}
				string id = originalMethod.GetID(null, null, true, false, true);
				MethodInfo methodInfo = originalMethod as MethodInfo;
				moduleDefinition = this._CreateDynModule(id, (methodInfo != null) ? methodInfo.ReturnType : null, array);
				this._CopyMethodToDefinition();
				MethodDefinition definition = this.Definition;
				if (!originalMethod.IsStatic)
				{
					definition.Parameters[0].Name = "this";
				}
				for (int j = 0; j < parameters.Length; j++)
				{
					definition.Parameters[j + num].Name = parameters[j].Name;
				}
				this._Module = moduleDefinition;
				moduleDefinition = null;
			}
			catch
			{
				if (moduleDefinition != null)
				{
					moduleDefinition.Dispose();
				}
				throw;
			}
		}

		// Token: 0x060016B1 RID: 5809 RVA: 0x0004B5E0 File Offset: 0x000497E0
		public MethodInfo Generate()
		{
			return this.Generate(null);
		}

		// Token: 0x060016B2 RID: 5810 RVA: 0x0004B5EC File Offset: 0x000497EC
		public MethodInfo Generate(object context)
		{
			string environmentVariable = Environment.GetEnvironmentVariable("MONOMOD_DMD_TYPE");
			string text = ((environmentVariable != null) ? environmentVariable.ToLower(CultureInfo.InvariantCulture) : null);
			if (text == "dynamicmethod" || text == "dm")
			{
				return DMDGenerator<DMDEmitDynamicMethodGenerator>.Generate(this, context);
			}
			if (text == "methodbuilder" || text == "mb")
			{
				return DMDGenerator<DMDEmitMethodBuilderGenerator>.Generate(this, context);
			}
			if (text == "cecil" || text == "md")
			{
				return DMDGenerator<DMDCecilGenerator>.Generate(this, context);
			}
			Type type = ReflectionHelper.GetType(environmentVariable);
			if (type != null)
			{
				if (!DynamicMethodDefinition.t__IDMDGenerator.IsCompatible(type))
				{
					throw new ArgumentException("Invalid DMDGenerator type: " + environmentVariable);
				}
				_IDMDGenerator idmdgenerator;
				if (!DynamicMethodDefinition._DMDGeneratorCache.TryGetValue(environmentVariable, out idmdgenerator))
				{
					idmdgenerator = (DynamicMethodDefinition._DMDGeneratorCache[environmentVariable] = Activator.CreateInstance(type) as _IDMDGenerator);
				}
				return idmdgenerator.Generate(this, context);
			}
			else
			{
				if (DynamicMethodDefinition._PreferCecil)
				{
					return DMDGenerator<DMDCecilGenerator>.Generate(this, context);
				}
				if (this.Debug)
				{
					return DMDGenerator<DMDEmitMethodBuilderGenerator>.Generate(this, context);
				}
				if (this.Definition.Body.ExceptionHandlers.Any((Mono.Cecil.Cil.ExceptionHandler eh) => eh.HandlerType == ExceptionHandlerType.Fault || eh.HandlerType == ExceptionHandlerType.Filter))
				{
					return DMDGenerator<DMDEmitMethodBuilderGenerator>.Generate(this, context);
				}
				return DMDGenerator<DMDEmitDynamicMethodGenerator>.Generate(this, context);
			}
		}

		// Token: 0x060016B3 RID: 5811 RVA: 0x0004B740 File Offset: 0x00049940
		public void Dispose()
		{
			if (this._IsDisposed)
			{
				return;
			}
			this._IsDisposed = true;
			this._Module.Dispose();
		}

		// Token: 0x060016B4 RID: 5812 RVA: 0x0004B75D File Offset: 0x0004995D
		public string GetDumpName(string type)
		{
			return string.Format("DMDASM.{0:X8}{1}", this.GUID.GetHashCode(), string.IsNullOrEmpty(type) ? "" : ("." + type));
		}

		// Token: 0x04000FC7 RID: 4039
		private static Mono.Cecil.Cil.OpCode[] _CecilOpCodes1X;

		// Token: 0x04000FC8 RID: 4040
		private static Mono.Cecil.Cil.OpCode[] _CecilOpCodes2X;

		// Token: 0x04000FC9 RID: 4041
		internal static readonly bool _IsNewMonoSRE = ReflectionHelper.IsMono && typeof(DynamicMethod).GetField("il_info", BindingFlags.Instance | BindingFlags.NonPublic) != null;

		// Token: 0x04000FCA RID: 4042
		internal static readonly bool _IsOldMonoSRE = ReflectionHelper.IsMono && !DynamicMethodDefinition._IsNewMonoSRE && typeof(DynamicMethod).GetField("ilgen", BindingFlags.Instance | BindingFlags.NonPublic) != null;

		// Token: 0x04000FCB RID: 4043
		private static bool _PreferCecil;

		// Token: 0x04000FCC RID: 4044
		internal static readonly ConstructorInfo c_DebuggableAttribute;

		// Token: 0x04000FCD RID: 4045
		internal static readonly ConstructorInfo c_UnverifiableCodeAttribute;

		// Token: 0x04000FCE RID: 4046
		internal static readonly ConstructorInfo c_IgnoresAccessChecksToAttribute;

		// Token: 0x04000FCF RID: 4047
		internal static readonly Type t__IDMDGenerator;

		// Token: 0x04000FD0 RID: 4048
		internal static readonly Dictionary<string, _IDMDGenerator> _DMDGeneratorCache;

		// Token: 0x04000FD2 RID: 4050
		private MethodDefinition _Definition;

		// Token: 0x04000FD3 RID: 4051
		private ModuleDefinition _Module;

		// Token: 0x04000FD4 RID: 4052
		public string Name;

		// Token: 0x04000FD5 RID: 4053
		public Type OwnerType;

		// Token: 0x04000FD6 RID: 4054
		public bool Debug;

		// Token: 0x04000FD7 RID: 4055
		private Guid GUID = Guid.NewGuid();

		// Token: 0x04000FD8 RID: 4056
		private bool _IsDisposed;

		// Token: 0x02000436 RID: 1078
		private enum TokenResolutionMode
		{
			// Token: 0x04000FDA RID: 4058
			Any,
			// Token: 0x04000FDB RID: 4059
			Type,
			// Token: 0x04000FDC RID: 4060
			Method,
			// Token: 0x04000FDD RID: 4061
			Field
		}
	}
}
