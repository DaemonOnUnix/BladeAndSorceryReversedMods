using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Mono.Cecil;
using Mono.Collections.Generic;

namespace MonoMod.Utils
{
	// Token: 0x02000352 RID: 850
	public static class ReflectionHelper
	{
		// Token: 0x060013D8 RID: 5080 RVA: 0x00048A90 File Offset: 0x00046C90
		private static MemberInfo _Cache(string cacheKey, MemberInfo value)
		{
			if (cacheKey != null && value == null)
			{
				MMDbgLog.Log("ResolveRefl failure: " + cacheKey);
			}
			if (cacheKey != null && value != null)
			{
				Dictionary<string, MemberInfo> resolveReflectionCache = ReflectionHelper.ResolveReflectionCache;
				lock (resolveReflectionCache)
				{
					ReflectionHelper.ResolveReflectionCache[cacheKey] = value;
				}
			}
			return value;
		}

		// Token: 0x060013D9 RID: 5081 RVA: 0x00048B00 File Offset: 0x00046D00
		public static Assembly Load(ModuleDefinition module)
		{
			Assembly assembly;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				module.Write(memoryStream);
				memoryStream.Seek(0L, SeekOrigin.Begin);
				assembly = ReflectionHelper.Load(memoryStream);
			}
			return assembly;
		}

		// Token: 0x060013DA RID: 5082 RVA: 0x00048B48 File Offset: 0x00046D48
		public static Assembly Load(Stream stream)
		{
			MemoryStream memoryStream = stream as MemoryStream;
			Assembly asm;
			if (memoryStream != null)
			{
				asm = Assembly.Load(memoryStream.GetBuffer());
			}
			else
			{
				using (MemoryStream memoryStream2 = new MemoryStream())
				{
					byte[] array = new byte[4096];
					int num;
					while (0 < (num = stream.Read(array, 0, array.Length)))
					{
						memoryStream2.Write(array, 0, num);
					}
					memoryStream2.Seek(0L, SeekOrigin.Begin);
					asm = Assembly.Load(memoryStream2.GetBuffer());
				}
			}
			AppDomain.CurrentDomain.AssemblyResolve += delegate(object s, ResolveEventArgs e)
			{
				if (!(e.Name == asm.FullName))
				{
					return null;
				}
				return asm;
			};
			return asm;
		}

		// Token: 0x060013DB RID: 5083 RVA: 0x00048BFC File Offset: 0x00046DFC
		public static Type GetType(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}
			Type type = Type.GetType(name);
			if (type != null)
			{
				return type;
			}
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			for (int i = 0; i < assemblies.Length; i++)
			{
				type = assemblies[i].GetType(name);
				if (type != null)
				{
					return type;
				}
			}
			return null;
		}

		// Token: 0x060013DC RID: 5084 RVA: 0x00048C54 File Offset: 0x00046E54
		public static Type ResolveReflection(this TypeReference mref)
		{
			return ReflectionHelper._ResolveReflection(mref, null) as Type;
		}

		// Token: 0x060013DD RID: 5085 RVA: 0x00048C62 File Offset: 0x00046E62
		public static MethodBase ResolveReflection(this MethodReference mref)
		{
			return ReflectionHelper._ResolveReflection(mref, null) as MethodBase;
		}

		// Token: 0x060013DE RID: 5086 RVA: 0x00048C70 File Offset: 0x00046E70
		public static FieldInfo ResolveReflection(this FieldReference mref)
		{
			return ReflectionHelper._ResolveReflection(mref, null) as FieldInfo;
		}

		// Token: 0x060013DF RID: 5087 RVA: 0x00048C7E File Offset: 0x00046E7E
		public static PropertyInfo ResolveReflection(this PropertyReference mref)
		{
			return ReflectionHelper._ResolveReflection(mref, null) as PropertyInfo;
		}

		// Token: 0x060013E0 RID: 5088 RVA: 0x00048C8C File Offset: 0x00046E8C
		public static EventInfo ResolveReflection(this EventReference mref)
		{
			return ReflectionHelper._ResolveReflection(mref, null) as EventInfo;
		}

		// Token: 0x060013E1 RID: 5089 RVA: 0x00048C9A File Offset: 0x00046E9A
		public static MemberInfo ResolveReflection(this MemberReference mref)
		{
			return ReflectionHelper._ResolveReflection(mref, null);
		}

		// Token: 0x060013E2 RID: 5090 RVA: 0x00048CA4 File Offset: 0x00046EA4
		private static MemberInfo _ResolveReflection(MemberReference mref, Module[] modules)
		{
			if (mref == null)
			{
				return null;
			}
			DynamicMethodReference dynamicMethodReference = mref as DynamicMethodReference;
			if (dynamicMethodReference != null)
			{
				return dynamicMethodReference.DynamicMethod;
			}
			MethodReference methodReference = mref as MethodReference;
			string text = ((methodReference != null) ? methodReference.GetID(null, null, true, false) : null) ?? mref.FullName;
			TypeReference typeReference;
			if ((typeReference = mref.DeclaringType) == null)
			{
				typeReference = (mref as TypeReference) ?? null;
			}
			TypeReference typeReference2 = typeReference;
			IMetadataScope metadataScope = ((typeReference2 != null) ? typeReference2.Scope : null);
			AssemblyNameReference assemblyNameReference = metadataScope as AssemblyNameReference;
			string asmName;
			string moduleName;
			if (assemblyNameReference == null)
			{
				ModuleDefinition moduleDefinition = metadataScope as ModuleDefinition;
				if (moduleDefinition == null)
				{
					if (!(metadataScope is ModuleReference))
					{
						if (metadataScope != null)
						{
						}
						asmName = null;
						moduleName = null;
					}
					else
					{
						asmName = typeReference2.Module.Assembly.FullName;
						moduleName = typeReference2.Module.Name;
					}
				}
				else
				{
					asmName = moduleDefinition.Assembly.FullName;
					moduleName = moduleDefinition.Name;
				}
			}
			else
			{
				asmName = assemblyNameReference.FullName;
				moduleName = null;
			}
			text = string.Concat(new string[]
			{
				text,
				" | ",
				asmName ?? "NOASSEMBLY",
				", ",
				moduleName ?? "NOMODULE"
			});
			Dictionary<string, MemberInfo> resolveReflectionCache = ReflectionHelper.ResolveReflectionCache;
			lock (resolveReflectionCache)
			{
				MemberInfo memberInfo;
				if (ReflectionHelper.ResolveReflectionCache.TryGetValue(text, out memberInfo) && memberInfo != null)
				{
					return memberInfo;
				}
			}
			if (mref is GenericParameter)
			{
				throw new NotSupportedException("ResolveReflection on GenericParameter currently not supported");
			}
			MethodReference methodReference2 = mref as MethodReference;
			Type type;
			if (methodReference2 != null && mref.DeclaringType is ArrayType)
			{
				type = ReflectionHelper._ResolveReflection(mref.DeclaringType, modules) as Type;
				string methodID = methodReference2.GetID(null, null, false, false);
				MethodBase methodBase = type.GetMethods((BindingFlags)(-1)).Cast<MethodBase>().Concat(type.GetConstructors((BindingFlags)(-1)))
					.FirstOrDefault((MethodBase m) => m.GetID(null, null, false, false, false) == methodID);
				if (methodBase != null)
				{
					return ReflectionHelper._Cache(text, methodBase);
				}
			}
			if (typeReference2 == null)
			{
				throw new ArgumentException("MemberReference hasn't got a DeclaringType / isn't a TypeReference in itself");
			}
			if (asmName == null && moduleName == null)
			{
				throw new NotSupportedException("Unsupported scope type " + typeReference2.Scope.GetType().FullName);
			}
			bool flag2 = true;
			bool flag3 = false;
			bool flag4 = false;
			Func<Type, bool> <>9__15;
			Func<MethodInfo, bool> <>9__16;
			Func<FieldInfo, bool> <>9__17;
			TypeSpecification typeSpecification;
			MemberInfo memberInfo2;
			for (;;)
			{
				if (flag4)
				{
					modules = null;
				}
				flag4 = true;
				if (modules == null)
				{
					Assembly[] array = null;
					if (flag2 && flag3)
					{
						flag3 = false;
						flag2 = false;
					}
					if (flag2)
					{
						Dictionary<string, Assembly> assemblyCache = ReflectionHelper.AssemblyCache;
						lock (assemblyCache)
						{
							Assembly assembly;
							if (ReflectionHelper.AssemblyCache.TryGetValue(asmName, out assembly))
							{
								array = new Assembly[] { assembly };
							}
						}
					}
					if (array == null)
					{
						if (!flag3)
						{
							Dictionary<string, Assembly[]> dictionary = ReflectionHelper.AssembliesCache;
							lock (dictionary)
							{
								ReflectionHelper.AssembliesCache.TryGetValue(asmName, out array);
							}
						}
						if (array == null)
						{
							array = AppDomain.CurrentDomain.GetAssemblies().Where(delegate(Assembly other)
							{
								AssemblyName name = other.GetName();
								return name.Name == asmName || name.FullName == asmName;
							}).ToArray<Assembly>();
							if (array.Length == 0)
							{
								Assembly assembly2 = Assembly.Load(new AssemblyName(asmName));
								if (assembly2 != null)
								{
									array = new Assembly[] { assembly2 };
								}
							}
							if (array.Length != 0)
							{
								Dictionary<string, Assembly[]> dictionary = ReflectionHelper.AssembliesCache;
								lock (dictionary)
								{
									ReflectionHelper.AssembliesCache[asmName] = array;
								}
							}
						}
					}
					IEnumerable<Module> enumerable;
					if (!string.IsNullOrEmpty(moduleName))
					{
						enumerable = array.Select((Assembly asm) => asm.GetModule(moduleName));
					}
					else
					{
						enumerable = array.SelectMany((Assembly asm) => asm.GetModules());
					}
					modules = enumerable.Where((Module mod) => mod != null).ToArray<Module>();
					if (modules.Length == 0)
					{
						break;
					}
				}
				TypeReference typeReference3 = mref as TypeReference;
				if (typeReference3 != null)
				{
					if (typeReference3.FullName == "<Module>")
					{
						goto Block_40;
					}
					typeSpecification = mref as TypeSpecification;
					if (typeSpecification != null)
					{
						goto Block_41;
					}
					type = modules.Select((Module module) => module.GetType(mref.FullName.Replace("/", "+"), false, false)).FirstOrDefault((Type m) => m != null);
					if (type == null)
					{
						type = modules.Select(delegate(Module module)
						{
							IEnumerable<Type> types = module.GetTypes();
							Func<Type, bool> func;
							if ((func = <>9__15) == null)
							{
								func = (<>9__15 = (Type m) => mref.Is(m));
							}
							return types.FirstOrDefault(func);
						}).FirstOrDefault((Type m) => m != null);
					}
					if (!(type == null) || flag3)
					{
						goto IL_613;
					}
				}
				else
				{
					bool flag5 = mref.DeclaringType.FullName == "<Module>";
					GenericInstanceMethod genericInstanceMethod = mref as GenericInstanceMethod;
					if (genericInstanceMethod != null)
					{
						memberInfo2 = ReflectionHelper._ResolveReflection(genericInstanceMethod.ElementMethod, modules);
						MethodInfo methodInfo = memberInfo2 as MethodInfo;
						MemberInfo memberInfo3;
						if (methodInfo == null)
						{
							memberInfo3 = null;
						}
						else
						{
							memberInfo3 = methodInfo.MakeGenericMethod(genericInstanceMethod.GenericArguments.Select((TypeReference arg) => ReflectionHelper._ResolveReflection(arg, null) as Type).ToArray<Type>());
						}
						memberInfo2 = memberInfo3;
					}
					else if (flag5)
					{
						if (mref is MethodReference)
						{
							memberInfo2 = modules.Select(delegate(Module module)
							{
								IEnumerable<MethodInfo> methods = module.GetMethods((BindingFlags)(-1));
								Func<MethodInfo, bool> func;
								if ((func = <>9__16) == null)
								{
									func = (<>9__16 = (MethodInfo m) => mref.Is(m));
								}
								return methods.FirstOrDefault(func);
							}).FirstOrDefault((MethodInfo m) => m != null);
						}
						else
						{
							if (!(mref is FieldReference))
							{
								goto IL_73D;
							}
							memberInfo2 = modules.Select(delegate(Module module)
							{
								IEnumerable<FieldInfo> fields = module.GetFields((BindingFlags)(-1));
								Func<FieldInfo, bool> func;
								if ((func = <>9__17) == null)
								{
									func = (<>9__17 = (FieldInfo m) => mref.Is(m));
								}
								return fields.FirstOrDefault(func);
							}).FirstOrDefault((FieldInfo m) => m != null);
						}
					}
					else
					{
						Type type2 = ReflectionHelper._ResolveReflection(mref.DeclaringType, modules) as Type;
						if (mref is MethodReference)
						{
							memberInfo2 = type2.GetMethods((BindingFlags)(-1)).Cast<MethodBase>().Concat(type2.GetConstructors((BindingFlags)(-1)))
								.FirstOrDefault((MethodBase m) => mref.Is(m));
						}
						else if (mref is FieldReference)
						{
							memberInfo2 = type2.GetFields((BindingFlags)(-1)).FirstOrDefault((FieldInfo m) => mref.Is(m));
						}
						else
						{
							memberInfo2 = type2.GetMembers((BindingFlags)(-1)).FirstOrDefault((MemberInfo m) => mref.Is(m));
						}
					}
					if (!(memberInfo2 == null) || flag3)
					{
						goto IL_807;
					}
				}
				flag3 = true;
			}
			throw new Exception("Cannot resolve assembly / module " + asmName + " / " + moduleName);
			Block_40:
			throw new ArgumentException("Type <Module> cannot be resolved to a runtime reflection type");
			Block_41:
			type = ReflectionHelper._ResolveReflection(typeSpecification.ElementType, null) as Type;
			if (type == null)
			{
				return null;
			}
			if (typeSpecification.IsByReference)
			{
				return ReflectionHelper._Cache(text, type.MakeByRefType());
			}
			if (typeSpecification.IsPointer)
			{
				return ReflectionHelper._Cache(text, type.MakePointerType());
			}
			if (typeSpecification.IsArray)
			{
				return ReflectionHelper._Cache(text, (typeSpecification as ArrayType).IsVector ? type.MakeArrayType() : type.MakeArrayType((typeSpecification as ArrayType).Dimensions.Count));
			}
			if (typeSpecification.IsGenericInstance)
			{
				return ReflectionHelper._Cache(text, type.MakeGenericType((typeSpecification as GenericInstanceType).GenericArguments.Select((TypeReference arg) => ReflectionHelper._ResolveReflection(arg, null) as Type).ToArray<Type>()));
			}
			IL_613:
			return ReflectionHelper._Cache(text, type);
			IL_73D:
			throw new NotSupportedException("Unsupported <Module> member type " + mref.GetType().FullName);
			IL_807:
			return ReflectionHelper._Cache(text, memberInfo2);
		}

		// Token: 0x060013E3 RID: 5091 RVA: 0x000494F8 File Offset: 0x000476F8
		public static SignatureHelper ResolveReflection(this Mono.Cecil.CallSite csite, Module context)
		{
			return csite.ResolveReflectionSignature(context);
		}

		// Token: 0x060013E4 RID: 5092 RVA: 0x00049504 File Offset: 0x00047704
		public static SignatureHelper ResolveReflectionSignature(this IMethodSignature csite, Module context)
		{
			SignatureHelper signatureHelper;
			switch (csite.CallingConvention)
			{
			case MethodCallingConvention.C:
				signatureHelper = SignatureHelper.GetMethodSigHelper(context, CallingConvention.Cdecl, csite.ReturnType.ResolveReflection());
				break;
			case MethodCallingConvention.StdCall:
				signatureHelper = SignatureHelper.GetMethodSigHelper(context, CallingConvention.StdCall, csite.ReturnType.ResolveReflection());
				break;
			case MethodCallingConvention.ThisCall:
				signatureHelper = SignatureHelper.GetMethodSigHelper(context, CallingConvention.ThisCall, csite.ReturnType.ResolveReflection());
				break;
			case MethodCallingConvention.FastCall:
				signatureHelper = SignatureHelper.GetMethodSigHelper(context, CallingConvention.FastCall, csite.ReturnType.ResolveReflection());
				break;
			case MethodCallingConvention.VarArg:
				signatureHelper = SignatureHelper.GetMethodSigHelper(context, CallingConventions.VarArgs, csite.ReturnType.ResolveReflection());
				break;
			default:
				if (csite.ExplicitThis)
				{
					signatureHelper = SignatureHelper.GetMethodSigHelper(context, CallingConventions.ExplicitThis, csite.ReturnType.ResolveReflection());
				}
				else
				{
					signatureHelper = SignatureHelper.GetMethodSigHelper(context, CallingConventions.Standard, csite.ReturnType.ResolveReflection());
				}
				break;
			}
			if (context != null)
			{
				List<Type> list = new List<Type>();
				List<Type> list2 = new List<Type>();
				using (Collection<ParameterDefinition>.Enumerator enumerator = csite.Parameters.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ParameterDefinition parameterDefinition = enumerator.Current;
						if (parameterDefinition.ParameterType.IsSentinel)
						{
							signatureHelper.AddSentinel();
						}
						if (parameterDefinition.ParameterType.IsPinned)
						{
							signatureHelper.AddArgument(parameterDefinition.ParameterType.ResolveReflection(), true);
						}
						else
						{
							list2.Clear();
							list.Clear();
							TypeReference typeReference = parameterDefinition.ParameterType;
							for (;;)
							{
								TypeSpecification typeSpecification = typeReference as TypeSpecification;
								if (typeSpecification == null)
								{
									break;
								}
								RequiredModifierType requiredModifierType = typeReference as RequiredModifierType;
								if (requiredModifierType == null)
								{
									OptionalModifierType optionalModifierType = typeReference as OptionalModifierType;
									if (optionalModifierType != null)
									{
										list2.Add(optionalModifierType.ModifierType.ResolveReflection());
									}
								}
								else
								{
									list.Add(requiredModifierType.ModifierType.ResolveReflection());
								}
								typeReference = typeSpecification.ElementType;
							}
							signatureHelper.AddArgument(parameterDefinition.ParameterType.ResolveReflection(), list.ToArray(), list2.ToArray());
						}
					}
					return signatureHelper;
				}
			}
			foreach (ParameterDefinition parameterDefinition2 in csite.Parameters)
			{
				signatureHelper.AddArgument(parameterDefinition2.ParameterType.ResolveReflection());
			}
			return signatureHelper;
		}

		// Token: 0x060013E5 RID: 5093 RVA: 0x0004974C File Offset: 0x0004794C
		public static Mono.Cecil.CallSite ImportCallSite(this ModuleDefinition moduleTo, ICallSiteGenerator signature)
		{
			return signature.ToCallSite(moduleTo);
		}

		// Token: 0x060013E6 RID: 5094 RVA: 0x00049755 File Offset: 0x00047955
		public static Mono.Cecil.CallSite ImportCallSite(this ModuleDefinition moduleTo, SignatureHelper signature)
		{
			return moduleTo.ImportCallSite(ReflectionHelper.f_SignatureHelper_module.GetValue(signature) as Module, signature.GetSignature());
		}

		// Token: 0x060013E7 RID: 5095 RVA: 0x00049773 File Offset: 0x00047973
		public static Mono.Cecil.CallSite ImportCallSite(this ModuleDefinition moduleTo, Module moduleFrom, int token)
		{
			return moduleTo.ImportCallSite(moduleFrom, moduleFrom.ResolveSignature(token));
		}

		// Token: 0x060013E8 RID: 5096 RVA: 0x00049784 File Offset: 0x00047984
		public static Mono.Cecil.CallSite ImportCallSite(this ModuleDefinition moduleTo, Module moduleFrom, byte[] data)
		{
			ReflectionHelper.<>c__DisplayClass21_0 CS$<>8__locals1;
			CS$<>8__locals1.moduleTo = moduleTo;
			CS$<>8__locals1.moduleFrom = moduleFrom;
			Mono.Cecil.CallSite callSite = new Mono.Cecil.CallSite(CS$<>8__locals1.moduleTo.TypeSystem.Void);
			Mono.Cecil.CallSite callSite2;
			using (MemoryStream memoryStream = new MemoryStream(data, false))
			{
				ReflectionHelper.<>c__DisplayClass21_1 CS$<>8__locals2;
				CS$<>8__locals2.reader = new BinaryReader(memoryStream);
				try
				{
					ReflectionHelper.<ImportCallSite>g__ReadMethodSignature|21_0(callSite, ref CS$<>8__locals1, ref CS$<>8__locals2);
					callSite2 = callSite;
				}
				finally
				{
					if (CS$<>8__locals2.reader != null)
					{
						((IDisposable)CS$<>8__locals2.reader).Dispose();
					}
				}
			}
			return callSite2;
		}

		// Token: 0x060013EA RID: 5098 RVA: 0x00049878 File Offset: 0x00047A78
		[CompilerGenerated]
		internal static void <ImportCallSite>g__ReadMethodSignature|21_0(IMethodSignature method, ref ReflectionHelper.<>c__DisplayClass21_0 A_1, ref ReflectionHelper.<>c__DisplayClass21_1 A_2)
		{
			byte b = A_2.reader.ReadByte();
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
			if ((b & 16) != 0)
			{
				ReflectionHelper.<ImportCallSite>g__ReadCompressedUInt32|21_1(ref A_2);
			}
			uint num = ReflectionHelper.<ImportCallSite>g__ReadCompressedUInt32|21_1(ref A_2);
			method.MethodReturnType.ReturnType = ReflectionHelper.<ImportCallSite>g__ReadTypeSignature|21_4(ref A_1, ref A_2);
			int num2 = 0;
			while ((long)num2 < (long)((ulong)num))
			{
				method.Parameters.Add(new ParameterDefinition(ReflectionHelper.<ImportCallSite>g__ReadTypeSignature|21_4(ref A_1, ref A_2)));
				num2++;
			}
		}

		// Token: 0x060013EB RID: 5099 RVA: 0x0004990C File Offset: 0x00047B0C
		[CompilerGenerated]
		internal static uint <ImportCallSite>g__ReadCompressedUInt32|21_1(ref ReflectionHelper.<>c__DisplayClass21_1 A_0)
		{
			byte b = A_0.reader.ReadByte();
			if ((b & 128) == 0)
			{
				return (uint)b;
			}
			if ((b & 64) == 0)
			{
				return (((uint)b & 4294967167U) << 8) | (uint)A_0.reader.ReadByte();
			}
			return (uint)((((int)b & -193) << 24) | ((int)A_0.reader.ReadByte() << 16) | ((int)A_0.reader.ReadByte() << 8) | (int)A_0.reader.ReadByte());
		}

		// Token: 0x060013EC RID: 5100 RVA: 0x00049980 File Offset: 0x00047B80
		[CompilerGenerated]
		internal static int <ImportCallSite>g__ReadCompressedInt32|21_2(ref ReflectionHelper.<>c__DisplayClass21_1 A_0)
		{
			byte b = A_0.reader.ReadByte();
			A_0.reader.BaseStream.Seek(-1L, SeekOrigin.Current);
			uint num = ReflectionHelper.<ImportCallSite>g__ReadCompressedUInt32|21_1(ref A_0);
			int num2 = (int)num >> 1;
			if ((num & 1U) == 0U)
			{
				return num2;
			}
			int num3 = (int)(b & 192);
			if (num3 == 0 || num3 == 64)
			{
				return num2 - 64;
			}
			if (num3 != 128)
			{
				return num2 - 268435456;
			}
			return num2 - 8192;
		}

		// Token: 0x060013ED RID: 5101 RVA: 0x000499EC File Offset: 0x00047BEC
		[CompilerGenerated]
		internal static TypeReference <ImportCallSite>g__GetTypeDefOrRef|21_3(ref ReflectionHelper.<>c__DisplayClass21_0 A_0, ref ReflectionHelper.<>c__DisplayClass21_1 A_1)
		{
			uint num = ReflectionHelper.<ImportCallSite>g__ReadCompressedUInt32|21_1(ref A_1);
			uint num2 = num >> 2;
			uint num3;
			switch (num & 3U)
			{
			case 0U:
				num3 = 33554432U | num2;
				break;
			case 1U:
				num3 = 16777216U | num2;
				break;
			case 2U:
				num3 = 452984832U | num2;
				break;
			default:
				num3 = 0U;
				break;
			}
			return A_0.moduleTo.ImportReference(A_0.moduleFrom.ResolveType((int)num3));
		}

		// Token: 0x060013EE RID: 5102 RVA: 0x00049A54 File Offset: 0x00047C54
		[CompilerGenerated]
		internal static TypeReference <ImportCallSite>g__ReadTypeSignature|21_4(ref ReflectionHelper.<>c__DisplayClass21_0 A_0, ref ReflectionHelper.<>c__DisplayClass21_1 A_1)
		{
			MetadataType metadataType = (MetadataType)A_1.reader.ReadByte();
			switch (metadataType)
			{
			case MetadataType.Void:
				return A_0.moduleTo.TypeSystem.Void;
			case MetadataType.Boolean:
				return A_0.moduleTo.TypeSystem.Boolean;
			case MetadataType.Char:
				return A_0.moduleTo.TypeSystem.Char;
			case MetadataType.SByte:
				return A_0.moduleTo.TypeSystem.SByte;
			case MetadataType.Byte:
				return A_0.moduleTo.TypeSystem.Byte;
			case MetadataType.Int16:
				return A_0.moduleTo.TypeSystem.Int16;
			case MetadataType.UInt16:
				return A_0.moduleTo.TypeSystem.UInt16;
			case MetadataType.Int32:
				return A_0.moduleTo.TypeSystem.Int32;
			case MetadataType.UInt32:
				return A_0.moduleTo.TypeSystem.UInt32;
			case MetadataType.Int64:
				return A_0.moduleTo.TypeSystem.Int64;
			case MetadataType.UInt64:
				return A_0.moduleTo.TypeSystem.UInt64;
			case MetadataType.Single:
				return A_0.moduleTo.TypeSystem.Single;
			case MetadataType.Double:
				return A_0.moduleTo.TypeSystem.Double;
			case MetadataType.String:
				return A_0.moduleTo.TypeSystem.String;
			case MetadataType.Pointer:
				return new PointerType(ReflectionHelper.<ImportCallSite>g__ReadTypeSignature|21_4(ref A_0, ref A_1));
			case MetadataType.ByReference:
				return new ByReferenceType(ReflectionHelper.<ImportCallSite>g__ReadTypeSignature|21_4(ref A_0, ref A_1));
			case MetadataType.ValueType:
			case MetadataType.Class:
				return ReflectionHelper.<ImportCallSite>g__GetTypeDefOrRef|21_3(ref A_0, ref A_1);
			case MetadataType.Var:
			case MetadataType.GenericInstance:
			case MetadataType.MVar:
				throw new NotSupportedException(string.Format("Unsupported generic callsite element: {0}", metadataType));
			case MetadataType.Array:
			{
				ArrayType arrayType = new ArrayType(ReflectionHelper.<ImportCallSite>g__ReadTypeSignature|21_4(ref A_0, ref A_1));
				uint num = ReflectionHelper.<ImportCallSite>g__ReadCompressedUInt32|21_1(ref A_1);
				uint[] array = new uint[ReflectionHelper.<ImportCallSite>g__ReadCompressedUInt32|21_1(ref A_1)];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = ReflectionHelper.<ImportCallSite>g__ReadCompressedUInt32|21_1(ref A_1);
				}
				int[] array2 = new int[ReflectionHelper.<ImportCallSite>g__ReadCompressedUInt32|21_1(ref A_1)];
				for (int j = 0; j < array2.Length; j++)
				{
					array2[j] = ReflectionHelper.<ImportCallSite>g__ReadCompressedInt32|21_2(ref A_1);
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
			case MetadataType.TypedByReference:
				return A_0.moduleTo.TypeSystem.TypedReference;
			case (MetadataType)23:
			case (MetadataType)26:
				break;
			case MetadataType.IntPtr:
				return A_0.moduleTo.TypeSystem.IntPtr;
			case MetadataType.UIntPtr:
				return A_0.moduleTo.TypeSystem.UIntPtr;
			case MetadataType.FunctionPointer:
			{
				FunctionPointerType functionPointerType = new FunctionPointerType();
				ReflectionHelper.<ImportCallSite>g__ReadMethodSignature|21_0(functionPointerType, ref A_0, ref A_1);
				return functionPointerType;
			}
			case MetadataType.Object:
				return A_0.moduleTo.TypeSystem.Object;
			case (MetadataType)29:
				return new ArrayType(ReflectionHelper.<ImportCallSite>g__ReadTypeSignature|21_4(ref A_0, ref A_1));
			case MetadataType.RequiredModifier:
				return new RequiredModifierType(ReflectionHelper.<ImportCallSite>g__GetTypeDefOrRef|21_3(ref A_0, ref A_1), ReflectionHelper.<ImportCallSite>g__ReadTypeSignature|21_4(ref A_0, ref A_1));
			case MetadataType.OptionalModifier:
				return new OptionalModifierType(ReflectionHelper.<ImportCallSite>g__GetTypeDefOrRef|21_3(ref A_0, ref A_1), ReflectionHelper.<ImportCallSite>g__ReadTypeSignature|21_4(ref A_0, ref A_1));
			default:
				if (metadataType == MetadataType.Sentinel)
				{
					return new SentinelType(ReflectionHelper.<ImportCallSite>g__ReadTypeSignature|21_4(ref A_0, ref A_1));
				}
				if (metadataType == MetadataType.Pinned)
				{
					return new PinnedType(ReflectionHelper.<ImportCallSite>g__ReadTypeSignature|21_4(ref A_0, ref A_1));
				}
				break;
			}
			throw new NotSupportedException(string.Format("Unsupported callsite element: {0}", metadataType));
		}

		// Token: 0x04000FE3 RID: 4067
		internal static readonly Dictionary<string, Assembly> AssemblyCache = new Dictionary<string, Assembly>();

		// Token: 0x04000FE4 RID: 4068
		internal static readonly Dictionary<string, Assembly[]> AssembliesCache = new Dictionary<string, Assembly[]>();

		// Token: 0x04000FE5 RID: 4069
		internal static readonly Dictionary<string, MemberInfo> ResolveReflectionCache = new Dictionary<string, MemberInfo>();

		// Token: 0x04000FE6 RID: 4070
		private const BindingFlags _BindingFlagsAll = (BindingFlags)(-1);

		// Token: 0x04000FE7 RID: 4071
		private static readonly FieldInfo f_SignatureHelper_module = typeof(SignatureHelper).GetField("m_module", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) ?? typeof(SignatureHelper).GetField("module", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
	}
}
