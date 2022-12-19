using System;
using System.Globalization;
using System.Reflection;
using System.Speech.Internal.SrgsParser;
using System.Speech.Recognition;
using System.Speech.Recognition.SrgsGrammar;
using System.Text;

namespace System.Speech.Internal.SrgsCompiler
{
	// Token: 0x02000102 RID: 258
	internal class AppDomainCompilerProxy : MarshalByRefObject
	{
		// Token: 0x06000912 RID: 2322 RVA: 0x00029D60 File Offset: 0x00027F60
		internal Exception CheckAssembly(byte[] il, int iCfg, string language, string nameSpace, string[] ruleNames, string[] methodNames, int[] methodScripts)
		{
			try
			{
				Assembly assembly = Assembly.Load(il);
				this._constructors = new string[ruleNames.Length];
				int i = 0;
				int num = ruleNames.Length;
				while (i < num)
				{
					string text = ruleNames[i];
					string text2 = methodNames[i];
					this._constructors[i] = string.Empty;
					string text3 = ((!string.IsNullOrEmpty(nameSpace)) ? (nameSpace + ".") : string.Empty) + text;
					Type type = assembly.GetType(text3);
					if (type == null)
					{
						XmlParser.ThrowSrgsException(SRID.CannotFindClass, new object[] { text, nameSpace });
					}
					if (!type.IsSubclassOf(typeof(Grammar)))
					{
						XmlParser.ThrowSrgsException(SRID.StrongTypedGrammarNotAGrammar, new object[] { text3, nameSpace });
					}
					MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					AppDomainCompilerProxy.ScriptRefStruct scriptRefStruct = new AppDomainCompilerProxy.ScriptRefStruct(text, (RuleMethodScript)methodScripts[i]);
					bool flag = false;
					foreach (MethodInfo methodInfo in methods)
					{
						if (methodInfo.Name == text2)
						{
							ParameterInfo[] parameters = methodInfo.GetParameters();
							Type type2 = null;
							switch (scriptRefStruct._method)
							{
							case RuleMethodScript.onInit:
							{
								string[] constructors = this._constructors;
								int num2 = i;
								constructors[num2] += this.GenerateConstructor(iCfg, parameters, language, text);
								type2 = typeof(SrgsRule[]);
								break;
							}
							case RuleMethodScript.onParse:
								AppDomainCompilerProxy.ThrowIfMultipleOverloads(flag, text2);
								if (parameters.Length != 2 || parameters[0].ParameterType != typeof(SemanticValue) || parameters[1].ParameterType != typeof(RecognizedWordUnit[]))
								{
									XmlParser.ThrowSrgsException(SRID.RuleScriptInvalidParameters, new object[] { text2, scriptRefStruct._rule });
								}
								type2 = typeof(object);
								break;
							case RuleMethodScript.onRecognition:
								AppDomainCompilerProxy.ThrowIfMultipleOverloads(flag, text2);
								if (parameters.Length != 1 || parameters[0].ParameterType != typeof(RecognitionResult))
								{
									XmlParser.ThrowSrgsException(SRID.RuleScriptInvalidParameters, new object[] { text2, scriptRefStruct._rule });
								}
								type2 = typeof(object);
								break;
							case RuleMethodScript.onError:
								AppDomainCompilerProxy.ThrowIfMultipleOverloads(flag, text2);
								if (parameters.Length != 1 || parameters[0].ParameterType != typeof(Exception))
								{
									XmlParser.ThrowSrgsException(SRID.RuleScriptInvalidParameters, new object[] { text2, scriptRefStruct._rule });
								}
								type2 = typeof(void);
								break;
							}
							if (methodInfo.ReturnType != type2)
							{
								XmlParser.ThrowSrgsException(SRID.RuleScriptInvalidReturnType, new object[] { text2, scriptRefStruct._rule });
							}
							flag = true;
						}
					}
					if (!flag)
					{
						XmlParser.ThrowSrgsException(SRID.RuleScriptNotFound, new object[]
						{
							text2,
							scriptRefStruct._rule,
							scriptRefStruct._method.ToString()
						});
					}
					if (!type.IsPublic)
					{
						XmlParser.ThrowSrgsException(SRID.ClassNotPublic, new object[] { text });
					}
					i++;
				}
			}
			catch (Exception ex)
			{
				return ex;
			}
			return null;
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x0002A09C File Offset: 0x0002829C
		internal string[] Constructors()
		{
			return this._constructors;
		}

		// Token: 0x06000914 RID: 2324 RVA: 0x0002A0A4 File Offset: 0x000282A4
		internal string GenerateConstructor(int iCfg, ParameterInfo[] parameters, string language, string classname)
		{
			string text = string.Empty;
			if (!(language == "C#"))
			{
				if (!(language == "VB.Net"))
				{
					XmlParser.ThrowSrgsException(SRID.UnsupportedLanguage, new object[] { language });
				}
				else
				{
					text = AppDomainCompilerProxy.WrapConstructorVB(iCfg, parameters, classname);
				}
			}
			else
			{
				text = AppDomainCompilerProxy.WrapConstructorCSharp(iCfg, parameters, classname);
			}
			return text;
		}

		// Token: 0x06000915 RID: 2325 RVA: 0x0002A0FD File Offset: 0x000282FD
		private static void ThrowIfMultipleOverloads(bool found, string method)
		{
			if (found)
			{
				XmlParser.ThrowSrgsException(SRID.OverloadNotAllowed, new object[] { method });
			}
		}

		// Token: 0x06000916 RID: 2326 RVA: 0x0002A114 File Offset: 0x00028314
		private static string WrapConstructorCSharp(int iCfg, ParameterInfo[] parameters, string classname)
		{
			StringBuilder stringBuilder = new StringBuilder(200);
			stringBuilder.Append(" public ");
			stringBuilder.Append(classname);
			stringBuilder.Append(" (");
			if (parameters != null)
			{
				int num = 0;
				foreach (ParameterInfo parameterInfo in parameters)
				{
					if (num++ > 0)
					{
						stringBuilder.Append(", ");
					}
					if (num == parameters.Length && parameterInfo.ParameterType.IsArray)
					{
						object[] customAttributes = parameterInfo.GetCustomAttributes(false);
						foreach (object obj in customAttributes)
						{
							if (obj is ParamArrayAttribute)
							{
								stringBuilder.Append("params ");
								break;
							}
						}
					}
					stringBuilder.Append(parameterInfo.ParameterType.FullName);
					stringBuilder.Append(" ");
					stringBuilder.Append(parameterInfo.Name);
				}
			}
			stringBuilder.Append(" )\n  {\n object [] onInitParams = new object [");
			stringBuilder.Append((parameters == null) ? 0 : parameters.Length);
			stringBuilder.Append("];\n");
			int num2 = 0;
			while (parameters != null && num2 < parameters.Length)
			{
				stringBuilder.Append("onInitParams [");
				stringBuilder.Append(num2);
				stringBuilder.Append("] = ");
				stringBuilder.Append(parameters[num2].Name);
				stringBuilder.Append(";\n");
				num2++;
			}
			stringBuilder.Append("ResourceName = \"");
			stringBuilder.Append(iCfg.ToString(CultureInfo.InvariantCulture));
			stringBuilder.Append(".CFG\";\nStgInit (onInitParams);");
			stringBuilder.Append("\n  } \n");
			return stringBuilder.ToString();
		}

		// Token: 0x06000917 RID: 2327 RVA: 0x0002A2BC File Offset: 0x000284BC
		private static string WrapConstructorVB(int iCfg, ParameterInfo[] parameters, string classname)
		{
			StringBuilder stringBuilder = new StringBuilder(200);
			stringBuilder.Append("Public Sub New");
			stringBuilder.Append(" (");
			if (parameters != null)
			{
				int num = 0;
				foreach (ParameterInfo parameterInfo in parameters)
				{
					if (num++ > 0)
					{
						stringBuilder.Append(", ");
					}
					if (!parameterInfo.ParameterType.IsByRef)
					{
						stringBuilder.Append("ByVal ");
					}
					if (num == parameters.Length && parameterInfo.ParameterType.IsArray)
					{
						object[] customAttributes = parameterInfo.GetCustomAttributes(false);
						foreach (object obj in customAttributes)
						{
							if (obj is ParamArrayAttribute)
							{
								stringBuilder.Append("ParamArray ");
								break;
							}
						}
					}
					stringBuilder.Append(parameterInfo.Name);
					if (parameterInfo.ParameterType.IsArray)
					{
						stringBuilder.Append("()");
					}
					stringBuilder.Append(" as ");
					stringBuilder.Append(parameterInfo.ParameterType.Name);
				}
			}
			stringBuilder.Append(" )\n  Dim onInitParams () as Object = {");
			int num2 = 0;
			while (parameters != null && num2 < parameters.Length)
			{
				if (num2 > 0)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append(parameters[num2].Name);
				num2++;
			}
			stringBuilder.Append("}\n");
			stringBuilder.Append("ResourceName = \"");
			stringBuilder.Append(iCfg.ToString(CultureInfo.InvariantCulture));
			stringBuilder.Append(".CFG\"\nStgInit (onInitParams)\n");
			stringBuilder.Append("\nEnd Sub \n");
			return stringBuilder.ToString();
		}

		// Token: 0x0400064E RID: 1614
		internal string[] _constructors;

		// Token: 0x020001AB RID: 427
		private class ScriptRefStruct
		{
			// Token: 0x06000BBC RID: 3004 RVA: 0x0002E1E2 File Offset: 0x0002C3E2
			internal ScriptRefStruct(string rule, RuleMethodScript method)
			{
				this._rule = rule;
				this._method = method;
			}

			// Token: 0x040009AA RID: 2474
			internal string _rule;

			// Token: 0x040009AB RID: 2475
			internal RuleMethodScript _method;
		}
	}
}
