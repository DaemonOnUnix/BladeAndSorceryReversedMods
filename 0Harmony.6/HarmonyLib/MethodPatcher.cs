using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Mono.Cecil;
using MonoMod.RuntimeDetour;
using MonoMod.Utils;

namespace HarmonyLib
{
	// Token: 0x02000024 RID: 36
	internal class MethodPatcher
	{
		// Token: 0x060000CF RID: 207 RVA: 0x00006618 File Offset: 0x00004818
		internal MethodPatcher(MethodBase original, MethodBase source, List<MethodInfo> prefixes, List<MethodInfo> postfixes, List<MethodInfo> transpilers, List<MethodInfo> finalizers, bool debug)
		{
			if (original == null)
			{
				throw new ArgumentNullException("original");
			}
			this.debug = debug;
			this.original = original;
			this.source = source;
			this.prefixes = prefixes;
			this.postfixes = postfixes;
			this.transpilers = transpilers;
			this.finalizers = finalizers;
			Memory.MarkForNoInlining(original);
			if (debug)
			{
				FileLog.LogBuffered("### Patch: " + original.FullDescription());
				FileLog.FlushBuffer();
			}
			this.idx = prefixes.Count<MethodInfo>() + postfixes.Count<MethodInfo>() + finalizers.Count<MethodInfo>();
			this.useStructReturnBuffer = StructReturnBuffer.NeedsFix(original);
			if (debug && this.useStructReturnBuffer)
			{
				FileLog.Log("### Note: A buffer for the returned struct is used. That requires an extra IntPtr argument before the first real argument");
			}
			this.returnType = AccessTools.GetReturnedType(original);
			this.patch = MethodPatcher.CreateDynamicMethod(original, string.Format("_Patch{0}", this.idx), debug);
			if (this.patch == null)
			{
				throw new Exception("Could not create replacement method");
			}
			this.il = this.patch.GetILGenerator();
			this.emitter = new Emitter(this.il, debug);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00006738 File Offset: 0x00004938
		internal MethodInfo CreateReplacement(out Dictionary<int, CodeInstruction> finalInstructions)
		{
			LocalBuilder[] array = MethodPatcher.DeclareLocalVariables(this.il, this.source ?? this.original);
			Dictionary<string, LocalBuilder> privateVars = new Dictionary<string, LocalBuilder>();
			LocalBuilder localBuilder = null;
			if (this.idx > 0)
			{
				localBuilder = this.DeclareLocalVariable(this.returnType, true);
				privateVars["__result"] = localBuilder;
			}
			Label? label = null;
			LocalBuilder localBuilder2 = null;
			if (this.prefixes.Any((MethodInfo fix) => MethodPatcher.PrefixAffectsOriginal(fix)))
			{
				localBuilder2 = this.DeclareLocalVariable(typeof(bool), false);
				this.emitter.Emit(OpCodes.Ldc_I4_1);
				this.emitter.Emit(OpCodes.Stloc, localBuilder2);
				label = new Label?(this.il.DefineLabel());
			}
			this.prefixes.Union(this.postfixes).Union(this.finalizers).ToList<MethodInfo>()
				.ForEach(delegate(MethodInfo fix)
				{
					if (fix.DeclaringType != null && !privateVars.ContainsKey(fix.DeclaringType.FullName))
					{
						(from patchParam in fix.GetParameters()
							where patchParam.Name == "__state"
							select patchParam).Do(delegate(ParameterInfo patchParam)
						{
							LocalBuilder localBuilder5 = this.DeclareLocalVariable(patchParam.ParameterType, false);
							privateVars[fix.DeclaringType.FullName] = localBuilder5;
						});
					}
				});
			LocalBuilder localBuilder3 = null;
			if (this.finalizers.Any<MethodInfo>())
			{
				localBuilder3 = this.DeclareLocalVariable(typeof(bool), false);
				privateVars["__exception"] = this.DeclareLocalVariable(typeof(Exception), false);
				Label? label2;
				this.emitter.MarkBlockBefore(new ExceptionBlock(ExceptionBlockType.BeginExceptionBlock, null), out label2);
			}
			this.AddPrefixes(privateVars, localBuilder2);
			if (label != null)
			{
				this.emitter.Emit(OpCodes.Ldloc, localBuilder2);
				this.emitter.Emit(OpCodes.Brfalse, label.Value);
			}
			MethodCopier methodCopier = new MethodCopier(this.source ?? this.original, this.il, array);
			methodCopier.SetArgumentShift(this.useStructReturnBuffer);
			methodCopier.SetDebugging(this.debug);
			foreach (MethodInfo methodInfo in this.transpilers)
			{
				methodCopier.AddTranspiler(methodInfo);
			}
			List<Label> list = new List<Label>();
			bool flag;
			methodCopier.Finalize(this.emitter, list, out flag);
			foreach (Label label3 in list)
			{
				this.emitter.MarkLabel(label3);
			}
			if (localBuilder != null)
			{
				this.emitter.Emit(OpCodes.Stloc, localBuilder);
			}
			if (label != null)
			{
				this.emitter.MarkLabel(label.Value);
			}
			this.AddPostfixes(privateVars, false);
			if (localBuilder != null)
			{
				this.emitter.Emit(OpCodes.Ldloc, localBuilder);
			}
			bool flag2 = this.AddPostfixes(privateVars, true);
			bool flag3 = this.finalizers.Any<MethodInfo>();
			if (flag3)
			{
				if (flag2)
				{
					this.emitter.Emit(OpCodes.Stloc, localBuilder);
					this.emitter.Emit(OpCodes.Ldloc, localBuilder);
				}
				this.AddFinalizers(privateVars, false);
				this.emitter.Emit(OpCodes.Ldc_I4_1);
				this.emitter.Emit(OpCodes.Stloc, localBuilder3);
				Label label4 = this.il.DefineLabel();
				this.emitter.Emit(OpCodes.Ldloc, privateVars["__exception"]);
				this.emitter.Emit(OpCodes.Brfalse, label4);
				this.emitter.Emit(OpCodes.Ldloc, privateVars["__exception"]);
				this.emitter.Emit(OpCodes.Throw);
				this.emitter.MarkLabel(label4);
				Label? label5;
				this.emitter.MarkBlockBefore(new ExceptionBlock(ExceptionBlockType.BeginCatchBlock, null), out label5);
				this.emitter.Emit(OpCodes.Stloc, privateVars["__exception"]);
				this.emitter.Emit(OpCodes.Ldloc, localBuilder3);
				Label label6 = this.il.DefineLabel();
				this.emitter.Emit(OpCodes.Brtrue, label6);
				bool flag4 = this.AddFinalizers(privateVars, true);
				this.emitter.MarkLabel(label6);
				Label label7 = this.il.DefineLabel();
				this.emitter.Emit(OpCodes.Ldloc, privateVars["__exception"]);
				this.emitter.Emit(OpCodes.Brfalse, label7);
				if (flag4)
				{
					this.emitter.Emit(OpCodes.Rethrow);
				}
				else
				{
					this.emitter.Emit(OpCodes.Ldloc, privateVars["__exception"]);
					this.emitter.Emit(OpCodes.Throw);
				}
				this.emitter.MarkLabel(label7);
				this.emitter.MarkBlockAfter(new ExceptionBlock(ExceptionBlockType.EndExceptionBlock, null));
				if (localBuilder != null)
				{
					this.emitter.Emit(OpCodes.Ldloc, localBuilder);
				}
			}
			if (this.useStructReturnBuffer)
			{
				LocalBuilder localBuilder4 = this.DeclareLocalVariable(this.returnType, false);
				this.emitter.Emit(OpCodes.Stloc, localBuilder4);
				this.emitter.Emit(this.original.IsStatic ? OpCodes.Ldarg_0 : OpCodes.Ldarg_1);
				this.emitter.Emit(OpCodes.Ldloc, localBuilder4);
				this.emitter.Emit(OpCodes.Stobj, this.returnType);
			}
			if (flag3 || flag)
			{
				this.emitter.Emit(OpCodes.Ret);
			}
			finalInstructions = this.emitter.GetInstructions();
			if (this.debug)
			{
				FileLog.LogBuffered("DONE");
				FileLog.LogBuffered("");
				FileLog.FlushBuffer();
			}
			return this.patch.Generate().Pin<MethodInfo>();
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00006D0C File Offset: 0x00004F0C
		internal static DynamicMethodDefinition CreateDynamicMethod(MethodBase original, string suffix, bool debug)
		{
			if (original == null)
			{
				throw new ArgumentNullException("original");
			}
			bool flag = StructReturnBuffer.NeedsFix(original);
			Type declaringType = original.DeclaringType;
			string text = ((declaringType != null) ? declaringType.FullName : null) + "." + original.Name + suffix;
			text = text.Replace("<>", "");
			ParameterInfo[] parameters = original.GetParameters();
			List<Type> list = new List<Type>();
			list.AddRange(parameters.Types());
			if (flag)
			{
				list.Insert(0, typeof(IntPtr));
			}
			if (!original.IsStatic)
			{
				if (AccessTools.IsStruct(original.DeclaringType))
				{
					list.Insert(0, original.DeclaringType.MakeByRefType());
				}
				else
				{
					list.Insert(0, original.DeclaringType);
				}
			}
			Type type = (flag ? typeof(void) : AccessTools.GetReturnedType(original));
			DynamicMethodDefinition dynamicMethodDefinition = new DynamicMethodDefinition(text, type, list.ToArray())
			{
				OwnerType = original.DeclaringType
			};
			int num = (original.IsStatic ? 0 : 1) + (flag ? 1 : 0);
			if (flag)
			{
				dynamicMethodDefinition.Definition.Parameters[original.IsStatic ? 0 : 1].Name = "retbuf";
			}
			if (!original.IsStatic)
			{
				dynamicMethodDefinition.Definition.Parameters[0].Name = "this";
			}
			for (int i = 0; i < parameters.Length; i++)
			{
				ParameterDefinition parameterDefinition = dynamicMethodDefinition.Definition.Parameters[i + num];
				parameterDefinition.Attributes = (Mono.Cecil.ParameterAttributes)parameters[i].Attributes;
				parameterDefinition.Name = parameters[i].Name;
			}
			if (debug)
			{
				List<string> list2 = list.Select((Type p) => p.FullDescription()).ToList<string>();
				if (list.Count == dynamicMethodDefinition.Definition.Parameters.Count)
				{
					for (int j = 0; j < list.Count; j++)
					{
						List<string> list3 = list2;
						int num2 = j;
						list3[num2] = list3[num2] + " " + dynamicMethodDefinition.Definition.Parameters[j].Name;
					}
				}
				FileLog.Log(string.Concat(new string[]
				{
					"### Replacement: static ",
					type.FullDescription(),
					" ",
					original.DeclaringType.FullName,
					"::",
					text,
					"(",
					list2.Join(null, ", "),
					")"
				}));
			}
			return dynamicMethodDefinition;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00006FA8 File Offset: 0x000051A8
		internal static LocalBuilder[] DeclareLocalVariables(ILGenerator il, MethodBase member)
		{
			MethodBody methodBody = member.GetMethodBody();
			IList<LocalVariableInfo> list = ((methodBody != null) ? methodBody.LocalVariables : null);
			if (list == null)
			{
				return new LocalBuilder[0];
			}
			return list.Select((LocalVariableInfo lvi) => il.DeclareLocal(lvi.LocalType, lvi.IsPinned)).ToArray<LocalBuilder>();
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00006FF8 File Offset: 0x000051F8
		private LocalBuilder DeclareLocalVariable(Type type, bool isReturnValue = false)
		{
			if (type.IsByRef && !isReturnValue)
			{
				type = type.GetElementType();
			}
			if (type.IsEnum)
			{
				type = Enum.GetUnderlyingType(type);
			}
			if (AccessTools.IsClass(type))
			{
				LocalBuilder localBuilder = this.il.DeclareLocal(type);
				this.emitter.Emit(OpCodes.Ldnull);
				this.emitter.Emit(OpCodes.Stloc, localBuilder);
				return localBuilder;
			}
			if (AccessTools.IsStruct(type))
			{
				LocalBuilder localBuilder2 = this.il.DeclareLocal(type);
				this.emitter.Emit(OpCodes.Ldloca, localBuilder2);
				this.emitter.Emit(OpCodes.Initobj, type);
				return localBuilder2;
			}
			if (AccessTools.IsValue(type))
			{
				LocalBuilder localBuilder3 = this.il.DeclareLocal(type);
				if (type == typeof(float))
				{
					this.emitter.Emit(OpCodes.Ldc_R4, 0f);
				}
				else if (type == typeof(double))
				{
					this.emitter.Emit(OpCodes.Ldc_R8, 0.0);
				}
				else if (type == typeof(long) || type == typeof(ulong))
				{
					this.emitter.Emit(OpCodes.Ldc_I8, 0L);
				}
				else
				{
					this.emitter.Emit(OpCodes.Ldc_I4, 0);
				}
				this.emitter.Emit(OpCodes.Stloc, localBuilder3);
				return localBuilder3;
			}
			return null;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00007164 File Offset: 0x00005364
		private static OpCode LoadIndOpCodeFor(Type type)
		{
			if (type.IsEnum)
			{
				return OpCodes.Ldind_I4;
			}
			if (type == typeof(float))
			{
				return OpCodes.Ldind_R4;
			}
			if (type == typeof(double))
			{
				return OpCodes.Ldind_R8;
			}
			if (type == typeof(byte))
			{
				return OpCodes.Ldind_U1;
			}
			if (type == typeof(ushort))
			{
				return OpCodes.Ldind_U2;
			}
			if (type == typeof(uint))
			{
				return OpCodes.Ldind_U4;
			}
			if (type == typeof(ulong))
			{
				return OpCodes.Ldind_I8;
			}
			if (type == typeof(sbyte))
			{
				return OpCodes.Ldind_I1;
			}
			if (type == typeof(short))
			{
				return OpCodes.Ldind_I2;
			}
			if (type == typeof(int))
			{
				return OpCodes.Ldind_I4;
			}
			if (type == typeof(long))
			{
				return OpCodes.Ldind_I8;
			}
			return OpCodes.Ldind_Ref;
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00007274 File Offset: 0x00005474
		private bool EmitOriginalBaseMethod()
		{
			MethodInfo methodInfo = this.original as MethodInfo;
			if (methodInfo != null)
			{
				this.emitter.Emit(OpCodes.Ldtoken, methodInfo);
			}
			else
			{
				ConstructorInfo constructorInfo = this.original as ConstructorInfo;
				if (constructorInfo == null)
				{
					return false;
				}
				this.emitter.Emit(OpCodes.Ldtoken, constructorInfo);
			}
			Type reflectedType = this.original.ReflectedType;
			if (reflectedType.IsGenericType)
			{
				this.emitter.Emit(OpCodes.Ldtoken, reflectedType);
			}
			this.emitter.Emit(OpCodes.Call, reflectedType.IsGenericType ? MethodPatcher.m_GetMethodFromHandle2 : MethodPatcher.m_GetMethodFromHandle1);
			return true;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00007314 File Offset: 0x00005514
		private void EmitCallParameter(MethodInfo patch, Dictionary<string, LocalBuilder> variables, bool allowFirsParamPassthrough, out LocalBuilder tmpObjectVar)
		{
			tmpObjectVar = null;
			bool flag = !this.original.IsStatic;
			ParameterInfo[] parameters = this.original.GetParameters();
			string[] array = parameters.Select((ParameterInfo p) => p.Name).ToArray<string>();
			List<ParameterInfo> list = patch.GetParameters().ToList<ParameterInfo>();
			if (allowFirsParamPassthrough && patch.ReturnType != typeof(void) && list.Count > 0 && list[0].ParameterType == patch.ReturnType)
			{
				list.RemoveRange(0, 1);
			}
			foreach (ParameterInfo parameterInfo in list)
			{
				LocalBuilder localBuilder2;
				if (parameterInfo.Name == "__originalMethod")
				{
					if (!this.EmitOriginalBaseMethod())
					{
						this.emitter.Emit(OpCodes.Ldnull);
					}
				}
				else if (parameterInfo.Name == "__instance")
				{
					if (this.original.IsStatic)
					{
						this.emitter.Emit(OpCodes.Ldnull);
					}
					else
					{
						object obj = this.original.DeclaringType != null && AccessTools.IsStruct(this.original.DeclaringType);
						bool isByRef = parameterInfo.ParameterType.IsByRef;
						object obj2 = obj;
						if (obj2 == isByRef)
						{
							this.emitter.Emit(OpCodes.Ldarg_0);
						}
						if (obj2 != null && !isByRef)
						{
							this.emitter.Emit(OpCodes.Ldarg_0);
							this.emitter.Emit(OpCodes.Ldobj, this.original.DeclaringType);
						}
						if (obj2 == 0 && isByRef)
						{
							this.emitter.Emit(OpCodes.Ldarga, 0);
						}
					}
				}
				else if (parameterInfo.Name.StartsWith("___", StringComparison.Ordinal))
				{
					string text = parameterInfo.Name.Substring("___".Length);
					FieldInfo fieldInfo;
					if (text.All(new Func<char, bool>(char.IsDigit)))
					{
						fieldInfo = AccessTools.DeclaredField(this.original.DeclaringType, int.Parse(text));
						if (fieldInfo == null)
						{
							throw new ArgumentException("No field found at given index in class " + this.original.DeclaringType.FullName, text);
						}
					}
					else
					{
						fieldInfo = AccessTools.Field(this.original.DeclaringType, text);
						if (fieldInfo == null)
						{
							throw new ArgumentException("No such field defined in class " + this.original.DeclaringType.FullName, text);
						}
					}
					if (fieldInfo.IsStatic)
					{
						this.emitter.Emit(parameterInfo.ParameterType.IsByRef ? OpCodes.Ldsflda : OpCodes.Ldsfld, fieldInfo);
					}
					else
					{
						this.emitter.Emit(OpCodes.Ldarg_0);
						this.emitter.Emit(parameterInfo.ParameterType.IsByRef ? OpCodes.Ldflda : OpCodes.Ldfld, fieldInfo);
					}
				}
				else if (parameterInfo.Name == "__state")
				{
					OpCode opCode = (parameterInfo.ParameterType.IsByRef ? OpCodes.Ldloca : OpCodes.Ldloc);
					LocalBuilder localBuilder;
					if (variables.TryGetValue(patch.DeclaringType.FullName, out localBuilder))
					{
						this.emitter.Emit(opCode, localBuilder);
					}
					else
					{
						this.emitter.Emit(OpCodes.Ldnull);
					}
				}
				else if (parameterInfo.Name == "__result")
				{
					Type returnedType = AccessTools.GetReturnedType(this.original);
					if (returnedType == typeof(void))
					{
						throw new Exception("Cannot get result from void method " + this.original.FullDescription());
					}
					Type type = parameterInfo.ParameterType;
					if (type.IsByRef && !returnedType.IsByRef)
					{
						type = type.GetElementType();
					}
					if (!type.IsAssignableFrom(returnedType))
					{
						throw new Exception(string.Concat(new string[]
						{
							"Cannot assign method return type ",
							returnedType.FullName,
							" to __result type ",
							type.FullName,
							" for method ",
							this.original.FullDescription()
						}));
					}
					OpCode opCode2 = ((parameterInfo.ParameterType.IsByRef && !returnedType.IsByRef) ? OpCodes.Ldloca : OpCodes.Ldloc);
					if (returnedType.IsValueType && parameterInfo.ParameterType == typeof(object).MakeByRefType())
					{
						opCode2 = OpCodes.Ldloc;
					}
					this.emitter.Emit(opCode2, variables["__result"]);
					if (returnedType.IsValueType)
					{
						if (parameterInfo.ParameterType == typeof(object))
						{
							this.emitter.Emit(OpCodes.Box, returnedType);
						}
						else if (parameterInfo.ParameterType == typeof(object).MakeByRefType())
						{
							this.emitter.Emit(OpCodes.Box, returnedType);
							tmpObjectVar = this.il.DeclareLocal(typeof(object));
							this.emitter.Emit(OpCodes.Stloc, tmpObjectVar);
							this.emitter.Emit(OpCodes.Ldloca, tmpObjectVar);
						}
					}
				}
				else if (variables.TryGetValue(parameterInfo.Name, out localBuilder2))
				{
					OpCode opCode3 = (parameterInfo.ParameterType.IsByRef ? OpCodes.Ldloca : OpCodes.Ldloc);
					this.emitter.Emit(opCode3, localBuilder2);
				}
				else
				{
					int argumentIndex;
					if (parameterInfo.Name.StartsWith("__", StringComparison.Ordinal))
					{
						if (!int.TryParse(parameterInfo.Name.Substring("__".Length), out argumentIndex))
						{
							throw new Exception("Parameter " + parameterInfo.Name + " does not contain a valid index");
						}
						if (argumentIndex < 0 || argumentIndex >= parameters.Length)
						{
							throw new Exception(string.Format("No parameter found at index {0}", argumentIndex));
						}
					}
					else
					{
						argumentIndex = patch.GetArgumentIndex(array, parameterInfo);
						if (argumentIndex == -1)
						{
							HarmonyMethod mergedFromType = HarmonyMethodExtensions.GetMergedFromType(parameterInfo.ParameterType);
							MethodType? methodType = mergedFromType.methodType;
							if (methodType == null)
							{
								mergedFromType.methodType = new MethodType?(MethodType.Normal);
							}
							MethodInfo methodInfo = mergedFromType.GetOriginalMethod() as MethodInfo;
							if (methodInfo != null)
							{
								ConstructorInfo constructor = parameterInfo.ParameterType.GetConstructor(new Type[]
								{
									typeof(object),
									typeof(IntPtr)
								});
								if (constructor != null)
								{
									Type declaringType = this.original.DeclaringType;
									if (methodInfo.IsStatic)
									{
										this.emitter.Emit(OpCodes.Ldnull);
									}
									else
									{
										this.emitter.Emit(OpCodes.Ldarg_0);
										if (declaringType.IsValueType)
										{
											this.emitter.Emit(OpCodes.Ldobj, declaringType);
											this.emitter.Emit(OpCodes.Box, declaringType);
										}
									}
									if (!methodInfo.IsStatic && !mergedFromType.nonVirtualDelegate)
									{
										this.emitter.Emit(OpCodes.Dup);
										this.emitter.Emit(OpCodes.Ldvirtftn, methodInfo);
									}
									else
									{
										this.emitter.Emit(OpCodes.Ldftn, methodInfo);
									}
									this.emitter.Emit(OpCodes.Newobj, constructor);
									continue;
								}
							}
							throw new Exception("Parameter \"" + parameterInfo.Name + "\" not found in method " + this.original.FullDescription());
						}
					}
					bool flag2 = !parameters[argumentIndex].IsOut && !parameters[argumentIndex].ParameterType.IsByRef;
					bool flag3 = !parameterInfo.IsOut && !parameterInfo.ParameterType.IsByRef;
					int num = argumentIndex + (flag ? 1 : 0) + (this.useStructReturnBuffer ? 1 : 0);
					if (flag2 == flag3)
					{
						this.emitter.Emit(OpCodes.Ldarg, num);
					}
					else if (flag2 && !flag3)
					{
						this.emitter.Emit(OpCodes.Ldarga, num);
					}
					else
					{
						this.emitter.Emit(OpCodes.Ldarg, num);
						this.emitter.Emit(MethodPatcher.LoadIndOpCodeFor(parameters[argumentIndex].ParameterType));
					}
				}
			}
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00007B70 File Offset: 0x00005D70
		private static bool PrefixAffectsOriginal(MethodInfo fix)
		{
			if (fix.ReturnType == typeof(bool))
			{
				return true;
			}
			return fix.GetParameters().Any(delegate(ParameterInfo p)
			{
				string name = p.Name;
				Type parameterType = p.ParameterType;
				return !(name == "__instance") && !(name == "__originalMethod") && !(name == "__state") && (p.IsOut || parameterType.IsByRef || (!AccessTools.IsValue(parameterType) && !AccessTools.IsStruct(parameterType)));
			});
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00007BC0 File Offset: 0x00005DC0
		private void AddPrefixes(Dictionary<string, LocalBuilder> variables, LocalBuilder runOriginalVariable)
		{
			this.prefixes.Do(delegate(MethodInfo fix)
			{
				if (!this.original.HasMethodBody())
				{
					throw new Exception("Methods without body cannot have prefixes. Use a transpiler instead.");
				}
				Label? label = (MethodPatcher.PrefixAffectsOriginal(fix) ? new Label?(this.il.DefineLabel()) : null);
				if (label != null)
				{
					this.emitter.Emit(OpCodes.Ldloc, runOriginalVariable);
					this.emitter.Emit(OpCodes.Brfalse_S, label.Value);
				}
				LocalBuilder localBuilder;
				this.EmitCallParameter(fix, variables, false, out localBuilder);
				this.emitter.Emit(OpCodes.Call, fix);
				if (localBuilder != null)
				{
					this.emitter.Emit(OpCodes.Ldloc, localBuilder);
					this.emitter.Emit(OpCodes.Unbox_Any, AccessTools.GetReturnedType(this.original));
					this.emitter.Emit(OpCodes.Stloc, variables["__result"]);
				}
				Type type = fix.ReturnType;
				if (type != typeof(void))
				{
					if (type != typeof(bool))
					{
						throw new Exception(string.Format("Prefix patch {0} has not \"bool\" or \"void\" return type: {1}", fix, fix.ReturnType));
					}
					this.emitter.Emit(OpCodes.Stloc, runOriginalVariable);
				}
				if (label != null)
				{
					this.il.MarkLabel(label.Value);
					this.emitter.Emit(OpCodes.Nop);
				}
			});
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00007C00 File Offset: 0x00005E00
		private bool AddPostfixes(Dictionary<string, LocalBuilder> variables, bool passthroughPatches)
		{
			bool result = false;
			this.postfixes.Where((MethodInfo fix) => passthroughPatches == (fix.ReturnType != typeof(void))).Do(delegate(MethodInfo fix)
			{
				if (!this.original.HasMethodBody())
				{
					throw new Exception("Methods without body cannot have postfixes. Use a transpiler instead.");
				}
				LocalBuilder localBuilder;
				this.EmitCallParameter(fix, variables, true, out localBuilder);
				this.emitter.Emit(OpCodes.Call, fix);
				if (localBuilder != null)
				{
					this.emitter.Emit(OpCodes.Ldloc, localBuilder);
					this.emitter.Emit(OpCodes.Unbox_Any, AccessTools.GetReturnedType(this.original));
					this.emitter.Emit(OpCodes.Stloc, variables["__result"]);
				}
				if (!(fix.ReturnType != typeof(void)))
				{
					return;
				}
				ParameterInfo parameterInfo = fix.GetParameters().FirstOrDefault<ParameterInfo>();
				if (parameterInfo != null && fix.ReturnType == parameterInfo.ParameterType)
				{
					result = true;
					return;
				}
				if (parameterInfo != null)
				{
					throw new Exception(string.Format("Return type of pass through postfix {0} does not match type of its first parameter", fix));
				}
				throw new Exception(string.Format("Postfix patch {0} must have a \"void\" return type", fix));
			});
			return result;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00007C60 File Offset: 0x00005E60
		private bool AddFinalizers(Dictionary<string, LocalBuilder> variables, bool catchExceptions)
		{
			bool rethrowPossible = true;
			this.finalizers.Do(delegate(MethodInfo fix)
			{
				if (!this.original.HasMethodBody())
				{
					throw new Exception("Methods without body cannot have finalizers. Use a transpiler instead.");
				}
				if (catchExceptions)
				{
					Label? label;
					this.emitter.MarkBlockBefore(new ExceptionBlock(ExceptionBlockType.BeginExceptionBlock, null), out label);
				}
				LocalBuilder localBuilder;
				this.EmitCallParameter(fix, variables, false, out localBuilder);
				this.emitter.Emit(OpCodes.Call, fix);
				if (localBuilder != null)
				{
					this.emitter.Emit(OpCodes.Ldloc, localBuilder);
					this.emitter.Emit(OpCodes.Unbox_Any, AccessTools.GetReturnedType(this.original));
					this.emitter.Emit(OpCodes.Stloc, variables["__result"]);
				}
				if (fix.ReturnType != typeof(void))
				{
					this.emitter.Emit(OpCodes.Stloc, variables["__exception"]);
					rethrowPossible = false;
				}
				if (catchExceptions)
				{
					Label? label2;
					this.emitter.MarkBlockBefore(new ExceptionBlock(ExceptionBlockType.BeginCatchBlock, null), out label2);
					this.emitter.Emit(OpCodes.Pop);
					this.emitter.MarkBlockAfter(new ExceptionBlock(ExceptionBlockType.EndExceptionBlock, null));
				}
			});
			return rethrowPossible;
		}

		// Token: 0x04000077 RID: 119
		private const string INSTANCE_PARAM = "__instance";

		// Token: 0x04000078 RID: 120
		private const string ORIGINAL_METHOD_PARAM = "__originalMethod";

		// Token: 0x04000079 RID: 121
		private const string RESULT_VAR = "__result";

		// Token: 0x0400007A RID: 122
		private const string STATE_VAR = "__state";

		// Token: 0x0400007B RID: 123
		private const string EXCEPTION_VAR = "__exception";

		// Token: 0x0400007C RID: 124
		private const string PARAM_INDEX_PREFIX = "__";

		// Token: 0x0400007D RID: 125
		private const string INSTANCE_FIELD_PREFIX = "___";

		// Token: 0x0400007E RID: 126
		private readonly bool debug;

		// Token: 0x0400007F RID: 127
		private readonly MethodBase original;

		// Token: 0x04000080 RID: 128
		private readonly MethodBase source;

		// Token: 0x04000081 RID: 129
		private readonly List<MethodInfo> prefixes;

		// Token: 0x04000082 RID: 130
		private readonly List<MethodInfo> postfixes;

		// Token: 0x04000083 RID: 131
		private readonly List<MethodInfo> transpilers;

		// Token: 0x04000084 RID: 132
		private readonly List<MethodInfo> finalizers;

		// Token: 0x04000085 RID: 133
		private readonly int idx;

		// Token: 0x04000086 RID: 134
		private readonly bool useStructReturnBuffer;

		// Token: 0x04000087 RID: 135
		private readonly Type returnType;

		// Token: 0x04000088 RID: 136
		private readonly DynamicMethodDefinition patch;

		// Token: 0x04000089 RID: 137
		private readonly ILGenerator il;

		// Token: 0x0400008A RID: 138
		private readonly Emitter emitter;

		// Token: 0x0400008B RID: 139
		private static readonly MethodInfo m_GetMethodFromHandle1 = typeof(MethodBase).GetMethod("GetMethodFromHandle", new Type[] { typeof(RuntimeMethodHandle) });

		// Token: 0x0400008C RID: 140
		private static readonly MethodInfo m_GetMethodFromHandle2 = typeof(MethodBase).GetMethod("GetMethodFromHandle", new Type[]
		{
			typeof(RuntimeMethodHandle),
			typeof(RuntimeTypeHandle)
		});
	}
}
