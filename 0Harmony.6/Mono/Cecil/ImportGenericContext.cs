using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200012C RID: 300
	internal struct ImportGenericContext
	{
		// Token: 0x1700011F RID: 287
		// (get) Token: 0x0600086B RID: 2155 RVA: 0x00021D8F File Offset: 0x0001FF8F
		public bool IsEmpty
		{
			get
			{
				return this.stack == null;
			}
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x00021D9A File Offset: 0x0001FF9A
		public ImportGenericContext(IGenericParameterProvider provider)
		{
			if (provider == null)
			{
				throw new ArgumentNullException("provider");
			}
			this.stack = null;
			this.Push(provider);
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x00021DB8 File Offset: 0x0001FFB8
		public void Push(IGenericParameterProvider provider)
		{
			if (this.stack == null)
			{
				this.stack = new Collection<IGenericParameterProvider>(1) { provider };
				return;
			}
			this.stack.Add(provider);
		}

		// Token: 0x0600086E RID: 2158 RVA: 0x00021DE2 File Offset: 0x0001FFE2
		public void Pop()
		{
			this.stack.RemoveAt(this.stack.Count - 1);
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x00021DFC File Offset: 0x0001FFFC
		public TypeReference MethodParameter(string method, int position)
		{
			for (int i = this.stack.Count - 1; i >= 0; i--)
			{
				MethodReference methodReference = this.stack[i] as MethodReference;
				if (methodReference != null && !(method != this.NormalizeMethodName(methodReference)))
				{
					return methodReference.GenericParameters[position];
				}
			}
			throw new InvalidOperationException();
		}

		// Token: 0x06000870 RID: 2160 RVA: 0x00021E57 File Offset: 0x00020057
		public string NormalizeMethodName(MethodReference method)
		{
			return method.DeclaringType.GetElementType().FullName + "." + method.Name;
		}

		// Token: 0x06000871 RID: 2161 RVA: 0x00021E7C File Offset: 0x0002007C
		public TypeReference TypeParameter(string type, int position)
		{
			for (int i = this.stack.Count - 1; i >= 0; i--)
			{
				TypeReference typeReference = ImportGenericContext.GenericTypeFor(this.stack[i]);
				if (!(typeReference.FullName != type))
				{
					return typeReference.GenericParameters[position];
				}
			}
			throw new InvalidOperationException();
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x00021ED4 File Offset: 0x000200D4
		private static TypeReference GenericTypeFor(IGenericParameterProvider context)
		{
			TypeReference typeReference = context as TypeReference;
			if (typeReference != null)
			{
				return typeReference.GetElementType();
			}
			MethodReference methodReference = context as MethodReference;
			if (methodReference != null)
			{
				return methodReference.DeclaringType.GetElementType();
			}
			throw new InvalidOperationException();
		}

		// Token: 0x06000873 RID: 2163 RVA: 0x00021F10 File Offset: 0x00020110
		public static ImportGenericContext For(IGenericParameterProvider context)
		{
			if (context == null)
			{
				return default(ImportGenericContext);
			}
			return new ImportGenericContext(context);
		}

		// Token: 0x04000310 RID: 784
		private Collection<IGenericParameterProvider> stack;
	}
}
