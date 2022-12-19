using System;
using System.Globalization;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using System.Speech.Recognition;
using System.Speech.Recognition.SrgsGrammar;
using System.Text;

namespace System.Speech.Internal.SrgsCompiler
{
	// Token: 0x02000184 RID: 388
	internal class AppDomainGrammarProxy : MarshalByRefObject
	{
		// Token: 0x060009B1 RID: 2481 RVA: 0x0002A664 File Offset: 0x00029664
		internal SrgsRule[] OnInit(string method, object[] parameters, string onInitParameters, out Exception exceptionThrown)
		{
			exceptionThrown = null;
			SrgsRule[] array3;
			try
			{
				if (!string.IsNullOrEmpty(onInitParameters))
				{
					parameters = this.MatchInitParameters(method, onInitParameters, this._rule, this._rule);
				}
				Type[] array = new Type[(parameters != null) ? parameters.Length : 0];
				if (parameters != null)
				{
					for (int i = 0; i < parameters.Length; i++)
					{
						array[i] = parameters[i].GetType();
					}
				}
				MethodInfo method2 = this._grammarType.GetMethod(method, array);
				if (method2 == null)
				{
					throw new InvalidOperationException(SR.Get(SRID.ArgumentMismatch, new object[0]));
				}
				SrgsRule[] array2 = null;
				if (method2 != null)
				{
					this._internetPermissionSet.PermitOnly();
					array2 = (SrgsRule[])method2.Invoke(this._grammar, parameters);
				}
				array3 = array2;
			}
			catch (Exception ex)
			{
				exceptionThrown = ex;
				array3 = null;
			}
			return array3;
		}

		// Token: 0x060009B2 RID: 2482 RVA: 0x0002A728 File Offset: 0x00029728
		internal object OnRecognition(string method, object[] parameters, out Exception exceptionThrown)
		{
			exceptionThrown = null;
			try
			{
				MethodInfo method2 = this._grammarType.GetMethod(method, 52);
				this._internetPermissionSet.PermitOnly();
				return method2.Invoke(this._grammar, parameters);
			}
			catch (Exception ex)
			{
				exceptionThrown = ex;
			}
			return null;
		}

		// Token: 0x060009B3 RID: 2483 RVA: 0x0002A77C File Offset: 0x0002977C
		internal object OnParse(string rule, string method, object[] parameters, out Exception exceptionThrown)
		{
			exceptionThrown = null;
			object obj;
			try
			{
				MethodInfo methodInfo;
				Grammar grammar;
				this.GetRuleInstance(rule, method, out methodInfo, out grammar);
				this._internetPermissionSet.PermitOnly();
				obj = methodInfo.Invoke(grammar, parameters);
			}
			catch (Exception ex)
			{
				exceptionThrown = ex;
				obj = null;
			}
			return obj;
		}

		// Token: 0x060009B4 RID: 2484 RVA: 0x0002A7CC File Offset: 0x000297CC
		internal void OnError(string rule, string method, object[] parameters, out Exception exceptionThrown)
		{
			exceptionThrown = null;
			try
			{
				MethodInfo methodInfo;
				Grammar grammar;
				this.GetRuleInstance(rule, method, out methodInfo, out grammar);
				this._internetPermissionSet.PermitOnly();
				methodInfo.Invoke(grammar, parameters);
			}
			catch (Exception ex)
			{
				exceptionThrown = ex;
			}
		}

		// Token: 0x060009B5 RID: 2485 RVA: 0x0002A818 File Offset: 0x00029818
		internal void Init(string rule, byte[] il, byte[] pdb)
		{
			this._assembly = Assembly.Load(il, pdb);
			this._grammarType = AppDomainGrammarProxy.GetTypeForRule(this._assembly, rule);
			if (this._grammarType == null)
			{
				throw new FormatException(SR.Get(SRID.RecognizerRuleNotFoundStream, new object[] { rule }));
			}
			this._rule = rule;
			try
			{
				this._grammar = (Grammar)this._assembly.CreateInstance(this._grammarType.FullName);
			}
			catch (MissingMemberException)
			{
				throw new ArgumentException(SR.Get(SRID.RuleScriptInvalidParameters, new object[]
				{
					this._grammarType.FullName,
					rule
				}), "rule");
			}
			this._internetPermissionSet = PolicyLevel.CreateAppDomainLevel().GetNamedPermissionSet("Internet");
			this._internetPermissionSet.AddPermission(new ReflectionPermission(1));
		}

		// Token: 0x060009B6 RID: 2486 RVA: 0x0002A8F8 File Offset: 0x000298F8
		private void GetRuleInstance(string rule, string method, out MethodInfo onParse, out Grammar grammar)
		{
			Type type = ((rule == this._rule) ? this._grammarType : AppDomainGrammarProxy.GetTypeForRule(this._assembly, rule));
			if (type == null || !type.IsSubclassOf(typeof(Grammar)))
			{
				throw new FormatException(SR.Get(SRID.RecognizerInvalidBinaryGrammar, new object[0]));
			}
			try
			{
				grammar = ((type == this._grammarType) ? this._grammar : ((Grammar)this._assembly.CreateInstance(type.FullName)));
			}
			catch (MissingMemberException)
			{
				throw new ArgumentException(SR.Get(SRID.RuleScriptInvalidParameters, new object[] { type.FullName, rule }), "rule");
			}
			onParse = grammar.MethodInfo(method);
		}

		// Token: 0x060009B7 RID: 2487 RVA: 0x0002A9C4 File Offset: 0x000299C4
		private static Type GetTypeForRule(Assembly assembly, string rule)
		{
			foreach (Type type in assembly.GetTypes())
			{
				if (type.Name == rule && type.IsPublic && type.IsSubclassOf(typeof(Grammar)))
				{
					return type;
				}
			}
			return null;
		}

		// Token: 0x060009B8 RID: 2488 RVA: 0x0002AA18 File Offset: 0x00029A18
		private object[] MatchInitParameters(string method, string onInitParameters, string grammar, string rule)
		{
			MethodInfo[] methods = this._grammarType.GetMethods();
			AppDomainGrammarProxy.NameValuePair[] array = AppDomainGrammarProxy.ParseInitParams(onInitParameters);
			object[] array2 = new object[array.Length];
			bool flag = false;
			int num = 0;
			while (num < methods.Length && !flag)
			{
				if (!(methods[num].Name != method))
				{
					ParameterInfo[] parameters = methods[num].GetParameters();
					if (parameters.Length <= array.Length)
					{
						flag = true;
						int num2 = 0;
						while (num2 < array.Length && flag)
						{
							AppDomainGrammarProxy.NameValuePair nameValuePair = array[num2];
							if (nameValuePair._name == null)
							{
								array2[num2] = nameValuePair._value;
							}
							else
							{
								bool flag2 = false;
								for (int i = 0; i < parameters.Length; i++)
								{
									if (parameters[i].Name == nameValuePair._name)
									{
										array2[i] = AppDomainGrammarProxy.ParseValue(parameters[i].ParameterType, nameValuePair._value);
										flag2 = true;
										break;
									}
								}
								if (!flag2)
								{
									flag = false;
								}
							}
							num2++;
						}
					}
				}
				num++;
			}
			if (!flag)
			{
				throw new FormatException(SR.Get(SRID.CantFindAConstructor, new object[]
				{
					grammar,
					rule,
					AppDomainGrammarProxy.FormatConstructorParameters(methods, method)
				}));
			}
			return array2;
		}

		// Token: 0x060009B9 RID: 2489 RVA: 0x0002AB4C File Offset: 0x00029B4C
		private static object ParseValue(Type type, string value)
		{
			if (type == typeof(string))
			{
				return value;
			}
			return type.InvokeMember("Parse", 256, null, null, new object[] { value }, CultureInfo.InvariantCulture);
		}

		// Token: 0x060009BA RID: 2490 RVA: 0x0002AB8C File Offset: 0x00029B8C
		private static string FormatConstructorParameters(MethodInfo[] cis, string method)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < cis.Length; i++)
			{
				if (cis[i].Name == method)
				{
					stringBuilder.Append((stringBuilder.Length > 0) ? " or sapi:parms=\"" : "sapi:parms=\"");
					ParameterInfo[] parameters = cis[i].GetParameters();
					for (int j = 0; j < parameters.Length; j++)
					{
						if (j > 0)
						{
							stringBuilder.Append(';');
						}
						ParameterInfo parameterInfo = parameters[j];
						stringBuilder.Append(parameterInfo.Name);
						stringBuilder.Append(':');
						stringBuilder.Append(parameterInfo.ParameterType.Name);
					}
					stringBuilder.Append("\"");
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060009BB RID: 2491 RVA: 0x0002AC44 File Offset: 0x00029C44
		private static AppDomainGrammarProxy.NameValuePair[] ParseInitParams(string initParameters)
		{
			string[] array = initParameters.Split(new char[] { ';' }, 0);
			AppDomainGrammarProxy.NameValuePair[] array2 = new AppDomainGrammarProxy.NameValuePair[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				int num = text.IndexOf(':');
				if (num >= 0)
				{
					array2[i]._name = text.Substring(0, num);
					array2[i]._value = text.Substring(num + 1);
				}
				else
				{
					array2[i]._value = text;
				}
			}
			return array2;
		}

		// Token: 0x040008E6 RID: 2278
		private Grammar _grammar;

		// Token: 0x040008E7 RID: 2279
		private Assembly _assembly;

		// Token: 0x040008E8 RID: 2280
		private string _rule;

		// Token: 0x040008E9 RID: 2281
		private Type _grammarType;

		// Token: 0x040008EA RID: 2282
		private PermissionSet _internetPermissionSet;

		// Token: 0x02000185 RID: 389
		private struct NameValuePair
		{
			// Token: 0x040008EB RID: 2283
			internal string _name;

			// Token: 0x040008EC RID: 2284
			internal string _value;
		}
	}
}
