using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Mono.Cecil;
using MonoMod.Utils;

namespace HarmonyLib
{
	// Token: 0x02000070 RID: 112
	public class InlineSignature : ICallSiteGenerator
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600020B RID: 523 RVA: 0x0000C1ED File Offset: 0x0000A3ED
		// (set) Token: 0x0600020C RID: 524 RVA: 0x0000C1F5 File Offset: 0x0000A3F5
		public bool HasThis { get; set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600020D RID: 525 RVA: 0x0000C1FE File Offset: 0x0000A3FE
		// (set) Token: 0x0600020E RID: 526 RVA: 0x0000C206 File Offset: 0x0000A406
		public bool ExplicitThis { get; set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600020F RID: 527 RVA: 0x0000C20F File Offset: 0x0000A40F
		// (set) Token: 0x06000210 RID: 528 RVA: 0x0000C217 File Offset: 0x0000A417
		public CallingConvention CallingConvention { get; set; } = CallingConvention.Winapi;

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000211 RID: 529 RVA: 0x0000C220 File Offset: 0x0000A420
		// (set) Token: 0x06000212 RID: 530 RVA: 0x0000C228 File Offset: 0x0000A428
		public List<object> Parameters { get; set; } = new List<object>();

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000213 RID: 531 RVA: 0x0000C231 File Offset: 0x0000A431
		// (set) Token: 0x06000214 RID: 532 RVA: 0x0000C239 File Offset: 0x0000A439
		public object ReturnType { get; set; } = typeof(void);

		// Token: 0x06000215 RID: 533 RVA: 0x0000C244 File Offset: 0x0000A444
		public override string ToString()
		{
			Type type = this.ReturnType as Type;
			string text;
			if (type == null)
			{
				object returnType = this.ReturnType;
				text = ((returnType != null) ? returnType.ToString() : null);
			}
			else
			{
				text = type.FullDescription();
			}
			return text + " (" + this.Parameters.Join(delegate(object p)
			{
				Type type2 = p as Type;
				if (type2 != null)
				{
					return type2.FullDescription();
				}
				if (p == null)
				{
					return null;
				}
				return p.ToString();
			}, ", ") + ")";
		}

		// Token: 0x06000216 RID: 534 RVA: 0x0000C2B8 File Offset: 0x0000A4B8
		internal static TypeReference GetTypeReference(ModuleDefinition module, object param)
		{
			Type type = param as Type;
			TypeReference typeReference;
			if (type == null)
			{
				InlineSignature inlineSignature = param as InlineSignature;
				if (inlineSignature == null)
				{
					InlineSignature.ModifierType modifierType = param as InlineSignature.ModifierType;
					if (modifierType == null)
					{
						throw new NotSupportedException(string.Format("Unsupported inline signature parameter type: {0} ({1})", param, (param != null) ? param.GetType().FullDescription() : null));
					}
					typeReference = modifierType.ToTypeReference(module);
				}
				else
				{
					typeReference = inlineSignature.ToFunctionPointer(module);
				}
			}
			else
			{
				typeReference = module.ImportReference(type);
			}
			return typeReference;
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000C328 File Offset: 0x0000A528
		CallSite ICallSiteGenerator.ToCallSite(ModuleDefinition module)
		{
			CallSite callSite = new CallSite(InlineSignature.GetTypeReference(module, this.ReturnType))
			{
				HasThis = this.HasThis,
				ExplicitThis = this.ExplicitThis,
				CallingConvention = (MethodCallingConvention)((byte)this.CallingConvention - 1)
			};
			foreach (object obj in this.Parameters)
			{
				callSite.Parameters.Add(new ParameterDefinition(InlineSignature.GetTypeReference(module, obj)));
			}
			return callSite;
		}

		// Token: 0x06000218 RID: 536 RVA: 0x0000C3C8 File Offset: 0x0000A5C8
		private FunctionPointerType ToFunctionPointer(ModuleDefinition module)
		{
			FunctionPointerType functionPointerType = new FunctionPointerType
			{
				ReturnType = InlineSignature.GetTypeReference(module, this.ReturnType),
				HasThis = this.HasThis,
				ExplicitThis = this.ExplicitThis,
				CallingConvention = (MethodCallingConvention)((byte)this.CallingConvention - 1)
			};
			foreach (object obj in this.Parameters)
			{
				functionPointerType.Parameters.Add(new ParameterDefinition(InlineSignature.GetTypeReference(module, obj)));
			}
			return functionPointerType;
		}

		// Token: 0x02000071 RID: 113
		public class ModifierType
		{
			// Token: 0x0600021A RID: 538 RVA: 0x0000C498 File Offset: 0x0000A698
			public override string ToString()
			{
				string[] array = new string[6];
				int num = 0;
				Type type = this.Type as Type;
				string text;
				if (type == null)
				{
					object type2 = this.Type;
					text = ((type2 != null) ? type2.ToString() : null);
				}
				else
				{
					text = type.FullDescription();
				}
				array[num] = text;
				array[1] = " mod";
				array[2] = (this.IsOptional ? "opt" : "req");
				array[3] = "(";
				int num2 = 4;
				Type modifier = this.Modifier;
				array[num2] = ((modifier != null) ? modifier.FullDescription() : null);
				array[5] = ")";
				return string.Concat(array);
			}

			// Token: 0x0600021B RID: 539 RVA: 0x0000C520 File Offset: 0x0000A720
			internal TypeReference ToTypeReference(ModuleDefinition module)
			{
				if (this.IsOptional)
				{
					return new OptionalModifierType(module.ImportReference(this.Modifier), InlineSignature.GetTypeReference(module, this.Type));
				}
				return new RequiredModifierType(module.ImportReference(this.Modifier), InlineSignature.GetTypeReference(module, this.Type));
			}

			// Token: 0x0400014D RID: 333
			public bool IsOptional;

			// Token: 0x0400014E RID: 334
			public Type Modifier;

			// Token: 0x0400014F RID: 335
			public object Type;
		}
	}
}
