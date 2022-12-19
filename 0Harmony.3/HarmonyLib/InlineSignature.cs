using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Mono.Cecil;
using MonoMod.Utils;

namespace HarmonyLib
{
	// Token: 0x0200006D RID: 109
	public class InlineSignature : ICallSiteGenerator
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x060001F0 RID: 496 RVA: 0x0000B00B File Offset: 0x0000920B
		// (set) Token: 0x060001F1 RID: 497 RVA: 0x0000B013 File Offset: 0x00009213
		public bool HasThis { get; set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x0000B01C File Offset: 0x0000921C
		// (set) Token: 0x060001F3 RID: 499 RVA: 0x0000B024 File Offset: 0x00009224
		public bool ExplicitThis { get; set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x060001F4 RID: 500 RVA: 0x0000B02D File Offset: 0x0000922D
		// (set) Token: 0x060001F5 RID: 501 RVA: 0x0000B035 File Offset: 0x00009235
		public CallingConvention CallingConvention { get; set; } = CallingConvention.Winapi;

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x0000B03E File Offset: 0x0000923E
		// (set) Token: 0x060001F7 RID: 503 RVA: 0x0000B046 File Offset: 0x00009246
		public List<object> Parameters { get; set; } = new List<object>();

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x060001F8 RID: 504 RVA: 0x0000B04F File Offset: 0x0000924F
		// (set) Token: 0x060001F9 RID: 505 RVA: 0x0000B057 File Offset: 0x00009257
		public object ReturnType { get; set; } = typeof(void);

		// Token: 0x060001FA RID: 506 RVA: 0x0000B060 File Offset: 0x00009260
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

		// Token: 0x060001FB RID: 507 RVA: 0x0000B0D4 File Offset: 0x000092D4
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

		// Token: 0x060001FC RID: 508 RVA: 0x0000B144 File Offset: 0x00009344
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

		// Token: 0x060001FD RID: 509 RVA: 0x0000B1E4 File Offset: 0x000093E4
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

		// Token: 0x0200006E RID: 110
		public class ModifierType
		{
			// Token: 0x060001FF RID: 511 RVA: 0x0000B2B4 File Offset: 0x000094B4
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

			// Token: 0x06000200 RID: 512 RVA: 0x0000B33C File Offset: 0x0000953C
			internal TypeReference ToTypeReference(ModuleDefinition module)
			{
				if (this.IsOptional)
				{
					return new OptionalModifierType(module.ImportReference(this.Modifier), InlineSignature.GetTypeReference(module, this.Type));
				}
				return new RequiredModifierType(module.ImportReference(this.Modifier), InlineSignature.GetTypeReference(module, this.Type));
			}

			// Token: 0x0400013C RID: 316
			public bool IsOptional;

			// Token: 0x0400013D RID: 317
			public Type Modifier;

			// Token: 0x0400013E RID: 318
			public object Type;
		}
	}
}
